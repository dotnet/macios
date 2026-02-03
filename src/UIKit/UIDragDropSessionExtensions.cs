//
// UIDragDropSessionExtensions.cs
//
// Authors:
//   Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft
//

#if !TVOS

#nullable enable

namespace UIKit {

	/// <summary>Contains extension methods for working with drag-and-drop sessions.</summary>
	public static class UIDragDropSessionExtensions {

		/// <summary>Loads objects of the specified type from the drop session and invokes the completion handler when done.</summary>
		/// <typeparam name="T">The type of objects to load from the drop session.</typeparam>
		/// <param name="session">The drop session to load objects from.</param>
		/// <param name="completion">The completion handler to invoke with the loaded objects.</param>
		/// <returns>An <see cref="NSProgress" /> object that can be used to track the loading progress.</returns>
		public static NSProgress LoadObjects<T> (this IUIDropSession session, Action<T []?> completion) where T : NSObject, INSItemProviderReading
		{
			// allowing null for 'completion' doesn't make much sense, but we have tests verifying that it's allowed,
			// so make the implementation work with a null 'completion' handler.
			ArgumentNullException.ThrowIfNull (session);

			return session.LoadObjects (new Class (typeof (T)), (v) => {
				var arr = v as T [];
				if (arr is null && v is not null) {
					arr = new T [v.Length];
					for (int i = 0; i < arr.Length; i++) {
						if (v [i] is not null)
							arr [i] = Runtime.ConstructNSObject<T> (v [i].Handle)!;
					}
				}
				GC.KeepAlive (v);

				if (completion is not null)
					completion (arr);
			});
		}

		/// <summary>Returns <see langword="true" /> if the specified <paramref name="session" /> can instantiate items of the specified <paramref name="type" />.</summary>
		/// <param name="session">The session to query.</param>
		/// <param name="type">The type of object to query about.</param>
		/// <returns><see langword="true" /> if the session can load objects of the specified type; otherwise, <see langword="false" />.</returns>
		public static bool CanLoadObjects (this IUIDragDropSession session, Type type)
		{
			ArgumentNullException.ThrowIfNull (session);
			ArgumentNullException.ThrowIfNull (type);

			return session.CanLoadObjects (new Class (type));
		}
	}
}

#endif
