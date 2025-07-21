//
// Unit tests for GKAgent3D
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//	
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

using System;

using Foundation;
using GameplayKit;

using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using VectorFloat3 = global::CoreGraphics.NVector3;

using Bindings.Test;
using NUnit.Framework;

namespace MonoTouchFixtures.GamePlayKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKAgent3DTest {
		[SetUp]
		public void Setup ()
		{
			// Headers and documentation say this was introduced in iOS 9.
			// My iOS 9.3.5 device doesn't agree:
			// > dyld: Symbol not found: _OBJC_CLASS_$_GKAgent3D
			// Apple's iOS 9 -> iOS 10 diff also says this class was introduced in iOS 10.
			TestRuntime.AssertXcodeVersion (8, 0);
		}
	}
}
