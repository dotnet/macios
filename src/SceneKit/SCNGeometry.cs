//
// SCNGeometry.cs: extensions to SCNGeometry
//
// Authors:
//   MIguel de Icaza (miguel@xamarin.com)
//
// Copyright Xamarin Inc
//
using System;

using XamCore.CoreGraphics;
using XamCore.Foundation;

namespace XamCore.SceneKit {
	public partial class SCNGeometry {
#if !XAMCORE_3_0
		[Obsolete ("Use the 'Create (SCNGeometrySource[], SCNGeometryElement[])' method instead, as it has a strongly typed return.")]
		public static NSObject FromSources (SCNGeometrySource [] sources, SCNGeometryElement [] elements)
		{
			return Create (sources, elements);
		}
#endif
	}
}
