//
// Unit tests for AUGraph
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioUnit;

namespace MonoTouchFixtures.AudioUnit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AUGraphTest {
		[Test]
		public void BasicOperations ()
		{
			using (var aug = new AUGraph ()) {
				aug.Open ();
				Assert.That (aug.IsOpen, Is.True, "#0");
				Assert.That (aug.IsInitialized, Is.False, "#0a");
				Assert.That (aug.IsRunning, Is.False, "#0b");

				var node = aug.AddNode (AudioComponentDescription.CreateOutput (AudioTypeOutput.Generic));
				int count;
				Assert.That (aug.GetNodeCount (out count), Is.EqualTo (AUGraphError.OK), "#1");
				Assert.That (count, Is.EqualTo (1), "#2");

				var info = aug.GetNodeInfo (node);
				Assert.That (info, Is.Not.Null, "#3");

				int node2;
				Assert.That (aug.GetNode (0, out node2), Is.EqualTo (AUGraphError.OK), "#4");
				Assert.That (node2, Is.EqualTo (1), "#4a");

				float max_load;
				Assert.That (aug.GetMaxCPULoad (out max_load), Is.EqualTo (AUGraphError.OK));
			}
		}

		[Test]
		public void Connections ()
		{
			using (var aug = new AUGraph ()) {
				aug.Open ();

				var node_1 = aug.AddNode (AudioComponentDescription.CreateGenerator (AudioTypeGenerator.AudioFilePlayer));
				var node_2 = aug.AddNode (AudioComponentDescription.CreateOutput (AudioTypeOutput.Generic));

				Assert.That (aug.ConnnectNodeInput (node_1, 0, node_2, 0), Is.EqualTo (AUGraphError.OK), "#1");
				uint count;
				aug.GetNumberOfInteractions (out count);
				Assert.That (count, Is.EqualTo (1), "#2");

				Assert.That (aug.Initialize (), Is.EqualTo (AUGraphError.OK), "#3");

				Assert.That (aug.ClearConnections (), Is.EqualTo (AUGraphError.OK), "#4");
				aug.GetNumberOfInteractions (out count);
				Assert.That (count, Is.EqualTo (0), "#5");
			}
		}

		[Test]
		public void CreateTest ()
		{
			int errCode;
			using (var aug = AUGraph.Create (out errCode)) {
				Assert.That (aug, Is.Not.Null, "CreateTest");
				Assert.That (errCode, Is.EqualTo (0), "CreateTest");

				// Make sure it is a working instance
				aug.Open ();
				Assert.That (aug.IsOpen, Is.True, "CreateTest #0");
				Assert.That (aug.IsInitialized, Is.False, "CreateTest #0a");
				Assert.That (aug.IsRunning, Is.False, "CreateTest #0b");
			}
		}

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "NewAUGraph")]
		static extern int NewAUGraph (ref IntPtr outGraph);

		[Test]
		public void GetNativeTest ()
		{
			IntPtr ret = IntPtr.Zero;
			var errCode = NewAUGraph (ref ret);
			Assert.That (errCode, Is.EqualTo (0), "GetNativeTest");
			Assert.That (ret, Is.Not.EqualTo (IntPtr.Zero), "ret");

			using (var aug = Runtime.GetINativeObject<AUGraph> (ret, true)) {
				Assert.That (aug, Is.Not.Null, "CreateTest");
				Assert.That ((IntPtr) aug.Handle, Is.EqualTo (ret), "Handle");

				// Make sure it is a working instance
				aug.Open ();
				Assert.That (aug.IsOpen, Is.True, "CreateTest #0");
				Assert.That (aug.IsInitialized, Is.False, "CreateTest #0a");
				Assert.That (aug.IsRunning, Is.False, "CreateTest #0b");
			}
		}
	}
}
