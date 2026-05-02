// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSTextViewDidChangeSelectionEventArgs : NSNotificationEventArgs {
	// This property needs a manual binding, because it's not using a constant string value,
	// it's using a literal string value (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public Foundation.NSValue? OldSelectedCharacterRange {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("NSOldSelectedCharacterRange");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<Foundation.NSValue> (value);
		}
	}
}
#endif // !__MACCATALYST__
