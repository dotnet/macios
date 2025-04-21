//
// SCNScene.cs: extensions to SCNScene
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)   
//
// Copyright Xamarin Inc.
//

using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace SceneKit {
	public partial class SCNScene : IEnumerable<SCNNode> {
		/// <param name="node">To be added.</param>
		///         <summary>Adds a node to the scene.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (SCNNode node)
		{
			RootNode.AddChildNode (node);
		}

		/// <summary>Returns an enumerator for iterating over the nodes in the scene.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<SCNNode> GetEnumerator ()
		{
			return RootNode.GetEnumerator ();
		}

		/// <summary>Internal.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}
