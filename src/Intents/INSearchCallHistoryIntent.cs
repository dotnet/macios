//
// INSearchCallHistoryIntent.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if !__MACOS__
#if !TVOS

#nullable enable

namespace Intents {
	public partial class INSearchCallHistoryIntent {

		/// <summary>Gets a Boolean value that indicates whether to search for unseen calls.</summary>
		/// <value><see langword="true" /> to search for unseen calls, <see langword="false" /> to search for seen calls, or <see langword="null" /> if not specified.</value>
		public bool? Unseen {
			get { return WeakUnseen?.BoolValue; }
		}
	}
}
#endif
#endif // __MACOS__
