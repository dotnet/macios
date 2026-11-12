// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLCaptureDescriptorTest {

		[Test]
		public void CaptureObjectOverloads ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ("Metal is not supported on this device.");

			using var descriptor = new MTLCaptureDescriptor ();
			Assert.That (descriptor.CaptureObject, Is.Null, "Initial CaptureObject");
			Assert.That (descriptor.TryGetCaptureObject (out IMTLDevice? outDevice), Is.False, "Initial TryGetCaptureObject IMTLDevice");
			Assert.That (outDevice, Is.Null, "Initial outDevice");
			Assert.That (descriptor.TryGetCaptureObject (out IMTLCommandQueue? outQueue), Is.False, "Initial TryGetCaptureObject IMTLCommandQueue");
			Assert.That (outQueue, Is.Null, "Initial outQueue");
			Assert.That (descriptor.TryGetCaptureObject (out IMTLCaptureScope? outScope), Is.False, "Initial TryGetCaptureObject IMTLCaptureScope");
			Assert.That (outScope, Is.Null, "Initial outScope");

			// Set IMTLDevice
			descriptor.SetCaptureObject (device);
			Assert.That (descriptor.CaptureObject?.Handle, Is.EqualTo (device.Handle), "Device CaptureObject Handle");
			Assert.That (descriptor.TryGetCaptureObject (out outDevice), Is.True, "Device TryGetCaptureObject IMTLDevice");
			using (outDevice) {
				Assert.That (outDevice?.Handle, Is.EqualTo (device.Handle), "Device outDevice Handle");
			}
			Assert.That (descriptor.TryGetCaptureObject (out outQueue), Is.False, "Device TryGetCaptureObject IMTLCommandQueue");
			Assert.That (outQueue, Is.Null, "Device outQueue");
			Assert.That (descriptor.TryGetCaptureObject (out outScope), Is.False, "Device TryGetCaptureObject IMTLCaptureScope");
			Assert.That (outScope, Is.Null, "Device outScope");

			descriptor.SetCaptureObject ((IMTLDevice?) null);
			Assert.That (descriptor.CaptureObject, Is.Null, "Cleared Device CaptureObject");

			// Set IMTLCommandQueue
			using var queue = device.CreateCommandQueue ();
			if (queue is not null) {
				descriptor.SetCaptureObject (queue);
				Assert.That (descriptor.CaptureObject?.Handle, Is.EqualTo (queue.Handle), "Queue CaptureObject Handle");
				Assert.That (descriptor.TryGetCaptureObject (out outQueue), Is.True, "Queue TryGetCaptureObject IMTLCommandQueue");
				using (outQueue) {
					Assert.That (outQueue?.Handle, Is.EqualTo (queue.Handle), "Queue outQueue Handle");
				}
				Assert.That (descriptor.TryGetCaptureObject (out outDevice), Is.False, "Queue TryGetCaptureObject IMTLDevice");
				Assert.That (outDevice, Is.Null, "Queue outDevice");
				Assert.That (descriptor.TryGetCaptureObject (out outScope), Is.False, "Queue TryGetCaptureObject IMTLCaptureScope");
				Assert.That (outScope, Is.Null, "Queue outScope");

				descriptor.SetCaptureObject ((IMTLCommandQueue?) null);
				Assert.That (descriptor.CaptureObject, Is.Null, "Cleared Queue CaptureObject");
			}

			// Set IMTLCaptureScope
			using var scope = MTLCaptureManager.Shared.CreateNewCaptureScope (device);
			if (scope is not null) {
				descriptor.SetCaptureObject (scope);
				Assert.That (descriptor.CaptureObject?.Handle, Is.EqualTo (scope.Handle), "Scope CaptureObject Handle");
				Assert.That (descriptor.TryGetCaptureObject (out outScope), Is.True, "Scope TryGetCaptureObject IMTLCaptureScope");
				using (outScope) {
					Assert.That (outScope?.Handle, Is.EqualTo (scope.Handle), "Scope outScope Handle");
				}
				Assert.That (descriptor.TryGetCaptureObject (out outDevice), Is.False, "Scope TryGetCaptureObject IMTLDevice");
				Assert.That (outDevice, Is.Null, "Scope outDevice");
				Assert.That (descriptor.TryGetCaptureObject (out outQueue), Is.False, "Scope TryGetCaptureObject IMTLCommandQueue");
				Assert.That (outQueue, Is.Null, "Scope outQueue");

				descriptor.SetCaptureObject ((IMTLCaptureScope?) null);
				Assert.That (descriptor.CaptureObject, Is.Null, "Cleared Scope CaptureObject");
			}
		}
	}
}
