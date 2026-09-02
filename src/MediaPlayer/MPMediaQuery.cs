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

#if !XAMCORE_5_0
#if !TVOS && !MONOMAC

using System.ComponentModel;

#nullable enable

namespace MediaPlayer {

	public partial class MPMediaQuery {
		/// <summary>Returns the media item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to retrieve.</param>
		/// <returns>The <see cref="MPMediaItem" /> at <paramref name="index" />.</returns>
		[Obsolete ("Use the 'Items' array instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public MPMediaItem GetItem (nuint index)
		{
			return Items! [(int) index];
		}

		/// <summary>Returns the item section at the specified index.</summary>
		/// <param name="index">The zero-based index of the section to retrieve.</param>
		/// <returns>The <see cref="MPMediaQuerySection" /> at <paramref name="index" />.</returns>
		[Obsolete ("Use the 'ItemSections' array instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public MPMediaQuerySection GetSection (nuint index)
		{
			return ItemSections! [(int) index];
		}

		/// <summary>Returns the media item collection at the specified index.</summary>
		/// <param name="index">The zero-based index of the collection to retrieve.</param>
		/// <returns>The <see cref="MPMediaItemCollection" /> at <paramref name="index" />.</returns>
		[Obsolete ("Use the 'Collections' array instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public MPMediaItemCollection GetCollection (nuint index)
		{
			return Collections! [(int) index];
		}
	}
}

#endif // !TVOS
#endif // !XAMCORE_5_0
