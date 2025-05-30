// 
// VTSession.cs: Property setting/reading
//
// Authors: Miguel de Icaza (miguel@xamarin.com)
//     
// Copyright 2014-2015 Xamarin Inc.
//

#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

namespace VideoToolbox {
	/// <summary>Base class of <see cref="VideoToolbox.VTCompressionSession" /> and <see cref="VideoToolbox.VTDecompressionSession" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class VTSession : NativeObject {
		[Preserve (Conditional = true)]
		internal VTSession (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		// All of them returns OSStatus mapped to VTStatus enum

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTSessionSetProperty (IntPtr handle, IntPtr propertyKey, IntPtr value);

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTSessionCopyProperty (IntPtr handle, IntPtr propertyKey, /* CFAllocator */ IntPtr allocator, IntPtr* propertyValueOut);

		[DllImport (Constants.VideoToolboxLibrary)]
		internal extern static VTStatus VTSessionSetProperties (IntPtr handle, IntPtr propertyDictionary);

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTSessionCopySerializableProperties (IntPtr handle, /* CFAllocator */ IntPtr allocator, IntPtr* dictionaryOut);

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTSessionCopySupportedPropertyDictionary (/* VTSessionRef */ IntPtr session, /* CFDictionaryRef* */ IntPtr* supportedPropertyDictionaryOut);

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus SetProperties (VTPropertyOptions options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			var optionsDictionary = options.Dictionary;
			VTStatus status = VTSessionSetProperties (Handle, optionsDictionary.Handle);
			GC.KeepAlive (optionsDictionary);
			return status;
		}

		/// <param name="propertyKey">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus SetProperty (NSString propertyKey, NSObject? value)
		{
			if (propertyKey is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyKey));

			VTStatus status = VTSessionSetProperty (Handle, propertyKey.Handle, value.GetHandle ());
			GC.KeepAlive (propertyKey);
			GC.KeepAlive (value);
			return status;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTPropertyOptions? GetProperties ()
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTSessionCopySerializableProperties (Handle, IntPtr.Zero, &ret);
			}
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;

			var dict = Runtime.GetNSObject<NSDictionary> (ret, true);
			if (dict is null)
				return null;
			return new VTPropertyOptions (dict);
		}

		/// <param name="propertyKey">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSObject? GetProperty (NSString propertyKey)
		{
			if (propertyKey is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyKey));

			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTSessionCopyProperty (Handle, propertyKey.Handle, IntPtr.Zero, &ret);
				GC.KeepAlive (propertyKey);
			}
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSObject> (ret, true);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary? GetSerializableProperties ()
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTSessionCopySerializableProperties (Handle, IntPtr.Zero, &ret);
			}
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;

			return Runtime.GetNSObject<NSDictionary> (ret, true);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSDictionary? GetSupportedProperties ()
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTSessionCopySupportedPropertyDictionary (Handle, &ret);
			}
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;

			return Runtime.GetNSObject<NSDictionary> (ret, true);
		}
	}
}
