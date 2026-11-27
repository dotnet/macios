// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Sharpie.Bind.Tests;

public class CommandLineTests {
	const string headerCode = """
		#ifdef __OBJC__
		@interface CommandLineClass
		@property int Value;
		@end
		#endif
		""";

	static BindTool CreateTool (string outputDirectory, params string [] arguments)
	{
		Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.MacOSX);
		var args = new List<string> {
			"--output=" + outputDirectory,
			"--sdk=macosx",
			"--clang-resource-dir=" + Extensions.GetClangResourceDirectory (),
			"--platform-assembly=" + Extensions.GetPlatformAssemblyPath (ApplePlatform.MacOSX),
		};
		args.AddRange (arguments);
		return Tools.CreateBindTool (args.ToArray ()) ?? throw new InvalidOperationException ("Expected a bind tool.");
	}

	[Test]
	public void RelativeHeaderWithScope ()
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var headers = Path.Combine (tmpdir, "Headers");
		Directory.CreateDirectory (headers);
		var header = Path.Combine (headers, "Test-Swift.h");
		File.WriteAllText (header, headerCode);
		var relativeHeader = Path.GetRelativePath (Environment.CurrentDirectory, header);
		var relativeScope = Path.GetRelativePath (Environment.CurrentDirectory, headers);
		var output = Path.Combine (tmpdir, "output");
		using var binder = CreateTool (output, "--scope=" + relativeScope, "--header=" + relativeHeader);
		Assert.That (binder.SourceFile, Is.EqualTo (relativeHeader), "Source file before parsing");
		var result = binder.BindInOrOut ();
		result.AssertSuccess (null);
		result.AssertNoWarnings ();
		Assert.That (File.ReadAllText (binder.OutputApiDefinitionPath), Does.Contain ("interface CommandLineClass"), "Binding");
	}

	[TestCase ("--framework=")]
	[TestCase ("--header=")]
	public void FrameworkInput (string option)
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var framework = Path.Combine (tmpdir, "CommandLineKit.framework");
		var headers = Path.Combine (framework, "Headers");
		Directory.CreateDirectory (headers);
		File.WriteAllText (Path.Combine (headers, "CommandLineKit.h"), headerCode);
		var relativeFramework = Path.GetRelativePath (Environment.CurrentDirectory, framework) + Path.DirectorySeparatorChar;
		using var binder = CreateTool (Path.Combine (tmpdir, "output"), option + relativeFramework);
		Assert.That (binder.SourceFramework, Is.EqualTo (relativeFramework), "Source framework");
		var result = binder.BindInOrOut ();
		result.AssertSuccess (null);
		result.AssertNoWarnings ();
		var code = File.ReadAllText (binder.OutputApiDefinitionPath);
		Assert.That (code, Does.Contain ("namespace CommandLineKit"), "Namespace");
		Assert.That (code, Does.Contain ("interface CommandLineClass"), "Binding");
	}

	[TestCase ("-c")]
	[TestCase ("--clang")]
	public void CompilerArguments (string delimiter)
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var clangArguments = new [] { "-I", "include", "--scope=compiler", "Other.h", "--clang" };
		var arguments = new List<string> { "--header=Input.h", delimiter };
		arguments.AddRange (clangArguments);
		using var binder = CreateTool (tmpdir, arguments.ToArray ());
		Assert.That (binder.SourceFile, Is.EqualTo ("Input.h"), "Source file");
		Assert.That (binder.DirectoriesInScope, Is.Empty, "Sharpie scope");
		Assert.That (binder.ClangArguments, Is.EqualTo (clangArguments), "Compiler arguments");
	}

	[TestCase ("-I", "include")]
	[TestCase ("-isysroot", "/path/to/sdk")]
	[TestCase ("-x", "objective-c")]
	[TestCase ("-include", "Other.h")]
	[TestCase ("-include-pch", "Prefix.pch")]
	[TestCase ("-ivfsoverlay", "overlay.yaml")]
	[TestCase ("-working-directory", "work")]
	[TestCase ("-iframeworkwithsysroot", "/path/to/frameworks")]
	[TestCase ("-isystem-after", "/path/to/include")]
	[TestCase ("-Xclang", "-Werror")]
	public void CompilerArgumentsBeforeDelimiter (string option, string value)
	{
		using var binder = CreateTool (Cache.CreateTemporaryDirectory (), option, value, "--header=Input.h");
		Assert.That (binder.SourceFile, Is.EqualTo ("Input.h"), "Source file");
		Assert.That (binder.ClangArguments, Is.EqualTo (new [] { option, value }), "Compiler arguments");
		Assert.That (binder.BindingResult.Errors, Is.Empty, "Parsing errors");
	}

	[Test]
	public void CompilerFlagBeforeHeader ()
	{
		using var binder = CreateTool (Cache.CreateTemporaryDirectory (), "-Werror", "--header=Input.h");
		Assert.That (binder.SourceFile, Is.EqualTo ("Input.h"), "Source file");
		Assert.That (binder.ClangArguments, Is.EqualTo (new [] { "-Werror" }), "Compiler arguments");
		Assert.That (binder.BindingResult.Errors, Is.Empty, "Parsing errors");
	}

	[Test]
	public void ResponseFile ()
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var responseFile = Path.Combine (tmpdir, "arguments.rsp");
		File.WriteAllText (responseFile, "--header=Input.h\n--namespace=FromResponseFile\n--clang\n-I\ninclude\nOther.h\n");
		using var binder = CreateTool (tmpdir, "@" + responseFile);
		Assert.That (binder.SourceFile, Is.EqualTo ("Input.h"), "Source file");
		Assert.That (binder.Namespace, Is.EqualTo ("FromResponseFile"), "Namespace");
		Assert.That (binder.ClangArguments, Is.EqualTo (new [] { "-I", "include", "Other.h" }), "Compiler arguments");
	}

	[TestCase ("--header=Input.h", "--header=Other.h", 20)]
	[TestCase ("--framework=Input.framework", "--framework=Other.framework", 20)]
	[TestCase ("--framework=Input.framework", "--header=Other.h", 16)]
	public void MultipleInputs (string first, string second, int error)
	{
		using var binder = CreateTool (Cache.CreateTemporaryDirectory (), first, second);
		var result = binder.Bind ();
		result.AssertErrors ((error, error == 16 ? "Cannot specify both a source framework and a source file." : "Only one input header or framework can be specified.", null, null));
		result.AssertNoWarnings ();
	}

	[Test]
	public void MissingInput ()
	{
		using var binder = new BindTool ();
		var result = binder.Bind ();
		result.AssertErrors ((19, "An input header or framework must be specified with --header or --framework.", null, null));
		result.AssertNoWarnings ();
	}

	[TestCase ("Input.h")]
	[TestCase ("Input.framework")]
	public void PositionalInput (string input)
	{
		using var binder = CreateTool (Cache.CreateTemporaryDirectory (), input);
		var result = binder.Bind ();
		result.AssertErrors ((19, "An input header or framework must be specified with --header or --framework.", null, null));
		result.AssertNoWarnings ();
	}

	[TestCase (false, false)]
	[TestCase (false, true)]
	[TestCase (true, false)]
	[TestCase (true, true)]
	public void EmptyOutput (bool split, bool outOfScope)
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var header = Path.Combine (tmpdir, "Test.h");
		File.WriteAllText (header, outOfScope ? headerCode : "");
		var output = Path.Combine (tmpdir, "output");
		using var binder = CreateTool (output, "--header=" + header, "--namespace=EmptyNamespace");
		binder.SplitDocuments = split;
		if (outOfScope)
			binder.DirectoriesInScope.Add (Path.Combine (tmpdir, "OtherHeaders"));
		var result = binder.BindInOrOut ();
		result.AssertSuccess ("");
		result.AssertWarnings ((1001, "No bindings were generated. Check that the input contains bindable declarations, that the correct language is selected, and that --scope includes the input headers.", null, null));
		Assert.That (File.Exists (binder.OutputApiDefinitionPath), Is.False, "No API definition file");
		Assert.That (File.Exists (binder.OutputStructsAndEnumsPath), Is.False, "No structs and enums file");
	}

	[Test]
	public void UnavailableOutput ()
	{
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var header = Path.Combine (tmpdir, "Test.h");
		File.WriteAllText (header, "__attribute__((unavailable))\n@interface UnavailableClass\n@end\n");
		using var binder = CreateTool (Path.Combine (tmpdir, "output"), "--header=" + header, "--namespace=EmptyNamespace");
		var result = binder.BindInOrOut ();
		result.AssertSuccess ("");
		result.AssertWarnings ((1001, "No bindings were generated. Check that the input contains bindable declarations, that the correct language is selected, and that --scope includes the input headers.", null, null));
		Assert.That (File.Exists (binder.OutputApiDefinitionPath), Is.False, "No API definition file");
	}
}
