// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using ClangSharp;
using ClangSharp.Interop;
using Extrospection;
using Mono.Cecil;
using Sharpie.Bind;

namespace Xamarin.Tests;

[TestFixture]
[NonParallelizable]
public class DeprecatedTests {
	const string Framework = "DeprecatedTests";

	[TestCase (false, false, true)]
	[TestCase (true, false, false)]
	[TestCase (true, true, true)]
	public void ClassAndCategoryNames (bool nativeCategory, bool managedCategory, bool expectedDiagnostic)
	{
		var directory = Cache.CreateTemporaryDirectory ();
		var header = Path.Combine (directory, "Surface.h");
		File.WriteAllText (header, nativeCategory ? """
			@interface Context
			@end
			__attribute__((availability(macos,introduced=10.0,deprecated=13.0)))
			@interface Context (Surface)
			@end
			""" : """
			__attribute__((availability(macos,introduced=10.0,deprecated=13.0)))
			@interface Surface
			@end
			""");

		using var module = ModuleDefinition.CreateModule (Framework, ModuleKind.Dll);
		var flags = TypeAttributes.Public;
		if (managedCategory)
			flags |= TypeAttributes.Abstract | TypeAttributes.Sealed;
		var type = new TypeDefinition (Framework, "Surface", flags, module.TypeSystem.Object);
		module.Types.Add (type);
		var attributeType = new TypeReference ("Tests", managedCategory ? "CategoryAttribute" : "RegisterAttribute", module, module);
		var constructor = new MethodReference (".ctor", module.TypeSystem.Void, attributeType) { HasThis = true };
		var attribute = new CustomAttribute (constructor);
		if (managedCategory) {
			attribute.Properties.Add (new CustomAttributeNamedArgument ("Name", new CustomAttributeArgument (module.TypeSystem.String, "Surface")));
		} else {
			constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.String));
			attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, "Surface"));
		}
		type.CustomAttributes.Add (attribute);

		var previousPlatform = Helpers.Platform;
		Helpers.Platform = Platforms.macOS;
		Log.On (Framework).Clear ();
		try {
			using var index = CXIndex.Create ();
			var result = CXTranslationUnit.TryParse (index, header, ["-x", "objective-c", "-Wno-objc-root-class"], [],
				CXTranslationUnit_Flags.CXTranslationUnit_IncludeAttributedTypes | CXTranslationUnit_Flags.CXTranslationUnit_VisitImplicitAttributes, out var handle);
			Assert.That (result, Is.EqualTo (CXErrorCode.CXError_Success), "Parsing synthetic header");
			using var unit = TranslationUnit.GetOrCreate (handle);
			Assert.That (unit.Handle.NumDiagnostics, Is.Zero, "Synthetic header diagnostics");
			var check = new DeprecatedCheck (new BindingResult ());
			check.VisitManagedType (type);
			check.Visit (unit.TranslationUnitDecl);
			check.EndVisit ();
			Assert.That (Log.On (Framework), Is.EqualTo (expectedDiagnostic
				? new [] { "!deprecated-attribute-missing! Surface missing a [Deprecated] attribute" }
				: []));
		} finally {
			Log.On (Framework).Clear ();
			Helpers.Platform = previousPlatform;
			Directory.Delete (directory, true);
		}
	}
}
