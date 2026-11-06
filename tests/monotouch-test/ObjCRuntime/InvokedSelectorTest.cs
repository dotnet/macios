// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Threading;

using CoreGraphics;
using Foundation;
using NUnit.Framework;
using ObjCRuntime;

using Bindings.Test;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class InvokedSelectorTest {
		[Preserve (AllMembers = true)]
		class Target : NSObject {
			public Action Callback;
			public Selector Observed;

			[Export ("selectorTestInvoke")]
			public void Invoke ()
			{
				Observed = Runtime.InvokedSelector;
				Callback?.Invoke ();
			}

			[Export ("selectorTestInner")]
			public void Inner ()
			{
				Observed = Runtime.InvokedSelector;
			}

			[Export ("selectorTestLong")]
			public long LongValue ()
			{
				Observed = Runtime.InvokedSelector;
				return 1L << 40;
			}

			[Export ("selectorTestFloat")]
			public float FloatValue ()
			{
				Observed = Runtime.InvokedSelector;
				return 1.25f;
			}

			[Export ("selectorTestRect")]
			public CGRect RectValue ()
			{
				Observed = Runtime.InvokedSelector;
				return new CGRect (1, 2, 3, 4);
			}

			[Export ("selectorTestStatic")]
			public static void StaticInvoke ()
			{
				Assert.That (Runtime.InvokedSelector?.Name, Is.EqualTo ("selectorTestStatic"), "Static selector");
			}
		}

		[Preserve (AllMembers = true)]
		class ThrowingTarget : ObjCExceptionTest {
			public Selector Observed;

			public override void ThrowManagedException ()
			{
				Observed = Runtime.InvokedSelector;
				throw new InvalidOperationException ("Selector test");
			}
		}

		[Preserve (AllMembers = true)]
		class ConstructorTarget : NSObject {
			public Selector Observed;

			[Export ("init")]
			public ConstructorTarget ()
			{
				Observed = Runtime.InvokedSelector;
			}
		}

		[Test]
		public void Constructor ()
		{
			var previous = Runtime.InvokedSelector;
			var handle = Messaging.IntPtr_objc_msgSend (Class.GetHandle (typeof (ConstructorTarget)), Selector.GetHandle ("alloc"));
			handle = Messaging.IntPtr_objc_msgSend (handle, Selector.GetHandle ("init"));
			using var target = Runtime.GetNSObject<ConstructorTarget> (handle, true);
			Assert.That (target.Observed?.Name, Is.EqualTo ("init"), "Constructor selector");
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Constructor restored");
		}

		[Test]
		public void InvocationAndManagedCalls ()
		{
			var previous = Runtime.InvokedSelector;
			using var target = new Target ();
			target.Invoke ();
			Assert.That (target.Observed, Is.EqualTo (previous), "Direct managed call");
			Messaging.void_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestInvoke"));
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestInvoke"), "Native call");
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Restored");
			Messaging.void_objc_msgSend (Class.GetHandle (typeof (Target)), Selector.GetHandle ("selectorTestStatic"));
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Static restored");
		}

		[Test]
		public void NestedInvocation ()
		{
			var previous = Runtime.InvokedSelector;
			using var outer = new Target ();
			using var inner = new Target ();
			outer.Callback = () => {
				var selector = Runtime.InvokedSelector;
				Messaging.void_objc_msgSend (inner.Handle, Selector.GetHandle ("selectorTestInner"));
				Assert.That (inner.Observed?.Name, Is.EqualTo ("selectorTestInner"), "Nested selector");
				Assert.That (Runtime.InvokedSelector, Is.EqualTo (selector), "Nested restoration");
			};
			Messaging.void_objc_msgSend (outer.Handle, Selector.GetHandle ("selectorTestInvoke"));
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Outer restoration");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ReturnValues (bool aliases)
		{
			if (aliases && !Runtime.DynamicRegistrationSupported)
				Assert.Ignore ("This test requires dynamic registration.");
			var suffix = aliases ? "Alias" : "";
			if (aliases && !returnAliasesConnected) {
				Runtime.ConnectMethod (typeof (Target).GetMethod (nameof (Target.LongValue), []), new Selector ("selectorTestLongAlias"));
				Runtime.ConnectMethod (typeof (Target).GetMethod (nameof (Target.FloatValue), []), new Selector ("selectorTestFloatAlias"));
				Runtime.ConnectMethod (typeof (Target).GetMethod (nameof (Target.RectValue), []), new Selector ("selectorTestRectAlias"));
				returnAliasesConnected = true;
			}
			var previous = Runtime.InvokedSelector;
			using var target = new Target ();
			Assert.That (Messaging.long_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestLong" + suffix)), Is.EqualTo (1L << 40), "Long return value");
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestLong" + suffix), "Long selector");
			Assert.That (Messaging.float_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestFloat" + suffix)), Is.EqualTo (1.25f), "Float return value");
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestFloat" + suffix), "Float selector");
			var selector = Selector.GetHandle ("selectorTestRect" + suffix);
			CGRect rect;
			if (Runtime.IsARM64CallingConvention) {
				rect = Messaging.CGRect_objc_msgSend (target.Handle, selector);
			} else {
				Messaging.CGRect_objc_msgSend_stret (out rect, target.Handle, selector);
			}
			Assert.That (rect, Is.EqualTo (new CGRect (1, 2, 3, 4)), "Struct return value");
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestRect" + suffix), "Struct selector");
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Restored");
		}

		[Test]
		public void ExceptionRestoration ()
		{
			if (global::System.Diagnostics.Debugger.IsAttached)
				Assert.Ignore ("The debugger changes managed exception marshalling.");
			var previous = Runtime.InvokedSelector;
			using var outer = new Target ();
			using var inner = new ThrowingTarget ();
			MarshalManagedExceptionHandler handler = (sender, args) => args.ExceptionMode = MarshalManagedExceptionMode.ThrowObjectiveCException;
			Runtime.MarshalManagedException += handler;
			try {
				outer.Callback = () => {
					inner.InvokeManagedExceptionThrowerAndCatch ();
					Assert.That (inner.Observed?.Name, Is.EqualTo ("throwManagedException"), "Throwing selector");
					Assert.That (Runtime.InvokedSelector?.Name, Is.EqualTo ("selectorTestInvoke"), "Exception restored outer selector");
				};
				Messaging.void_objc_msgSend (outer.Handle, Selector.GetHandle ("selectorTestInvoke"));
				Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Exception restored original selector");
			} finally {
				Runtime.MarshalManagedException -= handler;
			}
		}

		[Test]
		public void ThreadIsolation ()
		{
			using var target = new Target ();
			target.Callback = () => {
				Exception failure = null;
				var thread = new Thread (() => {
					try {
						Assert.That (Runtime.InvokedSelector, Is.Null, "Other thread");
						using var other = new Target ();
						Messaging.void_objc_msgSend (other.Handle, Selector.GetHandle ("selectorTestInvoke"));
						Assert.That (other.Observed?.Name, Is.EqualTo ("selectorTestInvoke"), "Other invocation");
						Assert.That (Runtime.InvokedSelector, Is.Null, "Other thread restored");
					} catch (Exception ex) {
						failure = ex;
					}
				});
				thread.Start ();
				Assert.That (thread.Join (TimeSpan.FromSeconds (30)), Is.True, "Thread completed");
				Assert.That (failure, Is.Null, "Other thread failure");
				Assert.That (Runtime.InvokedSelector?.Name, Is.EqualTo ("selectorTestInvoke"), "Original thread");
			};
			Messaging.void_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestInvoke"));
		}

		static bool aliasesConnected;
		static bool returnAliasesConnected;

		[Test]
		public void ConnectMultipleSelectors ()
		{
			if (!Runtime.DynamicRegistrationSupported)
				Assert.Ignore ("This test requires dynamic registration.");
			if (!aliasesConnected) {
				var method = typeof (Target).GetMethod (nameof (Target.Invoke), []);
				Runtime.ConnectMethod (method, new Selector ("selectorTestAliasA"));
				Runtime.ConnectMethod (method, new Selector ("selectorTestAliasB"));
				aliasesConnected = true;
			}
			var previous = Runtime.InvokedSelector;
			using var target = new Target ();
			Messaging.void_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestAliasA"));
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestAliasA"), "First alias");
			Messaging.void_objc_msgSend (target.Handle, Selector.GetHandle ("selectorTestAliasB"));
			Assert.That (target.Observed?.Name, Is.EqualTo ("selectorTestAliasB"), "Second alias");
			Assert.That (Runtime.InvokedSelector, Is.EqualTo (previous), "Restored");
		}
	}
}
