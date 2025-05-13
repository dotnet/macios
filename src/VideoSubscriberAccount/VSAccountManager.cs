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

		/// <param name="accessOptions">If not empty, may contain the key <see cref="VideoSubscriberAccount.VSCheckAccessOptionKeys" />.</param>
		///         <param name="completionHandler">Called by the system with the results of the permission check.</param>
		///         <summary>Checks whether the user has provided permission for the app to access their subscription information.</summary>
		///         <remarks>To be added.</remarks>
		public void CheckAccessStatus (VSAccountManagerAccessOptions accessOptions, Action<VSAccountAccessStatus, NSError> completionHandler)
		{
			if (accessOptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (accessOptions));
			if (completionHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (completionHandler));

			CheckAccessStatus (accessOptions.Dictionary, completionHandler);
		}

		/// <param name="accessOptions">To be added.</param>
		///         <summary>Asynchronously checks whether the user has provided permission for the app to access their subscription information.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public Task<VSAccountAccessStatus> CheckAccessStatusAsync (VSAccountManagerAccessOptions accessOptions)
		{
			if (accessOptions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (accessOptions));

			return CheckAccessStatusAsync (accessOptions.Dictionary);
		}
	}
}

#endif
