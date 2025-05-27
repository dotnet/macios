// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CloudKit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class ContactsTrampolines {

	[Export ("cnContactStoreListContactsHandler", ArgumentSemantic.Copy)]
	public partial Contacts.CNContactStoreListContactsHandler CNContactStoreListContactsHandler { get; set; }

	[Export ("cnContactStoreRequestAccessHandler", ArgumentSemantic.Copy)]
	public partial Contacts.CNContactStoreRequestAccessHandler CNContactStoreRequestAccessHandler { get; set; }
}
