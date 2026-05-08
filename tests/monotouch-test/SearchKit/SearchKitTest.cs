#if __MACOS__
using System.IO;

using SearchKit;

namespace apitest {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SearchKitTests {
		string path = $"/tmp/mmptest-my-{System.Diagnostics.Process.GetCurrentProcess ().Id}.index";

		[SetUp]
		public void Setup ()
		{
			SKIndex.LoadDefaultExtractorPlugIns ();
			if (File.Exists (path))
				File.Delete (path);

		}

		[TearDown]
		public void TearDown ()
		{
			if (File.Exists (path))
				File.Delete (path);
		}

		[Test]
		public void TestCreate ()
		{
			var idx = SKIndex.CreateWithUrl (new NSUrl ("file://" + path), "myIndex", SKIndexType.InvertedVector, null);
			if (idx is null)
				throw new Exception ();


			var d1 = new SKDocument (new NSUrl ("file:///etc/passwd"));
			var d2 = new SKDocument (new NSUrl ("file:///etc/fstab"));
			idx.AddDocument (d1, "text/plain", true);

			idx.AddDocumentWithText (d2, "This file contains some text like an Apple and an Orange", true);

			const int max = 10;
			nint [] ids = new nint [max];
			float [] scores = new float [max];
			nint nfound;
			bool more;

			using (var search = idx.Search ("some", SKSearchOptions.SpaceMeansOr)) {
				more = search.FindMatches (max, ref ids, ref scores, 1, out nfound);
				ClassicAssert.IsFalse (more);

				for (nint i = 0; i < nfound; i++) {
					var doc = idx.GetDocument (ids [i]);
					ClassicAssert.IsNotNull (doc, "TestCreate - GetDocument returned null");
				}
			}

			using (var search = idx.Search ("some", SKSearchOptions.SpaceMeansOr)) {
				more = search.FindMatches (max, ref ids, 1, out nfound);
				for (nint i = 0; i < nfound; i++) {
					var doc = idx.GetDocument (ids [i]);
					Console.WriteLine ("Got {0}", doc);
				}
			}

			idx.Compact ();
			idx.Flush ();
			idx.Close ();

			// Now open
			idx = SKIndex.FromUrl (new NSUrl ("file://" + path), "myIndex", true);
			ClassicAssert.NotNull (idx);

		}

		[Test]
		public void TestInMemory ()
		{
			var m = new NSMutableData ();
			var idx = SKIndex.CreateWithMutableData (m, "indexName", SKIndexType.Inverted, null);
			ClassicAssert.NotNull (idx);
			idx.AddDocumentWithText (new SKDocument (new NSUrl ("file:///etc/passwd")), "These are the contents of the passwd file, well, not really", true);
			idx.Flush ();
			idx.Compact ();
			idx.Close ();

			idx = SKIndex.FromMutableData (m, "indexName");
			ClassicAssert.NotNull (idx);
			idx.Close ();
		}

		[Test]
		public void TestTextAnalysis ()
		{
			var m = new NSMutableData ();
			var properties = new SKTextAnalysis () {
				StartTermChars = "",
				EndTermChars = "",
				TermChars = "\"-_@.'",
				MinTermLength = 3,
				StopWords = new NSSet ("all", "and", "its", "it's", "the")
			};

			var idx = SKIndex.CreateWithMutableData (m, "indexName", SKIndexType.Inverted, properties);
			ClassicAssert.NotNull (idx);

		}

		[Test]
		public void TestSummary ()
		{
			var sum = SKSummary.Create (
				"Once upon a time, there was a dog that loved to take long walks in the park and enjoyed jumping all around (maybe more so on hot days).\n\n" +
				"One day he ran into a solid rock in the park and was puzzled by it.\n\n" +
				"If I cook this rock enough, it will be soft and tasty.   I might even get lucky and find some salt.");

			ClassicAssert.NotNull (sum);
			var rankOrder = new nint [10];
			var sentenceIndex = new nint [10];
			var paragraphIndex = new nint [10];

			nint n;
			n = sum.GetSentenceSummaryInfo (10, rankOrder, sentenceIndex, paragraphIndex);
			ClassicAssert.AreEqual ((nint) 4, n);
			ClassicAssert.AreEqual ((nint) 2, paragraphIndex [3]); // 4th sentence (index 3) is on the 3rd (index 2) paragraph
			n = sum.GetSentenceSummaryInfo (10, null, sentenceIndex, paragraphIndex);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, rankOrder, null, paragraphIndex);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, rankOrder, sentenceIndex, null);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, null, null, paragraphIndex);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, null, sentenceIndex, null);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, rankOrder, null, null);
			ClassicAssert.AreEqual ((nint) 4, n);
			n = sum.GetSentenceSummaryInfo (10, null, null, null);
			ClassicAssert.AreEqual ((nint) 4, n);

			n = sum.GetParagraphSummaryInfo (10, rankOrder, paragraphIndex);
			n = sum.GetParagraphSummaryInfo (10, null, paragraphIndex);
			n = sum.GetParagraphSummaryInfo (10, rankOrder, null);
			n = sum.GetParagraphSummaryInfo (10, null, null);
			var sentence = sum.GetSentence (3);
			ClassicAssert.AreEqual ("I might even get lucky and find some salt.", sentence);
			var par = sum.GetParagraph (1);
			ClassicAssert.AreEqual ("One day he ran into a solid rock in the park and was puzzled by it.\n", par);
			var ssum = sum.GetSentenceSummary (1);
			ClassicAssert.NotNull (ssum);
			var psum = sum.GetParagraphSummary (1);
			ClassicAssert.NotNull (psum);
		}
	}
}
#endif // __MACOS__
