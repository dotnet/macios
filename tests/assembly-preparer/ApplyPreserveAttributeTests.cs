// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class ApplyPreserveAttributeTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void ExplicitInterfaceMembers (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;

		interface IMyInterface {
			int MyProperty { get; set; }
			event EventHandler MyEvent;
		}

		[Preserve (AllMembers = true)]
		class MyClass : IMyInterface {
			int IMyInterface.MyProperty { get; set; }
			event EventHandler IMyInterface.MyEvent {
				add {}
				remove {}
			}
		}
		";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var myClass = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var signatures = GetModuleConstructorSignatures (assemblyDefinition, "MyClass");
		var rootedMethods = GetExplicitInterfaceImplementationRoots (myClass, signatures);

		Assert.That (rootedMethods, Is.EquivalentTo (new [] {
			"IMyInterface.get_MyProperty",
			"IMyInterface.set_MyProperty",
			"IMyInterface.add_MyEvent",
			"IMyInterface.remove_MyEvent",
		}), "Rooted explicit accessors");
		Assert.That (signatures, Has.None.Contains ("IMyInterface"), "Explicit interface member signatures");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void GenericExplicitInterfaceMethods (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using System.Collections.Generic;
		using Foundation;

		interface IContract<TKey, TValue> {
			void Run (TKey key);
			void Run (TKey key, TValue value);
			T Create<T> () where T : class, IComparable<T>, new ();
			void Pair<T, U> (T t, U u) where T : U where U : IList<TKey>;
		}

		[Preserve (AllMembers = true)]
		class Owner<TOwner> : IContract<Dictionary<string, List<TOwner>>, long> {
			void IContract<Dictionary<string, List<TOwner>>, long>.Run (Dictionary<string, List<TOwner>> key) {}
			void IContract<Dictionary<string, List<TOwner>>, long>.Run (Dictionary<string, List<TOwner>> key, long value) {}
			T IContract<Dictionary<string, List<TOwner>>, long>.Create<T> () => new T ();
			void IContract<Dictionary<string, List<TOwner>>, long>.Pair<T, U> (T t, U u) {}
			public void Regular () {}
		}
		";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var owner = assemblyDefinition.MainModule.Types.Single (v => v.Name == "Owner`1");
		var signatures = GetModuleConstructorSignatures (assemblyDefinition, "Owner`1");
		var helpers = owner.Methods.Where (v => v.Name.StartsWith ("__linker_preserve_explicit_interface_implementation_", StringComparison.Ordinal)).ToArray ();
		Assert.That (helpers.Length, Is.EqualTo (4), "Helper count");
		Assert.That (signatures, Does.Contain ("Regular"), "Regular method");
		Assert.That (signatures, Has.None.Contains ("IContract"), "Explicit interface member signatures");

		var rootedMethods = GetExplicitInterfaceImplementationRoots (owner, signatures);
		Assert.That (rootedMethods.Count (v => v.EndsWith (".Run", StringComparison.Ordinal)), Is.EqualTo (2), "Run overloads");

		var create = helpers.Single (v => ((MethodReference) v.Body.Instructions [0].Operand).Name.EndsWith (".Create", StringComparison.Ordinal));
		Assert.That (create.GenericParameters.Count, Is.EqualTo (1), "Create generic parameters");
		var createT = create.GenericParameters [0];
		Assert.That (createT.HasReferenceTypeConstraint, Is.True, "class constraint");
		Assert.That (createT.HasDefaultConstructorConstraint, Is.True, "new () constraint");
		Assert.That (createT.Constraints.Select (v => v.ConstraintType.FullName), Is.EqualTo (new [] { "System.IComparable`1<T>" }), "Create constraints");
		Assert.That (((GenericInstanceType) createT.Constraints [0].ConstraintType).GenericArguments [0], Is.SameAs (createT), "Create constraint is rebound to the helper's generic parameter");

		var createInstance = (GenericInstanceMethod) create.Body.Instructions [2].Operand;
		Assert.That (createInstance.GenericArguments, Is.EqualTo (new [] { createT }), "Create instantiation");
		var createOwner = (GenericInstanceType) createInstance.ElementMethod.DeclaringType;
		Assert.That (createOwner.ElementType, Is.SameAs (owner), "Create declaring type");
		Assert.That (createOwner.GenericArguments, Is.EqualTo (new [] { owner.GenericParameters [0] }), "Create declaring type instantiation");

		var pair = helpers.Single (v => ((MethodReference) v.Body.Instructions [0].Operand).Name.EndsWith (".Pair", StringComparison.Ordinal));
		Assert.That (pair.GenericParameters [0].Constraints.Single ().ConstraintType, Is.SameAs (pair.GenericParameters [1]), "T : U constraint");
		Assert.That (pair.GenericParameters [1].Constraints.Single ().ConstraintType.FullName, Is.EqualTo ("System.Collections.Generic.IList`1<System.Collections.Generic.Dictionary`2<System.String,System.Collections.Generic.List`1<TOwner>>>"), "U constraint");

		Assert.That (signatures, Does.Contain (create.Name + "``1"), "Create helper signature");
		Assert.That (signatures, Does.Contain (pair.Name + "``2"), "Pair helper signature");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void PreserveIndividualExplicitInterfaceMethods (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;

		interface IMyInterface<T> {
			void Chosen ();
			void Conditional ();
			void NotChosen ();
		}

		class MyClass : IMyInterface<string> {
			[Preserve]
			void IMyInterface<string>.Chosen () {}
			[Preserve (Conditional = true)]
			void IMyInterface<string>.Conditional () {}
			void IMyInterface<string>.NotChosen () {}
		}
		";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var myClass = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var signatures = GetModuleConstructorSignatures (assemblyDefinition, "MyClass");
		Assert.That (GetExplicitInterfaceImplementationRoots (myClass, signatures), Is.EqualTo (new [] { "IMyInterface<System.String>.Chosen" }), "Unconditional");

		var cctorSignatures = myClass.GetStaticConstructor ().CustomAttributes
			.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute")
			.Where (v => v.ConstructorArguments.Count == 1)
			.Select (v => (string) v.ConstructorArguments [0].Value)
			.ToArray ();
		Assert.That (GetExplicitInterfaceImplementationRoots (myClass, cctorSignatures), Is.EqualTo (new [] { "IMyInterface<System.String>.Conditional" }), "Conditional");

		Assert.That (myClass.Methods.Count (v => v.Name.StartsWith ("__linker_preserve_explicit_interface_implementation_", StringComparison.Ordinal)), Is.EqualTo (2), "Helper count");
	}

	[Test]
	public void HelperNameDoesNotCollideWithUserMethod ()
	{
		var code = @"
		using Foundation;

		interface IMyInterface<T> { void Run (); }

		[Preserve (AllMembers = true)]
		class MyClass : IMyInterface<string> {
			int __linker_preserve_explicit_interface_implementation_0;
			static void __linker_preserve_explicit_interface_implementation_1 () {}
			static void __linker_preserve_explicit_interface_implementation_2 () {}
			void IMyInterface<string>.Run () {}
		}
		";

		AssertPrepare (ApplePlatform.MacOSX, true, code, out var assemblyDefinition);

		var myClass = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var signatures = GetModuleConstructorSignatures (assemblyDefinition, "MyClass");
		Assert.Multiple (() => {
			Assert.That (myClass.Methods.Count (v => v.Name == "__linker_preserve_explicit_interface_implementation_0"), Is.EqualTo (0), "Field name reused");
			Assert.That (myClass.Methods.Count (v => v.Name == "__linker_preserve_explicit_interface_implementation_1"), Is.EqualTo (1), "User method count 1");
			Assert.That (myClass.Methods.Count (v => v.Name == "__linker_preserve_explicit_interface_implementation_2"), Is.EqualTo (1), "User method count 2");
			Assert.That (signatures, Does.Contain ("__linker_preserve_explicit_interface_implementation_1"), "User method preserved 1");
			Assert.That (signatures, Does.Contain ("__linker_preserve_explicit_interface_implementation_2"), "User method preserved 2");
			Assert.That (GetExplicitInterfaceImplementationRoots (myClass, signatures), Is.EqualTo (new [] { "IMyInterface<System.String>.Run" }), "Rooted explicit method");
		});
	}

	[Test]
	public void HelperNameDoesNotCollideWithAbstractUserMethod ()
	{
		var code = @"
		using Foundation;

		interface IMyInterface<T> { void Run (); }

		[Preserve (AllMembers = true)]
		abstract class MyClass : IMyInterface<string> {
			public abstract void __linker_preserve_explicit_interface_implementation_0 ();
			void IMyInterface<string>.Run () {}
		}
		";

		AssertPrepare (ApplePlatform.MacOSX, true, code, out var assemblyDefinition);

		var myClass = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var signatures = GetModuleConstructorSignatures (assemblyDefinition, "MyClass");
		Assert.That (myClass.Methods.Single (v => v.Name == "__linker_preserve_explicit_interface_implementation_0").IsAbstract, Is.True, "User method is still abstract");
		Assert.That (signatures, Does.Contain ("__linker_preserve_explicit_interface_implementation_0"), "User method preserved");
		Assert.That (GetExplicitInterfaceImplementationRoots (myClass, signatures), Is.EqualTo (new [] { "IMyInterface<System.String>.Run" }), "Rooted explicit method");
	}

	static string [] GetModuleConstructorSignatures (AssemblyDefinition assemblyDefinition, string typeName)
	{
		var moduleConstructor = assemblyDefinition.MainModule.Types.Single (v => v.Name == "<Module>").GetStaticConstructor ();
		Assert.That (moduleConstructor, Is.Not.Null, "Module constructor");
		return moduleConstructor.CustomAttributes
			.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute")
			.Where (v => v.ConstructorArguments.Count >= 2)
			.Where (v => v.ConstructorArguments [0].Value is string)
			.Where (v => v.ConstructorArguments [1].Value switch {
				TypeReference tr => tr.FullName == typeName,
				string str => str == typeName,
				_ => false,
			})
			.Select (v => (string) v.ConstructorArguments [0].Value)
			.ToArray ();
	}

	// Returns the names of the methods referenced by the explicit interface implementation helpers whose signatures are in the given list.
	static string [] GetExplicitInterfaceImplementationRoots (TypeDefinition type, string [] signatures)
	{
		return type.Methods
			.Where (v => v.Name.StartsWith ("__linker_preserve_explicit_interface_implementation_", StringComparison.Ordinal))
			.Where (v => v.HasBody && v.Body.Instructions [0].OpCode == OpCodes.Ldtoken)
			.Where (v => signatures.Contains (v.HasGenericParameters ? v.Name + "``" + v.GenericParameters.Count : v.Name))
			.Select (v => ((MethodReference) v.Body.Instructions [0].Operand).Name)
			.ToArray ();
	}
}
