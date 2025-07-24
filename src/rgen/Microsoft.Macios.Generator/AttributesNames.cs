// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Microsoft.Macios.Generator;

/// <summary>
/// Contains all the names of the attributes that are used by the binding generator.
/// </summary>
static class AttributesNames {

	public const string CategoryAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.Category>";
	public const string ClassAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>";
	public const string CoreImageFilterAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.CoreImageFilter>";
	public const string SmartEnumAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.SmartEnum>";
	public const string BindFromAttribute = "ObjCBindings.BindFromAttribute";
	public const string ProtocolAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.Protocol>";
	public const string StrongDictionaryAttribute = "ObjCBindings.BindingTypeAttribute<ObjCBindings.StrongDictionary>";
	public const string FieldAttribute = "ObjCBindings.FieldAttribute";
	public const string EnumFieldAttribute = "ObjCBindings.FieldAttribute<ObjCBindings.EnumValue>";
	public const string FieldPropertyAttribute = "ObjCBindings.FieldAttribute<ObjCBindings.Property>";
	public const string ExportPropertyAttribute = "ObjCBindings.ExportAttribute<ObjCBindings.Property>";
	public const string ExportStrongDictionaryPropertyAttribute = "ObjCBindings.ExportAttribute<ObjCBindings.StrongDictionaryProperty>";
	public const string ExportMethodAttribute = "ObjCBindings.ExportAttribute<ObjCBindings.Method>";
	public const string ExportConstructorAttribute = "ObjCBindings.ExportAttribute<ObjCBindings.Constructor>";
	public const string SupportedOSPlatformAttribute = "System.Runtime.Versioning.SupportedOSPlatformAttribute";
	public const string UnsupportedOSPlatformAttribute = "System.Runtime.Versioning.UnsupportedOSPlatformAttribute";
	public const string ObsoletedOSPlatformAttribute = "System.Runtime.Versioning.ObsoletedOSPlatformAttribute";
	public const string NativeAttribute = "ObjCRuntime.NativeAttribute";
	public const string ForcedTypeAttribute = "ObjCBindings.ForcedTypeAttribute";
	public const string BlockCallbackAttribute = "ObjCRuntime.BlockCallbackAttribute";
	public const string CCallbackAttribute = "ObjCRuntime.CCallbackAttribute";

	public static readonly string [] BindingTypes = [
		CategoryAttribute,
		ClassAttribute,
		ProtocolAttribute,
		StrongDictionaryAttribute,
		CoreImageFilterAttribute,
		SmartEnumAttribute,
	];


	public static string? GetBindingTypeAttributeName<T> () where T : Enum
	{
		var type = typeof (T);
		if (type == typeof (ObjCBindings.Category)) {
			return CategoryAttribute;
		}
		if (type == typeof (ObjCBindings.Class)) {
			return ClassAttribute;
		}
		if (type == typeof (ObjCBindings.Protocol)) {
			return ProtocolAttribute;
		}
		if (type == typeof (ObjCBindings.StrongDictionary)) {
			return StrongDictionaryAttribute;
		}
		if (type == typeof (ObjCBindings.SmartEnum)) {
			return SmartEnumAttribute;
		}

		return null;
	}

	public static string? GetFieldAttributeName<T> () where T : Enum
	{
		// we cannot use a switch statement because typeof is not a constant value
		var type = typeof (T);
		if (type == typeof (ObjCBindings.Property)) {
			return FieldPropertyAttribute;
		}
		if (type == typeof (ObjCBindings.EnumValue)) {
			return EnumFieldAttribute;
		}
		return null;
	}

	public static string? GetExportAttributeName<T> () where T : Enum
	{
		// we cannot use a switch statement because typeof is not a constant value
		var type = typeof (T);
		if (type == typeof (ObjCBindings.Property)) {
			return ExportPropertyAttribute;
		}
		if (type == typeof (ObjCBindings.Method)) {
			return ExportMethodAttribute;
		}
		if (type == typeof (ObjCBindings.Constructor)) {
			return ExportConstructorAttribute;
		}
		if (type == typeof (ObjCBindings.StrongDictionaryProperty)) {
			return ExportStrongDictionaryPropertyAttribute;
		}
		return null;
	}

}
