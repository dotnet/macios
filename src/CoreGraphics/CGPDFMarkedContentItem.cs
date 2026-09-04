// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !__TVOS__

using System.Diagnostics.CodeAnalysis;
using CoreFoundation;

namespace CoreGraphics {

	/// <summary>Represents the association between drawn PDF content and its place in a PDF structure tree.</summary>
	[SupportedOSPlatform ("ios27.0")]
	[SupportedOSPlatform ("maccatalyst27.0")]
	[SupportedOSPlatform ("macos27.0")]
	[UnsupportedOSPlatform ("tvos")]
	public class CGPDFMarkedContentItem : NativeObject {

		[DynamicDependency (DynamicallyAccessedMemberTypes.NonPublicConstructors, typeof (CGPDFMarkedContentItem))]
		static CGPDFMarkedContentItem ()
		{
		}

		internal CGPDFMarkedContentItem (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGPDFMarkedContentItemRetain (IntPtr markedContentItem);

		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFMarkedContentItemRelease (IntPtr markedContentItem);

		/// <inheritdoc />
		protected internal override void Retain ()
		{
			CGPDFMarkedContentItemRetain (GetCheckedHandle ());
		}

		/// <inheritdoc />
		protected internal override void Release ()
		{
			CGPDFMarkedContentItemRelease (GetCheckedHandle ());
		}
	}
}

#endif // !__TVOS__
