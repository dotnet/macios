//
// MDLObject Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

using CoreGraphics;
using System.Linq;
using ModelIO;
using SceneKit;
#if !MONOMAC
using UIKit;
#endif

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLObjectTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
		}

		[Test]
		public void GetBoundingBox ()
		{
			using (var obj = MDLObject.FromNode (SCNNode.FromGeometry (SCNBox.Create (1, 1, 1, 0)))) {
				var bb = obj.GetBoundingBox (0);
			}
		}

		[Test]
		public void ProtocolTest ()
		{
			using (var obj = new MDLObject ()) {
				var p = new Protocol (typeof (IMDLComponent));
				obj.SetComponent (new MDLTransform (), p);
				Assert.That (obj.GetComponent (p), Is.Not.Null);
			}
		}

		[Test]
		public void ContainerEnumeration ()
		{
			using var container = new MDLObjectContainer ();
			using var first = new MDLObject ();
			using var second = new MDLObject ();
			Assert.That (container.ToArray (), Is.Empty, "Empty");
			container.AddObject (first);
			container.AddObject (second);
			Assert.That (container.ToArray (), Is.EqualTo (new [] { first, second }), "Class");
			IMDLObjectContainerComponent protocol = container;
			Assert.That (protocol.ToArray (), Is.EqualTo (new [] { first, second }), "Protocol");
		}
	}
}
