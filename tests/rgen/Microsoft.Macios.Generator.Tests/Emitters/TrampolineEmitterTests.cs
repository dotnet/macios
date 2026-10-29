// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Emitters;
using Microsoft.Macios.Generator.IO;
using Xamarin.Utils;
using Xunit;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class TrampolineEmitterTests : BaseGeneratorTestClass {
	[Theory]
	[InlineData (ApplePlatform.iOS)]
	[InlineData (ApplePlatform.MacOSX)]
	public void SharedNativeObjectInvokers (ApplePlatform platform)
	{
		const string source = """
			#nullable enable
			using Foundation;
			using CoreGraphics;
			namespace Test {
				public delegate void ObjectCallback (NSObject? value);
				public delegate void ImageCallback (CGImage? image);
				public delegate void ProtocolCallback (INSCopying? value);
				public delegate void NonNullableCallback (NSObject value);
				public delegate bool ReturningCallback (NSObject? value);
				public delegate void RefCallback (ref NSObject? value);
				public delegate void ArrayCallback (NSObject []? value);
				public delegate void StringCallback (string? value);
				public delegate void MultipleCallback (NSObject? first, NSObject? second);
				public delegate void ValueCallback (int value);
			}
			""";
		var (compilation, trees) = CreateCompilation (platform, sources: source);
		var model = compilation.GetSemanticModel (trees [0]);
		using var builder = new TabbedStringBuilder (new ());
		var emitter = new TrampolineEmitter (new RootContext (model), builder);
		var types = trees [0].GetRoot ().DescendantNodes ().OfType<DelegateDeclarationSyntax> ()
			.Select (declaration => new TypeInfo (model.GetDeclaredSymbol (declaration)!)).ToHashSet ();
		Assert.True (emitter.TryEmit (types, out var diagnostics));
		Assert.Null (diagnostics);
		var root = CSharpSyntaxTree.ParseText (builder.ToCode ()).GetRoot ();
		var shared = Assert.Single (root.DescendantNodes ().OfType<ClassDeclarationSyntax> (),
			c => c.Identifier.Text == "RgenNativeObjectActionInvoker");
		var invoke = Assert.Single (shared.Members.OfType<MethodDeclarationSyntax> ());
		Assert.Equal ("INativeObject?", invoke.ParameterList.Parameters.Single ().Type!.ToString ());
		Assert.Contains ("obj.GetHandle ()", invoke.ToString ());
		Assert.Contains ("GC.KeepAlive (obj)", invoke.ToString ());
		var sharedClasses = new [] { shared }.ToList ();
		foreach (var name in new [] { "ObjectCallback", "ImageCallback", "ProtocolCallback" }) {
			var invoker = Assert.Single (root.DescendantNodes ().OfType<ClassDeclarationSyntax> (),
				c => c.Identifier.Text == "NID" + name);
			Assert.Equal ("RgenNativeObjectActionInvoker", invoker.BaseList!.Types.Single ().ToString ());
			Assert.DoesNotContain (invoker.Members.OfType<MethodDeclarationSyntax> (), m => m.Identifier.Text == "Invoke");
			var create = Assert.Single (invoker.Members.OfType<MethodDeclarationSyntax> ());
			Assert.Equal ($"global::Test.{name}?", create.ReturnType.ToString ());
			Assert.Contains ("GetExistingManagedDelegate (block)", create.ToString ());
			sharedClasses.Add (invoker);
		}
		foreach (var name in new [] { "NonNullableCallback", "ReturningCallback", "RefCallback", "ArrayCallback", "StringCallback", "MultipleCallback", "ValueCallback" }) {
			var invoker = Assert.Single (root.DescendantNodes ().OfType<ClassDeclarationSyntax> (),
				c => c.Identifier.Text == "NID" + name);
			Assert.Equal ("TrampolineBlockBase", invoker.BaseList!.Types.Single ().ToString ());
			Assert.Contains (invoker.Members.OfType<MethodDeclarationSyntax> (), m => m.Identifier.Text == "Invoke");
		}

		var nativeDelegate = Assert.Single (root.DescendantNodes ().OfType<DelegateDeclarationSyntax> (),
			d => d.Identifier.Text == "RgenNativeObjectAction");
		var sharedSource = "#nullable enable\nusing System;\nusing System.Runtime.InteropServices;\nusing Foundation;\n" +
			"namespace ObjCRuntime { static partial class Trampolines {\n" +
			nativeDelegate.ToFullString () + string.Join ("\n", sharedClasses.Select (c => c.ToFullString ())) + "\n} }";
		using var assembly = new MemoryStream ();
		var result = compilation.AddSyntaxTrees (CSharpSyntaxTree.ParseText (sharedSource)).Emit (assembly);
		Assert.True (result.Success, string.Join ("\n", result.Diagnostics.Where (d => d.Severity == DiagnosticSeverity.Error)));
	}
}
