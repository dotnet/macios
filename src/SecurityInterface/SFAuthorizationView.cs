#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	/// <summary>A view that displays a lock icon for controlling access to a privileged operation.</summary>
	public partial class SFAuthorizationView {

		/// <summary>Sets the authorization right string to check for.</summary>
		/// <param name="authorizationString">The authorization right name as a UTF-8 string.</param>
		public void SetAuthorizationString (string authorizationString)
		{
			ArgumentNullException.ThrowIfNull (authorizationString);
			using var str = new TransientString (authorizationString);
			_SetAuthorizationString (str);
		}

		/// <summary>Gets or sets the authorization rights to check for.</summary>
		[DisallowNull]
		public AuthorizationRights? AuthorizationRights {
			get => Security.AuthorizationRights.FromHandle (_AuthorizationRights);
			set {
				ArgumentNullException.ThrowIfNull (value);
				_SetAuthorizationRights (value.GetCheckedHandle ());
				GC.KeepAlive (value);
			}
		}
	}
}
