//
// SCNScene.cs: extensions to SCNScene
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)   
//
// Copyright Xamarin Inc.
//

using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace SceneKit {
	public partial class SCNScene : IEnumerable<SCNNode> {
		/// <param name="node">The node.</param>
		///         <summary>Adds a node to the scene.</summary>
		public void Add (SCNNode node)
		{
			RootNode.AddChildNode (node);
		}

		/// <summary>Returns an enumerator for iterating over the nodes in the scene.</summary>
		public IEnumerator<SCNNode> GetEnumerator ()
		{
			return RootNode.GetEnumerator ();
		}

		/// <summary>Internal.</summary>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}
