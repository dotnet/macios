// Copyright 2011 Xamarin Inc. All rights reserved

#if IOS || MONOMAC
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AudioToolbox;
using AudioUnit;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioComponentTest {

		[Test]
		public void GetSetComponentList ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
#if !MONOMAC
			var types = new List<AudioTypeOutput> { AudioTypeOutput.Generic, AudioTypeOutput.Remote, AudioTypeOutput.VoiceProcessingIO };
#else
			var types = new List<AudioTypeOutput> { AudioTypeOutput.Generic, AudioTypeOutput.Default, AudioTypeOutput.HAL, AudioTypeOutput.System, AudioTypeOutput.VoiceProcessingIO };
#endif
			foreach (var t in types) {
				var resources = new ResourceUsageInfo ();
				resources.IOKitUserClient = new string [] { "CustomUserClient1" };
				resources.MachLookUpGlobalName = new string [] { "MachServiceName1" };
				resources.NetworkClient = false;
				resources.TemporaryExceptionReadWrite = false;

				var componentInfo = new AudioComponentInfo ();
				componentInfo.Type = t.ToString ();
				componentInfo.Subtype = "XMPL";
				componentInfo.Name = "XMPL";
				componentInfo.Version = 1;
				componentInfo.ResourceUsage = resources;
				var component = AudioComponent.FindComponent (t);
				if (component is null)
					continue;
				var l = component.ComponentList;
				Assert.That (l, Is.Null, "List is not null.");
				l = new AudioComponentInfo [] { componentInfo };
				//monotouchtests does not have permissions to deal with the hwd.
				Assert.Throws<InvalidOperationException> (() => component.ComponentList = l);
			}
		}

		[Test]
		public void GetSetNullComponentList ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
#if !MONOMAC
			var types = new List<AudioTypeOutput> { AudioTypeOutput.Generic, AudioTypeOutput.Remote, AudioTypeOutput.VoiceProcessingIO };
#else
			var types = new List<AudioTypeOutput> { AudioTypeOutput.Generic, AudioTypeOutput.Default, AudioTypeOutput.HAL, AudioTypeOutput.System, AudioTypeOutput.VoiceProcessingIO };
