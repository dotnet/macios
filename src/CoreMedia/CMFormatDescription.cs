// 
// CMFormatDescription.cs: Implements the managed CMFormatDescription
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//   Frank Krueger
//   Mono Team
//   Marek Safar (marek.safar@gmail.com)	
//     
// Copyright 2010 Novell, Inc
// Copyright 2012-2014 Xamarin Inc
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using Foundation;
using CoreFoundation;
using CoreGraphics;
using ObjCRuntime;
using CoreVideo;
using AudioToolbox;

namespace CoreMedia {

	/// <summary>Describes media data for audio, video, text and time codes </summary>
	///     <remarks>Some properties apply to all media types, while some others only apply to specific media types.   They are prefixed with Audio or Video in those cases.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMFormatDescription : NativeObject {
		[Preserve (Conditional = true)]
		internal CMFormatDescription (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFDictionaryRef */ IntPtr CMFormatDescriptionGetExtensions (/* CMFormatDescriptionRef */ IntPtr desc);

#if !COREBUILD

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary? GetExtensions ()
		{
			var cfDictRef = CMFormatDescriptionGetExtensions (Handle);
			return Runtime.GetNSObject<NSDictionary> (cfDictRef);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFPropertyListRef */ IntPtr CMFormatDescriptionGetExtension (/* CMFormatDescriptionRef */ IntPtr desc, /* CFStringRef */ IntPtr extensionkey);

		/// <param name="extensionKey">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSObject? GetExtension (string extensionKey)
		{
			var extensionKeyHandle = CFString.CreateNative (extensionKey);
			try {
				var r = CMFormatDescriptionGetExtension (Handle, extensionKeyHandle);
				return Runtime.GetNSObject<NSObject> (r);
			} finally {
				CFString.ReleaseNative (extensionKeyHandle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* FourCharCode */ uint CMFormatDescriptionGetMediaSubType (/* CMFormatDescriptionRef */ IntPtr desc);

		/// <summary>Returns the media subtype, ideally you should use the strongly typed versions instead.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Instead of using this uint value, you can use the specific
		/// 	  strongly-typed version: AudioFormatType, SubtitleFormatType,
		/// 	  ClosedCaptionFormatType, MuxedStreamType, VideoCodecType,
		/// 	  MetadataFormatType or TimeCodeFormatType.
		/// 	</remarks>
		public uint MediaSubType {
			get {
				return CMFormatDescriptionGetMediaSubType (Handle);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFormatType AudioFormatType {
			get {
				return MediaType == CMMediaType.Audio ? (AudioFormatType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMSubtitleFormatType SubtitleFormatType {
			get {
				return MediaType == CMMediaType.Subtitle ? (CMSubtitleFormatType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMClosedCaptionFormatType ClosedCaptionFormatType {
			get {
				return MediaType == CMMediaType.ClosedCaption ? (CMClosedCaptionFormatType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMMuxedStreamType MuxedStreamType {
			get {
				return MediaType == CMMediaType.Muxed ? (CMMuxedStreamType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMVideoCodecType VideoCodecType {
			get {
				return MediaType == CMMediaType.Video ? (CMVideoCodecType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMMetadataFormatType MetadataFormatType {
			get {
				return MediaType == CMMediaType.Metadata ? (CMMetadataFormatType) MediaSubType : 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTimeCodeFormatType TimeCodeFormatType {
			get {
				return MediaType == CMMediaType.TimeCode ? (CMTimeCodeFormatType) MediaSubType : 0;
			}
		}

		/// <summary>Gets the <see cref="MediaSubType" /> property as a <see cref="CMTaggedBufferGroupFormatType" />.</summary>
		/// <returns>The <see cref="MediaSubType" /> property as a <see cref="CMTaggedBufferGroupFormatType" />, if this format descripton's <see cref="MediaType" /> is <see cref="CMMediaType.TaggedBufferGroup" />, otherwise 0.</returns>
		/// <remarks>Only applicable if this format descripton's <see cref="MediaType" /> is <see cref="CMMediaType.TaggedBufferGroup" />, otherwise 0 is returned.</remarks>
		public CMTaggedBufferGroupFormatType TaggedBufferGroupFormatType {
			get {
				return MediaType == CMMediaType.TaggedBufferGroup ? (CMTaggedBufferGroupFormatType) MediaSubType : 0;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMMediaType CMFormatDescriptionGetMediaType (/* CMFormatDescriptionRef */ IntPtr desc);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMMediaType MediaType {
			get {
				return CMFormatDescriptionGetMediaType (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFTypeID */ nint CMFormatDescriptionGetTypeID ();

		/// <summary>Type identifier for the CoreMedia.CMFormatDescription type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isCMFormatDescription = (CFType.GetTypeID (foo.Handle) == CMFormatDescription.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		public static nint GetTypeID ()
		{
			return CMFormatDescriptionGetTypeID ();
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMFormatDescriptionError CMFormatDescriptionCreate (/* CFAllocatorRef */ IntPtr allocator, CMMediaType mediaType, /* FourCharCode */ uint mediaSubtype, /* CFDictionaryRef */ IntPtr extensions, /* CMFormatDescriptionRef* */ IntPtr* descOut);

		/// <param name="mediaType">media type that we want to create a wrapper for</param>
		///         <param name="mediaSubtype">The media subtype</param>
		///         <param name="error">Errors, if any, are returned here.</param>
		///         <summary>Creates a CMFormatDescription (or a subclass of it) based on a native handle and to have it wrapped in a specific type.</summary>
		///         <returns>
		///           <para>The return can be either a CMFormatDescription a <see cref="CoreMedia.CMAudioFormatDescription" /> or a <see cref="CoreMedia.CMVideoFormatDescription" /> depending on the mediaType parameter that you passed.</para>
		///           <para>
		///           </para>
		///           <para />
		///         </returns>
		///         <remarks>
		///           <para>In general, the <see cref="CoreMedia.CMFormatDescription.Create(System.IntPtr)" /> is a better option as it probes for the underlying type and creates the correct subclass of <see cref="CoreMedia.CMFormatDescription" /></para>
		///           <para>
		///           </para>
		///         </remarks>
		public static CMFormatDescription? Create (CMMediaType mediaType, uint mediaSubtype, out CMFormatDescriptionError error)
		{
			IntPtr handle;
			unsafe {
				error = CMFormatDescriptionCreate (IntPtr.Zero, mediaType, mediaSubtype, IntPtr.Zero, &handle);
			}
			if (error != CMFormatDescriptionError.None)
				return null;

			return Create (mediaType, handle, true);
		}

		/// <param name="handle">The native handle to a CMFormatDescription or a subclass of it.</param>
		///         <param name="owns">True if the handle is already owned by maanged code, false otherwise (and in this case, the code will manually call retain on the object).</param>
		///         <summary>Creates a CMFormatDescription (or a subclass of it) based on a native handle.</summary>
		///         <returns>
		///           <para>The return can be either a CMFormatDescription a <see cref="CoreMedia.CMAudioFormatDescription" /> or a <see cref="CoreMedia.CMVideoFormatDescription" />, you can use the C# <see langword="is" /> expression to find out which subclass to cast the result to if you need access to the audio or video specific elements.</para>
		///           <para>
		///           </para>
		///         </returns>
		///         <remarks>This is mostly used to support the binding infrastructure.</remarks>
		public static CMFormatDescription? Create (IntPtr handle, bool owns)
		{
			return Create (CMFormatDescriptionGetMediaType (handle), handle, owns);
		}

		/// <param name="handle">The native handle to a CMFormatDescription or a subclass of it.</param>
		///         <summary>Creates a CMFormatDescription (or a subclass of it) based on a native handle.</summary>
		///         <returns>The return can be either a CMFormatDescription a <see cref="CoreMedia.CMAudioFormatDescription" /> or a <see cref="CoreMedia.CMVideoFormatDescription" />, you can use the C# <see langword="is" /> expression to find out which subclass to cast the result to if you need access to the audio or video specific elements.</returns>
		///         <remarks>This is the recommended way of surfacing an unmanaged format description, as this will create the proper wrapper with a strong type for the audio or video versions of it.</remarks>
		public static CMFormatDescription? Create (IntPtr handle)
		{
			return Create (handle, false);
		}

		static CMFormatDescription? Create (CMMediaType type, IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				return null;

			switch (type) {
			case CMMediaType.Video:
				return new CMVideoFormatDescription (handle, owns);
			case CMMediaType.Audio:
				return new CMAudioFormatDescription (handle, owns);
			default:
				return new CMFormatDescription (handle, owns);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* AudioStreamBasicDescription */ IntPtr CMAudioFormatDescriptionGetStreamBasicDescription (/* CMAudioFormatDescriptionRef */ IntPtr desc);

		/// <summary>Audio-media only: Returns the AudioStreamBasicDescritpion object for the audio.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioStreamBasicDescription? AudioStreamBasicDescription {
			get {
				var ret = CMAudioFormatDescriptionGetStreamBasicDescription (Handle);
				if (ret != IntPtr.Zero) {
					unsafe {
						return *((AudioStreamBasicDescription*) ret);
					}
				}
				return null;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* AudioChannelLayout* */ IntPtr CMAudioFormatDescriptionGetChannelLayout (/* CMAudioFormatDescriptionRef */ IntPtr desc, /* size_t* */ nint* size);

		/// <summary>Audio-media only: describe the channel layout.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioChannelLayout? AudioChannelLayout {
			get {
				nint size;
				IntPtr res;
				unsafe {
					res = CMAudioFormatDescriptionGetChannelLayout (Handle, &size);
				}
				if (res == IntPtr.Zero || size == 0)
					return null;
				return AudioChannelLayout.FromHandle (res);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* AudioFormatListItem* */ IntPtr CMAudioFormatDescriptionGetFormatList (/* CMAudioFormatDescriptionRef */ IntPtr desc, /* size_t* */ nint* size);

		/// <summary>Audio-media only: the supported audio formats, sorted from most rich to less.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AudioFormat []? AudioFormats {
			get {
				unsafe {
					nint size;
					IntPtr v;
					unsafe {
						v = CMAudioFormatDescriptionGetFormatList (Handle, &size);
					}
					if (v == IntPtr.Zero)
						return null;
					var items = size / sizeof (AudioFormat);
					var ret = new AudioFormat [items];
					var ptr = (AudioFormat*) v;
					for (int i = 0; i < items; i++)
						ret [i] = ptr [i];
					return ret;
				}
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* const void* */ IntPtr CMAudioFormatDescriptionGetMagicCookie (/* CMAudioFormatDescriptionRef */ IntPtr desc, /* size_t* */ nint* size);

		/// <summary>Audio-media only: magic cookie that might need to be passed to some backends.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public byte []? AudioMagicCookie {
			get {
				nint size;
				IntPtr h;
				unsafe {
					h = CMAudioFormatDescriptionGetMagicCookie (Handle, &size);
				}
				if (h == IntPtr.Zero)
					return null;

				var result = new byte [size];
				Marshal.Copy (h, result, 0, result.Length);
				return result;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* AudioFormatListItem* */ IntPtr CMAudioFormatDescriptionGetMostCompatibleFormat (/* CMAudioFormatDescriptionRef */ IntPtr desc);

		/// <summary>Audio-media only: returns the most compaible audio format.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFormat AudioMostCompatibleFormat {
			get {
				unsafe {
					var ret = (AudioFormat*) CMAudioFormatDescriptionGetMostCompatibleFormat (Handle);
					if (ret is null)
						return new AudioFormat ();
					return *ret;
				}
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* AudioFormatListItem* */ IntPtr CMAudioFormatDescriptionGetRichestDecodableFormat (/* CMAudioFormatDescriptionRef */ IntPtr desc);

		/// <summary>Audio-media only: Returns the richest decodable format.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioFormat AudioRichestDecodableFormat {
			get {
				unsafe {
					var ret = (AudioFormat*) CMAudioFormatDescriptionGetRichestDecodableFormat (Handle);
					if (ret is null)
						return new AudioFormat ();
					return *ret;
				}
			}
		}

		// CMVideoDimensions => int32_t width + int32_t height

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CMVideoDimensions CMVideoFormatDescriptionGetDimensions (/* CMVideoFormatDescriptionRef */ IntPtr videoDesc);

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CGRect CMVideoFormatDescriptionGetCleanAperture (/* CMVideoFormatDescriptionRef */ IntPtr videoDesc, /* Boolean */ byte originIsAtTopLeft);

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static /* CFArrayRef */ IntPtr CMVideoFormatDescriptionGetExtensionKeysCommonWithImageBuffers ();

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CGSize CMVideoFormatDescriptionGetPresentationDimensions (/* CMVideoFormatDescriptionRef */ IntPtr videoDesc, /* Boolean */ byte usePixelAspectRatio, /* Boolean */ byte useCleanAperture);

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static /* Boolean */ byte CMVideoFormatDescriptionMatchesImageBuffer (/* CMVideoFormatDescriptionRef */ IntPtr videoDesc, /* CVImageBufferRef */ IntPtr imageBuffer);

#endif
	}

	/// <summary>A <see cref="CoreMedia.CMFormatDescription" /> that describes an audio format.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMAudioFormatDescription : CMFormatDescription {
		[Preserve (Conditional = true)]
		internal CMAudioFormatDescription (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		// TODO: Move more audio specific methods here
	}

	/// <summary>A <see cref="CoreMedia.CMFormatDescription" /> that describes video.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CMVideoFormatDescription : CMFormatDescription {
		[Preserve (Conditional = true)]
		internal CMVideoFormatDescription (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

#if !COREBUILD
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionCreate (
			/* CFAllocatorRef */ IntPtr allocator,
			CMVideoCodecType codecType,
			/* int32_t */ int width,
			/* int32_t */ int height,
			/* CFDictionaryRef */ IntPtr extensions,
			/* CMVideoFormatDescriptionRef* */ IntPtr* outDesc);

		static IntPtr CreateCMVideoFormatDescription (CMVideoCodecType codecType, CMVideoDimensions size, NSDictionary? extensions)
		{
			IntPtr handle;
			CMFormatDescriptionError error;
			unsafe {
				error = CMVideoFormatDescriptionCreate (IntPtr.Zero, codecType, size.Width, size.Height, extensions.GetHandle (), &handle);
				GC.KeepAlive (extensions);
			}
			if (error != CMFormatDescriptionError.None)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			return handle;
		}

		/// <summary>Initialize a new <see cref="CMVideoFormatDescription" /> instance.</summary>
		/// <param name="codecType">The video codec type for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		/// <param name="size">The video dimensions for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		public CMVideoFormatDescription (CMVideoCodecType codecType, CMVideoDimensions size)
			: base (CreateCMVideoFormatDescription (codecType, size, null), true)
		{
		}

		/// <summary>Initialize a new <see cref="CMVideoFormatDescription" /> instance.</summary>
		/// <param name="codecType">The video codec type for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		/// <param name="size">The video dimensions for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		/// <param name="extensions">An optional dictionary of extensions for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		public CMVideoFormatDescription (CMVideoCodecType codecType, CMVideoDimensions size, NSDictionary? extensions)
			: base (CreateCMVideoFormatDescription (codecType, size, extensions), true)
		{
		}

		/// <summary>Initialize a new <see cref="CMVideoFormatDescription" /> instance.</summary>
		/// <param name="codecType">The video codec type for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		/// <param name="size">The video dimensions for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		/// <param name="extensions">An optional dictionary of extensions for the new <see cref="CMVideoFormatDescription" /> instance.</param>
		public CMVideoFormatDescription (CMVideoCodecType codecType, CMVideoDimensions size, CMFormatDescriptionExtensions? extensions)
			: base (CreateCMVideoFormatDescription (codecType, size, extensions?.Dictionary), true)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMVideoDimensions Dimensions {
			get {
				return CMVideoFormatDescriptionGetDimensions (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionCreateForImageBuffer (
			/* CFAllocatorRef */ IntPtr allocator,
			/* CVImageBufferRef */ IntPtr imageBuffer,
			/* CMVideoFormatDescriptionRef* */ IntPtr* outDesc);

		/// <param name="imageBuffer">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMVideoFormatDescription? CreateForImageBuffer (CVImageBuffer imageBuffer, out CMFormatDescriptionError error)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));

			IntPtr desc;
			unsafe {
				error = CMVideoFormatDescriptionCreateForImageBuffer (IntPtr.Zero, imageBuffer.Handle, &desc);
				GC.KeepAlive (imageBuffer);
			}
			if (error != CMFormatDescriptionError.None)
				return null;

			return new CMVideoFormatDescription (desc, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionCreateFromH264ParameterSets (
			/* CFAllocatorRef */ IntPtr allocator,
			/* size_t  */ nuint parameterSetCount,
			/* const uint8_t* const* */ IntPtr* parameterSetPointers,
			/* size_t*  */ nuint* parameterSetSizes,
			/* int */ int NALUnitHeaderLength,
			/* CMFormatDescriptionRef* */ IntPtr* formatDescriptionOut);

		/// <param name="parameterSets">To be added.</param>
		///         <param name="nalUnitHeaderLength">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CMVideoFormatDescription? FromH264ParameterSets (List<byte []> parameterSets, int nalUnitHeaderLength, out CMFormatDescriptionError error)
		{
			if (parameterSets is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (parameterSets));

			if (parameterSets.Count < 2)
				ObjCRuntime.ThrowHelper.ThrowArgumentException ("parameterSets must contain at least two elements");

			if (nalUnitHeaderLength != 1 && nalUnitHeaderLength != 2 && nalUnitHeaderLength != 4)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (nalUnitHeaderLength), "must be 1, 2 or 4");

			var handles = new GCHandle [parameterSets.Count];
			try {
				var parameterSetSizes = new nuint [parameterSets.Count];
				var parameterSetPtrs = new IntPtr [parameterSets.Count];

				for (int i = 0; i < parameterSets.Count; i++) {
					handles [i] = GCHandle.Alloc (parameterSets [i], GCHandleType.Pinned); // This can't use unsafe code because we need to get the pointer for an unbound number of objects.
					parameterSetPtrs [i] = handles [i].AddrOfPinnedObject ();
					parameterSetSizes [i] = (nuint) parameterSets [i].Length;
				}

				IntPtr desc;
				unsafe {
					fixed (IntPtr* parameterSetPtrsPtr = parameterSetPtrs) {
						fixed (nuint* parameterSetSizesPtr = parameterSetSizes) {
							error = CMVideoFormatDescriptionCreateFromH264ParameterSets (IntPtr.Zero, (nuint) parameterSets.Count, parameterSetPtrsPtr, parameterSetSizesPtr, nalUnitHeaderLength, &desc);
						}
					}
				}
				if (error != CMFormatDescriptionError.None)
					return null;

				return new CMVideoFormatDescription (desc, true);
			} finally {
				for (int i = 0; i < parameterSets.Count; i++) {
					if (handles [i].IsAllocated)
						handles [i].Free ();
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionGetH264ParameterSetAtIndex (
			/* CMFormatDescriptionRef */ IntPtr videoDesc,
			/* size_t  */ nuint parameterSetIndex,
			/* const uint8_t** */ IntPtr* parameterSetPointerOut,
			/* size_t* */ nuint* parameterSetSizeOut,
			/* size_t* */ nuint* parameterSetCountOut,
			/* int* */ int* nalUnitHeaderLengthOut);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public byte []? GetH264ParameterSet (nuint index, out nuint parameterSetCount, out int nalUnitHeaderLength, out CMFormatDescriptionError error)
		{
			IntPtr ret;
			nuint parameterSetSizeOut;
			parameterSetCount = default;
			nalUnitHeaderLength = default;
			unsafe {
				error = CMVideoFormatDescriptionGetH264ParameterSetAtIndex (
							GetCheckedHandle (),
							index,
							&ret,
							&parameterSetSizeOut,
							(nuint*) Unsafe.AsPointer<nuint> (ref parameterSetCount),
							(int*) Unsafe.AsPointer<int> (ref nalUnitHeaderLength));
			}
			if (error != CMFormatDescriptionError.None)
				return null;

			var arr = new byte [(int) parameterSetSizeOut];
			Marshal.Copy (ret, arr, 0, (int) parameterSetSizeOut);

			return arr;
		}

		/// <param name="originIsAtTopLeft">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect GetCleanAperture (bool originIsAtTopLeft)
		{
			return CMVideoFormatDescriptionGetCleanAperture (Handle, originIsAtTopLeft.AsByte ());
		}

		/// <param name="usePixelAspectRatio">To be added.</param>
		///         <param name="useCleanAperture">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize GetPresentationDimensions (bool usePixelAspectRatio, bool useCleanAperture)
		{
			return CMVideoFormatDescriptionGetPresentationDimensions (Handle, usePixelAspectRatio.AsByte (), useCleanAperture.AsByte ());
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject? []? GetExtensionKeysCommonWithImageBuffers ()
		{
			var arr = CMVideoFormatDescriptionGetExtensionKeysCommonWithImageBuffers ();
			return CFArray.ArrayFromHandle<NSString> (arr);
		}

		/// <param name="imageBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool VideoMatchesImageBuffer (CVImageBuffer imageBuffer)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));
			bool result = CMVideoFormatDescriptionMatchesImageBuffer (Handle, imageBuffer.Handle) != 0;
			GC.KeepAlive (imageBuffer);
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionCreateFromHEVCParameterSets (
			/* CFAllocatorRef */ IntPtr allocator,
			/* size_t  */ nuint parameterSetCount,
			/* const uint8_t* const* */ IntPtr* parameterSetPointers,
			/* size_t*  */ nuint* parameterSetSizes,
			/* int */ int NALUnitHeaderLength,
			/* CFDictionaryRef */ IntPtr extensions,
			/* CMFormatDescriptionRef* */ IntPtr* formatDescriptionOut);

		/// <param name="parameterSets">To be added.</param>
		///         <param name="nalUnitHeaderLength">To be added.</param>
		///         <param name="extensions">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CMVideoFormatDescription? FromHevcParameterSets (List<byte []> parameterSets, int nalUnitHeaderLength, NSDictionary extensions, out CMFormatDescriptionError error)
		{
			if (parameterSets is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (parameterSets));

			if (parameterSets.Count < 3)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (parameterSets), "must contain at least three elements");

			if (nalUnitHeaderLength != 1 && nalUnitHeaderLength != 2 && nalUnitHeaderLength != 4)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (nalUnitHeaderLength), "must be 1, 2 or 4");

			var handles = new GCHandle [parameterSets.Count];
			try {
				var parameterSetSizes = new nuint [parameterSets.Count];
				var parameterSetPtrs = new IntPtr [parameterSets.Count];

				for (int i = 0; i < parameterSets.Count; i++) {
					handles [i] = GCHandle.Alloc (parameterSets [i], GCHandleType.Pinned); // This can't use unsafe code because we need to get the pointer for an unbound number of objects.
					parameterSetPtrs [i] = handles [i].AddrOfPinnedObject ();
					parameterSetSizes [i] = (nuint) parameterSets [i].Length;
				}

				IntPtr desc;
				unsafe {
					fixed (IntPtr* parameterSetPtrsPtr = parameterSetPtrs) {
						fixed (nuint* parameterSetSizesPtr = parameterSetSizes) {
							error = CMVideoFormatDescriptionCreateFromHEVCParameterSets (IntPtr.Zero, (nuint) parameterSets.Count, parameterSetPtrsPtr, parameterSetSizesPtr, nalUnitHeaderLength, extensions.GetHandle (), &desc);
							GC.KeepAlive (extensions);
						}
					}
				}
				if (error != CMFormatDescriptionError.None || desc == IntPtr.Zero)
					return null;

				return new CMVideoFormatDescription (desc, true);
			} finally {
				for (int i = 0; i < handles.Length; i++) {
					if (handles [i].IsAllocated)
						handles [i].Free ();
				}
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern /* OSStatus */ CMFormatDescriptionError CMVideoFormatDescriptionGetHEVCParameterSetAtIndex (
			/* CMFormatDescriptionRef */ IntPtr videoDesc,
			/* size_t  */ nuint parameterSetIndex,
			/* const uint8_t** */ IntPtr* parameterSetPointerOut,
			/* size_t* */ nuint* parameterSetSizeOut,
			/* size_t* */ nuint* parameterSetCountOut,
			/* int* */ int* nalUnitHeaderLengthOut);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public byte []? GetHevcParameterSet (nuint index, out nuint parameterSetCount, out int nalUnitHeaderLength, out CMFormatDescriptionError error)
		{
			IntPtr ret;
			nuint parameterSetSizeOut;
			parameterSetCount = default;
			nalUnitHeaderLength = default;
			unsafe {
				error = CMVideoFormatDescriptionGetHEVCParameterSetAtIndex (
							GetCheckedHandle (),
							index,
							&ret,
							&parameterSetSizeOut,
							(nuint*) Unsafe.AsPointer<nuint> (ref parameterSetCount),
							(int*) Unsafe.AsPointer<int> (ref nalUnitHeaderLength));
			}
			if (error != CMFormatDescriptionError.None)
				return null;

			var arr = new byte [(int) parameterSetSizeOut];
			Marshal.Copy (ret, arr, 0, (int) parameterSetSizeOut);

			return arr;
		}
#endif
	}
}
