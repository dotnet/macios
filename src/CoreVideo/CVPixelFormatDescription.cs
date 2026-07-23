// 
// CVPixelFormatDescription.cs: Implements the managed CVPixelFormatDescription
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
// Copyright 2015 Xamarin Inc.
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
using System.ComponentModel;
using CoreFoundation;

#if XAMCORE_5_0
using CVFillExtendedPixelsCallBackDataStruct = CoreVideo.CVFillExtendedPixelsCallBackData;
#endif

#nullable enable

namespace CoreVideo {
	/// <summary>A class that supports the definition of customer pixel formats.</summary>
	///     <remarks>To be added.</remarks>
	public partial class CVPixelFormatDescription {
#if !COREBUILD
#if !XAMCORE_5_0
		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.Name" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.Name' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString NameKey = CVPixelFormatKeys.Name;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.Constant" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.Constant' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ConstantKey = CVPixelFormatKeys.Constant;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.CodecType" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.CodecType' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString CodecTypeKey = CVPixelFormatKeys.CodecType;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.FourCC" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.FourCCKey' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString FourCCKey = CVPixelFormatKeys.FourCC;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.Planes" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.Planes' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString PlanesKey = CVPixelFormatKeys.Planes;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BlockWidth" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BlockWidth' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BlockWidthKey = CVPixelFormatKeys.BlockWidth;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BlockHeight" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BlockHeight' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BlockHeightKey = CVPixelFormatKeys.BlockHeight;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BitsPerBlock" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BitsPerBlock' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BitsPerBlockKey = CVPixelFormatKeys.BitsPerBlock;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BlockHorizontalAlignment" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BlockHorizontalAlignment' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BlockHorizontalAlignmentKey = CVPixelFormatKeys.BlockHorizontalAlignment;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BlockVerticalAlignment" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BlockVerticalAlignment' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BlockVerticalAlignmentKey = CVPixelFormatKeys.BlockVerticalAlignment;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.BlackBlock" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.BlackBlock' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString BlackBlockKey = CVPixelFormatKeys.BlackBlock;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.HorizontalSubsampling" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.HorizontalSubsampling' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString HorizontalSubsamplingKey = CVPixelFormatKeys.HorizontalSubsampling;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.VerticalSubsampling" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.VerticalSubsampling' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString VerticalSubsamplingKey = CVPixelFormatKeys.VerticalSubsampling;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.OpenGLFormat" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.OpenGLFormat' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString OpenGLFormatKey = CVPixelFormatKeys.OpenGLFormat;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.OpenGLType" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.OpenGLType' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString OpenGLTypeKey = CVPixelFormatKeys.OpenGLType;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.OpenGLInternalFormat" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.OpenGLInternalFormat' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString OpenGLInternalFormatKey = CVPixelFormatKeys.OpenGLInternalFormat;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.CGBitmapInfo" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.CGBitmapInfo' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString CGBitmapInfoKey = CVPixelFormatKeys.CGBitmapInfo;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.QDCompatibility" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.QDCompatibility' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString QDCompatibilityKey = CVPixelFormatKeys.QDCompatibility;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.CGBitmapContextCompatibility" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.CGBitmapContextCompatibility' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString CGBitmapContextCompatibilityKey = CVPixelFormatKeys.CGBitmapContextCompatibility;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.CGImageCompatibility" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.CGImageCompatibility' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString CGImageCompatibilityKey = CVPixelFormatKeys.CGImageCompatibility;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.OpenGLCompatibility" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.OpenGLCompatibility' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString OpenGLCompatibilityKey = CVPixelFormatKeys.OpenGLCompatibility;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.FillExtendedPixelsCallback" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.FillExtendedPixelsCallback' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString FillExtendedPixelsCallbackKey = CVPixelFormatKeys.FillExtendedPixelsCallback;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.ContainsRgb" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.ContainsRgb' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ContainsRgb = CVPixelFormatKeys.ContainsRgb;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.ContainsYCbCr" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.ContainsYCbCr' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ContainsYCbCr = CVPixelFormatKeys.ContainsYCbCr;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.ComponentRange" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.ComponentRange' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ComponentRangeKey = CVPixelFormatKeys.ComponentRange;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatComponentRangeKeys.FullRange" />.</summary>
		[Obsolete ("Use 'CVPixelFormatComponentRangeKeys.FullRange' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ComponentRangeFullRangeKey = CVPixelFormatComponentRangeKeys.FullRange;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatComponentRangeKeys.VideoRange" />.</summary>
		[Obsolete ("Use 'CVPixelFormatComponentRangeKeys.VideoRange' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ComponentRangeVideoRangeKey = CVPixelFormatComponentRangeKeys.VideoRange;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatComponentRangeKeys.WideRange" />.</summary>
		[Obsolete ("Use 'CVPixelFormatComponentRangeKeys.WideRange' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static readonly NSString ComponentRangeWideRangeKey = CVPixelFormatComponentRangeKeys.WideRange;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.ContainsGrayscale" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.ContainsGrayscale' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static readonly NSString ContainsGrayscaleKey = CVPixelFormatKeys.ContainsGrayscale;

		/// <summary>Obsolete alias for <see cref="CVPixelFormatKeys.ContainsSenselArray" />.</summary>
		[Obsolete ("Use 'CVPixelFormatKeys.ContainsSenselArray' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'CVPixelFormatKeys.ContainsSenselArray.get' is only supported on: 'ios' 16.0 and later, 'maccatalyst' 16.0 and later, 'macOS/OSX' 13.0 and later, 'tvos' 16.0 and later.
		public static readonly NSString ContainsSenselArray = CVPixelFormatKeys.ContainsSenselArray;
#pragma warning restore CA1416
#endif // !XAMCORE_5_0

		// note: bad documentation, ref: https://bugzilla.xamarin.com/show_bug.cgi?id=13917
		[DllImport (Constants.CoreVideoLibrary)]
		extern static/* CFArrayRef __nullable */ IntPtr CVPixelFormatDescriptionArrayCreateWithAllPixelFormatTypes (
			/* CFAllocatorRef __nullable */ IntPtr allocator);

		/// <summary>Get all the known pixel format types.</summary>
		public static NSNumber [] AllTypes {
			get {
				return NSArray.NonNullArrayFromHandleDropNullElements<NSNumber> (CVPixelFormatDescriptionArrayCreateWithAllPixelFormatTypes (IntPtr.Zero), releaseHandle: true);
			}
		}

		/// <summary>Get all the known pixel format types.</summary>
		public static CVPixelFormatType [] AllPixelFormatTypes {
			get {
				var all = AllTypes;
				var rv = new CVPixelFormatType [all.Length];
				for (var i = 0; i < rv.Length; i++)
					rv [i] = (CVPixelFormatType) all [i].Int32Value;
				return rv;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CFDictionaryRef __nullable */ IntPtr CVPixelFormatDescriptionCreateWithPixelFormatType (
			/* CFAllocatorRef __nullable */ IntPtr allocator, int /* OSType = int32_t */ pixelFormat);

		/// <summary>Create a description of the specified pixel format.</summary>
		/// <param name="pixelFormat">The pixel format to create a description of.</param>
		public static NSDictionary? Create (CVPixelFormatType pixelFormat)
		{
			return Runtime.GetNSObject<NSDictionary> (CVPixelFormatDescriptionCreateWithPixelFormatType (IntPtr.Zero, (int) pixelFormat), true);
		}

		/// <summary>Create a description of the specified pixel format.</summary>
		/// <param name="pixelFormat">The pixel format to create a description of.</param>
		public static CVPixelFormatDescription? CreatePixelFormat (CVPixelFormatType pixelFormat)
		{
			var dict = Create (pixelFormat);
			if (dict is null)
				return null;
			return new CVPixelFormatDescription (dict);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelFormatDescriptionRegisterDescriptionWithPixelFormatType (
			/* CFDictionaryRef __nonnull */ IntPtr description, int /* OSType = int32_t */ pixelFormat);

		/// <summary>Register a new pixel format with CoreVideo.</summary>
		/// <param name="description">The pixel format description for the pixel format to register.</param>
		/// <param name="pixelFormat">The pixel format to register.</param>
		public static void Register (NSDictionary description, CVPixelFormatType pixelFormat)
		{
			if (description is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (description));

			CVPixelFormatDescriptionRegisterDescriptionWithPixelFormatType (description.Handle, (int) pixelFormat);
			GC.KeepAlive (description);
		}

		/// <summary>Register a new pixel format with CoreVideo.</summary>
		/// <param name="description">The pixel format description for the pixel format to register.</param>
		/// <param name="pixelFormat">The pixel format to register.</param>
		public static void Register (CVPixelFormatDescription description, CVPixelFormatType pixelFormat)
		{
			Register (description?.Dictionary!, pixelFormat);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreVideoLibrary)]
		extern static byte CVIsCompressedPixelFormatAvailable (int /* OSType = int32_t */ pixelFormat);

		/// <summary>Check if the specified pixel format is supported on this platform.</summary>
		/// <param name="pixelFormat">The pixel format to check.</param>
		/// <returns>Whether the specified pixel format is supported or not.</returns>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool IsPixelFormatAvailable (CVPixelFormatType pixelFormat)
		{
			return CVIsCompressedPixelFormatAvailable ((int) pixelFormat) != 0;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public CVFillExtendedPixelsCallBackDataStruct? FillExtendedPixelsCallbackStruct {
			get {
				var data = FillExtendedPixelsCallback;
				if (data is null)
					return null;
				var bytes = data.ToArray ();
				unsafe {
					if (bytes.Length < sizeof (CVFillExtendedPixelsCallBackDataStruct))
						throw new InvalidOperationException ($"The size of the callback data structure is smaller than expected (got {bytes.Length} bytes, expected at least {sizeof (CVFillExtendedPixelsCallBackDataStruct)} bytes)");
					fixed (byte* ptr = bytes)
						return Marshal.PtrToStructure<CVFillExtendedPixelsCallBackDataStruct> ((IntPtr) ptr);
				}
			}
			set {
				if (value is null) {
					FillExtendedPixelsCallback = null;
					return;
				}
				NSData data;
				CVFillExtendedPixelsCallBackDataStruct v = value.Value;
				unsafe {
					data = NSData.FromBytes ((IntPtr) (&v), (nuint) sizeof (CVFillExtendedPixelsCallBackDataStruct));
				}
				FillExtendedPixelsCallback = data;
			}
		}

#if !XAMCORE_5_0
		[Obsolete ("Use 'ComponentRangeValue' instead, the property type is incorrect for this property.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public CVPixelFormatComponentRange? ComponentRange {
			get => null;
			set { }
		}
#endif // !XAMCORE_5_0

#endif // !COREBUILD
	}
}
