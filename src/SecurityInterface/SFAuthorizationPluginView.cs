#nullable enable

using System;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	public partial class SFAuthorizationPluginView {

		/// <summary>Initializes the view with the authorization callbacks and engine reference provided by the plugin host.</summary>
		/// <param name="callbacks">A pointer to the authorization callbacks supplied by the authorization plugin host.</param>
		/// <param name="engineRef">The authorization engine reference supplied by the authorization plugin host.</param>
		/// <remarks>The callback pointer and engine reference are borrowed and must remain valid for the lifetime of this view.</remarks>
		public unsafe SFAuthorizationPluginView (AuthorizationCallbacks* callbacks, AuthorizationEngine engineRef)
			: base (NSObjectFlag.Empty)
		{
			if (callbacks is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callbacks));
			ArgumentNullException.ThrowIfNull (engineRef);
			var engineHandle = engineRef.GetCheckedHandle ();
			InitializeHandle (_InitWithCallbacks ((IntPtr) callbacks, engineHandle), "initWithCallbacks:andEngineRef:");
			GC.KeepAlive (engineRef);
		}

		/// <summary>Gets the authorization engine reference for communicating with the Security Server.</summary>
		/// <remarks>The returned wrapper does not own the native engine reference.</remarks>
		public AuthorizationEngine EngineRef {
			get {
				var engine = AuthorizationEngine.Create (_EngineRef);
				if (engine is null)
					throw new InvalidOperationException ("The native authorization plugin view returned a null engine reference.");
				return engine;
			}
		}

		/// <summary>Gets the authorization callbacks structure for communicating with the Security Server.</summary>
		/// <remarks>The returned pointer is borrowed and must not be freed or mutated.</remarks>
		public unsafe AuthorizationCallbacks* Callbacks => (AuthorizationCallbacks*) _Callbacks;
	}
}
