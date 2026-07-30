//
// StatusCode.cs: SystemConfiguration error handling
//
// Authors:
//    Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012, 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace SystemConfiguration {

	// https://developer.apple.com/library/mac/#documentation/SystemConfiguration/Reference/SystemConfiguration_Utilities/Reference/reference.html
	/// <summary>Provides access to a text description associated with a <see cref="StatusCode" />.</summary>
	public static class StatusCodeError {
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern internal static StatusCode /* int */ SCError ();

		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static IntPtr /* const char* */ SCErrorString (int code);

		/// <summary>Returns the human-readable description for the specified status code.</summary>
		/// <param name="statusCode">The <see cref="StatusCode" /> to describe.</param>
		/// <returns>A string containing the description of the status code, or <see langword="null" /> if no description is available.</returns>
		public static string? GetErrorDescription (StatusCode statusCode)
		{
			var ptr = SCErrorString ((int) statusCode);
			return Marshal.PtrToStringAnsi (ptr);
		}
	}
}
