using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Xamarin.Utils;
using Xamarin.Tests;

namespace Xamarin.MMP.Tests {
	public class BindingProjectNoEmbeddingTests {
		static void Touch (string projectPath) => File.SetLastWriteTimeUtc (projectPath, DateTime.UtcNow);

		static void AssertNoResourceWithName (string tmpDir, string projectName, string resourceName)
		{
			string bindingName = BindingProjectTests.RemoveCSProj (projectName);
			string bindingLibraryPath = Path.Combine (tmpDir, $"bin/Debug/{bindingName}.dll");
			string resourceOutput = TI.RunAndAssert ("/Library/Frameworks/Mono.framework/Commands/monodis", new [] { "--presources", bindingLibraryPath }, "monodis");
			Assert.That (resourceOutput.Contains (resourceName), Is.False, "Binding project output contained embedded library");
		}

		static void AssertFileInBundle (string tmpDir, BindingProjectType type, string path, bool assertIsSymLink = false)
		{
			string bundlePath = Path.Combine (tmpDir, $"bin/Debug/{(type == BindingProjectType.Modern ? "UnifiedExample" : "XM45Example")}.app/Contents/{path}");
			Assert.That (File.Exists (bundlePath), Is.True, $"{path} not in bundle as expected.");
			if (assertIsSymLink)
				Assert.That (File.GetAttributes (bundlePath).HasFlag (FileAttributes.ReparsePoint), Is.True);
		}

		[TestCase (BindingProjectType.Modern, false)]
		[TestCase (BindingProjectType.Full, true)]
		public void LibrariesEmbeddedProperly (BindingProjectType type, bool useProjectReference)
		{
			MMPTests.RunMMPTest (tmpDir => {
				var projects = BindingProjectTests.GenerateTestProject (type, tmpDir);
				BindingProjectTests.SetNoEmbedding (projects.Item1);

				BindingProjectTests.SetupAndBuildLinkedTestProjects (projects.Item1, projects.Item2, tmpDir, useProjectReference, setupDefaultNativeReference: true);

				AssertNoResourceWithName (tmpDir, projects.Item1.ProjectName, "SimpleClassDylib.dylib");
				AssertFileInBundle (tmpDir, type, "MonoBundle/SimpleClassDylib.dylib");
			});
		}

		[TestCase (BindingProjectType.Modern, true)]
		[TestCase (BindingProjectType.Full, false)]
		public void FrameworksEmbeddedProperly (BindingProjectType type, bool useProjectReference)
		{
			Configuration.AssertDotNetAvailable (); // not really a .NET test, but the logic that builds our native test frameworks is (unintentionally, but untrivially unravelable) .NET-only
			MMPTests.RunMMPTest (tmpDir => {
				string frameworkPath = FrameworkBuilder.CreateThinFramework (tmpDir);

				var projects = BindingProjectTests.GenerateTestProject (type, tmpDir);
				BindingProjectTests.SetNoEmbedding (projects.Item1);
				projects.Item1.ItemGroup = NativeReferenceTests.CreateSingleNativeRef (frameworkPath, "Framework");

				BindingProjectTests.SetupAndBuildLinkedTestProjects (projects.Item1, projects.Item2, tmpDir, useProjectReference, false);

				var frameworkName = Path.GetFileNameWithoutExtension (frameworkPath);
				AssertNoResourceWithName (tmpDir, projects.Item1.ProjectName, frameworkName);
				AssertFileInBundle (tmpDir, type, $"Frameworks/{frameworkName}.framework/{frameworkName}", assertIsSymLink: true);
			});
		}

		[TestCase (BindingProjectType.Modern)]
		public void DoesNotSupportLinkWith (BindingProjectType type)
		{
			MMPTests.RunMMPTest (tmpDir => {
				var projects = BindingProjectTests.GenerateTestProject (type, tmpDir);
				BindingProjectTests.SetNoEmbedding (projects.Item1);

				projects.Item1.LinkWithName = "SimpleClassDylib.dylib";

				BindingProjectTests.SetupAndBuildBindingProject (projects.Item1, false);
			});
		}

