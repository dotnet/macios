//
// Unit tests for CMTag
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2024 Microsoft Corp. All rights reserved.
//

using CoreMedia;
using CoreVideo;

using Xamarin.Utils;

using MonoTouchFixtures.AVFoundation;

namespace MonoTouchFixtures.CoreMedia {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMTaggedBufferGroupTests {
		[Test]
		public void GetTypeIdTest ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.That (CMTaggedBufferGroup.GetTypeId (), Is.Not.EqualTo (0), "GetTypeId");
		}

		[Test]
		public void Create_PixelBuffers ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { pixelBuffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");

				Assert.Throws<ArgumentException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection },
						new [] { pixelBuffer, pixelBuffer },
						out var _), "AE 1 Different element count");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						null,
						new [] { pixelBuffer },
						out var _), "ANE 1");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection },
						(CVPixelBuffer []) null,
						out var _), "ANE 2");
			}
		}

		static CMSampleBuffer CreateCMSampleBuffer ()
		{
			var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
			var desc = CMVideoFormatDescription.CreateForImageBuffer (pixelBuffer, out var fde);
			var sampleTiming = new CMSampleTimingInfo ();
			return CMSampleBuffer.CreateForImageBuffer (pixelBuffer, true, desc, sampleTiming, out var sbe);
		}

		[Test]
		public void Create_MediaBuffers ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var sampleBuffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { sampleBuffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");

				Assert.Throws<ArgumentException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection },
						new [] { sampleBuffer, sampleBuffer },
						out var _), "AE 1 Different element count");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						null,
						new [] { sampleBuffer },
						out var _), "ANE 1");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection },
						(CMSampleBuffer []) null,
						out var _), "ANE 2");
			}
		}

		[Test]
		public void Create_MixedBuffers ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var sampleBuffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new NativeObject [] { pixelBuffer, sampleBuffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");

				Assert.Throws<ArgumentException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection1 },
						new NativeObject [] { pixelBuffer, sampleBuffer },
						out var _), "AE 1 Different element count");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						null,
						new NativeObject [] { pixelBuffer, sampleBuffer },
						out var _), "ANE 1");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection1 },
						(NativeObject []) null,
						out var _), "ANE 2");

				Assert.Throws<ArgumentException> (() => CMTaggedBufferGroup.Create (
						new [] { tagCollection1 },
						new NativeObject [] { tagCollection2 },
						out var _), "AE 2 Wrong element type");
			}
		}

		[Test]
		public void Combine ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group1 = CMTaggedBufferGroup.Create (
					new [] { tagCollection1 },
					new [] { buffer1 },
					out var status1);

				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer2 = CreateCMSampleBuffer (); // new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group2 = CMTaggedBufferGroup.Create (
					new [] { tagCollection2 },
					new [] { buffer2 },
					out var status2);

				using var group = CMTaggedBufferGroup.Combine (out var status, group1, group2);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");
			}
		}

		[Test]
		public void GetTagCollection ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { pixelBuffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetTagCollection (0).Handle, Is.EqualTo (tagCollection.Handle), "#0");

				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetTagCollection (-1), "AOORE: -1");
				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetTagCollection (1), "AOORE: 1");
			}
		}

		[Test]
		public void GetPixelBuffer ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetPixelBuffer (0), Is.Not.Null, "#0 A");

				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetPixelBuffer (-1), "AOORE: -1");
				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetPixelBuffer (1), "AOORE: 1");
			}

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status B");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count B");
				Assert.That (group.GetPixelBuffer (0), Is.Null, "#0 B");
			}
		}

		[Test]
		public void GetSampleBuffer ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetSampleBuffer (0).Handle, Is.EqualTo (buffer.Handle), "#0 A");

				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetSampleBuffer (-1), "AOORE: -1");
				Assert.Throws<ArgumentOutOfRangeException> (() => group.GetSampleBuffer (1), "AOORE: 1");
			}

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status B");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count B");
				Assert.That (group.GetSampleBuffer (0), Is.Null, "#0 B");
			}
		}

		[Test]
		public void GetMixedBuffers ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var sampleBuffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new NativeObject [] { pixelBuffer, sampleBuffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status C");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count C");

				Assert.That (group.GetPixelBuffer (0), Is.Not.Null, "#0 C1");
				Assert.That (group.GetPixelBuffer (1), Is.Null, "#1 C1");

				Assert.That (group.GetSampleBuffer (0), Is.Null, "#0 C2");
				Assert.That (group.GetSampleBuffer (1), Is.Not.Null, "#1 C2");
			}
		}

		[Test]
		public void GetPixelBuffer_Tag ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetPixelBuffer (CMTag.MediaTypeVideo, out var index).Handle, Is.EqualTo (buffer.Handle), "Video A");
				Assert.That ((int) index, Is.EqualTo (0), "Index A");
				Assert.That (group.GetPixelBuffer (CMTag.MediaTypeAudio, out index), Is.Null, "Audio A");
			}

			{
				// buffer of wrong type
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetPixelBuffer (CMTag.MediaTypeVideo, out var index), Is.Null, "Video A");
			}

			{
				// more than one of the buffers match
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var buffer2 = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new [] { buffer1, buffer2 },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");
				Assert.That (group.GetPixelBuffer (CMTag.MediaTypeVideo, out var index), Is.Null, "Video A");
			}
		}

		[Test]
		public void GetPixelBuffer_TagCollection ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetPixelBuffer (tagCollection, out var index).Handle, Is.EqualTo (buffer.Handle), "Video A");
				Assert.That ((int) index, Is.EqualTo (0), "Index A");
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeAudio);
				Assert.That (group.GetPixelBuffer (tagCollection2, out index), Is.Null, "Audio A");
			}

			{
				// buffer of wrong type
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetPixelBuffer (tagCollection, out var index), Is.Null, "Video A");
			}

			{
				// more than one of the buffers match
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var buffer2 = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new [] { buffer1, buffer2 },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");
				Assert.That (group.GetPixelBuffer (tagCollection1, out var index), Is.Null, "Video A");
			}
		}

		[Test]
		public void GetSampleBuffer_Tag ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetSampleBuffer (CMTag.MediaTypeVideo, out var index).Handle, Is.EqualTo (buffer.Handle), "Video A");
				Assert.That ((int) index, Is.EqualTo (0), "Index A");
				Assert.That (group.GetSampleBuffer (CMTag.MediaTypeAudio, out index), Is.Null, "Audio A");
			}

			{
				// buffer of wrong type
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetSampleBuffer (CMTag.MediaTypeVideo, out var index), Is.Null, "Video A");
			}

			{
				// more than one of the buffers match
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = CreateCMSampleBuffer ();
				using var buffer2 = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new [] { buffer1, buffer2 },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");
				Assert.That (group.GetSampleBuffer (CMTag.MediaTypeVideo, out var index), Is.Null, "Video A");
			}
		}

		[Test]
		public void GetSampleBuffer_TagCollection ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				Assert.That (group.GetSampleBuffer (tagCollection, out var index), Is.Not.Null, "Video A");
				Assert.That ((int) index, Is.EqualTo (0), "Index A");
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeAudio);
				Assert.That (group.GetSampleBuffer (tagCollection2, out index), Is.Null, "Audio A");
			}

			{
				// buffer of wrong type
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status B");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count B");
				Assert.That (group.GetSampleBuffer (tagCollection, out var index), Is.Null, "Video B");
			}

			{
				// more than one of the buffers match
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = CreateCMSampleBuffer ();
				using var buffer2 = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new [] { buffer1, buffer2 },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status C");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count C");
				Assert.That (group.GetSampleBuffer (tagCollection1, out var index), Is.Null, "Video C");
			}
		}

		[Test]
		public void GetNumberOfMatches ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				// more than one of the buffers match
				using var tagCollection1 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var tagCollection2 = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer1 = CreateCMSampleBuffer ();
				using var buffer2 = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection1, tagCollection2 },
					new [] { buffer1, buffer2 },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (2), "Count A");
				Assert.That ((int) group.GetNumberOfMatches (tagCollection1), Is.EqualTo (2), "Matches 1 A");
				using var tagCollection3 = CMTagCollection.Create (CMTag.MediaTypeAudio);
				Assert.That ((int) group.GetNumberOfMatches (tagCollection3), Is.EqualTo (0), "Matches 2 A");
			}
		}

		[Test]
		public void CreateFormatDescription ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				using var desc = group.CreateFormatDescription (out status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status Desc A");
				Assert.That (desc.MediaType, Is.EqualTo (CMMediaType.TaggedBufferGroup), $"Desc.MediaType: {AVFoundationEnumTest.FourCC ((int) desc.MediaType)}");
				Assert.That (desc.TaggedBufferGroupFormatType, Is.EqualTo (CMTaggedBufferGroupFormatType.TaggedBufferGroup), "Desc.TaggedBufferGroupFormatType");
			}
		}

		[Test]
		public void CreateFormatDescriptionWithExtensions ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				using var desc = group.CreateFormatDescription (null, out status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status Desc A");
				Assert.That (desc.MediaType, Is.EqualTo (CMMediaType.TaggedBufferGroup), $"Desc.MediaType: {AVFoundationEnumTest.FourCC ((int) desc.MediaType)}");
				Assert.That (desc.TaggedBufferGroupFormatType, Is.EqualTo (CMTaggedBufferGroupFormatType.TaggedBufferGroup), "Desc.TaggedBufferGroupFormatType A");
			}

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status B");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count B");
				using var extensions = new NSDictionary ();
				using var desc = group.CreateFormatDescription (extensions, out status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status Desc B");
				Assert.That (desc.MediaType, Is.EqualTo (CMMediaType.TaggedBufferGroup), $"Desc.MediaType: {AVFoundationEnumTest.FourCC ((int) desc.MediaType)}");
				Assert.That (desc.TaggedBufferGroupFormatType, Is.EqualTo (CMTaggedBufferGroupFormatType.TaggedBufferGroup), "Desc.TaggedBufferGroupFormatType B");
			}
		}

		[Test]
		public void Matches ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				using var desc = group.CreateFormatDescription (out status);
				Assert.That (group.Matches (desc), Is.True, "Matches A");
				using var desc2 = CMFormatDescription.Create (CMMediaType.ClosedCaption, (uint) CMClosedCaptionFormatType.CEA608, out var fde);
				Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "FDE");
				Assert.That (group.Matches (desc2), Is.False, "Matches B");
			}
		}

		[Test]
		public void CreateSampleBuffer ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				using var formatDescription = CMFormatDescription.Create (CMMediaType.TaggedBufferGroup, (uint) CMTaggedBufferGroupFormatType.TaggedBufferGroup, out var fde);
				Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "FDE A");
				using var sampleBuffer = group.CreateSampleBuffer (CMTime.Zero, CMTime.FromSeconds (1, 1), formatDescription, out status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status CSB A");
			}
		}

		[Test]
		public void GetTaggedBufferGroup ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			{
				using var tagCollection = CMTagCollection.Create (CMTag.MediaTypeVideo);
				using var buffer = CreateCMSampleBuffer ();
				using var group = CMTaggedBufferGroup.Create (
					new [] { tagCollection },
					new [] { buffer },
					out var status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status A");
				Assert.That ((int) group.Count, Is.EqualTo (1), "Count A");
				using var formatDescription = CMFormatDescription.Create (CMMediaType.TaggedBufferGroup, (uint) CMTaggedBufferGroupFormatType.TaggedBufferGroup, out var fde);
				Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "FDE A");
				using var sampleBuffer = group.CreateSampleBuffer (CMTime.Zero, CMTime.FromSeconds (1, 1), formatDescription, out status);
				Assert.That (status, Is.EqualTo (CMTaggedBufferGroupError.Success), "Status CSB A");

				Assert.That (sampleBuffer.TaggedBufferGroup, Is.Not.Null, "CMSampleBuffer.GetTaggedBufferGroup A");
				Assert.That (CMTaggedBufferGroup.GetTaggedBufferGroup (sampleBuffer), Is.Not.Null, "CMTaggedBufferGroup.GetTaggedBufferGroup A");

				Assert.That (buffer.TaggedBufferGroup, Is.Null, "CMSampleBuffer.GetTaggedBufferGroup B");
				Assert.That (CMTaggedBufferGroup.GetTaggedBufferGroup (buffer), Is.Null, "CMTaggedBufferGroup.GetTaggedBufferGroup B");

				Assert.Throws<ArgumentNullException> (() => CMTaggedBufferGroup.GetTaggedBufferGroup (null), "ANE");
			}
		}
	}
}
