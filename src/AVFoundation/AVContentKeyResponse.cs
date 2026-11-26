
//
// Copyright 2019 Microsoft Corp
//
// Authors:
//   Manuel de la Pena mandel@microsoft.com 
//
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {

	public partial class AVContentKeyResponse {

		/// <param name="fairPlayStreamingKeyResponseData">The Fair Play key data from which to create a response.</param>
		/// <summary>Creates and returns a new response object from the provided key data.</summary>
		/// <returns>A new <see cref="AVContentKeyResponse" /> created from the specified key data.</returns>
		public static AVContentKeyResponse Create (NSData fairPlayStreamingKeyResponseData) => Create (fairPlayStreamingKeyResponseData, AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData);

		public static AVContentKeyResponse Create (NSData data, AVContentKeyResponseDataType dataType = AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData)
		{
			switch (dataType) {
			case AVContentKeyResponseDataType.AuthorizationTokenData:
				return AVContentKeyResponse._CreateWithAuthorizationToken (data);
			case AVContentKeyResponseDataType.FairPlayStreamingKeyResponseData:
				return AVContentKeyResponse._CreateWithFairPlayStreamingKeyResponseData (data);
			default:
				throw new ArgumentOutOfRangeException (nameof (dataType), dataType, "Invalid enum value.");
			}
		}
	}
}
