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
			Assert.True (t1 == t2);
			Assert.False (t1 != t2);
		}

		[Test]
		public void EqualityOperator_FalseForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (9, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.False (t1 == t2);
			Assert.True (t1 != t2);
		}

		[Test]
		public void EqualsMethod_TrueForIdenticalValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.True (t1.Equals (t2));
			Assert.True (t1.Equals ((object) t2));
		}

		[Test]
		public void EqualsMethod_FalseForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 6, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.False (t1.Equals (t2));
			Assert.False (t1.Equals ((object) t2));
		}

		[Test]
		public void GetHashCode_EqualForIdenticalValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.AreEqual (t1.GetHashCode (), t2.GetHashCode ());
		}

		[Test]
		public void GetHashCode_NotEqualForDifferentValues ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = new AVCaptureTimecode (1, 2, 3, 4, 6, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			Assert.AreNotEqual (t1.GetHashCode (), t2.GetHashCode ());
		}

		[Test]
		public void AddFramesTest ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			var t2 = t1.AddFrames (10);
			Assert.True (t1 != t2);
		}

		[Test]
		public void MetadataSampleBufferTest ()
		{
			var t1 = new AVCaptureTimecode (1, 2, 3, 4, 5, new CMTime (60, 30), AVCaptureTimecodeSourceType.FrameCount);
			using var sampleBuffer = t1.CreateMetadataSampleBufferAssociatedWithPresentationTimeStamp (new CMTime (60, 60));
			Assert.IsNotNull (sampleBuffer, "sampleBuffer");
			Assert.IsTrue (sampleBuffer.IsValid, "IsValid");
			Assert.IsTrue (1 == sampleBuffer.NumSamples, "NumSamples");

			using var sampleBuffer2 = t1.CreateMetadataSampleBufferForDuration (new CMTime (60, 60));
			Assert.IsNotNull (sampleBuffer2, "sampleBuffer2");
			Assert.IsTrue (sampleBuffer2.IsValid, "IsValid");
			Assert.IsTrue (1 == sampleBuffer2.NumSamples, "NumSamples");
		}
	}
}
