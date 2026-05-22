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
				Assert.That ((nfloat) style.TailIndent, Is.EqualTo (settings.TailIndent), "TailIndent");
				Assert.That ((nfloat) style.ParagraphSpacingBefore, Is.EqualTo (settings.ParagraphSpacingBefore), "ParagraphSpacingBefore");
				Assert.That ((nfloat) style.ParagraphSpacing, Is.EqualTo (settings.ParagraphSpacing), "ParagraphSpacing");
				Assert.That ((nfloat) style.LineSpacing, Is.EqualTo (settings.LineSpacing), "LineSpacing");
				Assert.That ((nfloat) style.MinimumLineHeight, Is.EqualTo (settings.MinimumLineHeight), "MinimumLineHeight");
				Assert.That ((nfloat) style.MaximumLineHeight, Is.EqualTo (settings.MaximumLineHeight), "MaximumLineHeight");
				Assert.That ((nfloat) style.LineHeightMultiple, Is.EqualTo (settings.LineHeightMultiple), "LineHeightMultiple");
				Assert.That ((nfloat) style.DefaultTabInterval, Is.EqualTo (settings.DefaultTabInterval), "DefaultTabInterval");
				Assert.That ((nfloat) style.HeadIndent, Is.EqualTo (settings.HeadIndent), "HeadIndent");
				Assert.That ((nfloat) style.FirstLineHeadIndent, Is.EqualTo (settings.FirstLineHeadIndent), "FirstLineHeadIndent");
				Assert.That (style.LineBreakMode, Is.EqualTo (settings.LineBreakMode), "LineBreakMode");
				Assert.That (style.BaseWritingDirection, Is.EqualTo (settings.BaseWritingDirection), "LineBreakMode");
				Assert.That (style.Alignment, Is.EqualTo (settings.Alignment), "Alignment");

				var styleTabStops = style.GetTabStops ();
				Assert.That (styleTabStops.Length, Is.EqualTo (settings.TabStops.Count ()), "TabStops");
				Assert.That (styleTabStops.Any (t => t.Location == 2 && t.TextAlignment == CTTextAlignment.Justified), Is.True, "Has Justified tab at 2");
				Assert.That (styleTabStops.Any (t => t.Location == 1 && t.TextAlignment == CTTextAlignment.Natural), Is.True, "Has Natural tab at 1");
			});
		}
	}
}
