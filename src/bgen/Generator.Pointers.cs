// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;
using System.Reflection;

#nullable enable

public partial class Generator {
	BindingException InvalidPointerParameter (ParameterInfo parameter, string reason)
	{
		return new BindingException (1130, true, parameter.Name, parameter.Member.DeclaringType, parameter.Member.Name, reason);
	}

	bool IsBlittablePointerElement (Type type)
	{
		if (type.IsPointer)
			return true;
		if (type.IsEnum)
			return !IsNativeEnum (type) && IsBlittablePointerElement (type.GetEnumUnderlyingType ());
		if (type.IsPrimitive)
			return type != TypeCache.System_Boolean && type != TypeCache.System_Char;
		if (type == TypeCache.System_IntPtr || type == TypeCache.System_UIntPtr)
			return true;
		if (!type.IsValueType || type.IsAutoLayout || type.IsGenericType)
			return false;
		return type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.All (field => !AttributeManager.HasAttribute<System.Runtime.InteropServices.MarshalAsAttribute> (field) && IsBlittablePointerElement (field.FieldType));
	}

	void ValidatePointerParameter (ParameterInfo parameter)
	{
		var asStruct = AttributeManager.HasAttribute<PointerAsStructAttribute> (parameter);
		var asArray = AttributeManager.HasAttribute<PointerAsArrayAttribute> (parameter);
		if (!asStruct && !asArray)
			return;

		var type = parameter.ParameterType;
		if (asStruct && asArray || HasBindAsAttribute (parameter))
			throw InvalidPointerParameter (parameter, "Pointer attributes cannot be combined with each other or with BindAs.");

		if (asStruct) {
			if (!type.IsPointer && !type.IsByRef)
				throw InvalidPointerParameter (parameter, "PointerAsStruct requires a pointer, ref, or out parameter.");
			if (parameter.IsIn)
				throw InvalidPointerParameter (parameter, "PointerAsStruct does not support in parameters.");
		} else if (!type.IsSZArray) {
			throw InvalidPointerParameter (parameter, "PointerAsArray requires a one-dimensional array passed by value.");
		}

		var element = type.GetElementType () ?? throw InvalidPointerParameter (parameter, "The parameter must have a struct element type.");
		if (!element.IsValueType || element.IsPrimitive || element.IsEnum || !IsBlittablePointerElement (element))
			throw InvalidPointerParameter (parameter, "The element type must be a blittable struct.");

		if (!asArray)
			return;

		if (parameter.Member is not MethodInfo method || method.DeclaringType is null)
			throw InvalidPointerParameter (parameter, "PointerAsArray requires a binding method.");
		if (method.DeclaringType.IsSubclassOf (TypeCache.System_Delegate) || IsProtocol (method.DeclaringType) || IsModel (method.DeclaringType))
			throw InvalidPointerParameter (parameter, "PointerAsArray does not support callbacks, protocols, or models.");
		if (method.IsSpecialName || AttributeManager.HasAttribute<AsyncAttribute> (method) ||
			AttributeManager.HasAttribute<AbstractAttribute> (method) || AttributeManager.HasAttribute<OverrideAttribute> (method))
			throw InvalidPointerParameter (parameter, "PointerAsArray requires a synchronous, non-overridable method.");

		GetPointerArrayLength (method, parameter);
	}

	(string Expression, bool Signed) GetPointerArrayLength (MethodInfo method, ParameterInfo parameter)
	{
		var attribute = AttributeManager.GetOneCustomAttribute<PointerAsArrayAttribute> (parameter);
		var sources = (attribute.LengthFromParameter is not null ? 1 : 0) +
			(attribute.LengthFromMember is not null ? 1 : 0) +
			(attribute.HasLength ? 1 : 0);
		if (sources != 1 || (attribute.HasLength && attribute.Length < 0))
			throw InvalidPointerParameter (parameter, "Specify exactly one nonnegative Length, LengthFromParameter, or LengthFromMember.");

		if (attribute.Length >= 0)
			return (attribute.Length.ToString (System.Globalization.CultureInfo.InvariantCulture), true);

		Type lengthType;
		string expression;
		if (attribute.LengthFromParameter is not null) {
			var source = method.GetParameters ().FirstOrDefault (p => p.Name == attribute.LengthFromParameter);
			if (source is null || source.ParameterType.IsByRef || HasBindAsAttribute (source))
				throw InvalidPointerParameter (parameter, "LengthFromParameter must name an integral parameter passed by value.");
			lengthType = source.ParameterType;
			expression = source.Name.GetSafeParamName () ?? throw InvalidPointerParameter (parameter, "The length parameter must have a valid name.");
		} else {
			var name = attribute.LengthFromMember ?? throw InvalidPointerParameter (parameter, "A length source is required.");
			if (AttributeManager.HasAttribute<CategoryAttribute> (method.DeclaringType))
				throw InvalidPointerParameter (parameter, "LengthFromMember is not supported on categories.");
			Type? declaringType = method.DeclaringType;
			MemberInfo? member = null;
			while (declaringType is not null && member is null) {
				member = declaringType.GetMember (name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
					.FirstOrDefault (m => m is PropertyInfo || m is FieldInfo);
				declaringType = ReflectionExtensions.GetBaseTypeAttribute (declaringType, this)?.BaseType ?? declaringType.BaseType;
			}
			bool isStatic;
			switch (member) {
			case PropertyInfo property when property.GetMethod?.IsPublic == true && property.GetIndexParameters ().Length == 0 &&
				!property.IsUnavailable (this) && !property.GetMethod.IsUnavailable (this) && !HasBindAsAttribute (property):
				lengthType = property.PropertyType;
				isStatic = property.GetMethod.IsStatic || AttributeManager.IsStatic (property);
				break;
			case FieldInfo field:
				lengthType = field.FieldType;
				isStatic = field.IsStatic;
				break;
			default:
				throw InvalidPointerParameter (parameter, "LengthFromMember must name a readable integral property or field.");
			}
			if ((AttributeManager.IsStatic (method) || method.Name == "Constructor") && !isStatic)
				throw InvalidPointerParameter (parameter, "A static method or constructor requires a static length member.");
			if (member is FieldInfo constant && constant.IsLiteral && !lengthType.IsEnum) {
				expression = Convert.ToString (constant.GetRawConstantValue (), System.Globalization.CultureInfo.InvariantCulture) ??
					throw InvalidPointerParameter (parameter, "The length field must have an integral value.");
				if (lengthType == TypeCache.System_UInt64)
					expression += "UL";
				else if (lengthType == TypeCache.System_Int64)
					expression += "L";
			} else {
				var owner = isStatic ? TypeManager.FormatType (method.DeclaringType, member.DeclaringType ??
					throw InvalidPointerParameter (parameter, "The length member must have a declaring type.")) : "this";
				expression = $"{owner}.{name.GetSafeParamName ()}";
			}
		}

		if (lengthType == TypeCache.System_Byte || lengthType == TypeCache.System_UInt16 || lengthType == TypeCache.System_UInt32 ||
			lengthType == TypeCache.System_UInt64 || lengthType == TypeCache.System_UIntPtr)
			return (expression, false);
		if (lengthType == TypeCache.System_SByte || lengthType == TypeCache.System_Int16 || lengthType == TypeCache.System_Int32 ||
			lengthType == TypeCache.System_Int64 || lengthType == TypeCache.System_IntPtr)
			return (expression, true);
		throw InvalidPointerParameter (parameter, "The length source must have an integral type.");
	}
}
