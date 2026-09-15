//
// SystemConfigurationException.cs: SystemConfiguration error handling
//
// Authors:
//    Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace SystemConfiguration {

	/// <summary>An exception relating to network reachability. The cause of the exception is specified by the <see cref="StatusErrorCode" /> property.</summary>
	public class SystemConfigurationException : Exception {
		/// <summary>Creates a new <see cref="SystemConfigurationException" /> wrapping the specified <paramref name="statusErrorCode" />.</summary>
		/// <param name="statusErrorCode">The <see cref="StatusCode" /> that caused this exception.</param>
		public SystemConfigurationException (StatusCode statusErrorCode)
			: base (StatusCodeError.GetErrorDescription (statusErrorCode))
		{
			StatusErrorCode = statusErrorCode;
		}

		/// <summary>Gets the <see cref="StatusCode" /> that describes the cause of this exception.</summary>
		/// <value>The status code associated with this exception.</value>
		public StatusCode StatusErrorCode { get; private set; }

		internal static SystemConfigurationException FromMostRecentCall ()
		{
			var code = StatusCodeError.SCError ();
			return new SystemConfigurationException (code);
		}
	}
}
