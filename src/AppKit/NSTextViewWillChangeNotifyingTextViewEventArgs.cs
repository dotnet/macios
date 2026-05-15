// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSTextViewWillChangeNotifyingTextViewEventArgs : NSNotificationEventArgs {
	// These properties need manual bindings, because they're not using constant string values,
	// they're using literal string values (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public NSTextView? OldView {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("NSOldNotifyingTextView");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<NSTextView> (value);
		}
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public NSTextView? NewView {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("NSNewNotifyingTextView");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<NSTextView> (value);
		}
	}
}
#endif // !__MACCATALYST__
