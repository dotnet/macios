using System;
using Microsoft.DotNet.XHarness.Common.Logging;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Logging;

namespace Xharness {
	public class DeviceLogCapturerFactory {
		public IDeviceLogCapturer Create (ILog mainLog, ILog deviceLog, string? deviceName)
		{
			return new DeviceLogCapturer (mainLog, deviceLog, deviceName);
		}
	}
}
