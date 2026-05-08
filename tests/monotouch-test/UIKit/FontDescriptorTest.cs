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
			ClassicAssert.IsNull (attrs.Family, "Family");
			ClassicAssert.IsNull (attrs.Name, "Name");
			ClassicAssert.IsNull (attrs.Face, "Face");
			ClassicAssert.IsNull (attrs.Size, "Size");
			ClassicAssert.IsNull (attrs.VisibleName, "VisibleName");
			ClassicAssert.IsNull (attrs.TextStyle, "TextStyle");
			ClassicAssert.IsNull (attrs.Matrix, "Matrix");
			ClassicAssert.IsNull (attrs.CharacterSet, "CharacterSet");
			ClassicAssert.IsNull (attrs.CascadeList, "CascadeList");
			ClassicAssert.IsNull (attrs.Traits, "Traits");
			ClassicAssert.IsNull (attrs.FixedAdvance, "FixedAdvance");
			ClassicAssert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings");
			ClassicAssert.IsNotNull (attrs.FeatureSettings, "FeatureSettings");
			ClassicAssert.AreEqual (0, attrs.FeatureSettings.Length, "FeatureSettings.Length");
		}

		[Test]
		public void UIFontAttributes_StringProperties ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Family = "Helvetica";
			ClassicAssert.AreEqual ("Helvetica", attrs.Family, "Family set");

			attrs.Name = "Helvetica-Bold";
			ClassicAssert.AreEqual ("Helvetica-Bold", attrs.Name, "Name set");

			attrs.Face = "Bold";
			ClassicAssert.AreEqual ("Bold", attrs.Face, "Face set");

			attrs.VisibleName = "Helvetica Bold";
			ClassicAssert.AreEqual ("Helvetica Bold", attrs.VisibleName, "VisibleName set");

			// Set back to null
			attrs.Family = null;
			ClassicAssert.IsNull (attrs.Family, "Family cleared");
		}

		[Test]
		public void UIFontAttributes_Size ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Size = 14.0f;
			ClassicAssert.IsTrue (attrs.Size.HasValue, "Size.HasValue");
			ClassicAssert.AreEqual (14.0f, attrs.Size.Value, "Size.Value");

			attrs.Size = null;
			ClassicAssert.IsNull (attrs.Size, "Size cleared");
		}

		[Test]
		public void UIFontAttributes_TextStyle_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			attrs.TextStyle = UIFontTextStyle.Body.GetConstant ();
			ClassicAssert.IsNotNull (attrs.TextStyle, "TextStyle set");

			attrs.TextStyle = null;
			ClassicAssert.IsNull (attrs.TextStyle, "TextStyle cleared");
		}

		[Test]
		public void UIFontAttributes_Matrix ()
		{
			var attrs = new UIFontAttributes ();
			var transform = CGAffineTransform.MakeScale (2, 2);

			attrs.Matrix = transform;
			ClassicAssert.IsTrue (attrs.Matrix.HasValue, "Matrix.HasValue");
			ClassicAssert.AreEqual (transform, attrs.Matrix.Value, "Matrix.Value");

			attrs.Matrix = null;
			ClassicAssert.IsNull (attrs.Matrix, "Matrix cleared");
		}

		[Test]
		public void UIFontAttributes_CharacterSet_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var cs = NSCharacterSet.UppercaseLetters;

			attrs.CharacterSet = cs;
			ClassicAssert.IsNotNull (attrs.CharacterSet, "CharacterSet set");

			attrs.CharacterSet = null;
			ClassicAssert.IsNull (attrs.CharacterSet, "CharacterSet cleared");
		}

		[Test]
		public void UIFontAttributes_CascadeList_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var desc = new UIFontDescriptor ();

			attrs.CascadeList = new [] { desc };
			ClassicAssert.IsNotNull (attrs.CascadeList, "CascadeList set");
			ClassicAssert.AreEqual (1, attrs.CascadeList.Length, "CascadeList.Length");

			attrs.CascadeList = null;
			ClassicAssert.IsNull (attrs.CascadeList, "CascadeList cleared");
		}

		[Test]
		public void UIFontAttributes_Traits_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var traits = new UIFontTraits ();
			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;

			attrs.Traits = traits;
			ClassicAssert.IsNotNull (attrs.Traits, "Traits set");
			ClassicAssert.AreEqual (UIFontDescriptorSymbolicTraits.Bold, attrs.Traits.SymbolicTrait, "Traits.SymbolicTrait");

			attrs.Traits = null;
			ClassicAssert.IsNull (attrs.Traits, "Traits cleared");
		}

		[Test]
		public void UIFontAttributes_FixedAdvance ()
		{
			var attrs = new UIFontAttributes ();

			attrs.FixedAdvance = 10.0f;
			ClassicAssert.IsTrue (attrs.FixedAdvance.HasValue, "FixedAdvance.HasValue");
			ClassicAssert.AreEqual (10.0f, attrs.FixedAdvance.Value, "FixedAdvance.Value");

			attrs.FixedAdvance = null;
			ClassicAssert.IsNull (attrs.FixedAdvance, "FixedAdvance cleared");
		}

		[Test]
		public void UIFontDescriptor_Properties_FromFont ()
		{
			var font = UIFont.BoldSystemFontOfSize (20);
			var descriptor = font.FontDescriptor;

			ClassicAssert.IsNotNull (descriptor.Family, "Family");
			ClassicAssert.IsNotNull (descriptor.Name, "Name");
			ClassicAssert.IsNotNull (descriptor.Face, "Face");
			ClassicAssert.IsTrue (descriptor.Size.HasValue, "Size.HasValue");
			ClassicAssert.AreEqual (20.0f, descriptor.Size.Value, "Size.Value");
			ClassicAssert.IsNotNull (descriptor.CascadeList, "CascadeList");
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

			ClassicAssert.IsNotNull (cascadeList, "CascadeList never null");
			ClassicAssert.IsNotNull (featureSettings, "FeatureSettings never null");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle1 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle1;
			ClassicAssert.IsNotNull (descriptor, "PreferredTitle1");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle2 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle2;
			ClassicAssert.IsNotNull (descriptor, "PreferredTitle2");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle3 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle3;
			ClassicAssert.IsNotNull (descriptor, "PreferredTitle3");
		}

		[Test]
		public void UIFontDescriptor_PreferredCallout ()
		{
			var descriptor = UIFontDescriptor.PreferredCallout;
			ClassicAssert.IsNotNull (descriptor, "PreferredCallout");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_Empty ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors ();
			ClassicAssert.IsNotNull (results, "empty mandatoryKeys");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_WithKeys ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors (UIFontDescriptorAttribute.Family);
			ClassicAssert.IsNotNull (results, "with Family key");
		}

		[Test]
		public void UIFontTraits_DefaultConstructor ()
		{
			var traits = new UIFontTraits ();
			ClassicAssert.IsNull (traits.SymbolicTrait, "SymbolicTrait");
			ClassicAssert.IsNull (traits.Weight, "Weight");
			ClassicAssert.IsNull (traits.Width, "Width");
			ClassicAssert.IsNull (traits.Slant, "Slant");
		}

		[Test]
		public void UIFontTraits_SymbolicTrait_SetAndClear ()
		{
			var traits = new UIFontTraits ();

			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;
			ClassicAssert.IsTrue (traits.SymbolicTrait.HasValue, "SymbolicTrait.HasValue after set");
			ClassicAssert.AreEqual (UIFontDescriptorSymbolicTraits.Bold, traits.SymbolicTrait.Value, "SymbolicTrait.Value");

			traits.SymbolicTrait = null;
			ClassicAssert.IsNull (traits.SymbolicTrait, "SymbolicTrait after null");
		}

		[Test]
		public void UIFontTraits_FromDescriptor ()
		{
			var font = UIFont.BoldSystemFontOfSize (12);
			var descriptor = font.FontDescriptor;
			var traits = descriptor.Traits;

			ClassicAssert.IsNotNull (traits, "Traits from bold font");
			ClassicAssert.IsTrue (traits.SymbolicTrait.HasValue, "SymbolicTrait.HasValue");
			ClassicAssert.IsTrue (traits.SymbolicTrait.Value.HasFlag (UIFontDescriptorSymbolicTraits.Bold), "Has Bold trait");
		}

		[Test]
		public void UIFontAttributes_FromDictionary ()
		{
			var font = UIFont.BoldSystemFontOfSize (14);
			var descriptor = font.FontDescriptor;
			var fontAttrs = descriptor.FontAttributes;

			ClassicAssert.IsTrue (fontAttrs.Size.HasValue, "Size.HasValue");
			ClassicAssert.AreEqual (14.0f, fontAttrs.Size.Value, "Size.Value");
		}

		[Test]
		public void UIFontAttributes_WeakFeatureSettings_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			ClassicAssert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings initially null");

			attrs.WeakFeatureSettings = null;
			ClassicAssert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings after null set");
		}
	}
}
#endif
