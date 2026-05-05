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

			// Mobile platforms use mlaunch to run the app in the simulator.
			// Override MlaunchPath if set via the environment variable.
			var mlaunchPath = Environment.GetEnvironmentVariable ("MLAUNCH_PATH");
			if (!string.IsNullOrEmpty (mlaunchPath)) {
				properties ["MlaunchPath"] = mlaunchPath;
			}

			DotNet.Execute ("test", proj, properties);
		}
	}
}
