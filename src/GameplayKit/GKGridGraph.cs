//
// GKGridGraph.cs: Implements some nicer methods for GKGridGraph
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using Vector2i = global::CoreGraphics.NVector2i;

namespace GameplayKit {
	public partial class GKGridGraph {
		/// <summary>Gets the node at the specified <paramref name="position" />.</summary>
		/// <typeparam name="NodeType">The type of grid graph node to return.</typeparam>
		/// <param name="position">The position.</param>
		public NodeType? GetNodeAt<NodeType> (Vector2i position) where NodeType : GKGridGraphNode
		{
			return Runtime.GetNSObject<NodeType> (_GetNodeAt (position));
		}
	}
}
