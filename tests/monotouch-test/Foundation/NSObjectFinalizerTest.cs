// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;

using CoreFoundation;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSObjectFinalizerTest {
		class StringSubclass : NSString {
			public StringSubclass () : base (new string ('x', 100))
			{
			}
		}

		class MutableStringSubclass : NSMutableString {
			public MutableStringSubclass (NativeHandle handle) : base (handle)
			{
			}
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static (WeakReference Reference, NativeHandle Handle, nint RetainCount) CreateString (bool mutable, bool subclass)
		{
			NSString value;
			if (mutable) {
				if (subclass) {
					using var nativeString = new CFMutableString ();
					value = new MutableStringSubclass (nativeString.Handle);
				} else {
					value = new NSMutableString (0);
				}
			} else {
				value = subclass ? new StringSubclass () : new NSString (new string ('x', 100));
			}

			// Retaining a native user type can keep its managed wrapper alive.
			if (!subclass)
				value.DangerousRetain ();
			return (new WeakReference (value, true), value.Handle, TestRuntime.CFGetRetainCount (value.Handle));
		}

		[TestCase (false, false)]
		[TestCase (true, false)]
		[TestCase (false, true)]
		[TestCase (true, true)]
		public void FinalizerRelease (bool mutable, bool subclass)
		{
			Assert.That (NSThread.IsMain, Is.True, "The main thread must not drain the disposer while collecting.");
			var (reference, handle, retainCount) = CreateString (mutable, subclass);
			try {
				GC.Collect ();
				GC.WaitForPendingFinalizers ();

				var target = (NSObject) reference.Target;
				if (subclass) {
					Assert.That (target, Is.Not.Null, "Subclasses must remain queued for main-thread release.");
					Assert.That (target.Handle, Is.EqualTo (handle), "Queued handle");
					Assert.That (TestRuntime.CFGetRetainCount (handle), Is.EqualTo (retainCount), "Queued retain count");
				} else {
					Assert.That (target?.Handle ?? NativeHandle.Zero, Is.EqualTo (NativeHandle.Zero), "Finalized handle");
					Assert.That (TestRuntime.CFGetRetainCount (handle), Is.EqualTo (retainCount - 1), "Finalized retain count");
				}
			} finally {
				if (!subclass)
					NSObject.DangerousRelease (handle);
			}
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ExplicitDispose (bool mutable)
		{
			using NSString value = mutable ? new NSMutableString (0) : new NSString (new string ('x', 100));
			value.Dispose ();
			value.Dispose ();
			Assert.That (value.Handle, Is.EqualTo (NativeHandle.Zero), "Disposed handle");
		}
	}
}
