// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Tests.Extensions;

/// <summary>
/// Helper extensions for binding tests.
/// </summary>
static class BindingExtensions {

	public static bool TryGetEnumValue (this Binding self, string selector, out EnumMember? enumMember)
	{
		var dict = self.EnumMembers.ToDictionary (
			keySelector: p => p.Selector!,
			elementSelector: p => new EnumMember? (p));
		return dict.TryGetValue (selector, out enumMember);
	}

	public static bool TryGetProperty (this Binding self, string selector, out Property? property)
	{
		var dict = self.Properties.ToDictionary (
			keySelector: p => p.Selector!,
			elementSelector: p => new Property? (p));
		return dict.TryGetValue (selector, out property);
	}

	public static bool TryGetConstructor (this Binding self, string selector, out Constructor? constructor)
	{
		var dict = self.Constructors.ToDictionary (
			keySelector: p => p.Selector!,
			elementSelector: p => new Constructor? (p));
		return dict.TryGetValue (selector, out constructor);
	}

	public static bool TryGetMethod (this Binding self, string selector, out Method? method)
	{
		var dict = self.Methods.ToDictionary (
			keySelector: p => p.Selector!,
			elementSelector: p => new Method? (p));
		return dict.TryGetValue (selector, out method);
	}

	public static bool TryGetEvent (this Binding self, string selector, out Event? @event)
	{
		var dict = self.Events.ToDictionary (
			keySelector: p => p.Name,
			elementSelector: p => new Event? (p));
		return dict.TryGetValue (selector, out @event);
	}
}
