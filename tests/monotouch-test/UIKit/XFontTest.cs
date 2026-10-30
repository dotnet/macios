// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Collections.Generic;
using CoreText;

#if __MACOS__
using AppKit;
using XFont = AppKit.NSFont;
#else
using UIKit;
using XFont = UIKit.UIFont;
#endif

namespace MonoTouchFixtures.XKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontTest {
		static TestCaseData Factory (string name, Func<XFont?> factory, bool nullable = false)
		{
			return new TestCaseData (factory, nullable).SetName ($"Factory_{name}");
		}

		static XFont? FromDescriptor (bool legacy)
		{
			using var font = XFont.CreateSystemFont (12);
			using var descriptor = font.FontDescriptor;
			return legacy ? XFont.FromDescriptor (descriptor, 12) : XFont.CreateFromDescriptor (descriptor, 12);
		}

		static IEnumerable<TestCaseData> FontFactories ()
		{
			yield return Factory ("CreateFromName", () => XFont.CreateFromName ("Helvetica", 12));
			yield return Factory ("FromName", () => XFont.FromName ("Helvetica", 12));
			yield return Factory ("FromFontName", () => XFont.FromFontName ("Helvetica", 12));
			yield return Factory ("CreateFromDescriptor", () => FromDescriptor (false));
			yield return Factory ("FromDescriptor", () => FromDescriptor (true));
			yield return Factory ("FromDescription_Size", () => {
				using var font = XFont.CreateSystemFont (12);
				using var descriptor = font.FontDescriptor;
				return XFont.FromDescription (descriptor, 12);
			});
			yield return Factory ("CreateSystemFont", () => XFont.CreateSystemFont (12));
			yield return Factory ("SystemFontOfSize", () => XFont.SystemFontOfSize (12));
			yield return Factory ("CreateSystemFont_Weight", () => XFont.CreateSystemFont (12, (nfloat) 0));
			yield return Factory ("SystemFontOfSize_Weight", () => XFont.SystemFontOfSize (12, (nfloat) 0));
			yield return Factory ("CreateBoldSystemFont", () => XFont.CreateBoldSystemFont (12));
			yield return Factory ("BoldSystemFontOfSize", () => XFont.BoldSystemFontOfSize (12));
			yield return Factory ("CreateMonospacedDigitSystemFont", () => XFont.CreateMonospacedDigitSystemFont (12, (nfloat) 0));
			yield return Factory ("MonospacedDigitSystemFontOfSize", () => XFont.MonospacedDigitSystemFontOfSize (12, (nfloat) 0));
			yield return Factory ("CreateMonospacedSystemFont", () => XFont.CreateMonospacedSystemFont (12, (nfloat) 0));
			yield return Factory ("GetMonospacedSystemFont", () => XFont.GetMonospacedSystemFont (12, (nfloat) 0));
			yield return Factory ("MonospacedSystemFont", () => XFont.MonospacedSystemFont (12, 0));
			yield return Factory ("GetFont", () => {
				using var font = XFont.CreateSystemFont (10);
				return font.GetFont (12);
			});
			yield return Factory ("WithSize", () => {
				using var font = XFont.CreateSystemFont (10);
				return font.WithSize (12);
			});
			yield return Factory ("CreateWithSize", () => {
				using var font = XFont.CreateSystemFont (10);
				return font.CreateWithSize (12);
			});
			yield return Factory ("CreateSystemFont_Width", () => {
				TestRuntime.AssertXcodeVersion (14, 0);
				return XFont.CreateSystemFont (12, (nfloat) 0, (nfloat) 0);
			});
			yield return Factory ("SystemFontOfSize_Width", () => {
				TestRuntime.AssertXcodeVersion (14, 0);
				return XFont.SystemFontOfSize (12, (nfloat) 0, (nfloat) 0);
			});
#if __MACOS__
			yield return Factory ("CreateFromName_Matrix", () => XFont.CreateFromName ("Helvetica", new nfloat [] { 12, 0, 0, 12, 0, 0 }));
			yield return Factory ("PrinterFont", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.PrinterFont;
			});
			yield return Factory ("ScreenFont", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.ScreenFont;
			});
			yield return Factory ("ScreenFontWithRenderingMode", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.ScreenFontWithRenderingMode (NSFontRenderingMode.Default);
			});
			yield return Factory ("CreateScreenFont", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.CreateScreenFont (NSFontRenderingMode.Default);
			});
			yield return Factory ("CreateFromDescriptor_Transform", () => {
				using var font = XFont.CreateSystemFont (12);
				using var descriptor = font.FontDescriptor;
				using var transform = new NSAffineTransform ();
				return XFont.CreateFromDescriptor (descriptor, transform);
			});
			yield return Factory ("FromDescription_Transform", () => {
				using var font = XFont.CreateSystemFont (12);
				using var descriptor = font.FontDescriptor;
				return XFont.FromDescription (descriptor, null);
			});
			yield return Factory ("FromCTFont", () => {
				using var font = new CTFont ("Helvetica", 12);
				return XFont.FromCTFont (font);
			});
			yield return Factory ("CreateFromCTFont", () => {
				using var font = new CTFont ("Helvetica", 12);
				return XFont.CreateFromCTFont (font);
			});
			yield return Factory ("CreateUserFont", () => XFont.CreateUserFont (12), nullable: true);
			yield return Factory ("UserFontOfSize", () => XFont.UserFontOfSize (12), nullable: true);
			yield return Factory ("CreateUserFixedPitchFont", () => XFont.CreateUserFixedPitchFont (12), nullable: true);
			yield return Factory ("UserFixedPitchFontOfSize", () => XFont.UserFixedPitchFontOfSize (12), nullable: true);
			yield return Factory ("CreateLabelFont", () => XFont.CreateLabelFont (12));
			yield return Factory ("LabelFontOfSize", () => XFont.LabelFontOfSize (12));
			yield return Factory ("CreateTitleBarFont", () => XFont.CreateTitleBarFont (12));
			yield return Factory ("TitleBarFontOfSize", () => XFont.TitleBarFontOfSize (12));
			yield return Factory ("CreateMenuFont", () => XFont.CreateMenuFont (12));
			yield return Factory ("MenuFontOfSize", () => XFont.MenuFontOfSize (12));
			yield return Factory ("CreateMenuBarFont", () => XFont.CreateMenuBarFont (12));
			yield return Factory ("MenuBarFontOfSize", () => XFont.MenuBarFontOfSize (12));
			yield return Factory ("CreateMessageFont", () => XFont.CreateMessageFont (12));
			yield return Factory ("MessageFontOfSize", () => XFont.MessageFontOfSize (12));
			yield return Factory ("CreatePaletteFont", () => XFont.CreatePaletteFont (12));
			yield return Factory ("PaletteFontOfSize", () => XFont.PaletteFontOfSize (12));
			yield return Factory ("CreateToolTipsFont", () => XFont.CreateToolTipsFont (12));
			yield return Factory ("ToolTipsFontOfSize", () => XFont.ToolTipsFontOfSize (12));
			yield return Factory ("CreateControlContentFont", () => XFont.CreateControlContentFont (12));
			yield return Factory ("ControlContentFontOfSize", () => XFont.ControlContentFontOfSize (12));
			yield return Factory ("CreatePreferredFont", () => {
				using var options = new NSDictionary ();
				return XFont.CreatePreferredFont ("NSFontTextStyleBody", options);
			});
			yield return Factory ("GetPreferredFont", () => {
				using var options = new NSDictionary ();
				return XFont.GetPreferredFont ("NSFontTextStyleBody", options);
			});
			yield return Factory ("CreateVerticalFont", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.CreateVerticalFont ();
			});
			yield return Factory ("GetVerticalFont", () => {
				using var font = XFont.CreateSystemFont (12);
				return font.GetVerticalFont ();
			});
