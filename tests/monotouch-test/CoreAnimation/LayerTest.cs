//
// Unit tests for CALayer
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2011 Xamarin Inc. All rights reserved.
//

using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CoreGraphics;
using CoreAnimation;

namespace MonoTouchFixtures.CoreAnimation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LayerTest {

		static void Log (string message, [CallerMemberName] string member = "")
		{
			Console.WriteLine ($"[LayerTest.{member}] [Thread {Thread.CurrentThread.ManagedThreadId}] {message}");
		}

		[Test]
		public void Mask ()
		{
			Log ("start");
			using (CALayer layer = new CALayer ()) {
				Assert.Null (layer.Mask, "Mask/default");
				layer.Mask = new CALayer ();
				Assert.NotNull (layer.Mask, "Mask/assigned");
				layer.Mask = null;
				Assert.Null (layer.Mask, "Mask/nullable");
			}
			Log ("done");
		}

		[Test]
		public void CAActionTest ()
		{
			Log ("start");
			// bug 2441
			CAActionTestClass obj = new CAActionTestClass ();
			Assert.IsNull (obj.ActionForKey ("animation"), "a");
			Assert.IsNull (obj.Actions, "b");
			Assert.IsNull (CAActionTestClass.DefaultActionForKey ("animation"), "c");

			var animationKey = new NSString ("animation");
			var basicAnimationKey = new NSString ("basicAnimation");
			var dict = NSDictionary.FromObjectsAndKeys (
				new NSObject [] { new CABasicAnimation (), new CAAnimation () },
				new NSObject [] { basicAnimationKey, animationKey }
			);
			obj.Actions = dict;
			Assert.That (obj.Actions == dict, "d");

			Assert.That (obj.ActionForKey ("animation") == dict [animationKey], "e");
			Assert.That (obj.ActionForKey ("basicAnimation") == dict [basicAnimationKey], "f");
			Assert.IsNull (CAActionTestClass.DefaultActionForKey ("animation"), "g");
			Assert.IsNull (CALayer.DefaultActionForKey ("animation"), "h");
			Log ("done");
		}

		class CAActionTestClass : CALayer {

		}

		[Test]
		public void ConvertPoint ()
		{
			Log ("start");
			using (CALayer layer = new CALayer ()) {
				Assert.True (layer.ConvertPointFromLayer (CGPoint.Empty, null).IsEmpty, "From/Empty/null");
				Assert.True (layer.ConvertPointToLayer (CGPoint.Empty, null).IsEmpty, "To/Empty/null");
			}
			Log ("done");
		}

		[Test]
		public void ConvertRect ()
		{
			Log ("start");
			using (CALayer layer = new CALayer ()) {
				Assert.True (layer.ConvertRectFromLayer (CGRect.Empty, null).IsEmpty, "From/Empty/null");
				Assert.True (layer.ConvertRectToLayer (CGRect.Empty, null).IsEmpty, "To/Empty/null");
			}
			Log ("done");
		}

		[Test]
		public void ConvertTime ()
		{
			Log ("start");
			using (CALayer layer = new CALayer ()) {
				Assert.That (layer.ConvertTimeFromLayer (0.0d, null), Is.EqualTo (0.0d), "From/0.0d/null");
				Assert.That (layer.ConvertTimeToLayer (0.0d, null), Is.EqualTo (0.0d), "To/0.0d/null");
			}
			Log ("done");
		}

		[Test]
		public void AddAnimation ()
		{
			Log ("start");
			using (var layer = new CALayer ()) {
				var animation = new CABasicAnimation ();
				Assert.IsNull (layer.AnimationForKey ("key"), "#key A");
				layer.AddAnimation (animation, "key");
				Assert.IsNotNull (layer.AnimationForKey ("key"), "#key B");
			}
			Log ("done");
		}


		static int TextLayersDisposed;
		static int Generation;
		[Test]
		public void TestBug26532 ()
		{
			Log ("start");
			TextLayersDisposed = 0;
			Generation++;

			const int layerCount = 50;
			Exception ex = null;
			var thread = new Thread (() => {
				try {
					Log ("background thread started");
					var frame = new CGRect (0, 0, 200, 200);
					using (var layer = new CALayer ()) {
						for (int i = 0; i < layerCount; i++) {
							TextCALayer textLayer = new TextCALayer () {
								Secret = "42",
							};
							layer.AddSublayer (textLayer);
						}

						Log ("calling GC.Collect on background thread");
						GC.Collect ();
						Log ("GC.Collect on background thread completed");

						foreach (var slayer in layer.Sublayers.OfType<TextCALayer> ()) {
							Assert.AreEqual ("42", slayer.Secret);
						}

						Log ("removing sublayers");
						foreach (var slayer in layer.Sublayers.OfType<TextCALayer> ())
							slayer.RemoveFromSuperLayer ();
						Log ("sublayers removed");
					}
					Log ("background thread done");
				} catch (Exception e) {
					Log ($"background thread exception: {e}");
					ex = e;
				}
			});
			thread.Start ();
			thread.Join ();

			Log ("background thread joined, starting GC loop");
			var watch = new Stopwatch ();
			watch.Start ();
			int gcCount = 0;
			while (watch.ElapsedMilliseconds < 2000 && TextLayersDisposed < layerCount / 2) {
				gcCount++;
				Log ($"GC.Collect iteration {gcCount}, TextLayersDisposed={TextLayersDisposed}");
				GC.Collect ();
				NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (0.05));
			}
			Log ($"GC loop done after {gcCount} iterations, TextLayersDisposed={TextLayersDisposed}");

			Assert.IsNull (ex, "Exceptions");
			Assert.That (TextLayersDisposed, Is.AtLeast (layerCount / 2), "disposed text layers");
			Log ("done");
		}

		public class TextCALayer : CALayer {
			public string Secret;
			public int generation;

			public TextCALayer ()
			{
				generation = Generation;
			}

			protected override void Dispose (bool disposing)
			{
				if (generation == Generation) {
					TextLayersDisposed++;
				} else {
					Console.WriteLine ("TextCALayer.Dispose called for an object from a previous test run.");
				}
				base.Dispose (disposing);
			}
		}

		class Layer : CALayer { }
		class LayerDelegate : CALayerDelegate { }

		[Test]
		public void TestCALayerDelegateDispose ()
		{
			Log ("start");
			var del = new LayerDelegate ();
			var t = new Thread (() => {
				Log ("background thread: creating Layer, setting delegate, disposing");
				var l = new Layer ();
				l.Delegate = del;
				l.Dispose ();
				Log ("background thread: done");
			}) {
				IsBackground = true,
			};
			t.Start ();
			t.Join ();
			Log ("background thread joined, calling GC.Collect #1");
			GC.Collect ();
			Log ("GC.Collect #1 done, running runloop");

			NSRunLoop.Main.RunUntil (NSDate.Now.AddSeconds (0.1));

			Log ("runloop done, calling GC.Collect #2");
			GC.Collect ();
			Log ("GC.Collect #2 done, disposing delegate");
			del.Dispose ();
			Log ("done");
		}
	}
}
