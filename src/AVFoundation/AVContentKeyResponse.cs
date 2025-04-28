
//
// Copyright 2019 Microsoft Corp
//
// Authors:
//   Manuel de la Pena mandel@microsoft.com 
//
using Foundation;
using ObjCRuntime;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {

	public partial class AVContentKeyResponse {

		/// <param name="fairPlayStreamingKeyResponseData">The Fair Play key data from which to create a response.</param>
		///         <summary>Creates and returns a new response object from the provided key data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AVContentKeyResponse Create (NSData fairPlayStreamingKeyResponseData) => Create (fairPlayStreamingKeyResponseData, AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData);

		public static AVContentKeyResponse Create (NSData data, AVContentKeyResponseDataType dataType = AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData)
		{
			switch (dataType) {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'AVContentKeyResponse._InitWithAuthorizationToken(NSData)' is only supported on: 'ios' 13.0 and later, 'tvos' 13.0 and later.
			case AVContentKeyResponseDataType.AuthorizationTokenData:
				return AVContentKeyResponse._CreateWithAuthorizationToken (data);
#pragma warning restore CA1416
			case AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData:
				return AVContentKeyResponse._CreateWithFairPlayStreamingKeyResponseData (data);
			default:
				throw new ArgumentOutOfRangeException (nameof (dataType), dataType, "Invalid enum value.");
			}
		}
	}
}
