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

		[Test]
		[Category ("WindowsInclusive")]
		[TestCase ("SubscribeToDeviceUpdates", "The 'DeviceUpdatesPipeName' property must be set to the name of a named pipe created by the caller.")]
		[TestCase ("StreamDeviceLog", "Exactly one of the 'DeviceLogPipeName' and 'DeviceLogFile' properties must be set.")]
		[TestCase ("StreamDeviceLog", "Exactly one of the 'DeviceLogPipeName' and 'DeviceLogFile' properties must be set.", "placeholder", "placeholder.log")]
		public void RequirePipeName (string target, string expectedError, string? deviceLogPipeName = null, string? deviceLogFile = null)
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);

			var project = GetProjectPath ("MySimpleApp", "iossimulator-arm64", ApplePlatform.iOS, out _);
			var properties = GetDefaultProperties (includeRemoteProperties: false);
			properties ["Device"] = "placeholder";
			if (deviceLogPipeName is not null)
				properties ["DeviceLogPipeName"] = deviceLogPipeName;
			if (deviceLogFile is not null)
				properties ["DeviceLogFile"] = deviceLogFile;

			var result = DotNet.AssertBuildFailure (project, properties, target: target);
			var errors = BinLog.GetBuildLogErrors (result.BinLogPath).Select (v => v.Message).ToArray ();
			Assert.That (errors, Does.Contain (expectedError), "Missing pipe name");
		}
	}
}
