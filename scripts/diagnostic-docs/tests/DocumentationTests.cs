// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Text;
using System.Text.Json;

using NUnit.Framework;

namespace DiagnosticDocumentation.Tests;

[TestFixture]
public class DocumentationTests {
	string root = "";
	string docs = "";

	[SetUp]
	public void SetUp ()
	{
		root = Path.Combine (Path.GetTempPath (), "diagnostic docs " + Guid.NewGuid ().ToString ("N"));
		docs = Path.Combine (root, "docs");
		Directory.CreateDirectory (Path.Combine (docs, "messages"));
		Directory.CreateDirectory (Path.Combine (docs, "website"));
		Directory.CreateDirectory (Path.Combine (docs, "includes", "diagnostics"));
		Directory.CreateDirectory (Path.Combine (root, "tools", "mtouch"));
		Directory.CreateDirectory (Path.Combine (root, "src"));
		File.WriteAllText (Path.Combine (root, "tools", "mtouch", "Errors.resx"), "<root><data name=\"MX0003\"><value>Shared message</value></data></root>");
		File.WriteAllText (Path.Combine (root, "src", "Resources.resx"), "<root/>");
		WriteJson ("allocations.json", new [] { new Allocation ("MX0003", ["MT0003", "MM0003"]) });
		WriteJson<Renumbering []> ("renumberings.json", []);
		WriteJson ("legacy-indexes.json", new [] {
			new LegacyIndex ("mtouch-errors.md", "---\ntitle: \"Legacy\"\n---", "# Legacy", [
				new LegacyCategory ("## MT0xxx: tool errors", [new LegacyEntry ("MT0003", ["### MT0003: Shared message"])])
			])
		});
		WritePage ("MT0003");
		WritePage ("MM0003");
	}

	[TearDown]
	public void TearDown ()
	{
		Directory.Delete (root, true);
	}

	void WriteJson<T> (string name, T value)
	{
		File.WriteAllText (Path.Combine (docs, "messages", name), JsonSerializer.Serialize (value));
	}

	void WritePage (string code, string meaning = "shared", string body = "\n## Issue\n\nAn issue.\n\n## Solution\n\nA solution.\n")
	{
		var metadata = new Dictionary<string, object> {
			["title"] = code,
			["description"] = code + " diagnostic",
			["ms.date"] = "10/19/2026",
			["f1_keywords"] = new [] { code },
			["diagnostic_message"] = "Shared message",
			["diagnostic_id"] = meaning,
			["diagnostic_status"] = "current",
			["diagnostic_category"] = code [..2] + "0xxx: tool errors",
		};
		var text = "---\n" + string.Concat (metadata.Select (v => v.Key + ": " + JsonSerializer.Serialize (v.Value) + "\n")) + "---\n\n# " + code + "\n" + body;
		File.WriteAllText (Path.Combine (docs, "messages", code.ToLowerInvariant () + ".md"), text);
	}

	Documentation Validated (string? source = null)
	{
		var documentation = new Documentation (root, source ?? docs);
		documentation.Validate ();
		return documentation;
	}

	[Test]
	public void GeneratesDeterministicallyAndChecksWithoutWriting ()
	{
		Validated ().Generate (true, false);
		var index = Path.Combine (docs, "messages", "index.md");
		var expected = File.ReadAllText (index);
		var timestamp = File.GetLastWriteTimeUtc (index);
		Validated ().Generate (false, false);
		Assert.That (File.GetLastWriteTimeUtc (index), Is.EqualTo (timestamp), "check must not rewrite files");
		Validated ().Generate (true, false);
		Assert.That (File.ReadAllText (index), Is.EqualTo (expected));
		var legacy = File.ReadAllText (Path.Combine (docs, "website", "mtouch-errors.md"));
		Assert.That (legacy, Does.Contain ("name=\"MT0003\""));
		Assert.That (legacy, Does.Contain ("id=\"mt0003-shared-message\""));
		Assert.That (legacy, Does.Contain ("https://learn.microsoft.com/dotnet/ios/messages/mt0003"));
	}

	[Test]
	public void AcceptsAllPublicPrefixes ()
	{
		WritePage ("BI0003");
		WritePage ("MX0003");
		Assert.DoesNotThrow (() => Validated ());
	}

