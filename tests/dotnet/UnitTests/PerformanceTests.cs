// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Xamarin.Tests {
	[TestFixture]
	public class PerformanceTests : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		public void PrepareAssemblies (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			if (string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("ENABLE_PERFORMANCE_TESTS")))
				Assert.Ignore ("Test ignored because ENABLE_PERFORMANCE_TESTS is not set.");

			var projects = new string [] { "MySimpleApp", "monotouch-test" };
			var results = new List<PrepareAssembliesTestResult> ();
			var attempts = 3;
			foreach (var project in projects) {
				var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);

				var result = new PrepareAssembliesTestResult {
					Project = project,
				};
				foreach (var propertyValue in new bool [] { false, true }) {
					var properties = GetDefaultProperties (runtimeIdentifiers);
					properties ["PrepareAssemblies"] = propertyValue.ToString ();

					for (var i = 1; i <= attempts; i++) {
						Clean (project_path);
						var rv = DotNet.AssertBuild (project_path, properties);
						(propertyValue ? result.EnabledBuildTimes : result.DisabledBuildTimes).Add (rv.Duration);
					}
				}
				results.Add (result);
			}
			foreach (var result in results.OrderBy (v => v.Project)) {
				Console.WriteLine ($"Results for: {result.Project}");
				Console.WriteLine ($" Enabled timings: {string.Join (", ", result.EnabledBuildTimes.Select (v => v.ToString ()))} average: {TimeSpan.FromTicks ((long) result.EnabledBuildTimes.Average (v => v.Ticks))}");
				Console.WriteLine ($" Disabled timings: {string.Join (", ", result.DisabledBuildTimes.Select (v => v.ToString ()))} average: {TimeSpan.FromTicks ((long) result.DisabledBuildTimes.Average (v => v.Ticks))}");
			}
		}

		class PrepareAssembliesTestResult {
			public required string Project;
			public List<TimeSpan> EnabledBuildTimes = new ();
			public List<TimeSpan> DisabledBuildTimes = new ();
		}
	}
}

