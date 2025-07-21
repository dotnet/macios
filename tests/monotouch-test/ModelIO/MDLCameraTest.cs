//
// MDLCamera Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc.
//

#if !MONOMAC

using System;
using CoreGraphics;
using Foundation;
#if !MONOMAC
using UIKit;
#endif
#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;
using ObjCRuntime;

using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using VectorFloat3 = global::CoreGraphics.NVector3;

using Bindings.Test;

using NUnit.Framework;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDCameraTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}
	}
}

#endif // !MONOMAC
