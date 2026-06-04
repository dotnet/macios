#nullable enable

namespace HomeKit {
	/// <summary>Common capabilities of an <see cref="HomeKit.HMCharacteristic" />, such as whether it's writable or supports events.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public class HMCharacteristicProperties {

		/// <summary>Gets or sets a value that tells whether the property supports numbered changes.</summary>
		public bool SupportsChangeNumber { get; set; }

		/// <summary>Gets or sets a value that tells whether the property support bonjour notifications.</summary>
		public bool SupportsBonjourNotification { get; set; }

		/// <summary>Gets or sets a value that tells whether the property supports event notifications.</summary>
		public bool SupportsEventNotification { get; set; }

		/// <summary>Gets or sets a value that tells whether the property is readable.</summary>
		public bool Readable { get; set; }

		/// <summary>Gets or sets a value that tells whether the property is writable.</summary>
		public bool Writable { get; set; }
	}
}
