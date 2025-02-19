#nullable enable

using System;

using ObjCRuntime;
using Foundation;

namespace HomeKit {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class HMCharacteristicProperties {

		/// <summary>Gets or sets a value that tells whether the property supports numbered changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SupportsChangeNumber { get; set; }

		/// <summary>Gets or sets a value that tells whether the property support bonjour notifications.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SupportsBonjourNotification { get; set; }

		/// <summary>Gets or sets a value that tells whether the property supports event notifications.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SupportsEventNotification { get; set; }

		/// <summary>Gets or sets a value that tells whether the property is readable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Readable { get; set; }

		/// <summary>Gets or sets a value that tells whether the property is writable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Writable { get; set; }
	}
}
