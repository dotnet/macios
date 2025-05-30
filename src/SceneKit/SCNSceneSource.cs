//
// SCNSceneSource.cs: extensions to SCNSceneSource
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright Xamarin Inc.
//

using System;
using System.Collections;
using System.Collections.Generic;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace SceneKit {
	public partial class SCNSceneSource {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="uid">To be added.</param>
		///         <summary>Gets the entry that is identified by <paramref name="uid" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSObject? GetEntryWithIdentifier<T> (string uid)
		{
			return GetEntryWithIdentifier (uid, new Class (typeof (T)));
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>Returns the list of identifiers that identify objects that belong to the specified class.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public string [] GetIdentifiersOfEntries<T> ()
		{
			var klass = new Class (typeof (T));
			string [] result = CFArray.StringArrayFromHandle (Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("identifiersOfEntriesWithClass:"), klass.Handle))!;
			GC.KeepAlive (klass);
			return result;
		}
	}
}
