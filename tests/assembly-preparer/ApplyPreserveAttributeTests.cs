// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
		using System.Collections.Generic;
		using Foundation;

		interface IMyInterface {
			int MyProperty { get; set; }
			event EventHandler MyEvent;
		}

		interface IMyGenericInterface<T> {
			void MyMethod (T value, List<T> values);
			void MyGenericMethod<U> (U value);
			int MyProperty { get; set; }
			event EventHandler MyEvent;
		}

		[Preserve (AllMembers = true)]
		class MyClass : IMyInterface, IMyGenericInterface<string> {
			int IMyInterface.MyProperty { get; set; }
			event EventHandler IMyInterface.MyEvent {
				add {}
				remove {}
			}
			void IMyGenericInterface<string>.MyMethod (string value, List<string> values) {}
			void IMyGenericInterface<string>.MyGenericMethod<U> (U value) {}
			int IMyGenericInterface<string>.MyProperty {
				get { return 0; }
				set {}
			}
			event EventHandler IMyGenericInterface<string>.MyEvent {
				add {}
				remove {}
			}
		}
		";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var moduleConstructor = assemblyDefinition.MainModule.Types.Single (v => v.Name == "<Module>").GetStaticConstructor ();
		Assert.That (moduleConstructor, Is.Not.Null, "Module constructor");
		var signatures = moduleConstructor.CustomAttributes
			.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute")
			.Where (v => v.ConstructorArguments.Count == 2)
			.Where (v => v.ConstructorArguments [0].Value is string)
			.Where (v => ((TypeReference) v.ConstructorArguments [1].Value).FullName == "MyClass")
			.Select (v => (string) v.ConstructorArguments [0].Value)
			.ToArray ();

		Assert.That (signatures, Does.Contain ("IMyInterface#MyProperty"), "Property");
		Assert.That (signatures, Does.Contain ("IMyInterface#MyEvent"), "Event");
		Assert.That (signatures, Has.None.Contains ("IMyInterface."), "Unescaped explicit interface member");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#MyMethod"), "Generic interface method");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#MyGenericMethod``1"), "Generic interface generic method");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#MyProperty"), "Generic interface property");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#get_MyProperty"), "Generic interface property getter");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#set_MyProperty"), "Generic interface property setter");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#MyEvent"), "Generic interface event");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#add_MyEvent"), "Generic interface event add");
		Assert.That (signatures, Does.Contain ("IMyGenericInterface<System#String>#remove_MyEvent"), "Generic interface event remove");
		Assert.That (signatures, Has.None.Contains ("IMyGenericInterface{"), "Unparseable generic interface member");

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "IMyGenericInterface<System.String>.MyMethod");
		Assert.That (DocumentationComments.GetSignature (method), Is.EqualTo ("IMyGenericInterface<System#String>#MyMethod(System.String,System.Collections.Generic.List{System.String})"), "Generic parameter type");
		var genericMethod = type.Methods.Single (v => v.Name == "IMyGenericInterface<System.String>.MyGenericMethod");
		Assert.That (DocumentationComments.GetSignature (genericMethod), Is.EqualTo ("IMyGenericInterface<System#String>#MyGenericMethod``1(``0)"), "Generic method parameter");
	}
}
