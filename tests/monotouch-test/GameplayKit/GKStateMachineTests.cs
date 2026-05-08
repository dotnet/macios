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

			ClassicAssert.Null (sm.CurrentState, "CurrentState");

			ClassicAssert.NotNull (sm, "StateMachine must not be null");
			sm.EnterState (typeof (ChaseState));

			var chaseState = sm.GetState (typeof (ChaseState));
			ClassicAssert.NotNull (chaseState, "ChaseState must not be null");
			ClassicAssert.AreSame (chaseState, sm.CurrentState, "Must be same state");

			var canEnterState = sm.EnterState (typeof (UndefinedState));
			ClassicAssert.IsFalse (canEnterState, "Should not be able to enter that state since we did not allow it");
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
