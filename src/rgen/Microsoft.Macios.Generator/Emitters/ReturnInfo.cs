// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Represents information about a return value from a native invocation.
/// </summary>
readonly record struct ReturnInfo {
	/// <summary>
	/// The type information of the return value.
	/// </summary>
	public TypeInfo Type { get; init; }
	/// <summary>
	/// The [BindAs] attribute data, if any.
	/// </summary>
	public BindFromData? BindAs { get; init; }

	/// <summary>
	/// True if the returned handle should be released after use.
	/// </summary>
	public bool? ReleaseHandle { get; init; } = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="ReturnInfo"/> struct from a <see cref="Property"/>.
	/// </summary>
	/// <param name="property">The property to create the return info from.</param>
	public ReturnInfo (in Property property)
	{
		Type = property.ReturnType;
		BindAs = property.BindAs;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ReturnInfo"/> struct from a <see cref="DelegateInfo"/>.
	/// </summary>
	/// <param name="delegateInfo">The delegate info to create the return info from.</param>
	public ReturnInfo (in DelegateInfo delegateInfo)
	{
		Type = delegateInfo.ReturnType;
		BindAs = null;
		ReleaseHandle = false;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ReturnInfo"/> struct from a <see cref="Method"/>.
	/// </summary>
	/// <param name="method">The method to create the return info from.</param>
	public ReturnInfo (in Method method)
	{
		Type = method.ReturnType;
		BindAs = method.BindAs;
	}

	/// <summary>
	/// Implicitly converts a <see cref="Property"/> to a <see cref="ReturnInfo"/>.
	/// </summary>
	/// <param name="property">The property to convert.</param>
	public static implicit operator ReturnInfo (in Property property)
		=> new (property);

	/// <summary>
	/// Implicitly converts a <see cref="DelegateInfo"/> to a <see cref="ReturnInfo"/>.
	/// </summary>
	/// <param name="delegateInfo">The delegate info to convert.</param>
	public static implicit operator ReturnInfo (in DelegateInfo delegateInfo)
		=> new (delegateInfo);

	/// <summary>
	/// Implicitly converts a <see cref="Method"/> to a <see cref="ReturnInfo"/>.
	/// </summary>
	/// <param name="method">The method to convert.</param>
	public static implicit operator ReturnInfo (in Method method)
		=> new (method);
}
