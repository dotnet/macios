//
// Unit tests for MFMessageComposeViewController
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if HAS_MESSAGE

using System.Drawing;
using UIKit;

using MessageUI;

namespace MonoTouchFixtures.MessageUI {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MessageComposeViewControllerTest {

		[Test]
		public void MessageComposeDelegate ()
		{
			if (!MFMessageComposeViewController.CanSendText)
				Assert.Inconclusive ("Not configured to send text");

			using (var mail = new MFMessageComposeViewController ()) {
				Assert.That (mail.MessageComposeDelegate, Is.Null, "MessageComposeDelegate");
				mail.Finished += (sender, e) => { };
				Assert.That (mail.MessageComposeDelegate, Is.Not.Null, "MessageComposeDelegate");
			}
		}
	}
}

#endif // HAS_MESSAGE