#endif
			foreach (var t in types) {
				var resources = new ResourceUsageInfo ();
				resources.IOKitUserClient = new string [] { "CustomUserClient1" };
				resources.MachLookUpGlobalName = new string [] { "MachServiceName1" };
				resources.NetworkClient = false;
				resources.TemporaryExceptionReadWrite = false;

				var componentInfo = new AudioComponentInfo ();
				componentInfo.Type = t.ToString ();
				componentInfo.Subtype = "XMPL";
				componentInfo.Name = "XMPL";
				componentInfo.Version = 1;
				componentInfo.ResourceUsage = resources;
				var component = AudioComponent.FindComponent (t);
				if (component is null)
					continue;
				//monotouchtests does not have permissions to deal with the hwd.
				Assert.Throws<InvalidOperationException> (() => component.ComponentList = null);
			}
		}

		// test the diff properties of the ResourceUsageInfo since it was manually done

		[Test]
		public void TestResourceUsageInfoIOKitUserClient ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			var clientId = "CustomUserClient1";
			var resources = new ResourceUsageInfo ();
			resources.IOKitUserClient = new string [] { clientId };
			var userClientList = resources.IOKitUserClient;
			Assert.That (userClientList, Is.Not.Null);
			Assert.That (userClientList.Length, Is.EqualTo (1), "List does not have all client ids.");
			Assert.That (userClientList [0], Is.EqualTo (clientId), "Client ids are not the same.");

			// similar test but with null values.

			resources.IOKitUserClient = null;
			Assert.That (resources.IOKitUserClient, Is.Null, "Value was not set to null.");
		}

		[Test]
		public void TestResourceUsageInfoMachLookUpGlobalName ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			var serviceName = "MachServiceName1";
			var resources = new ResourceUsageInfo ();
			resources.MachLookUpGlobalName = new string [] { serviceName };
			var serviceNames = resources.MachLookUpGlobalName;
			Assert.That (serviceNames, Is.Not.Null, "Returned list is null");
			Assert.That (serviceNames.Length, Is.EqualTo (1), "List does not have all service names.");
			Assert.That (serviceNames [0], Is.EqualTo (serviceName), "Service names are not equal.");

			// similar test but with null values

			resources.MachLookUpGlobalName = null;
			Assert.That (resources.MachLookUpGlobalName, Is.Null, "Value was no set to null.");
		}

		[Test]
		public void TestConfigurationInfo ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			var resources = new ResourceUsageInfo ();
			resources.IOKitUserClient = new string [] { "CustomUserClient1" };
			resources.MachLookUpGlobalName = new string [] { "MachServiceName1" };
			resources.NetworkClient = false;
			resources.TemporaryExceptionReadWrite = false;

			var componentInfo = new AudioComponentInfo ();
			componentInfo.Type = AudioTypeOutput.Generic.ToString ();
			componentInfo.Subtype = "XMPL";
			componentInfo.Name = "XMPL";
			componentInfo.Version = 1;
			componentInfo.ResourceUsage = resources;
			using var component = AudioComponent.FindComponent (AudioTypeOutput.Generic);
			Assert.That (component, Is.Not.Null);
			// assert the property and break
			var configInfo = component.GetConfigurationInfo ();
			Assert.That (configInfo, Is.Not.Null);
		}

		[Test]
		[Ignore ("This test times out randomly")]
		public void TestValidation ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			if (TestRuntime.CheckXcodeVersion (16, 0) && TestRuntime.IsInCI)
				Assert.Ignore ("This test started randomly timing out in Xcode 16 beta 1, for unknown reasons.");

			var tcs = new TaskCompletionSource<bool> ();
			var thread = new Thread (() => {
				try {
					var resources = new ResourceUsageInfo ();
					resources.IOKitUserClient = new string [] { "CustomUserClient1" };
					resources.MachLookUpGlobalName = new string [] { "MachServiceName1" };
					resources.NetworkClient = false;
					resources.TemporaryExceptionReadWrite = false;

					var componentInfo = new AudioComponentInfo ();
					componentInfo.Type = AudioTypeOutput.Generic.ToString ();
					componentInfo.Subtype = "XMPL";
					componentInfo.Name = "XMPL";
					componentInfo.Version = 1;
					componentInfo.ResourceUsage = resources;
					using var component = AudioComponent.FindComponent (AudioTypeOutput.Generic);
					Assert.That (component, Is.Not.Null);
					// validate and break
					var validation = component.Validate (null);
					Assert.That (new List<AudioComponentValidationResult> () { AudioComponentValidationResult.Unknown, AudioComponentValidationResult.Passed }, Does.Contain (validation), "validation");
					tcs.SetResult (true);
				} catch (Exception e) {
					tcs.SetException (e);
				}
			});
			thread.IsBackground = true;
			thread.Start ();
			Assert.That (tcs.Task.Wait (TimeSpan.FromSeconds (20)), Is.True, "Timed out");
		}

		[Test]
		[Ignore ("This test times out randomly")]
		public void TestValidationAsync ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);

			if (TestRuntime.CheckXcodeVersion (16, 0) && TestRuntime.IsInCI)
				Assert.Ignore ("This test started randomly timing out in Xcode 16 beta 1, for unknown reasons.");

			var resources = new ResourceUsageInfo ();
			resources.IOKitUserClient = new string [] { "CustomUserClient1" };
			resources.MachLookUpGlobalName = new string [] { "MachServiceName1" };
			resources.NetworkClient = false;
			resources.TemporaryExceptionReadWrite = false;

			var componentInfo = new AudioComponentInfo ();
			componentInfo.Type = AudioTypeOutput.Generic.ToString ();
			componentInfo.Subtype = "XMPL";
			componentInfo.Name = "XMPL";
			componentInfo.Version = 1;
			componentInfo.ResourceUsage = resources;
			using var component = AudioComponent.FindComponent (AudioTypeOutput.Generic);
			Assert.That (component, Is.Not.Null);

			var cbEvent = new AutoResetEvent (false);
			Action<AudioComponentValidationResult, NSDictionary?> cb = (AudioComponentValidationResult _, NSDictionary? _) => {
				cbEvent.Set ();
			};
			component.ValidateAsync (cb);
			Assert.That (cbEvent.WaitOne (20000), Is.True, "Cb was not called.");
		}
	}
}

#endif
