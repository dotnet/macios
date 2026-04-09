#if __MACOS__
using System;
using NUnit.Framework;
using AppKit;
using Foundation;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFAuthorizationViewTest {

		[Test]
		public void Constructor ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.That (view.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void AuthorizationState_InitialValue ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			var state = view.AuthorizationState;
			Assert.That (state, Is.EqualTo (SFAuthorizationViewState.Startup), "Initial state should be Startup");
		}

		[Test]
		public void IsEnabled_Default ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			// The default enabled state depends on the system, just verify it doesn't crash
			var _ = view.IsEnabled;
		}

		[Test]
		public void SetEnabled ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.DoesNotThrow (() => view.SetEnabled (false), "SetEnabled false");
			Assert.DoesNotThrow (() => view.SetEnabled (true), "SetEnabled true");
		}

		[Test]
		public void SetAuthorizationString ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.DoesNotThrow (() => view.SetAuthorizationString ("com.example.test"), "SetAuthorizationString");
		}

		[Test]
		public void SetAuthorizationString_NullThrows ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.Throws<ArgumentNullException> (() => view.SetAuthorizationString (null!));
		}

		[Test]
		public void AuthorizationRightsSet_SetAndGet ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			using var rights = new AuthorizationRights ("com.example.right1");
			view.AuthorizationRightsSet = rights;

			var retrieved = view.AuthorizationRightsSet;
			Assert.That (retrieved, Is.Not.Null, "Should get rights back");
			Assert.That (retrieved!.Count, Is.EqualTo (1), "Count");
			Assert.That (retrieved [0].Name, Is.EqualTo ("com.example.right1"), "Name");
			retrieved.Dispose ();
		}

		[Test]
		public void AuthorizationRightsSet_SetNull ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.DoesNotThrow (() => view.AuthorizationRightsSet = null, "Setting null should not throw");
		}

		[Test]
		public void Delegate_SetAndGet ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.That (view.WeakDelegate, Is.Null, "Delegate should initially be null");
			view.WeakDelegate = NSObject.FromObject ("test");
			Assert.That (view.WeakDelegate, Is.Not.Null, "Delegate should be set");
		}

		[Test]
		public void SetFlags ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.DoesNotThrow (() => view.SetFlags (0), "SetFlags 0");
		}

		[Test]
		public void SetAutoupdate ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.DoesNotThrow (() => view.SetAutoupdate (false), "SetAutoupdate false");
			Assert.DoesNotThrow (() => view.SetAutoupdate (true, 60.0), "SetAutoupdate with interval");
		}
	}
}
#endif // __MACOS__
