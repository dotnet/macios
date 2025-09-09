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

using System;
using System.Threading.Tasks;
using Foundation;

namespace VideoSubscriberAccount {
	public partial class VSAccountManager {

		/// <summary>Checks whether the user has provided permission for the app to access their subscription information.</summary>
		/// <param name="accessOptions">If not empty, may specify whether the user should be asked for access permission.</param>
		/// <param name="completionHandler">Called by the system with the results of the permission check.</param>
		public void CheckAccessStatus (VSAccountManagerAccessOptions accessOptions, Action<VSAccountAccessStatus, NSError> completionHandler)
		{
			if (accessOptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (accessOptions));
			if (completionHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (completionHandler));

			CheckAccessStatus (accessOptions.Dictionary, completionHandler);
		}

		/// <summary>Asynchronously checks whether the user has provided permission for the app to access their subscription information.</summary>
		/// <param name="accessOptions">To be added.</param>
		/// <returns>To be added.</returns>
		public Task<VSAccountAccessStatus> CheckAccessStatusAsync (VSAccountManagerAccessOptions accessOptions)
		{
			if (accessOptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (accessOptions));

			return CheckAccessStatusAsync (accessOptions.Dictionary);
		}
	}
}

#endif
