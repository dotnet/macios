//
// UIDragDropSessionExtensions.cs
//
// Authors:
//   Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft
//

#if !TVOS

using System;
using ObjCRuntime;
using Foundation;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public static class UIDragDropSessionExtensions {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="session">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSProgress LoadObjects<T> (this IUIDropSession session, Action<T []> completion) where T : NSObject, INSItemProviderReading
		{
			return session.LoadObjects (new Class (typeof (T)), (v) => {
				var arr = v as T [];
				if (arr is null && v is not null) {
					arr = new T [v.Length];
					for (int i = 0; i < arr.Length; i++) {
						if (v [i] is not null)
							arr [i] = Runtime.ConstructNSObject<T> (v [i].Handle);
					}
				}

				completion (arr);
			});
		}

		/// <param name="session">The session to query.</param>
		///         <param name="type">The type of object to query about.</param>
		///         <summary>Returns <see langword="true" /> if the specified <paramref name="session" /> can instantiate items of the specified <paramref name="type" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool CanLoadObjects (this IUIDragDropSession session, Type type)
		{
			return session.CanLoadObjects (new Class (type));
		}
	}
}

#endif
