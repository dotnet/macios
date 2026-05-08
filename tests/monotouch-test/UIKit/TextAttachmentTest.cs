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
				ClassicAssert.IsTrue (ta.Bounds.IsEmpty, "Bounds");
				ClassicAssert.Null (ta.Contents, "Contents");
				ClassicAssert.Null (ta.FileType, "FileType");
				ClassicAssert.Null (ta.FileWrapper, "FileWrapper");
				ClassicAssert.Null (ta.Image, "Image");
			}
		}
	}
}

#endif // !MONOMAC
