//
// SCNAnimatable.cs: helper methods
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreAnimation;

#nullable enable

namespace SceneKit {
	public partial class SCNAnimatable {

		/// <param name="animation">The animation to add.</param>
		///         <param name="key">The animation key.</param>
		///         <summary>Adds <paramref name="animation" />, identified with the specified <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		public void AddAnimation (CAAnimation animation, string? key = null)
		{
			var nskey = key is null ? null : new NSString (key);

			AddAnimation (animation, nskey);
			nskey?.Dispose ();
		}
	}
}
