//
// MTKTextureLoaderOptions.cs strong dictionary
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalKit {
#if !COREBUILD

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	public partial class MTKTextureLoaderOptions : DictionaryContainer {

		/// <summary>Gets an orable flag value that describes for which of the following the texture will be used: for blitting; as a render target; as a shader read source or write target; as a pixel format view; or for an unknown use.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MTLTextureUsage? TextureUsage {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureUsageKey);
				if (val is not null)
					return (MTLTextureUsage) (uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureUsageKey, (nuint) (uint) value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureUsageKey);
			}
		}

		/// <summary>Gets a value that describes whether read-write operations will be optimized, or be performed in the (non-optimized) order in which they are specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MTLCpuCacheMode? TextureCpuCacheMode {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey);
				if (val is not null)
					return (MTLCpuCacheMode) (uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey, (nuint) (uint) value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey);
			}
		}

#if NET
		/// <summary>Gets or sets the texture storage mode of the new texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public MTLStorageMode? TextureStorageMode {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureStorageModeKey);
				if (val is not null)
					return (MTLStorageMode) (uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureStorageModeKey, (nuint) (uint) value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureStorageModeKey);
			}
		}

#if NET
		/// <summary>Gets or sets a value that indicates the orientation of cube map texture data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public MTKTextureLoaderCubeLayout? CubeLayout {
			get {
				var val = GetNSStringValue (MTKTextureLoaderKeys.CubeLayoutKey);
				if (val is null)
					return null;
				return MTKTextureLoaderCubeLayoutExtensions.GetValue (val);
			}
			set {
				if (value.HasValue)
					SetStringValue (MTKTextureLoaderKeys.CubeLayoutKey, value.Value.GetConstant ());
				else
					RemoveValue (MTKTextureLoaderKeys.CubeLayoutKey);
			}
		}

#if NET
		/// <summary>Gets or sets a value that controls when to flip the texture.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public MTKTextureLoaderOrigin? Origin {
			get {
				var val = GetNSStringValue (MTKTextureLoaderKeys.OriginKey);
				if (val is null)
					return null;
				return MTKTextureLoaderOriginExtensions.GetValue (val);
			}
			set {
				if (value.HasValue)
					SetStringValue (MTKTextureLoaderKeys.OriginKey, value.Value.GetConstant ());
				else
					RemoveValue (MTKTextureLoaderKeys.OriginKey);
			}
		}
	}
#endif
}
