//
// Unit tests for CTFrame
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using CoreText;
using CoreGraphics;

namespace MonoTouchFixtures.CoreText {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CTFrameTests {

		[Test] // #4677
		public void GetPathTest ()
		{
			using (var framesetter = new CTFramesetter (new NSAttributedString ("Testing, testing, 1, 2, 3..."))) {
				using (var frame = framesetter.GetFrame (new NSRange (0, 0), new CGPath (), null)) {
					using (var f = frame.GetPath ()) {
					}
					using (var f = frame.GetPath ()) {
						Console.WriteLine (f.BoundingBox);
					}
				}
			}
		}

		[Test]
		public void CTTypesetterCreateTest ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			using (var framesetter = new CTFramesetter (new NSAttributedString ("Testing, testing, 1, 2, 3...")))
			using (var type = framesetter.GetTypesetter ())
			using (var newFrame = CTFramesetter.Create (type)) {
				Assert.That (type, Is.Not.Null, "Create");
				var type2 = newFrame.GetTypesetter ();
				Assert.That (type, Is.Not.Null, "type2");
				Assert.That (type2.Handle, Is.EqualTo (type.Handle), "Same typesetter");
			}
		}
	}
}
