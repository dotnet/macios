// Copyright 2011 Xamarin Inc. All rights reserved.

#if IOS

#nullable enable

namespace UIKit {

	public partial class UIToolbar : UIView {

		// note: we cannot autogenerate this overload and still update the (same) __mt_Items_var local
		// previously we 'lost' the managed reference to the array and this caused bug #410
		// http://bugzilla.xamarin.com/show_bug.cgi?id=410

		/// <summary>Sets the items on the toolbar, optionally animating the transition.</summary>
		/// <param name="items">The array of <see cref="UIBarButtonItem" /> instances to display on the toolbar.</param>
		/// <param name="animated">Whether to animate the transition to the new items.</param>
		[Export ("setItems:animated:")]
		public virtual void SetItems (UIBarButtonItem [] items, bool animated)
		{
			ArgumentNullException.ThrowIfNull (items);

			// must be identical the [get|set]_Items
			var nsa_items = NSArray.FromNSObjects (items);
			var nsa_itemsHandle = nsa_items.Handle;

			if (IsDirectBinding) {
				ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle_bool (this.Handle, Selector.GetHandle ("setItems:animated:"), nsa_itemsHandle, animated ? (byte) 1 : (byte) 0);
			} else {
				ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle_bool (this.SuperHandle, Selector.GetHandle ("setItems:animated:"), nsa_itemsHandle, animated ? (byte) 1 : (byte) 0);
			}
			nsa_items.Dispose ();
		}
	}
}

#endif // IOS