	[Test]
	public void RejectsConflictingCurrentMeanings ()
	{
		WritePage ("MM0003", "unrelated");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("Conflicting current diagnostic number"));
	}

	[Test]
	public void RejectsMissingResourceDocumentation ()
	{
		File.Delete (Path.Combine (docs, "messages", "mm0003.md"));
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("no documentation for MM0003"));
	}

	[Test]
	public void RejectsNewUnmappedResource ()
	{
		File.WriteAllText (Path.Combine (root, "src", "Resources.resx"), "<root><data name=\"BI1234\"><value>New error</value></data></root>");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("Undocumented diagnostic resource allocations: BI1234"));
	}

	[Test]
	public void RejectsAnAllocationMappedToAnUnrelatedNumber ()
	{
		WriteJson ("allocations.json", new [] { new Allocation ("MX0003", ["MT0004"]) });
		WritePage ("MT0004");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("Invalid public-code mapping"));
	}

	[Test]
	public void RejectsAnUndocumentedNumericEmitter ()
	{
		var directory = Path.Combine (root, "src", "bgen");
		Directory.CreateDirectory (directory);
		File.WriteAllText (Path.Combine (directory, "Emitter.cs"), "throw ErrorHelper.CreateError (1234, argument);");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("undocumented emitted diagnostic BI1234"));
	}

	[TestCase ("Log.LogError (1234, null, message);")]
	[TestCase ("log.LogWarning (1234, null, message);")]
	[TestCase ("Log.LogMessage (MessageImportance.Low, 1234, null, 0, message);")]
	public void RejectsAnUndocumentedMsBuildEmitter (string statement)
	{
		var directory = Path.Combine (root, "msbuild", "Xamarin.MacDev.Tasks");
		Directory.CreateDirectory (directory);
		File.WriteAllText (Path.Combine (directory, "Emitter.cs"), statement);
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("undocumented emitted diagnostic MT1234"));
	}

	[Test]
	public void RequiresCurrentDocumentationForAnMsBuildEmitter ()
	{
		var path = Path.Combine (docs, "messages", "mt0003.md");
		File.WriteAllText (path, File.ReadAllText (path).Replace ("\"current\"", "\"historical\""));
		var directory = Path.Combine (root, "msbuild", "Xamarin.MacDev.Tasks");
		Directory.CreateDirectory (directory);
		File.WriteAllText (Path.Combine (directory, "Emitter.cs"), "Log.LogError (3, null, message);");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("emitted MSBuild diagnostic MT0003 must have current documentation"));
	}

	[Test]
	public void RejectsAnEmitterUsingARetiredBindingNumber ()
	{
		var path = Path.Combine (docs, "messages", "bi0003.md");
		WritePage ("BI0003");
		File.WriteAllText (path, File.ReadAllText (path).Replace ("\"current\"", "\"historical\""));
		var directory = Path.Combine (root, "src", "bgen");
		Directory.CreateDirectory (directory);
		File.WriteAllText (Path.Combine (directory, "Emitter.cs"), "throw new BindingException (3, true, argument);");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("emits historical binding diagnostic BI0003"));
	}

	[Test]
	public void PreservesAliasesAndDuplicateHeadingFragments ()
	{
		WriteJson ("legacy-indexes.json", new [] {
			new LegacyIndex ("mtouch-errors.md", "---\ntitle: \"Legacy\"\n---", "# Legacy", [
				new LegacyCategory ("## MT0xxx: tool errors", [new LegacyEntry ("MT0003", ["### MT0003: Shared message", "### MT0003: Shared message"])])
			], new Dictionary<string, string> { ["MM202x"] = "MM0003" })
		});
		Validated ().Generate (true, false);
		var legacy = File.ReadAllText (Path.Combine (docs, "website", "mtouch-errors.md"));
		Assert.That (legacy, Does.Contain ("name=\"MM202x\""));
		Assert.That (legacy, Does.Contain ("id=\"mt0003-shared-message-1\""));
	}

	[TestCase ("diagnostic_status: \"current\"", "diagnostic_status: \"maybe\"")]
	[TestCase ("f1_keywords: [\"MT0003\"]", "f1_keywords: [\"MM0003\"]")]
	[TestCase ("diagnostic_id: \"shared\"", "diagnostic_id: \"\"")]
	public void RejectsInvalidMetadata (string from, string to)
	{
		var path = Path.Combine (docs, "messages", "mt0003.md");
		File.WriteAllText (path, File.ReadAllText (path).Replace (from, to));
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ());
	}

	[Test]
	public void RejectsDuplicateMetadata ()
	{
		var path = Path.Combine (docs, "messages", "mt0003.md");
		File.WriteAllText (path, File.ReadAllText (path).Replace ("diagnostic_status:", "title: \"duplicate\"\ndiagnostic_status:"));
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("duplicate metadata"));
	}

	[Test]
	public void ExpandsSharedIncludes ()
	{
		var include = Path.Combine (docs, "includes", "diagnostics", "shared.md");
		File.WriteAllText (include, "\n## Issue\n\nShared issue\n\n## Solution\n\nShared solution\n");
		WritePage ("MT0003", body: "\n[!INCLUDE [Shared](../includes/diagnostics/shared.md)]\n");
		Assert.DoesNotThrow (() => Validated ());
	}

	[Test]
	public void RejectsCyclicIncludes ()
	{
		File.WriteAllText (Path.Combine (docs, "includes", "diagnostics", "shared.md"), "[!INCLUDE [Shared](shared.md)]");
		WritePage ("MT0003", body: "\n[!INCLUDE [Shared](../includes/diagnostics/shared.md)]\n");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("Cyclic"));
	}

	[TestCase ("[Missing](missing.md)", "missing link target")]
	[TestCase ("[!INCLUDE [Missing](../includes/diagnostics/missing.md)]", "missing link target")]
	[TestCase ("[Old](~/ios/foo.md)", "legacy site-root")]
	public void RejectsBrokenLinks (string link, string error)
	{
		WritePage ("MT0003", body: "\n## Issue\n\n" + link + "\n\n## Solution\n\nFix it.\n");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains (error));
	}

	[Test]
	public void RejectsStaleIndex ()
	{
		var documentation = Validated ();
		documentation.Generate (true, false);
		File.AppendAllText (Path.Combine (docs, "messages", "index.md"), "An independent index edit.\n");
		Assert.That (() => documentation.Generate (false, false), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("index-only edits"));
	}

	[Test]
	public void ReservesRenumberedDiagnostic ()
	{
		WriteJson ("renumberings.json", new [] { new Renumbering ("MT0003", "MT0004", "A genuine conflict.") });
		WritePage ("MT0004");
		Assert.That (() => Validated (), Throws.TypeOf<InvalidDataException> ().With.Message.Contains ("historical diagnostic reservation"));
	}

	[Test]
	public void CommandReportsInvalidInputWithoutAnUnhandledException ()
	{
		Assert.That (Program.Main (["check", root]), Is.EqualTo (1), "missing generated index");
		Assert.That (Program.Main ([]), Is.EqualTo (1), "invalid arguments");
	}

	static string RepositoryRoot ()
	{
		var path = new DirectoryInfo (TestContext.CurrentContext.TestDirectory);
		while (path is not null && !File.Exists (Path.Combine (path.FullName, "docs", "sync-mobile-docs.sh")))
			path = path.Parent;
		return path?.FullName ?? throw new InvalidOperationException ("Cannot find the repository root.");
	}

	void PrepareSyncFixture ()
	{
		var repository = RepositoryRoot ();
		foreach (var name in new [] { "sync-mobile-docs.sh", "update-diagnostic-docs.sh" })
			File.Copy (Path.Combine (repository, "docs", name), Path.Combine (docs, name));
		var project = Path.Combine (root, "scripts", "diagnostic-docs");
		Directory.CreateDirectory (project);
		File.Copy (Path.Combine (repository, "scripts", "diagnostic-docs", "Program.cs"), Path.Combine (project, "Program.cs"));
		File.WriteAllText (Path.Combine (project, "diagnostic-docs.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net$(BundledNETCoreAppTargetFrameworkVersion)</TargetFramework><OutputType>Exe</OutputType><ImplicitUsings>enable</ImplicitUsings><Nullable>enable</Nullable></PropertyGroup></Project>");
		Directory.CreateDirectory (Path.Combine (docs, "building-apps"));
		File.WriteAllText (Path.Combine (docs, "building-apps", "sample.md"), "Original build documentation.\n");
		File.WriteAllText (Path.Combine (docs, "includes", "diagnostics", "shared.md"), "Shared guidance.\n");
		Validated ().Generate (true, false);
	}

	(int ExitCode, string Output) RunSync (string destination, bool reverse = false)
	{
		var repository = RepositoryRoot ();
		var sdk = Path.Combine (repository, "builds", "downloads", "dotnet", "dotnet");
		var start = new ProcessStartInfo ("bash") {
			WorkingDirectory = root,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
		};
		start.Environment ["DOTNET"] = File.Exists (sdk) ? sdk : "dotnet";
		start.ArgumentList.Add (Path.Combine (docs, "sync-mobile-docs.sh"));
		start.ArgumentList.Add ("--docs-mobile-repo");
		start.ArgumentList.Add (destination);
		if (reverse)
			start.ArgumentList.Add ("--reverse");
		using var process = new Process { StartInfo = start };
		var output = new StringBuilder ();
		DataReceivedEventHandler capture = (_, args) => {
			lock (output) {
				output.AppendLine (args.Data);
			}
		};
		process.OutputDataReceived += capture;
		process.ErrorDataReceived += capture;
		process.Start ();
		process.BeginOutputReadLine ();
		process.BeginErrorReadLine ();
		if (!process.WaitForExit (120_000)) {
			process.Kill (true);
			Assert.Fail ("Sync did not finish.");
		}
		process.WaitForExit ();
		return (process.ExitCode, output.ToString ());
	}

	[Test]
	public void SyncRoundTripAndIndependentIndexEdit ()
	{
		PrepareSyncFixture ();
		var destination = Path.Combine (root, "mobile docs");
		var ios = Path.Combine (destination, "docs", "ios");
		Directory.CreateDirectory (ios);
		File.WriteAllText (Path.Combine (ios, "unrelated.md"), "Do not touch.");
		var result = RunSync (destination);
		Assert.That (result.ExitCode, Is.EqualTo (0), result.Output);
		Assert.That (File.Exists (Path.Combine (ios, "messages", "mt0003.md")), Is.True);
		Assert.That (File.Exists (Path.Combine (ios, "messages", "allocations.json")), Is.False);
		Assert.That (File.ReadAllText (Path.Combine (ios, "unrelated.md")), Is.EqualTo ("Do not touch."));
		result = RunSync (destination);
		Assert.That (result.ExitCode, Is.EqualTo (0), result.Output);
		result = RunSync (destination, true);
		Assert.That (result.ExitCode, Is.EqualTo (0), result.Output);
		var incomingPage = Path.Combine (ios, "messages", "mt0003.md");
		File.AppendAllText (incomingPage, "\nA new explanation.\n");
		result = RunSync (destination, true);
		Assert.That (result.ExitCode, Is.EqualTo (0), result.Output);
		Assert.That (File.ReadAllText (Path.Combine (docs, "messages", "mt0003.md")), Does.Contain ("A new explanation."));
		File.WriteAllText (incomingPage, File.ReadAllText (incomingPage).Replace ("diagnostic_message: \"Shared message\"", "diagnostic_message: \"Changed message\""));
		result = RunSync (destination, true);
		Assert.That (result.ExitCode, Is.Not.EqualTo (0), "Changing index metadata must require regenerating the incoming index.");
		var incoming = Validated (ios);
		incoming.Generate (true, true);
		result = RunSync (destination, true);
		Assert.That (result.ExitCode, Is.EqualTo (0), result.Output);
		Assert.That (File.ReadAllText (Path.Combine (docs, "messages", "index.md")), Does.Contain ("Changed message"));
		File.AppendAllText (Path.Combine (ios, "messages", "index.md"), "Independent edit.\n");
		File.WriteAllText (Path.Combine (ios, "building-apps", "sample.md"), "Must not be copied.");
		result = RunSync (destination, true);
		Assert.That (result.ExitCode, Is.Not.EqualTo (0), result.Output);
		Assert.That (result.Output, Does.Contain ("index-only edits"));
		Assert.That (File.ReadAllText (Path.Combine (docs, "building-apps", "sample.md")), Is.EqualTo ("Original build documentation.\n"));
	}

	[Test]
	public void RejectsNonPublishingDestinationBeforeCopying ()
	{
		PrepareSyncFixture ();
		var result = RunSync (root);
		Assert.That (result.ExitCode, Is.Not.EqualTo (0), result.Output);
		Assert.That (result.Output, Does.Contain ("Expected a docs-mobile checkout"));
	}

	[Test]
	public void ForwardSyncDoesNotCopyWhenValidationFails ()
	{
		PrepareSyncFixture ();
		File.AppendAllText (Path.Combine (docs, "messages", "index.md"), "Independent edit.\n");
		var destination = Path.Combine (root, "mobile docs");
		var ios = Path.Combine (destination, "docs", "ios");
		Directory.CreateDirectory (ios);
		var result = RunSync (destination);
		Assert.That (result.ExitCode, Is.Not.EqualTo (0), result.Output);
		Assert.That (Directory.Exists (Path.Combine (ios, "messages")), Is.False);
		Assert.That (Directory.Exists (Path.Combine (ios, "building-apps")), Is.False);
	}
}
