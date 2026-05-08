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
			ClassicAssert.AreEqual (CGToneMapping.Default, t.Method, "Method");
			ClassicAssert.IsNull (t.Options, "Options");
			ClassicAssert.IsNull (t.ToneMappingOptions, "ToneMappingOptions");
		}

		[Test]
		public void PropertySetGetTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var t = new CGContentToneMappingInfo ();
			t.Method = CGToneMapping.ImageSpecificLumaScaling;
			ClassicAssert.AreEqual (CGToneMapping.ImageSpecificLumaScaling, t.Method, "Method#1");
			t.Method = CGToneMapping.Default;
			ClassicAssert.AreEqual (CGToneMapping.Default, t.Method, "Method#2");

			using var dict = new NSDictionary ();
			t.Options = dict;
			ClassicAssert.AreSame (dict, t.Options, "Options#1");
			var toneMappingOptions = t.ToneMappingOptions!;
			ClassicAssert.AreSame (dict, toneMappingOptions.Dictionary, "ToneMappingOptions#1");

			ClassicAssert.IsFalse (toneMappingOptions.Use100nitsHlgOotf.HasValue, "ToneMappingOptions.Use100nitsHlgOotf #1");
			ClassicAssert.IsFalse (toneMappingOptions.UseBT1886ForCoreVideoGamma.HasValue, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #1");
			ClassicAssert.IsFalse (toneMappingOptions.SkipBoostToHdr.HasValue, "ToneMappingOptions.SkipBoostToHdr #1");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaDefog.HasValue, "ToneMappingOptions.ExrToneMappingGammaDefog #1");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaExposure.HasValue, "ToneMappingOptions.ExrToneMappingGammaExposure #1");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaKneeLow.HasValue, "ToneMappingOptions.ExrToneMappingGammaKneeLow #1");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #1");

			using var mutableDict = new NSMutableDictionary ();
			t.Options = mutableDict;
			ClassicAssert.AreSame (mutableDict, t.Options, "Options#2");
			toneMappingOptions = t.ToneMappingOptions!;
			ClassicAssert.AreSame (mutableDict, toneMappingOptions.Dictionary, "ToneMappingOptions#2");

			ClassicAssert.IsFalse (toneMappingOptions.Use100nitsHlgOotf.HasValue, "ToneMappingOptions.Use100nitsHlgOotf #2");
			ClassicAssert.IsFalse (toneMappingOptions.UseBT1886ForCoreVideoGamma.HasValue, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #2");
			ClassicAssert.IsFalse (toneMappingOptions.SkipBoostToHdr.HasValue, "ToneMappingOptions.SkipBoostToHdr #2");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaDefog.HasValue, "ToneMappingOptions.ExrToneMappingGammaDefog #2");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaExposure.HasValue, "ToneMappingOptions.ExrToneMappingGammaExposure #2");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaKneeLow.HasValue, "ToneMappingOptions.ExrToneMappingGammaKneeLow #2");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #2");

			toneMappingOptions.Use100nitsHlgOotf = false;
			toneMappingOptions.UseBT1886ForCoreVideoGamma = true;
			toneMappingOptions.SkipBoostToHdr = null;
			toneMappingOptions.ExrToneMappingGammaDefog = 1.0f;
			toneMappingOptions.ExrToneMappingGammaExposure = -1.0f;
			toneMappingOptions.ExrToneMappingGammaKneeLow = 0.0f;
			toneMappingOptions.ExrToneMappingGammaKneeHigh = null;

			ClassicAssert.IsFalse (toneMappingOptions.Use100nitsHlgOotf.Value, "ToneMappingOptions.Use100nitsHlgOotf #3");
			ClassicAssert.IsTrue (toneMappingOptions.UseBT1886ForCoreVideoGamma.Value, "ToneMappingOptions.UseBT1886ForCoreVideoGamma #3");
			ClassicAssert.IsFalse (toneMappingOptions.SkipBoostToHdr.HasValue, "ToneMappingOptions.SkipBoostToHdr #3");
			ClassicAssert.AreEqual (1.0f, toneMappingOptions.ExrToneMappingGammaDefog.Value, "ToneMappingOptions.ExrToneMappingGammaDefog #3");
			ClassicAssert.AreEqual (-1.0f, toneMappingOptions.ExrToneMappingGammaExposure.Value, "ToneMappingOptions.ExrToneMappingGammaExposure #3");
			ClassicAssert.AreEqual (0.0f, toneMappingOptions.ExrToneMappingGammaKneeLow.Value, "ToneMappingOptions.ExrToneMappingGammaKneeLow #3");
			ClassicAssert.IsFalse (toneMappingOptions.ExrToneMappingGammaKneeHigh.HasValue, "ToneMappingOptions.ExrToneMappingGammaKneeHigh #3");
		}
	}
}