#else
			yield return Factory ("CreateSystemFont_WeightEnum", () => XFont.CreateSystemFont (12, UIFontWeight.Regular));
			yield return Factory ("SystemFontOfSize_WeightEnum", () => XFont.SystemFontOfSize (12, UIFontWeight.Regular));
			yield return Factory ("CreateSystemFont_WidthEnum", () => {
				TestRuntime.AssertXcodeVersion (14, 0);
				return XFont.CreateSystemFont (12, UIFontWeight.Regular, UIFontWidth.Standard);
			});
			yield return Factory ("SystemFontOfSize_WidthEnum", () => {
				TestRuntime.AssertXcodeVersion (14, 0);
				return XFont.SystemFontOfSize (12, UIFontWeight.Regular, UIFontWidth.Standard);
			});
			yield return Factory ("CreateMonospacedDigitSystemFont_WeightEnum", () => XFont.CreateMonospacedDigitSystemFont (12, UIFontWeight.Regular));
			yield return Factory ("MonospacedDigitSystemFontOfSize_WeightEnum", () => XFont.MonospacedDigitSystemFontOfSize (12, UIFontWeight.Regular));
			yield return Factory ("CreateMonospacedSystemFont_WeightEnum", () => XFont.CreateMonospacedSystemFont (12, UIFontWeight.Regular));
			yield return Factory ("GetMonospacedSystemFont_WeightEnum", () => XFont.GetMonospacedSystemFont (12, UIFontWeight.Regular));
			yield return Factory ("CreateItalicSystemFont", () => XFont.CreateItalicSystemFont (12));
			yield return Factory ("ItalicSystemFontOfSize", () => XFont.ItalicSystemFontOfSize (12));
			yield return Factory ("CreatePreferredFont", () => XFont.CreatePreferredFont (UIFontTextStyle.Body));
			yield return Factory ("GetPreferredFontForTextStyle", () => XFont.GetPreferredFontForTextStyle (UIFontTextStyle.Body));
			yield return Factory ("CreatePreferredFont_NullTraits", () => XFont.CreatePreferredFont (UIFontTextStyle.Body, null));
			yield return Factory ("GetPreferredFontForTextStyle_NullTraits", () => XFont.GetPreferredFontForTextStyle (UIFontTextStyle.Body, null));
			yield return Factory ("CreatePreferredFont_String", () => {
				using var style = new NSString ("UICTFontTextStyleBody");
				return XFont.CreatePreferredFont (style);
			});
			yield return Factory ("GetPreferredFontForTextStyle_String", () => {
				using var style = new NSString ("UICTFontTextStyleBody");
				return XFont.GetPreferredFontForTextStyle (style);
			});
			yield return Factory ("CreatePreferredFont_StringTraits", () => {
				using var style = new NSString ("UICTFontTextStyleBody");
				using var traits = new UITraitCollection ();
				return XFont.CreatePreferredFont (style, traits);
			});
			yield return Factory ("GetPreferredFontForTextStyle_StringTraits", () => {
				using var style = new NSString ("UICTFontTextStyleBody");
				using var traits = new UITraitCollection ();
				return XFont.GetPreferredFontForTextStyle (style, traits);
			});
			yield return Factory ("PreferredBody", () => XFont.PreferredBody);
			yield return Factory ("PreferredHeadline", () => XFont.PreferredHeadline);
			yield return Factory ("PreferredSubheadline", () => XFont.PreferredSubheadline);
			yield return Factory ("PreferredFootnote", () => XFont.PreferredFootnote);
			yield return Factory ("PreferredCaption1", () => XFont.PreferredCaption1);
			yield return Factory ("PreferredCaption2", () => XFont.PreferredCaption2);
			yield return Factory ("PreferredTitle1", () => XFont.PreferredTitle1);
			yield return Factory ("PreferredTitle2", () => XFont.PreferredTitle2);
			yield return Factory ("PreferredTitle3", () => XFont.PreferredTitle3);
			yield return Factory ("PreferredCallout", () => XFont.PreferredCallout);
