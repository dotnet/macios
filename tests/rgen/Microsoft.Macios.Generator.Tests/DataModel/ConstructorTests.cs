// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class ConstructorTests : BaseGeneratorTestClass {
	class TestDataFromConstructorDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string emptyConstructor = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;	

		[Export<Constructor> (""init"")]
		public TestClass () {
			name = ""Test"";
		}
	}
}
";

			yield return [
				emptyConstructor,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("init", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["init"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string singleParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;	
		[Export<Constructor> (""initWithName:"")]
		public TestClass (string inName) {
			name = inName;
		}
	}
}
";
			yield return [
				singleParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "inName"),
					]
				)
			];

			const string multiParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		[Export<Constructor> (""initWithName:withAge:"")]
		public TestClass (string inName, int inAge) {
			name = inName;
			age = inAge;
		}
	}
}
";

			yield return [
				multiParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
					]
				)
			];

			const string nullableParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		[Export<Constructor> (""initWithName:withAge:"")]
		public TestClass (string? inName, int inAge) {
			name = inName ?? string.Empty;
			age = inAge;
		}
	}
}
";

			yield return [
				nullableParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
					]
				)
			];

			const string paramsCollectionParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, params string[] inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				paramsCollectionParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
						new (position: 2, type: ReturnTypeForArray ("string"), name: "inSurnames") {
							IsParams = true,
						},
					]
				)
			];

			const string arrayParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, string[] inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				arrayParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
						new (position: 2, type: ReturnTypeForArray ("string"), name: "inSurnames") {
							IsParams = false,
						},
					]
				)
			];

			const string nullableArrayParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, string[]? inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				nullableArrayParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
						new (position: 2, type: ReturnTypeForArray ("string", isNullable: true), name: "inSurnames"),
					]
				)
			];

			const string arrayOfNullableParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, string?[] inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				arrayOfNullableParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
						new (position: 2, type: ReturnTypeForArray ("string?"), name: "inSurnames"),
					]
				)
			];

			const string nullableArrayOfNullableParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, string?[]? inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				nullableArrayOfNullableParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, ReturnTypeForString (isNullable: true), "inName"),
						new (1, ReturnTypeForInt (), "inAge"),
						new (2, ReturnTypeForArray ("string?", isNullable: true), "inSurnames"),
					]
				)
			];

			const string twoDimensionalArrayParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		int age;
		string [] surnames;

		[Export<Constructor> (""initWithName:withAge:withSurnames:"")]
		public TestClass (string? inName, int inAge, string[][] inSurnames) {
			name = inName ?? string.Empty;
			age = inAge;
			surnames = inSurnames;
		}
	}
}
";

			yield return [
				twoDimensionalArrayParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:withAge:withSurnames:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:withAge:withSurnames:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName"),
						new (position: 1, type: ReturnTypeForInt (), name: "inAge"),
						new (position: 2, type: ReturnTypeForArray ("string[]"), name: "inSurnames") {
							IsParams = false,
						},
					]
				)
			];

			const string optionalParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass {
		string name;
		[Export<Constructor> (""initWithName:"")]
		public TestClass (string? inName = null) {
			name = inName ?? string.Empty;
		}
	}
}
";

			yield return [
				optionalParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName") {
							IsOptional = true,
						},
					]
				)
			];

			const string genericParameter = @"
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[BindingType<Class>]
	public class TestClass<T> {
		T name;
		[Export<Constructor> (""initWithName:"")]
		public TestClass (T? inName = null) {
			name = T;
		}
	}
}
";

			yield return [
				genericParameter,
				new Constructor (
					type: "TestClass",
					symbolAvailability: new (),
					exportData: new ("initWithName:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForGeneric ("T", isNullable: true), name: "inName") {
							IsOptional = true,
						},
					]
				)
			];

			const string availabilityPresent = @"
using System.Runtime.Versioning;
using System;
using ObjCBindings;
using ObjCRuntime;

namespace NS {
	[SupportedOSPlatform (""ios"")]
	[SupportedOSPlatform (""tvos"")]
	[BindingType<Class>]
	public class TestClass {
		string name;
		[Export<Constructor> (""initWithName:"")]
		public TestClass (string? inName = null) {
			name = inName ?? string.Empty;
		}
	}
}
";
			var builder = SymbolAvailability.CreateBuilder ();
			builder.Add (new SupportedOSPlatformData ("ios"));
			builder.Add (new SupportedOSPlatformData ("tvos"));

			yield return [
				availabilityPresent,
				new Constructor (
					type: "TestClass",
					symbolAvailability: builder.ToImmutable (),
					exportData: new ("initWithName:", ArgumentSemantic.None),
					attributes: [
						new ("ObjCBindings.ExportAttribute<ObjCBindings.Constructor>", ["initWithName:"])
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (position: 0, type: ReturnTypeForString (isNullable: true), name: "inName") {
							IsOptional = true,
						},
					]
				)
			];
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFromConstructorDeclaration>]
	void FromConstructorDeclaration (ApplePlatform platform, string inputText, Constructor expected)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<ConstructorDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Constructor.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		Assert.Equal (expected, changes);
	}
}
