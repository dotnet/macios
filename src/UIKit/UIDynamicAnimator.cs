//
// UIDynamicAnimator.cs: Extension methods to improve the API on UIDynamicAnimator
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2013 Xamarin Inc
//

using System.Collections;
using System.Collections.Generic;

namespace UIKit {
	public partial class UIDynamicAnimator :
	IEnumerable<UIDynamicBehavior> {
		/// <param name="behaviors">Behaviors that you want to add to the animator</param>
		///         <summary>Adds the array of specified behaviors.</summary>
		///         <remarks>
		///           <para>The following example shows how you can add a couple of behaviors to an animator:</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// public override void ViewDidLoad ()
		/// {
		/// 	base.ViewDidLoad ();
		///
		/// 	var gravityBehavior = new UIGravityBehavior (square);
		/// 	var collisionBehavior = new UICollisionBehavior (square) {
		/// 		TranslatesReferenceBoundsIntoBoundary = true
		/// 	};
		/// 	collisionBehavior.BeganBoundaryContact += BeganContact;
		/// 	collisionBehavior.EndedBoundaryContact += EndedContact;
		///
		/// 	animator = new UIDynamicAnimator (View);
		///
		/// 	// Add the two behaviors at once
		/// 	animator.AddBehaviors (gravityBehavior, collisionBehavior);
		/// }
		/// 	    ]]></code>
		///           </example>
		///         </remarks>
		public void AddBehaviors (params UIDynamicBehavior [] behaviors)
		{
			foreach (var behavior in behaviors)
				AddBehavior (behavior);
		}

		/// <param name="behaviors">Array of behaviors to be removed from the animator.</param>
		///         <summary>Removes the listed behaviors from the animator.</summary>
		///         <remarks>
		///         </remarks>
		public void RemoveBehaviors (params UIDynamicBehavior [] behaviors)
		{
			foreach (var behavior in behaviors)
				RemoveBehavior (behavior);
		}

		/// <param name="behavior">To be added.</param>
		///         <summary>Adds the specified behavior.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (UIDynamicBehavior behavior)
		{
			AddBehavior (behavior);
		}

		/// <summary>Returns an enumerator that iterates over the dynamic behaviors in the animator.</summary>
		IEnumerator<UIDynamicBehavior> IEnumerable<UIDynamicBehavior>.GetEnumerator ()
		{
			foreach (var behavior in Behaviors)
				yield return behavior;
		}

		/// <summary>Retrieves the behaviors via an enumerator.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			foreach (var behavior in Behaviors)
				yield return behavior;
		}
	}
}
