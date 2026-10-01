#nullable enable

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Photos;

namespace MonoTouchFixtures.Photos {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CloudIdentifierTest {
		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (16, 2);

			using var identifier = PHCloudIdentifier.Create ("test");
			Assert.That (identifier, Is.Null);
		}
	}
}
