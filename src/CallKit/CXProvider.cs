//
// CXProvider extensions and syntax sugar
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

#if !MONOMAC
using System;
using Foundation;
using ObjCRuntime;

namespace CallKit {
	public partial class CXProvider {

		/// <typeparam name="T">The type of call action to return.</typeparam>
		///         <param name="callUuid">The identifier of the call for which to return pending call actions.</param>
		///         <summary>Returns a list of the actions of class <typeparamref name="T" /> that have yet to be completed on the call that is identified by <paramref name="callUuid" />.</summary>
		///         <returns>A list of the actions of type <typeparamref name="T" /> that have yet to be completed on the call that is identified by <paramref name="callUuid" />.</returns>
		///         <remarks>To be added.</remarks>
		public CXCallAction [] GetPendingCallActions<T> (NSUuid callUuid)
		{
			return GetPendingCallActions (new Class (typeof (T)), callUuid);
		}
	}
}
#endif // !MONOMAC
