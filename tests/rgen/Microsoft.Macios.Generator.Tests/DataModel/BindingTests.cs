// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
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
