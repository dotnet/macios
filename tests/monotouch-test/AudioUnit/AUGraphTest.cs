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
				ClassicAssert.IsTrue (aug.IsOpen, "#0");
				ClassicAssert.IsFalse (aug.IsInitialized, "#0a");
				ClassicAssert.IsFalse (aug.IsRunning, "#0b");

				var node = aug.AddNode (AudioComponentDescription.CreateOutput (AudioTypeOutput.Generic));
				int count;
				ClassicAssert.AreEqual (AUGraphError.OK, aug.GetNodeCount (out count), "#1");
				ClassicAssert.AreEqual (1, count, "#2");

				var info = aug.GetNodeInfo (node);
				ClassicAssert.IsNotNull (info, "#3");

				int node2;
				ClassicAssert.AreEqual (AUGraphError.OK, aug.GetNode (0, out node2), "#4");
				ClassicAssert.AreEqual (1, node2, "#4a");

				float max_load;
				ClassicAssert.AreEqual (AUGraphError.OK, aug.GetMaxCPULoad (out max_load));
			}
		}

		[Test]
		public void Connections ()
		{
			using (var aug = new AUGraph ()) {
				aug.Open ();

				var node_1 = aug.AddNode (AudioComponentDescription.CreateGenerator (AudioTypeGenerator.AudioFilePlayer));
				var node_2 = aug.AddNode (AudioComponentDescription.CreateOutput (AudioTypeOutput.Generic));

				ClassicAssert.AreEqual (AUGraphError.OK, aug.ConnnectNodeInput (node_1, 0, node_2, 0), "#1");
				uint count;
				aug.GetNumberOfInteractions (out count);
				ClassicAssert.AreEqual (1, count, "#2");

				ClassicAssert.AreEqual (AUGraphError.OK, aug.Initialize (), "#3");

				ClassicAssert.AreEqual (AUGraphError.OK, aug.ClearConnections (), "#4");
				aug.GetNumberOfInteractions (out count);
				ClassicAssert.AreEqual (0, count, "#5");
			}
		}

		[Test]
		public void CreateTest ()
		{
			int errCode;
			using (var aug = AUGraph.Create (out errCode)) {
				ClassicAssert.NotNull (aug, "CreateTest");
				ClassicAssert.AreEqual (0, errCode, "CreateTest");

				// Make sure it is a working instance
				aug.Open ();
				ClassicAssert.IsTrue (aug.IsOpen, "CreateTest #0");
				ClassicAssert.IsFalse (aug.IsInitialized, "CreateTest #0a");
				ClassicAssert.IsFalse (aug.IsRunning, "CreateTest #0b");
			}
		}

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "NewAUGraph")]
		static extern int NewAUGraph (ref IntPtr outGraph);

		[Test]
		public void GetNativeTest ()
		{
			IntPtr ret = IntPtr.Zero;
			var errCode = NewAUGraph (ref ret);
			ClassicAssert.AreEqual (0, errCode, "GetNativeTest");
			Assert.That (ret, Is.Not.EqualTo (IntPtr.Zero), "ret");

			using (var aug = Runtime.GetINativeObject<AUGraph> (ret, true)) {
				ClassicAssert.NotNull (aug, "CreateTest");
				Assert.That ((IntPtr) aug.Handle, Is.EqualTo (ret), "Handle");

				// Make sure it is a working instance
				aug.Open ();
				ClassicAssert.IsTrue (aug.IsOpen, "CreateTest #0");
				ClassicAssert.IsFalse (aug.IsInitialized, "CreateTest #0a");
				ClassicAssert.IsFalse (aug.IsRunning, "CreateTest #0b");
			}
		}
	}
}
