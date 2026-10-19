#pragma warning disable APL0003
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;

namespace TestNamespace;

[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("ios11.0")]
[SupportedOSPlatform ("tvos11.0")]
[BindingType<Class> ()]
public partial class CIImage {

	[SupportedOSPlatform ("maccatalyst13.1")]
	[Field<Property> ("FormatRGBA16Int")]
	public static partial int FormatRGBA16Int { get; }

	[SupportedOSPlatform ("maccatalyst13.1")]
	[Field<Property> ("kCIFormatABGR8")]
	public static partial int FormatABGR8 { get; }

	[SupportedOSPlatform ("maccatalyst13.1")]
	[Field<Property> ("kCIFormatLA8")]
	public static partial int FormatLA8 {
		get;

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		set;
	}

	[SupportedOSPlatform ("maccatalyst13.1")]
	[ObsoletedOSPlatform ("ios13.0", "Use 'Replacement' instead.")]
	[ObsoletedOSPlatform ("tvos13.0", "Use 'Replacement' instead.")]
	[ObsoletedOSPlatform ("macos10.15", "Use 'Replacement' instead.")]
	[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'Replacement' instead.")]
	[Field<Property> ("kCIFormatLA8", Flags = Property.Notification)]
	public static partial NSString DidProcessEditingNotification { get; }

}
