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
			Assert.IsNull (attrs.Family, "Family");
			Assert.IsNull (attrs.Name, "Name");
			Assert.IsNull (attrs.Face, "Face");
			Assert.IsNull (attrs.Size, "Size");
			Assert.IsNull (attrs.VisibleName, "VisibleName");
			Assert.IsNull (attrs.TextStyle, "TextStyle");
			Assert.IsNull (attrs.Matrix, "Matrix");
			Assert.IsNull (attrs.CharacterSet, "CharacterSet");
			Assert.IsNull (attrs.CascadeList, "CascadeList");
			Assert.IsNull (attrs.Traits, "Traits");
			Assert.IsNull (attrs.FixedAdvance, "FixedAdvance");
			Assert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings");
			Assert.IsNotNull (attrs.FeatureSettings, "FeatureSettings");
			Assert.AreEqual (0, attrs.FeatureSettings.Length, "FeatureSettings.Length");
		}

		[Test]
		public void UIFontAttributes_StringProperties ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Family = "Helvetica";
			Assert.AreEqual ("Helvetica", attrs.Family, "Family set");

			attrs.Name = "Helvetica-Bold";
			Assert.AreEqual ("Helvetica-Bold", attrs.Name, "Name set");

			attrs.Face = "Bold";
			Assert.AreEqual ("Bold", attrs.Face, "Face set");

			attrs.VisibleName = "Helvetica Bold";
			Assert.AreEqual ("Helvetica Bold", attrs.VisibleName, "VisibleName set");

			// Set back to null
			attrs.Family = null;
			Assert.IsNull (attrs.Family, "Family cleared");
		}

		[Test]
		public void UIFontAttributes_Size ()
		{
			var attrs = new UIFontAttributes ();

			attrs.Size = 14.0f;
			Assert.IsTrue (attrs.Size.HasValue, "Size.HasValue");
			Assert.AreEqual (14.0f, attrs.Size.Value, "Size.Value");

			attrs.Size = null;
			Assert.IsNull (attrs.Size, "Size cleared");
		}

		[Test]
		public void UIFontAttributes_TextStyle_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			attrs.TextStyle = UIFontTextStyle.Body.GetConstant ();
			Assert.IsNotNull (attrs.TextStyle, "TextStyle set");

			attrs.TextStyle = null;
			Assert.IsNull (attrs.TextStyle, "TextStyle cleared");
		}

		[Test]
		public void UIFontAttributes_Matrix ()
		{
			var attrs = new UIFontAttributes ();
			var transform = CGAffineTransform.MakeScale (2, 2);

			attrs.Matrix = transform;
			Assert.IsTrue (attrs.Matrix.HasValue, "Matrix.HasValue");
			Assert.AreEqual (transform, attrs.Matrix.Value, "Matrix.Value");

			attrs.Matrix = null;
			Assert.IsNull (attrs.Matrix, "Matrix cleared");
		}

		[Test]
		public void UIFontAttributes_CharacterSet_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var cs = NSCharacterSet.UppercaseLetters;

			attrs.CharacterSet = cs;
			Assert.IsNotNull (attrs.CharacterSet, "CharacterSet set");

			attrs.CharacterSet = null;
			Assert.IsNull (attrs.CharacterSet, "CharacterSet cleared");
		}

		[Test]
		public void UIFontAttributes_CascadeList_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var desc = new UIFontDescriptor ();

			attrs.CascadeList = new [] { desc };
			Assert.IsNotNull (attrs.CascadeList, "CascadeList set");
			Assert.AreEqual (1, attrs.CascadeList.Length, "CascadeList.Length");

			attrs.CascadeList = null;
			Assert.IsNull (attrs.CascadeList, "CascadeList cleared");
		}

		[Test]
		public void UIFontAttributes_Traits_NullClears ()
		{
			var attrs = new UIFontAttributes ();
			var traits = new UIFontTraits ();
			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;

			attrs.Traits = traits;
			Assert.IsNotNull (attrs.Traits, "Traits set");
			Assert.AreEqual (UIFontDescriptorSymbolicTraits.Bold, attrs.Traits.SymbolicTrait, "Traits.SymbolicTrait");

			attrs.Traits = null;
			Assert.IsNull (attrs.Traits, "Traits cleared");
		}

		[Test]
		public void UIFontAttributes_FixedAdvance ()
		{
			var attrs = new UIFontAttributes ();

			attrs.FixedAdvance = 10.0f;
			Assert.IsTrue (attrs.FixedAdvance.HasValue, "FixedAdvance.HasValue");
			Assert.AreEqual (10.0f, attrs.FixedAdvance.Value, "FixedAdvance.Value");

			attrs.FixedAdvance = null;
			Assert.IsNull (attrs.FixedAdvance, "FixedAdvance cleared");
		}

		[Test]
		public void UIFontDescriptor_Properties_FromFont ()
		{
			var font = UIFont.BoldSystemFontOfSize (20);
			var descriptor = font.FontDescriptor;

			Assert.IsNotNull (descriptor.Family, "Family");
			Assert.IsNotNull (descriptor.Name, "Name");
			Assert.IsNotNull (descriptor.Face, "Face");
			Assert.IsTrue (descriptor.Size.HasValue, "Size.HasValue");
			Assert.AreEqual (20.0f, descriptor.Size.Value, "Size.Value");
			Assert.IsNotNull (descriptor.CascadeList, "CascadeList");
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

			Assert.IsNotNull (cascadeList, "CascadeList never null");
			Assert.IsNotNull (featureSettings, "FeatureSettings never null");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle1 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle1;
			Assert.IsNotNull (descriptor, "PreferredTitle1");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle2 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle2;
			Assert.IsNotNull (descriptor, "PreferredTitle2");
		}

		[Test]
		public void UIFontDescriptor_PreferredTitle3 ()
		{
			var descriptor = UIFontDescriptor.PreferredTitle3;
			Assert.IsNotNull (descriptor, "PreferredTitle3");
		}

		[Test]
		public void UIFontDescriptor_PreferredCallout ()
		{
			var descriptor = UIFontDescriptor.PreferredCallout;
			Assert.IsNotNull (descriptor, "PreferredCallout");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_Empty ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors ();
			Assert.IsNotNull (results, "empty mandatoryKeys");
		}

		[Test]
		public void UIFontDescriptor_GetMatchingFontDescriptors_WithKeys ()
		{
			var font = UIFont.SystemFontOfSize (12);
			var descriptor = font.FontDescriptor;

			var results = descriptor.GetMatchingFontDescriptors (UIFontDescriptorAttribute.Family);
			Assert.IsNotNull (results, "with Family key");
		}

		[Test]
		public void UIFontTraits_DefaultConstructor ()
		{
			var traits = new UIFontTraits ();
			Assert.IsNull (traits.SymbolicTrait, "SymbolicTrait");
			Assert.IsNull (traits.Weight, "Weight");
			Assert.IsNull (traits.Width, "Width");
			Assert.IsNull (traits.Slant, "Slant");
		}

		[Test]
		public void UIFontTraits_SymbolicTrait_SetAndClear ()
		{
			var traits = new UIFontTraits ();

			traits.SymbolicTrait = UIFontDescriptorSymbolicTraits.Bold;
			Assert.IsTrue (traits.SymbolicTrait.HasValue, "SymbolicTrait.HasValue after set");
			Assert.AreEqual (UIFontDescriptorSymbolicTraits.Bold, traits.SymbolicTrait.Value, "SymbolicTrait.Value");

			traits.SymbolicTrait = null;
			Assert.IsNull (traits.SymbolicTrait, "SymbolicTrait after null");
		}

		[Test]
		public void UIFontTraits_FromDescriptor ()
		{
			var font = UIFont.BoldSystemFontOfSize (12);
			var descriptor = font.FontDescriptor;
			var traits = descriptor.Traits;

			Assert.IsNotNull (traits, "Traits from bold font");
			Assert.IsTrue (traits.SymbolicTrait.HasValue, "SymbolicTrait.HasValue");
			Assert.IsTrue (traits.SymbolicTrait.Value.HasFlag (UIFontDescriptorSymbolicTraits.Bold), "Has Bold trait");
		}

		[Test]
		public void UIFontAttributes_FromDictionary ()
		{
			var font = UIFont.BoldSystemFontOfSize (14);
			var descriptor = font.FontDescriptor;
			var fontAttrs = descriptor.FontAttributes;

			Assert.IsTrue (fontAttrs.Size.HasValue, "Size.HasValue");
			Assert.AreEqual (14.0f, fontAttrs.Size.Value, "Size.Value");
		}

		[Test]
		public void UIFontAttributes_WeakFeatureSettings_NullClears ()
		{
			var attrs = new UIFontAttributes ();

			Assert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings initially null");

			attrs.WeakFeatureSettings = null;
			Assert.IsNull (attrs.WeakFeatureSettings, "WeakFeatureSettings after null set");
		}
	}
}
#endif
