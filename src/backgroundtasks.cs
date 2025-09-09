//
// BackgroundTasks C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//
using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace BackgroundTasks {

	[TV (13, 0), NoMac, iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTaskRequest))]
	[DisableDefaultCtor]
	interface BGAppRefreshTaskRequest {
		[Export ("initWithIdentifier:")]
		NativeHandle Constructor (string identifier);
	}

	[TV (13, 0), NoMac, iOS (13, 0)]
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
	[TV (13, 0), NoMac, iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BGTaskRequest : NSCopying {
		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("earliestBeginDate", ArgumentSemantic.Copy)]
		NSDate EarliestBeginDate { get; set; }
	}

	[TV (13, 0), NoMac, iOS (13, 0)]
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

	[TV (13, 0), NoMac, iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTask))]
	[DisableDefaultCtor]
	interface BGAppRefreshTask {
	}

	[TV (13, 0), NoMac, iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (BGTask))]
	[DisableDefaultCtor]
	interface BGProcessingTask {
	}

	[TV (13, 0), NoMac, iOS (13, 0)]
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

		[Export ("submitTaskRequest:error:")]
		bool Submit (BGTaskRequest taskRequest, [NullAllowed] out NSError error);

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

	[TV (17, 0), NoMac, iOS (17, 0), MacCatalyst (17, 0)]
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

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Native]
	public enum BGContinuedProcessingTaskRequestSubmissionStrategy : long {
		Fail,
		Queue,
	}

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Native]
	[Flags]
	public enum BGContinuedProcessingTaskRequestResources : long {
		Default = 0x0,
		Gpu = (1L << 0),
	}

}
