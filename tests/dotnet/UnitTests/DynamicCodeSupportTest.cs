// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class DynamicCodeSupportTest : TestBaseClass {
		const string featureSwitchName = "System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported";

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void SupportedWithCoreCLR (ApplePlatform platform, string runtimeIdentifiers)
		{
			// CoreCLR supports dynamic code (JIT and/or Reflection.Emit), so we must not claim otherwise:
			// libraries such as Entity Framework Core use this feature switch to detect NativeAOT, and would
			// otherwise refuse to work at all. See https://github.com/dotnet/macios/issues/26430.
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, featureSwitchName);
			// If the feature switch isn't set at all, the default from dotnet/sdk (which is 'true') applies.
			var value = featureSwitch?.GetMetadata ("Value") ?? "true";
			Assert.That (value, Is.EqualTo ("true"), "Dynamic code must be supported when using CoreCLR.");
		}

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false")]
		public void UserSpecifiedValue (ApplePlatform platform, string runtimeIdentifiers, string dynamicCodeSupport)
		{
			// When the user sets $(DynamicCodeSupport), the value must be passed straight through to the
			// 'System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported' trimmer feature switch.
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["DynamicCodeSupport"] = dynamicCodeSupport;

			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, featureSwitchName);
			Assert.That (featureSwitch, Is.Not.Null, "The IsDynamicCodeSupported feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (dynamicCodeSupport), "The feature switch value must match the user-specified value.");
		}
	}
}
