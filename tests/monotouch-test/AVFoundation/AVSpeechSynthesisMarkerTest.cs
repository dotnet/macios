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
					ClassicAssert.AreEqual (range, marker.TextRange, "TextRange W");
					ClassicAssert.AreEqual (byteOffset, (nint) marker.ByteSampleOffset, "ByteSampleOffset W");
					ClassicAssert.AreEqual (AVSpeechSynthesisMarkerMark.Word, marker.Mark, "AVSpeechSynthesisMarkerMark W");
					ClassicAssert.IsNull (marker.BookmarkName, "BookmarkName W");
					ClassicAssert.IsNull (marker.Phoneme, "Phoneme W");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (range, byteOffset, AVSpeechSynthesisMarkerRangeOption.Sentence);
					ClassicAssert.AreEqual (range, marker.TextRange, "TextRange S");
					ClassicAssert.AreEqual (byteOffset, (nint) marker.ByteSampleOffset, "ByteSampleOffset S");
					ClassicAssert.AreEqual (AVSpeechSynthesisMarkerMark.Sentence, marker.Mark, "AVSpeechSynthesisMarkerMark S");
					ClassicAssert.IsNull (marker.BookmarkName, "BookmarkName S");
					ClassicAssert.IsNull (marker.Phoneme, "Phoneme S");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (range, byteOffset, AVSpeechSynthesisMarkerRangeOption.Paragraph);
					ClassicAssert.AreEqual (range, marker.TextRange, "TextRange P");
					ClassicAssert.AreEqual (byteOffset, (nint) marker.ByteSampleOffset, "ByteSampleOffset P");
					ClassicAssert.AreEqual (AVSpeechSynthesisMarkerMark.Paragraph, marker.Mark, "AVSpeechSynthesisMarkerMark P");
					ClassicAssert.IsNull (marker.BookmarkName, "BookmarkName P");
					ClassicAssert.IsNull (marker.Phoneme, "Phoneme P");
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
					ClassicAssert.AreEqual (range, marker.TextRange, "TextRange P");
					ClassicAssert.AreEqual (byteOffset, (nint) marker.ByteSampleOffset, "ByteSampleOffset P");
					ClassicAssert.AreEqual (AVSpeechSynthesisMarkerMark.Phoneme, marker.Mark, "AVSpeechSynthesisMarkerMark P");
					ClassicAssert.IsNull (marker.BookmarkName, "BookmarkName P");
					ClassicAssert.AreEqual (value, marker.Phoneme, "Phoneme P");
				}
				{
					using var marker = new AVSpeechSynthesisMarker (value, byteOffset, AVSpeechSynthesisMarkerStringOption.Bookmark);
					ClassicAssert.AreEqual (range, marker.TextRange, "TextRange B");
					ClassicAssert.AreEqual (byteOffset, (nint) marker.ByteSampleOffset, "ByteSampleOffset B");
					ClassicAssert.AreEqual (AVSpeechSynthesisMarkerMark.Bookmark, marker.Mark, "AVSpeechSynthesisMarkerMark B");
					ClassicAssert.IsNull (marker.Phoneme, "Phoneme B");
				}
			});
		}
	}
}
