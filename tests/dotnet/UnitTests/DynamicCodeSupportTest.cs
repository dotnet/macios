// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class DynamicCodeSupportTest : TestBaseClass {
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

			Assert.That (GetDynamicCodeSupport (rv.BinLogPath), Is.EqualTo ("true"), "Dynamic code must be supported when using CoreCLR.");
		}

		// Note: iOS/tvOS aren't covered here, because publishing for those platforms requires a device
		// runtime identifier (and thus code signing), which isn't always available.
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void UnsupportedWithNativeAot (ApplePlatform platform, string runtimeIdentifiers)
		{
			// NativeAOT doesn't support dynamic code at all.
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["PublishAot"] = "true";

			var rv = DotNet.AssertPublish (project_path, properties);

			Assert.That (GetDynamicCodeSupport (rv.BinLogPath), Is.EqualTo ("false"), "Dynamic code must not be supported when using NativeAOT.");
		}

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false")]
		public void UserSpecifiedValue (ApplePlatform platform, string runtimeIdentifiers, string dynamicCodeSupport)
		{
			// When the user sets $(DynamicCodeSupport), the value must be used as-is.
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["DynamicCodeSupport"] = dynamicCodeSupport;

			var rv = DotNet.AssertBuild (project_path, properties);

			Assert.That (GetDynamicCodeSupport (rv.BinLogPath), Is.EqualTo (dynamicCodeSupport), "The user-specified value must be used.");
		}

		// Returns the effective value of the $(DynamicCodeSupport) property, which becomes the
		// 'System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported' trimmer feature switch.
		static string GetDynamicCodeSupport (string binLogPath)
		{
			// If the property isn't set at all, dotnet/sdk's default (which is 'true') applies.
			if (!BinLog.TryFindPropertyValue (binLogPath, "DynamicCodeSupport", out var value) || string.IsNullOrEmpty (value))
				return "true";
			return value;
		}
	}
}
