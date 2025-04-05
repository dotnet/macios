//
// CNContactStore.cs:
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace Contacts {
	public partial class CNContactStore {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="identifier">To be added.</param>
		///         <param name="keys">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Retrieves the <see cref="T:Contacts.CNContact" /> with the specified <paramref name="identifier" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CNContact? GetUnifiedContact<T> (string identifier, T [] keys, out NSError? error)
			where T : INSObjectProtocol, INSSecureCoding, INSCopying
		{
			using (var array = NSArray.From<T> (keys))
				return GetUnifiedContact (identifier, array, out error);
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="predicate">To be added.</param>
		///         <param name="keys">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Retrieves the unified <see cref="T:Contacts.CNContact" /> objects, populated with data for <paramref name="keys" />, for which the <paramref name="predicate" /> returns <see langword="true" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CNContact []? GetUnifiedContacts<T> (NSPredicate predicate, T [] keys, out NSError? error)
			where T : INSObjectProtocol, INSSecureCoding, INSCopying
		{
			using (var array = NSArray.From<T> (keys))
				return GetUnifiedContacts (predicate, array, out error);
		}

#if MONOMAC
		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="keys">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSObject? GetUnifiedMeContact<T> (T [] keys, out NSError? error)
			where T : INSObjectProtocol, INSSecureCoding, INSCopying
		{
			using (var array = NSArray.From<T> (keys))
				return GetUnifiedMeContact (array, out error);
		}
#endif

	}
}
