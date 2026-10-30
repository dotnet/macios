// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Xamarin.Tests;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class FixedPointerEscapeAnalyzerTests {
	static CSharpCompilation CreateCompilation (string source, params string [] defines)
		=> CSharpCompilation.Create (
			"FixedPointerTests",
			[CSharpSyntaxTree.ParseText (source, new CSharpParseOptions (LanguageVersion.Latest, preprocessorSymbols: defines))],
			[MetadataReference.CreateFromFile (typeof (object).Assembly.Location)],
			new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

	static Task<ImmutableArray<Diagnostic>> Analyze (Compilation compilation)
		=> compilation.WithAnalyzers (ImmutableArray.Create<DiagnosticAnalyzer> (new FixedPointerEscapeAnalyzer ()))
			.GetAnalyzerDiagnosticsAsync ();

	[Theory]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return (IntPtr) p;")]
	[InlineData ("byte*", "fixed (byte* p = bytes) return p;")]
	[InlineData ("void*", "fixed (byte* p = bytes) return (void*) (p + 1);")]
	[InlineData ("nint", "fixed (byte* p = bytes) return (nint) p;")]
	[InlineData ("UIntPtr", "fixed (byte* p = bytes) return new UIntPtr (p);")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return new IntPtr (p);")]
	[InlineData ("byte*", "fixed (byte* p = bytes) return &p[1];")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) { var alias = (IntPtr) p; return alias; }")]
	[InlineData ("IntPtr", "IntPtr alias; fixed (byte* p = bytes) alias = (IntPtr) p; return alias;")]
	[InlineData ("IntPtr", "IntPtr alias = IntPtr.Zero; fixed (byte* p = bytes) { if (bytes.Length > 1) alias = (IntPtr) p; } return alias;")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return bytes.Length > 1 ? (IntPtr) p : IntPtr.Zero;")]
	[InlineData ("IntPtr", "IntPtr alias = IntPtr.Zero; fixed (byte* p = bytes) { while (bytes.Length > 1) { alias = (IntPtr) p; break; } } return alias;")]
	[InlineData ("byte*", "fixed (byte* p = bytes, q = bytes) return q;")]
	[InlineData ("long", "fixed (byte* p = bytes) return (long) p + 1;")]
	[InlineData ("nint", "nint alias = 0; fixed (byte* p = bytes) alias += (nint) p; return alias;")]
	[InlineData ("byte*", "fixed (byte* p = bytes, q = bytes) return &(bytes.Length > 1 ? p : q)[1];")]
	[InlineData ("void*", "fixed (byte* p = bytes) return ((IntPtr) p).ToPointer ();")]
	[InlineData ("long", "fixed (byte* p = bytes) return ((IntPtr) p).ToInt64 ();")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return IntPtr.Add ((IntPtr) p, 1);")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return IntPtr.Subtract ((IntPtr) p, 1);")]
	public async Task ReturningFixedPointer (string returnType, string body)
	{
		var compilation = CreateCompilation ($"using System; unsafe class Test {{ byte[] bytes = new byte[4]; {returnType} Method () {{ {body} }} }}");
		Assert.Empty (compilation.GetDiagnostics ().Where (d => d.Severity == DiagnosticSeverity.Error));
		var diagnostic = Assert.Single (await Analyze (compilation));
		Assert.Equal ("RBI0043", diagnostic.Id);
		Assert.Equal (DiagnosticSeverity.Error, diagnostic.Severity);
		Assert.StartsWith ("Do not return a pointer obtained from a fixed statement;", diagnostic.GetMessage ());
	}

	[Theory]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) return Native (p);")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) { Native (p); return IntPtr.Zero; }")]
	[InlineData ("int", "fixed (byte* p = bytes) return *p;")]
	[InlineData ("int", "fixed (byte* p = bytes) return p[1];")]
	[InlineData ("bool", "fixed (byte* p = bytes) return p is not null;")]
	[InlineData ("long", "fixed (byte* p = bytes) return (p + 1) - p;")]
	[InlineData ("IntPtr", "IntPtr alias; fixed (byte* p = bytes) alias = (IntPtr) p; alias = IntPtr.Zero; return alias;")]
	[InlineData ("IntPtr", "fixed (byte* p = bytes) { var alias = (IntPtr) p; alias = IntPtr.Zero; return alias; }")]
	[InlineData ("byte*", "return (byte*) 123;")]
	[InlineData ("IntPtr", "return System.Runtime.InteropServices.Marshal.AllocHGlobal (4);")]
	[InlineData ("byte*", "fixed (byte* p = bytes) return &((byte*) Native (p))[0];")]
	[InlineData ("string", "fixed (byte* p = bytes) return ((IntPtr) p).ToString ();")]
	public async Task NoEscape (string returnType, string body)
	{
		var compilation = CreateCompilation ($"using System; unsafe class Test {{ byte[] bytes = new byte[4]; static IntPtr Native (byte* p) => IntPtr.Zero; {returnType} Method () {{ {body} }} }}");
		Assert.Empty (compilation.GetDiagnostics ().Where (d => d.Severity == DiagnosticSeverity.Error));
		Assert.Empty (await Analyze (compilation));
	}

	[Theory]
	[InlineData ("IntPtr Property { get { fixed (byte* p = bytes) return (IntPtr) p; } }")]
	[InlineData ("IntPtr Method () { IntPtr Local () { fixed (byte* p = bytes) return (IntPtr) p; } return Local (); }")]
	[InlineData ("IntPtr Method () { Func<IntPtr> f = () => { fixed (byte* p = bytes) return (IntPtr) p; }; return f (); }")]
	[InlineData ("Func<IntPtr> Method () => () => { fixed (byte* p = bytes) return (IntPtr) p; };")]
	[InlineData ("Func<IntPtr> Property => () => { fixed (byte* p = bytes) return (IntPtr) p; };")]
	public async Task AccessorsAndNestedFunctions (string member)
	{
		var compilation = CreateCompilation ($"using System; unsafe class Test {{ byte[] bytes = new byte[4]; {member} }}");
		Assert.Empty (compilation.GetDiagnostics ().Where (d => d.Severity == DiagnosticSeverity.Error));
		Assert.Equal ("RBI0043", Assert.Single (await Analyze (compilation)).Id);
	}

	[Theory]
	[InlineData ("__IOS__", false)]
	[InlineData ("__TVOS__", false)]
	[InlineData ("__MACOS__", false)]
	[InlineData ("__MACCATALYST__", false)]
	[InlineData ("__IOS__", true)]
	[InlineData ("__TVOS__", true)]
	[InlineData ("__MACOS__", true)]
	[InlineData ("__MACCATALYST__", true)]
	public async Task RepositorySources (string platform, bool nextMajor)
	{
		var platformDefines = platform switch {
			"__IOS__" => new [] { "IOS" },
			"__TVOS__" => new [] { "TVOS" },
			"__MACOS__" => new [] { "MONOMAC" },
			"__MACCATALYST__" => new [] { "IOS", "__IOS__", "MACCATALYST" },
			_ => throw new ArgumentOutOfRangeException (nameof (platform)),
		};
		var defines = platformDefines.Concat (["NET", platform])
			.Concat (Enumerable.Range (6, Environment.Version.Major - 5).Select (version => $"NET{version}_0_OR_GREATER"))
			.ToArray ();
		if (nextMajor)
			defines = defines.Append ("XAMCORE_5_0").ToArray ();
		var files = new [] { "src", "msbuild", "tools", "tests", "runtime" }
			.SelectMany (directory => Directory.EnumerateFiles (Path.Combine (Configuration.SourceRoot, directory), "*.cs", SearchOption.AllDirectories));
		foreach (var file in files) {
			var source = File.ReadAllText (file);
			if (!source.Contains ("fixed", StringComparison.Ordinal))
				continue;
			var compilation = CreateCompilation (source, defines).AddSyntaxTrees (
				CSharpSyntaxTree.ParseText ("global using System; global using System.Runtime.InteropServices;"));
			var diagnostics = await Analyze (compilation);
			Assert.True (diagnostics.IsEmpty, $"{file}: {string.Join (Environment.NewLine, diagnostics)}");
		}
	}
}
