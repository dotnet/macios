#if !__MACCATALYST__

using System;
using ObjCRuntime;

#nullable enable

namespace SceneKit {
	public partial class SCNRenderingOptions {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public SCNRenderingApi? RenderingApi {
			get {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SCNRenderingOptionsKeys.RenderingApiKey.get' is only supported on: 'ios', 'macOS/OSX', 'tvos'.
				// This CA1416 is an analyzer bug: https://github.com/dotnet/roslyn-analyzers/issues/7633
				var val = GetNUIntValue (SCNRenderingOptionsKeys.RenderingApiKey);
#pragma warning restore CA1416
				if (val is not null)
					return (SCNRenderingApi) (uint) val;
				return null;
			}

			set {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SCNRenderingOptionsKeys.RenderingApiKey.get' is only supported on: 'ios', 'macOS/OSX', 'tvos'.
				// This CA1416 is an analyzer bug: https://github.com/dotnet/roslyn-analyzers/issues/7633
				if (value.HasValue)
					SetNumberValue (SCNRenderingOptionsKeys.RenderingApiKey, (nuint) (uint) value.Value);
				else
					RemoveValue (SCNRenderingOptionsKeys.RenderingApiKey);
#pragma warning restore CA1416
			}
		}
	}
}

#endif
