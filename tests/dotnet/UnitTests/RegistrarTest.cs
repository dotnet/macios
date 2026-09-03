using Mono.Cecil;

namespace Xamarin.Tests {
	[TestFixture]
	public class RegistrarTest : TestBaseClass {
		[TestCase ("None", "partial-static")]
		[TestCase ("SdkOnly", "trimmable-static")]
		public void DefaultCoreCLRSimulatorRegistrar (string linkMode, string expectedRegistrar)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = linkMode;
			properties ["UseMonoRuntime"] = "false";

			var result = DotNet.AssertBuild (projectPath, properties);

			Assert.That (BinLog.TryFindPropertyValue (result.BinLogPath, "Registrar", out var registrar), Is.True, "Could not find the 'Registrar' property in the binlog.");
			Assert.That (registrar, Is.EqualTo (expectedRegistrar), "Registrar");
			Assert.That (BinLog.TryFindPropertyValue (result.BinLogPath, "PrepareAssemblies", out var prepareAssemblies), Is.True, "Could not find the 'PrepareAssemblies' property in the binlog.");
			Assert.That (prepareAssemblies, Is.EqualTo ("true"), "PrepareAssemblies");
		}

		[Test]
		public void ChangeDefaultCoreCLRSimulatorRegistrar ()
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "None";
			properties ["UseMonoRuntime"] = "false";
			properties ["Registrar"] = "trimmable-static";
			DotNet.AssertBuild (projectPath, properties);

			properties.Remove ("Registrar");
			var result = DotNet.AssertBuild (projectPath, properties);

			Assert.That (BinLog.TryFindPropertyValue (result.BinLogPath, "Registrar", out var registrar), Is.True, "Could not find the 'Registrar' property in the binlog.");
			Assert.That (registrar, Is.EqualTo ("partial-static"), "Registrar");

			var targets = BinLog.GetAllTargets (result.BinLogPath);
			Assert.That (targets.Any (v => v.TargetName == "_PrepareAssemblies" && !v.Skipped), Is.True, "_PrepareAssemblies should execute when the registrar changes.");

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifiers);
			var registrarItemsPath = Path.Combine (objDir, "linker-items", "_RegistrarFile.items");
			Assert.That (registrarItemsPath, Does.Exist, "_RegistrarFile.items");
			Assert.That (File.ReadAllText (registrarItemsPath), Does.Not.Contain ("<_RegistrarFile Include="), "_RegistrarFile.items");
		}

		// This test does evil things that the AOT runtime complains about, so it only works when not running the AOT compiler (aka x64 when using Mono).
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64", true)]
		[TestCase (ApplePlatform.MacOSX, null, true)]
		[TestCase (ApplePlatform.iOS, "iossimulator-x64", false)]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-x64", false)]
		public void InvalidStaticRegistrarValidation (ApplePlatform platform, string? runtimeIdentifiers, bool validated)
		{
			var project = "MyRegistrarApp";
			var configuration = "Debug";

			runtimeIdentifiers ??= GetDefaultRuntimeIdentifier (platform);

			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Registrar"] = "static";
			// enable the linker (so that the main assembly is modified)
			properties ["LinkMode"] = "full";
			properties ["MtouchLink"] = "full";

			DotNet.AssertBuild (projectPath, properties);

			var appDir = GetAppPath (projectPath, platform, runtimeIdentifiers, configuration);
			var asmDir = Path.Combine (appDir, GetRelativeAssemblyDirectory (platform));
			var appExecutable = Path.Combine (asmDir, project + ".dll");

			// Save the first version of the main assembly in memory
			var firstAssembly = File.ReadAllBytes (appExecutable);

			// Build again, including additional code
			properties ["AdditionalDefineConstants"] = "INCLUDED_ADDITIONAL_CODE";
			DotNet.AssertBuild (projectPath, properties);

			// Revert to the original version of the main assembly
			File.WriteAllBytes (appExecutable, firstAssembly);

			Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", "1");
			try {
				if (validated) {
					ExecuteProjectWithMagicWordAndAssert (projectPath, platform, runtimeIdentifiers);
				} else if (CanExecute (platform, runtimeIdentifiers)) {
					var rv = base.Execute (GetNativeExecutable (platform, appDir), out var output, out _);
					Assert.That (rv.ExitCode, Is.EqualTo (1), "Expected no validation");
				}
			} finally {
				Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", null);
			}
		}

		[TestCase (ApplePlatform.MacCatalyst, false)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.MacCatalyst, true)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.TVOS, true)]
		public void ClassRewriterTest (ApplePlatform platform, bool rewriteHandles)
		{
			var project = "MyClassRedirectApp";
			var configuration = "Debug";
			var runtimeIdentifiers = GetDefaultRuntimeIdentifier (platform);
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath (project, platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties ();
			properties ["Registrar"] = "static";
			// enable the linker (so that the main assembly is modified)
			properties ["LinkMode"] = "full";
			properties ["MtouchLink"] = "full";
			properties ["InlineClassGetHandle"] = "disabled";
			if (rewriteHandles)
				properties ["MtouchExtraArgs"] = "--optimize=redirect-class-handles";

			DotNet.AssertBuild (projectPath, properties);

			var appDir = GetAppPath (projectPath, platform, runtimeIdentifiers, configuration);
			var asmDir = Path.Combine (appDir, GetRelativeAssemblyDirectory (platform));

			var appExecutable = Path.Combine (asmDir, project + ".dll");
			var platformDll = Path.Combine (asmDir, Configuration.GetBaseLibraryName (platform));
			Assert.That (File.Exists (platformDll), "No platform dll.");
			var module = ModuleDefinition.ReadModule (platformDll);
			var classHandlesMaybe = AllTypes (module).FirstOrDefault (t => t.FullName == "ObjCRuntime.Runtime/ClassHandles");
			Assert.That (classHandlesMaybe, Is.Not.Null, "Couldn't find ClassHandles type.");
			var classHandles = classHandlesMaybe!;
			if (!rewriteHandles) {
				// NB: there is always at least one field named "unused"
				var fields = classHandles.Fields.Where (f => f.Name != "unused").Select (f => f.Name).ToList ();
				var sb = new StringBuilder ();
				foreach (var f in fields) {
					sb.Append (" ").Append (f);
				}
				Assert.That (fields.Count == 0, "There are fields in classHandles - rewriter was called when it should have done nothing." + sb);
			} else {
				// NB: there is always at least one field named "unused"
				Assert.That (classHandles.HasFields && classHandles.Fields.Count () > 1, "There are no fields in ClassHandles - rewriter did nothing.");
				var field = classHandles.Fields.FirstOrDefault (f => f.Name.Contains ("SomeObj"));
				Assert.That (field, Is.Not.Null, "Didn't find a field for 'SomeObj'");
			}
		}

		IEnumerable<TypeDefinition> AllTypes (ModuleDefinition module)
		{
			foreach (var type in module.Types) {
				yield return type;
				foreach (var t in InnerTypes (type))
					yield return t;
			}
		}

		IEnumerable<TypeDefinition> InnerTypes (TypeDefinition type)
		{
			if (type.HasNestedTypes) {
				foreach (var t in type.NestedTypes) {
					yield return t;
					foreach (var nt in InnerTypes (t))
						yield return nt;
				}
			}
		}
	}
}
