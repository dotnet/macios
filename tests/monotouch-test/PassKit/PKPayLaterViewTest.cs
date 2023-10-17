#if __IOS__ && !__MACCATALYST__

using System;
using Foundation;
using UIKit;
using PassKit;
using NUnit.Framework;

namespace MonoTouchFixtures.PassKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PKPayLaterViewTest {

		[Test]
		public void ValidateAmountTest ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			// From testing with Xcode 15.0.0, all values give the value of false to the completion handler.
			// Perhaps this will change in the future.
			// Apple feedback was submitted here: https://feedbackassistant.apple.com/feedback/13268898
			for (int i = 0; i < 1000; i++){
				PKPayLaterView.ValidateAmount (new NSDecimalNumber (i), "USD", (eligible) => {
					Assert.False (eligible);
				});
			}
		}
	}
}

#endif
