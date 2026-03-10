// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

enum PlatformName : byte {
	None,
	MacOSX,
	MacOSXAppExtension,
	iOS,
	iOSAppExtension,
	WatchOS,
	WatchOSAppExtension,
	TvOS,
	TvOSAppExtension,
	Swift,
	BridgeOS,
	MacCatalyst,
	DriverKit,
	VisionOS,
	VisionOSAppExtension,
	MacCatalystAppExtension,
}

enum AvailabilityKind {
	Introduced,
	Deprecated,
	Obsoleted,
	Unavailable,
}

class AvailabilityBaseAttribute : ICSharpCode.NRefactory.CSharp.Attribute {
	public bool IsShorthand { get; private set; }

	public AvailabilityBaseAttribute (
		AvailabilityKind availabilityKind,
		PlatformName platform,
		VersionTuple version,
		string? message,
		bool asShorthand = false)
	{
		if (asShorthand &&
			(availabilityKind == AvailabilityKind.Introduced ||
				availabilityKind == AvailabilityKind.Unavailable) &&
			String.IsNullOrWhiteSpace (message)) {
			string? shorthandName = null;
			switch (platform) {
			case PlatformName.iOS:
				shorthandName = "iOS";
				break;
			case PlatformName.MacOSX:
				shorthandName = "Mac";
				break;
			case PlatformName.WatchOS:
				shorthandName = "Watch";
				break;
			case PlatformName.TvOS:
				shorthandName = "TV";
				break;
			case PlatformName.MacCatalyst:
				shorthandName = "MacCatalyst";
				break;
			}

			if (shorthandName is not null) {
				if (availabilityKind == AvailabilityKind.Unavailable)
					shorthandName = "No" + shorthandName;
				Type = AstType.Create (shorthandName);
				IsShorthand = true;
			}
		}

		if (Type.IsNull) {
			Type = AstType.Create ("ObjCRuntime." + availabilityKind);

			Arguments.Add (new MemberReferenceExpression (
				new TypeReferenceExpression (AstType.Create ("ObjCRuntime.PlatformName")),
				platform.ToString ()
			));
		}

		if (!version.IsEmpty) {
			Arguments.Add (new PrimitiveExpression (version.Major));
			Arguments.Add (new PrimitiveExpression (version.Minor.GetValueOrDefault ()));
			if (version.Subminor is not null)
				Arguments.Add (new PrimitiveExpression (version.Subminor.Value));
		}

		if (!asShorthand) {
			if (!String.IsNullOrWhiteSpace (message))
				Arguments.Add (new NamedArgumentExpression ("message", new PrimitiveExpression (message)));
		}
	}

	public static IEnumerable<AvailabilityBaseAttribute> FromClang (Attr attr)
	{
		var platform = GetPlatform (attr.AvailabilityAttributePlatformIdentifierName);

		var shorthand = ShorthandFromClang (platform, attr);
		if (shorthand is not null) {
			yield return shorthand;
			yield break;
		}

		if (attr.AvailabilityAttributeIntroduced.HasValue && !attr.AvailabilityAttributeIntroduced.Value.IsEmpty)
			yield return new AvailabilityBaseAttribute (
				AvailabilityKind.Introduced, platform, attr.AvailabilityAttributeIntroduced ?? default, attr.AvailabilityAttributeMessage);

		if (attr.AvailabilityAttributeDeprecated.HasValue && !attr.AvailabilityAttributeDeprecated.Value.IsEmpty)
			yield return new AvailabilityBaseAttribute (
				AvailabilityKind.Deprecated, platform, attr.AvailabilityAttributeDeprecated.Value, attr.AvailabilityAttributeMessage);

		if (attr.AvailabilityAttributeObsoleted.HasValue && !attr.AvailabilityAttributeObsoleted.Value.IsEmpty)
			yield return new AvailabilityBaseAttribute (
				AvailabilityKind.Obsoleted, platform, attr.AvailabilityAttributeObsoleted.Value, attr.AvailabilityAttributeMessage);

		if (attr.AvailabilityAttributeUnavailable)
			yield return new AvailabilityBaseAttribute (
				AvailabilityKind.Unavailable, platform, VersionTuple.Empty, attr.AvailabilityAttributeMessage);
	}

	static PlatformName GetPlatform (string? name)
	{
		switch (name) {
		case "ios":
			return PlatformName.iOS;
		case "ios_app_extension":
			return PlatformName.iOSAppExtension;
		case "macosx":
		case "macos":
			return PlatformName.MacOSX;
		case "macosx_app_extension":
		case "macos_app_extension":
			return PlatformName.MacOSXAppExtension;
		case "watchos":
			return PlatformName.WatchOS;
		case "watchos_app_extension":
			return PlatformName.WatchOSAppExtension;
		case "tvos":
			return PlatformName.TvOS;
		case "tvos_app_extension":
			return PlatformName.TvOSAppExtension;
		case "swift":
			return PlatformName.Swift;
		case "bridgeos":
			return PlatformName.BridgeOS;
		case "uikitformac":
		case "macCatalyst":
		case "maccatalyst":
			return PlatformName.MacCatalyst;
		case "driverkit":
			return PlatformName.DriverKit;
		case "xros":
		case "visionOS":
		case "visionos":
			return PlatformName.VisionOS;
		case "xros_app_extension":
		case "visionos_app_extension":
			return PlatformName.VisionOSAppExtension;
		case "maccatalyst_app_extension":
			return PlatformName.MacCatalystAppExtension;
		default:
			throw new Exception ($"Unsupported clang availability platform: '{name}'");
		}
	}

	static AvailabilityBaseAttribute? ShorthandFromClang (PlatformName platform, Attr attr)
	{
		// shorthand attributes are only returned iff there
		// is one attribute of any kind for the platform
		var hasIntroduced = attr.AvailabilityAttributeIntroduced.HasValue && !attr.AvailabilityAttributeIntroduced.Value.IsEmpty;
		var hasDeprecated = attr.AvailabilityAttributeDeprecated.HasValue && !attr.AvailabilityAttributeDeprecated.Value.IsEmpty;
		var hasObsoleted = attr.AvailabilityAttributeObsoleted.HasValue && !attr.AvailabilityAttributeObsoleted.Value.IsEmpty;
		var hasUnavailable = attr.AvailabilityAttributeUnavailable;

		if (hasIntroduced && !hasDeprecated && !hasObsoleted && !hasUnavailable)
			return new AvailabilityBaseAttribute (AvailabilityKind.Introduced,
				platform, attr.AvailabilityAttributeIntroduced ?? default, attr.AvailabilityAttributeMessage, true);
		else if (hasUnavailable && !hasIntroduced && !hasDeprecated && !hasObsoleted)
			return new AvailabilityBaseAttribute (AvailabilityKind.Unavailable,
				platform, VersionTuple.Empty, attr.AvailabilityAttributeMessage, true);

		return null;
	}
}
