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
			Assert.That (CFunctions.theUltimateAnswer (), Is.EqualTo (42), "a");
#if !__MACOS__
			Assert.That (CFunctions.object_theUltimateAnswer (), Is.EqualTo (42), "object");
			Assert.That (CFunctions.ar_theUltimateAnswer (), Is.EqualTo (42), "ar");
#endif
		}

		[Test]
		public void ObjCClass ()
		{
			using (var obj = new FrameworkTest ()) {
				Assert.That (obj.Func (), Is.EqualTo (42), "a");
			}
		}
	}
}
