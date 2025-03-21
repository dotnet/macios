using System;
using System.Reflection;

#nullable enable

public static class CustomAttributeDataExtensions {
	public static Type GetAttributeType (this CustomAttributeData data) => data.AttributeType;
}
