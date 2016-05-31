﻿//
// Unit tests for SCNNode
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

using System;
#if XAMCORE_2_0
using CoreAnimation;
using Foundation;
using SceneKit;
using UIKit;
#else
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.SceneKit;
using MonoTouch.UIKit;
#endif
using NUnit.Framework;

#if XAMCORE_2_0
using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using PointF=CoreGraphics.CGPoint;
#else
using nfloat=global::System.Single;
using nint=global::System.Int32;
using nuint=global::System.UInt32;
#endif

namespace MonoTouchFixtures.SceneKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NodeTest {

		[Test]
		public void AddAnimation ()
		{
			if (!TestRuntime.CheckSystemAndSDKVersion (8,0))
				Assert.Ignore ("Requires iOS8");

			using (var a = CAAnimation.CreateAnimation ())
			using (var n = SCNNode.Create ()) {
				n.AddAnimation (a, (NSString) null);
				n.AddAnimation (a, (string) null);
				string key = "key";
				n.AddAnimation (a, key);
				using (var s = new NSString (key))
					n.AddAnimation (a, key);
			}
		}
	}
}

#endif // !__WATCHOS__
