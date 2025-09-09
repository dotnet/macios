//
// SystemConfigurationException.cs: SystemConfiguration error handling
//
// Authors:
//    Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;

namespace SystemConfiguration {

	/// <summary>An exception relating to network reachability. The cause of the exception is specified by the <see cref="SystemConfiguration.SystemConfigurationException.StatusErrorCode" /> property.</summary>
	///     <remarks>To be added.</remarks>
	public class SystemConfigurationException : Exception {
		/// <param name="statusErrorCode">To be added.</param>
		///         <summary>Creates a new <see cref="SystemConfiguration.SystemConfigurationException" /> wrapping the <paramref name="statusErrorCode" />.</summary>
		///         <remarks>To be added.</remarks>
		public SystemConfigurationException (StatusCode statusErrorCode)
			: base (StatusCodeError.GetErrorDescription (statusErrorCode))
		{
			StatusErrorCode = statusErrorCode;
		}

		/// <summary>The <see cref="SystemConfiguration.StatusCode" /> wrapped in this <see cref="SystemConfiguration.SystemConfigurationException" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public StatusCode StatusErrorCode { get; private set; }

		internal static SystemConfigurationException FromMostRecentCall ()
		{
			var code = StatusCodeError.SCError ();
			return new SystemConfigurationException (code);
		}
	}
}
