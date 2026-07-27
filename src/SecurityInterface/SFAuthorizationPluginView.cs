#nullable enable

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	public partial class SFAuthorizationPluginView {
		const nint retainNonatomic = 1;
		static readonly IntPtr callbacksAssociationKey = Selector.GetHandle ("xamarin_SFAuthorizationPluginView_callbacks");

		[DllImport (Constants.ObjectiveCLibrary)]
		static extern void objc_setAssociatedObject (IntPtr obj, IntPtr key, IntPtr value, nint policy);

		/// <summary>Initializes the view with the authorization callbacks and engine reference provided by the plugin host.</summary>
		/// <param name="callbacks">The authorization callbacks for communicating with the Security Server.</param>
		/// <param name="engineRef">The authorization engine reference.</param>
		public SFAuthorizationPluginView (AuthorizationCallbacks callbacks, AuthorizationEngine engineRef)
			: base (NSObjectFlag.Empty)
		{
			ArgumentNullException.ThrowIfNull (callbacks);
			ArgumentNullException.ThrowIfNull (engineRef);
			using var callbacksOwner = callbacks.Owns ? callbacks.CreateOwnedDataCopy () : null;
			var callbacksPointer = callbacksOwner?.Bytes ?? callbacks.GetCheckedPointer ();
			var engineHandle = engineRef.GetCheckedHandle ();
			InitializeHandle (_InitWithCallbacks (callbacksPointer, engineHandle), "initWithCallbacks:andEngineRef:");
			GC.KeepAlive (engineRef);
			if (callbacksOwner is not null) {
				objc_setAssociatedObject (Handle, callbacksAssociationKey, callbacksOwner.GetNonNullHandle (nameof (callbacksOwner)), retainNonatomic);
				GC.KeepAlive (callbacksOwner);
			}
			GC.KeepAlive (callbacks);
		}

		/// <summary>Gets the authorization engine reference for communicating with the Security Server.</summary>
		public AuthorizationEngine EngineRef {
			get {
				var engine = AuthorizationEngine.Create (_EngineRef);
				if (engine is null)
					throw new InvalidOperationException ("The native authorization plugin view returned a null engine reference.");
				return engine;
			}
		}

		/// <summary>Gets the authorization callbacks structure for communicating with the Security Server.</summary>
		public AuthorizationCallbacks? Callbacks => AuthorizationCallbacks.Create (_Callbacks);
	}
}
