// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CarPlay;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CarPlayTrampolines {
	
	[Export<Property> ("cpBarButtonHandler", ArgumentSemantic.Copy)]
	public partial CarPlay.CPBarButtonHandler CPBarButtonHandler { get; set; }

	[Export<Property> ("cpListImageRowItemHandler", ArgumentSemantic.Copy)]
	public partial CarPlay.CPListImageRowItemHandler CPListImageRowItemHandler { get; set; }

	[Export<Property> ("cpSearchTemplateDelegateUpdateHandler", ArgumentSemantic.Copy)]
	public partial CarPlay.CPSearchTemplateDelegateUpdateHandler CPSearchTemplateDelegateUpdateHandler { get; set; }

	[Export<Property> ("cpSelectableListItemHandler", ArgumentSemantic.Copy)]
	public partial CarPlay.CPSelectableListItemHandler CPSelectableListItemHandler { get; set; }
}
