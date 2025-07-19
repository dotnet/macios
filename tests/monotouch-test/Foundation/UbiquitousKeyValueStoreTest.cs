//
// Unit tests for NSUbiquitousKeyValueStore
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using Foundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSUbiquitousKeyValueStoreTest {
		[Test]
		public void Indexer ()
		{
			using (var store = new NSUbiquitousKeyValueStore ()) {
				using (var key = new NSString ("key")) {
					using (var value = new NSString ("value")) {
						var expectNull = false;
#if __MACOS__ || __MACCATALYST__
						// We don't build with the required entitlement (com.apple.developer.ubiquity-kvstore-identifier)
						// for this to work on desktop.
						expectNull = true;
#elif __TVOS__ || __IOS__
						// On device we don't build with the required entitlement (com.apple.developer.ubiquity-kvstore-identifier),
						// so we get back a null value.
						expectNull = TestRuntime.IsDevice;
#endif

						store [key] = value;
						if (expectNull) {
							Assert.Null (store [key], "key 1");
						} else {
							Assert.AreEqual (value, store [key], "key 1");
						}

						store [(string) key] = value;
						if (expectNull) {
							Assert.Null (store [key], "key 2");
						} else {
							Assert.AreEqual (value, store [key], "key 2");
						}
					}
				}
			}
		}
	}
}
