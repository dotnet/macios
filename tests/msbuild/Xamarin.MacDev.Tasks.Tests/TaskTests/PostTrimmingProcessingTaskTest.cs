// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.MacDev.Tasks;
using Xamarin.Tests;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class PostTrimmingProcessingTaskTest : TestBase {

		(string GeneratedCode, ITaskItem [] AllowUndefinedSymbols) RunTask (string [] survivingClasses, string [] typeMapLines)
		{
			var tempDir = Cache.CreateTemporaryDirectory ();

			var survivingClassesFile = Path.Combine (tempDir, "surviving-classes.txt");
			File.WriteAllLines (survivingClassesFile, survivingClasses);

			var typeMapFile = Path.Combine (tempDir, "type-map.txt");
			File.WriteAllLines (typeMapFile, typeMapLines);

			var outputDir = Path.Combine (tempDir, "output");

			var task = CreateTask<PostTrimmingProcessing> ();
			task.Architecture = "arm64";
			task.OutputDirectory = outputDir;
			task.SurvivingClassesFiles = new ITaskItem [] { new TaskItem (survivingClassesFile) };
			task.TypeMapFilePath = typeMapFile;

			ExecuteTask (task);

			var generatedCode = File.ReadAllText (Path.Combine (outputDir, "inlined-class-gethandle.m"));
			return (generatedCode, task.AllowUndefinedSymbols ?? []);
		}

		[Test]
		public void ThirdPartyClassIsRisky ()
		{
			// A binding class that doesn't belong to any known platform framework (empty Framework)
			// can't be proven to exist natively, so it should get the objc_getClass fallback and be
			// added to the list of symbols allowed to be undefined at link time.
			var (code, allowUndefined) = RunTask (
				new [] { "ThirdPartyClass" },
				new [] { "Class=ThirdPartyClass|Framework=|Introduced=|IsWrapper=true|IsStubClass=false" });

			Assert.That (code, Does.Contain ("objc_getClass (\"ThirdPartyClass\")"), "fallback");
			Assert.That (allowUndefined.Select (v => v.ItemSpec), Does.Contain ("_OBJC_CLASS_$_ThirdPartyClass"), "allow undefined");
		}

		[Test]
		public void UnknownClassIsRisky ()
		{
			// A class that isn't in the type map at all can't be proven to exist either.
			var (code, allowUndefined) = RunTask (
				new [] { "UnknownClass" },
				new [] { "Class=SomethingElse|Framework=UIKit|Introduced=|IsWrapper=true|IsStubClass=false" });

			Assert.That (code, Does.Contain ("objc_getClass (\"UnknownClass\")"), "fallback");
			Assert.That (allowUndefined.Select (v => v.ItemSpec), Does.Contain ("_OBJC_CLASS_$_UnknownClass"), "allow undefined");
		}

		[Test]
		public void PlatformClassIsNotRisky ()
		{
			// A class that belongs to a known platform framework is expected to exist in the SDK we link
			// against, so it should keep the direct native reference (no fallback) and must not be added
			// to the list of symbols allowed to be undefined (so a genuinely missing SDK class still errors).
			var (code, allowUndefined) = RunTask (
				new [] { "UIView" },
				new [] { "Class=UIView|Framework=UIKit|Introduced=|IsWrapper=true|IsStubClass=false" });

			Assert.That (code, Does.Contain ("return [UIView class];"), "direct reference");
			Assert.That (code, Does.Not.Contain ("objc_getClass (\"UIView\")"), "no fallback");
			Assert.That (allowUndefined.Select (v => v.ItemSpec), Does.Not.Contain ("_OBJC_CLASS_$_UIView"), "no allow undefined");
		}
	}
}
