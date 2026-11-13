// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;

using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class FinalizerReleaseTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", null, true)]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false", true)]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true", true)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", null, true)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "false", true)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "true", true)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "false", false)]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "true", false)]
		public void FeatureSwitch (ApplePlatform platform, string runtimeIdentifier, string? forceMainThread, bool trimmed)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GetProjectPath ("EnsureUIThreadApp", runtimeIdentifiers: runtimeIdentifier, platform: platform, out var appPath);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["MtouchLink"] = trimmed ? "SdkOnly" : "None";
			properties ["LinkMode"] = trimmed ? "SdkOnly" : "None";
			if (forceMainThread is not null)
				properties ["ForceMainThreadFinalization"] = forceMainThread;

			DotNet.AssertBuild (projectPath, properties);

			var runtimeConfig = Directory.GetFiles (appPath, "*.runtimeconfig.json", SearchOption.AllDirectories).Single ();
			using var configuration = JsonDocument.Parse (File.ReadAllText (runtimeConfig));
			var configProperties = configuration.RootElement.GetProperty ("runtimeOptions").GetProperty ("configProperties");
			var hasSwitch = configProperties.TryGetProperty ("Foundation.NSObject.ForceMainThreadFinalization", out var switchValue);
			Assert.That (hasSwitch, Is.EqualTo (!trimmed), "Only untrimmed builds need runtime configuration for the switch.");
			if (!trimmed) {
				Assert.That (switchValue.GetBoolean (), Is.EqualTo (forceMainThread == "true"), "Runtime feature switch");
				return;
			}

			var platformAssembly = Path.Combine (appPath, GetRelativeAssemblyDirectory (platform), Configuration.GetBaseLibraryName (platform));
			using var assembly = AssemblyDefinition.ReadAssembly (platformAssembly);
			var nsObject = assembly.MainModule.Types.Single (type => type.FullName == "Foundation.NSObject");
			var enabled = forceMainThread != "true";
			Assert.That (nsObject.Methods.Any (method => method.Name == "InitializeMainThreadRelease"), Is.EqualTo (enabled), "Release-policy initialization must be trimmed away when opting out.");
			Assert.That (nsObject.Methods.Any (method => method.Name == "get_ForceMainThreadFinalization"), Is.False, "The feature switch must be constant-folded.");

			var dispose = nsObject.Methods.Single (method => method.Name == "Dispose" && method.Parameters.Count == 1);
			Assert.That (dispose.Body.Instructions.Any (instruction => instruction.Operand is MethodReference method && method.Name == "get_flags"), Is.EqualTo (enabled), "Opting out must remove the additional flag check from Dispose.");
			Assert.That (nsObject.Fields.Any (field => field.Name.Contains ("MainThread")), Is.False, "The release policy must not add an instance field.");
		}
	}
}