		[TestCase (true)]
		[TestCase (false)]
		public void MultipleNativeReferences (bool useProjectReference)
		{
			MMPTests.RunMMPTest (tmpDir => {
				// This is a bit of a hack, you can't just rename dylibs like this
				string secondNativeLibPath = Path.Combine (tmpDir, "SimpleClassDylib2.dylib");
				File.Copy (NativeReferenceTests.SimpleDylibPath, secondNativeLibPath);

				var projects = BindingProjectTests.GenerateTestProject (BindingProjectType.Modern, tmpDir);
				BindingProjectTests.SetNoEmbedding (projects.Item1);
				projects.Item1.ItemGroup += NativeReferenceTests.CreateSingleNativeRef (secondNativeLibPath, "Dynamic");

				BindingProjectTests.SetupAndBuildLinkedTestProjects (projects.Item1, projects.Item2, tmpDir, useProjectReference, setupDefaultNativeReference: true);

				// manifest and 2 dylibs
				Assert.That (Directory.GetFiles (Path.Combine (tmpDir, "bin/Debug/MobileBinding.resources")).Length, Is.EqualTo (3));

				// 2 dylibs + libMonoPosixHelper.dylib + libmono-native.dylib
				Assert.That (Directory.GetFiles (Path.Combine (tmpDir, "bin/Debug/UnifiedExample.app/Contents/MonoBundle")).Where (x => x.EndsWith (".dylib")).Count (), Is.EqualTo (4));
			});
		}

		[TestCase (true)]
		[TestCase (false)]
		public void MultipleDependencyChain (bool useProjectReference)
		{
			// App can depend on Lib that depends on binding lib with native reference and everything gets packaged in correctly
			MMPTests.RunMMPTest (tmpDir => {
				TI.UnifiedTestConfig binding = new TI.UnifiedTestConfig (tmpDir) { ProjectName = "MobileBinding.csproj" };
				binding.ItemGroup += NativeReferenceTests.CreateSingleNativeRef (NativeReferenceTests.SimpleDylibPath, "Dynamic");
				binding.APIDefinitionConfig += @"[BaseType (typeof (NSObject))]
  interface SimpleClass {
    [Export (""doIt"")]
    int DoIt ();
  }";
				BindingProjectTests.SetNoEmbedding (binding);

				TI.GenerateBindingLibraryProject (binding);
				TI.BuildProject (Path.Combine (tmpDir, "MobileBinding.csproj"));


				TI.UnifiedTestConfig library = new TI.UnifiedTestConfig (tmpDir) { ProjectName = "UnifiedLibrary" };
				library.TestCode = "public class MyClass { public static void Go () { var c = new ExampleBinding.SimpleClass (); c.DoIt (); } }";

				if (useProjectReference)
					library.References = $@"<ProjectReference Include=""MobileBinding.csproj"" />";
				else
					library.References = $@"<Reference Include=""MobileBinding""><HintPath>{Path.Combine (tmpDir, "bin/Debug", "MobileBinding.dll")}</HintPath></Reference>";

				TI.GenerateUnifiedLibraryProject (library);
				TI.BuildProject (Path.Combine (tmpDir, "UnifiedLibrary.csproj"));

				TI.UnifiedTestConfig project = new TI.UnifiedTestConfig (tmpDir) { ProjectName = "UnifiedExample.csproj" };
				project.TestCode = "MyClass.Go ();";

				if (useProjectReference)
					project.References = $@"<ProjectReference Include=""UnifiedLibrary.csproj"" />";
				else
					project.References = $@"<Reference Include=""UnifiedLibrary""><HintPath>{Path.Combine (tmpDir, "bin/Debug", "UnifiedLibrary.dll")}</HintPath></Reference>";

				TI.GenerateUnifiedExecutableProject (project);
				TI.BuildProject (Path.Combine (tmpDir, "UnifiedExample.csproj"));
			});
		}

		[Test]
		public void CleanShouldRemoveBundle ()
		{
			MMPTests.RunMMPTest (tmpDir => {
				var projects = BindingProjectTests.GenerateTestProject (BindingProjectType.Modern, tmpDir);
				BindingProjectTests.SetNoEmbedding (projects.Item1);

				BindingProjectTests.SetupAndBuildBindingProject (projects.Item1, true);

				TI.CleanUnifiedProject (Path.Combine (tmpDir, projects.Item1.ProjectName));
				Assert.That (Directory.Exists (Path.Combine (tmpDir, "bin/Debug/MobileBinding.resources")), Is.False, "Resource bundle was not cleaned up");
			});
		}
	}
}
