//
// ARPointCloud.cs: Nicer code for ARPointCloud
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
#if NET
using Vector3 = global::CoreGraphics.NVector3;
#else
using Vector3 = global::OpenTK.NVector3;
#endif

#nullable enable

namespace ARKit {
	public partial class ARPointCloud {

		/// <summary>A set of positions in the world coordinate system. Image-processing tentatively believes that the position is a point on a real-world surface.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe Vector3 [] Points {
			get {
				var count = (int) Count;
				var rv = new Vector3 [count];
				var ptr = (Vector3*) GetRawPoints ();
				for (int i = 0; i < count; i++)
					rv [i] = *ptr++;
				return rv;
			}
		}

		/// <summary>Gets an array of identifiers that correspond, index by index, to each point in <see cref="ARKit.ARPointCloud.Points" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe ulong [] Identifiers {
			get {
				var count = (int) Count;
				var rv = new ulong [count];
				var ptr = (ulong*) GetRawIdentifiers ();
				for (int i = 0; i < count; i++)
					rv [i] = *ptr++;
				return rv;
			}
		}
	}
}
