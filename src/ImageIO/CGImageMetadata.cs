//
// CGImageMetadata.cs
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014, Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreFoundation;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace ImageIO {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public partial class CGImageMetadataEnumerateOptions {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Recursive { get; set; }

		internal NSMutableDictionary ToDictionary ()
		{
			var dict = new NSMutableDictionary ();

			if (Recursive)
				dict.LowlevelSetObject (CFBoolean.TrueHandle, kCGImageMetadataEnumerateRecursively);

			return dict;
		}
	}

	/// <param name="path">To be added.</param>
	///     <param name="tag">To be added.</param>
	///     <summary>Callback for the &lt;see cref=M:MonoTouch.ImageIO.CGImageMetadata.EnumerateTags/&gt; method.</summary>
	///     <returns>To be added.</returns>
	///     <remarks>To be added.</remarks>
	public delegate bool CGImageMetadataTagBlock (NSString path, CGImageMetadataTag tag);

	// CGImageMetadata.h
#if NET
	/// <summary>An immutable container for metadata. (See <see cref="T:ImageIO.CGMutableImageMetadata" />.)</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public partial class CGImageMetadata : NativeObject {
#if !NET
		public CGImageMetadata (NativeHandle handle)
			: base (handle, false)
		{
		}
#endif

		[Preserve (Conditional = true)]
		internal CGImageMetadata (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.ImageIOLibrary)]
		static extern /* CGImageMetadataRef __nullable */ IntPtr CGImageMetadataCreateFromXMPData (
			/* CFDataRef __nonnull */ IntPtr data);

		/// <param name="data">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGImageMetadata (NSData data)
			: base (CGImageMetadataCreateFromXMPData (data.GetNonNullHandle (nameof (data))), true, verify: true)
		{
			GC.KeepAlive (data);
		}

		/// <summary>Type identifier for the ImageIO.CGImageMetadata type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="M:CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isCGImageMetadata = (CFType.GetTypeID (foo.Handle) == CGImageMetadata.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		[DllImport (Constants.ImageIOLibrary, EntryPoint = "CGImageMetadataGetTypeID")]
		public extern static /* CFTypeID */ nint GetTypeID ();


		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CFStringRef __nullable */ IntPtr CGImageMetadataCopyStringValueWithPath (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata, /* CGImageMetadataTagRef __nullable */ IntPtr parent,
			/* CFStringRef __nonnull*/ IntPtr path);

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSString? GetStringValue (CGImageMetadata? parent, NSString path)
		{
			// parent may be null
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			var result = CGImageMetadataCopyStringValueWithPath (Handle, parent.GetHandle (), path.Handle);
			GC.KeepAlive (parent);
			GC.KeepAlive (path);
			return Runtime.GetNSObject<NSString> (result, true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CFArrayRef __nullable */ IntPtr CGImageMetadataCopyTags (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGImageMetadataTag []? GetTags ()
		{
			var result = CGImageMetadataCopyTags (Handle);
			return CFArray.ArrayFromHandleFunc (result, (handle) => new CGImageMetadataTag (handle, false), true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CGImageMetadataTagRef __nullable */ IntPtr CGImageMetadataCopyTagWithPath (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata, /* CGImageMetadataTagRef __nullable */ IntPtr parent,
			/* CFStringRef __nonnull */ IntPtr path);

		/// <param name="parent">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGImageMetadataTag? GetTag (CGImageMetadata? parent, NSString path)
		{
			// parent may be null
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			IntPtr result = CGImageMetadataCopyTagWithPath (Handle, parent.GetHandle (), path.Handle);
			GC.KeepAlive (parent);
			GC.KeepAlive (path);
			return (result == IntPtr.Zero) ? null : new CGImageMetadataTag (result, true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern unsafe static void CGImageMetadataEnumerateTagsUsingBlock (/* CGImageMetadataRef __nonnull */ IntPtr metadata,
						/* CFStringRef __nullable */ IntPtr rootPath, /* CFDictionaryRef __nullable */ IntPtr options, BlockLiteral* block);

#if !NET
		delegate byte TrampolineCallback (IntPtr blockPtr, NativeHandle key, NativeHandle value);

		[MonoPInvokeCallback (typeof (TrampolineCallback))]
#else
		[UnmanagedCallersOnly]
#endif
		static byte TagEnumerator (IntPtr block, NativeHandle key, NativeHandle value)
		{
			var nsKey = Runtime.GetNSObject<NSString> (key, false)!;
			var nsValue = Runtime.GetINativeObject<CGImageMetadataTag> (value, false)!;
			var del = BlockLiteral.GetTarget<CGImageMetadataTagBlock> (block);
			return del (nsKey, nsValue) ? (byte) 1 : (byte) 0;
		}

#if !NET
		static unsafe readonly TrampolineCallback static_action = TagEnumerator;
#endif

		/// <param name="rootPath">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="block">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void EnumerateTags (NSString? rootPath, CGImageMetadataEnumerateOptions? options, CGImageMetadataTagBlock block)
		{
			using var o = options?.ToDictionary ();
			unsafe {
#if NET
				delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, byte> trampoline = &TagEnumerator;
				using var block_handler = new BlockLiteral (trampoline, block, typeof (CGImageMetadata), nameof (TagEnumerator));
#else
				using var block_handler = new BlockLiteral ();
				block_handler.SetupBlockUnsafe (static_action, block);
#endif
				CGImageMetadataEnumerateTagsUsingBlock (Handle, rootPath.GetHandle (), o.GetHandle (), &block_handler);
				GC.KeepAlive (rootPath);
				GC.KeepAlive (o);
			}
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CFDataRef __nullable */ IntPtr CGImageMetadataCreateXMPData (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata, /* CFDictionaryRef __nullable */ IntPtr options);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? CreateXMPData ()
		{
			// note: there's no options defined for iOS7 (needs to be null)
			// we'll need to add an overload if this change in the future
			IntPtr result = CGImageMetadataCreateXMPData (Handle, IntPtr.Zero);
			return Runtime.GetNSObject<NSData> (result, true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static /* CGImageMetadataTagRef __nullable */ IntPtr CGImageMetadataCopyTagMatchingImageProperty (
			/* CGImageMetadataRef __nonnull */ IntPtr metadata, /* CFStringRef __nonnull */ IntPtr dictionaryName,
			/* CFStringRef __nonnull */ IntPtr propertyName);

		/// <param name="dictionaryName">To be added.</param>
		///         <param name="propertyName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGImageMetadataTag? CopyTagMatchingImageProperty (NSString dictionaryName, NSString propertyName)
		{
			if (dictionaryName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dictionaryName));
			if (propertyName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyName));
			IntPtr result = CGImageMetadataCopyTagMatchingImageProperty (Handle, dictionaryName.Handle, propertyName.Handle);
			GC.KeepAlive (dictionaryName);
			GC.KeepAlive (propertyName);
			return result == IntPtr.Zero ? null : new CGImageMetadataTag (result, true);
		}
	}
}
