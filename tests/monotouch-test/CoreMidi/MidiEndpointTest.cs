//
// Unit tests for MidiEndpointTest
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//	
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if !__TVOS__
using System;
using System.Collections.Generic;

using AudioToolbox;
using Foundation;
using CoreMidi;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEndpointTest {
		[Test]
		public void CorrectDisposeTest ()
		{
			// Test for bug 43582 - https://bugzilla.xamarin.com/show_bug.cgi?id=43582
			// This will throw if bug dispose code isn't fixed
			// System.InvalidOperationException: Handle is not initialized
			Assert.DoesNotThrow (() => {
				for (int i = 0; i < Midi.SourceCount; i++) {
					using (var endpoint = MidiEndpoint.GetSource (i)) {
						if (endpoint.Handle == 0)
							continue;
					}
				}
			});
		}

		[Test]
		public void SendTest ()
		{
			var anyChecks = false;

			for (var i = 0; i < Midi.DeviceCount; i++) {
				using var device = Midi.GetDevice (i);
				Assert.IsNotNull (device, "Device");
				for (var e = 0; e < device.EntityCount; e++) {
					using var entity = device.GetEntity (e);
					var endpoints = new List<MidiEndpoint> ();
					for (var d = 0; d < entity.Destinations; d++)
						endpoints.Add (entity.GetDestination (d));
					for (var d = 0; d < entity.Sources; d++)
						endpoints.Add (entity.GetSource (d));

					foreach (var ep in endpoints) {
						Assert.NotNull (ep, "EndPoint");

						// These APIs returns -50 (GeneralParamError) no matter what I do :/

						Assert.AreEqual (AudioQueueStatus.GeneralParamError, (AudioQueueStatus) ep.GetRefCons (out var ref1, out var ref2), "GetRefCons A");
						Assert.That (ref1, Is.EqualTo (IntPtr.Zero), "GetRefCons A 1");
						Assert.That (ref2, Is.EqualTo (IntPtr.Zero), "GetRefCons A 2");

						ref1 = unchecked((IntPtr) 0xfee1600d);
						ref2 = 0x42f00f00;
						Assert.AreEqual (AudioQueueStatus.GeneralParamError, (AudioQueueStatus) ep.SetRefCons (ref1, ref2), "SetRefCons B");
						Assert.AreEqual (AudioQueueStatus.GeneralParamError, (AudioQueueStatus) ep.GetRefCons (out ref1, out ref2), "GetRefCons C");
						Assert.That (ref1, Is.EqualTo (IntPtr.Zero) /* 0xfee1600d */, "GetRefCons C 1");
						Assert.That (ref2, Is.EqualTo (IntPtr.Zero) /* 0x42f00f00 */, "GetRefCons C 2");

						Assert.AreEqual (AudioQueueStatus.GeneralParamError, (AudioQueueStatus) ep.SetRefCons (IntPtr.Zero, IntPtr.Zero), "SetRefCons D");

						Assert.AreEqual (AudioQueueStatus.GeneralParamError, (AudioQueueStatus) ep.GetRefCons (out ref1, out ref2), "GetRefCons E");
						Assert.That (ref1, Is.EqualTo (IntPtr.Zero), "GetRefCons E 1");
						Assert.That (ref2, Is.EqualTo (IntPtr.Zero), "GetRefCons E 2");

						anyChecks = true;
					}
				}
			}

			if (!anyChecks)
				Assert.Inconclusive ("No applicable MidiEntity found.");
		}
	}
}
#endif
