using System;
using System.Reflection;

#nullable enable

//
// Used to encapsulate flags about types in either the parameter or the return value
// For now, it only supports the [PlainString] attribute on strings.
//
public class MarshalInfo {
	public Generator Generator { get; }
	public bool PlainString { get; }
	public Type Type { get; }
	public bool IsOut { get; }

	public bool IsAligned;

	// Used for parameters
	public MarshalInfo (Generator generator, MethodInfo mi, ParameterInfo pi)
	{
		this.Generator = generator;
		PlainString = Generator.AttributeManager.HasAttribute<PlainStringAttribute> (pi);
		Type = pi.ParameterType;
		IsOut = pi.IsOut;
	}

	// Used to return values
	public MarshalInfo (Generator generator, MethodInfo mi)
	{
		this.Generator = generator;
		PlainString = Generator.AttributeManager.HasAttribute<PlainStringAttribute> (mi.ReturnParameter);
		Type = mi.ReturnType;
	}
}
