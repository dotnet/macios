//
// HMCompat.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright (C) Microsoft Corporation. All rights reserved.
//

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Foundation;
using CoreFoundation;
using ObjCRuntime;

#nullable enable
namespace HomeKit {
#if !XAMCORE_5_0
#if __IOS__ && !__MACCATALYST__
	public unsafe partial class HMAccessorySetupManager {

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete (Constants.RemovedFromHomeKit)]
		public virtual void AddAndSetUpAccessories (HMMatterTopology topology, Action<NSError> completion) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete (Constants.RemovedFromHomeKit)]
		public virtual Task AddAndSetUpAccessoriesAsync (HMMatterTopology topology) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete (Constants.RemovedFromHomeKit)]
		public virtual void PerformMatterEcosystemAccessorySetup (HMAccessorySetupRequest request, HMMatterTopology topology, Action<NSError> completion) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete (Constants.RemovedFromHomeKit)]
		public virtual Task PerformMatterEcosystemAccessorySetupAsync (HMAccessorySetupRequest request, HMMatterTopology topology) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
	}

#endif

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
	[Register ("HMMatterRoom", SkipRegistration = true)]
	public partial class HMMatterRoom : NSObject, INSCoding, INSCopying, INSSecureCoding {
		public override NativeHandle ClassHandle => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterRoom (NativeHandle handle) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public HMMatterRoom (NSCoder coder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterRoom (NSObjectFlag t) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public HMMatterRoom (NSUuid uuid, string name) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual string Name => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual NSUuid Uuid => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual NSObject Copy (NSZone? zone) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual void EncodeTo (NSCoder encoder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
	[Register ("HMMatterHome", SkipRegistration = true)]
	public partial class HMMatterHome : NSObject, INSCoding, INSCopying, INSSecureCoding {

		public override NativeHandle ClassHandle => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		protected HMMatterHome (NativeHandle handle) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public HMMatterHome (NSCoder coder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterHome (NSObjectFlag t) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public HMMatterHome (NSUuid uuid, string name) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual NSObject Copy (NSZone? zone) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual void EncodeTo (NSCoder encoder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual string Name => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual NSUuid Uuid => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
	[Register ("HMMatterTopology", SkipRegistration = true)]
	public partial class HMMatterTopology : NSObject, INSCoding, INSCopying, INSSecureCoding {

		public override NativeHandle ClassHandle => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		protected HMMatterTopology (NativeHandle handle) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public HMMatterTopology (NSCoder coder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterTopology (NSObjectFlag t) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public HMMatterTopology (HMMatterHome [] homes) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual HMMatterHome [] Homes => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual NSObject Copy (NSZone? zone) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual void EncodeTo (NSCoder encoder) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
	}

	public delegate void HMFetchRoomHandler (HMMatterRoom [] rooms, NSError error);

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
	[Register ("HMMatterRequestHandler", SkipRegistration = true)]
	public partial class HMMatterRequestHandler : NSObject, INSExtensionRequestHandling {
		public override NativeHandle ClassHandle => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public HMMatterRequestHandler () => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterRequestHandler (NativeHandle handle) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		protected HMMatterRequestHandler (NSObjectFlag t) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual void FetchRooms (HMMatterHome home, HMFetchRoomHandler completion) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual Task<HMMatterRoom []> FetchRoomsAsync (HMMatterHome home) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual void PairAccessory (HMMatterHome home, string onboardingPayload, Action<NSError> completion) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual Task PairAccessoryAsync (HMMatterHome home, string onboardingPayload) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual void ConfigureAccessory (string accessoryName, HMMatterRoom accessoryRoom, Action<NSError> completion) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
		public virtual Task ConfigureAccessoryAsync (string accessoryName, HMMatterRoom accessoryRoom) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);

		public virtual void BeginRequestWithExtensionContext (NSExtensionContext context) => throw new InvalidOperationException (Constants.RemovedFromHomeKit);
	}

#endif
}
