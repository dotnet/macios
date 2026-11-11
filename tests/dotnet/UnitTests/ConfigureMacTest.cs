// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class ConfigureMacTest : TestBaseClass {
		[Test]
		[Category ("WindowsInclusive")]
		public void RejectInvalidPort ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);

			var project = GetProjectPath ("MySimpleApp", "iossimulator-arm64", ApplePlatform.iOS, out _);
			var properties = GetDefaultProperties (includeRemoteProperties: false);
			properties ["ServerAddress"] = "127.0.0.1";
			properties ["ServerUser"] = "placeholder";
			properties ["ServerSshPort"] = "0";

			var result = DotNet.AssertBuildFailure (project, properties, target: "ConfigureMac");
			var errors = BinLog.GetBuildLogErrors (result.BinLogPath).ToArray ();
			Assert.That (errors.Select (v => v.Code), Does.Contain ("RMAC1001"), "Invalid SSH port");
		}
	}
}
