//
// SCNNode.cs: extensions to SCNNode
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)   
//
// Copyright Xamarin Inc.
//

using System;
using System.Collections;
using System.Collections.Generic;

using CoreAnimation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace SceneKit {
	public partial class SCNNode : IEnumerable, IEnumerable<SCNNode> {
		/// <param name="node">To be added.</param>
		///         <summary>Adds <paramref name="node" /> as a child of this <see cref="T:SceneKit.SCNNode" />.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (SCNNode node)
		{
			AddChildNode (node);
		}

		/// <param name="nodes">To be added.</param>
		///         <summary>Adds the specified <paramref name="nodes" /> as children of this <see cref="T:SceneKit.SCNNode" />.</summary>
		///         <remarks>To be added.</remarks>
		public void AddNodes (params SCNNode [] nodes)
		{
			if (nodes is null)
				return;
			foreach (var n in nodes)
				AddChildNode (n);
		}

		/// <summary>Gets an enumerator for iterating over the node's descendants.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<SCNNode> GetEnumerator ()
		{
			foreach (var node in ChildNodes)
				yield return node;
		}

		/// <summary>Gets an enumerator for the node's children.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		/// <param name="animation">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>Adds the <paramref name="animation" /> to this <see cref="T:SceneKit.SCNNode" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public void AddAnimation (CAAnimation animation, string? key)
		{
			if (key is null) {
				((ISCNAnimatable) this).AddAnimation (animation, (NSString?) null);
			} else {
				using (var s = new NSString (key))
					((ISCNAnimatable) this).AddAnimation (animation, s);
			}
		}

		public void RemoveAnimation (string key, nfloat duration)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			using (var s = new NSString (key))
				((ISCNAnimatable) this).RemoveAnimation (s, duration);
		}

		/// <param name="key">To be added.</param>
		///         <summary>Removes the animation that is identified by the provided <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveAnimation (string key)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			using (var s = new NSString (key))
				((ISCNAnimatable) this).RemoveAnimation (s);
		}

		/// <param name="key">To be added.</param>
		///         <summary>Returns the animation that is identified by the supplied <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CAAnimation? GetAnimation (string key)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			using (var s = new NSString (key))
				return ((ISCNAnimatable) this).GetAnimation (s);
		}

		/// <param name="key">To be added.</param>
		///         <summary>Pauses the animation that is identified by the provided <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public void PauseAnimation (string key)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			using (var s = new NSString (key))
				((ISCNAnimatable) this).PauseAnimation (s);
		}

		/// <param name="key">To be added.</param>
		///         <summary>Resumes the animation that is identified by the provided <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public void ResumeAnimation (string key)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			using (var s = new NSString (key))
				((ISCNAnimatable) this).ResumeAnimation (s);
		}

		/// <param name="key">To be added.</param>
		///         <summary>Returns a Boolean value that tells whether the animation that is identified by the specified <paramref name="key" /> is paused.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool IsAnimationPaused (string key)
		{
			if (string.IsNullOrEmpty (key))
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (key));

			bool isPaused;

			using (var s = new NSString (key))
				isPaused = ((ISCNAnimatable) this).IsAnimationPaused (s);

			return isPaused;
		}

#if !NET
		// SCNNodePredicate is defined as:
		// 	delegate bool SCNNodePredicate (SCNNode node, out bool stop);
		// but the actual objective-c definition of the block is
		// 	void (^)(SCNNode *child, BOOL *stop)
		//
		[Obsolete ("Use the overload that takes a 'SCNNodeHandler' instead.")]
		public virtual void EnumerateChildNodes (SCNNodePredicate predicate)
		{
			SCNNodeHandler predHandler = (SCNNode node, out bool stop) => {
				predicate (node, out stop);
			};
			EnumerateChildNodes (predHandler);
		}
#endif
	}
}
