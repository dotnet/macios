using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.Utils;

namespace Xamarin.MMP.Tests {
	[TestFixture]
	public class TargetFrameworkMutateTests {
		const string MigrateCSProjTag = "<MigrateToNewXMIdentifier>true</MigrateToNewXMIdentifier>";

		public bool MatchesTFI (string expected, BuildResult buildOutput)
		{
			var tfiLine = buildOutput.BuildOutputLines.FirstOrDefault (x => x.StartsWith ("\t\t\tTargetFrameworkIdentifier=", StringComparison.Ordinal));
			if (tfiLine is null)
				return false;
			return tfiLine.Contains (expected);
		}

		[TestCase (true)]
		[TestCase (false)]
		public void ShouldNotMutateWithoutOptInFlag (bool xm45)
		{
			MMPTests.RunMMPTest (tmpDir => {
				TI.UnifiedTestConfig test = new TI.UnifiedTestConfig (tmpDir) {
					XM45 = xm45
				};
				var buildOutput = TI.TestUnifiedExecutable (test).BuildResult;
				string standardTFI = xm45 ? ".NETFramework" : "Xamarin.Mac";
				Assert.That (MatchesTFI (standardTFI, buildOutput), Is.True, $"Build did not have expected TFI.");
			});
		}

		[Test]
		public void ShouldNotMutateModernWithOptInFlag ()
		{
			MMPTests.RunMMPTest (tmpDir => {
				TI.UnifiedTestConfig test = new TI.UnifiedTestConfig (tmpDir) { CSProjConfig = MigrateCSProjTag };
				var buildOutput = TI.TestUnifiedExecutable (test).BuildResult;
				Assert.That (MatchesTFI ("Xamarin.Mac", buildOutput), Is.True, $"Build did not have expected TFI.");
			});
		}

		[Test]
		public void ShouldMutateFullWithOptInFlag ()
		{
			MMPTests.RunMMPTest (tmpDir => {
				TI.UnifiedTestConfig test = new TI.UnifiedTestConfig (tmpDir) {
					XM45 = true,
					CSProjConfig = MigrateCSProjTag
				};
				var buildOutput = TI.TestUnifiedExecutable (test).BuildResult;
				Assert.That (MatchesTFI ("Xamarin.Mac.NET", buildOutput), Is.True, $"Build did not have expected TFI.");
			});
		}
	}
}
