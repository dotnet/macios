#if __MACOS__

using AppKit;
using CoreText;
using NUnit.Framework;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTextAlignmentConversionTest {

		static readonly NSTextAlignment [] alignments = new [] {
			NSTextAlignment.Left,
			NSTextAlignment.Right,
			NSTextAlignment.Center,
			NSTextAlignment.Justified,
			NSTextAlignment.Natural,
		};

		[Test]
		public void ToCTTextAlignment ()
		{
			Assert.AreEqual (CTTextAlignment.Left, AppKitFramework.NSTextAlignmentToCTTextAlignment (NSTextAlignment.Left), "Left");
			Assert.AreEqual (CTTextAlignment.Right, AppKitFramework.NSTextAlignmentToCTTextAlignment (NSTextAlignment.Right), "Right");
			Assert.AreEqual (CTTextAlignment.Center, AppKitFramework.NSTextAlignmentToCTTextAlignment (NSTextAlignment.Center), "Center");
			Assert.AreEqual (CTTextAlignment.Justified, AppKitFramework.NSTextAlignmentToCTTextAlignment (NSTextAlignment.Justified), "Justified");
			Assert.AreEqual (CTTextAlignment.Natural, AppKitFramework.NSTextAlignmentToCTTextAlignment (NSTextAlignment.Natural), "Natural");
		}

		[Test]
		public void FromCTTextAlignment ()
		{
			Assert.AreEqual (NSTextAlignment.Left, AppKitFramework.NSTextAlignmentFromCTTextAlignment (CTTextAlignment.Left), "Left");
			Assert.AreEqual (NSTextAlignment.Right, AppKitFramework.NSTextAlignmentFromCTTextAlignment (CTTextAlignment.Right), "Right");
			Assert.AreEqual (NSTextAlignment.Center, AppKitFramework.NSTextAlignmentFromCTTextAlignment (CTTextAlignment.Center), "Center");
			Assert.AreEqual (NSTextAlignment.Justified, AppKitFramework.NSTextAlignmentFromCTTextAlignment (CTTextAlignment.Justified), "Justified");
			Assert.AreEqual (NSTextAlignment.Natural, AppKitFramework.NSTextAlignmentFromCTTextAlignment (CTTextAlignment.Natural), "Natural");
		}

		[Test]
		public void RoundTrip ()
		{
			// Verifies the managed wrappers apply the NSTextAlignmentExtensions converter
			// (Center/Right are switched on arm64 natively) so the round-trip is stable.
			foreach (var alignment in alignments) {
				var ct = AppKitFramework.NSTextAlignmentToCTTextAlignment (alignment);
				Assert.AreEqual (alignment, AppKitFramework.NSTextAlignmentFromCTTextAlignment (ct), alignment.ToString ());
			}
		}
	}
}

#endif // __MACOS__
