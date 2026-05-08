//
// Framework tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using MonoTouch;

using Bindings.Test;

namespace MonoTouchFixtures {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FrameworkTests {
		[Test]
		public void CFunction ()
		{
			ClassicAssert.AreEqual (42, CFunctions.theUltimateAnswer (), "a");
#if !__MACOS__
			ClassicAssert.AreEqual (42, CFunctions.object_theUltimateAnswer (), "object");
			ClassicAssert.AreEqual (42, CFunctions.ar_theUltimateAnswer (), "ar");
#endif
		}

		[Test]
		public void ObjCClass ()
		{
			using (var obj = new FrameworkTest ()) {
				ClassicAssert.AreEqual (42, obj.Func (), "a");
			}
		}
	}
}
