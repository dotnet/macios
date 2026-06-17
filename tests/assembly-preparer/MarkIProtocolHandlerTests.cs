// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class MarkIProtocolHandlerTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void DynamicRegistrar (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		interface IProtocol {
		}

		class MyClass : NSObject, IProtocol {
		}
		";

		AssertPrepare (platform, isCoreCLR, RegistrarMode.Dynamic, code, out var assemblyDefinition);

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var cctor = type.GetStaticConstructor ();
		Assert.That (cctor, Is.Not.Null, "Expected a static constructor to be added");
		var attribs = cctor.CustomAttributes?.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute").OrderBy (v => string.Join (", ", v.ConstructorArguments.Select (v => v.Value?.ToString ()))).ToArray ();
		Assert.That (attribs, Is.Not.Null, "Attributes");
		Assert.That (attribs.Count, Is.EqualTo (1), "Attribute count");
		// PreserveProtocolsStep adds DDA(DynamicallyAccessedMemberTypes.Interfaces, typeof(MyClass))
		Assert.That ((int) attribs [0].ConstructorArguments [0].Value, Is.EqualTo (0x2000), "First attribute's first argument (DynamicallyAccessedMemberTypes.Interfaces)");
		Assert.That (((TypeReference) attribs [0].ConstructorArguments [1].Value).FullName, Is.EqualTo ("MyClass"), "First attribute's second argument");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void ManagedStaticRegistrar (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		interface IProtocol {
		}

		class MyClass : NSObject, IProtocol {
		}
		";

		// With ManagedStatic registrar, PreserveProtocolsStep doesn't run,
		// so MyClass's cctor should not have any DDA for protocol preservation.
		AssertPrepare (platform, isCoreCLR, RegistrarMode.ManagedStatic, code, out var assemblyDefinition);

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var cctor = type.GetStaticConstructor ();
		if (cctor is not null) {
			var ddaAttribs = cctor.CustomAttributes?.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute").ToArray ();
			Assert.That (ddaAttribs, Is.Empty, "No DynamicDependencyAttributes expected on MyClass's cctor");
		}
	}
}
