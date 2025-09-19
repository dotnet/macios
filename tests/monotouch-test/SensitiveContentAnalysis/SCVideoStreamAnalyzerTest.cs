#if HAS_SENSITIVECONTENTANALYSIS

using System;

using Foundation;
using SensitiveContentAnalysis;

using NUnit.Framework;


namespace MonoTouchFixtures.SensitiveContentAnalysis;

#if __IOS__ && !__MACCATALYST__

[TestFixture]
[Preserve (AllMembers = true)]
public class SCVideoStreamAnalyzerTests {
	[Test]
	public void Create ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);

		using var obj = SCVideoStreamAnalyzer.Create ("placeholder", SCVideoStreamAnalyzerStreamDirection.Outgoing, out var error);
		// There are a number of other reasons for this to fail:
		// * We don't have a valid participant uuid.
		// * A specific entitlement is required (com.apple.developer.sensitivecontentanalysis.client)
		// * Only works when "the Communication Safety parental control in Screen Time is enabled, or when the Sensitive Content Warnings setting is turned on".
		Assert.That (obj, Is.Null, "Null");
		Assert.That (error, Is.Not.Null, "Error");
	}
}

#endif // __IOS__ && !__MACCATALYST__

#endif // HAS_SENSITIVECONTENTANALYSIS
