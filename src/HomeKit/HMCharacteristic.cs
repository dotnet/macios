#nullable enable

using System;
using ObjCRuntime;
using Foundation;

namespace HomeKit {

	partial class HMCharacteristic {
		/// <summary>Gets a value that tells whether the characteristic supports event notifications.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SupportsEventNotification {
			get {
				foreach (var p in Properties) {
					if (p == HMCharacteristicPropertyInternal.SupportsEventNotification)
						return true;
				}
				return false;
			}
		}

		/// <summary>Gets a value that tells whether the characteristic value is readable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Readable {
			get {
				foreach (var p in Properties) {
					if (p == HMCharacteristicPropertyInternal.Readable)
						return true;
				}
				return false;
			}
		}

		/// <summary>Gets a value that tells whether the characteristic is writeable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Writable {
			get {
				foreach (var p in Properties) {
					if (p == HMCharacteristicPropertyInternal.Writable)
						return true;
				}
				return false;
			}
		}

		/// <summary>If <see langword="true" />, the property should not be displayed to the user.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool Hidden {
			get {
				foreach (var p in Properties) {
					if (p == HMCharacteristicPropertyInternal.Hidden)
						return true;
				}
				return false;
			}
		}
	}
}
