// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__MACCATALYST__
namespace AppKit;

partial class NSTextAlternativesSelectedAlternativeStringEventArgs : NSNotificationEventArgs {
	// This property needs a manual binding, because it's not using a constant string value,
	// it's using a literal string value (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public string? AlternativeString {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("NSAlternativeString");
			var value = userinfo.LowlevelObjectForKey (key);
			return CoreFoundation.CFString.FromHandle (value);
		}
	}
}
#endif // !__MACCATALYST__