#endif
		}

		[TestCaseSource (nameof (FontFactories))]
		public void FactoryReturnsIndependentWrappers (Func<XFont?> factory, bool nullable)
		{
			using var first = factory ();
			using var second = factory ();
			if (first is null || second is null) {
				Assert.That (nullable, Is.True, "Only user-selected fonts may be unavailable.");
				Assert.That (first, Is.Null, "First font");
				Assert.That (second, Is.Null, "Second font");
				return;
			}
			Assert.That (first, Is.Not.SameAs (second), "Managed wrappers");

			var name = second.FontName;
			var size = second.PointSize;
			first.Dispose ();
			Assert.That (first.Handle, Is.EqualTo (NativeHandle.Zero), "Disposed font");
			Assert.That (second.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Surviving font");
			Assert.That (second.FontName, Is.EqualTo (name), "Surviving font name");
			Assert.That (second.PointSize, Is.EqualTo (size), "Surviving font size");

			using var third = factory () ?? throw new AssertionException ("Font created after disposal is null.");
			Assert.That (third, Is.Not.SameAs (first), "New managed wrapper");
			Assert.That (third, Is.Not.SameAs (second), "Surviving wrapper is not reused");
			Assert.That (third.Handle, Is.Not.EqualTo (NativeHandle.Zero), "New font handle");
			Assert.That (third.FontName, Is.EqualTo (name), "New font name");
			Assert.That (third.PointSize, Is.EqualTo (size), "New font size");
		}

		[Test]
		public void InvalidFontName ()
		{
			const string name = "This font does not exist - issue 6241";
			using var font = XFont.CreateFromName (name, 12);
			using var legacy = XFont.FromName (name, 12);
			Assert.That (font, Is.Null, "CreateFromName");
			Assert.That (legacy, Is.Null, "FromName");
#if __MACOS__
			using var original = XFont.FromFontName (name, 12);
			Assert.That (original, Is.Null, "FromFontName");
			using var descriptor = NSFontDescriptor.FromNameSize (name, 12);
			using var fromDescriptor = XFont.CreateFromDescriptor (descriptor, 12);
			using var fromDescription = XFont.FromDescription (descriptor, 12);
			using var fromTransform = XFont.CreateFromDescriptor (descriptor, (NSAffineTransform?) null);
			Assert.That (fromDescriptor, Is.Null, "CreateFromDescriptor");
			Assert.That (fromDescription, Is.Null, "FromDescription");
			Assert.That (fromTransform, Is.Null, "CreateFromDescriptor with null transform");
			Assert.That (XFont.CreateFromCTFont (null), Is.Null, "CreateFromCTFont");
			Assert.That (XFont.FromCTFont (null), Is.Null, "FromCTFont");
#endif
		}

		[Test]
#pragma warning disable CS8625 // Deliberately pass null to verify argument validation.
		public void NullArguments ()
		{
			Assert.Throws<ArgumentNullException> (() => XFont.CreateFromName (null, 12), "Name");
			Assert.Throws<ArgumentNullException> (() => XFont.CreateFromDescriptor (null, 12), "Descriptor");
			var nameException = Assert.Throws<ArgumentNullException> (() => XFont.FromFontName (null, 12));
			Assert.That (nameException?.ParamName, Is.EqualTo ("fontName"), "Legacy name parameter");
			var descriptorException = Assert.Throws<ArgumentNullException> (() => XFont.FromDescription (null, 12));
			Assert.That (descriptorException?.ParamName, Is.EqualTo ("fontDescriptor"), "Legacy descriptor parameter");
#if __MACOS__
			Assert.Throws<ArgumentNullException> (() => XFont.CreateFromName (null, XFont.IdentityMatrix), "Matrix name");
			Assert.Throws<ArgumentNullException> (() => XFont.CreateFromName ("Helvetica", (nfloat []?) null), "Matrix");
#else
			Assert.Throws<ArgumentNullException> (() => XFont.CreatePreferredFont (null), "Text style");
			Assert.Throws<ArgumentOutOfRangeException> (() => XFont.CreatePreferredFont ((UIFontTextStyle) int.MaxValue), "Invalid text style");
#endif
		}
#pragma warning restore CS8625

		[Test]
		public void Equality ()
		{
			using var first = XFont.CreateSystemFont (12);
			using var second = XFont.CreateSystemFont (12);
			using var different = XFont.CreateBoldSystemFont (24);
			XFont? missing = null;
			Assert.That (first == second, Is.True, "Equal fonts");
			Assert.That (first == second, Is.EqualTo (first.Handle == second.Handle), "Native identity");
			Assert.That (first != second, Is.False, "Equal fonts inequality");
			Assert.That (first.Equals ((object) second), Is.True, "Object equality");
			Assert.That (first.Equals (second), Is.True, "NSObject equality");
			Assert.That (first.GetHashCode (), Is.EqualTo (second.GetHashCode ()), "Equal hashes");
			Assert.That (first == different, Is.False, "Different fonts");
			Assert.That (first != different, Is.True, "Different fonts inequality");
			Assert.That (first.Equals ("not a font"), Is.False, "Unrelated object");
			Assert.That (first.Name, Is.EqualTo (first.FontName), "Name aliases");
			Assert.That (first.Equals ((object?) null), Is.False, "Object null");
			Assert.That (first == missing, Is.False, "Font and null");
			Assert.That (missing == first, Is.False, "Null and font");
			Assert.That (missing == (XFont?) null, Is.True, "Two nulls");
			Assert.That (missing != first, Is.True, "Null inequality");
		}

		[Test]
		public void StringRepresentation ()
		{
			using var font = XFont.CreateSystemFont (12);
			Assert.That (font.ToString (), Is.EqualTo ($"{font.FontName} {font.PointSize}"), "Font name and size");
		}

#if !__MACOS__
		[TestCase (UIFontWidth.Condensed)]
		[TestCase (UIFontWidth.Standard)]
		[TestCase (UIFontWidth.Expanded)]
		[TestCase (UIFontWidth.Compressed)]
		public void FontWidth (UIFontWidth width)
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			var constant = width.GetConstant ();
			Assert.That (constant, Is.Not.Null, "Native width constant");
			Assert.That (UIFontWidthExtensions.GetValue (constant.GetValueOrDefault ()), Is.EqualTo (width), "Width round trip");
			using var font = XFont.CreateSystemFont (12, UIFontWeight.Regular, width);
			using var native = XFont.CreateSystemFont (12, UIFontWeight.Regular.GetWeight (), constant.GetValueOrDefault ());
			Assert.That (font, Is.EqualTo (native), "Width factory");
		}

		[Test]
		public void InvalidFontWidth ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			var width = (UIFontWidth) int.MaxValue;
			Assert.That (width.GetConstant (), Is.Null, "Unknown width");
			Assert.Throws<ArgumentException> (() => XFont.CreateSystemFont (12, UIFontWeight.Regular, width), "Canonical factory");
			Assert.Throws<ArgumentException> (() => XFont.SystemFontOfSize (12, UIFontWeight.Regular, width), "Legacy factory");
		}
