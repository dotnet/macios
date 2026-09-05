#nullable enable

namespace Xamarin.Tests {
	public class MaxSupportedOSPlatformVersionTest : TestBaseClass {
		// This test builds a project that doesn't set SupportedOSPlatformVersion explicitly (and
		// doesn't import tests/common/shared-dotnet.csproj, which would otherwise pin it to each
		// platform's minimum supported OS version). In this configuration the .NET SDK defaults
		// SupportedOSPlatformVersion to the highest installed TargetPlatformVersion (i.e. the
		// newest Xcode SDK version), which is also what happens for any "plain" dotnet build that
		// doesn't explicitly pin a deployment target.
		//
		// With a deployment target this high, the platform assemblies may contain types that
		// Apple has obsoleted (removed) as of that exact SDK version, and the static registrar
		// must correctly avoid emitting native implementations for such types (or their protocol
		// conformances), or the native compiler will fail to build the generated registrar code.
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void Build (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project = "MaxSupportedOSPlatformVersion";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);

			DotNet.AssertBuild (project_path, properties);
		}
	}
}
