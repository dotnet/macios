// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using Accounts;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AccountsTrampolines {

	[Export<Property> ("acAccountStoreRemoveCompletionHandler", ArgumentSemantic.Copy)]
	public partial Accounts.ACAccountStoreRemoveCompletionHandler ACAccountStoreRemoveCompletionHandler { get; set; }

	[Export<Property> ("acAccountStoreSaveCompletionHandler", ArgumentSemantic.Copy)]
	public partial Accounts.ACAccountStoreSaveCompletionHandler ACAccountStoreSaveCompletionHandler { get; set; }

	[Export<Property> ("acRequestCompletionHandler", ArgumentSemantic.Copy)]
	public partial Accounts.ACRequestCompletionHandler ACRequestCompletionHandler { get; set; }
}
