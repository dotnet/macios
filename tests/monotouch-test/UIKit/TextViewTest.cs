// Copyright 2011-2013 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TextViewTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UITextView tv = new UITextView (frame)) {
				Assert.That (tv.Frame, Is.EqualTo (frame), "Frame");
			}
		}

		[Test]
		public void EmptySelection ()
		{
			using (UITextView tv = new UITextView ()) {
				Assert.That (tv.SelectedRange.Length, Is.EqualTo ((nint) 0), "SelectedRange");
#if XAMCORE_5_0
				Assert.That (tv.TypingAttributes, Is.Not.Null, "default");
				tv.TypingAttributes = new NSDictionary ();
				Assert.That (tv.TypingAttributes, Is.Not.Null, "assigned");
#else
				Assert.That (tv.TypingAttributes, Is.Null, "default");
				// ^ without a [PreSnippet] attribute this would crash like:
				// 7   monotouchtest                 	0x00006340 mono_sigill_signal_handler + 64
				// 8   WebKit                        	0x06b6afa5 -[WebView(WebPrivate) styleAtSelectionStart] + 53
				// 9   UIKit                         	0x028daa8a -[UIWebDocumentView typingAttributes] + 50
				// 10  UIKit                         	0x0285de57 -[UITextView typingAttributes] + 42
				tv.TypingAttributes = new NSDictionary ();
				// Assert.That (tv.TypingAttributes, Is.Not.Null, "assigned");
				// ^ this would still crash
#endif
			}
		}

#if !XAMCORE_6_0
		[Test]
		public void EmptySelection2 ()
		{
			using (UITextView tv = new UITextView ()) {
				Assert.That (tv.SelectedRange.Length, Is.EqualTo ((nint) 0), "SelectedRange");
				Assert.That (tv.TypingAttributes2, Is.Not.Null, "default");
				tv.TypingAttributes = new NSDictionary ();
				Assert.That (tv.TypingAttributes2, Is.Not.Null, "assigned");
			}
		}
#endif // !XAMCORE_6_0

		[Test]
		public void NonEmptySelection ()
		{
			using (UITextView tv = new UITextView ()) {
				tv.Text = "Bla bla bla";
				tv.SelectAll (tv);
				Assert.That (tv.SelectedRange.Length, Is.Not.EqualTo (0), "SelectedRange");
				Assert.That (tv.TypingAttributes, Is.Not.Null, "TypingAttributes");
			}
		}

		[Test]
		// if this fails ping lobrien (or doc team) since it means Apple changed the defaults we documented
		public void LayoutManager ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			using (UITextView tv = new UITextView ()) {
				var lm = tv.LayoutManager;
				Assert.That (lm.AllowsNonContiguousLayout, Is.True, "AllowsNonContiguousLayout");
				Assert.That (lm.ExtraLineFragmentRect.IsEmpty, Is.True.Or.False, "ExtraLineFragmentRect");
				Assert.That (lm.ExtraLineFragmentTextContainer, Is.Not.Null, "ExtraLineFragmentTextContainer");
				Assert.That (lm.ExtraLineFragmentUsedRect.IsEmpty, Is.True.Or.False, "ExtraLineFragmentUsedRect");
				Assert.That (lm.FirstUnlaidCharacterIndex, Is.EqualTo ((nuint) 0), "FirstUnlaidCharacterIndex");
				Assert.That (lm.FirstUnlaidGlyphIndex, Is.EqualTo ((nuint) 0), "FirstUnlaidGlyphIndex");
				Assert.That (lm.HasNonContiguousLayout, Is.False, "HasNonContiguousLayout");
#if !__MACCATALYST__
				Assert.That (lm.HyphenationFactor, Is.EqualTo ((nfloat) 0), "HyphenationFactor");
#endif
				Assert.That (lm.NumberOfGlyphs, Is.EqualTo ((nuint) 0), "NumberOfGlyphs");
				Assert.That (lm.ShowsControlCharacters, Is.False, "ShowsControlCharacters");
				Assert.That (lm.ShowsInvisibleCharacters, Is.False, "ShowsInvisibleCharacters");
				Assert.That (lm.TextStorage, Is.Not.Null, "TextStorage");
				Assert.That (lm.UsesFontLeading, Is.True, "UsesFontLeading");
			}
		}

		[Test]
		public void TextInputTraits ()
		{
			// UITextInputTraits members are not required but we marked them abstract
			// that's even more confusing since they all fails for respondToSelector tests but works in real life
			using (UITextView tv = new UITextView ()) {
				// this is just to show we can get and set those values (even if respondToSelector returns NO)
				tv.SetAutocapitalizationType (tv.GetAutocapitalizationType ());
				tv.SetAutocorrectionType (tv.GetAutocorrectionType ());
				tv.SetEnablesReturnKeyAutomatically (tv.GetEnablesReturnKeyAutomatically ());
				tv.SetKeyboardAppearance (tv.GetKeyboardAppearance ());
				tv.SetKeyboardType (tv.GetKeyboardType ());
				tv.SetReturnKeyType (tv.GetReturnKeyType ());
				tv.SetSecureTextEntry (tv.GetSecureTextEntry ());
				tv.SetSpellCheckingType (tv.GetSpellCheckingType ());
			}
		}
	}
}

#endif // !MONOMAC
