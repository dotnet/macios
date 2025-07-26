// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System;
using System.Runtime.Versioning;
using Foundation;
using CoreGraphics;
using Metal;
using ObjCBindings;

namespace TestNamespace;

[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<StrongDictionary> ()]
public partial class CARendererOptions : DictionaryContainer {

	[Binding<StrongDictionaryKeys> (Name = "CARendererOptionKeys", Flags = StrongDictionaryKeys.BackwardCompatible)]
	public static partial class Keys {

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst13.1")]
		[Field<Property> ("AVCaptureDeviceTypeBuiltInTelephotoCamera")]
		public static partial NSString ColorSpace { get; }

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst13.1")]
		[Field<Property> ("kCARendererMetalCommandQueue")]
		public static partial NSString MetalCommandQueue { get; }
	}

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<StrongDictionaryProperty> (nameof (Keys.ColorSpace), StrongDictionaryKeyClass = typeof (Keys))]
	public partial CGColorSpace? ColorSpace { get; set; }

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<StrongDictionaryProperty> (nameof (Keys.MetalCommandQueue), StrongDictionaryKeyClass = typeof (Keys))]
	public partial IMTLCommandQueue? MetalCommandQueue { get; set; }

	// this is a random property to test the generator's handling of properties that are not marked as StrongDictionaryProperty.
	public int Random => Int32.MaxValue;
}
