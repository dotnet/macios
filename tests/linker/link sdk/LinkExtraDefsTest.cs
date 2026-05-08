//
// Unit tests for the mtouch's --xml linker option
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.Reflection;

namespace LinkSdk {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LinkExtraDefsTest {

		// ensure the types in extra-linker-defs.xml are included
		// even if they are:
		// * part of SDK / product assemblies, where adding [Preserve] is not possible;
		// * not used anywhere in this application

		// note: reflection is used so we're testing the XML-based preservation
		// 	not the normal linking process

		[Test]
		public void Corlib ()
		{
			var t = Type.GetType ("System.Security.PermissionSet, " + typeof (int).Assembly.GetName ().Name);
			Assert.That (t, Is.Not.Null, "System.Security.PermissionSet");
			if (t is null)
				throw new InvalidOperationException ("System.Security.PermissionSet");
		}

		[Test]
		public void System ()
		{
			var t = Type.GetType ("System.Net.Mime.ContentType, System.Net.Mail");
			Assert.That (t, Is.Not.Null, "System.Net.Mime.ContentType");
			if (t is null)
				throw new InvalidOperationException ("System.Net.Mime.ContentType");
			// we asked for ParseValue to be preserved
			Assert.That (t.GetMethod ("ParseValue", BindingFlags.Instance | BindingFlags.NonPublic), Is.Not.Null, "Parse");
		}

#if !__MACOS__
		[Test]
		public void MonoTouch ()
		{
			var t = Type.GetType ("CoreBluetooth.CBUUID, " + typeof (NSObject).Assembly.ToString ());
			Assert.That (t, Is.Not.Null, "[MonoTouch.]CoreBluetooth.CBUUID");
			if (t is null)
				throw new InvalidOperationException ("CoreBluetooth.CBUUID");
			// check (generated) fields since we instructed the linker to keep them
			var f = t.GetFields (BindingFlags.NonPublic | BindingFlags.Static);
			Assert.That (f.Length, Is.Not.EqualTo (0), "fields were preserved");
		}
#endif
	}
}
