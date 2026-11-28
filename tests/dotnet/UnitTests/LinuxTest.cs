// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace Xamarin.Tests {
	[TestFixture]
	[Category ("Linux")]
	public class LinuxTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void BuildMySimpleApp (ApplePlatform platform, string runtimeIdentifier)
		{
			Configuration.IgnoreIfNotOn (OSPlatform.Linux);
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifier, includeRemoteProperties: false);
			var result = DotNet.AssertBuild (projectPath, properties);

			AssertTargetExecuted (BinLog.GetAllTargets (result.BinLogPath), "CoreCompile", "C# compilation");
			var assemblyPath = Path.Combine (GetBinDir (projectPath, platform, runtimeIdentifier), "MySimpleApp.dll");
			Assert.That (assemblyPath, Does.Exist, "Compiled application assembly");
			using var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly (assemblyPath);
			Assert.That (assembly.MainModule.EntryPoint?.DeclaringType.FullName, Is.EqualTo ("MySimpleApp.Program"), "Compiled application entry point");
		}
	}
}
