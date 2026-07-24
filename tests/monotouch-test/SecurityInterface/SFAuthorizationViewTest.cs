#if __MACOS__
#nullable enable

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
		public void AuthorizationRights_InitiallyNull ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.That (view.AuthorizationRights, Is.Null, "AuthorizationRights");
		}

		[Test]
		public void AuthorizationRights_SetAndGet ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			var rights = new AuthorizationRights (new AuthorizationRight ("com.example.test", new byte [] { 1, 2, 3 }));
			view.AuthorizationRights = rights;
			rights.Dispose ();

			using var copiedRights = view.AuthorizationRights;
			Assert.That (copiedRights, Is.Not.Null, "AuthorizationRights");
			if (copiedRights is null)
				return;
			Assert.That (copiedRights.Count, Is.EqualTo (1), "Count");
			Assert.That (copiedRights [0].Name, Is.EqualTo ("com.example.test"), "Name");
			Assert.That (copiedRights [0].Value, Is.EqualTo (new byte [] { 1, 2, 3 }), "Value");
		}

		[Test]
		public void AuthorizationRights_NullThrows ()
		{
			using var view = new SFAuthorizationView (new global::CoreGraphics.CGRect (0, 0, 100, 100));
			Assert.Throws<ArgumentNullException> (() => view.AuthorizationRights = null);
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
			Assert.DoesNotThrow (() => view.SetFlags (AuthorizationFlags.Defaults), "SetFlags");
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
