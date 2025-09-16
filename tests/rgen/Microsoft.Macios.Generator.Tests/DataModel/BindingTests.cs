// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using Constructor = Microsoft.Macios.Generator.DataModel.Constructor;
using Method = Microsoft.Macios.Generator.DataModel.Method;
using Property = Microsoft.Macios.Generator.DataModel.Property;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class BindingTests : BaseGeneratorTestClass {
	class TestDataSkipEnumValueDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string notAttributeInValue = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

[BindingType]
enum AVMediaCharacteristics {
	Visual = 0,
}
";

			yield return [notAttributeInValue, true];

			const string wrongAttributeInValue = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
enum AVMediaCharacteristics {
	[Field<Property> (""AVMediaCharacteristicVisual"")]
	Visual = 0,
}
";
			yield return [wrongAttributeInValue, true];

			const string presentAttributeInValue = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
enum AVMediaCharacteristics {
	[Field<EnumValue> (""AVMediaCharacteristicVisual"")]
	Visual = 0,
}
";
			yield return [presentAttributeInValue, false];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataSkipEnumValueDeclaration>]
	public void SkipEnumValueDeclaration (ApplePlatform platform, string inputText, bool expected)
	{
		var (compilation, sourceTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<EnumMemberDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		Assert.Equal (expected, Binding.Skip (node, semanticModel));
	}


	class TestDataSkipPropertyDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{

			const string notPartialProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	public string Name { get; set; }
}
";

			yield return [notPartialProperty, true];

			const string missingAttributeInProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	public partial string Name { get; set; }
}
";
			yield return [missingAttributeInProperty, true];

			const string wrongAttributeInProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Field<EnumValue> (""name"")]
	public partial string Name { get;set; }
}
";
			yield return [wrongAttributeInProperty, true];

			const string exportFieldAttributeInProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Export<Field> (""name"")]
	public partial string Name { get;set; }
}
";
			yield return [exportFieldAttributeInProperty, true];

			const string fieldAttributeInProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Field<Property> (""name"")]
	public partial string Name { get;set; }
}
";
			yield return [fieldAttributeInProperty, false];

			const string propertyAttributeInProperty = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Export<Property> (""name"")]
	public partial string Name { get;set; }
}
";
			yield return [propertyAttributeInProperty, false];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataSkipPropertyDeclaration>]
	public void SkipPropertyDeclaration (ApplePlatform platform, string inputText, bool expected)
	{
		var (compilation, sourceTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		Assert.Equal (expected, Binding.PropertySkip (node, semanticModel));
	}

	class TestDataSkipMethodDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string notPartialMethod = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Export<Method> (""name"")]
	public void GetName() {}
}
";
			yield return [notPartialMethod, true];

			const string wrongAttributeFlag = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Export<Property> (""name"")]
	public partial void GetName();
}
";
			yield return [wrongAttributeFlag, true];

			const string correctMethod = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

