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
	public class SFCertificatePanelTest {

		[Test]
		public void SharedCertificatePanel ()
		{
			var panel = SFCertificatePanel.SharedCertificatePanel;
			Assert.That (panel, Is.Not.Null, "SharedCertificatePanel should not be null");
			Assert.That (panel.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void CertificateView ()
		{
			var panel = SFCertificatePanel.SharedCertificatePanel;
			// CertificateView may be null until the panel has been presented
			var view = panel.CertificateView;
		}

		[Test]
		public void Properties ()
		{
			var panel = SFCertificatePanel.SharedCertificatePanel;

			Assert.DoesNotThrow (() => panel.SetShowsHelp (false), "SetShowsHelp");
			Assert.That (panel.ShowsHelp, Is.False, "ShowsHelp");

			Assert.DoesNotThrow (() => panel.SetDefaultButtonTitle ("OK"), "SetDefaultButtonTitle");
			Assert.DoesNotThrow (() => panel.SetAlternateButtonTitle ("Cancel"), "SetAlternateButtonTitle");

			panel.SetHelpAnchor ("testAnchor");
			Assert.That (panel.HelpAnchor, Is.EqualTo ("testAnchor"), "HelpAnchor round-trip");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFCertificateTrustPanelTest {

		[Test]
		public void SharedCertificateTrustPanel ()
		{
			var panel = SFCertificateTrustPanel.SharedCertificateTrustPanel;
			Assert.That (panel, Is.Not.Null, "SharedCertificateTrustPanel should not be null");
			Assert.That (panel.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void InformativeText ()
		{
			var panel = SFCertificateTrustPanel.SharedCertificateTrustPanel;
			panel.SetInformativeText ("Test informative text");
			Assert.That (panel.InformativeText, Is.EqualTo ("Test informative text"), "InformativeText round-trip");
		}
	}
}
#endif // __MACOS__
