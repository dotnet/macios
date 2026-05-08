//
// NSTextAttachment Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !MONOMAC

using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TextAttachmentTest {

		[Test]
		public void CtorNull ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (var ta = new NSTextAttachment (null, null)) {
				Assert.That (ta.Bounds.IsEmpty, Is.True, "Bounds");
				Assert.That (ta.Contents, Is.Null, "Contents");
				Assert.That (ta.FileType, Is.Null, "FileType");
				Assert.That (ta.FileWrapper, Is.Null, "FileWrapper");
				Assert.That (ta.Image, Is.Null, "Image");
			}
		}
	}
}

#endif // !MONOMAC
