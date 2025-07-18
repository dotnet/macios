// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Attributes;

readonly struct BindingTypeData : IEquatable<BindingTypeData> {

	/// <summary>
	/// Original name of the ObjC class or protocol.
	/// </summary>
	public string? Name { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData"/> struct.
	/// </summary>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	public BindingTypeData (string? name)
	{
		Name = name;
	}

	/// <summary>
	/// Try to parse the attribute data to retrieve the information of an ExportAttribute&lt;T&gt;.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="data">The parsed data. Null if we could not parse the attribute data.</param>
	/// <returns>True if the data was parsed.</returns>
	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out BindingTypeData? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		string? name;
		switch (count) {
		case 0:
			name = null;
			break;
		case 1:
			name = (string?) attributeData.ConstructorArguments [0].Value!;
			break;
		default:
			// 0 should not be an option..
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = new (name);
			return true;
		}

		foreach (var (paramName, value) in attributeData.NamedArguments) {
			switch (paramName) {
			case "Name":
				name = (string?) value.Value!;
				break;
			default:
				data = null;
				return false;
			}
		}

		data = new (name);
		return true;
	}

	/// <inheritdoc />
	public bool Equals (BindingTypeData other) => Name == other.Name;

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingTypeData other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Name);
	}

	/// <summary>
	/// Compares two <see cref="BindingTypeData"/> instances for equality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (BindingTypeData x, BindingTypeData y)
	{
		return x.Equals (y);
	}

	/// <summary>
	/// Compares two <see cref="BindingTypeData"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (BindingTypeData x, BindingTypeData y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Name: '{Name}' }}";
	}
}