#endif

#if __MACOS__
		[Test]
		public void FontMatrix ()
		{
			nfloat [] matrix = { 12, 0, 0, 12, 0, 0 };
			using var font = XFont.CreateFromName ("Helvetica", matrix) ?? throw new AssertionException ("Font is null.");
			Assert.That (font.Matrix, Is.EqualTo (matrix), "Round-trip matrix");
			var copy = font.Matrix;
			copy [0] = 100;
			Assert.That (font.Matrix, Is.EqualTo (matrix), "Native matrix is not exposed");
			Assert.That (XFont.IdentityMatrix, Is.EqualTo (new nfloat [] { 1, 0, 0, 1, 0, 0 }), "Identity matrix");
			using var missing = XFont.CreateFromName ("This font does not exist - issue 6241", matrix);
			Assert.That (missing, Is.Null, "Missing matrix font");
			Assert.Throws<ArgumentException> (() => XFont.CreateFromName ("Helvetica", new nfloat [5]), "Invalid matrix size");
		}

		[Test]
		public void LegacyGlyphMetrics ()
		{
			using var font = XFont.CreateFromName ("Helvetica", 12) ?? throw new AssertionException ("Font is null.");
			var glyph = font.GlyphWithName ("A");
			Assert.That (glyph, Is.Not.Zero, "Glyph");
			Assert.That (font.GetGlyphWithName ("A"), Is.EqualTo (glyph), "Canonical glyph lookup");
			Assert.That (font.GetBoundingRect (glyph), Is.EqualTo (font.BoundingRectForGlyph (glyph)), "Canonical glyph bounds");
			Assert.That (font.GetAdvancement (glyph), Is.EqualTo (font.AdvancementForGlyph (glyph)), "Canonical glyph advancement");
			Assert.That (font.GetBoundingRects (new uint [] { glyph }), Is.EqualTo (new [] { font.BoundingRectForGlyph (glyph) }), "Bounds");
			Assert.That (font.GetAdvancements (new uint [] { glyph }), Is.EqualTo (new [] { font.AdvancementForGlyph (glyph) }), "Advancements");
			uint [] empty = [];
			Assert.That (font.GetBoundingRects (empty), Is.Empty, "Empty bounds");
			Assert.That (font.GetAdvancements (empty), Is.Empty, "Empty advancements");
		}
#endif

		[Test]
		public void FontInformation ()
		{
#if __MACOS__
			Assert.That (XFont.GetSystemFontSize (NSControlSize.Small), Is.EqualTo (XFont.SystemFontSizeForControlSize (NSControlSize.Small)), "Control font size");
#else
			Assert.That (XFont.GetFontNames ("Helvetica"), Does.Contain ("Helvetica"), "Font family");
#endif
		}
	}
}
