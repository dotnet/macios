using CoreMedia;

#nullable enable

#if !__MACCATALYST__

namespace Cinematic {
	[SupportedOSPlatform ("tvos17.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios17.0")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public enum CNDecisionIdentifierType {
		Single,
		Group,
	}

	public partial class CNDecision {

		public CNDecision (CMTime time, long detectionId, bool isStrong, CNDecisionIdentifierType identifierType)
			: base (NSObjectFlag.Empty)
		{

			switch (identifierType) {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 17.0 and later, 'maccatalyst' 17.0 and later. 'CNDecisionIdentifierType.Single' and 'CNDecisionIdentifierType.Group' are only supported on: 'ios' 15.0 and later.
			case CNDecisionIdentifierType.Single:
				InitializeHandle (_InitWithSingleIdentifier (time, detectionId, isStrong), "initWithTime:detectionID:strong:");
				break;
			case CNDecisionIdentifierType.Group:
#pragma warning restore CA1416
				InitializeHandle (_InitWithGroupIdentifier (time, detectionId, isStrong), "initWithTime:detectionGroupID:strong:");
				break;
			default:
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (identifierType), $"Unknown identifier type: {identifierType}");
				break;
			}
		}
	}
}
#endif
