// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Represents information about an argument used in a native invocation.
/// </summary>
readonly record struct ArgumentInfo {

	/// <summary>
	/// The name of the argument.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// The type information of the argument.
	/// </summary>
	public TypeInfo Type { get; init; }

	/// <summary>
	/// The [BindAs] attribute data, if any.
	/// </summary>
	public BindFromData? BindAs { get; init; }

	/// <summary>
	/// True if the argument is passed by reference.
	/// </summary>
	public bool IsByRef { get; init; }

	/// <summary>
	/// The reference kind of the argument.
	/// </summary>
	public ReferenceKind ReferenceKind { get; init; } = ReferenceKind.None;

	/// <summary>
	/// True if the argument is a C-style function pointer callback.
	/// </summary>
	public bool IsCCallback { get; init; }

	/// <summary>
	/// True if the argument is an Objective-C block callback.
	/// </summary>
	public bool IsBlockCallback { get; init; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgumentInfo"/> struct from a <see cref="Parameter"/>.
	/// </summary>
	/// <param name="parameter">The parameter to create the argument info from.</param>
	public ArgumentInfo (in Parameter parameter)
	{
		Name = parameter.Name;
		Type = parameter.Type;
		BindAs = parameter.BindAs;
		IsByRef = parameter.IsByRef;
		ReferenceKind = parameter.ReferenceKind;
		IsCCallback = false;
		IsBlockCallback = false;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgumentInfo"/> struct from a <see cref="DelegateParameter"/>.
	/// </summary>
	/// <param name="parameter">The delegate parameter to create the argument info from.</param>
	public ArgumentInfo (in DelegateParameter parameter)
	{
		Name = parameter.Name;
		Type = parameter.Type;
		BindAs = parameter.BindAs;
		IsByRef = parameter.IsByRef;
		ReferenceKind = parameter.ReferenceKind;
		IsCCallback = parameter.IsCCallback;
		IsBlockCallback = parameter.IsBlockCallback;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgumentInfo"/> struct from a <see cref="Property"/>.
	/// This constructor is used when treating the property as a setter argument.
	/// </summary>
	/// <param name="property">The property to create the argument info from.</param>
	public ArgumentInfo (in Property property)
	{
		Name = "value";
		Type = property.ReturnType;
		BindAs = property.BindAs;
		IsByRef = false;
		ReferenceKind = ReferenceKind.None;
		IsCCallback = false;
		IsBlockCallback = false;
	}

	/// <summary>
	/// Implicitly converts a <see cref="Parameter"/> to an <see cref="ArgumentInfo"/>.
	/// </summary>
	/// <param name="parameter">The parameter to convert.</param>
	public static implicit operator ArgumentInfo (in Parameter parameter)
		=> new (parameter);

	/// <summary>
	/// Implicitly converts a <see cref="DelegateParameter"/> to an <see cref="ArgumentInfo"/>.
	/// </summary>
	/// <param name="parameter">The delegate parameter to convert.</param>
	public static implicit operator ArgumentInfo (in DelegateParameter parameter)
		=> new (parameter);

	/// <summary>
	/// Implicitly converts a <see cref="Property"/> to an <see cref="ArgumentInfo"/>.
	/// This is used when treating the property as a setter argument.
	/// </summary>
	/// <param name="parameter">The property to convert.</param>
	public static implicit operator ArgumentInfo (in Property parameter)
		=> new (parameter);

	/// <summary>
	/// Creates a new instance of <see cref="ArgumentInfo"/> with the specified reference kind.
	/// </summary>
	/// <param name="referenceKind">The new reference kind.</param>
	/// <returns>A new <see cref="ArgumentInfo"/> instance with the updated reference kind.</returns>
	public ArgumentInfo WithRef (ReferenceKind referenceKind)
	{
		if (ReferenceKind == referenceKind)
			return this;

		return this with {
			ReferenceKind = referenceKind,
			IsByRef = referenceKind != ReferenceKind.None,
		};
	}
}
