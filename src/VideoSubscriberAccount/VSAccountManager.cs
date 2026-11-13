//
// VSAccountManager extensions & syntax sugar
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

#if !MONOMAC && !__MACCATALYST__

using System.Threading.Tasks;

namespace VideoSubscriberAccount {
	public partial class VSAccountManager {

		/// <summary>Checks whether the user has provided permission for the app to access their subscription information.</summary>
		/// <param name="accessOptions">If not empty, may specify whether the user should be asked for access permission.</param>
		/// <param name="completionHandler">Called by the system with the results of the permission check.</param>
		public void CheckAccessStatus (VSAccountManagerAccessOptions accessOptions, Action<VSAccountAccessStatus, NSError> completionHandler)
		{
			ArgumentNullException.ThrowIfNull (accessOptions);
			ArgumentNullException.ThrowIfNull (completionHandler);

			CheckAccessStatus (accessOptions.Dictionary, completionHandler);
		}

		/// <summary>Asynchronously checks whether the user has provided permission for the app to access their subscription information.</summary>
		/// <param name="accessOptions">If not empty, may specify whether the user should be asked for access permission.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the current <see cref="VSAccountAccessStatus" />.</returns>
		public Task<VSAccountAccessStatus> CheckAccessStatusAsync (VSAccountManagerAccessOptions accessOptions)
		{
			ArgumentNullException.ThrowIfNull (accessOptions);

			return CheckAccessStatusAsync (accessOptions.Dictionary);
		}
	}
}

#endif
