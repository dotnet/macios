// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSViewColumnMoveEventArgs : NSNotificationEventArgs {
	// These properties need manual bindings, because they're not using constant string values,
	// they're using literal string values (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public nint OldColumn {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return 0;

			using var key = new TransientCFString ("NSOldColumn");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<Foundation.NSNumber> (value)?.NIntValue ?? 0;
		}
	}

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public nint NewColumn {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return 0;

			using var key = new TransientCFString ("NSNewColumn");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<Foundation.NSNumber> (value)?.NIntValue ?? 0;
		}
	}
}
#endif // !__MACCATALYST__
