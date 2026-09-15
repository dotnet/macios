//
// Unit tests for Protocol
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2018 Microsoft Inc. All rights reserved.
//

using System.Reflection;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ProtocolTest {
		[Test]
		public void Ctors ()
		{
			var data = new [] {
				new { Type = typeof (INSObjectProtocol), Name = "NSObject" }, // protocol name doesn't match at all
				new { Type = typeof (INSUrlSessionDownloadDelegate), Name = "NSURLSessionDownloadDelegate" }, // different casing in native
				new { Type = typeof (global::CloudKit.ICKRecordValue), Name = "CKRecordValue" }, // protocol name is the same in native and managed
			};

			foreach (var d in data) {
				Assert.That (new Protocol (d.Type).Handle, Is.Not.EqualTo (IntPtr.Zero), $"{d.Name} type");
				Assert.That (new Protocol (d.Name).Handle, Is.Not.EqualTo (IntPtr.Zero), $"{d.Name} string");
				Assert.That (new Protocol (d.Name).Name, Is.EqualTo (d.Name), $"{d.Name} name");
				Assert.That (new Protocol (d.Type).Name, Is.EqualTo (d.Name), $"{d.Name} type name");
				Assert.That (new Protocol (new Protocol (d.Name).Handle).Name, Is.EqualTo (d.Name), $"{d.Name} IntPtr name");
				Assert.That (new Protocol (Protocol.GetHandle (d.Name)).Name, Is.EqualTo (d.Name), $"{d.Name} GetHandle name");
			}
		}
	}
}
