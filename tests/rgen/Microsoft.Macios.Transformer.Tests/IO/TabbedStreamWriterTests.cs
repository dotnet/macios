// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Macios.Generator.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Attributes;

namespace Microsoft.Macios.Transformer.Tests.IO;

public class TabbedStreamWriterTests : IDisposable {
	readonly string tempFile = Path.GetTempFileName ();

	public void Dispose ()
	{
		if (File.Exists (tempFile))
			File.Delete (tempFile);
	}

	string ReadFile ()
	{
		using var reader = new StreamReader (tempFile);
		return reader.ReadToEnd ();
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void ConstructorNotBlockTest (int tabCount, string expectedTabs)
	{
		using (var block = new TabbedStreamWriter (tempFile, tabCount)) {
			block.WriteLine ("Test");
		}
		Assert.Equal ($"{expectedTabs}Test\n", ReadFile ());
	}

	[Theory]
	[InlineData (0, "")]
	[InlineData (1, "\t")]
	[InlineData (5, "\t\t\t\t\t")]
	public void ConstructorBlockTest (int tabCount, string expectedTabs)
	{
		using (var block = new TabbedStreamWriter (tempFile, tabCount, true)) {
			block.WriteLine ("Test");
		}

		Assert.Equal ($"{expectedTabs}{{\n{expectedTabs}\tTest\n{expectedTabs}}}\n", ReadFile ());
	}

	[Fact]
	public void ConstructorBlockNestedTest ()
	{
		using (var block = new TabbedStreamWriter (tempFile, 0, false)) {
			block.WriteLine ("// create the first block");
			using (var block2 = block.CreateBlock ("using (var test1 = new Test ())", true)) {
				block2.WriteLine ("// call in first block");
			}
			block.WriteLine ();
			block.WriteLine ("// create second block");
			using (var block3 = block.CreateBlock ("using (var test2 = new Test ())", true)) {
				block3.WriteLine ("// create nested block");
				using (var block4 = block3.CreateBlock ("using (var test3 = new Test ())", true)) {
					block4.WriteLine ("// code inside test2.test3");
				}
			}
		}

		const string expectedResult = @"// create the first block
using (var test1 = new Test ())
{
	// call in first block
}

// create second block
using (var test2 = new Test ())
{
	// create nested block
	using (var test3 = new Test ())
	{
		// code inside test2.test3
	}
}
";
		Assert.Equal (expectedResult, ReadFile ());
	}

	[Fact]
	public async Task ConstructorBlockNestedAsyncTest ()
	{
		await using (var block = new TabbedStreamWriter (tempFile, 0, false)) {
			await block.WriteLineAsync ("// create the first block");
			await using (var block2 = block.CreateBlock ("using (var test1 = new Test ())", true)) {
				await block2.WriteLineAsync ("// call in first block");
			}
			await block.WriteLineAsync ();
			await block.WriteLineAsync ("// create second block");
			await using (var block3 = block.CreateBlock ("using (var test2 = new Test ())", true)) {
				await block3.WriteLineAsync ("// create nested block");
				await using (var block4 = block3.CreateBlock ("using (var test3 = new Test ())", true)) {
					await block4.WriteLineAsync ("// code inside test2.test3");
				}
			}
		}

		const string expectedResult = @"// create the first block
using (var test1 = new Test ())
{
	// call in first block
}

// create second block
using (var test2 = new Test ())
{
	// create nested block
	using (var test3 = new Test ())
	{
		// code inside test2.test3
	}
}
";
		Assert.Equal (expectedResult, ReadFile ());
	}

	public static IEnumerable<object []> AppendCommentsTriviaTestData ()
	{
		// Case 1: Single line comment
		var trivia1 = SyntaxFactory.ParseLeadingTrivia ("// A single line comment");
		var expected1 = "// A single line comment\n";
		yield return [trivia1, expected1];

		// Case 2: Multi-line comment
		var trivia2 = SyntaxFactory.ParseLeadingTrivia ("/* A multi-line\n   comment */");
		var expected2 = "/* A multi-line\n   comment */";
		yield return [trivia2, expected2];

		// Case 3: Single line doc comment
		var trivia3 = SyntaxFactory.ParseLeadingTrivia ("/// A single line doc comment");
		var expected3 = "/// A single line doc comment\n";
		yield return [trivia3, expected3];

		// Case 4: Multi-line doc comment
		var trivia4 = SyntaxFactory.ParseLeadingTrivia (
			"""
			/*
			* A multi-line doc comment
			*/
			""");
		var expected4 = """
		                /*
		                * A multi-line doc comment
		                */
		                """;
		yield return [trivia4, expected4];

		// Case 5: Mix of comments and other trivia
		var trivia5 = SyntaxFactory.ParseLeadingTrivia (@"
    // A comment
    /// A doc comment
");
		var expected5 = "// A comment\n/// A doc comment\n";
		yield return [trivia5, expected5];

		// Case 6: No comments, only other trivia
		var trivia6 = SyntaxFactory.ParseLeadingTrivia ("\n    ");
		var expected6 = "";
		yield return [trivia6, expected6];

		// Case 7: Empty trivia list
		var trivia7 = new SyntaxTriviaList ();
		var expected7 = "";
		yield return [trivia7, expected7];
	}

	[Theory]
	[MemberData (nameof (AppendCommentsTriviaTestData))]
	public async Task AppendCommentsTrivia (SyntaxTriviaList trivia, string expected)
	{
		await using (var writer = new TabbedStreamWriter (tempFile)) {
			await writer.AppendCommentsTrivia (trivia);
		}
		Assert.Equal (expected, ReadFile ());
	}

	public static IEnumerable<object []> AppendUsingDirectivesTestData ()
	{
		// Case 1: Empty set
		yield return new object [] {
			new HashSet<string>(),
			"using ObjCBindings;\nusing ObjCRuntime;\nusing System.Runtime.Versioning;\n"
		};

		// Case 2: With some directives
		yield return new object [] {
			new HashSet<string> { "System", "System.Collections.Generic" },
			"using ObjCBindings;\nusing ObjCRuntime;\nusing System;\nusing System.Collections.Generic;\nusing System.Runtime.Versioning;\n"
		};

		// Case 3: With duplicates of default directives
		yield return new object [] {
			new HashSet<string> { "ObjCRuntime", "System" },
			"using ObjCBindings;\nusing ObjCRuntime;\nusing System;\nusing System.Runtime.Versioning;\n"
		};

		// Case 4: With all default directives present
		yield return new object [] {
			new HashSet<string> { "ObjCRuntime", "ObjCBindings", "System.Runtime.Versioning" },
			"using ObjCBindings;\nusing ObjCRuntime;\nusing System.Runtime.Versioning;\n"
		};
	}

	[Theory]
	[MemberData (nameof (AppendUsingDirectivesTestData))]
	public async Task AppendUsingDirectives (IReadOnlySet<string> directives, string expected)
	{
		await using (var writer = new TabbedStreamWriter (tempFile)) {
			await writer.AppendUsingDirectives (directives);
		}
		Assert.Equal (expected, ReadFile ());
	}

	public static IEnumerable<object []> AppendMemberAvailabilityTestData ()
	{
		var builder = SymbolAvailability.CreateBuilder ();

		// single platform, available no version
		builder.Add (new SupportedOSPlatformData ("ios"));
		yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"ios\")]\n"];
		builder.Clear ();

		// single platform available with version
		builder.Add (new SupportedOSPlatformData ("macos13.0"));
		yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos13.0\")]\n"];
		builder.Clear ();

		// single platform available with version, unavailable with version
		builder.Add (new SupportedOSPlatformData ("ios"));
		builder.Add (new UnsupportedOSPlatformData ("ios13.0"));
		yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"ios\")]\n[UnsupportedOSPlatform (\"ios13.0\")]\n"];
		builder.Clear ();

