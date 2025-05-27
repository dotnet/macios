// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AuthenticationServices;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AuthenticationServicesTrampolines {

	[Export<Property> ("asCredentialIdentityStoreCompletionHandler", ArgumentSemantic.Copy)]
	public partial AuthenticationServices.ASCredentialIdentityStoreCompletionHandler ASCredentialIdentityStoreCompletionHandler { get; set; }

	[Export<Property> ("asCredentialIdentityStoreGetCredentialIdentitiesHandler", ArgumentSemantic.Copy)]
	public partial AuthenticationServices.ASCredentialIdentityStoreGetCredentialIdentitiesHandler ASCredentialIdentityStoreGetCredentialIdentitiesHandler { get; set; }

	[Export<Property> ("asCredentialProviderExtensionRequestCompletionHandler", ArgumentSemantic.Copy)]
	public partial AuthenticationServices.ASCredentialProviderExtensionRequestCompletionHandler ASCredentialProviderExtensionRequestCompletionHandler { get; set; }

	[Export<Property> ("asSettingsHelperRequestToTurnOnCredentialProviderExtensionCallback", ArgumentSemantic.Copy)]
	public partial AuthenticationServices.ASSettingsHelperRequestToTurnOnCredentialProviderExtensionCallback ASSettingsHelperRequestToTurnOnCredentialProviderExtensionCallback { get; set; }

	[Export<Property> ("asWebAuthenticationSessionCompletionHandler", ArgumentSemantic.Copy)]
	public partial AuthenticationServices.ASWebAuthenticationSessionCompletionHandler ASWebAuthenticationSessionCompletionHandler { get; set; }
}
