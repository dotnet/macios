//
// DeviceCheck C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation.
//

namespace DeviceCheck {

	[MacCatalyst (13, 1)]
	[ErrorDomain ("DCErrorDomain")]
	[Native]
	public enum DCError : long {
		/// <summary>An unknown system failure occurred.</summary>
		UnknownSystemFailure,
		/// <summary>The DeviceCheck feature is not supported on this device.</summary>
		FeatureUnsupported,
		InvalidInput,
		InvalidKey,
		ServerUnavailable,
	}

	/// <param name="token">The generated token.</param>
	///     <param name="error">The error that was encountered, or <see langword="null" /> if no error occurred.</param>
	///     <summary>A completion handler for DeviceCheck token generation.</summary>
	[MacCatalyst (13, 1)]
	delegate void DCDeviceGenerateTokenCompletionHandler ([NullAllowed] NSData token, [NullAllowed] NSError error);

	/// <summary>A device representation for associating users and devices with tracking data.</summary>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // From the documentation it seems the only way to create a usable instance is to use the static CurrentDevice property.
	[BaseType (typeof (NSObject))]
	interface DCDevice {

		[Static]
		[Export ("currentDevice")]
		DCDevice CurrentDevice { get; }

		/// <summary>Gets a Boolean value that tells whether the <see cref="DeviceCheck.DCDevice.CurrentDevice" /> supports the DeviceCheck API.</summary>
		/// <value><see langword="true" /> if the current device supports the DeviceCheck API; otherwise, <see langword="false" />.</value>
		[Export ("supported")]
		bool Supported { [Bind ("isSupported")] get; }

		[Async (XmlDocs = """
			<summary>Generates an identification token for <see cref="DeviceCheck.DCDevice.CurrentDevice" /> and invokes a handler after the operation completes.</summary>
			<returns>
			  <para>A task that represents the asynchronous GenerateToken operation. The value of the TResult parameter is a DeviceCheck.DCDeviceGenerateTokenCompletionHandler.</para>
			</returns>
			<remarks>
			  <para copied="true">The GenerateTokenAsync method can be used with C# async by returning control to the caller with a Task that represents the operation.</para>
			</remarks>
			""")]
		[Export ("generateTokenWithCompletionHandler:")]
		void GenerateToken (DCDeviceGenerateTokenCompletionHandler completion);
	}

	[iOS (14, 0)]
	[TV (15, 0)]
	[MacCatalyst (14, 5)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface DCAppAttestService {

		[Static]
		[Export ("sharedService")]
		DCAppAttestService SharedService { get; }

		[Export ("supported")]
		bool Supported { [Bind ("isSupported")] get; }

		[Async]
		[Export ("generateKeyWithCompletionHandler:")]
		void GenerateKey (Action<string, NSError> completionHandler);

		[Async]
		[Export ("attestKey:clientDataHash:completionHandler:")]
		void AttestKey (string keyId, NSData clientDataHash, Action<NSData, NSError> completionHandler);

		[Async]
		[Export ("generateAssertion:clientDataHash:completionHandler:")]
		void GenerateAssertion (string keyId, NSData clientDataHash, Action<NSData, NSError> completionHandler);
	}
}
