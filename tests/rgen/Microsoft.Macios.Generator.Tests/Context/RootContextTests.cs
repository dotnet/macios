// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Context;

public class RootContextTests : BaseGeneratorTestClass {
	[Theory]
	[AllSupportedPlatforms ("CoreImage")]
	[AllSupportedPlatforms ("MobileCoreServices")]
	[AllSupportedPlatforms ("PdfKit")]
	public void TryComputeLibraryNameDefaultFrameworkTests (ApplePlatform platform, string ns)
	{
		const string inputText = @"
namespace MyNamespace {
	public class MyClass {
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (compilation.SyntaxTrees);
		var rootContext = new RootContext (compilation.GetSemanticModel (syntaxTrees [0]));
		Assert.True (rootContext.TryComputeLibraryName (null, ns, out var libName, out var libPath));
		Assert.Equal (ns, libName);
		Assert.Null (libPath);
	}

	[Theory]
	[AllSupportedPlatforms ("CoreImageLibrary", "/System/Library/Frameworks/CoreImage.framework/CoreImage")]
	[AllSupportedPlatforms ("PdfKitLibrary", "/System/Library/Frameworks/PDFKit.framework/PDFKit")]
	[PlatformInlineData (ApplePlatform.iOS, "MobileCoreServicesLibrary", "/System/Library/Frameworks/MobileCoreServices.framework/MobileCoreServices")]
	[PlatformInlineData (ApplePlatform.TVOS, "MobileCoreServicesLibrary", "/System/Library/Frameworks/MobileCoreServices.framework/MobileCoreServices")]
	[PlatformInlineData (ApplePlatform.MacCatalyst, "MobileCoreServicesLibrary", "/System/Library/Frameworks/MobileCoreServices.framework/MobileCoreServices")]
	[PlatformInlineData (ApplePlatform.MacOSX, "MobileCoreServicesLibrary", "/System/Library/Frameworks/CoreServices.framework/CoreServices")]
	public void FrameworkLibraryPaths (ApplePlatform platform, string fieldName, string expectedPath)
	{
		var (compilation, _) = CreateCompilation (platform, sources: "class Test {}");
		var constants = compilation.GetTypeByMetadataName ("ObjCRuntime.Constants");
		Assert.NotNull (constants);
		var field = Assert.IsAssignableFrom<IFieldSymbol> (Assert.Single (constants.GetMembers (fieldName)));
		Assert.Equal (expectedPath, field.ConstantValue);
	}

	[Theory]
	[AllSupportedPlatforms ("MyOwnLib", false)]
	[AllSupportedPlatforms ("Test", false)]
	[AllSupportedPlatforms ("CoreData", true)]
	[AllSupportedPlatforms ("GameKit", true)]
	[AllSupportedPlatforms ("NetworkExtension", true)]
	[AllSupportedPlatforms ("MobileCoreServices", true)]
	public void IsSystemLibraryTests (ApplePlatform platform, string lib, bool expectedResult)
	{
		const string inputText = @"
namespace MyNamespace {
	public class MyClass {
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (compilation.SyntaxTrees);
		var rootContext = new RootContext (compilation.GetSemanticModel (syntaxTrees [0]));
		Assert.Equal (rootContext.IsSystemLibrary (lib), expectedResult);
	}
}
