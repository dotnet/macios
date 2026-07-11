// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class DynamicRegistrationSupportedTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false")]
		public void UserSpecifiedValue (ApplePlatform platform, string runtimeIdentifiers, string dynamicRegistrationSupported)
		{
			// When the user sets $(DynamicRegistrationSupported), the value must be passed straight through to
			// the 'ObjCRuntime.Runtime.DynamicRegistrationSupported' trimmer feature switch (the assembly-preparer
			// doesn't need to compute it, so RegistrarRemovalTrackingStep is skipped).
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			// DynamicRegistrationSupported maps to the 'remove-dynamic-registrar' optimization, which is only
			// valid with a static registrar, so use one (managed-static) and enable trimming.
			properties ["MtouchLink"] = "SdkOnly";
			properties ["Registrar"] = "managed-static";
			properties ["PrepareAssemblies"] = "true";
			properties ["PostProcessAssemblies"] = "true";
			properties ["DynamicRegistrationSupported"] = dynamicRegistrationSupported;

			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, "ObjCRuntime.Runtime.DynamicRegistrationSupported");
			Assert.That (featureSwitch, Is.Not.Null, "The DynamicRegistrationSupported feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (dynamicRegistrationSupported), "The feature switch value must match the user-specified value.");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void SkippedWhenPlatformAssemblyNotTrimmed (ApplePlatform platform, string runtimeIdentifiers)
		{
			// When the platform assembly isn't being trimmed (link mode None), the dynamic registrar can't be
			// removed, so RegistrarRemovalTrackingStep is skipped and no DynamicRegistrationSupported feature
			// switch is emitted (the dynamic registrar is kept, which is the default).
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "None";
			properties ["PrepareAssemblies"] = "true";
			properties ["PostProcessAssemblies"] = "true";

			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, "ObjCRuntime.Runtime.DynamicRegistrationSupported");
			Assert.That (featureSwitch, Is.Null, "No DynamicRegistrationSupported feature switch should be set when the platform assembly isn't trimmed.");
		}

		// Returns the last RuntimeHostConfigurationOption item with the given name (ItemSpec) added during the build.
		static ITaskItem? GetRuntimeHostConfigurationOption (string binLogPath, string name)
		{
			ITaskItem? rv = null;
			foreach (var args in BinLog.ReadBuildEvents (binLogPath)) {
				if (args is not TaskParameterEventArgs tpea)
					continue;
				if (tpea.Kind != TaskParameterMessageKind.AddItem)
					continue;
				if (tpea.ItemType != "RuntimeHostConfigurationOption")
					continue;
				foreach (var item in tpea.Items) {
					if (item is ITaskItem taskItem && taskItem.ItemSpec == name)
						rv = taskItem;
				}
			}
			return rv;
		}
	}
}