[BindingType]
public class TestClass {
	[Export<Method> (""name"")]
	public partial void GetName();
}
";
			yield return [correctMethod, false];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataSkipMethodDeclaration>]
	public void SkipMethodDeclaration (ApplePlatform platform, string inputText, bool expected)
	{
		var (compilation, sourceTrees) =
			CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		Assert.Equal (expected, Binding.Skip (node, semanticModel));
	}

	class TestDataTrimForPlatform : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// Binding not supported in platform - should return Default
			var bindingNotSupported = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[UnsupportedOSPlatform (""tvos"")]
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();
	}
}
";
			yield return [bindingNotSupported, ApplePlatform.TVOS, true, 0, 0, 0];

			// All members supported - should return all
			var allMembersSupported = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass () {}

		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();

		[Export<Property> (""property1"")]
		public virtual partial string Property1 { get; set; }
	}
}
";
			yield return [allMembersSupported, ApplePlatform.iOS, false, 1, 1, 1];

			// Mixed platform support - some members excluded
			var mixedSupport = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Constructor> (""init2"")]
		public MyClass (string value);

		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Method> (""method2"")]
		public virtual partial void Method2 ();

		[Export<Property> (""property1"")]
		public virtual partial string Property1 { get; set; }

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Property> (""property2"")]
		public virtual partial string Property2 { get; set; }
	}
}
";
			yield return [mixedSupport, ApplePlatform.TVOS, false, 1, 1, 1];
			yield return [mixedSupport, ApplePlatform.iOS, false, 2, 2, 2];

			// No constructors supported in platform
			var noConstructorsSupported = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[UnsupportedOSPlatform (""tvos"")]
		[Export<Constructor> (""init"")]
		public MyClass ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Constructor> (""init2"")]
		public MyClass (string value);

		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();

		[Export<Property> (""property1"")]
		public virtual partial string Property1 { get; set; }
	}
}
";
			yield return [noConstructorsSupported, ApplePlatform.TVOS, false, 0, 1, 1];

			// No methods supported in platform
			var noMethodsSupported = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Method> (""method2"")]
		public virtual partial void Method2 ();

		[Export<Property> (""property1"")]
		public virtual partial string Property1 { get; set; }
	}
}
";
			yield return [noMethodsSupported, ApplePlatform.TVOS, false, 1, 0, 1];

			// No properties supported in platform
			var noPropertiesSupported = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[Export<Method> (""method1"")]
		public virtual partial void Method1 ();

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Property> (""property1"")]
		public virtual partial string Property1 { get; set; }

		[UnsupportedOSPlatform (""tvos"")]
		[Export<Property> (""property2"")]
		public virtual partial string Property2 { get; set; }
	}
}
";
			yield return [noPropertiesSupported, ApplePlatform.TVOS, false, 1, 1, 0];

			// Platform-specific availability with iOS version constraints
			var versionConstraints = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[SupportedOSPlatform (""ios15.0"")]
		[Export<Method> (""newMethod"")]
		public virtual partial void NewMethod ();

		[SupportedOSPlatform (""ios14.0"")]
		[Export<Property> (""newProperty"")]
		public virtual partial string NewProperty { get; set; }
	}
}
";
			yield return [versionConstraints, ApplePlatform.iOS, false, 1, 1, 1];

			// not supported platform
			var notSupportedPlatform = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	[UnsupportedOSPlatform (""tvos""]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[SupportedOSPlatform (""ios15.0"")]
		[Export<Method> (""newMethod"")]
		public virtual partial void NewMethod ();

		[SupportedOSPlatform (""ios14.0"")]
		[Export<Property> (""newProperty"")]
		public virtual partial string NewProperty { get; set; }
	}
}
";
			yield return [notSupportedPlatform, ApplePlatform.TVOS, true, 0, 0, 0];

			// not supported platform version. It means that we do not return the default binding, just
			// that we have a unsupported version.
			var notSupportedPlatformVersion = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	[UnsupportedOSPlatform (""tvos14.0""]
	public partial class MyClass {
		[Export<Constructor> (""init"")]
		public MyClass ();

		[SupportedOSPlatform (""ios15.0"")]
		[Export<Method> (""newMethod"")]
		public virtual partial void NewMethod ();

		[SupportedOSPlatform (""ios14.0"")]
		[Export<Property> (""newProperty"")]
		public virtual partial string NewProperty { get; set; }
	}
}
";
			yield return [notSupportedPlatformVersion, ApplePlatform.TVOS, false, 1, 1, 1];

			var complexExample = @"