readonly struct BindingTypeData<T> : IEquatable<BindingTypeData<T>> where T : Enum {

	/// <summary>
	/// Original name of the ObjC class or protocol.
	/// </summary>
	public string? Name { get; }

	/// <summary>
	/// The domain of an error enumerator. This has to be used with the SmartEnum flag.
	/// </summary>
	public string? ErrorDomain { get; init; }

	/// <summary>
	/// The library name of an error/smart enum.
	/// </summary>
	public string? LibraryName { get; init; }

	/// <summary>
	/// The configuration flags used on the exported class/interface.
	/// </summary>
	public T? Flags { get; } = default;

	/// <summary>
	/// The visibility of the default constructor for a core image filter.
	/// </summary>
	public MethodAttributes DefaultCtorVisibility { get; init; } = MethodAttributes.Public;

	/// <summary>
	/// The visibility of the IntPtr constructor for a core image filter.
	/// </summary>
	public MethodAttributes IntPtrCtorVisibility { get; init; } = MethodAttributes.PrivateScope;

	/// <summary>
	/// The visibility of the string constructor for a core image filter.
	/// </summary>
	public MethodAttributes StringCtorVisibility { get; init; } = MethodAttributes.PrivateScope;

	/// <summary>
	/// The type that the category extends.
	/// </summary>
	public TypeInfo? CategoryType { get; init; } = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData{T}"/> struct.
	/// </summary>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	public BindingTypeData (string? name)
	{
		Name = name;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData{T}"/> struct.
	/// </summary>
	/// <param name="flags">The configuration flags.</param>
	public BindingTypeData (T? flags)
	{
		Name = null;
		Flags = flags;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData{T}"/> struct.
	/// </summary>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	/// <param name="flags">The configuration flags.</param>
	public BindingTypeData (string? name, T? flags)
	{
		Name = name;
		Flags = flags;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData"/> struct for a category.
	/// </summary>
	/// <param name="categoryType">The type that the category extends.</param>
	public BindingTypeData (TypeInfo? categoryType)
	{
		Name = categoryType?.Name;
		CategoryType = categoryType;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingTypeData"/> struct for a category.
	/// </summary>
	/// <param name="categoryType">The type that the category extends.</param>
	/// <param name="flags">The configuration flags.</param>
	public BindingTypeData (TypeInfo? categoryType, T? flags)
	{
		Name = categoryType?.Name;
		CategoryType = categoryType;
		Flags = flags;
	}

	/// <summary>
	/// Try to parse the attribute data to retrieve the information of an ExportAttribute&lt;T&gt;.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="data">The parsed data. Null if we could not parse the attribute data.</param>
	/// <returns>True if the data was parsed.</returns>
	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out BindingTypeData<T>? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		string? name = null;
		T? flags = default;
		// category related data
		TypeInfo? categoryType = null;

		// check if we have a category type, we can do that by checking the type of the flag
		var isCategory = typeof (T) == typeof (ObjCBindings.Category);

		switch (count) {
		case 0:
			// use the defaults
			name = null;
			flags = default;
			break;
		case 1:
			var value = attributeData.ConstructorArguments [0].Value;
			if (isCategory && value is INamedTypeSymbol typeSymbol) {
				categoryType = new (typeSymbol);
				name = categoryType.Value.Name;
			} else if (!isCategory && value is string str) {
				name = str;
			} else if (value is not null) {
				flags = (T) value;
			}
			break;
		case 2:
			if (isCategory) {
				categoryType = new ((INamedTypeSymbol) attributeData.ConstructorArguments [0].Value!);
				name = categoryType.Value.Name;
			} else {
				// we have the name and the config flags present
				name = (string?) attributeData.ConstructorArguments [0].Value!;
			}
			flags = (T) attributeData.ConstructorArguments [1].Value!;
			break;
		default:
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			if (isCategory) {
				data = flags is not null ?
					new (categoryType, flags) : new (categoryType);
			} else {
				data = flags is not null ?
					new (name, flags) : new (name);
			}
			return true;
		}

		// the named types are different depending on the type of the flag, if we are dealing with a category or not.
		if (isCategory && TryExtractCategoryNamedParameters (attributeData, out name, ref flags, out categoryType)) {
			data = CreateCategoryBindingData (flags, categoryType);
			return true;
		} else if (TryExtractClassNamedParameters (attributeData, out name, ref flags, out string? errorDomain, out string? libraryName, out MethodAttributes defaultCtorVisibility, out MethodAttributes intPtrCtorVisibility, out MethodAttributes stringCtorVisibility)) {
			data = CreateClassBindingData (flags, name, errorDomain, libraryName, defaultCtorVisibility, intPtrCtorVisibility, stringCtorVisibility);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Creates a new instance of <see cref="BindingTypeData{T}"/> for a class.
	/// </summary>
	/// <param name="flags">The configuration flags.</param>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	/// <param name="errorDomain">The domain of an error enumerator.</param>
	/// <param name="libraryName">The library name of an error/smart enum.</param>
	/// <param name="defaultCtorVisibility">The visibility of the default constructor.</param>
	/// <param name="intPtrCtorVisibility">The visibility of the IntPtr constructor.</param>
	/// <param name="stringCtorVisibility">The visibility of the string constructor.</param>
	/// <returns>A new instance of <see cref="BindingTypeData{T}"/>.</returns>
	static BindingTypeData<T> CreateClassBindingData (T? flags, string? name, string? errorDomain,
		string? libraryName, MethodAttributes defaultCtorVisibility, MethodAttributes intPtrCtorVisibility,
		MethodAttributes stringCtorVisibility)
	{
		return flags is not null
			? new (name, flags) {
				ErrorDomain = errorDomain,
				LibraryName = libraryName,
				DefaultCtorVisibility = defaultCtorVisibility,
				IntPtrCtorVisibility = intPtrCtorVisibility,
				StringCtorVisibility = stringCtorVisibility,
			}
			: new (name) {
				ErrorDomain = errorDomain,
				LibraryName = libraryName,
				DefaultCtorVisibility = defaultCtorVisibility,
				IntPtrCtorVisibility = intPtrCtorVisibility,
				StringCtorVisibility = stringCtorVisibility,
			};
	}

	/// <summary>
	/// Creates a new instance of <see cref="BindingTypeData{T}"/> for a category.
	/// </summary>
	/// <param name="flags">The configuration flags.</param>
	/// <param name="categoryType">The type that the category extends.</param>
	/// <returns>A new instance of <see cref="BindingTypeData{T}"/>.</returns>
	static BindingTypeData<T> CreateCategoryBindingData (T? flags, TypeInfo? categoryType)
	{
		return flags is not null
			? new (categoryType, flags)
			: new (categoryType);
	}

	/// <summary>
	/// Tries to extract the named parameters for a class from the attribute data.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	/// <param name="flags">The configuration flags.</param>
	/// <param name="errorDomain">The domain of an error enumerator.</param>
	/// <param name="libraryName">The library name of an error/smart enum.</param>
	/// <param name="defaultCtorVisibility">The visibility of the default constructor.</param>
	/// <param name="intPtrCtorVisibility">The visibility of the IntPtr constructor.</param>
	/// <param name="stringCtorVisibility">The visibility of the string constructor.</param>
	/// <returns>True if the data was parsed.</returns>
	static bool TryExtractClassNamedParameters (AttributeData attributeData,
		out string? name, ref T? flags, out string? errorDomain, out string? libraryName,
		out MethodAttributes defaultCtorVisibility, out MethodAttributes intPtrCtorVisibility,
		out MethodAttributes stringCtorVisibility)
	{
		name = null;
		errorDomain = null;
		libraryName = null;
		defaultCtorVisibility = MethodAttributes.PrivateScope;
		intPtrCtorVisibility = MethodAttributes.PrivateScope;
		stringCtorVisibility = MethodAttributes.PrivateScope;

		foreach (var (paramName, value) in attributeData.NamedArguments) {
			switch (paramName) {
			case "Name":
				name = (string?) value.Value!;
				break;
			case "Flags":
				flags = (T) value.Value!;
				break;
			case "ErrorDomain":
				errorDomain = (string?) value.Value!;
				break;
			case "LibraryName":
				libraryName = (string?) value.Value!;
				break;
			case "DefaultCtorVisibility":
				defaultCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			case "IntPtrCtorVisibility":
				intPtrCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			case "StringCtorVisibility":
				stringCtorVisibility = (MethodAttributes) Convert.ToSingle ((int) value.Value!);
				break;
			default:
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Tries to extract the named parameters for a category from the attribute data.
	/// </summary>
	/// <param name="attributeData">The attribute data to be parsed.</param>
	/// <param name="name">The original name of the ObjC class or protocol.</param>
	/// <param name="flags">The configuration flags.</param>
	/// <param name="categoryType">The type that the category extends.</param>
	/// <returns>True if the data was parsed.</returns>
	static bool TryExtractCategoryNamedParameters (AttributeData attributeData, out string? name, ref T? flags, out TypeInfo? categoryType)
	{
		name = null;
		categoryType = null;
		foreach (var (paramName, value) in attributeData.NamedArguments) {
			switch (paramName) {
			case "Name":
				name = (string?) value.Value!;
				break;
			case "Flags":
				flags = (T) value.Value!;
				break;
			case "CategoryType":
				categoryType = new ((INamedTypeSymbol) value.Value!);
				break;
			default:
				return false;
			}
		}

		return true;
	}

	/// <inheritdoc />
	public bool Equals (BindingTypeData<T> other)
	{
		if (Name != other.Name)
			return false;
		if (CategoryType != other.CategoryType)
			return false;
		if (Flags is not null && other.Flags is not null) {
			return Flags.Equals (other.Flags);
		}
		return false;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingTypeData<T> other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Name, Flags);
	}

	/// <summary>
	/// Compares two <see cref="BindingTypeData{T}"/> instances for equality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (BindingTypeData<T> x, BindingTypeData<T> y)
	{
		return x.Equals (y);
	}

	/// <summary>
	/// Compares two <see cref="BindingTypeData{T}"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (BindingTypeData<T> x, BindingTypeData<T> y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Name: '{Name}', CategoryType: '{CategoryType?.ToString () ?? "null"}', Flags: '{Flags}' }}";
	}
}
