// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Xamarin.Tests {
	[TestFixture]
	public class WarningPropertiesTest : TestBaseClass {
		[TestCase ("", false, "", "", false, 1)]
		[TestCase ("MT1234", false, "", "", false, 0)]
		[TestCase ("CS1234,MT1234", true, "", "", false, 0)]
		[TestCase ("", true, "", "", true, 0)]
		[TestCase ("", false, "MT1234", "", true, 0)]
		[TestCase ("", true, "", "MT1234", false, 1)]
		[TestCase ("", true, "MT1234", "MT1234", false, 1)]
		[TestCase ("", false, "MT1234", "MT1234", true, 0)]
		public void MSBuildTasks (string noWarn, bool treatWarningsAsErrors, string warningsAsErrors, string warningsNotAsErrors, bool fails, int warningCount)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var projectPath = Path.Combine (directory, "Warnings.proj");
			var targetsPath = Path.Combine (Configuration.RootPath, "msbuild", "Xamarin.Shared", "Xamarin.Shared.targets");
			File.WriteAllText (projectPath, $"""
				<Project DefaultTargets="Build">
					<PropertyGroup>
						<_TaskAssemblyName>$(MSBuildToolsPath)/Microsoft.Build.Tasks.Core.dll</_TaskAssemblyName>
						<NoWarn>{noWarn}</NoWarn>
						<TreatWarningsAsErrors>{treatWarningsAsErrors}</TreatWarningsAsErrors>
						<WarningsAsErrors>{warningsAsErrors}</WarningsAsErrors>
						<WarningsNotAsErrors>{warningsNotAsErrors}</WarningsNotAsErrors>
					</PropertyGroup>
					<Import Project="{targetsPath}" />
					<Target Name="Restore" />
					<Target Name="Build">
						<Warning Code="MT1234" Text="Test warning" />
					</Target>
				</Project>
				""");
			var result = fails ? DotNet.AssertBuildFailure (projectPath) : DotNet.AssertBuild (projectPath);
			var warnings = BinLog.GetBuildLogWarnings (result.BinLogPath).ToArray ();
			Assert.That (warnings.Length, Is.EqualTo (warningCount), "Warning count");
			var errors = BinLog.GetBuildLogErrors (result.BinLogPath).ToArray ();
			Assert.That (errors.Length, Is.EqualTo (fails ? 1 : 0), "Error count");
			if (fails)
				Assert.That (errors [0].Code, Is.EqualTo ("MT1234"), "Promoted warning code");
		}
	}
}
