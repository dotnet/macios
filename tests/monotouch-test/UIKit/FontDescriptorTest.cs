// Copyright 2026 Microsoft Corp. All rights reserved

#if !MONOMAC
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontDescriptorTest {

		[Test]
		public void UIFontAttributes_DefaultConstructor ()
		{
			var attrs = new UIFontAttributes ();
			Assert.That (attrs.Family, Is.Null, "Family");
			Assert.That (attrs.Name, Is.Null, "Name");
			Assert.That (attrs.Face, Is.Null, "Face");
			Assert.That (attrs.Size, Is.Null, "Size");
			Assert.That (attrs.VisibleName, Is.Null, "VisibleName");
			Assert.That (attrs.TextStyle, Is.Null, "TextStyle");
			Assert.That (attrs.Matrix, Is.Null, "Matrix");
			Assert.That (attrs.CharacterSet, Is.Null, "CharacterSet");
			Assert.That (attrs.CascadeList, Is.Null, "CascadeList");
			Assert.That (attrs.Traits, Is.Null, "Traits");
			Assert.That (attrs.FixedAdvance, Is.Null, "FixedAdvance");
			Assert.That (attrs.WeakFeatureSettings, Is.Null, "WeakFeatureSettings");
			Assert.That (attrs.FeatureSettings, Is.Not.Null, "FeatureSettings");
			Assert.That (attrs.FeatureSettings.Length, Is.EqualTo (0), "FeatureSettings.Length");
		}

		[Test]
		public void UIFontAttributes_StringProperties ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Family = "Helvetica";
			Assert.That (attrs.Family, Is.EqualTo ("Helvetica"), "Family set");

			attrs.Name = "Helvetica-Bold";
			Assert.That (attrs.Name, Is.EqualTo ("Helvetica-Bold"), "Name set");

			attrs.Face = "Bold";
			Assert.That (attrs.Face, Is.EqualTo ("Bold"), "Face set");

			attrs.VisibleName = "Helvetica Bold";
			Assert.That (attrs.VisibleName, Is.EqualTo ("Helvetica Bold"), "VisibleName set");

			// Set back to null
			attrs.Family = null;
			Assert.That (attrs.Family, Is.Null, "Family cleared");
		}

		[Test]
		public void UIFontAttributes_Size ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Size = 14.0f;
			Assert.That (attrs.Size.HasValue, Is.True, "Size.HasValue");
			Assert.That (attrs.Size.Value, Is.EqualTo (14.0f), "Size.Value");

			attrs.Size = null;
			Assert.That (attrs.Size, Is.Null, "Size cleared");
		}

		[Test]
		public void UIFontAttributes_TextStyle_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			attrs.TextStyle = UIFontTextStyle.Body.GetConstant ();
			Assert.That (attrs.TextStyle, Is.Not.Null, "TextStyle set");

			attrs.TextStyle = null;
			Assert.That (attrs.TextStyle, Is.Null, "TextStyle cleared");
		}

		[Test]
		public void UIFontAttributes_Matrix ()
		{
			var attrs = new UIFontAttributes ();
			var transform = CGAffineTransform.MakeScale (2, 2);

			attrs.Matrix = transform;
			Assert.That (attrs.Matrix.HasValue, Is.True, "Matrix.HasValue");
			Assert.That (attrs.Matrix.Value, Is.EqualTo (transform), "Matrix.Value");

			attrs.Matrix = null;
			Assert.That (attrs.Matrix, Is.Null, "Matrix cleared");
		}

		[Test]
		public void UIFontAttributes_CharacterSet_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var cs = NSCharacterSet.UppercaseLetters;

			attrs.CharacterSet = cs;
			Assert.That (attrs.CharacterSet, Is.Not.Null, "CharacterSet set");

			attrs.CharacterSet = null;
			Assert.That (attrs.CharacterSet, Is.Null, "CharacterSet cleared");
		}

		[Test]
		public void UIFontAttributes_CascadeList_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var desc = new UIFontDescriptor ();

			attrs.CascadeList = new [] { desc };
			Assert.That (attrs.CascadeList, Is.Not.Null, "CascadeList set");
			Assert.That (attrs.CascadeList.Length, Is.EqualTo (1), "CascadeList.Length");

			attrs.CascadeList = null;
			Assert.That (attrs.CascadeList, Is.Null, "CascadeList cleared");
		}

		[Test]
		public void UIFontAttributes_Traits_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var traits = new UIFontTraits ();
			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;

			attrs.Traits = traits;
			Assert.That (attrs.Traits, Is.Not.Null, "Traits set");
			Assert.That (attrs.Traits.SymbolicTrait, Is.EqualTo (UIFontDescriptorSymbolicTraits.Bold), "Traits.SymbolicTrait");

			attrs.Traits = null;
			Assert.That (attrs.Traits, Is.Null, "Traits cleared");
		}

		[Test]
		public void UIFontAttributes_FixedAdvance ()
		{
			var attrs = new UIFontAttributes ();

			attrs.FixedAdvance = 10.0f;
			Assert.That (attrs.FixedAdvance.HasValue, Is.True, "FixedAdvance.HasValue");
			Assert.That (attrs.FixedAdvance.Value, Is.EqualTo (10.0f), "FixedAdvance.Value");

			attrs.FixedAdvance = null;
			Assert.That (attrs.FixedAdvance, Is.Null, "FixedAdvance cleared");
		}

		[Test]
		public void UIFontDescriptor_Properties_FromFont ()
		{
			var font = UIFont.BoldSystemFontOfSize (20);
			var descriptor = font.FontDescriptor;

			Assert.That (descriptor.Family, Is.Not.Null, "Family");
			Assert.That (descriptor.Name, Is.Not.Null, "Name");
			Assert.That (descriptor.Face, Is.Not.Null, "Face");
			Assert.That (descriptor.Size.HasValue, Is.True, "Size.HasValue");
			Assert.That (descriptor.Size.Value, Is.EqualTo (20.0f), "Size.Value");
			Assert.That (descriptor.CascadeList, Is.Not.Null, "CascadeList");
		}

		[Test]
		public void UIFontDescriptor_EmptyDescriptor_NullableProperties ()
		{
			var descriptor = new UIFontDescriptor ();

			// These may or may not be null depending on system defaults,
			// but they should not throw
			var family = descriptor.Family;
			var name = descriptor.Name;
			var face = descriptor.Face;
			var size = descriptor.Size;
			var visibleName = descriptor.VisibleName;
			var textStyle = descriptor.TextStyle;
			var charSet = descriptor.CharacterSet;
			var cascadeList = descriptor.CascadeList;
			var traits = descriptor.Traits;
			var fixedAdvance = descriptor.FixedAdvance;
			var weakFeature = descriptor.WeakFeatureSettings;
			var featureSettings = descriptor.FeatureSettings;

			Assert.That (cascadeList, Is.Not.Null, "CascadeList never null");
			Assert.That (featureSettings, Is.Not.Null, "FeatureSettings never null");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle1 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle1;
			Assert.That (descriptor, Is.Not.Null, "PreferredTitle1");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle2 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle2;
			Assert.That (descriptor, Is.Not.Null, "PreferredTitle2");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle3 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle3;
			Assert.That (descriptor, Is.Not.Null, "PreferredTitle3");
		}

		[Test]
		public void UIFontDescriptor_PreferredCallout ()
		{
			var descriptor = UIFontDescriptor.PreferredCallout;
			Assert.That (descriptor, Is.Not.Null, "PreferredCallout");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_Empty ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors ();
			Assert.That (results, Is.Not.Null, "empty mandatoryKeys");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_WithKeys ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors (UIFontDescriptorAttribute.Family);
			Assert.That (results, Is.Not.Null, "with Family key");
		}

		[Test]
		public void UIFontTraits_DefaultConstructor ()
		{
			var traits = new UIFontTraits ();
			Assert.That (traits.SymbolicTrait, Is.Null, "SymbolicTrait");
			Assert.That (traits.Weight, Is.Null, "Weight");
			Assert.That (traits.Width, Is.Null, "Width");
			Assert.That (traits.Slant, Is.Null, "Slant");
		}

		[Test]
		public void UIFontTraits_SymbolicTrait_SetAndClear ()
		{
			var traits = new UIFontTraits ();

			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;
			Assert.That (traits.SymbolicTrait.HasValue, Is.True, "SymbolicTrait.HasValue after set");
			Assert.That (traits.SymbolicTrait.Value, Is.EqualTo (UIFontDescriptorSymbolicTraits.Bold), "SymbolicTrait.Value");

			traits.SymbolicTrait = null;
			Assert.That (traits.SymbolicTrait, Is.Null, "SymbolicTrait after null");
		}

		[Test]
		public void UIFontTraits_FromDescriptor ()
		{
			var font = UIFont.BoldSystemFontOfSize (12);
			var descriptor = font.FontDescriptor;
			var traits = descriptor.Traits;

			Assert.That (traits, Is.Not.Null, "Traits from bold font");
			Assert.That (traits.SymbolicTrait.HasValue, Is.True, "SymbolicTrait.HasValue");
			Assert.That (traits.SymbolicTrait.Value.HasFlag (UIFontDescriptorSymbolicTraits.Bold), Is.True, "Has Bold trait");
		}

		[Test]
		public void UIFontAttributes_FromDictionary ()
		{
			var font = UIFont.BoldSystemFontOfSize (14);
			var descriptor = font.FontDescriptor;
			var fontAttrs = descriptor.FontAttributes;

			Assert.That (fontAttrs.Size.HasValue, Is.True, "Size.HasValue");
			Assert.That (fontAttrs.Size.Value, Is.EqualTo (14.0f), "Size.Value");
		}

		[Test]
		public void UIFontAttributes_WeakFeatureSettings_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			Assert.That (attrs.WeakFeatureSettings, Is.Null, "WeakFeatureSettings initially null");

			attrs.WeakFeatureSettings = null;
			Assert.That (attrs.WeakFeatureSettings, Is.Null, "WeakFeatureSettings after null set");
		}
	}
}
#endif
