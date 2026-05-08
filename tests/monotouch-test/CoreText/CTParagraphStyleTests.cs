//
// Unit tests for CTParagraphStyle
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System.Linq;

using CoreText;

namespace MonoTouchFixtures.CoreText {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CTParagraphStyleTests {

		[Test]
		public void StylePropertiesTest ()
		{
			var settings = new CTParagraphStyleSettings () {
				TailIndent = 5,
				ParagraphSpacingBefore = 5,
				ParagraphSpacing = 5,
				LineSpacing = 5,
				MinimumLineHeight = 5,
				MaximumLineHeight = 5,
				LineHeightMultiple = 5,
				DefaultTabInterval = 5,
				HeadIndent = 5,
				FirstLineHeadIndent = 5,
				LineBreakMode = CTLineBreakMode.TruncatingHead,
				BaseWritingDirection = CTWritingDirection.Natural,
				Alignment = CTTextAlignment.Justified,
				TabStops = new [] {
					new CTTextTab (CTTextAlignment.Justified, 2),
					new CTTextTab (CTTextAlignment.Natural, 1)
				}
			};

			var style = new CTParagraphStyle (settings);
			Assert.DoesNotThrow (() => {
				ClassicAssert.AreEqual (settings.TailIndent, (nfloat) style.TailIndent, "TailIndent");
				ClassicAssert.AreEqual (settings.ParagraphSpacingBefore, (nfloat) style.ParagraphSpacingBefore, "ParagraphSpacingBefore");
				ClassicAssert.AreEqual (settings.ParagraphSpacing, (nfloat) style.ParagraphSpacing, "ParagraphSpacing");
				ClassicAssert.AreEqual (settings.LineSpacing, (nfloat) style.LineSpacing, "LineSpacing");
				ClassicAssert.AreEqual (settings.MinimumLineHeight, (nfloat) style.MinimumLineHeight, "MinimumLineHeight");
				ClassicAssert.AreEqual (settings.MaximumLineHeight, (nfloat) style.MaximumLineHeight, "MaximumLineHeight");
				ClassicAssert.AreEqual (settings.LineHeightMultiple, (nfloat) style.LineHeightMultiple, "LineHeightMultiple");
				ClassicAssert.AreEqual (settings.DefaultTabInterval, (nfloat) style.DefaultTabInterval, "DefaultTabInterval");
				ClassicAssert.AreEqual (settings.HeadIndent, (nfloat) style.HeadIndent, "HeadIndent");
				ClassicAssert.AreEqual (settings.FirstLineHeadIndent, (nfloat) style.FirstLineHeadIndent, "FirstLineHeadIndent");
				ClassicAssert.AreEqual (settings.LineBreakMode, style.LineBreakMode, "LineBreakMode");
				ClassicAssert.AreEqual (settings.BaseWritingDirection, style.BaseWritingDirection, "LineBreakMode");
				ClassicAssert.AreEqual (settings.Alignment, style.Alignment, "Alignment");

				var styleTabStops = style.GetTabStops ();
				ClassicAssert.AreEqual (settings.TabStops.Count (), styleTabStops.Length, "TabStops");
				ClassicAssert.True (styleTabStops.Any (t => t.Location == 2 && t.TextAlignment == CTTextAlignment.Justified));
				ClassicAssert.True (styleTabStops.Any (t => t.Location == 1 && t.TextAlignment == CTTextAlignment.Natural));
			});
		}
	}
}
