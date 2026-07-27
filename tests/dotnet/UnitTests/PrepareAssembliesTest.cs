// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class PrepareAssembliesTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "managed-static", "SdkOnly", "Release")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "dynamic", "None", "Debug")]
		public void IncrementalBuild (ApplePlatform platform, string runtimeIdentifiers, string registrar, string linkMode, string configuration)
		{
			// An incremental (second, no-source-change) build with PrepareAssemblies=true must not fail.
			// The '_PrepareAssemblies' and '_PostprocessAssemblies' targets must not run as partial
			// incremental builds, because the assembly-preparer needs the complete set of assemblies to
			// resolve inter-assembly references (otherwise it fails with MT4116/MT2362). See
			// https://github.com/dotnet/macios/issues/25938.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["PrepareAssemblies"] = "true";
			properties ["MtouchLink"] = linkMode;
			properties ["Registrar"] = registrar;

			// The first (clean) build must succeed.
			DotNet.AssertBuild (project_path, properties);

			// The second (incremental) build, without any changes, must also succeed.
			DotNet.AssertBuild (project_path, properties);
		}

		[TestCase (true, true, "trimmable-static", null, null, true)]
		[TestCase (false, true, "trimmable-static", null, null, false)]
		[TestCase (true, false, "trimmable-static", null, null, false)]
		[TestCase (true, true, "managed-static", null, null, false)]
		[TestCase (true, true, "trimmable-static", "false", null, false)]
		[TestCase (true, true, "trimmable-static", null, "true", false)]
		public void ExportAttributeRemovalEligibility (bool prepareAssemblies, bool postProcessAssemblies, string registrar, string? trimExportAttributes, string? dynamicRegistrationSupported, bool expectedRemoval)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var project = "MySimpleApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["AdditionalDefineConstants"] = "EXPORT_ATTRIBUTE_REMOVAL";
			properties ["Configuration"] = configuration;
			properties ["EnableAssemblyILStripping"] = "true";
			properties ["MtouchLink"] = registrar == "managed-static" ? "SdkOnly" : "Full";
			properties ["PostProcessAssemblies"] = postProcessAssemblies.ToString ();
			properties ["PrepareAssemblies"] = prepareAssemblies.ToString ();
			properties ["Registrar"] = registrar;
			if (trimExportAttributes is not null)
				properties ["TrimExportAttributes"] = trimExportAttributes;
			if (dynamicRegistrationSupported is not null)
				properties ["DynamicRegistrationSupported"] = dynamicRegistrationSupported;

			string platformAssemblyPath;
			string appAssemblyPath;
			string? target = null;
			if (!prepareAssemblies) {
				DotNet.AssertBuild (projectPath, properties, target: "Compile");
				platformAssemblyPath = Configuration.GetBaseLibraryImplementations (platform).First ();
				appAssemblyPath = Path.Combine (GetObjDir (projectPath, platform, runtimeIdentifiers, configuration), project + ".dll");
			} else {
				target = "Compile;_ComputePublishTrimmed;_ComputeLinkMode;_ComputeLinkerArguments;_PrepareAssemblies;_SetDynamicRegistrationSupportedFeature;_SetTrimExportAttributesFeature;_ComputeFrameworkFilesToPublish;_ComputeDynamicLibrariesToPublish;ComputeFilesToPublish;_ComputeStripAssemblyIL;_StripAssemblyIL";
				var assemblyDirectory = Path.Combine (GetObjDir (projectPath, platform, runtimeIdentifiers, configuration), "stripped");
				DotNet.AssertBuild (projectPath, properties, target: target);
				platformAssemblyPath = Path.Combine (assemblyDirectory, Configuration.GetBaseLibraryName (platform));
				appAssemblyPath = Path.Combine (assemblyDirectory, project + ".dll");
			}

			AssertExportMetadata (platformAssemblyPath, appAssemblyPath, !expectedRemoval);

			if (expectedRemoval) {
				properties ["DynamicRegistrationSupported"] = "true";
				DotNet.AssertBuild (projectPath, properties, target: target);
				AssertExportMetadata (platformAssemblyPath, appAssemblyPath, true);
			}
		}

		static void AssertExportMetadata (string platformAssemblyPath, string appAssemblyPath, bool expected)
		{
			using var platformAssembly = AssemblyDefinition.ReadAssembly (platformAssemblyPath);
			var bundlePath = platformAssembly.MainModule.GetType ("Foundation.NSBundle").Properties.Single (v => v.Name == "BundlePath");
			AssertExport (bundlePath.GetMethod, expected, "direct-binding NSObject wrapper");

			var nsObjectDescription = platformAssembly.MainModule.GetType ("Foundation.NSObject").Properties.Single (v => v.Name == "Description");
			AssertExport (nsObjectDescription.GetMethod, expected, "wrapper ancestor with application subclasses");

			using var appAssembly = AssemblyDefinition.ReadAssembly (appAssemblyPath);
			var applicationType = appAssembly.MainModule.GetType ("MySimpleApp.ExportMetadataApplicationType");
			AssertAttribute (applicationType.Methods.Single (v => v.Name == "ApplicationExport"), "ExportAttribute", expected, "application export");
			AssertAttribute (applicationType.Methods.Single (v => v.Name == "ApplicationAction"), "ActionAttribute", expected, "application action");
			AssertAttribute (applicationType.Properties.Single (v => v.Name == "ApplicationOutlet"), "OutletAttribute", expected, "application outlet");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ExportAttributeRemovalWithNSXpcInterfaceUsage (bool explicitlyEnabled)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var project = "MySimpleApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["AdditionalDefineConstants"] = "EXPORT_ATTRIBUTE_REMOVAL;EXPORT_ATTRIBUTE_REMOVAL_NSXPC";
			properties ["Configuration"] = configuration;
			properties ["MtouchLink"] = "Full";
			properties ["PostProcessAssemblies"] = "true";
			properties ["PrepareAssemblies"] = "true";
			properties ["Registrar"] = "trimmable-static";
			properties ["DynamicRegistrationSupported"] = "false";
			if (explicitlyEnabled)
				properties ["TrimExportAttributes"] = "true";

			var target = "Compile;_ComputePublishTrimmed;_ComputeLinkMode;_ComputeLinkerArguments;_PrepareAssemblies";
			var expectedMessage = "Export attributes cannot be removed because the application uses an NSXpcInterface overload that obtains a selector from MethodInfo.";
			if (explicitlyEnabled) {
				for (var i = 0; i < 2; i++) {
					var result = DotNet.AssertBuildFailure (projectPath, properties, target: target);
					var errors = BinLog.GetBuildLogErrors (result.BinLogPath).ToArray ();
					Assert.That (errors.Select (v => v.Message), Has.Some.Contains (expectedMessage), $"Error #{i + 1}");
				}
			} else {
				var result = DotNet.AssertBuild (projectPath, properties, target: target);
				var warnings = BinLog.GetBuildLogWarnings (result.BinLogPath).ToArray ();
				Assert.That (warnings.Select (v => v.Message), Has.Some.Contains (expectedMessage), "Warning");
			}
		}

		static void AssertExport (ICustomAttributeProvider provider, bool expected, string message)
		{
			AssertAttribute (provider, "ExportAttribute", expected, message);
		}

		static void AssertAttribute (ICustomAttributeProvider provider, string name, bool expected, string message)
		{
			var actual = provider.CustomAttributes.Any (v => v.AttributeType.Namespace == "Foundation" && v.AttributeType.Name == name);
			Assert.That (actual, Is.EqualTo (expected), message);
		}
	}
}
