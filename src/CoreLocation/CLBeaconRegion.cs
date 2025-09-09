#nullable enable

#if __IOS__ || __MACCATALYST__ || __MACOS__

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreLocation {
	/// <summary>This enum is used to select how to initialize a new instance of a <see cref="CLBeaconRegion" />.</summary>
	public enum CLBeaconRegionUuidType {
		/// <summary>The specified <see cref="NSUuid" /> is a proximity uuid.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'CLBeaconRegionUuidType.Uuid' instead, the constructor for this value deprecated.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CLBeaconRegionUuidType.Uuid' instead, the constructor for this value deprecated.")]
		[ObsoletedOSPlatform ("macos", "Use 'CLBeaconRegionUuidType.Uuid' instead, the constructor for this value deprecated.")]
		ProximityUuid,
		/// <summary>The specified <see cref="NSUuid" /> is not a proximity uuid.</summary>
		Uuid,
	}

	public partial class CLBeaconRegion {
		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="proximityUuid" />.</summary>
		/// <param name="proximityUuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		[ObsoletedOSPlatform ("ios13.0", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("macos", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public CLBeaconRegion (NSUuid proximityUuid, string identifier)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithProximityUuid (proximityUuid, identifier), "initWithProximityUUID:identifier:");
		}

		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="uuid" />.</summary>
		/// <param name="uuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		/// <param name="uuidType">Specifies whether the beacon is a proximity uuid or not.</param>
		public CLBeaconRegion (NSUuid uuid, string identifier, CLBeaconRegionUuidType uuidType)
			: base (NSObjectFlag.Empty)
		{
			switch (uuidType) {
			case CLBeaconRegionUuidType.ProximityUuid:
				InitializeHandle (_InitWithProximityUuid (uuid, identifier), "initWithProximityUUID:identifier:");
				break;
			case CLBeaconRegionUuidType.Uuid:
				InitializeHandle (_InitWithUuid (uuid, identifier), "initWithUUID:identifier:");
				break;
			default:
				throw new ArgumentException (nameof (uuidType));
			}
		}

		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="proximityUuid" /> and that assigns the <see cref="CoreLocation.CLBeaconRegion.Major" /> property.</summary>
		/// <param name="proximityUuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="major">Can be used by the app developer for any purpose.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		[ObsoletedOSPlatform ("ios13.0", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("macos", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public CLBeaconRegion (NSUuid proximityUuid, ushort major, string identifier)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithProximityUuid (proximityUuid, major, identifier), "initWithProximityUUID:major:identifier:");
		}

		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="uuid" /> and that assigns the <see cref="CoreLocation.CLBeaconRegion.Major" /> property.</summary>
		/// <param name="uuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="major">Can be used by the app developer for any purpose.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		/// <param name="uuidType">Specifies whether the beacon is a proximity uuid or not.</param>
		public CLBeaconRegion (NSUuid uuid, ushort major, string identifier, CLBeaconRegionUuidType uuidType)
			: base (NSObjectFlag.Empty)
		{
			switch (uuidType) {
			case CLBeaconRegionUuidType.ProximityUuid:
				InitializeHandle (_InitWithProximityUuid (uuid, major, identifier), "initWithProximityUUID:major:identifier:");
				break;
			case CLBeaconRegionUuidType.Uuid:
				InitializeHandle (_InitWithUuid (uuid, major, identifier), "initWithUUID:major:identifier:");
				break;
			default:
				throw new ArgumentException (nameof (uuidType));
			}
		}

		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="proximityUuid" /> and that assigns the <see cref="CoreLocation.CLBeaconRegion.Major" /> and <see cref="CoreLocation.CLBeaconRegion.Minor" /> properties.</summary>
		/// <param name="proximityUuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="major">Can be used by the app developer for any purpose.</param>
		/// <param name="minor">Can be used by the app developer for any purpose.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		[ObsoletedOSPlatform ("ios13.0", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[ObsoletedOSPlatform ("macos", "Use the constructor that takes an 'CLBeaconRegionUuidType' value instead, and pass 'CLBeaconRegionUuidType.Uuid'.")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public CLBeaconRegion (NSUuid proximityUuid, ushort major, ushort minor, string identifier)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithProximityUuid (proximityUuid, major, minor, identifier), "initWithProximityUUID:major:minor:identifier:");
		}

		/// <summary>Constructor that produces a region identified by <paramref name="identifier" /> that reports iBeacons associated with the <paramref name="uuid" /> and that assigns the <see cref="CoreLocation.CLBeaconRegion.Major" /> and <see cref="CoreLocation.CLBeaconRegion.Minor" /> properties.</summary>
		/// <param name="uuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="major">Can be used by the app developer for any purpose.</param>
		/// <param name="minor">Can be used by the app developer for any purpose.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		/// <param name="uuidType">Specifies whether the beacon is a proximity uuid or not.</param>
		public CLBeaconRegion (NSUuid uuid, ushort major, ushort minor, string identifier, CLBeaconRegionUuidType uuidType)
			: base (NSObjectFlag.Empty)
		{
			switch (uuidType) {
			case CLBeaconRegionUuidType.ProximityUuid:
				InitializeHandle (_InitWithProximityUuid (uuid, major, minor, identifier), "initWithProximityUUID:major:minor:identifier:");
				break;
			case CLBeaconRegionUuidType.Uuid:
				InitializeHandle (_InitWithUuid (uuid, major, minor, identifier), "initWithUUID:major:minor:identifier:");
				break;
			default:
				throw new ArgumentException (nameof (uuidType));
			}
		}

		/// <summary>Create a new <see cref="CLBeaconRegion" /> instance.</summary>
		/// <param name="uuid">The unique ID of the iBeacons of interest.</param>
		/// <param name="major">Can be used by the app developer for any purpose.</param>
		/// <param name="minor">Can be used by the app developer for any purpose.</param>
		/// <param name="identifier">The name of the region to be created.</param>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static CLBeaconRegion Create (NSUuid uuid, ushort? major, ushort? minor, string identifier)
		{
			if (!major.HasValue)
				return new CLBeaconRegion (uuid, identifier, CLBeaconRegionUuidType.Uuid);
			if (!minor.HasValue)
				return new CLBeaconRegion (uuid, major.Value, identifier, CLBeaconRegionUuidType.Uuid);
			return new CLBeaconRegion (uuid, major.Value, minor.Value, identifier, CLBeaconRegionUuidType.Uuid);
		}
	}
}

#endif
