// Copyright 2019 Microsoft Corporation

using CoreFoundation;

namespace ExecutionPolicy {

	[Native]
	public enum EPDeveloperToolStatus : long {
		NotDetermined = 0,
		Restricted,
		Denied,
		Authorized,
	}

	[Native]
	[ErrorDomain ("EPErrorDomain")]
	public enum EPError : long {
		Generic = 1,
		NotADeveloperTool,
	}

	[BaseType (typeof (NSObject))]
	interface EPDeveloperTool {

		[Export ("authorizationStatus")]
		EPDeveloperToolStatus AuthorizationStatus { get; }

		[Export ("requestDeveloperToolAccessWithCompletionHandler:")]
		void RequestDeveloperToolAccess (Action<bool> handler);
	}

	[BaseType (typeof (NSObject))]
	interface EPExecutionPolicy {

		[Export ("addPolicyExceptionForURL:error:")]
		bool AddPolicyException (NSUrl url, [NullAllowed] out NSError error);
	}
}
