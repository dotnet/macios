// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Compares two <see cref="ArgumentInfo"/> objects for sorting purposes, determining the order of argument conversions.
/// </summary>
class ArgumentInfoConversionComparer : IComparer<ArgumentInfo> {

	/// <inheritdoc />
	public int Compare (ArgumentInfo x, ArgumentInfo y)
	{
		var xPriority = GetPriority (x);
		var yPriority = GetPriority (y);

		return xPriority != yPriority
			? xPriority.CompareTo (yPriority)
			: x.Position.CompareTo (y.Position);
	}

	/// <summary>
	/// Gets the priority of an argument for conversion ordering.
	/// </summary>
	/// <param name="arg">The argument info.</param>
	/// <returns>An integer representing the priority.</returns>
	static int GetPriority (ArgumentInfo arg)
	{
		// the logic here is to determine the priority of the argument based on its type and attributes. The expected order is:
		// 1. NSObject (non-nullable) -> 1 GetNonNullHandle will throw if null so we want to execute it first
		// 2. INativeObject (non-nullable) -> 2 GetNonNullHandle will throw if null so we want to execute it second
		// 3. Reference type (non-nullable) -> 3 We will throw is null so we want to execute it third
		// 4. BindAs (non-nullable) -> 4 We will need a conversion to the type specified in BindAs so we want to execute it as late as possible
		// 5. Other reference types (nullable) -> 5 We will not throw if null so we want to execute it last
#pragma warning disable format
		return arg.Type switch { 
			{ IsNSObject: true, IsNullable: false } => 1, 
			{ IsINativeObject: true, IsNullable: false } => 2, 
			{ IsReferenceType: true, IsNullable: false } => 3,
			_ => arg.BindAs is not null ? 4 : 5
		};
#pragma warning restore format
	}
}
