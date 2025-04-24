//
// StatusCode.cs: SystemConfiguration error handling
//
// Authors:
//    Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012, 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace SystemConfiguration {

	// https://developer.apple.com/library/mac/#documentation/SystemConfiguration/Reference/SystemConfiguration_Utilities/Reference/reference.html
	/// <summary>Provides access to a text description associated with a <see cref="SystemConfiguration.StatusCode" />.</summary>
	///     <remarks>To be added.</remarks>
	public static class StatusCodeError {
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern internal static StatusCode /* int */ SCError ();

		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static IntPtr /* const char* */ SCErrorString (int code);

		/// <param name="statusCode">To be added.</param>
		///         <summary>Description for the status code.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? GetErrorDescription (StatusCode statusCode)
		{
			var ptr = SCErrorString ((int) statusCode);
			return Marshal.PtrToStringAnsi (ptr);
		}
	}
}
