// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Macios.Analyzers;
using NUnit.Framework;

namespace Microsoft.Macios.Analyzers.Tests {
	[TestFixture]
	public class UIButtonConstructorAnalyzerTests {
		const string bindings = """
			namespace UIKit {
				public enum UIButtonType { Custom, System }
				public class UIButton {
					public UIButton () {}
					public UIButton (UIButtonType type) {}
					public UIButton (int value) {}
					public static UIButton FromType (UIButtonType type) => new UIButton (type);
				}
			}
			""";

		static readonly MetadataReference coreLibrary = MetadataReference.CreateFromFile (typeof (object).Assembly.Location);
		static readonly MetadataReference platform = CreatePlatformReference ();

		static CSharpCompilation CreateCompilation (string source, params MetadataReference [] references)
		{
			return CSharpCompilation.Create ("Test",
				[CSharpSyntaxTree.ParseText (source, new CSharpParseOptions (LanguageVersion.CSharp12))],
				references.Prepend (coreLibrary),
				new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary));
		}

		static MetadataReference CreatePlatformReference ()
		{
			using var stream = new MemoryStream ();
			var result = CreateCompilation (bindings).Emit (stream);
			Assert.That (result.Success, Is.True, string.Join ("\n", result.Diagnostics));
			return MetadataReference.CreateFromImage (stream.ToArray ());
		}

		static async Task<ImmutableArray<Diagnostic>> Analyze (string source, bool referencePlatform = true, bool expectCompilerErrors = false)
		{
			var compilation = CreateCompilation (source, referencePlatform ? [platform] : []);
			var errors = compilation.GetDiagnostics ().Where (v => v.Severity == DiagnosticSeverity.Error).ToArray ();
			if (expectCompilerErrors)
				Assert.That (errors, Is.Not.Empty);
			else
				Assert.That (errors, Is.Empty);
			return await compilation.WithAnalyzers (ImmutableArray.Create<DiagnosticAnalyzer> (new UIButtonConstructorAnalyzer ())).GetAnalyzerDiagnosticsAsync ();
		}

		[TestCase ("class Derived : UIButton { public Derived () : base (UIButtonType.System) {} }", ": base (UIButtonType.System)")]
		[TestCase ("class Derived : UIButton { public Derived (UIButtonType type) : base (type: type) {} }", ": base (type: type)")]
		[TestCase ("class Derived : UIButton { public Derived () : base ((UIButtonType) 0) {} }", ": base ((UIButtonType) 0)")]
		[TestCase ("class Derived : UIButton { public Derived () : this (UIButtonType.System) {} public Derived (UIButtonType type) : base (type) {} }", ": base (type)")]
		[TestCase ("class Derived : UIButton { public Derived () : base (UIButtonType.Custom) {} } class FurtherDerived : Derived {}", ": base (UIButtonType.Custom)")]
		[TestCase ("class Derived (UIButtonType type) : UIButton (type);", "UIButton (type)")]
		[TestCase ("class Derived () : global::UIKit.UIButton (global::UIKit.UIButtonType.System);", "global::UIKit.UIButton (global::UIKit.UIButtonType.System)")]
		[TestCase ("class Derived<T> : UIButton { public Derived () : base (UIButtonType.Custom) {} }", ": base (UIButtonType.Custom)")]
		[TestCase ("using Button = UIKit.UIButton; class Derived : Button { public Derived () : base (UIButtonType.Custom) {} }", ": base (UIButtonType.Custom)")]
		public async Task UnsafeBaseConstructor (string source, string expectedSpan)
		{
			var diagnostics = await Analyze ("using UIKit;\n" + source);
			Assert.That (diagnostics, Has.Length.EqualTo (1));
			var diagnostic = diagnostics [0];
			Assert.That (diagnostic.Id, Is.EqualTo ("APL1000"));
			Assert.That (diagnostic.Severity, Is.EqualTo (DiagnosticSeverity.Warning));
			Assert.That (diagnostic.Location.SourceTree?.GetText ().ToString (diagnostic.Location.SourceSpan), Is.EqualTo (expectedSpan));
			Assert.That (diagnostic.GetMessage (), Is.EqualTo ("UIButton(UIButtonType) cannot initialize a UIButton subclass because it wraps an object returned by a native factory. Use a supported base constructor when subclassing."));
		}

		[TestCase ("class Test { UIButton button = new UIButton (UIButtonType.System); }")]
		[TestCase ("class Test { UIButton button = new (UIButtonType.Custom); }")]
		[TestCase ("class Test { UIButton button = UIButton.FromType (UIButtonType.System); }")]
		[TestCase ("class Derived : UIButton { public Derived () : base () {} }")]
		[TestCase ("class Derived : UIButton {}")]
		[TestCase ("class Derived : UIButton { public Derived () : base (42) {} }")]
		[TestCase ("class Derived (int value) : UIButton (value);")]
		[TestCase ("class Derived : UIButton { public Derived (UIButtonType type) : base () {} } class FurtherDerived : Derived { public FurtherDerived () : base (UIButtonType.Custom) {} }")]
		[TestCase ("class Derived : UIButton { public Derived () : this (UIButtonType.Custom) {} public Derived (UIButtonType type) : base () {} }")]
		[TestCase ("class Other { public Other (UIButtonType type) {} } class Derived : Other { public Derived () : base (UIButtonType.Custom) {} }")]
		[TestCase ("namespace Other { enum UIButtonType { Custom } class UIButton { public UIButton (UIButtonType type) {} } class Derived : UIButton { public Derived () : base (UIButtonType.Custom) {} } }")]
		public async Task SafeConstruction (string source)
		{
			Assert.That (await Analyze ("using UIKit;\n" + source), Is.Empty);
		}

		[Test]
		public async Task NoUIKitReference ()
		{
			Assert.That (await Analyze ("class Test {}", referencePlatform: false), Is.Empty);
		}

		[TestCase ("class Derived : UIKit.UIButton { public Derived () : base (\"invalid\") {} }")]
		[TestCase ("class Derived () : UIKit.UIButton (missing);")]
		public async Task UnresolvedConstructor (string source)
		{
			Assert.That (await Analyze (source, expectCompilerErrors: true), Is.Empty);
		}

		[Test]
		public async Task GeneratedCode ()
		{
			Assert.That (await Analyze ("""
				// <auto-generated/>
				class Derived : UIKit.UIButton {
					public Derived () : base (UIKit.UIButtonType.Custom) {}
				}
				"""), Is.Empty);
		}

		[Test]
		public async Task SuppressedWarning ()
		{
			Assert.That (await Analyze ("""
				using UIKit;
				#pragma warning disable APL1000
				class Derived : UIButton {
					public Derived () : base (UIButtonType.Custom) {}
				}
				"""), Is.Empty);
		}
	}
}
