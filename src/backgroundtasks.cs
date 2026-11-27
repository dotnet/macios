//
// BackgroundTasks C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//

using CoreFoundation;

namespace BackgroundTasks {

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTaskRequest))]
	[DisableDefaultCtor]
	interface BGAppRefreshTaskRequest {
		[Export ("initWithIdentifier:")]
		NativeHandle Constructor (string identifier);
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTaskRequest))]
	[DisableDefaultCtor]
	interface BGProcessingTaskRequest {
		[Export ("initWithIdentifier:")]
		NativeHandle Constructor (string identifier);

		[Export ("requiresNetworkConnectivity")]
		bool RequiresNetworkConnectivity { get; set; }

		[Export ("requiresExternalPower")]
		bool RequiresExternalPower { get; set; }
	}

	[Abstract]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BGTaskRequest : NSCopying {
		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("earliestBeginDate", ArgumentSemantic.Copy)]
		NSDate EarliestBeginDate { get; set; }
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BGTask {
		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("expirationHandler", ArgumentSemantic.Strong)]
		Action ExpirationHandler { get; set; }

		[Export ("setTaskCompletedWithSuccess:")]
		void SetTaskCompleted (bool success);
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTask))]
	[DisableDefaultCtor]
	interface BGAppRefreshTask {
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTask))]
	[DisableDefaultCtor]
	interface BGProcessingTask {
	}

	[NoMac, iOS (27, 0), TV (27, 0), MacCatalyst (27, 0)]
	delegate void BGTaskSchedulerSubmitCompletionHandler ([NullAllowed] NSError error);

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BGTaskScheduler {
		[Static]
		[Export ("sharedScheduler", ArgumentSemantic.Strong)]
		BGTaskScheduler Shared { get; }

		[Advice ("This API is not available when using iOS App Extensions.")]
		[Export ("registerForTaskWithIdentifier:usingQueue:launchHandler:")]
		bool Register (string identifier, [NullAllowed] DispatchQueue queue, Action<BGTask> launchHandler);

		[Deprecated (PlatformName.iOS, 27, 0, message: "Use 'Submit (BGTaskRequest, BGTaskSchedulerSubmitCompletionHandler)' instead to capture all error conditions.")]
		[Deprecated (PlatformName.TvOS, 27, 0, message: "Use 'Submit (BGTaskRequest, BGTaskSchedulerSubmitCompletionHandler)' instead to capture all error conditions.")]
		[Deprecated (PlatformName.MacCatalyst, 27, 0, message: "Use 'Submit (BGTaskRequest, BGTaskSchedulerSubmitCompletionHandler)' instead to capture all error conditions.")]
		[Export ("submitTaskRequest:error:")]
		bool Submit (BGTaskRequest taskRequest, [NullAllowed] out NSError error);

		[iOS (27, 0), TV (27, 0), MacCatalyst (27, 0)]
		[Async]
		[Export ("submitTaskRequest:completionHandler:")]
		void Submit (BGTaskRequest taskRequest, BGTaskSchedulerSubmitCompletionHandler completionHandler);

		[Export ("cancelTaskRequestWithIdentifier:")]
		void Cancel (string identifier);

		[Export ("cancelAllTaskRequests")]
		void CancelAll ();

		[Async]
		[Export ("getPendingTaskRequestsWithCompletionHandler:")]
		void GetPending (Action<BGTaskRequest []> completionHandler);

		[NoMac, NoTV, NoMacCatalyst, iOS (26, 0)]
		[Static]
		[Export ("supportedResources")]
		BGContinuedProcessingTaskRequestResources SupportedResources { get; }
	}

#if XAMCORE_5_0
	[NoTV]
#else
	[TV (17, 0)]
	[Obsoleted (PlatformName.TvOS, 27, 0, message: "This type is not available on tvOS 27.0 or later.")]
#endif
	[NoMac, iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (BGProcessingTask))]
	interface BGHealthResearchTask { }

	[TV (17, 0), NoMac, iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (BGProcessingTaskRequest))]
	interface BGHealthResearchTaskRequest {
		[Export ("protectionTypeOfRequiredData")]
		string ProtectionTypeOfRequiredData { get; set; }
	}

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[BaseType (typeof (BGTask))]
	interface BGContinuedProcessingTask : NSProgressReporting {
		[Export ("title")]
		string Title { get; }

		[Export ("subtitle")]
		string Subtitle { get; }

		[Export ("updateTitle:subtitle:")]
		void UpdateTitle (string title, string subtitle);
	}

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[BaseType (typeof (BGTaskRequest))]
	interface BGContinuedProcessingTaskRequest {
		[Export ("title")]
		string Title { get; set; }

		[Export ("subtitle")]
		string Subtitle { get; set; }

		[Export ("strategy", ArgumentSemantic.Assign)]
		BGContinuedProcessingTaskRequestSubmissionStrategy Strategy { get; set; }

		[Export ("requiredResources", ArgumentSemantic.Assign)]
		BGContinuedProcessingTaskRequestResources RequiredResources { get; set; }

		[Export ("initWithIdentifier:title:subtitle:")]
		NativeHandle Constructor (string identifier, string title, string subtitle);
	}

	/// <summary>Enumerates how the scheduler handles a continued processing task request that can't run immediately.</summary>
	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Native]
	public enum BGContinuedProcessingTaskRequestSubmissionStrategy : long {
		/// <summary>Fail the submission if the scheduler has no capacity for the request, or if the system is under heavy load and can't run it immediately.</summary>
		Fail,
		/// <summary>Add the request to the back of a queue if it can't run immediately. The system cancels queued requests when the user removes the app from the app switcher.</summary>
		Queue,
	}

	/// <summary>Enumerates the extra system resources a continued processing task request can ask for.</summary>
	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Native]
	[Flags]
	public enum BGContinuedProcessingTaskRequestResources : long {
		/// <summary>No special system resources are required. The scheduler assumes the default resources, which allow background CPU and network access.</summary>
		Default = 0x0,
		/// <summary>The task needs background GPU access. The system rejects the request if the app lacks the required entitlement. Not all devices support background GPU execution, and the system doesn't guarantee that the task runs when the GPU is heavily contended.</summary>
		Gpu = (1L << 0),
	}

}
