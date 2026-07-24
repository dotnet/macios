#nullable enable

using System;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	public partial class SFAuthorizationPluginView {

		/// <summary>Initializes the view with the authorization callbacks and engine reference provided by the plugin host.</summary>
		/// <param name="callbacks">The authorization callbacks for communicating with the Security Server.</param>
		/// <param name="engineRef">The authorization engine reference.</param>
		public SFAuthorizationPluginView (AuthorizationCallbacks callbacks, AuthorizationEngine engineRef)
			: base (NSObjectFlag.Empty)
		{
			ArgumentNullException.ThrowIfNull (callbacks);
			ArgumentNullException.ThrowIfNull (engineRef);
			InitializeHandle (_InitWithCallbacks (callbacks.Handle, engineRef.GetCheckedHandle ()), "initWithCallbacks:andEngineRef:");
			GC.KeepAlive (callbacks);
			GC.KeepAlive (engineRef);
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
