// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.ComponentModel;

#nullable enable

namespace CoreML;

#if !XAMCORE_5_0 && !__TVOS__
partial class MLModelCollection : NSObject {
	/// <summary>This property always returns <see langword="null" />.</summary>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This property always returns null.")]
	public static NSString? DidChangeNotification {
		get {
			return null;
		}
	}

	public static partial class Notifications {
		/// <summary>This method does nothing, and only returns a placeholder instance.</summary>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This method does nothing.")]
		public static NSObject ObserveDidChange (EventHandler<NSNotificationEventArgs> handler)
		{
			return new NSObject ();
		}

		/// <summary>This method does nothing, and only returns a placerholder instance.</summary>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This method does nothing.")]
		public static NSObject ObserveDidChange (NSObject objectToObserve, EventHandler<NSNotificationEventArgs> handler)
		{
			return new NSObject ();
		}
	}
}
#endif // !XAMCORE_5_0 && !__TVOS__

