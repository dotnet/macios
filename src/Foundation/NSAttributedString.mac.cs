//
// NSAttributedString.cs: extensions for NSAttributedString
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#nullable enable

#if MONOMAC

using System;

using AppKit;
using ObjCRuntime;
using WebKit;
//using CoreText;

namespace Foundation {
	public partial class NSAttributedString {
		public NSAttributedString (string str,
			NSFont? font = null,
			NSColor? foregroundColor = null,
			NSColor? backgroundColor = null,
			NSColor? strokeColor = null,
			NSColor? underlineColor = null,
			NSColor? strikethroughColor = null,
			NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
			NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None,
			NSParagraphStyle? paragraphStyle = null,
			float strokeWidth = 0,
			NSShadow? shadow = null,
			NSUrl? link = null,
			bool superscript = false,
			NSTextAttachment? attachment = null,
			NSLigatureType ligature = NSLigatureType.Default,
			float baselineOffset = 0,
			float kerningAdjustment = 0,
			float obliqueness = 0,
			float expansion = 0,
			NSCursor? cursor = null,
			string? toolTip = null,
			int characterShape = 0,
			NSGlyphInfo? glyphInfo = null,
			NSArray? writingDirection = null,
			bool markedClauseSegment = false,
			NSTextLayoutOrientation verticalGlyphForm = NSTextLayoutOrientation.Horizontal,
			NSTextAlternatives? textAlternatives = null,
			NSSpellingState spellingState = NSSpellingState.None) : this (str, NSStringAttributes.ToDictionary (
				font: font,
				foregroundColor: foregroundColor,
				backgroundColor: backgroundColor,
				strokeColor: strokeColor,
				underlineColor: underlineColor,
				strikethroughColor: strikethroughColor,
				underlineStyle: underlineStyle,
				strikethroughStyle: strikethroughStyle,
				paragraphStyle: paragraphStyle,
				strokeWidth: strokeWidth,
				shadow: shadow,
				link: link,
				superscript: superscript,
				attachment: attachment,
				ligature: ligature,
				baselineOffset: baselineOffset,
				kerningAdjustment: kerningAdjustment,
				obliqueness: obliqueness,
				expansion: expansion,
				cursor: cursor,
				toolTip: toolTip,
				characterShape: characterShape,
				glyphInfo: glyphInfo,
				writingDirection: writingDirection,
				markedClauseSegment: markedClauseSegment,
				verticalGlyphForm: verticalGlyphForm,
				textAlternatives: textAlternatives,
				spellingState: spellingState
			))
		{
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="rtfData" /> data as RTF.</summary>
		/// <param name="rtfData">The data to parse, in RTF format.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an RTF document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithRTF (NSData rtfData, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithRtf (rtfData, out resultDocumentAttributes), "initWithRTF:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="rtfdData" /> data as RTFD.</summary>
		/// <param name="rtfdData">The data to parse, in RTFD format.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an RTFD document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithRTFD (NSData rtfdData, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithRtfd (rtfdData, out resultDocumentAttributes), "initWithRTFD:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="htmlData" /> data as HTML.</summary>
		/// <param name="htmlData">The data to parse, in HTML format.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an HTML document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithHTML (NSData htmlData, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithHTML (htmlData, out resultDocumentAttributes), "initWithHTML:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="htmlData" /> data as HTML.</summary>
		/// <param name="htmlData">The data to parse, in HTML format.</param>
		/// <param name="baseUrl">The base URL for any links in the HTML content.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an HTML document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithHTML (NSData htmlData, NSUrl baseUrl, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithHTML (htmlData, baseUrl, out resultDocumentAttributes), "initWithHTML:baseURL:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="htmlData" /> data as HTML.</summary>
		/// <param name="htmlData">The data to parse, in HTML format.</param>
		/// <param name="options">Any additional options when loading the HTML content.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an HTML document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithHTML (NSData htmlData, NSDictionary options, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithHTML (htmlData, options, out resultDocumentAttributes), "initWithHTML:options:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="htmlData" /> data as HTML.</summary>
		/// <param name="htmlData">The data to parse, in HTML format.</param>
		/// <param name="options">Any additional options when loading the HTML content.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from an HTML document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? CreateWithHTML (NSData htmlData, NSAttributedStringDocumentAttributes options, out NSDictionary resultDocumentAttributes)
		{
			return CreateWithHTML (htmlData, options.GetDictionary (), out resultDocumentAttributes);
		}

#if XAMCORE_5_0
		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="wordDocFormat" /> data as a Microsoft Word document.</summary>
		/// <param name="wordDocFormat">The data to parse, in Microsoft Word format.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from a Microsoft Word document</returns>
#else
		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="wordDocFormat" /> data as a Microsoft Word document.</summary>
		/// <param name="wordDocFormat">The data to parse, in Microsoft Word format.</param>
		/// <param name="docAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from a Microsoft Word document</returns>
#endif
		[SupportedOSPlatform ("macos")]
#if XAMCORE_5_0
		public static NSAttributedString? CreateWithDocFormat (NSData wordDocFormat, out NSDictionary resultDocumentAttributes)
#else
		public static NSAttributedString? CreateWithDocFormat (NSData wordDocFormat, out NSDictionary docAttributes)
#endif
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
#if XAMCORE_5_0
			rv.InitializeHandle (rv._InitWithDocFormat (wordDocFormat, out resultDocumentAttributes), "initWithDocFormat:documentAttributes:", false);
#else
			rv.InitializeHandle (rv._InitWithDocFormat (wordDocFormat, out docAttributes), "initWithDocFormat:documentAttributes:", false);
#endif
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create an <see cref="NSAttributedString" /> by parsing the <paramref name="wrapper" /> with RTFD data.</summary>
		/// <param name="wrapper">The data to parse, in RTFD format.</param>
		/// <param name="resultDocumentAttributes">Upon return, any document-specific attributes.</param>
		/// <returns>A newly created <see cref="NSAttributedString" />, created from a Microsoft Word document</returns>
		[SupportedOSPlatform ("macos")]
		public static NSAttributedString? Create (NSFileWrapper wrapper, out NSDictionary resultDocumentAttributes)
		{
			var rv = new NSAttributedString (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithRTFDFileWrapper (wrapper, out resultDocumentAttributes), "initWithRTFDFileWrapper:documentAttributes:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <param name="location">To be added.</param>
		/// <param name="effectiveRange">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public NSStringAttributes? GetAppKitAttributes (nint location, out NSRange effectiveRange)
		{
			var attr = GetAttributes (location, out effectiveRange);
			return attr is null ? null : new NSStringAttributes (attr);
		}

		/// <param name="location">To be added.</param>
		/// <param name="longestEffectiveRange">To be added.</param>
		/// <param name="rangeLimit">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public NSStringAttributes? GetAppKitAttributes (nint location, out NSRange longestEffectiveRange, NSRange rangeLimit)
		{
			var attr = GetAttributes (location, out longestEffectiveRange, rangeLimit);
			return attr is null ? null : new NSStringAttributes (attr);
		}
	}
}

#endif // MONOMAC
