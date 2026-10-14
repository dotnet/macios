// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Extrospection;
using Mono.Cecil;
using Sharpie.Bind;

namespace Xamarin.Tests;

[TestFixture]
[NonParallelizable]
public class ObjCInterfaceTests {
	const string Framework = "InterfaceTests";

	static TypeDefinition AddType (ModuleDefinition module, string parentName, int constructorArguments, bool constructorWrapper, bool? propertyWrapper, bool skip, bool protocol)
	{
		var parent = new TypeDefinition (Framework, parentName, TypeAttributes.Public, module.TypeSystem.Object);
		module.Types.Add (parent);
		var type = new TypeDefinition ("", "_SharedDelegate", TypeAttributes.NestedAssembly, module.TypeSystem.Object);
		parent.NestedTypes.Add (type);

		var attributeType = new TypeReference ("Foundation", "RegisterAttribute", module, module);
		var constructor = new MethodReference (".ctor", module.TypeSystem.Void, attributeType) { HasThis = true };
		var attribute = new CustomAttribute (constructor);
		if (constructorArguments > 0) {
			constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.String));
			attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, "_SharedDelegate"));
		}
		if (constructorArguments > 1) {
			constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.Boolean));
			attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.Boolean, constructorWrapper));
		}
		if (propertyWrapper.HasValue)
			attribute.Properties.Add (new CustomAttributeNamedArgument ("IsWrapper", new CustomAttributeArgument (module.TypeSystem.Boolean, propertyWrapper.Value)));
		if (skip)
			attribute.Properties.Add (new CustomAttributeNamedArgument ("SkipRegistration", new CustomAttributeArgument (module.TypeSystem.Boolean, true)));
		type.CustomAttributes.Add (attribute);

		if (protocol) {
			var protocolType = new TypeReference ("Foundation", "ProtocolAttribute", module, module);
			type.CustomAttributes.Add (new CustomAttribute (new MethodReference (".ctor", module.TypeSystem.Void, protocolType) { HasThis = true }));
		}
		return type;
	}

	[TestCase (0, false, null, false, false, false)]
	[TestCase (1, false, null, false, false, false)]
	[TestCase (2, false, null, false, false, false)]
	[TestCase (2, true, null, false, false, true)]
	[TestCase (1, false, true, false, false, true)]
	[TestCase (2, false, true, false, false, true)]
	[TestCase (2, true, false, false, false, false)]
	[TestCase (2, true, null, true, false, false)]
	[TestCase (2, true, null, false, true, false)]
	public void DuplicateRegistration (int constructorArguments, bool constructorWrapper, bool? propertyWrapper, bool skip, bool protocol, bool duplicate)
	{
		using var module = ModuleDefinition.CreateModule ("InterfaceTests", ModuleKind.Dll);
		var first = AddType (module, "First", constructorArguments, constructorWrapper, propertyWrapper, skip, protocol);
		var second = AddType (module, "Second", constructorArguments, constructorWrapper, propertyWrapper, skip, protocol);
		var check = new ObjCInterfaceCheck (new BindingResult ());
		Log.On (Framework).Clear ();
		try {
			check.VisitManagedType (first);
			check.VisitManagedType (second);
			var expected = duplicate
				? new [] { $"!duplicate-register! _SharedDelegate exists as both {second.FullName} and {first.FullName}" }
				: [];
			Assert.That (Log.On (Framework), Is.EqualTo (expected));
		} finally {
			Log.On (Framework).Clear ();
		}
	}
}
