// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !__TVOS__

using System.Diagnostics.CodeAnalysis;
using CoreFoundation;

namespace CoreGraphics {

	/// <summary>Represents an element in a PDF document's logical structure tree.</summary>
	[SupportedOSPlatform ("ios27.0")]
	[SupportedOSPlatform ("maccatalyst27.0")]
	[SupportedOSPlatform ("macos27.0")]
	[UnsupportedOSPlatform ("tvos")]
	public class CGPDFStructureElement : NativeObject {

		[DynamicDependency (DynamicallyAccessedMemberTypes.NonPublicConstructors, typeof (CGPDFStructureElement))]
		static CGPDFStructureElement ()
		{
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPDFStructureElementCreate (CGPdfTagType tagType);

		/// <summary>Creates a structure element with the specified PDF structure tag.</summary>
		/// <param name="tagType">The structure tag for the new element.</param>
		/// <remarks>Use a structural tag such as <see cref="CGPdfTagType.Document" /> or <see cref="CGPdfTagType.Paragraph" />. <see cref="CGPdfTagType.Artifact" /> is non-structural and is not valid for structure elements.</remarks>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public CGPDFStructureElement (CGPdfTagType tagType)
			: base (CGPDFStructureElementCreate (tagType), owns: true)
		{
		}

		internal CGPDFStructureElement (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPDFStructureElementRetain (IntPtr structureElement);

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementRelease (IntPtr structureElement);

		/// <inheritdoc />
		protected internal override void Retain ()
		{
			CGPDFStructureElementRetain (GetCheckedHandle ());
		}

		/// <inheritdoc />
		protected internal override void Release ()
		{
			CGPDFStructureElementRelease (GetCheckedHandle ());
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementSetTitle (IntPtr structureElement, IntPtr title);

		/// <summary>Sets the title of the structure element.</summary>
		/// <param name="title">The title to set.</param>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetTitle (string title)
		{
			if (title is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (title));

			var titleHandle = CFString.CreateNative (title);
			try {
				CGPDFStructureElementSetTitle (GetCheckedHandle (), titleHandle);
			} finally {
				CFString.ReleaseNative (titleHandle);
			}
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementSetLanguageIdentifier (IntPtr structureElement, IntPtr languageIdentifier);

		/// <summary>Sets the language identifier of the structure element.</summary>
		/// <param name="languageIdentifier">The language identifier to set.</param>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetLanguageIdentifier (string languageIdentifier)
		{
			if (languageIdentifier is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (languageIdentifier));

			var languageIdentifierHandle = CFString.CreateNative (languageIdentifier);
			try {
				CGPDFStructureElementSetLanguageIdentifier (GetCheckedHandle (), languageIdentifierHandle);
			} finally {
				CFString.ReleaseNative (languageIdentifierHandle);
			}
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementSetAlternativeText (IntPtr structureElement, IntPtr alternativeText);

		/// <summary>Sets alternative text for the structure element.</summary>
		/// <param name="alternativeText">The alternative text to set.</param>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetAlternativeText (string alternativeText)
		{
			if (alternativeText is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (alternativeText));

			var alternativeTextHandle = CFString.CreateNative (alternativeText);
			try {
				CGPDFStructureElementSetAlternativeText (GetCheckedHandle (), alternativeTextHandle);
			} finally {
				CFString.ReleaseNative (alternativeTextHandle);
			}
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementSetExpansionText (IntPtr structureElement, IntPtr expansionText);

		/// <summary>Sets the expanded form of an abbreviation in the structure element.</summary>
		/// <param name="expansionText">The expansion text to set.</param>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetExpansionText (string expansionText)
		{
			if (expansionText is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (expansionText));

			var expansionTextHandle = CFString.CreateNative (expansionText);
			try {
				CGPDFStructureElementSetExpansionText (GetCheckedHandle (), expansionTextHandle);
			} finally {
				CFString.ReleaseNative (expansionTextHandle);
			}
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFStructureElementSetActualText (IntPtr structureElement, IntPtr actualText);

		/// <summary>Sets replacement text for the content associated with the structure element.</summary>
		/// <param name="actualText">The replacement text to set.</param>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetActualText (string actualText)
		{
			if (actualText is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (actualText));

			var actualTextHandle = CFString.CreateNative (actualText);
			try {
				CGPDFStructureElementSetActualText (GetCheckedHandle (), actualTextHandle);
			} finally {
				CFString.ReleaseNative (actualTextHandle);
			}
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern OSStatus CGPDFStructureElementAddStructureElement (IntPtr structureElement, IntPtr childStructureElement);

		/// <summary>Adds a child structure element.</summary>
		/// <param name="child">The child structure element to add.</param>
		/// <returns>An <c>OSStatus</c> value, where zero indicates success.</returns>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public OSStatus AddChild (CGPDFStructureElement child)
		{
			var status = CGPDFStructureElementAddStructureElement (GetCheckedHandle (), child.GetNonNullHandle (nameof (child)));
			GC.KeepAlive (child);
			return status;
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern OSStatus CGPDFStructureElementAddMarkedContentItem (IntPtr structureElement, IntPtr markedContentItem);

		/// <summary>Adds marked content to the structure element.</summary>
		/// <param name="markedContentItem">The marked-content item to add.</param>
		/// <returns>An <c>OSStatus</c> value, where zero indicates success.</returns>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		public OSStatus AddMarkedContentItem (CGPDFMarkedContentItem markedContentItem)
		{
			var status = CGPDFStructureElementAddMarkedContentItem (GetCheckedHandle (), markedContentItem.GetNonNullHandle (nameof (markedContentItem)));
			GC.KeepAlive (markedContentItem);
			return status;
		}
	}
}

#endif // !__TVOS__
