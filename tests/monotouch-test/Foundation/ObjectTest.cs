//
// Unit tests for NSObject
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012, 2015 Xamarin Inc. All rights reserved.
//

using System.Drawing;
using System.Reflection;
using System.Threading;

using CoreGraphics;
using Security;
#if MONOMAC
using AppKit;
using PlatformException = ObjCRuntime.ObjCException;
using UIView = AppKit.NSView;
#else
using UIKit;
using PlatformException = ObjCRuntime.ObjCException;
#endif
using Xamarin.Utils;

using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;
using PointF = CoreGraphics.CGPoint;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSObjectTest {

		bool GetIsDirectBinding (NSObject obj)
		{
			var flags = TestRuntime.GetFlags (obj);
			return (flags & 4) == 4;
		}

		class MyObject : NSObject {

			public bool GetIsDirectBinding ()
			{
				return this.IsDirectBinding;
			}
		}

		[Test]
		public void IsDirectBinding ()
		{
			using (var o1 = new NSObject ()) {
				Assert.That (GetIsDirectBinding (o1), Is.True, "inside monotouch.dll");
			}
			using (var o2 = new MyObject ()) {
				Assert.That (o2.GetIsDirectBinding (), Is.False, "outside monotouch.dll");
			}
		}

		[Test]
		public void SuperClass ()
		{
			Class c = new Class ("NSObject");
			Assert.That (c.Name, Is.EqualTo ("NSObject"), "Name");
			Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			Assert.That (c.SuperClass, Is.EqualTo (NativeHandle.Zero), "SuperClass");
		}

		[Test]
		public void FromObject_INativeObject ()
		{
			// https://bugzilla.xamarin.com/show_bug.cgi?id=8458
			using (CGPath p = CGPath.FromRect (new CGRect (1, 2, 3, 4))) {
				Assert.That (NSObject.FromObject (p), Is.Not.Null, "CGPath");
			}
			using (CGColor c = new CGColor (CGColorSpace.CreateDeviceRGB (), new nfloat [] { 0.1f, 0.2f, 0.3f, 1.0f })) {
				Assert.That (NSObject.FromObject (c), Is.Not.Null, "CGColor");
			}
			var hasSecAccessControl = TestRuntime.CheckXcodeVersion (6, 0);
			if (hasSecAccessControl) {
				using (var sac = new SecAccessControl (SecAccessible.WhenPasscodeSetThisDeviceOnly)) {
					Assert.That (NSObject.FromObject (sac), Is.Not.Null, "SecAccessControl");
				}
			}
		}

		[Test]
		public void FromObject_Handle ()
		{
			using (CGPath p = CGPath.FromRect (new CGRect (1, 2, 3, 4))) {
				Assert.That (NSObject.FromObject (p.Handle), Is.Not.Null, "CGPath");
			}
			using (CGColor c = new CGColor (CGColorSpace.CreateDeviceRGB (), new nfloat [] { 0.1f, 0.2f, 0.3f, 1.0f })) {
				Assert.That (NSObject.FromObject (c.Handle), Is.Not.Null, "CGColor");
			}
		}

		[Test]
		public void FromObject_NativeTypes ()
		{
			// to avoid issues like https://github.com/mono/xwt/commit/9b110e848030d5f6a0319212fd21bac02efad2c1
			using (var nativeint = (NSNumber) NSObject.FromObject ((nint) (-42))) {
				Assert.That (nativeint.Int32Value, Is.EqualTo (-42), "nint");
			}
			using (var nativeuint = (NSNumber) NSObject.FromObject ((nuint) 42)) {
				Assert.That (nativeuint.UInt32Value, Is.EqualTo (42), "nuint");
			}
			using (var nativefloat = (NSNumber) NSObject.FromObject ((nfloat) 3.14)) {
				Assert.That (nativefloat.FloatValue, Is.EqualTo (3.14f), "nfloat");
			}
		}

		[Test]
		public void ValueForInvalidKeyTest ()
		{
			// https://bugzilla.xamarin.com/show_bug.cgi?id=13243
			Assert.Throws<PlatformException> (() => {
				using (var str = new NSString ("test")) {
					str.ValueForKey (str);
				}
			});
		}

		[Test]
		public void Copy ()
		{
			IntPtr nscopying = Runtime.GetProtocol ("NSCopying");
			Assert.That (nscopying, Is.Not.EqualTo (IntPtr.Zero), "NSCopying");

			IntPtr nsmutablecopying = Runtime.GetProtocol ("NSMutableCopying");
			Assert.That (nsmutablecopying, Is.Not.EqualTo (IntPtr.Zero), "NSMutableCopying");

			// NSObject does not conform to NSCopying
			using (var o = new NSObject ()) {
				Assert.That (o.ConformsToProtocol (nscopying), Is.False, "NSObject/NSCopying");
				Assert.That (o.ConformsToProtocol (nsmutablecopying), Is.False, "NSObject/NSMutableCopying");
			}

			// NSNumber conforms to NSCopying - but not NSMutableCopying
			using (var n = new NSNumber (-1)) {
				Assert.That (n.ConformsToProtocol (nscopying), Is.True, "NSNumber/NSCopying");
				using (var xn = n.Copy ()) {
					Assert.That (xn, Is.Not.Null, "NSNumber/Copy/NotNull");
					Assert.That (xn, Is.SameAs (n), "NSNumber/Copy/NotSame");
				}
				Assert.That (n.ConformsToProtocol (nsmutablecopying), Is.False, "NSNumber/NSMutableCopying");
			}

			// NSMutableString conforms to NSCopying - but not NSMutableCopying
			using (var s = new NSMutableString (1)) {
				Assert.That (s.ConformsToProtocol (nscopying), Is.True, "NSMutableString/NSCopying");
				using (var xs = s.Copy ()) {
					Assert.That (xs, Is.Not.Null, "NSMutableString/Copy/NotNull");
					Assert.That (xs, Is.Not.SameAs (s), "NSMutableString/Copy/NotSame");
				}
				Assert.That (s.ConformsToProtocol (nsmutablecopying), Is.True, "NSMutableString/NSMutableCopying");
				using (var xs = s.MutableCopy ()) {
					Assert.That (xs, Is.Not.Null, "NSMutableString/MutableCopy/NotNull");
					Assert.That (xs, Is.Not.SameAs (s), "NSMutableString/MutableCopy/NotSame");
				}
			}
		}

		[Test]
		public void Encode ()
		{
			IntPtr nscoding = Runtime.GetProtocol ("NSCoding");
			Assert.That (nscoding, Is.Not.EqualTo (IntPtr.Zero), "NSCoding");

			// NSNumber conforms to NSCoding
			using (var n = new NSNumber (-1)) {
				Assert.That (n.ConformsToProtocol (nscoding), Is.True, "NSNumber/NSCoding");
				using (var d = new NSMutableData ())
				using (var a = new NSKeyedArchiver (d)) {
					n.EncodeTo (a);
					a.FinishEncoding ();
				}
			}
		}

		[Test]
		public void Equality ()
		{
			using (var o1 = new NSObject ())
			using (var o2 = new NSObject ()) {
				Assert.That (o1.Equals ((object) null), Is.False, "Equals(object) null");
				Assert.That (o1.Equals ((object) o2), Is.False, "Equals(object) 1-2");
				Assert.That (o2.Equals ((object) o1), Is.False, "Equals(object) 2-1");

				Assert.That (o1.Equals (3), Is.False, "Equals(object) 1-3");

				Assert.That (o1.Equals ((NSObject) null), Is.False, "Equals(NSObject) null");
				Assert.That (o1.Equals ((NSObject) o2), Is.False, "Equals(NSObject) 1-2");
				Assert.That (o2.Equals ((NSObject) o1), Is.False, "Equals(NSObject) 2-1");

				// on a more positive note...
				Assert.That (o1.Equals ((object) o1), Is.True, "Equals(object) 1-1");
				Assert.That (o2.Equals ((NSObject) o2), Is.True, "Equals(NSObject) 2-2");
			}
		}

		class NSOverrideEqualObject : NSObject {

			public NSOverrideEqualObject (bool throwEquals)
			{
				Throw = throwEquals;
			}

			bool Throw { get; set; }

			public bool Direct {
				get { return IsDirectBinding; }
			}

			public override bool Equals (object obj)
			{
				if (Throw)
					throw new NotFiniteNumberException ();
				return base.Equals (obj);
			}

			public override int GetHashCode ()
			{
				return 42;
			}
		}

		[Test]
		public void SubclassEquality ()
		{
			using (var o1 = new NSObject ())
			using (var o2 = new NSOverrideEqualObject (true))
			using (var o3 = new NSOverrideEqualObject (false)) {
				// true, same object
				Assert.That (o1.Equals (o1), Is.True, "direct - direct / same");
				Assert.That (o3.Equals (o3), Is.True, "indirect - indirect / same");

				// false, good since there's state in o2 and o3 that does not exists in o1 (direct / native only)
				Assert.That (o1.Equals (o2), Is.False, "direct - indirect");
				Assert.That (o3.Equals (o1), Is.False, "indirect - direct");

				// default is false, which is good since the managed state (Throw) differs between o2 and o3
				Assert.That (o3.Equals (o2), Is.False, "indirect - indirect");

				// throws (as implemented above)
				Assert.Throws<NotFiniteNumberException> (() => { o2.Equals ((object) o1); }, "Equals(object) 2-1");

				// throws (as IEquatable<NSObject>.Equals calls _overriden_ Equals
				Assert.Throws<NotFiniteNumberException> (() => { o2.Equals ((NSObject) o1); }, "Equals(NSObject) 2-1");
			}
		}

		[Test]
		public void ObserverTest ()
		{
			bool observed = false;
			using (var o = new UIView ()) {
				using (var observer = o.AddObserver ("frame", NSKeyValueObservingOptions.OldNew, change => {
					var old = ((NSValue) change.OldValue).CGRectValue;
					var @new = ((NSValue) change.NewValue).CGRectValue;
					Assert.That (old.ToString (), Is.EqualTo ("{{0, 0}, {0, 0}}"), "#old");
					Assert.That (@new.ToString (), Is.EqualTo ("{{0, 0}, {123, 234}}"), "#new");
					observed = true;
				})) {
					o.Frame = new CGRect (0, 0, 123, 234);
				}
			}
			Assert.That (observed, Is.True, "observed");
		}

		[Test]
		public void InvokeTest ()
		{
			var evt = new ManualResetEvent (false);
			using (var obj = new NSObject ())
				obj.Invoke (() => evt.Set (), .2);
			while (!evt.WaitOne (1))
				NSRunLoop.Current.RunUntil (NSRunLoopMode.Default, NSDate.Now.AddSeconds (1));

			Assert.That (evt.WaitOne (1), Is.True, "Our invoke was not fired?");
		}
	}
}
