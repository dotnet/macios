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
			if (callbacks is null)
				ThrowHelper.ThrowArgumentNullException (nameof (callbacks));
			if (engineRef is null)
				ThrowHelper.ThrowArgumentNullException (nameof (engineRef));
			InitializeHandle (_InitWithCallbacks (callbacks.Handle, engineRef), "initWithCallbacks:andEngineRef:");
			GC.KeepAlive (callbacks);
			GC.KeepAlive (engineRef);
		}

		/// <summary>Gets the authorization callbacks structure for communicating with the Security Server.</summary>
		public AuthorizationCallbacks? Callbacks => AuthorizationCallbacks.Create (_Callbacks);
	}
}
