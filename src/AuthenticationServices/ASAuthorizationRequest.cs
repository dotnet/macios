//
// ASAuthorization.cs
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright 2019 Microsoft Corporation
//

#nullable enable

namespace AuthenticationServices {
	public partial class ASAuthorizationRequest {
		public T? GetProvider<T> () where T : NSObject, IASAuthorizationProvider => Runtime.GetINativeObject<T> (_Provider, false);
	}
}
