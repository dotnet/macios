#nullable enable

using System;
using System.Buffers;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using Metal;
using MetalPerformanceShaders;

namespace MetalPerformanceShadersGraph {
	/// <summary>This enum is used to select how to initialize a new instance of a <see cref="MPSGraphExecutable" />.</summary>
	[SupportedOSPlatform ("ios17.0")]
	[SupportedOSPlatform ("maccatalyst17.0")]
	[SupportedOSPlatform ("macos14.0")]
	[SupportedOSPlatform ("tvos17.0")]
	public enum MPSGraphExecutableInitializationOption {
		/// <summary>The <c>packageUrl</c> parameter passed to the constructor is a url to a CoreML package.</summary>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		CoreMLPackage,

		/// <summary>The <c>packageUrl</c> parameter passed to the constructor is a url to a MPSGraph package.</summary>
		MPSGraphPackage,
	}

	public partial class MPSGraphExecutable {
		/// <summary>Create a new MPSGraphExecutable instance from a package url and a compilation descriptor..</summary>
		/// <param name="packageUrl">The url to the package to use.</param>
		/// <param name="compilationDescriptor">The optional compilation descriptor use.</param>
		/// <param name="option">Use this option to specify whether the package url points to a CoreML package or an MPSGraph package.</param>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		public MPSGraphExecutable (NSUrl packageUrl, MPSGraphCompilationDescriptor? compilationDescriptor, MPSGraphExecutableInitializationOption option)
			: base (NSObjectFlag.Empty)
		{
			switch (option) {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 17.0 and later, 'maccatalyst' 17.0 and later, 'macOS/OSX' 14.0 and later, 'tvos' 17.0 and later. 'MPSGraphExecutableInitializationOption.CoreMLPackage' is only supported on: 'ios' 18.0 and later, 'maccatalyst' 18.0 and later, 'macOS/OSX' 15.0 and later, 'tvos' 18.0 and later.
			case MPSGraphExecutableInitializationOption.CoreMLPackage:
				InitializeHandle (_InitWithCoreMLPackage (packageUrl, compilationDescriptor));
#pragma warning restore CA1416
				break;
			case MPSGraphExecutableInitializationOption.MPSGraphPackage:
				InitializeHandle (_InitWithMPSGraphPackage (packageUrl, compilationDescriptor));
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (option), option, "Invalid enum value.");
			}
		}
	}
}
