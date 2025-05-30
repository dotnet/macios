//
// SKKeyframeSequence helpers
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013,2015 Xamarin Inc.
//

using System;
using System.Collections.Generic;
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif
using Foundation;
using ObjCRuntime;

#nullable enable

namespace SpriteKit {
	public partial class SKKeyframeSequence {

		/// <param name="values">To be added.</param>
		///         <param name="times">To be added.</param>
		///         <summary>Creates a new <see cref="SpriteKit.SKKeyframeSequence" /> with the specified <paramref name="values" /> and <paramref name="times" />.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public SKKeyframeSequence (NSObject [] values, NSNumber [] times) :
			this (values, NSArray.FromNSObjects (times))
		{
		}

		/// <param name="values">To be added.</param>
		///         <param name="times">To be added.</param>
		///         <summary>Creates a new <see cref="SpriteKit.SKKeyframeSequence" /> with the specified <paramref name="values" /> and <paramref name="times" />.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public SKKeyframeSequence (NSObject [] values, float [] times) :
			this (values, NSArray.From (times))
		{
		}

		/// <param name="values">To be added.</param>
		///         <param name="times">To be added.</param>
		///         <summary>Creates a new <see cref="SpriteKit.SKKeyframeSequence" /> with the specified <paramref name="values" /> and <paramref name="times" />.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public SKKeyframeSequence (NSObject [] values, double [] times) :
			this (values, NSArray.From (times))
		{
		}
	}
}
