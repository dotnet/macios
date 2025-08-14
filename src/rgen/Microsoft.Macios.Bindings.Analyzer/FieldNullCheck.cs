// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq.Expressions;

namespace Microsoft.Macios.Bindings.Analyzer;

/// <summary>
/// Defines a contract for checking if a field in an object is null or has a default value.
/// </summary>
/// <typeparam name="T">The type of the object containing the field.</typeparam>
public interface IFieldNullCheck<T> {
	/// <summary>
	/// Checks if the field in the specified object is null or has a default value.
	/// </summary>
	/// <param name="data">The object to check.</param>
	/// <returns><c>true</c> if the field is null or default; otherwise, <c>false</c>.</returns>
	bool IsNull (T data);
	/// <summary>
	/// Gets the name of the field being checked.
	/// </summary>
	string FieldName { get; }
}

/// <summary>
/// Provides an implementation of <see cref="IFieldNullCheck{T}"/> that uses an expression selector and a default value checker.
/// </summary>
/// <typeparam name="T">The type of the object containing the field.</typeparam>
/// <typeparam name="TField">The type of the field.</typeparam>
public class FieldNullCheck<T, TField> : IFieldNullCheck<T> {
	/// <summary>
	/// Gets or sets the expression used to select the field from the object.
	/// </summary>
	public Expression<Func<T, TField>> Selector { get; init; }
	/// <summary>
	/// Gets or sets the function used to determine if the field's value is its default value.
	/// </summary>
	public Func<TField, bool> IsDefaultValue { get; init; }

	/// <summary>
	/// Gets the name of the field being checked.
	/// </summary>
	public string FieldName { get; init; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FieldNullCheck{T, TField}"/> class.
	/// </summary>
	/// <param name="selector">The expression used to select the field.</param>
	/// <param name="isDefaultValue">The function used to check for the default value.</param>
	/// <param name="fieldName">The name of the field. If null, it's inferred from the selector.</param>
	public FieldNullCheck (Expression<Func<T, TField>> selector, Func<TField, bool> isDefaultValue, string? fieldName = null)
	{
		Selector = selector ?? throw new ArgumentNullException (nameof (selector));
		IsDefaultValue = isDefaultValue ?? throw new ArgumentNullException (nameof (isDefaultValue));
		FieldName = fieldName
					?? (Selector.Body as MemberExpression)?.Member.Name
					?? throw new InvalidOperationException ("Selector is not a member expression");
	}

	/// <summary>
	/// Checks if the selected field in the specified object is null or has a default value.
	/// </summary>
	/// <param name="data">The object to check.</param>
	/// <returns><c>true</c> if the field is null or default; otherwise, <c>false</c>.</returns>
	public bool IsNull (T data)
	{
		var value = Selector.Compile () (data);
		return IsDefaultValue (value);
	}

}
