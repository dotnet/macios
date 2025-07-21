/// <summary>This enum is used in the availability attributes to specify which platform any given attribute applies to.</summary>
public enum PlatformName : byte {
	/// <summary>Unspecified platform.</summary>
	None,
	/// <summary>The macOS platform.</summary>
	MacOSX,
	/// <summary>The iOS platform.</summary>
	iOS,
	/// <summary>The watchOS platform.</summary>
	WatchOS,
	/// <summary>The tvOS platform.</summary>
	TvOS,
	/// <summary>The Mac Catalyst platform.</summary>
	MacCatalyst,
}
