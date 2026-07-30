using Foundation;
using AVFoundation;
using CoreMedia;
using NUnit.Framework;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptureTimecodeTests {

		[SetUp]
		public void Setup () => TestRuntime.AssertXcodeVersion (26, 0);

		[Test]
		public void EqualityOperator_TrueForIdenticalValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t1 == t2, Is.True);
			Assert.That (t1 != t2, Is.False);
		}

		[Test]
		public void EqualityOperator_FalseForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (9, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t1 == t2, Is.False);
			Assert.That (t1 != t2, Is.True);
		}

		[Test]
		public void EqualsMethod_TrueForIdenticalValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t1.Equals (t2), Is.True);
			Assert.That (t1.Equals ((object) t2), Is.True);
		}

		[Test]
		public void EqualsMethod_FalseForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 6, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t1.Equals (t2), Is.False);
			Assert.That (t1.Equals ((object) t2), Is.False);
		}

		[Test]
		public void GetHashCode_EqualForIdenticalValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t2.GetHashCode (), Is.EqualTo (t1.GetHashCode ()));
		}

		[Test]
		public void GetHashCode_NotEqualForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 6, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.That (t2.GetHashCode (), Is.Not.EqualTo (t1.GetHashCode ()));
		}

		[Test]
		public void AddFramesTest ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = t1.AddFrames (10);
			Assert.That (t1 != t2, Is.True);
		}

		[Test]
		public void MetadataSampleBufferTest ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			using var sampleBuffer = t1.CreateMetadataSampleBufferAssociatedWithPresentationTimeStamp (new CMTime (60, 60));
			Assert.That (sampleBuffer, Is.Not.Null, "sampleBuffer");
			Assert.That (sampleBuffer.IsValid, Is.True, "IsValid");
			Assert.That (1 == sampleBuffer.NumSamples, Is.True, "NumSamples");

			using var sampleBuffer2 = t1.CreateMetadataSampleBufferForDuration (new CMTime (60, 60));
			Assert.That (sampleBuffer2, Is.Not.Null, "sampleBuffer2");
			Assert.That (sampleBuffer2.IsValid, Is.True, "IsValid");
			Assert.That (1 == sampleBuffer2.NumSamples, Is.True, "NumSamples");
		}
	}
}
