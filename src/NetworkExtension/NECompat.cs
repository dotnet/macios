#nullable enable

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;

namespace NetworkExtension {
	[Obsolete (Constants.ApiRemovedGeneral)]
	[Register ("NEFailureHandlerProvider", SkipRegistration = true)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public class NEFailureHandlerProvider : NEProvider {
		public override NativeHandle ClassHandle => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);

		protected NEFailureHandlerProvider (NSObjectFlag t) : base (t) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
		protected internal NEFailureHandlerProvider (NativeHandle handle) : base (handle) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);

		public unsafe virtual void HandleFailure (NSError error, Action completionHandler) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
		public unsafe virtual Task HandleFailureAsync (NSError error) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
	}
}
