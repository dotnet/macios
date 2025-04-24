//
// GKObstacleGraph.cs: Implements Generic variant of GKObstacleGraph
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace GameplayKit {

	public partial class GKObstacleGraph {
#if !NET
		public virtual GKGraphNode2D [] GetNodes (GKPolygonObstacle obstacle)
#else
		/// <param name="obstacle">To be added.</param>
		///         <summary>Returns the array of <see cref="GameplayKit.GKGraphNode2D" /> corresponding to the <paramref name="obstacle" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public GKGraphNode2D [] GetNodes (GKPolygonObstacle obstacle)
#endif
		{
			return NSArray.ArrayFromHandle<GKGraphNode2D> (_GetNodes (obstacle));
		}
	}

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	[Register ("GKObstacleGraph", SkipRegistration = true)]
	public partial class GKObstacleGraph<NodeType> : GKObstacleGraph where NodeType : GKGraphNode2D {

		[Preserve (Conditional = true)]
		internal GKObstacleGraph (NativeHandle handle) : base (handle)
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public GKObstacleGraph (NSCoder coder) : base (coder)
		{
		}

		/// <param name="obstacles">To be added.</param>
		///         <param name="bufferRadius">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public GKObstacleGraph (GKPolygonObstacle [] obstacles, float bufferRadius) : base (obstacles, bufferRadius, new Class (typeof (NodeType)))
		{
		}

		/// <param name="obstacles">To be added.</param>
		///         <param name="bufferRadius">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static new GKObstacleGraph<NodeType>? FromObstacles (GKPolygonObstacle [] obstacles, float bufferRadius)
		{
			return Runtime.GetNSObject<GKObstacleGraph<NodeType>> (GraphWithObstacles (obstacles, bufferRadius, new Class (typeof (NodeType))));
		}

		/// <param name="obstacle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public new NodeType [] GetNodes (GKPolygonObstacle obstacle)
		{
			return NSArray.ArrayFromHandle<NodeType> (_GetNodes (obstacle));
		}
	}
}
