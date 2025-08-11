// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct TypeInfo {

	/// <summary>
	/// True if the type needs to use a stret call.
	/// </summary>
	public bool NeedsStret { get; init; }

	/// <summary>
	/// The events for the type.
	/// </summary>
	public ImmutableArray<(string Name, string? EventArgs)> Events { get; init; } = [];

	/// <summary>
	/// Tries to get the event data for a given member symbol.
	/// </summary>
	/// <param name="member">The member symbol.</param>
	/// <param name="context">The root context.</param>
	/// <param name="eventData">When this method returns, contains the event data if the member is an event; otherwise, null.</param>
	/// <returns><c>true</c> if the member is an event and event data was retrieved; otherwise, <c>false</c>.</returns>
	static bool TryGetEventForMember (ISymbol member, RootContext context,
		[NotNullWhen (true)] out (string Name, string? EventArgs)? eventData)
	{
		eventData = null;
		// if the member is not a method, we will return false
		if (member is not IMethodSymbol methodSymbol)
			return false;

		// if the method does not have a ExportAttribute, we will skip it
		if (!methodSymbol.HasAttribute (AttributesNames.ExportMethodAttribute))
			return false;

		// at this point we need to know if the method is a event or not, if it is we will add it to the list
		if (!Method.TryCreate (methodSymbol, context, out var method) || !method.Value.IsEvent)
			return false;

		// we need to calculate the event args type, if any.
		string? eventArgs = method switch { { ExportMethodData.EventArgsType.IsNullOrDefault: false }
												// get the type string for the event args type
												=> method.Value.ExportMethodData.EventArgsType.GetIdentifierSyntax ().ToString (), { ExportMethodData.EventArgsTypeName: not null }
																																	   // use the given type name, we will generate those too
																																	   => method.Value.ExportMethodData.EventArgsTypeName,
			_ => null,
		};
		eventData = (method.Value.Name, eventArgs);
		return true;
	}

	/// <summary>
	/// Gets the events for a given interface symbol.
	/// </summary>
	/// <param name="symbol">The interface symbol.</param>
	/// <param name="context">The root context.</param>
	/// <returns>An immutable array of tuples containing the event name and its event arguments type name.</returns>
	static ImmutableArray<(string Name, string? ReturnType)> GetInterfaceEvents (ITypeSymbol symbol, RootContext context)
	{
		var parentMethodsBucket = ImmutableArray.CreateBuilder<(string Name, string? EventArgs)> ();
		// add the events for the current symbol
		foreach (var member in symbol.GetMembers ()) {
			if (TryGetEventForMember (member, context, out var data)) {
				parentMethodsBucket.Add (data.Value);
			}
		}

		// add the events from the parent interfaces
		foreach (var i in symbol.AllInterfaces) {
			foreach (var member in i.GetMembers ()) {
				if (TryGetEventForMember (member, context, out var data)) {
					parentMethodsBucket.Add (data.Value);
				}
			}
		}
		return parentMethodsBucket.ToImmutable ();
	}

	internal TypeInfo (ITypeSymbol symbol, RootContext context) : this (symbol)
	{
		NeedsStret = symbol.NeedsStret (context.Compilation);
		if (symbol.IsProtocol ()) {
			Events = GetInterfaceEvents (symbol, context);
		}
	}

	/// <summary>
	/// Creates a <see cref="TypeInfo"/> for a delegate proxy (trampoline).
	/// </summary>
	/// <param name="delegateType">The <see cref="TypeInfo"/> of the delegate.</param>
	/// <returns>A new <see cref="TypeInfo"/> representing the delegate proxy, or the original <paramref name="delegateType"/> if it's not a delegate.</returns>
	public static TypeInfo CreateDelegateProxy (TypeInfo delegateType)
	{
		if (!delegateType.IsDelegate)
			return delegateType;

		// build a new type info that is a delegate proxy
		var trampolineName = Nomenclator.GetTrampolineName (delegateType);
		var proxyName = Nomenclator.GetTrampolineClassName (
			trampolineName, Nomenclator.TrampolineClassType.StaticBridgeClass);

		var trampolineInfo = new TypeInfo (
			name: $"ObjCRuntime.Trampolines.{proxyName}",
			specialType: SpecialType.None,
			isNullable: false,
			isBlittable: false,
			isSmartEnum: false,
			isArray: false,
			isReferenceType: true,
			isStruct: false
		) {
			Delegate = null,
			EnumUnderlyingType = null,
			IsGenericType = false,
			IsTask = false,
			TypeArguments = []
		};
		return trampolineInfo;
	}

}
