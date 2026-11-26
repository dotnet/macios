// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.Versioning;

using Cinematic;
using CoreMedia;

namespace MonoTouchFixtures.Cinematic {
	[TestFixture]
	[Preserve (AllMembers = true)]
#if __MACCATALYST__
	[SupportedOSPlatform ("maccatalyst26.0")]
#endif
	public class CNDecisionTest {
		[TestCase (CNDecisionIdentifierType.Single, false)]
		[TestCase (CNDecisionIdentifierType.Group, true)]
		public void Constructor (CNDecisionIdentifierType identifierType, bool isGroup)
		{
#if __MACCATALYST__
			TestRuntime.AssertXcodeVersion (26, 0);
#else
			TestRuntime.AssertXcodeVersion (15, 0);
#endif
			TestRuntime.AssertNotSimulator ("Cinematic is not available in the simulator.");
			var time = new CMTime (10, 30);
			const long identifier = 42;
			using var decision = new CNDecision (time, identifier, true, identifierType);

			Assert.That (decision.Time, Is.EqualTo (time), "Time");
			Assert.That (decision.StrongDecision, Is.True, "StrongDecision");
			Assert.That (decision.GroupDecision, Is.EqualTo (isGroup), "GroupDecision");
			Assert.That (isGroup ? decision.DetectionGroupId : decision.DetectionId, Is.EqualTo (identifier), "Identifier");
		}
	}
}
