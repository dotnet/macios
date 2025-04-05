//
// GameKit.cs: This file describes the API that the generator will produce for GameKit
//
// Authors:
//   Miguel de Icaza
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2009, Novell, Inc.
// Copyright 2012-2014 Xamarin Inc. All rights reserved
//

using System;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

#nullable enable

namespace GameKit {

	// NSUInteger -> GKPeerPickerController.h
	/// <summary>An enumeration whose values specify acceptable ping for peer-to-peer connections.</summary>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 7, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum GKPeerPickerConnectionType : ulong {
		/// <summary>To be added.</summary>
		Online = 1 << 0,
		/// <summary>To be added.</summary>
		Nearby = 1 << 1,
	}

	// untyped enum -> GKPublicConstants.h
	[NoMac]
	[Deprecated (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[ErrorDomain ("GKVoiceChatServiceErrorDomain")]
	public enum GKVoiceChatServiceError {
		/// <summary>An internal error occurred.</summary>
		Internal = 32000,
		/// <summary>Remote packets stopped.</summary>
		NoRemotePackets = 32001,
		/// <summary>The client was unable to connect to the service.</summary>
		UnableToConnect = 32002,
		/// <summary>The remote participant ended the voice chat.</summary>
		RemoteParticipantHangup = 32003,
		/// <summary>The call ID was not valid.</summary>
		InvalidCallID = 32004,
		/// <summary>The service cannot access audio hardware.</summary>
		AudioUnavailable = 32005,
		/// <summary>The client was not initialized.</summary>
		UninitializedClient = 32006,
		/// <summary>The client is missing a required method.</summary>
		ClientMissingRequiredMethods = 32007,
		/// <summary>The remote participant was already in a voice chat.</summary>
		RemoteParticipantBusy = 32008,
		/// <summary>The remote participant cancelled the call before it started.</summary>
		RemoteParticipantCancelled = 32009,
		/// <summary>An invalid response was received from the remote participant.</summary>
		RemoteParticipantResponseInvalid = 32010,
		/// <summary>The remote participant did not accept the invitation.</summary>
		RemoteParticipantDeclinedInvite = 32011,
		/// <summary>The specified method was not valid when it was called.</summary>
		MethodCurrentlyInvalid = 32012,
		/// <summary>The network could not be accessed.</summary>
		NetworkConfiguration = 32013,
		/// <summary>The remote client version was not supported.</summary>
		UnsupportedRemoteVersion = 32014,
		/// <summary>The client is out of memory.</summary>
		OutOfMemory = 32015,
		/// <summary>A parameter was not valid.</summary>
		InvalidParameter = 32016,
	}

	// untyped enum -> GKPublicConstants.h
	[Deprecated (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[Deprecated (PlatformName.MacOSX, 10, 10)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	public enum GKSendDataMode {
		/// <summary>Slower, but rebroadcast until received. Reliable data is guaranteed to arrive in order of transmission.</summary>
		Reliable,
		/// <summary>Fast, but not guaranteed to arrive and may arrive out of order.</summary>
		Unreliable,
	}

	// untyped enum -> GKPublicConstants.h
	[Deprecated (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[Deprecated (PlatformName.MacOSX, 10, 10)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	public enum GKSessionMode {
		/// <summary>Acting as a server.</summary>
		Server,
		/// <summary>Acting as a client.</summary>
		Client,
		/// <summary>Acting as a peer.</summary>
		Peer,
	}

	// untyped enum -> GKPublicConstants.h
	[Deprecated (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[Deprecated (PlatformName.MacOSX, 10, 10)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	public enum GKPeerConnectionState {
		/// <summary>To be added.</summary>
		Available,
		/// <summary>To be added.</summary>
		Unavailable,
		/// <summary>To be added.</summary>
		Connected,
		/// <summary>To be added.</summary>
		Disconnected,
		/// <summary>To be added.</summary>
		Connecting,
		ConnectedRelay = 5,
	}

	// NSInteger -> GKLeaderboard.h
	[Native]
	public enum GKLeaderboardTimeScope : long {
		/// <summary>To be added.</summary>
		Today,
		/// <summary>To be added.</summary>
		Week,
		/// <summary>To be added.</summary>
		AllTime,
	}

	// NSInteger -> GKLeaderboard.h
	[Native]
	public enum GKLeaderboardPlayerScope : long {
		/// <summary>To be added.</summary>
		Global,
		/// <summary>To be added.</summary>
		FriendsOnly,
	}

	// NSInteger -> GKError.h
	[Native ("GKErrorCode")]
	[ErrorDomain ("GKErrorDomain")]
	public enum GKError : long {
		/// <summary>No error occurred.</summary>
		None = 0,
		/// <summary>An unknown error occurred.</summary>
		Unknown = 1,
		/// <summary>The user cancelled the operation.</summary>
		Cancelled,
		/// <summary>There was an error in communicating with the game server.</summary>
		CommunicationsFailure,
		/// <summary>The user denied an operation.</summary>
		UserDenied,
		/// <summary>The user name or password was invalid.</summary>
		InvalidCredentials,
		/// <summary>The player is not authenticated.</summary>
		NotAuthenticated,
		/// <summary>The player is authenticating.</summary>
		AuthenticationInProgress,
		/// <summary>The in-game representation of a player was invalid.</summary>
		InvalidPlayer,
		/// <summary>The score is not set.</summary>
		ScoreNotSet,
		/// <summary>The requested item or service is blocked.</summary>
		ParentalControlsBlocked,
		/// <summary>The player's status message is longer than allowed.</summary>
		PlayerStatusExceedsMaximumLength,
		/// <summary>The player's status message is not valid.</summary>
		PlayerStatusInvalid,
		/// <summary>The match request is logically impossible.</summary>
		MatchRequestInvalid = 13,
		/// <summary>The player is not old enough to access the item or service.</summary>
		Underage,
		/// <summary>The game center did not recognize the app or game. This can be caused by an invalid bundle identifier.</summary>
		GameUnrecognized,
		/// <summary>The Game Center is not supported by the device.</summary>
		NotSupported,
		/// <summary>A parameter was incorrect or miscategorized.</summary>
		InvalidParameter,
		/// <summary>There was a connection from an unexpected player.</summary>
		UnexpectedConnection,
		/// <summary>The challenge was invalid.</summary>
		ChallengeInvalid = 19,
		/// <summary>The match data was too large.</summary>
		TurnBasedMatchDataTooLarge,
		/// <summary>The maximum number of sessions was reached before the operation was requested.</summary>
		TurnBasedTooManySessions,
		/// <summary>The in-game representation of a player in a turn-based game was invalid.</summary>
		TurnBasedInvalidParticipant,
		/// <summary>An attempt was made to play out of turn.</summary>
		TurnBasedInvalidTurn,
		/// <summary>The session for a turn-based game was in an invalid state.</summary>
		TurnBasedInvalidState,
		/// <summary>The receiver is not currently receiving invitations.</summary>
		InvitationsDisabled = 25, // iOS 7.0
		/// <summary>The player's photo could not be retrieved.</summary>
		PlayerPhotoFailure = 26,  // iOS 8.0
		/// <summary>The iCloud container was unavailable.</summary>
		UbiquityContainerUnavailable = 27, // iOS 8.0
		/// <summary>The match is not connected.</summary>
		MatchNotConnected = 28,
		/// <summary>The session request was not valid.</summary>
		GameSessionRequestInvalid = 29,
		RestrictedToAutomatch = 30,
		ApiNotAvailable = 31,
		NotAuthorized = 32,
		ConnectionTimeout = 33,
		ApiObsolete = 34,
		ICloudUnavailable = 35,
		LockdownMode = 36,
		AppUnlisted = 37,
		DebugMode = 38,
		FriendListDescriptionMissing = 100,
		FriendListRestricted = 101,
		FriendListDenied = 102,
		FriendRequestNotAvailable = 103,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum GKConnectionState : long {
		/// <summary>To be added.</summary>
		NotConnected,
		/// <summary>To be added.</summary>
		Connected,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum GKTransportType : long {
		/// <summary>To be added.</summary>
		Unreliable,
		/// <summary>To be added.</summary>
		Reliable,
	}

	[Deprecated (PlatformName.MacOSX, 10, 14)]
	[Deprecated (PlatformName.TvOS, 12, 0)]
	[Deprecated (PlatformName.iOS, 12, 0)]
	[Native]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[ErrorDomain ("GKGameSessionErrorDomain")]
	public enum GKGameSessionErrorCode : long {
		/// <summary>To be added.</summary>
		Unknown = 1,
		/// <summary>To be added.</summary>
		NotAuthenticated = 2,
		/// <summary>To be added.</summary>
		SessionConflict = 3,
		/// <summary>To be added.</summary>
		SessionNotShared = 4,
		/// <summary>To be added.</summary>
		ConnectionCancelledByUser = 5,
		/// <summary>To be added.</summary>
		ConnectionFailed = 6,
		/// <summary>To be added.</summary>
		SessionHasMaxConnectedPlayers = 7,
		/// <summary>To be added.</summary>
		SendDataNotConnected = 8,
		/// <summary>To be added.</summary>
		SendDataNoRecipients = 9,
		/// <summary>To be added.</summary>
		SendDataNotReachable = 10,
		/// <summary>To be added.</summary>
		SendRateLimitReached = 11,
		/// <summary>To be added.</summary>
		BadContainer = 12,
		/// <summary>To be added.</summary>
		CloudQuotaExceeded = 13,
		/// <summary>To be added.</summary>
		NetworkFailure = 14,
		/// <summary>To be added.</summary>
		CloudDriveDisabled = 15,
		/// <summary>To be added.</summary>
		InvalidSession = 16,
	}

	// NSInteger -> GKMatch.h
	[Deprecated (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[Deprecated (PlatformName.MacOSX, 10, 10)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[Native]
	public enum GKMatchSendDataMode : long {
		/// <summary>Slower, but rebroadcast until received. Reliable data is guaranteed to arrive in order of transmission.</summary>
		///         <altmember cref="T:GameKit.GKSendDataMode" />
		Reliable,
		/// <summary>Fast, but not guaranteed to arrive and may arrive out of order.</summary>
		Unreliable,
	}

	// NSInteger -> GKMatch.h
	[Native]
	public enum GKPlayerConnectionState : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Connected,
		/// <summary>To be added.</summary>
		Disconnected,
	}

	// NSInteger -> GKVoiceChat.h
	[Deprecated (PlatformName.iOS, 18, 0, message: "No longer supported.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "No longer supported.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "No longer supported.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "No longer supported.")]
	[Native]
	public enum GKVoiceChatPlayerState : long {
		/// <summary>To be added.</summary>
		Connected,
		/// <summary>To be added.</summary>
		Disconnected,
		/// <summary>To be added.</summary>
		Speaking,
		/// <summary>To be added.</summary>
		Silent,
		/// <summary>To be added.</summary>
		Connecting,
	}

	// NSInteger -> GKPlayer.h
	[Native]
	public enum GKPhotoSize : long {
		/// <summary>To be added.</summary>
		Small,
		/// <summary>To be added.</summary>
		Normal,
	}

	// NSInteger -> GKTurnBasedMatch.h
	[Native]
	public enum GKTurnBasedMatchStatus : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Open,
		/// <summary>To be added.</summary>
		Ended,
		/// <summary>To be added.</summary>
		Matching,
	}

	// NSInteger -> GKTurnBasedMatch.h
	[Native]
	public enum GKTurnBasedParticipantStatus : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Invited,
		/// <summary>To be added.</summary>
		Declined,
		/// <summary>To be added.</summary>
		Matching,
		/// <summary>To be added.</summary>
		Active,
		/// <summary>To be added.</summary>
		Done,
	}

	// NSInteger -> GKTurnBasedMatch.h
	[Native]
	public enum GKTurnBasedMatchOutcome : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Quit,
		/// <summary>To be added.</summary>
		Won,
		/// <summary>To be added.</summary>
		Lost,
		/// <summary>To be added.</summary>
		Tied,
		/// <summary>To be added.</summary>
		TimeExpired,
		/// <summary>To be added.</summary>
		First,
		/// <summary>To be added.</summary>
		Second,
		/// <summary>To be added.</summary>
		Third,
		/// <summary>To be added.</summary>
		Fourth,
		/// <summary>To be added.</summary>
		CustomRange = 0xff0000,
	}

	// NSInteger -> GKChallenge.h
	[MacCatalyst (13, 1)]
	[Native]
	public enum GKChallengeState : long {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Pending,
		/// <summary>To be added.</summary>
		Completed,
		/// <summary>To be added.</summary>
		Declined,
	}

	// NSInteger -> GKGameCenterViewController.h
	[MacCatalyst (13, 1)]
	[Native]
	public enum GKGameCenterViewControllerState : long {
		/// <summary>To be added.</summary>
		Default = -1,
		/// <summary>To be added.</summary>
		Leaderboards,
		/// <summary>To be added.</summary>
		Achievements,
		/// <summary>To be added.</summary>
		Challenges,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		LocalPlayerProfile = 3,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		Dashboard = 4,
		[iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[TV (15, 0)]
		LocalPlayerFriendsList = 5,
	}

	// NSInteger -> GKMatchmaker.h
	[Native]
	[Deprecated (PlatformName.iOS, 18, 4, message: "Use 'GKInviteRecipientResponse' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 4, message: "Use 'GKInviteRecipientResponse' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 4, message: "Use 'GKInviteRecipientResponse' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 4, message: "Use 'GKInviteRecipientResponse' instead.")]
	public enum GKInviteeResponse : long {
		/// <summary>To be added.</summary>
		Accepted = 0,
		/// <summary>To be added.</summary>
		Declined = 1,
		/// <summary>To be added.</summary>
		Failed = 2,
		/// <summary>To be added.</summary>
		Incompatible = 3,
		/// <summary>To be added.</summary>
		UnableToConnect = 4,
		/// <summary>To be added.</summary>
		NoAnswer = 5,
	}

	// NSUInteger -> GKMatchmaker.h
	[Native]
	public enum GKMatchType : ulong {
		/// <summary>To be added.</summary>
		PeerToPeer,
		/// <summary>To be added.</summary>
		Hosted,
		/// <summary>To be added.</summary>
		TurnBased,
	}

	// uint8_t -> GKTurnBasedMatch.h
	public enum GKTurnBasedExchangeStatus : sbyte {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Active,
		/// <summary>To be added.</summary>
		Complete,
		/// <summary>To be added.</summary>
		Resolved,
		/// <summary>To be added.</summary>
		Canceled,
	}

	[Native]
	public enum GKInviteRecipientResponse : long {
		/// <summary>The recipient accepted.</summary>
		Accepted = 0,
		/// <summary>The recipient declined.</summary>
		Declined = 1,
		/// <summary>The request failed.</summary>
		Failed = 2,
		/// <summary>The versions of the game that are owned by the host and recipient are incompatible.</summary>
		Incompatible = 3,
		/// <summary>To be added.</summary>
		UnableToConnect = 4,
		/// <summary>The recipient did not answer.</summary>
		NoAnswer = 5,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GKAccessPointLocation : long {
		TopLeading,
		TopTrailing,
		BottomLeading,
		BottomTrailing,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GKLeaderboardType : long {
		Classic,
		Recurring,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GKMatchmakingMode : long {
		Default = 0,
		NearbyOnly = 1,
		AutomatchOnly = 2,
		[TV (15, 0)]
		[iOS (15, 0)]
		[MacCatalyst (15, 0)]
		InviteOnly = 3,
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[Native]
	public enum GKFriendsAuthorizationStatus : long {
		NotDetermined = 0,
		Restricted,
		Denied,
		Authorized,
	}
}
