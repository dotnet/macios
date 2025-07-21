
using System;
using System.Diagnostics;

using Foundation;
using CoreGraphics;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.Simd {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NMatrix4dTest {

		[Test]
		public void Identity ()
		{
			var identity = new NMatrix4d {
				M11 = 1d,
				M22 = 1d,
				M33 = 1d,
				M44 = 1d,
			};
			Asserts.AreEqual (identity, NMatrix4d.Identity, "identity");
		}
	}
}
