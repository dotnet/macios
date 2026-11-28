using CoreMedia;

#nullable enable

namespace Cinematic {
	/// <summary>Specifies whether a decision identifies a single detection or a group of detections.</summary>
	[SupportedOSPlatform ("tvos17.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios17.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	public enum CNDecisionIdentifierType {
		/// <summary>The identifier refers to a single detection.</summary>
		Single,
		/// <summary>The identifier refers to a group, and the decision focuses on its best detection.</summary>
		Group,
	}

	/// <summary>Represents a decision to focus on a detection or group of detections at a specific time.</summary>
	public partial class CNDecision {

		/// <summary>Creates a decision to focus on a detection, or on the best detection in a group.</summary>
		/// <param name="time">The first presentation time at which the subject should be in focus.</param>
		/// <param name="detectionId">The identifier of the detection or group to focus on.</param>
		/// <param name="isStrong">Whether to keep the subject in focus for as long as possible.</param>
		/// <param name="identifierType">Whether <paramref name="detectionId" /> identifies a detection or a group.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="identifierType" /> is not a valid value.</exception>
		public CNDecision (CMTime time, long detectionId, bool isStrong, CNDecisionIdentifierType identifierType)
			: base (NSObjectFlag.Empty)
		{

			switch (identifierType) {
			case CNDecisionIdentifierType.Single:
				InitializeHandle (_InitWithSingleIdentifier (time, detectionId, isStrong), "initWithTime:detectionID:strong:");
				break;
			case CNDecisionIdentifierType.Group:
				InitializeHandle (_InitWithGroupIdentifier (time, detectionId, isStrong), "initWithTime:detectionGroupID:strong:");
				break;
			default:
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (identifierType), $"Unknown identifier type: {identifierType}");
				break;
			}
		}
	}
}
