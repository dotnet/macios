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
			Assert.That (componentSystem, Is.Not.Null, "GKComponentSystem type ctor must not be null");
			Assert.That (componentSystem.ComponentType, Is.EqualTo (typeof (MySubcomponent)));
		}

		[Test]
		public void IndexerTest ()
		{
			var componentSystem = new GKComponentSystem<MySubcomponent> ();
			Assert.That (componentSystem, Is.Not.Null, "GKComponentSystem type ctor must not be null");
			Assert.That (componentSystem.ComponentType, Is.EqualTo (typeof (MySubcomponent)));

			componentSystem.AddComponent (new MySubcomponent (0));
			componentSystem.AddComponent (new MySubcomponent (1));
			componentSystem.AddComponent (new MySubcomponent (2));

			Assert.That (componentSystem.Components.Length == 3, Is.True, "componentSystem.Components must be 3");
			var secondComponent = componentSystem [1] as MySubcomponent;
			Assert.That (secondComponent, Is.Not.Null, "secondComponent must not be null");
			Assert.That (secondComponent.Id == 1, Is.True, "secondComponent.Id must be 1");
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
