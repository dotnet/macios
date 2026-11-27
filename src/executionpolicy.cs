// Copyright 2019 Microsoft Corporation

using CoreFoundation;

namespace ExecutionPolicy {

	/// <summary>Enumerates whether a process has developer tool privileges.</summary>
	[MacCatalyst (16, 0)]
	[Native]
	public enum EPDeveloperToolStatus : long {
		/// <summary>The user hasn't decided whether the process gets developer tool privileges.</summary>
		NotDetermined = 0,
		/// <summary>The process can't get developer tool privileges, and the user can't change that.</summary>
		Restricted,
		/// <summary>The user denied developer tool privileges to the process.</summary>
		Denied,
		/// <summary>The process has developer tool privileges.</summary>
		Authorized,
	}

	/// <summary>Enumerates the errors that the ExecutionPolicy framework reports.</summary>
	[MacCatalyst (16, 0)]
	[Native]
	[ErrorDomain ("EPErrorDomain")]
	public enum EPError : long {
		/// <summary>A general error occurred.</summary>
		Generic = 1,
		/// <summary>The process isn't a developer tool.</summary>
		NotADeveloperTool,
	}

	[MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface EPDeveloperTool {

		[Export ("authorizationStatus")]
		EPDeveloperToolStatus AuthorizationStatus { get; }

		[Export ("requestDeveloperToolAccessWithCompletionHandler:")]
		void RequestDeveloperToolAccess (Action<bool> handler);
	}

	[MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface EPExecutionPolicy {

		[Export ("addPolicyExceptionForURL:error:")]
		bool AddPolicyException (NSUrl url, [NullAllowed] out NSError error);
	}
}
