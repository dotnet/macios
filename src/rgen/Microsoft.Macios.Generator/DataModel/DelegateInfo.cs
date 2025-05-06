// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Readonly structure that describes a delegate callback passed as a parameter.
/// </summary>
sealed record DelegateInfo {

	/// <summary>
	/// Method name.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Method return type.
	/// </summary>
	public TypeInfo ReturnType { get; }

	/// <summary>
	/// True if the delegate was decorated with the BlockCallbackAttribute.
	/// </summary>
	public bool IsBlockCallback { get; init; }

	/// <summary>
	/// True if the delegate was decorated with the CCallbackAttribute.
	/// </summary>
	public bool IsCCallback { get; init; }

	/// <summary>
	/// Parameters list.
	/// </summary>
	public ImmutableArray<DelegateParameter> Parameters { get; } = [];

	public DelegateInfo (string name, TypeInfo returnType, ImmutableArray<DelegateParameter> parameters)
	{
		Name = name;
		ReturnType = returnType;
		Parameters = parameters;
	}

	public static bool TryCreate (INamedTypeSymbol symbol, [NotNullWhen (true)] out DelegateInfo? change)
	{
		if (symbol.DelegateInvokeMethod is null) {
			change = null;
			return false;
		}

		var method = symbol.DelegateInvokeMethod;
		var parametersBucket = ImmutableArray.CreateBuilder<DelegateParameter> ();
		// loop over the parameters of the construct since changes on those implies a change in the generated code
		foreach (var parameter in method.Parameters) {
			if (!DelegateParameter.TryCreate (parameter, out var parameterChange))
				continue;
			parametersBucket.Add (parameterChange.Value);
		}

		change = new (
			name: method.Name,
			returnType: new (method.ReturnType),
			parameters: parametersBucket.ToImmutableArray ()) {
			IsBlockCallback = symbol.HasAttribute ("ObjCRuntime.BlockCallbackAttribute"),
			IsCCallback = symbol.HasAttribute ("ObjCRuntime.CCallbackAttribute"),
		};
		return true;
	}

	/// <inheritdoc/>
	public bool Equals (DelegateInfo? other)
	{
		if (other is null)
			return false;
		if (Name != other.Name)
			return false;
		if (ReturnType != other.ReturnType)
			return false;

		var paramComparer = new DelegateParameterEqualityComparer ();
		return paramComparer.Equals (Parameters, other.Parameters);
	}

	/// <inheritdoc/>
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (Name);
		hashCode.Add (ReturnType);

		foreach (var parameter in Parameters) {
			hashCode.Add (parameter);
		}

		return hashCode.ToHashCode ();
	}

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ($"Name: {Name}, ");
		sb.Append ($"ReturnType: {ReturnType}, ");
		sb.Append ("Parameters: [");
		sb.AppendJoin (", ", Parameters);
		sb.Append ("] }}");
		return sb.ToString ();
	}
}
