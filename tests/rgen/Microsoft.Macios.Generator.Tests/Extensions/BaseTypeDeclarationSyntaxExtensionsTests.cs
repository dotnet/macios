// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Extensions;

public class BaseTypeDeclarationSyntaxExtensionsTests : BaseGeneratorTestClass, IEnumerable<object []> {

	const string filescopedNamespaceClass = @"
namespace Test;
public class Foo {
}
";

	const string filescopedNamespaceNestedEnum = @"
namespace Test;
public class Foo {
	public enum Bar {
	}	
}
";

	const string namespaceClass = @"
namespace Test {
	public class Foo {
	}
}
";

	const string severalNamespaces = @"
namespace Test {
	public class Foo {}
}
namespace Test2 {
	public class Bar {}
}
";

	const string nestedNamespaces = @"
namespace Foo {
	namespace Bar {
		public class Test {}
	}
}
";

	const string nestedEnum = @"
namespace Foo {
	namespace Bar {
		public class Test {
			public enum Final {
				None,
			}
		}
	}
}
";
	T GetDeclaration<T> (ApplePlatform platform, string inputText, out SemanticModel semanticModel) where T : BaseTypeDeclarationSyntax
	{
		var (compilation, sourceTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		var declaration = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<T> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		return declaration;
	}

	public IEnumerator<object []> GetEnumerator ()
	{
		foreach (var platform in Configuration.GetIncludedPlatforms ()) {
			yield return [GetDeclaration<ClassDeclarationSyntax> (platform, filescopedNamespaceClass, out SemanticModel semanticModel),
				semanticModel,
				"Test.Foo"];
			yield return [GetDeclaration<EnumDeclarationSyntax> (platform, filescopedNamespaceNestedEnum, out semanticModel),
				semanticModel,
				"Test.Foo.Bar"];
			yield return [GetDeclaration<ClassDeclarationSyntax> (platform, namespaceClass, out semanticModel),
				semanticModel,
				"Test.Foo"];
			yield return [GetDeclaration<ClassDeclarationSyntax> (platform, severalNamespaces, out semanticModel),
				semanticModel,
				"Test.Foo"];
			yield return [GetDeclaration<ClassDeclarationSyntax> (platform, nestedNamespaces, out semanticModel),
				semanticModel,
				"Foo.Bar.Test"];
			yield return [GetDeclaration<EnumDeclarationSyntax> (platform, nestedEnum, out semanticModel),
				semanticModel,
				"Foo.Bar.Test.Final"];
		}
	}

	IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

	[Theory]
	[ClassData (typeof (BaseTypeDeclarationSyntaxExtensionsTests))]
	public void GetFullyQualifiedIdentifier<T> (T declaration, SemanticModel semanticModel, string expected)
		where T : BaseTypeDeclarationSyntax
	{
		Assert.Equal (expected, declaration.GetFullyQualifiedIdentifier (semanticModel));
	}
}
