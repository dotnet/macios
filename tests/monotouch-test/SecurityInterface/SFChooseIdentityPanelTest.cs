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
	public class SFChooseIdentityPanelTest {

		[Test]
		public void SharedChooseIdentityPanel ()
		{
			var panel = SFChooseIdentityPanel.SharedChooseIdentityPanel;
			Assert.That (panel, Is.Not.Null, "SharedChooseIdentityPanel should not be null");
			Assert.That (panel.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void Properties ()
		{
			// Panel property setters may trigger deferred UI operations on headless CI.
			TestRuntime.IgnoreInCI ("SFChooseIdentityPanel property setters may trigger UI operations on headless CI.");
			var panel = SFChooseIdentityPanel.SharedChooseIdentityPanel;

			Assert.DoesNotThrow (() => panel.SetShowsHelp (true), "SetShowsHelp");
			Assert.That (panel.ShowsHelp, Is.True, "ShowsHelp");

			panel.SetDefaultButtonTitle ("Select");
			panel.SetAlternateButtonTitle ("Cancel");

			panel.SetHelpAnchor ("identityHelp");
			Assert.That (panel.HelpAnchor, Is.EqualTo ("identityHelp"), "HelpAnchor round-trip");

			panel.SetInformativeText ("Choose an identity");
			Assert.That (panel.InformativeText, Is.EqualTo ("Choose an identity"), "InformativeText round-trip");

			panel.SetDomain ("com.example.test");
			Assert.That (panel.Domain, Is.EqualTo ("com.example.test"), "Domain round-trip");
		}

		[Test]
		public void Identity_InitiallyNull ()
		{
			var panel = SFChooseIdentityPanel.SharedChooseIdentityPanel;
			// Identity is null until a user selects one from the panel
			var identity = panel.Identity;
			Assert.That (identity, Is.Null, "Identity should be null when no selection made");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFChooseIdentityTableCellViewTest {

		[Test]
		public void Constructor ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			Assert.That (cellView.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void IssuerTextField_DefaultNull ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			Assert.That (cellView.IssuerTextField, Is.Null, "IssuerTextField should initially be null");
		}

		[Test]
		public void IssuerTextField_SetAndGet ()
		{
			using var cellView = new SFChooseIdentityTableCellView (new global::CoreGraphics.CGRect (0, 0, 200, 44));
			using var textField = new NSTextField ();
			cellView.IssuerTextField = textField;
			Assert.That (cellView.IssuerTextField, Is.Not.Null, "IssuerTextField should be set");
		}
	}
}
#endif // __MACOS__
