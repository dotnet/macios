﻿//
// Unit tests for NSTimer
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using System.Reflection;
#if XAMCORE_2_0
using Foundation;
using ObjCRuntime;
#else
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#endif
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSUbiquitousKeyValueStoreTest {
#if !__WATCHOS__
		// Looks like NSUbiquitousKeyValueStore doesn't work on watchOS:
		// http://stackoverflow.com/questions/37412775/nsubiquitouskeyvaluestore-is-unavailable-watchos-2
		// https://forums.developer.apple.com/thread/47564
		[Test]
		public void Indexer ()
		{
			Assert.Ignore ("Doesn't work on watchOS");

			using (var store = new NSUbiquitousKeyValueStore ()) {
				using (var key = new NSString ("key")) {
					using (var value = new NSString ("value")) {
						store [key] = value;
						Assert.AreEqual (value, store [key], "key 1");

						store [(string) key] = value;
						Assert.AreEqual (value, store [(string) key], "key 2");
					}

				}
			}
		}
#endif
	}
}
