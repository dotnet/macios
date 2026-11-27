//
// BackgroundTasks C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//

#nullable enable

namespace BackgroundTasks {

	/// <summary>Enumerates the errors that can occur when scheduling a background task.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("BGTaskSchedulerErrorDomain")]
	public enum BGTaskSchedulerErrorCode : long {
		/// <summary>The app or extension can't schedule background work. For example, the user disabled background refresh, the app runs in the simulator, or the extension type can't schedule background tasks.</summary>
		Unavailable = 1,
		/// <summary>There are too many pending tasks of the requested type. Cancel some existing requests and submit the request again.</summary>
		TooManyPendingTaskRequests = 2,
		/// <summary>The app isn't permitted to schedule the task. For example, the app doesn't declare the required background mode or the task identifier in its Info.plist, the task asks for unavailable resources, or the user denied background launches for the app.</summary>
		NotPermitted = 3,
		/// <summary>The system couldn't run a continued processing task request immediately. The system only returns this error when the request uses the <see cref="BGContinuedProcessingTaskRequestSubmissionStrategy.Fail" /> strategy.</summary>
		ImmediateRunIneligible = 4,
	}
}
