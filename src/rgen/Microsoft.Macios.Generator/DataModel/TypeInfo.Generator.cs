// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
	public ImmutableArray<EventInfo> Events { get; init; } = [];

	/// <summary>
	/// Tries to get the event data for a given member symbol.
	/// </summary>
	/// <param name="typeNamespace">The namespace of the types whose events we are collecting.</param>
	/// <param name="member">The member symbol.</param>
	/// <param name="context">The root context.</param>
	/// <param name="eventInfo">When this method returns, contains the event data if the member is an event; otherwise, null.</param>
	/// <returns><c>true</c> if the member is an event and event data was retrieved; otherwise, <c>false</c>.</returns>
	static bool TryGetEventForMember (string typeNamespace, ISymbol member, RootContext context,
		[NotNullWhen (true)] out EventInfo? eventInfo)
	{
		eventInfo = null;
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
#pragma warning disable format
		var (eventArgs, toGenerate, isTuple) = method switch { 
			{ ExportMethodData.EventArgsType.IsNullOrDefault: false }
				// get the type string for the event args type
				=> (method.Value.ExportMethodData.EventArgsType.GetIdentifierSyntax ().ToString (), false, method.Value.ExportMethodData.EventArgsType.IsNamedTuple), 
			{ ExportMethodData.EventArgsTypeName: not null } 
				=> (method.Value.ExportMethodData.EventArgsTypeName, true, false),
			{ Parameters.Length: 2 }
				// return the tye of the second parameter, which is the event args type
				=> (method.Value.Parameters[1].Type.GetIdentifierSyntax ().ToString (), false, false),
			{ Parameters.Length: > 2} 
				// return a unnamed tuple type with the parameters starting from the second one
				=> ($"({string.Join (", ", method.Value.Parameters.Skip (1).Select (p => p.Type.GetIdentifierSyntax ().ToString ()))})", false, true),
			// the default is a method that does not required an event args type
			_ => (null, false, false)
		};
#pragma warning restore format

		// gather the parameter info, do not retrieve use a type info since that create a circular structure,
		// the c# compiler will complain about it.
		var paramInfo = ImmutableArray.CreateBuilder<(string Name, string Type)> ();
		var usingsInfo = new HashSet<string> ();
		foreach (var p in method.Value.Parameters) {
			paramInfo.Add ((p.Name, p.Type.GetIdentifierSyntax ().ToString ()));
			// collect the namespaces of the parameters, use a set to avoid duplicates
			var ns = string.Join ('.', p.Type.Namespace);
			if (!string.IsNullOrEmpty (ns) && ns != typeNamespace) {
				usingsInfo.Add (ns);
			}
		}
		eventInfo = new () {
			Namespace = typeNamespace,
			Name = method.Value.Name,
			Usings = [.. usingsInfo],
			EventArgsType = eventArgs,
			EventArgsIsTuple = isTuple,
			// full method signature with params and public
			MethodSignature = method.Value.WithModifiers (SyntaxKind.PublicKeyword).ToDeclaration ().ToString (),
			MethodSelector = method.Value.ExportMethodData.Selector!,
			MethodParameters = paramInfo.ToImmutable (),
			ToGenerate = toGenerate
		};
		return true;
	}

	/// <summary>
	/// Gets the events for a given interface symbol.
	/// </summary>
	/// <param name="typeNamespace">The namespace of the types whose events we are collecting.</param>
	/// <param name="symbol">The interface symbol.</param>
	/// <param name="context">The root context.</param>
	/// <returns>An immutable array of tuples containing the event name and its event arguments type name.</returns>
	static ImmutableArray<EventInfo> GetInterfaceEvents (string typeNamespace, ITypeSymbol symbol, RootContext context)
	{
		var parentMethodsBucket = ImmutableArray.CreateBuilder<EventInfo> ();
		// add the events for the current symbol
		foreach (var member in symbol.GetMembers ()) {
			if (TryGetEventForMember (typeNamespace, member, context, out var data)) {
				parentMethodsBucket.Add (data.Value);
			}
		}

		// add the events from the parent interfaces
		foreach (var i in symbol.AllInterfaces) {
			foreach (var member in i.GetMembers ()) {
				if (TryGetEventForMember (typeNamespace, member, context, out var data)) {
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
			Events = GetInterfaceEvents (string.Join ('.', Namespace), symbol, context);
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
