#nullable enable

using System;
using ObjCRuntime;
using Foundation;

namespace HomeKit {

	partial class HMActionSet {

#if NET
		/// <summary>What kind of <see cref="HomeKit.HMActionSetType" /><c>this</c> is&gt;.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public HMActionSetType ActionSetType {
			get {
				var s = _ActionSetType;
				// safety in case the field does not exists / cannot be loaded / new in future iOS versions...
				if (s is null)
					return HMActionSetType.Unknown;
				if (s == HMActionSetTypesInternal.WakeUp)
					return HMActionSetType.WakeUp;
				if (s == HMActionSetTypesInternal.Sleep)
					return HMActionSetType.Sleep;
				if (s == HMActionSetTypesInternal.HomeDeparture)
					return HMActionSetType.HomeDeparture;
				if (s == HMActionSetTypesInternal.HomeArrival)
					return HMActionSetType.HomeArrival;
				if (s == HMActionSetTypesInternal.UserDefined)
					return HMActionSetType.UserDefined;
				if (s == HMActionSetTypesInternal.TriggerOwned)
					return HMActionSetType.TriggerOwned;
				return HMActionSetType.Unknown;
			}
		}
	}
}
