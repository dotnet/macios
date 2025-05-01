// 
// MPMediaItem.cs: 
//
// Authors:
//   Geoff Norton.
//   Miguel de Icaza
//     
// Copyright 2011 Novell, Inc
// Copyright 2011-2012, 2014-2015 Xamarin, Inc
//

#if !TVOS && !MONOMAC

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MediaPlayer {

	public partial class MPMediaQuery {
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public MPMediaItem GetItem (nuint index)
		{
			using (var array = new NSArray (Messaging.IntPtr_objc_msgSend (Handle, Selector.GetHandle ("items"))))
				return array.GetItem<MPMediaItem> (index);
		}

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public MPMediaQuerySection GetSection (nuint index)
		{
			using (var array = new NSArray (Messaging.IntPtr_objc_msgSend (Handle, Selector.GetHandle ("itemSections"))))
				return array.GetItem<MPMediaQuerySection> (index);
		}

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public MPMediaItemCollection GetCollection (nuint index)
		{
			using (var array = new NSArray (Messaging.IntPtr_objc_msgSend (Handle, Selector.GetHandle ("collections"))))
				return array.GetItem<MPMediaItemCollection> (index);
		}
	}
}

#endif // !TVOS
