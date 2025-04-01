#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CoreMedia {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class CMAttachmentBearer {

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CFDictionaryRef */ IntPtr CMCopyDictionaryOfAttachments (/* CFAllocatorRef */ IntPtr allocator, /* CMAttachmentBearerRef */ IntPtr target,
			/* CMAttachmentMode */ CMAttachmentMode attachmentMode);

		/// <param name="target">The object on which this method operates.</param>
		///         <param name="attachmentMode">An out parameter that receives a value that tells whether the attachments should propagate or not.</param>
		///         <summary>Returns an array of all the bearer's attachments and the attachment propagation mode to <paramref name="attachmentMode" />.</summary>
		///         <returns>An array of all the bearer's attachments.</returns>
		///         <remarks>To be added.</remarks>
		public static NSDictionary? GetAttachments (this ICMAttachmentBearer target, CMAttachmentMode attachmentMode)
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			var attachments = CMCopyDictionaryOfAttachments (IntPtr.Zero, target.Handle, attachmentMode);
			GC.KeepAlive (target);
			if (attachments == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSDictionary> (attachments, true);
		}

		// There is some API that needs a more strongly typed version of a NSDictionary
		// and there is no easy way to downcast from NSDictionary to NSDictionary<TKey, TValue>
		/// <typeparam name="TKey">To be added.</typeparam>
		///         <typeparam name="TValue">To be added.</typeparam>
		///         <param name="target">To be added.</param>
		///         <param name="attachmentMode">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSDictionary<TKey, TValue>? GetAttachments<TKey, TValue> (this ICMAttachmentBearer target, CMAttachmentMode attachmentMode)
			where TKey : class, INativeObject
			where TValue : class, INativeObject
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			var attachments = CMCopyDictionaryOfAttachments (IntPtr.Zero, target.Handle, attachmentMode);
			GC.KeepAlive (target);
			if (attachments == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSDictionary<TKey, TValue>> (attachments, true);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* CFTypeRef */ IntPtr CMGetAttachment (/* CMAttachmentBearerRef */ IntPtr target, /* CFStringRef */ IntPtr key,
			/* CMAttachmentMode */ CMAttachmentMode* attachmentModeOut);
		/// <typeparam name="T">The type of attachment to get.</typeparam>
		///         <param name="target">The object on which this method operates.</param>
		///         <param name="key">The string that identifies the attachment to return.</param>
		///         <param name="attachmentModeOut">An out parameter that receives a value that tells whether the attachment should propagate or not.</param>
		///         <summary>Returns the attachment that is identifed by <paramref name="key" /> and writes the attachment propagation mode to <paramref name="attachmentModeOut" />.</summary>
		///         <returns>The specified attachment.</returns>
		///         <remarks>To be added.</remarks>
		public static T? GetAttachment<T> (this ICMAttachmentBearer target, string key, out CMAttachmentMode attachmentModeOut) where T : class, INativeObject
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			var nsKey = CFString.CreateNative (key);
			IntPtr attchm;
			attachmentModeOut = default;
			unsafe {
				attchm = CMGetAttachment (target.Handle, nsKey, (CMAttachmentMode*) Unsafe.AsPointer<CMAttachmentMode> (ref attachmentModeOut));
				GC.KeepAlive (target);
			}
			CFString.ReleaseNative (nsKey);
			if (attchm != IntPtr.Zero)
				return Runtime.GetINativeObject<T> (attchm, false);
			return default (T);
		}
		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="target">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <param name="attachmentModeOut">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static T? GetAttachment<T> (this ICMAttachmentBearer target, CMSampleBufferAttachmentKey key, out CMAttachmentMode attachmentModeOut) where T : class, INativeObject
		{
			return GetAttachment<T> (target, key.GetConstant (), out attachmentModeOut);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMPropagateAttachments (/* CMAttachmentBearerRef */ IntPtr source, /* CMAttachmentBearerRef */ IntPtr destination);
		/// <param name="source">The source bearer.</param>
		///         <param name="destination">The destination bearer.</param>
		///         <summary>Propagates the attachments that belong to <paramref name="source" /> and are allowed to propagate to <paramref name="destination" />.</summary>
		///         <remarks>To be added.</remarks>
		public static void PropagateAttachments (this ICMAttachmentBearer source, ICMAttachmentBearer destination)
		{
			if (source is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (source));
			if (destination is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destination));
			CMPropagateAttachments (source.Handle, destination.Handle);
			GC.KeepAlive (source);
			GC.KeepAlive (destination);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMRemoveAllAttachments (/*CMAttachmentBearerRef*/ IntPtr target);
		/// <param name="target">The object on which this method operates.</param>
		///         <summary>Removes all of <paramref name="target" />'s attachment.'</summary>
		///         <remarks>To be added.</remarks>
		public static void RemoveAllAttachments (this ICMAttachmentBearer target)
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			CMRemoveAllAttachments (target.Handle);
			GC.KeepAlive (target);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMRemoveAttachment (/* CMAttachmentBearerRef */ IntPtr target, /* CFStringRef */ IntPtr key);
		/// <param name="target">The object on which this method operates.</param>
		///         <param name="key">The string that identifies the attachment to remove.</param>
		///         <summary>Removes the attachment that is identifed by <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public static void RemoveAttachment (this ICMAttachmentBearer target, string key)
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			var nsKey = CFString.CreateNative (key);
			CMRemoveAttachment (target.Handle, nsKey);
			GC.KeepAlive (target);
			CFString.ReleaseNative (nsKey);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMSetAttachment (/* CMAttachmentBearerRef */ IntPtr target, /* CFStringRef */ IntPtr key, /* CFTypeRef */ IntPtr value,
			/* CMAttachmentMode */ CMAttachmentMode attachmentMode);
		/// <param name="target">The object on which this method operates.</param>
		///         <param name="key">The string that identifies the attachment to set.</param>
		///         <param name="value">The object to attach.</param>
		///         <param name="attachmentMode">A value that tells whether the attachment should propagate or not.</param>
		///         <summary>Attaches <paramref name="value" /> to the bearer with the specified <paramref name="key" /> and <paramref name="attachmentMode" />.</summary>
		///         <remarks>To be added.</remarks>
		public static void SetAttachment (this ICMAttachmentBearer target, string key, INativeObject value, CMAttachmentMode attachmentMode)
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			var nsKey = CFString.CreateNative (key);
			CMSetAttachment (target.Handle, nsKey, value.Handle, attachmentMode);
			GC.KeepAlive (target);
			GC.KeepAlive (value);
			CFString.ReleaseNative (nsKey);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMSetAttachments (/* CMAttachmentBearerRef */ IntPtr target, /* CFDictionaryRef */ IntPtr theAttachments,
			/* CMAttachmentMode */ CMAttachmentMode attachmentMode);
		/// <param name="target">The object on which this method operates.</param>
		///         <param name="theAttachments">The objects to attach to the bearer.</param>
		///         <param name="attachmentMode">The attachment mode to use for all the attachments in <paramref name="theAttachments" />.</param>
		///         <summary>Attaches <paramref name="theAttachments" /> to the bearer with the specified <paramref name="attachmentMode" />.</summary>
		///         <remarks>To be added.</remarks>
		public static void SetAttachments (this ICMAttachmentBearer target, NSDictionary theAttachments, CMAttachmentMode attachmentMode)
		{
			if (target is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (target));
			if (theAttachments is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (theAttachments));
			CMSetAttachments (target.Handle, theAttachments.Handle, attachmentMode);
			GC.KeepAlive (target);
			GC.KeepAlive (theAttachments);
		}
	}
}
