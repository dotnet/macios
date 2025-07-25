using System;
using Foundation;
using ObjCBindings;

namespace GameKit;

public partial class OuterClass {

	public partial class InnerClass {

		[Native ("GKErrorCode")]
		[BindingType<SmartEnum> (Flags = SmartEnum.ErrorCode, ErrorDomain = "GKErrorDomain")]
		public enum NestedGKError : long {
			None = 0,
			Unknown = 1,
			Cancelled,
			CommunicationsFailure,
			UserDenied,
			InvalidCredentials,
			NotAuthenticated,
			AuthenticationInProgress,
			InvalidPlayer,
			ScoreNotSet,
			ParentalControlsBlocked,
			PlayerStatusExceedsMaximumLength,
			PlayerStatusInvalid,
			MatchRequestInvalid = 13,
			Underage,
			GameUnrecognized,
			NotSupported,
			InvalidParameter,
			UnexpectedConnection,
			ChallengeInvalid = 19,
			TurnBasedMatchDataTooLarge,
			TurnBasedTooManySessions,
			TurnBasedInvalidParticipant,
			TurnBasedInvalidTurn,
			TurnBasedInvalidState,
			InvitationsDisabled = 25,
			PlayerPhotoFailure = 26,
			UbiquityContainerUnavailable = 27,
			MatchNotConnected = 28,
			GameSessionRequestInvalid = 29,
			RestrictedToAutomatch = 30,
			ApiNotAvailable = 31,
			NotAuthorized = 32,
			ConnectionTimeout = 33,
			ApiObsolete = 34,
			ICloudUnavailable = 35,
			LockdownMode = 36,
			AppUnlisted = 37,
			FriendListDescriptionMissing = 100,
			FriendListRestricted = 101,
			FriendListDenied = 102,
			FriendRequestNotAvailable = 103,
		}
	}
}
