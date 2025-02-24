//
// CGMutableImageMetadata.cs
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014, Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace ImageIO {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class CGMutableImageMetadata : CGImageMetadata {

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CGMutableImageMetadataRef __nonnull */ IntPtr CGImageMetadataCreateMutable ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGMutableImageMetadata ()
			: base (CGImageMetadataCreateMutable (), true)
		{
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CGMutableImageMetadataRef __nullable */ IntPtr CGImageMetadataCreateMutableCopy (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata);

		/// <param name="metadata">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGMutableImageMetadata (CGImageMetadata metadata)
			: base (CGImageMetadataCreateMutableCopy (metadata.GetNonNullHandle (nameof (metadata))), true)
		{
			GC.KeepAlive (metadata);
		}

		[DllImport (Constants.ImageIOLibrary)]
		unsafe extern static byte CGImageMetadataRegisterNamespaceForPrefix (
			/* CGMutableImageMetadataRef __nonnull */ IntPtr metadata, /* CFStringRef __nonnull */ IntPtr xmlns,
			/* CFStringRef __nonnull */ IntPtr prefix, /* CFErrorRef __nullable */ IntPtr* error);

		/// <param name="xmlns">To be added.</param>
		///         <param name="prefix">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool RegisterNamespace (NSString xmlns, NSString prefix, out NSError? error)
		{
			if (xmlns is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (xmlns));
			if (prefix is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (prefix));
			byte result;
			IntPtr err;
			unsafe {
				result = CGImageMetadataRegisterNamespaceForPrefix (Handle, xmlns.Handle, prefix.Handle, &err);
				GC.KeepAlive (xmlns);
				GC.KeepAlive (prefix);
			}
			error = Runtime.GetNSObject<NSError> (err);
			return result != 0;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static byte CGImageMetadataSetTagWithPath (/* CGMutableImageMetadataRef __nonnull */ IntPtr metadata,
			/* CGImageMetadataTagRef __nullable */ IntPtr parent, /* CFStringRef __nonnull */ IntPtr path,
			/* CGImageMetadataTagRef __nonnull */ IntPtr tag);

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="tag">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetTag (CGImageMetadataTag? parent, NSString path, CGImageMetadataTag tag)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			if (tag is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (tag));
			bool result = CGImageMetadataSetTagWithPath (Handle, parent.GetHandle (), path.Handle, tag.Handle) != 0;
			GC.KeepAlive (parent);
			GC.KeepAlive (path);
			GC.KeepAlive (tag);
			return result;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static byte CGImageMetadataSetValueWithPath (/* CGMutableImageMetadataRef __nonnull */ IntPtr metadata,
			/* CGImageMetadataTagRef __nullable */ IntPtr parent, /* CFStringRef __nonnull */ IntPtr path,
			/* CFTypeRef __nonnull */ IntPtr value);

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetValue (CGImageMetadataTag? parent, NSString path, NSObject value)
		{
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
			bool result = SetValue (parent, path, value.Handle);
			GC.KeepAlive (value);
			return result;
		}

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetValue (CGImageMetadataTag? parent, NSString path, bool value)
		{
			return SetValue (parent, path, value ? CFBoolean.TrueHandle : CFBoolean.FalseHandle);
		}

		bool SetValue (CGImageMetadataTag? parent, NSString path, IntPtr value)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			bool result = CGImageMetadataSetValueWithPath (Handle, parent.GetHandle (), path.Handle, value) != 0;
			GC.KeepAlive (parent);
			GC.KeepAlive (path);
			return result;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static byte CGImageMetadataRemoveTagWithPath (/* CGMutableImageMetadataRef __nonnull */ IntPtr metadata,
			/* CGImageMetadataTagRef __nullable */ IntPtr parent, /* CFStringRef __nonnull */ IntPtr path);

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool RemoveTag (CGImageMetadataTag? parent, NSString path)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			bool result = CGImageMetadataRemoveTagWithPath (Handle, parent.GetHandle (), path.Handle) != 0;
			GC.KeepAlive (parent);
			GC.KeepAlive (path);
			return result;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static byte CGImageMetadataSetValueMatchingImageProperty (
			/* CGMutableImageMetadataRef __nonnull */ IntPtr metadata,
			/* CFStringRef __nonnull */ IntPtr dictionaryName, /* CFStringRef __nonnull */ IntPtr propertyName,
			/* CFTypeRef __nonnull */ IntPtr value);

		/// <param name="dictionaryName">To be added.</param>
		///         <param name="propertyName">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetValueMatchingImageProperty (NSString dictionaryName, NSString propertyName, NSObject value)
		{
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
			bool result = SetValueMatchingImageProperty (dictionaryName, propertyName, value.Handle);
			GC.KeepAlive (value);
			return result;
		}

		/// <param name="dictionaryName">To be added.</param>
		///         <param name="propertyName">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetValueMatchingImageProperty (NSString dictionaryName, NSString propertyName, bool value)
		{
			return SetValueMatchingImageProperty (dictionaryName, propertyName, value ? CFBoolean.TrueHandle : CFBoolean.FalseHandle);
		}

		bool SetValueMatchingImageProperty (NSString dictionaryName, NSString propertyName, IntPtr value)
		{
			if (dictionaryName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dictionaryName));
			if (propertyName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyName));
			bool result = CGImageMetadataSetValueMatchingImageProperty (Handle, dictionaryName.Handle, propertyName.Handle, value) != 0;
			GC.KeepAlive (dictionaryName);
			GC.KeepAlive (propertyName);
			return result;
		}
	}
}
