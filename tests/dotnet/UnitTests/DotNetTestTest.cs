using Xamarin.Tests;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class DotNetTestTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iostest")]
		[TestCase (ApplePlatform.TVOS, "tvostest")]
		// macOS and Mac Catalyst don't use mlaunch (they use 'open' via Desktop.targets),
		// so MTP support requires a different approach for these platforms.
		// [TestCase (ApplePlatform.MacCatalyst, "maccatalysttest")]
		// [TestCase (ApplePlatform.MacOSX, "macostest")]
		public void DotNetTest (ApplePlatform platform, string template)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var tmpDir = Cache.CreateTemporaryDirectory ();
			var outputDir = Path.Combine (tmpDir, template);
			DotNet.AssertNew (outputDir, template);
			var proj = Path.Combine (outputDir, $"{template}.csproj");

			// Replace generated tests with a single passing test
			var testFile = Path.Combine (outputDir, "Test1.cs");
			File.WriteAllText (testFile, $@"namespace {template};

[TestClass]
public sealed class Test1 {{
	[TestMethod]
	public void TestMethod1 ()
	{{
	}}
}}
");

			var properties = GetDefaultProperties ();

			// Build first to ensure the app bundle is created with the correct properties.
			DotNet.Execute ("build", proj, properties);

			// Verify ComputeRunArguments targets work with diagnostic output.
			// This helps diagnose failures that dotnet test hides (it uses loggers: null internally).
			DotNet.Execute ("build", proj, properties, target: "ComputeRunArguments");

			// Now run dotnet test with --no-build since we already built above.
			DotNet.Execute ("test", proj, properties, extraArguments: new [] { "--no-build" });
		}
	}
}
