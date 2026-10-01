#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using NUnit.Framework;
using ObjCRuntime;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFCertificatePanelTest {

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern void InvokeDidEnd (IntPtr receiver, IntPtr selector, IntPtr sheet, nint returnCode, IntPtr contextInfo);

		sealed class TestCertificatePanel : SFCertificatePanel {

			public override void BeginSheet (NSWindow docWindow, NSObject? modalDelegate, Selector? didEndSelector, nint contextInfo, SecCertificate [] certificates, bool showGroup)
			{
				Assert.That (modalDelegate, Is.Not.Null, "ModalDelegate");
				Assert.That (didEndSelector, Is.Not.Null, "DidEndSelector");
				if (modalDelegate is null || didEndSelector is null)
					return;
				InvokeDidEnd (modalDelegate.Handle, didEndSelector.Handle, Handle, (nint) NSModalResponse.OK, contextInfo);
			}
		}

		[Test]
		public void BeginSheet_Callback ()
		{
			using var panel = new TestCertificatePanel ();
			using var window = new NSWindow ();
			var count = 0;
			var response = default (NSModalResponse);

			panel.BeginSheet (window, [], false, value => {
				count++;
				response = value;
			});

			Assert.That (count, Is.EqualTo (1), "Callback count");
			Assert.That (response, Is.EqualTo (NSModalResponse.OK), "Response");
		}
	}
}
#endif // __MACOS__
