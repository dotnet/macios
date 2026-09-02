// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSViewColumnResizeEventArgs : NSNotificationEventArgs {
	// These properties need manual bindings, because they're not using constant string values,
	// they're using literal string values (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public NSTableColumn? Column {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("NSTableColumn");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<NSTableColumn> (value);
		}
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public nint OldWidth {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return 0;

			using var key = new TransientCFString ("NSOldWidth");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<Foundation.NSNumber> (value)?.NIntValue ?? 0;
		}
	}
}
#endif // !__MACCATALYST__
