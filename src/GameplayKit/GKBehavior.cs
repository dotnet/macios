//
// GKBehavior.cs: Implements some nicer methods for GKBehavior
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;

namespace GameplayKit {
	public partial class GKBehavior {

		/// <param name="index">To be added.</param>
		/// <summary>Retrieves the <see cref="T:GameplayKit.GKGoal" /> at the specified index. (see <see cref="P:GameplayKit.GKBehavior.GoalCount" />)</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public GKGoal this [nuint index] {
			get { return ObjectAtIndexedSubscript (index); }
		}

		/// <param name="goal">To be added.</param>
		/// <summary>Retrieves the weight for the <paramref name="goal" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public NSNumber this [GKGoal goal] {
			// The docs show that ObjectForKeyedSubscript should return 0.0 if the GKGoal is not
			// available but actually returns null: https://developer.apple.com/documentation/gameplaykit/gkbehavior/1388723-objectforkeyedsubscript?language=objc
			// radar filed here: https://feedbackassistant.apple.com/feedback/9979863
			get { return ObjectForKeyedSubscript (goal) ?? throw new ArgumentOutOfRangeException (nameof (goal)); }
			set { SetObject (value, goal); }
		}
	}
}
