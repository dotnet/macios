// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSWorkspaceFileOperationEventArgs : NSNotificationEventArgs {
	// This property needs a manual binding, because it's not using a constant string value,
	// it's using a literal string value (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public nint FileType {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return 0;

			using var key = new TransientCFString ("NSOperationNumber");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<Foundation.NSNumber> (value)?.NIntValue ?? 0;
		}
	}
}
#endif // !__MACCATALYST__
