//
// Unit tests for GKSession
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using System.IO;
using System.Threading;
using GameKit;

namespace MonoTouchFixtures.GameKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SessionTest {

		[Test]
		public void NullAllowed ()
		{
			using (var session = new GKSession (null, "displayName", GKSessionMode.Client)) {
			}

			using (var session = new GKSession ("sessionID", null, GKSessionMode.Peer)) {
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
