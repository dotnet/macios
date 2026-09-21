//
// Unit tests for GKEntity
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
	public class GKEntityTests {

		[Test]
		public void GetAndRemoveTest ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var entity = GKEntity.GetEntity ();
			entity.AddComponent (new NumberComponent (10));
			entity.AddComponent (new NameComponent ("Ten"));
			Assert.That (entity.Components.Length == 2, Is.True, "entity.Components length must be 2");

			// Test component retrieval by type
			var component = entity.GetComponent (typeof (NumberComponent)) as NumberComponent;
			Assert.That (component, Is.Not.Null, "Component must not be null");
			Assert.That (component.Id == 10, Is.True, "Component Id must be 10");

			// Test component removal by type
			Assert.That (entity.GetComponent (typeof (NameComponent)), Is.Not.Null, "Component typeof NameComponent must not be null");
			entity.RemoveComponent (typeof (NameComponent));
			Assert.That (entity.Components.Length == 1, Is.True, "entity.Components length must be 1");
			Assert.That (entity.GetComponent (typeof (NameComponent)), Is.Null, "Component typeof NameComponent must be null");
		}

		[Test]
		public void BadGetComponent ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var entity = GKEntity.GetEntity ();
			Assert.Throws<ArgumentNullException> (() => entity.GetComponent (null));
		}

		[Test]
		public void BadRemoval ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			var entity = GKEntity.GetEntity ();
			Assert.Throws<ArgumentNullException> (() => entity.RemoveComponent (null));
		}
	}

	[Preserve (AllMembers = true)]
	class NameComponent : GKComponent {

		public string Name { get; private set; }

		public NameComponent (string name)
		{
			Name = name;
		}

		public NameComponent (IntPtr handle) : base (handle) { }
	}

	[Preserve (AllMembers = true)]
	class NumberComponent : GKComponent {

		public int Id { get; private set; }

		public NumberComponent (int id)
		{
			Id = id; ;
		}

		public NumberComponent (IntPtr handle) : base (handle) { }
	}
}
