//
// SKAction helpers
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc.
//

using CoreGraphics;
using CoreFoundation;

#nullable enable

namespace SpriteKit {
	public partial class SKAction {

		/// <param name="size">The size.</param>
		///         <param name="duration">The duration.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		public static SKAction ResizeTo (CGSize size, double duration)
		{
			return SKAction.ResizeTo (size.Width, size.Height, duration);
		}
	}
}
