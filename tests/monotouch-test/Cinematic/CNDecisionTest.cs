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
		[TestCase (CNDecisionIdentifierType.Single, true)]
		[TestCase (CNDecisionIdentifierType.Group, true)]
		[TestCase (CNDecisionIdentifierType.Single, false)]
		[TestCase (CNDecisionIdentifierType.Group, false)]
		public void Constructor (CNDecisionIdentifierType identifierType, bool isStrong)
		{
#if __MACCATALYST__
			TestRuntime.AssertXcodeVersion (26, 0);
#else
			TestRuntime.AssertXcodeVersion (15, 0);
#endif
			TestRuntime.AssertNotSimulator ("Cinematic is not available in the simulator.");
			var time = new CMTime (10, 30);
			const long identifier = 42;
			using var decision = new CNDecision (time, identifier, isStrong, identifierType);

			Assert.That (decision.Time, Is.EqualTo (time), "Time");
			Assert.That (decision.StrongDecision, Is.EqualTo (isStrong), "StrongDecision");
			Assert.That (decision.GroupDecision, Is.EqualTo (identifierType == CNDecisionIdentifierType.Group), "GroupDecision");
			var actualIdentifier = identifierType == CNDecisionIdentifierType.Group ? decision.DetectionGroupId : decision.DetectionId;
			Assert.That (actualIdentifier, Is.EqualTo (identifier), "Identifier");
		}

		[Test]
		public void Constructor_InvalidIdentifierType ()
		{
#if __MACCATALYST__
			TestRuntime.AssertXcodeVersion (26, 0);
#else
			TestRuntime.AssertXcodeVersion (15, 0);
#endif

			Assert.Throws<ArgumentOutOfRangeException> (() => new CNDecision (new CMTime (10, 30), 42, true, (CNDecisionIdentifierType) 42));
		}
	}
}
