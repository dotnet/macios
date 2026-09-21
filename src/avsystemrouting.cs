#nullable enable

using Foundation;
using ObjCRuntime;
using UniformTypeIdentifiers;

namespace AVSystemRouting {

	[iOS (27, 0)]
	[Native]
	public enum AVSystemRouteLaunchMode : long {
		Application,
		Player,
	}

	[iOS (27, 0)]
	[Native]
	public enum AVSystemRouteEventReason : long {
		Activate,
		Deactivate,
	}

	[iOS (27, 0)]
	[ErrorDomain ("AVSystemRoutingErrorDomain")]
	[Native]
	public enum AVSystemRoutingError : long {
		ConnectionFailed = -73985,
	}

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRoute {
		[Export ("protocolType", ArgumentSemantic.Copy)]
		UTType ProtocolType { get; }

		[Export ("routeSymbolName", ArgumentSemantic.Copy)]
		string RouteSymbolName { get; }

		[Export ("routeDisplayName", ArgumentSemantic.Copy)]
		string RouteDisplayName { get; }

		[Export ("addSession:")]
		bool AddSession (AVSystemRouteSession session);

		[Export ("removeSession:")]
		void RemoveSession (AVSystemRouteSession session);

		// From the AVSystemRoute (CustomExtensionCommunication) category.
		[Export ("routeDataChannel")]
		AVSystemRouteDataChannel RouteDataChannel { get; }
	}

	delegate void AVSystemRouteSessionStartHandler ([NullAllowed] NSError launchError, [NullAllowed] AVSystemRouteMediaSession mediaSession);

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRouteSession {
		[Export ("initWithURL:mode:")]
		NativeHandle Constructor (NSUrl url, AVSystemRouteLaunchMode mode);

		[Async (ResultTypeName = "AVSystemRouteSessionStartResult")]
		[Export ("startWithCompletionHandler:")]
		void Start (AVSystemRouteSessionStartHandler completionHandler);

		[Export ("stop")]
		void Stop ();
	}

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRouteMediaSession {
		// AVPlaybackUserInterfaceControllable is tracked in the AVKit todo.
		[NullAllowed, Export ("playbackControl")]
		NSObject PlaybackControl { get; }

		[NullAllowed, Export ("dataChannel")]
		AVSystemRouteDataChannel DataChannel { get; }
	}

	delegate void AVSystemRouteDataCompletionHandler ([NullAllowed] NSError error);

	interface IAVSystemRouteDataDelegate { }

	[iOS (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface AVSystemRouteDataDelegate {
		[Abstract]
		[Export ("receiveData:completionHandler:")]
		void ReceiveData (NSData data, AVSystemRouteDataCompletionHandler completionHandler);
	}

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRouteDataChannel {
		[Async]
		[Export ("sendData:completionHandler:")]
		void SendData (NSData data, AVSystemRouteDataCompletionHandler completionHandler);

		[Wrap ("WeakDataDelegate")]
		[NullAllowed]
		IAVSystemRouteDataDelegate DataDelegate { get; set; }

		[NullAllowed, Export ("dataDelegate", ArgumentSemantic.Weak)]
		NSObject WeakDataDelegate { get; set; }
	}

	delegate void AVSystemRouteControllerObserverCompletionHandler (bool success);

	interface IAVSystemRouteControllerObserver { }

	[iOS (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface AVSystemRouteControllerObserver {
		[Abstract]
		[Export ("systemRouteController:handleEvent:completionHandler:")]
		void HandleEvent (AVSystemRouteController controller, AVSystemRouteEvent @event, AVSystemRouteControllerObserverCompletionHandler completionHandler);
	}

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRouteController {
		[Static]
		[Export ("sharedController")]
		AVSystemRouteController SharedController { get; }

		[Static]
		[Export ("supportedExtensionAvailable")]
		bool SupportedExtensionAvailable { [Bind ("isSupportedExtensionAvailable")] get; }

		[Export ("addObserver:")]
		bool AddObserver (IAVSystemRouteControllerObserver observer);

		[Export ("removeObserver:")]
		void RemoveObserver (IAVSystemRouteControllerObserver observer);
	}

	[iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVSystemRouteEvent {
		[Export ("reason")]
		AVSystemRouteEventReason Reason { get; }

		[Export ("route")]
		AVSystemRoute Route { get; }
	}
}
