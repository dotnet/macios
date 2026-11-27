
namespace ServiceManagement {
	/// <summary>Enumerates the registration status of a service.</summary>
	[Native]
	[NoTV, NoiOS, MacCatalyst (16, 0)]
	public enum SMAppServiceStatus : long {
		/// <summary>The service hasn't been registered, or the app unregistered it.</summary>
		NotRegistered,
		/// <summary>The service is registered and enabled.</summary>
		Enabled,
		/// <summary>The service is registered, but the user must approve it in System Settings before it can run.</summary>
		RequiresApproval,
		/// <summary>An error occurred and the service couldn't be found.</summary>
		NotFound,
	}

	// @interface SMAppService : NSObject
	[NoTV, NoiOS, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SMAppService {
		[Static]
		[Export ("loginItemServiceWithIdentifier:")]
		SMAppService CreateLoginItemService (string identifier);

		[Static]
		[Export ("mainAppService")]
		SMAppService MainApp { get; }

		[Static]
		[Export ("agentServiceWithPlistName:")]
		SMAppService CreateAgentService (string plistName);

		[Static]
		[Export ("daemonServiceWithPlistName:")]
		SMAppService CreateDaemonService (string plistName);

		[Export ("registerAndReturnError:")]
		bool Register ([NullAllowed] out NSError error);

		[Wrap ("Register (out var _)")]
		bool Register ();

		[Export ("unregisterAndReturnError:")]
		bool Unregister ([NullAllowed] out NSError error);

		[Wrap ("Unregister (out var _)")]
		bool Unregister ();

		[Async]
		[Export ("unregisterWithCompletionHandler:")]
		void Unregister (Action<NSError> handler);

		[Export ("status")]
		SMAppServiceStatus Status { get; }

		[Static]
		[Export ("statusForLegacyURL:")]
		SMAppServiceStatus GetStatus (NSUrl legacyUrl);

		[Static]
		[Export ("openSystemSettingsLoginItems")]
		void OpenSystemSettingsLoginItems ();
	}
}
