//
// Unit tests for GKStateMachine
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using GameplayKit;

namespace MonoTouchFixtures.GameplayKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKStateMachineTests {

		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void StateMachineTests ()
		{
			var sm = new GKStateMachine (new GKState [] {
				new ChaseState (),
				new FleeState ()
			});

			Assert.That (sm.CurrentState, Is.Null, "CurrentState");

			Assert.That (sm, Is.Not.Null, "StateMachine must not be null");
			sm.EnterState (typeof (ChaseState));

			var chaseState = sm.GetState (typeof (ChaseState));
			Assert.That (chaseState, Is.Not.Null, "ChaseState must not be null");
			Assert.That (sm.CurrentState, Is.SameAs (chaseState), "Must be same state");

			var canEnterState = sm.EnterState (typeof (UndefinedState));
			Assert.That (canEnterState, Is.False, "Should not be able to enter that state since we did not allow it");
		}
	}

	class ChaseState : GKState {
		public override bool IsValidNextState (Class stateClass)
		{
			return (Class.Lookup (stateClass) != typeof (UndefinedState));
		}
	}

	class FleeState : GKState { }

	class UndefinedState : GKState { }
}