		// several platforms available no version
		builder.Add (new SupportedOSPlatformData ("ios"));
		builder.Add (new SupportedOSPlatformData ("tvos"));
		builder.Add (new SupportedOSPlatformData ("macos"));
		yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos\")]\n[SupportedOSPlatform (\"ios\")]\n[SupportedOSPlatform (\"tvos\")]\n"];
		builder.Clear ();

		// several platforms available with version
		builder.Add (new SupportedOSPlatformData ("ios12.0"));
		builder.Add (new SupportedOSPlatformData ("tvos12.0"));
		builder.Add (new SupportedOSPlatformData ("macos10.0"));
		yield return [builder.ToImmutable (), "[SupportedOSPlatform (\"macos10.0\")]\n[SupportedOSPlatform (\"ios12.0\")]\n[SupportedOSPlatform (\"tvos12.0\")]\n"];
		builder.Clear ();

		// several platforms unsupported
		// several platforms available with version
		builder.Add (new UnsupportedOSPlatformData ("ios12.0"));
		builder.Add (new UnsupportedOSPlatformData ("tvos12.0"));
		builder.Add (new UnsupportedOSPlatformData ("macos"));
		yield return [builder.ToImmutable (), "[UnsupportedOSPlatform (\"macos\")]\n[UnsupportedOSPlatform (\"ios12.0\")]\n[UnsupportedOSPlatform (\"tvos12.0\")]\n"];
		builder.Clear ();
	}

	[Theory]
	[MemberData (nameof (AppendMemberAvailabilityTestData))]
	async Task AppendMemberAvailabilityAsyncTest (SymbolAvailability availability, string expected)
	{
		await using (var writer = new TabbedStreamWriter (tempFile)) {
			await writer.AppendMemberAvailabilityAsync (availability);
		}
		Assert.Equal (expected, ReadFile ());
	}
}
