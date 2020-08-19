﻿// Unit test for AVAudioFormat
// Authors: 
// 		Whitney Schmidt (whschm@microsoft.com)
// Copyright 2020 Microsoft Corp.

using System;
using Foundation;
using AVFoundation;
using NUnit.Framework;
namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAudioFormatTest {

		[Test]
		public void TestEqualOperatorSameInstace ()
		{
			using (var format = new AVAudioFormat ())
				Assert.IsTrue (format == format, "format == format");
		}

		[Test]
		public void TestEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ())
			{
				Assert.IsFalse (format == null, "format == null");
				Assert.IsFalse (null == format, "null == format");
			}
			using (AVAudioFormat nullFormat = null)
			{
				Assert.IsTrue (nullFormat == null, "nullFormat == null");
				Assert.IsTrue (null == nullFormat, "null == nullFormat");
			}
		}

		[Test]
		public void TestNotEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ())
			{
				Assert.IsTrue (format != null, "format != null");
				Assert.IsTrue (null != format, "null != format");
			}
			using (AVAudioFormat nullFormat = null)
			{
				Assert.IsFalse (nullFormat != null, "nullFormat != null");
				Assert.IsFalse (null != nullFormat, "null != nullFormat");
			}

		}
	}
}
