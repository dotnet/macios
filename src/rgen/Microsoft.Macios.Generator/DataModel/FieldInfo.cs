// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Macios.Generator.Attributes;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Struct that unfies the data found in a FieldAttribute and extra information calculated such as
/// the library name and path needed for a field.
/// </summary>
readonly struct FieldInfo<T> : IEquatable<FieldInfo<T>> where T : Enum {

	/// <summary>
	/// The initialization state of the struct.
	/// </summary>
	StructState State { get; init; } = StructState.Default;

	/// <summary>
	/// Gets the default, uninitialized instance of <see cref="FieldInfo{T}"/>.
	/// </summary>
	public static FieldInfo<T> Default { get; } = new (StructState.Default);

	/// <summary>
	/// Gets a value indicating whether the instance is the default, uninitialized instance.
	/// </summary>
	public bool IsNullOrDefault => State == StructState.Default;

	/// <summary>
	/// Name of the library that contains the smart enum definition.
	/// </summary>
	public string LibraryName { get; }

	/// <summary>
	/// Path of the library that contains the smart enum definition.
	/// </summary>
	public string? LibraryPath { get; }

	/// <summary>
	/// The data of the field attribute used to mark the value as a binding.
	/// </summary>
	public FieldData<T> FieldData { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FieldInfo{T}"/> struct.
	/// </summary>
	/// <param name="state">The initialization state.</param>
	public FieldInfo (StructState state)
	{
		State = state;
		LibraryName = string.Empty;
		LibraryPath = null;
		FieldData = default;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FieldInfo{T}"/> struct.
	/// </summary>
	/// <param name="fieldData">The field data.</param>
	/// <param name="libraryName">The library name.</param>
	/// <param name="libraryPath">The library path.</param>
	public FieldInfo (FieldData<T> fieldData, string libraryName, string? libraryPath = null) : this (StructState.Initialized)
	{
		LibraryName = libraryName;
		LibraryPath = libraryPath;
		FieldData = fieldData;
	}

	/// <summary>
	/// Deconstructs the <see cref="FieldInfo{T}"/> into its components.
	/// </summary>
	/// <param name="fieldData">The field data.</param>
	/// <param name="libraryName">The library name.</param>
	/// <param name="libraryPath">The library path.</param>
	public void Deconstruct (out FieldData<T> fieldData, out string libraryName, out string? libraryPath)
	{
		fieldData = FieldData;
		libraryName = LibraryName;
		libraryPath = LibraryPath;
	}

	/// <inheritdoc />
	public bool Equals (FieldInfo<T> other)
	{
		if (State == StructState.Default && other.State == StructState.Default)
			return true;
		if (FieldData != other.FieldData)
			return false;
		if (LibraryName != other.LibraryName)
			return false;
		return LibraryPath == other.LibraryPath;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is FieldInfo<T> other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (FieldData, LibraryName, LibraryPath);
	}

	/// <summary>
	/// Compares two <see cref="FieldInfo{T}"/> instances for equality.
	/// </summary>
	/// <param name="x">The first instance.</param>
	/// <param name="y">The second instance.</param>
	/// <returns><c>true</c> if the instances are equal, <c>false</c> otherwise.</returns>
	public static bool operator == (FieldInfo<T> x, FieldInfo<T> y)
	{
		return x.Equals (y);
	}

	/// <summary>
	/// Compares two <see cref="FieldInfo{T}"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first instance.</param>
	/// <param name="y">The second instance.</param>
	/// <returns><c>true</c> if the instances are not equal, <c>false</c> otherwise.</returns>
	public static bool operator != (FieldInfo<T> x, FieldInfo<T> y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ State = {State}, FieldData = {FieldData}, LibraryName = {LibraryName}, LibraryPath = {LibraryPath ?? "null"} }}";
	}

}
