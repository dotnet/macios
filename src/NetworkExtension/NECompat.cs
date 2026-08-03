#nullable enable

using System.ComponentModel;
using System.Threading.Tasks;

namespace NetworkExtension {
	[Obsolete (Constants.ApiRemovedGeneral)]
	[Register ("NEFailureHandlerProvider", SkipRegistration = true)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public class NEFailureHandlerProvider : NEProvider {
		public override NativeHandle ClassHandle => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);

		/// <summary>Constructor to call on derived classes to skip initialization and merely allocate the object.</summary>
		/// <param name="t">Unused sentinel value, pass NSObjectFlag.Empty.</param>
		protected NEFailureHandlerProvider (NSObjectFlag t) : base (t) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
		/// <summary>A constructor used when creating managed representations of unmanaged objects. Called by the runtime.</summary>
		/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
		protected internal NEFailureHandlerProvider (NativeHandle handle) : base (handle) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);

		public unsafe virtual void HandleFailure (NSError error, Action completionHandler) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
		public unsafe virtual Task HandleFailureAsync (NSError error) => throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
	}
}
