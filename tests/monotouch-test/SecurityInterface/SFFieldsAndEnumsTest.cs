#if __MACOS__
#nullable enable

using System;
using NUnit.Framework;
using Foundation;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFAuthorizationPluginViewFieldsTest {

		[Test]
		public void UserNameKey ()
		{
			var key = SFAuthorizationPluginViewKeys.UserNameKey;
			Assert.That (key, Is.Not.Null, "UserNameKey should not be null");
			Assert.That ((string) key, Is.Not.Empty, "UserNameKey should not be empty");
		}

		[Test]
		public void UserShortNameKey ()
		{
			var key = SFAuthorizationPluginViewKeys.UserShortNameKey;
			Assert.That (key, Is.Not.Null, "UserShortNameKey should not be null");
			Assert.That ((string) key, Is.Not.Empty, "UserShortNameKey should not be empty");
		}

		[Test]
		public void DisplayViewException ()
		{
			var exc = SFAuthorizationPluginViewExceptions.DisplayViewException;
			Assert.That (exc, Is.Not.Null, "DisplayViewException should not be null");
			Assert.That ((string) exc, Is.Not.Empty, "DisplayViewException should not be empty");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFEnumsTest {

		[Test]
		public void AuthorizationViewState_Values ()
		{
			Assert.That (Enum.GetUnderlyingType (typeof (SFAuthorizationViewState)), Is.EqualTo (typeof (uint)), "Underlying type");
			Assert.That ((uint) SFAuthorizationViewState.Startup, Is.EqualTo (0u));
			Assert.That ((uint) SFAuthorizationViewState.Locked, Is.EqualTo (1u));
			Assert.That ((uint) SFAuthorizationViewState.InProgress, Is.EqualTo (2u));
			Assert.That ((uint) SFAuthorizationViewState.Unlocked, Is.EqualTo (3u));
		}

		[Test]
		public void ButtonType_Values ()
		{
			Assert.That (Enum.GetUnderlyingType (typeof (SFButtonType)), Is.EqualTo (typeof (uint)), "Underlying type");
			Assert.That ((uint) SFButtonType.Cancel, Is.EqualTo (0u));
			Assert.That ((uint) SFButtonType.Ok, Is.EqualTo (1u));
			Assert.That ((uint) SFButtonType.Back, Is.EqualTo (0u));
			Assert.That ((uint) SFButtonType.Login, Is.EqualTo (1u));
		}

		[Test]
		public void ViewType_Values ()
		{
			Assert.That (Enum.GetUnderlyingType (typeof (SFViewType)), Is.EqualTo (typeof (uint)), "Underlying type");
			Assert.That ((uint) SFViewType.IdentityAndCredentials, Is.EqualTo (0u));
			Assert.That ((uint) SFViewType.Credentials, Is.EqualTo (1u));
		}

		[Test]
		public void AuthorizationResult_Values ()
		{
			Assert.That ((uint) AuthorizationResult.Allow, Is.EqualTo (0u));
			Assert.That ((uint) AuthorizationResult.Deny, Is.EqualTo (1u));
			Assert.That ((uint) AuthorizationResult.Undefined, Is.EqualTo (2u));
			Assert.That ((uint) AuthorizationResult.UserCanceled, Is.EqualTo (3u));
		}

		[Test]
		public void AuthorizationContextFlags_Values ()
		{
			Assert.That ((uint) AuthorizationContextFlags.None, Is.EqualTo (0u));
			Assert.That ((uint) AuthorizationContextFlags.Extractable, Is.EqualTo (1u));
			Assert.That ((uint) AuthorizationContextFlags.Volatile, Is.EqualTo (2u));
			Assert.That ((uint) AuthorizationContextFlags.Sticky, Is.EqualTo (4u));
		}
	}
}
#endif // __MACOS__