using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	[SupportedOSPlatform (""ios""]
	[SupportedOSPlatform (""macos""]
	[SupportedOSPlatform (""maccatalyst13.0""]
	[UnsupportedOSPlatform (""tvos""]
	public partial class MyClass {

		[SupportedOSPlatform (""tvos14.0""]
		[Export<Constructor> (""init"")]
		public MyClass ();

		[SupportedOSPlatform (""tvos14.0""]
		[SupportedOSPlatform (""ios15.0"")]
		[UnsupportedOSPlatform (""maccatalyst""]
		[Export<Method> (""newMethod"")]
		public virtual partial void NewMethod ();

		[SupportedOSPlatform (""tvos14.0""]
		[SupportedOSPlatform (""ios14.0"")]
		[UnsupportedOSPlatform (""macos""]
		[Export<Property> (""newProperty"")]
		public virtual partial string NewProperty { get; set; }
	}
}
";
			// tvos should return default
			yield return [complexExample, ApplePlatform.TVOS, true, 0, 0, 0];
			// contains all of the constructors, methods andproperties 
			yield return [complexExample, ApplePlatform.iOS, false, 1, 1, 1];
			// missing method
			yield return [complexExample, ApplePlatform.MacCatalyst, false, 1, 0, 1];
			// missing property
			yield return [complexExample, ApplePlatform.MacOSX, false, 1, 1, 0];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataTrimForPlatform>]
	public void TrimForPlatformTest (ApplePlatform platform, string inputText, ApplePlatform targetPlatform,
		bool shouldReturnDefault, int expectedConstructors, int expectedMethods, int expectedProperties)
	{
		var (compilation, sourceTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);

		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var declaration = sourceTrees [0].GetRoot ()
			.DescendantNodes ().OfType<BaseTypeDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		var context = new RootContext (semanticModel);

		var binding = Binding.FromDeclaration (declaration, context);
		Assert.NotNull (binding);

		var trimmedBinding = binding.Value.TrimForPlatform (targetPlatform);

		if (shouldReturnDefault) {
			Assert.True (trimmedBinding.IsNullOrDefault);
			return;
		}

		Assert.False (trimmedBinding.IsNullOrDefault);

		// Validate constructors
		Assert.Equal (expectedConstructors, trimmedBinding.Constructors.Length);
		foreach (var constructor in trimmedBinding.Constructors) {
			Assert.True (constructor.SymbolAvailability.IsSupported (targetPlatform),
				$"Constructor {constructor.Type} should be supported on {targetPlatform}");
		}

		// Validate methods
		Assert.Equal (expectedMethods, trimmedBinding.Methods.Length);
		foreach (var method in trimmedBinding.Methods) {
			Assert.True (method.SymbolAvailability.IsSupported (targetPlatform),
				$"Method {method.Name} should be supported on {targetPlatform}");
		}

		// Validate properties
		Assert.Equal (expectedProperties, trimmedBinding.Properties.Length);
		foreach (var property in trimmedBinding.Properties) {
			Assert.True (property.SymbolAvailability.IsSupported (targetPlatform),
				$"Property {property.Name} should be supported on {targetPlatform}");
		}

		// Ensure no extra members are included by checking original binding
		var originalConstructorsCount = binding.Value.Constructors.Count (c => c.SymbolAvailability.IsSupported (targetPlatform));
		var originalMethodsCount = binding.Value.Methods.Count (m => m.SymbolAvailability.IsSupported (targetPlatform));
		var originalPropertiesCount = binding.Value.Properties.Count (p => p.SymbolAvailability.IsSupported (targetPlatform));

		Assert.Equal (originalConstructorsCount, trimmedBinding.Constructors.Length);
		Assert.Equal (originalMethodsCount, trimmedBinding.Methods.Length);
		Assert.Equal (originalPropertiesCount, trimmedBinding.Properties.Length);
	}

	[Fact]
	public void EnumIndexTest ()
	{
		var bindingInfo = new BindingInfo (new BindingTypeData<ObjCBindings.SmartEnum> ());
		string presentSelector = "AVCaptureDeviceTypeBuiltInMicrophone";
		string missingSelector = "AVCaptureDeviceTypeBuiltInWideAngleCamera";

		var binding = new Binding (
			bindingInfo: bindingInfo,
			name: "TestBinding",
			@namespace: ["TestNamespace"],
			fullyQualifiedSymbol: "TestNamespace.TestBinding",
			symbolAvailability: new ()) {
			EnumMembers = [
				new (
					name: "BuiltInMicrophone",
					libraryName: "AVCaptureDeviceTypeBuiltInMicrophone",
					libraryPath: null,
					fieldData: new (presentSelector),
					symbolAvailability: new (),
					attributes: []),
			],
		};
		EnumMember? member;
		Assert.False (binding.TryGetEnumValue (missingSelector, out member));
		Assert.Null (member);
		Assert.True (binding.TryGetEnumValue (presentSelector, out member));
		Assert.NotNull (member);
	}

	[Fact]
	public void PropertyIndexTest ()
	{
		var bindingInfo = new BindingInfo (new BindingTypeData<ObjCBindings.Class> ());
		string presentSelector = "name";
		string missingSelector = "surname";

		var binding = new Binding (
			bindingInfo: bindingInfo,
			name: "TestBinding",
			@namespace: ["TestNamespace"],
			fullyQualifiedSymbol: "TestNamespace.TestBinding",
			symbolAvailability: new ()) {
			Properties = [
				new (
					name: "Name",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Property>", ["name"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							exportPropertyData: ExportData<ObjCBindings.Property>.Default,
							attributes: [],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							exportPropertyData: ExportData<ObjCBindings.Property>.Default,
							attributes: [],
							modifiers: []
						),
					]
				) {
					ExportPropertyData = new ("name")
				}
			]
		};

		Property? member;
		Assert.False (binding.TryGetProperty (missingSelector, out member));
		Assert.Null (member);
		Assert.True (binding.TryGetProperty (presentSelector, out member));
		Assert.NotNull (member);
	}

	[Fact]
	public void ConstructorIndexTest ()
	{
		var bindingInfo = new BindingInfo (new BindingTypeData<ObjCBindings.Class> ());
		string presentSelector = "initWithName:";
		string missingSelector = "initWithName:Surname:";

		var binding = new Binding (
			bindingInfo: bindingInfo,
			name: "TestBinding",
			@namespace: ["TestNamespace"],
			fullyQualifiedSymbol: "TestNamespace.TestBinding",
			symbolAvailability: new ()) {
			Constructors = [
				new (
					type: "MyClass",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new (presentSelector)
				}
			]
		};

		Constructor? member;
		Assert.False (binding.TryGetConstructor (missingSelector, out member));
		Assert.Null (member);
		Assert.True (binding.TryGetConstructor (presentSelector, out member));
		Assert.NotNull (member);
	}

	[Fact]
	public void EventIndexTest ()
	{
		var bindingInfo = new BindingInfo (new BindingTypeData<ObjCBindings.Class> ());
		string presentSelector = "Changed";
		string missingSelector = "Added";

		var binding = new Binding (
			bindingInfo: bindingInfo,
			name: "TestBinding",
			@namespace: ["TestNamespace"],
			fullyQualifiedSymbol: "TestNamespace.TestBinding",
			symbolAvailability: new ()) {
			Events = [
				new (
					name: "Changed",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							exportPropertyData: ExportData<ObjCBindings.Property>.Default,
							attributes: [],
							modifiers: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							exportPropertyData: ExportData<ObjCBindings.Property>.Default,
							attributes: [],
							modifiers: []
						)
					])
			],
		};

		Event? member;
		Assert.False (binding.TryGetEvent (missingSelector, out member));
		Assert.Null (member);
		Assert.True (binding.TryGetEvent (presentSelector, out member));
		Assert.NotNull (member);
	}

	[Fact]
	public void MethodIndexTest ()
	{
		var bindingInfo = new BindingInfo (new BindingTypeData<ObjCBindings.Class> ());
		string presentSelector = "withName:";
		string missingSelector = "withName:Surname:";

		var binding = new Binding (
			bindingInfo: bindingInfo,
			name: "TestBinding",
			@namespace: ["TestNamespace"],
			fullyQualifiedSymbol: "TestNamespace.TestBinding",
			symbolAvailability: new ()) {
			Methods = [
				new (
					type: "NS.MyClass",
					name: "SetName",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new ("withName:"),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Method>", ["withName:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name")
					]
				),
			]
		};

		Method? member;
		Assert.False (binding.TryGetMethod (missingSelector, out member));
		Assert.Null (member);
		Assert.True (binding.TryGetMethod (presentSelector, out member));
		Assert.NotNull (member);
	}
}

