#nullable enable

using System;
using ObjCRuntime;

namespace SecurityInterface {

	/// <summary>A view that displays a lock icon for controlling access to a privileged operation.</summary>
	public partial class SFAuthorizationView {

		/// <summary>Sets the authorization right string to check for.</summary>
		/// <param name="authorizationString">The authorization right name as a UTF-8 string.</param>
		public void SetAuthorizationString (string authorizationString)
		{
			if (authorizationString is null)
				ThrowHelper.ThrowArgumentNullException (nameof (authorizationString));
			using var str = new TransientString (authorizationString);
			_SetAuthorizationString (str);
		}

		/// <summary>Gets or sets the authorization rights to check for.</summary>
		public AuthorizationRights? AuthorizationRightsSet {
			get => AuthorizationRights.FromHandle (_AuthorizationRights);
			set {
				_SetAuthorizationRights (value?.Handle ?? NativeHandle.Zero);
				GC.KeepAlive (value);
			}
		}
	}
}
