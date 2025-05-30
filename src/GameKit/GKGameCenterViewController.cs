using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace GameKit {
	/// <summary>This enum is used to select how to initialize a new instance of a <see cref="GKGameCenterViewController" />.</summary>
	public enum GKGameCenterViewControllerInitializationOption {
		/// <summary>The <c>id</c> parameter passed to the constructor is an achievement ID.</summary>
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		Achievement,
		/// <summary>The <c>id</c> parameter passed to the constructor is a leaderboard set ID.</summary>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		LeaderboardSet,
	}

	public partial class GKGameCenterViewController {
		/// <summary>Create a new GKGameCenterViewController instance that presents an achievement.</summary>
		/// <param name="achievementId">The ID of the achievement to show.</param>
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		public GKGameCenterViewController (string achievementId)
			: this (achievementId, GKGameCenterViewControllerInitializationOption.Achievement)
		{
		}

		/// <summary>Create a new GKGameCenterViewController instance that presents an achievement or a leaderboard set.</summary>
		/// <param name="id">The ID of the achievement or the leaderboard set to show.</param>
		/// <param name="option">Use this option to specify whether the GKGameCenterViewController shows an achievement or a leader board set.</param>
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos14.0")]
		public GKGameCenterViewController (string id, GKGameCenterViewControllerInitializationOption option)
			: base (NSObjectFlag.Empty)
		{
			switch (option) {
			case GKGameCenterViewControllerInitializationOption.Achievement:
				InitializeHandle (_InitWithAchievementId (id));
				break;
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later, 'macOS/OSX' 12.0 and later, 'tvos' 14.0 and later. 'GKGameCenterViewController._InitWithLeaderboardSetId(string)' is only supported on: 'ios' 18.0 and later, 'maccatalyst' 18.0 and later, 'macOS/OSX' 15.0 and later, 'tvos' 18.0 and later.
			case GKGameCenterViewControllerInitializationOption.LeaderboardSet:
				InitializeHandle (_InitWithLeaderboardSetId (id));
#pragma warning restore CA1416
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (option), option, "Invalid enum value.");
			}
		}
	}
}
