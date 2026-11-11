// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	[NonParallelizable]
	[Category ("RemoteBuildHang")]
	public class RemoteBuildFailureTest : TestBaseClass {
		[TestCase ("AfterConnect", "Build", false, "Error")]
		[TestCase ("AfterConnect", "Build", false, "RemoteExec")]
		[TestCase ("AfterConnect", "Build", false, "ErrorWithGoodbye")]
		public void FailureAfterRemoteConnection (string failureTarget, string target, bool parallel, string failureKind)
		{
			Configuration.IgnoreIfNotOnWindows ();
			Assert.That (Configuration.IsBuildingRemotely, Is.True, "This diagnostic requires a remote Mac");

			var project = GenerateProject (ApplePlatform.iOS, $"RemoteFailure{failureTarget.Replace ("_", "")}{target}{parallel}{failureKind}", "iossimulator-arm64", out _);
			var directory = Path.GetDirectoryName (project) ?? throw new InvalidOperationException ("The generated project has no directory.");
			const string failure = "REMOTE_BUILD_HANG_DIAGNOSTIC";
			File.WriteAllText (Path.Combine (directory, "Program.cs"), failureKind == "Compiler"
				? $"#error {failure}\n"
				: "System.Console.WriteLine (\"Remote build diagnostic\");\n");
			var document = XDocument.Load (project);
			var root = document.Root ?? throw new InvalidOperationException ("The generated project has no root element.");
			var injectedTarget = new XElement ("Target",
				new XAttribute ("Name", "InjectRemoteBuildFailure"),
				new XAttribute (failureTarget == "CoreCompile" ? "BeforeTargets" : "AfterTargets", failureTarget),
				new XElement ("Message",
					new XAttribute ("Importance", "high"),
					new XAttribute ("Text", $"{failure}: target={failureTarget}, session=$(BuildSessionId), mac=$(IsMacEnabled), connected=$(_DidEstablishConnection)")));
			switch (failureKind) {
			case "Error":
			case "ErrorWithGoodbye":
				injectedTarget.Add (new XElement ("Error", new XAttribute ("Text", failure)));
				if (failureKind == "ErrorWithGoodbye")
					injectedTarget.Add (new XElement ("OnError", new XAttribute ("ExecuteTargets", "_SayGoodbye")));
				break;
			case "LocalExec":
			case "RemoteExec":
				var exec = new XElement ("Exec", new XAttribute ("Command", "exit 17"));
				if (failureKind == "RemoteExec")
					exec.Add (new XAttribute ("SessionId", "$(BuildSessionId)"));
				injectedTarget.Add (exec);
				break;
			}
			root.Add (injectedTarget);
			document.Save (project);

			var properties = GetDefaultProperties ("iossimulator-arm64", includeRemoteProperties: true);
			properties ["EnableCodeSigning"] = "false";
			properties ["ContinueOnDisconnected"] = "false";
			var result = DotNet.Execute ("build", project, properties, assert_success: false, target: target, msbuildParallelism: parallel, timeout: TimeSpan.FromMinutes (2));
			var errors = BinLog.GetBuildLogErrors (result.BinLogPath).ToArray ();
			Assert.That (result.ExitCode, Is.Not.EqualTo (0), "The injected failure must fail the build");
			Assert.That (errors, Is.Not.Empty, "The build must report an error");
			var messages = BinLog.GetBuildMessages (result.BinLogPath).Select (v => v.Message).ToArray ();
			Assert.That (messages, Has.Some.Contains ($"{failure}: target={failureTarget}"), "The requested failure target must run");
			Assert.That (messages, Has.Some.Contains ("mac=true").IgnoreCase, "The failure must happen with an active remote connection");
			Assert.That (messages, Has.Some.Contains ("connected=true").IgnoreCase, "The failure must follow a successful remote connection");
		}
	}
}
