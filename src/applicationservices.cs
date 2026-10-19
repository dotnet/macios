// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Foundation;
using ObjCRuntime;

namespace ApplicationServices {
	/// <summary>Error codes returned by the accessibility client APIs.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 0)]
	enum AXError : int {
		/// <summary>The operation succeeded.</summary>
		Success = 0,
		/// <summary>A system error occurred.</summary>
		Failure = -25200,
		/// <summary>An argument is invalid.</summary>
		IllegalArgument = -25201,
		/// <summary>The accessibility element is invalid.</summary>
		InvalidUIElement = -25202,
		/// <summary>The accessibility observer is invalid.</summary>
		InvalidUIElementObserver = -25203,
		/// <summary>The application is busy, unresponsive, or could not be contacted.</summary>
		CannotComplete = -25204,
		/// <summary>The element does not support the attribute.</summary>
		AttributeUnsupported = -25205,
		/// <summary>The element does not support the action.</summary>
		ActionUnsupported = -25206,
		/// <summary>The element does not support the notification.</summary>
		NotificationUnsupported = -25207,
		/// <summary>The application does not implement the operation.</summary>
		NotImplemented = -25208,
		/// <summary>The notification is already registered.</summary>
		NotificationAlreadyRegistered = -25209,
		/// <summary>The notification is not registered.</summary>
		NotificationNotRegistered = -25210,
		/// <summary>Access to the accessibility API is disabled.</summary>
		ApiDisabled = -25211,
		/// <summary>The requested value does not exist.</summary>
		NoValue = -25212,
		/// <summary>The element does not support the parameterized attribute.</summary>
		ParameterizedAttributeUnsupported = -25213,
		/// <summary>The operation requires greater precision.</summary>
		NotEnoughPrecision = -25214,
	}

	/// <summary>The structure stored in an accessibility value.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 11)]
	enum AXValueType : uint {
		/// <summary>An unsupported structure.</summary>
		Illegal = 0,
		/// <summary>A Core Graphics point.</summary>
		CGPoint = 1,
		/// <summary>A Core Graphics size.</summary>
		CGSize = 2,
		/// <summary>A Core Graphics rectangle.</summary>
		CGRect = 3,
		/// <summary>A Core Foundation range.</summary>
		CFRange = 4,
		/// <summary>An accessibility error code.</summary>
		AXError = 5,
	}

	/// <summary>Options for retrieving multiple accessibility attributes.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 4)]
	[System.Flags]
	enum AXCopyMultipleAttributeOptions : uint {
		/// <summary>Return individual errors as accessibility values in the result array.</summary>
		None = 0,
		/// <summary>Stop as soon as an attribute cannot be retrieved.</summary>
		StopOnError = 1,
	}

	/// <summary>Keys for the accessibility trust-check options dictionary.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 9)]
	[Static]
	interface AXTrustedCheckOptions {
		/// <summary>A Boolean value indicating whether to prompt the user to grant accessibility access.</summary>
		[Field ("kAXTrustedCheckOptionPrompt")]
		NSString Prompt { get; }
	}
}
