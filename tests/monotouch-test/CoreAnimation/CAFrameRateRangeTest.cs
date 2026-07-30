
using CoreAnimation;
namespace MonoTouchFixtures.CoreAnimation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CAFrameRateRangeTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
		}

		[Test]
		public void IsEqualToTest ()
			=> Assert.That (CAFrameRateRange.Default.IsEqualTo (CAFrameRateRange.Default), Is.True);

		[Test]
		public void DefaultTest ()
			=> Assert.That (CAFrameRateRange.Default, Is.Not.Null, "Default");
	}
}
