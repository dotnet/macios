//
// Unit tests for AVSpeechSynthesisMarker

using AVFoundation;

#nullable enable

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVSpeechSynthesisMarkerTest {
		[Test]
		public void NSRangeCtor ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var range = new NSRange (1, 2);
			nint byteOffset = 314;
			Assert.Multiple (() => {
				{
					using var marker = new AVSpeechSynthesisMarker (range, byteOffset, AVSpeechSynthesisMarkerRangeOption.Word);
					Assert.That (marker.TextRange, Is.EqualTo (range), "TextRange W");
					Assert.That ((nint) marker.ByteSampleOffset, Is.EqualTo (byteOffset), "ByteSampleOffset W");
					Assert.That (marker.Mark, Is.EqualTo (AVSpeechSynthesisMarkerMark.Word), "AVSpeechSynthesisMarkerMark W");
					Assert.That (marker.BookmarkName, Is.Null, "BookmarkName W");
					Assert.That (marker.Phoneme, Is.Null, "Phoneme W");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (range, byteOffset, AVSpeechSynthesisMarkerRangeOption.Sentence);
					Assert.That (marker.TextRange, Is.EqualTo (range), "TextRange S");
					Assert.That ((nint) marker.ByteSampleOffset, Is.EqualTo (byteOffset), "ByteSampleOffset S");
					Assert.That (marker.Mark, Is.EqualTo (AVSpeechSynthesisMarkerMark.Sentence), "AVSpeechSynthesisMarkerMark S");
					Assert.That (marker.BookmarkName, Is.Null, "BookmarkName S");
					Assert.That (marker.Phoneme, Is.Null, "Phoneme S");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (range, byteOffset, AVSpeechSynthesisMarkerRangeOption.Paragraph);
					Assert.That (marker.TextRange, Is.EqualTo (range), "TextRange P");
					Assert.That ((nint) marker.ByteSampleOffset, Is.EqualTo (byteOffset), "ByteSampleOffset P");
					Assert.That (marker.Mark, Is.EqualTo (AVSpeechSynthesisMarkerMark.Paragraph), "AVSpeechSynthesisMarkerMark P");
					Assert.That (marker.BookmarkName, Is.Null, "BookmarkName P");
					Assert.That (marker.Phoneme, Is.Null, "Phoneme P");
				}
			});
		}

		[Test]
		public void StringCtor ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var range = new NSRange (0, 0);
			var value = "hello world";
			nint byteOffset = 314;
			Assert.Multiple (() => {
				{
					using var marker = new AVSpeechSynthesisMarker (value, byteOffset, AVSpeechSynthesisMarkerStringOption.Phoneme);
					Assert.That (marker.TextRange, Is.EqualTo (range), "TextRange P");
					Assert.That ((nint) marker.ByteSampleOffset, Is.EqualTo (byteOffset), "ByteSampleOffset P");
					Assert.That (marker.Mark, Is.EqualTo (AVSpeechSynthesisMarkerMark.Phoneme), "AVSpeechSynthesisMarkerMark P");
					Assert.That (marker.BookmarkName, Is.Null, "BookmarkName P");
					Assert.That (marker.Phoneme, Is.EqualTo (value), "Phoneme P");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (value, byteOffset, AVSpeechSynthesisMarkerStringOption.Bookmark);
					Assert.That (marker.TextRange, Is.EqualTo (range), "TextRange B");
					Assert.That ((nint) marker.ByteSampleOffset, Is.EqualTo (byteOffset), "ByteSampleOffset B");
					Assert.That (marker.Mark, Is.EqualTo (AVSpeechSynthesisMarkerMark.Bookmark), "AVSpeechSynthesisMarkerMark B");
					Assert.That (marker.Phoneme, Is.Null, "Phoneme B");
				}
			});
		}
	}
}
