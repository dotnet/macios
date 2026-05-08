// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;
using CoreGraphics;
using NUnit.Framework;
using ObjCRuntime;

namespace MonoTouchFixtures.CoreGraphics {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGContentToneMappingInfoTest {
		[Test]
		public void DefaultValuesTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var t = new CGContentToneMappingInfo ();
			Assert.That (t.Method, Is.EqualTo (CGToneMapping.Default), "Method");
			Assert.That (t.Options, Is.Null, "Options");
			Assert.That (t.ToneMappingOptions, Is.Null, "ToneMappingOptions");
		}

		[Test]
		public void PropertySetGetTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var t = new CGContentToneMappingInfo ();
			t.Method = CGToneMapping.ImageSpecificLumaScaling;
			Assert.That (t.Method, Is.EqualTo (CGToneMapping.ImageSpecificLumaScaling), "Method#1");
			t.Method = CGToneMapping.Default;
			Assert.That (t.Method, Is.EqualTo (CGToneMapping.Default), "Method#2");

			using var dict = new NSDictionary ();
			t.Options = dict;
			Assert.That (t.Options, Is.SameAs (dict), "Options#1");
			var toneMappingOptions = t.ToneMappingOptions!;
			Assert.That (toneMappingOptions.Dictionary, Is.SameAs (dict), "ToneMappingOptions#1");

			Assert.That (toneMappingOptions.Use100nitsHlgOotf.HasValue, Is.False, "ToneMappingOptions.Use100nitsHlgOotf #1");
			Assert.That (toneMappingOptions.UseBT1886ForCoreVideoGamma.HasValue, Is.False, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #1");
			Assert.That (toneMappingOptions.SkipBoostToHdr.HasValue, Is.False, "ToneMappingOptions.SkipBoostToHdr #1");
			Assert.That (toneMappingOptions.ExrToneMappingGammaDefog.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaDefog #1");
			Assert.That (toneMappingOptions.ExrToneMappingGammaExposure.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaExposure #1");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeLow.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaKneeLow #1");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #1");

			using var mutableDict = new NSMutableDictionary ();
			t.Options = mutableDict;
			Assert.That (t.Options, Is.SameAs (mutableDict), "Options#2");
			toneMappingOptions = t.ToneMappingOptions!;
			Assert.That (toneMappingOptions.Dictionary, Is.SameAs (mutableDict), "ToneMappingOptions#2");

			Assert.That (toneMappingOptions.Use100nitsHlgOotf.HasValue, Is.False, "ToneMappingOptions.Use100nitsHlgOotf #2");
			Assert.That (toneMappingOptions.UseBT1886ForCoreVideoGamma.HasValue, Is.False, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #2");
			Assert.That (toneMappingOptions.SkipBoostToHdr.HasValue, Is.False, "ToneMappingOptions.SkipBoostToHdr #2");
			Assert.That (toneMappingOptions.ExrToneMappingGammaDefog.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaDefog #2");
			Assert.That (toneMappingOptions.ExrToneMappingGammaExposure.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaExposure #2");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeLow.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaKneeLow #2");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #2");

			toneMappingOptions.Use100nitsHlgOotf = false;
			toneMappingOptions.UseBT1886ForCoreVideoGamma = true;
			toneMappingOptions.SkipBoostToHdr = null;
			toneMappingOptions.ExrToneMappingGammaDefog = 1.0f;
			toneMappingOptions.ExrToneMappingGammaExposure = -1.0f;
			toneMappingOptions.ExrToneMappingGammaKneeLow = 0.0f;
			toneMappingOptions.ExrToneMappingGammaKneeHigh = null;

			Assert.That (toneMappingOptions.Use100nitsHlgOotf.Value, Is.False, "ToneMappingOptions.Use100nitsHlgOotf #3");
			Assert.That (toneMappingOptions.UseBT1886ForCoreVideoGamma.Value, Is.True, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #3");
			Assert.That (toneMappingOptions.SkipBoostToHdr.HasValue, Is.False, "ToneMappingOptions.SkipBoostToHdr #3");
			Assert.That (toneMappingOptions.ExrToneMappingGammaDefog.Value, Is.EqualTo (1.0f), "ToneMappingOptions.ExrToneMappingGammaDefog #3");
			Assert.That (toneMappingOptions.ExrToneMappingGammaExposure.Value, Is.EqualTo (-1.0f), "ToneMappingOptions.ExrToneMappingGammaExposure #3");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeLow.Value, Is.EqualTo (0.0f), "ToneMappingOptions.ExrToneMappingGammaKneeLow #3");
			Assert.That (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, Is.False, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #3");
		}
	}
}
