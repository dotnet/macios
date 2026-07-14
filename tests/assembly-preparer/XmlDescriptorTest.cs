// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Xamarin.Linker.Steps;

namespace AssemblyPreparerTests;

[TestFixture]
public class XmlDescriptorTest {
	static ModuleDefinition CreateModule ()
	{
		var assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition ("TestAsm", new Version (1, 0, 0, 0)), "TestAsm", ModuleKind.Dll);
		return assembly.MainModule;
	}

	static TypeDefinition CreateType (ModuleDefinition module, string @namespace = "NS", string name = "MyType")
	{
		var type = new TypeDefinition (@namespace, name, TypeAttributes.Public | TypeAttributes.Class, module.TypeSystem.Object);
		module.Types.Add (type);
		return type;
	}

	static MethodDefinition AddMethod (TypeDefinition type, string name, TypeReference returnType, params TypeReference [] parameters)
	{
		var method = new MethodDefinition (name, MethodAttributes.Public | MethodAttributes.Static, returnType);
		foreach (var p in parameters)
			method.Parameters.Add (new ParameterDefinition (p));
		type.Methods.Add (method);
		return method;
	}

	[Test]
	public void PreserveMethod_EmitsUnconditionalMethod ()
	{
		var module = CreateModule ();
		var type = CreateType (module);
		var method = AddMethod (type, "Convert", module.TypeSystem.String, module.TypeSystem.Int32);

		var descriptor = new XmlDescriptor ();
		Assert.That (descriptor.IsEmpty, Is.True, "IsEmpty before");
		descriptor.PreserveMethod (method);
		Assert.That (descriptor.IsEmpty, Is.False, "IsEmpty after");

		var document = descriptor.CreateXml ();
		var assemblyElement = document.Root!.Elements ("assembly").Single ();
		Assert.That ((string?) assemblyElement.Attribute ("fullname"), Is.EqualTo ("TestAsm"), "assembly fullname");

		var typeElement = assemblyElement.Elements ("type").Single ();
		Assert.That ((string?) typeElement.Attribute ("fullname"), Is.EqualTo ("NS.MyType"), "type fullname");
		Assert.That ((string?) typeElement.Attribute ("preserve"), Is.EqualTo ("nothing"), "type preserve");
		// A required method makes the type itself required, so there's no required="false" on the type.
		Assert.That (typeElement.Attribute ("required"), Is.Null, "type required");

		var methodElement = typeElement.Elements ("method").Single ();
		Assert.That ((string?) methodElement.Attribute ("signature"), Is.EqualTo ("System.String Convert(System.Int32)"), "method signature");
		Assert.That ((string?) methodElement.Attribute ("required"), Is.EqualTo ("true"), "method required");
	}

	[Test]
	public void PreserveMethod_Conditional_MakesTypeNotRequired ()
	{
		var module = CreateModule ();
		var type = CreateType (module);
		var method = AddMethod (type, "Convert", module.TypeSystem.String, module.TypeSystem.Int32);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveMethod (method, required: false);

		var typeElement = descriptor.CreateXml ().Root!.Descendants ("type").Single ();
		Assert.That ((string?) typeElement.Attribute ("required"), Is.EqualTo ("false"), "type required");
		Assert.That ((string?) typeElement.Attribute ("preserve"), Is.EqualTo ("nothing"), "type preserve");

		var methodElement = typeElement.Elements ("method").Single ();
		Assert.That ((string?) methodElement.Attribute ("required"), Is.EqualTo ("false"), "method required");
	}

	[Test]
	public void PreserveField_EmitsField ()
	{
		var module = CreateModule ();
		var type = CreateType (module);
		var field = new FieldDefinition ("MyField", FieldAttributes.Public, module.TypeSystem.Int32);
		type.Fields.Add (field);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveField (field);

		var typeElement = descriptor.CreateXml ().Root!.Descendants ("type").Single ();
		var fieldElement = typeElement.Elements ("field").Single ();
		Assert.That ((string?) fieldElement.Attribute ("name"), Is.EqualTo ("MyField"), "field name");
		Assert.That ((string?) fieldElement.Attribute ("required"), Is.EqualTo ("true"), "field required");
	}

	[Test]
	public void PreserveTypeWithAllMembers_EmitsPreserveAll ()
	{
		var module = CreateModule ();
		var type = CreateType (module);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveTypeWithAllMembers (type);

		var typeElement = descriptor.CreateXml ().Root!.Descendants ("type").Single ();
		Assert.That ((string?) typeElement.Attribute ("preserve"), Is.EqualTo ("all"), "type preserve");
		Assert.That (typeElement.Elements ().Count (), Is.EqualTo (0), "no child members");
	}

	[Test]
	public void PreserveTypeFields_EmitsPreserveFields ()
	{
		var module = CreateModule ();
		var type = CreateType (module);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveTypeFields (type);

		var typeElement = descriptor.CreateXml ().Root!.Descendants ("type").Single ();
		Assert.That ((string?) typeElement.Attribute ("preserve"), Is.EqualTo ("fields"), "type preserve");
	}

	[Test]
	public void PreserveMethod_GenericSignature_UsesNameInsteadOfSignature ()
	{
		var module = CreateModule ();
		var type = CreateType (module);
		// A method with a generic parameter in its signature can't be expressed via a signature the
		// linker can resolve, so the descriptor must fall back to the method name.
		var method = new MethodDefinition ("GenericMethod", MethodAttributes.Public | MethodAttributes.Static, module.TypeSystem.Void);
		var gp = new GenericParameter ("T", method);
		method.GenericParameters.Add (gp);
		method.Parameters.Add (new ParameterDefinition (gp));
		type.Methods.Add (method);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveMethod (method);

		var methodElement = descriptor.CreateXml ().Root!.Descendants ("method").Single ();
		Assert.That ((string?) methodElement.Attribute ("name"), Is.EqualTo ("GenericMethod"), "method name");
		Assert.That (methodElement.Attribute ("signature"), Is.Null, "no signature");
	}

	[Test]
	public void Save_OnlyWritesWhenChanged ()
	{
		var module = CreateModule ();
		var type = CreateType (module);
		var method = AddMethod (type, "Convert", module.TypeSystem.String, module.TypeSystem.Int32);

		var descriptor = new XmlDescriptor ();
		descriptor.PreserveMethod (method);

		var path = Path.Combine (Xamarin.Cache.CreateTemporaryDirectory (), "descriptor.xml");

		Assert.That (descriptor.Save (path), Is.True, "first save writes");
		Assert.That (path, Does.Exist, "file exists");

		var writeTime = File.GetLastWriteTimeUtc (path);

		// Saving the same content again must not rewrite the file.
		Assert.That (descriptor.Save (path), Is.False, "second save is a no-op");
		Assert.That (File.GetLastWriteTimeUtc (path), Is.EqualTo (writeTime), "file untouched");

		// Adding a member changes the content, which must be written.
		descriptor.PreserveMethod (AddMethod (type, "Convert2", module.TypeSystem.String, module.TypeSystem.Int32));
		Assert.That (descriptor.Save (path), Is.True, "changed content rewrites");
	}
}
