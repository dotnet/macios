//
// SKAction helpers
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc.
//

using System;
using CoreGraphics;
using CoreFoundation;

#nullable enable

namespace SpriteKit {
	public partial class SKAction {

		/// <param name="size">To be added.</param>
		///         <param name="duration">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKAction ResizeTo (CGSize size, double duration)
		{
			return SKAction.ResizeTo (size.Width, size.Height, duration);
		}
	}
}
