//
// Unit tests for GKComponentSystem
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using GameplayKit;

namespace MonoTouchFixtures.GamePlayKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKComponentSystemTests {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void InitWithComponentClassType ()
		{
			var componentSystem = new GKComponentSystem<MySubcomponent> ();
			ClassicAssert.NotNull (componentSystem, "GKComponentSystem type ctor must not be null");
			ClassicAssert.AreEqual (typeof (MySubcomponent), componentSystem.ComponentType);
		}

		[Test]
		public void IndexerTest ()
		{
			var componentSystem = new GKComponentSystem<MySubcomponent> ();
			ClassicAssert.NotNull (componentSystem, "GKComponentSystem type ctor must not be null");
			ClassicAssert.AreEqual (typeof (MySubcomponent), componentSystem.ComponentType);

			componentSystem.AddComponent (new MySubcomponent (0));
			componentSystem.AddComponent (new MySubcomponent (1));
			componentSystem.AddComponent (new MySubcomponent (2));

			ClassicAssert.IsTrue (componentSystem.Components.Length == 3, "componentSystem.Components must be 3");
			var secondComponent = componentSystem [1] as MySubcomponent;
			ClassicAssert.NotNull (secondComponent, "secondComponent must not be null");
			ClassicAssert.IsTrue (secondComponent.Id == 1, "secondComponent.Id must be 1");
		}
	}

	[Preserve (AllMembers = true)]
	class MySubcomponent : GKComponent {

		public int Id { get; private set; }

		public MySubcomponent (int id)
		{
			Id = id;
		}

		public MySubcomponent (IntPtr handle) : base (handle) { }
	}
}
