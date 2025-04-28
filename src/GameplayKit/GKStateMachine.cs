//
// GKStateMachine.cs: Implements some nicer methods for GKStateMachine
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace GameplayKit {
	public partial class GKStateMachine : NSObject {

		/// <param name="stateType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public GKState? GetState (Type stateType)
		{
			return GetState (GKState.GetClass (stateType, "stateType"));
		}

		/// <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public GKState? GetState (GKState state)
		{
			return GetState (GKState.GetClass (state, "state"));
		}

		/// <param name="stateType">Must be a subclass of <see cref="T:GameplayKit.GKState" /></param>
		///         <summary>Whether a transition from <see cref="P:GameplayKit.GKStateMachine.CurrentState" /> to <paramref name="stateType" /> is valid.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool CanEnterState (Type stateType)
		{
			return CanEnterState (GKState.GetClass (stateType, "stateType"));
		}

		/// <param name="state">To be added.</param>
		///         <summary>Returns <see langword="true" /> if a transition from the current state of the state machine leads to <paramref name="state" />. Otherwise, returns <see langword="false" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool CanEnterState (GKState state)
		{
			return CanEnterState (GKState.GetClass (state, "state"));
		}

		/// <param name="stateType">To be added.</param>
		///         <summary>Attempts to transition from <see cref="P:GameplayKit.GKStateMachine.CurrentState" /> to <paramref name="stateType" />.</summary>
		///         <returns>
		///           <see langword="true" /> if the transition succeeded.</returns>
		///         <remarks>To be added.</remarks>
		public virtual bool EnterState (Type stateType)
		{
			return EnterState (GKState.GetClass (stateType, "stateType"));
		}

		/// <param name="state">To be added.</param>
		///         <summary>Attempts to transition from the current state to the specified <paramref name="state" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual bool EnterState (GKState state)
		{
			return EnterState (GKState.GetClass (state, "state"));
		}
	}
}
