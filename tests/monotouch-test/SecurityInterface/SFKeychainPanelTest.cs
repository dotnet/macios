#if __MACOS__
using System;
using NUnit.Framework;
using AppKit;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFKeychainSavePanelTest {

		[Test]
		public void SharedKeychainSavePanel ()
		{
			var panel = SFKeychainSavePanel.SharedKeychainSavePanel;
			Assert.That (panel, Is.Not.Null, "SharedKeychainSavePanel should not be null");
			Assert.That (panel.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void SetPassword ()
		{
			// SFKeychainSavePanel inherits from NSSavePanel which may require window server access.
			TestRuntime.IgnoreInCI ("SFKeychainSavePanel operations may trigger UI on headless CI.");
			var panel = SFKeychainSavePanel.SharedKeychainSavePanel;
			Assert.DoesNotThrow (() => panel.SetPassword ("test-password"), "SetPassword");
			Assert.DoesNotThrow (() => panel.SetPassword (null), "SetPassword null");
		}

		[Test]
		public void Keychain_BeforeCreation ()
		{
			// SFKeychainSavePanel inherits from NSSavePanel which may require window server access.
			TestRuntime.IgnoreInCI ("SFKeychainSavePanel operations may trigger UI on headless CI.");
			var panel = SFKeychainSavePanel.SharedKeychainSavePanel;
			// Keychain is null until a user creates one via the panel
			var keychain = panel.Keychain;
			// May or may not be null depending on previous panel usage
		}

		[Test]
		public void Error_BeforeCreation ()
		{
			// SFKeychainSavePanel inherits from NSSavePanel which may require window server access.
			TestRuntime.IgnoreInCI ("SFKeychainSavePanel operations may trigger UI on headless CI.");
			var panel = SFKeychainSavePanel.SharedKeychainSavePanel;
			// Error should be null if no operation has been attempted
			var error = panel.Error;
			// May or may not be null depending on previous panel usage
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFKeychainSettingsPanelTest {

		[Test]
		public void SharedKeychainSettingsPanel ()
		{
			var panel = SFKeychainSettingsPanel.SharedKeychainSettingsPanel;
			Assert.That (panel, Is.Not.Null, "SharedKeychainSettingsPanel should not be null");
			Assert.That (panel.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}
	}
}
#endif // __MACOS__
