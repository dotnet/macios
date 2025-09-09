// 
// CTFont.cs: Implements the managed CTFont
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using CoreFoundation;

namespace CoreText {
	/// <summary>Represents a CoreText Font.</summary>
	///     <remarks>
	///       <para>
	/// 	CoreText does not synthesize font styles (italic and bold).
	/// 	This means that if you pick a font that has neither a Bolded
	/// 	or Italicized versions available, CoreText will not create a
	/// 	dynamic font that is merely a slanted version of the font for
	/// 	italic, or a boldened version from the original font.  In
	/// 	those cases, if you want to synthesize the font, you could
	/// 	apply a Matrix transformation to slant the font (it will still
	/// 	be wrong, but will look slanted).  For bolding, you could
	/// 	stroke the font twice, or manually extend the glyph path.
	///
	///       </para>
	///     </remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/SimpleTextInput/">SimpleTextInput</related>
	public partial class CTFont : NativeObject {
	}
	/// <summary>Font Descriptors contain a description of font features that can identify a font.</summary>
	///     <remarks>
	///       <para>
	/// 	Font Descriptors contain a description of font features and can
	/// 	completely identify a font.  Sometimes the description is not
	/// 	complete enough, and the system will pick a font that matches
	/// 	the specified parameters.
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// var attributes = new CTFontDescriptorAttributes () {
	/// 	FamilyName = "Times New Roman",
	/// 	Size = 18
	/// };
	///
	/// var fontDescriptor = new CTFontDescriptor (attributes);
	/// ]]></code>
	///       </example>
	///     </remarks>
	public partial class CTFontDescriptor : NativeObject {
	}
}
