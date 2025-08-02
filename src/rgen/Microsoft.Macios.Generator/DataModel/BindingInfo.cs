// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// This struct works as a union to store the possible BindingTypeData that can be present in the bindings.
/// </summary>
[StructLayout (LayoutKind.Explicit)]
readonly struct BindingInfo : IEquatable<BindingInfo> {
	// make the struct smaller by making all wrapped structs be at index 0
	[FieldOffset (0)] readonly BindingType bindingType;
	[FieldOffset (8)] readonly BindingTypeData bindingTypeData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Class> classData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Protocol> protocolData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.Category> categoryData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.CoreImageFilter> coreImageFilterData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.SmartEnum> smartEnumData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.StrongDictionary> strongDictionaryData;
	[FieldOffset (8)] readonly BindingTypeData<ObjCBindings.StrongDictionaryKeys> strongDictionaryKeysData;

	/// <summary>
	/// The type of the binding.
	/// </summary>
	public BindingType BindingType => bindingType;

	/// <summary>
	/// Returns if the binding has been declared to be thread safe.
	/// </summary>
	public bool IsThreadSafe {
		get {
			return bindingType switch {
				BindingType.Category => categoryData.Flags.HasFlag (Category.IsThreadSafe),
				BindingType.Class => classData.Flags.HasFlag (Class.IsThreadSafe),
				BindingType.Protocol => protocolData.Flags.HasFlag (Protocol.IsThreadSafe),
				_ => false
			};
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct.
	/// </summary>
	/// <param name="type">The binding type.</param>
	/// <param name="data">The binding data.</param>
	public BindingInfo (BindingType type, BindingTypeData data)
	{
		bindingType = type;
		bindingTypeData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a class binding.
	/// </summary>
	/// <param name="data">The class binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.Class> data)
	{
		bindingType = BindingType.Class;
		classData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a protocol binding.
	/// </summary>
	/// <param name="data">The protocol binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.Protocol> data)
	{
		bindingType = BindingType.Protocol;
		protocolData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a category binding.
	/// </summary>
	/// <param name="data">The category binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.Category> data)
	{
		bindingType = BindingType.Category;
		categoryData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a Core Image filter binding.
	/// </summary>
	/// <param name="data">The Core Image filter binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.CoreImageFilter> data)
	{
		bindingType = BindingType.CoreImageFilter;
		coreImageFilterData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a smart enum binding.
	/// </summary>
	/// <param name="data">The smart enum binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.SmartEnum> data)
	{
		bindingType = BindingType.SmartEnum;
		smartEnumData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a strong dictionary binding.
	/// </summary>
	/// <param name="data">The strong dictionary binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.StrongDictionary> data)
	{
		bindingType = BindingType.StrongDictionary;
		strongDictionaryData = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BindingInfo"/> struct for a strong dictionary keys binding.
	/// </summary>
	/// <param name="data">The strong dictionary keys binding data.</param>
	public BindingInfo (BindingTypeData<ObjCBindings.StrongDictionaryKeys> data)
	{
		bindingType = BindingType.StrongDictionaryKeys;
		strongDictionaryKeysData = data;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData"/>.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	public static implicit operator BindingTypeData (BindingInfo info) => info.bindingTypeData;

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a class.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.Class"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.Class> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Class)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Class for binding type {info.BindingType}");
		return info.classData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a protocol.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.Protocol"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.Protocol> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Protocol)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Protocol for binding type {info.BindingType}");
		return info.protocolData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a category.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.Category"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.Category> (BindingInfo info)
	{
		if (info.BindingType != BindingType.Category)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.Category for binding type {info.BindingType}");
		return info.categoryData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a Core Image filter.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.CoreImageFilter"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.CoreImageFilter> (BindingInfo info)
	{
		if (info.BindingType != BindingType.CoreImageFilter)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.SmartEnum for binding type {info.BindingType}");
		return info.coreImageFilterData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a smart enum.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.SmartEnum"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.SmartEnum> (BindingInfo info)
	{
		if (info.BindingType != BindingType.SmartEnum)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.SmartEnum for binding type {info.BindingType}");
		return info.smartEnumData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a strong dictionary.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.StrongDictionary"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.StrongDictionary> (BindingInfo info)
	{
		if (info.BindingType != BindingType.StrongDictionary)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.StrongDictionary for binding type {info.BindingType}");
		return info.strongDictionaryData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingInfo"/> to a <see cref="BindingTypeData{T}"/> for a strong dictionary keys.
	/// </summary>
	/// <param name="info">The <see cref="BindingInfo"/> to convert.</param>
	/// <exception cref="InvalidCastException">Thrown if the binding type is not <see cref="BindingType.StrongDictionaryKeys"/>.</exception>
	public static implicit operator BindingTypeData<ObjCBindings.StrongDictionaryKeys> (BindingInfo info)
	{
		if (info.BindingType != BindingType.StrongDictionaryKeys)
			throw new InvalidCastException ($"Invalid cast to ObjCBindings.StrongDictionaryKeys for binding type {info.BindingType}");
		return info.strongDictionaryKeysData;
	}

	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData"/> to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData data) => new (BindingType.Unknown, data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a class to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Class> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a protocol to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Protocol> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a category to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.Category> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a Core Image filter to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.CoreImageFilter> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a smart enum to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.SmartEnum> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a strong dictionary to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.StrongDictionary> data) => new (data);
	/// <summary>
	/// Implicitly converts a <see cref="BindingTypeData{T}"/> for a strong dictionary keys to a <see cref="BindingInfo"/>.
	/// </summary>
	/// <param name="data">The <see cref="BindingTypeData{T}"/> to convert.</param>
	public static implicit operator BindingInfo (BindingTypeData<ObjCBindings.StrongDictionaryKeys> data) => new (data);

	/// <inheritdoc />
	public bool Equals (BindingInfo other)
	{
		if (bindingType != other.bindingType)
			return false;
		switch (bindingType) {
		case BindingType.Unknown:
			return bindingTypeData == other.bindingTypeData;
		case BindingType.SmartEnum:
			return bindingTypeData == other.bindingTypeData;
		case BindingType.Class:
			return classData == other.classData;
		case BindingType.Protocol:
			return protocolData == other.protocolData;
		case BindingType.Category:
			return categoryData == other.categoryData;
		case BindingType.StrongDictionary:
			return strongDictionaryData == other.strongDictionaryData;
		case BindingType.StrongDictionaryKeys:
			return strongDictionaryKeysData == other.strongDictionaryKeysData;
		}
		return false;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindingInfo other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode () => bindingType switch {
		BindingType.SmartEnum => HashCode.Combine (bindingType, bindingTypeData),
		BindingType.Class => HashCode.Combine (bindingType, classData),
		BindingType.Protocol => HashCode.Combine (bindingType, protocolData),
		BindingType.Category => HashCode.Combine (bindingType, categoryData),
		BindingType.StrongDictionary => HashCode.Combine (bindingType, strongDictionaryData),
		BindingType.StrongDictionaryKeys => HashCode.Combine (bindingType, strongDictionaryKeysData),
		_ => HashCode.Combine (bindingType, bindingTypeData)
	};

	/// <summary>
	/// Compares two <see cref="BindingInfo"/> instances for equality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
	public static bool operator == (BindingInfo x, BindingInfo y)
	{
		return x.Equals (y);
	}

	/// <summary>
	/// Compares two <see cref="BindingInfo"/> instances for inequality.
	/// </summary>
	/// <param name="x">The first instance to compare.</param>
	/// <param name="y">The second instance to compare.</param>
	/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
	public static bool operator != (BindingInfo x, BindingInfo y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString () => bindingType switch {
		BindingType.Class => $"{{ BindingType: {bindingType}, BindingData: {classData} }}",
		BindingType.Protocol => $"{{ BindingType: {bindingType}, BindingData: {protocolData} }}",
		BindingType.Category => $"{{ BindingType: {bindingType}, BindingData: {categoryData} }}",
		BindingType.CoreImageFilter => $"{{ BindingType: {bindingType}, BindingData: {coreImageFilterData} }}",
		BindingType.SmartEnum => $"{{ BindingType: {bindingType}, BindingData: {smartEnumData} }}",
		BindingType.StrongDictionary => $"{{ BindingType: {bindingType}, BindingData: {strongDictionaryData} }}",
		BindingType.StrongDictionaryKeys => $"{{ BindingType: {bindingType}, BindingData: {strongDictionaryKeysData} }}",
		_ => throw new NotImplementedException ()
	};
}
