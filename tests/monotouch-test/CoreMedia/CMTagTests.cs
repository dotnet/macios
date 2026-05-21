//
// Unit tests for CMTag
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

using CoreMedia;

using Xamarin.Utils;

using MonoTouchFixtures.AVFoundation;

namespace MonoTouchFixtures.CoreMedia {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMTagTests {

		public static string FourCC (int value)
		{
			return AVFoundationEnumTest.FourCC (value);
		}

		public static string FourCC (uint value)
		{
			return AVFoundationEnumTest.FourCC (value);
		}

		public static uint FourCC (string value)
		{
			return AVFoundationEnumTest.FourCC (value);
		}

		[Test]
		public void Default ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var tag = default (CMTag);
			AssertTag (tag, CMTagCategory.Undefined, CMTagDataType.Invalid, 0, false, false, 0, false, 0, false, 0, false, 0, "Default");
		}

		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				AssertTag (
					CMTag.CreateWithSInt64Value (CMTagCategory.MediaType, 314),
					CMTagCategory.MediaType, CMTagDataType.SInt64, 314, true, false, 0, false, 0, false, 0, true, 314, "CreateWithSInt64Value");

				AssertTag (
					CMTag.CreateWithFloat64Value (CMTagCategory.MediaType, 3.14),
					CMTagCategory.MediaType, CMTagDataType.Float64, 4614253070214989087, true, true, 3.14, false, 0, false, 0, false, 0, "CreateWithFloat64Value");

				AssertTag (
					CMTag.CreateWithOSTypeValue (CMTagCategory.MediaType, 314),
					CMTagCategory.MediaType, CMTagDataType.OSType, 314, true, false, 0, true, 314, false, 0, false, 0, "CreateWithOSTypeValue");

				AssertTag (
					CMTag.CreateWithFlagsValue (CMTagCategory.MediaType, 314),
					CMTagCategory.MediaType, CMTagDataType.Flags, 314, true, false, 0, false, 0, true, 314, false, 0, "CreateWithFlagsValue");
			});
		}

		[Test]
		public void Equals ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				Assert.That (CMTag.Equals (default (CMTag), default (CMTag)), Is.EqualTo (true), "Default");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.Invalid), Is.EqualTo (true), "Invalid");
				Assert.That (CMTag.Equals (CMTag.MediaTypeVideo, CMTag.MediaTypeVideo), Is.EqualTo (true), "MediaTypeVideo");
				Assert.That (CMTag.Equals (CMTag.MediaSubTypeMebx, CMTag.MediaSubTypeMebx), Is.EqualTo (true), "MediaSubTypeMebx");
				Assert.That (CMTag.Equals (CMTag.MediaTypeAudio, CMTag.MediaTypeAudio), Is.EqualTo (true), "MediaTypeAudio");
				Assert.That (CMTag.Equals (CMTag.MediaTypeMetadata, CMTag.MediaTypeMetadata), Is.EqualTo (true), "MediaTypeMetadata");
				Assert.That (CMTag.Equals (CMTag.StereoLeftEye, CMTag.StereoLeftEye), Is.EqualTo (true), "StereoLeftEye");
				Assert.That (CMTag.Equals (CMTag.StereoRightEye, CMTag.StereoRightEye), Is.EqualTo (true), "StereoRightEye");
				Assert.That (CMTag.Equals (CMTag.StereoLeftAndRightEye, CMTag.StereoLeftAndRightEye), Is.EqualTo (true), "StereoLeftAndRightEye");
				Assert.That (CMTag.Equals (CMTag.StereoNone, CMTag.StereoNone), Is.EqualTo (true), "StereoNone");
				Assert.That (CMTag.Equals (CMTag.StereoInterpretationOrderReversed, CMTag.StereoInterpretationOrderReversed), Is.EqualTo (true), "StereoInterpretationOrderReversed");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeRectangular, CMTag.ProjectionTypeRectangular), Is.EqualTo (true), "ProjectionTypeRectangular");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeEquirectangular, CMTag.ProjectionTypeEquirectangular), Is.EqualTo (true), "ProjectionTypeEquirectangular");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeHalfEquirectangular, CMTag.ProjectionTypeHalfEquirectangular), Is.EqualTo (true), "ProjectionTypeHalfEquirectangular");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeFisheye, CMTag.ProjectionTypeFisheye), Is.EqualTo (true), "ProjectionTypeFisheye");
				Assert.That (CMTag.Equals (CMTag.PackingTypeNone, CMTag.PackingTypeNone), Is.EqualTo (true), "PackingTypeNone");
				Assert.That (CMTag.Equals (CMTag.PackingTypeSideBySide, CMTag.PackingTypeSideBySide), Is.EqualTo (true), "PackingTypeSideBySide");
				Assert.That (CMTag.Equals (CMTag.PackingTypeOverUnder, CMTag.PackingTypeOverUnder), Is.EqualTo (true), "PackingTypeOverUnder");

				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.MediaTypeVideo), Is.EqualTo (false), "Invalid vs MediaTypeVideo");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.MediaSubTypeMebx), Is.EqualTo (false), "Invalid vs MediaSubTypeMebx");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.MediaTypeAudio), Is.EqualTo (false), "Invalid vs MediaTypeAudio");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.MediaTypeMetadata), Is.EqualTo (false), "Invalid vs MediaTypeMetadata");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.StereoLeftEye), Is.EqualTo (false), "Invalid vs StereoLeftEye");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.StereoRightEye), Is.EqualTo (false), "Invalid vs StereoRightEye");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.StereoLeftAndRightEye), Is.EqualTo (false), "Invalid vs StereoLeftAndRightEye");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.StereoNone), Is.EqualTo (false), "Invalid vs StereoNone");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.StereoInterpretationOrderReversed), Is.EqualTo (false), "Invalid vs StereoInterpretationOrderReversed");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.ProjectionTypeRectangular), Is.EqualTo (false), "Invalid vs ProjectionTypeRectangular");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.ProjectionTypeEquirectangular), Is.EqualTo (false), "Invalid vs ProjectionTypeEquirectangular");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.ProjectionTypeHalfEquirectangular), Is.EqualTo (!TestRuntime.CheckXcodeVersion (16, 0)), "Invalid vs ProjectionTypeHalfEquirectangular");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.ProjectionTypeFisheye), Is.EqualTo (false), "Invalid vs ProjectionTypeFisheye");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.PackingTypeNone), Is.EqualTo (false), "Invalid vs PackingTypeNone");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.PackingTypeSideBySide), Is.EqualTo (false), "Invalid vs PackingTypeSideBySide");
				Assert.That (CMTag.Equals (CMTag.Invalid, CMTag.PackingTypeOverUnder), Is.EqualTo (false), "Invalid vs PackingTypeOverUnder");

				Assert.That (CMTag.Equals (CMTag.MediaTypeVideo, CMTag.Invalid), Is.EqualTo (false), "MediaTypeVideo vs Invalid");
				Assert.That (CMTag.Equals (CMTag.MediaSubTypeMebx, CMTag.Invalid), Is.EqualTo (false), "MediaSubTypeMebx vs Invalid");
				Assert.That (CMTag.Equals (CMTag.MediaTypeAudio, CMTag.Invalid), Is.EqualTo (false), "MediaTypeAudio vs Invalid");
				Assert.That (CMTag.Equals (CMTag.MediaTypeMetadata, CMTag.Invalid), Is.EqualTo (false), "MediaTypeMetadata vs Invalid");
				Assert.That (CMTag.Equals (CMTag.StereoLeftEye, CMTag.Invalid), Is.EqualTo (false), "StereoLeftEye vs Invalid");
				Assert.That (CMTag.Equals (CMTag.StereoRightEye, CMTag.Invalid), Is.EqualTo (false), "StereoRightEye vs Invalid");
				Assert.That (CMTag.Equals (CMTag.StereoLeftAndRightEye, CMTag.Invalid), Is.EqualTo (false), "StereoLeftAndRightEye vs Invalid");
				Assert.That (CMTag.Equals (CMTag.StereoNone, CMTag.Invalid), Is.EqualTo (false), "StereoNone vs Invalid");
				Assert.That (CMTag.Equals (CMTag.StereoInterpretationOrderReversed, CMTag.Invalid), Is.EqualTo (false), "StereoInterpretationOrderReversed vs Invalid");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeRectangular, CMTag.Invalid), Is.EqualTo (false), "ProjectionTypeRectangular vs Invalid");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeEquirectangular, CMTag.Invalid), Is.EqualTo (false), "ProjectionTypeEquirectangular vs Invalid");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeHalfEquirectangular, CMTag.Invalid), Is.EqualTo (!TestRuntime.CheckXcodeVersion (16, 0)), "ProjectionTypeHalfEquirectangular vs Invalid");
				Assert.That (CMTag.Equals (CMTag.ProjectionTypeFisheye, CMTag.Invalid), Is.EqualTo (false), "ProjectionTypeFisheye vs Invalid");
				Assert.That (CMTag.Equals (CMTag.PackingTypeNone, CMTag.Invalid), Is.EqualTo (false), "PackingTypeNone vs Invalid");
				Assert.That (CMTag.Equals (CMTag.PackingTypeSideBySide, CMTag.Invalid), Is.EqualTo (false), "PackingTypeSideBySide vs Invalid");
				Assert.That (CMTag.Equals (CMTag.PackingTypeOverUnder, CMTag.Invalid), Is.EqualTo (false), "PackingTypeOverUnder vs Invalid");
			});
		}

		[Test]
		public void Compare ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				Assert.That (CMTag.Compare (default (CMTag), default (CMTag)), Is.EqualTo (CFComparisonResult.EqualTo), "Default");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.Invalid), Is.EqualTo (CFComparisonResult.EqualTo), "Invalid");
				Assert.That (CMTag.Compare (CMTag.MediaTypeVideo, CMTag.MediaTypeVideo), Is.EqualTo (CFComparisonResult.EqualTo), "MediaTypeVideo");
				Assert.That (CMTag.Compare (CMTag.MediaSubTypeMebx, CMTag.MediaSubTypeMebx), Is.EqualTo (CFComparisonResult.EqualTo), "MediaSubTypeMebx");
				Assert.That (CMTag.Compare (CMTag.MediaTypeAudio, CMTag.MediaTypeAudio), Is.EqualTo (CFComparisonResult.EqualTo), "MediaTypeAudio");
				Assert.That (CMTag.Compare (CMTag.MediaTypeMetadata, CMTag.MediaTypeMetadata), Is.EqualTo (CFComparisonResult.EqualTo), "MediaTypeMetadata");
				Assert.That (CMTag.Compare (CMTag.StereoLeftEye, CMTag.StereoLeftEye), Is.EqualTo (CFComparisonResult.EqualTo), "StereoLeftEye");
				Assert.That (CMTag.Compare (CMTag.StereoRightEye, CMTag.StereoRightEye), Is.EqualTo (CFComparisonResult.EqualTo), "StereoRightEye");
				Assert.That (CMTag.Compare (CMTag.StereoLeftAndRightEye, CMTag.StereoLeftAndRightEye), Is.EqualTo (CFComparisonResult.EqualTo), "StereoLeftAndRightEye");
				Assert.That (CMTag.Compare (CMTag.StereoNone, CMTag.StereoNone), Is.EqualTo (CFComparisonResult.EqualTo), "StereoNone");
				Assert.That (CMTag.Compare (CMTag.StereoInterpretationOrderReversed, CMTag.StereoInterpretationOrderReversed), Is.EqualTo (CFComparisonResult.EqualTo), "StereoInterpretationOrderReversed");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeRectangular, CMTag.ProjectionTypeRectangular), Is.EqualTo (CFComparisonResult.EqualTo), "ProjectionTypeRectangular");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeEquirectangular, CMTag.ProjectionTypeEquirectangular), Is.EqualTo (CFComparisonResult.EqualTo), "ProjectionTypeEquirectangular");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeHalfEquirectangular, CMTag.ProjectionTypeHalfEquirectangular), Is.EqualTo (CFComparisonResult.EqualTo), "ProjectionTypeHalfEquirectangular");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeFisheye, CMTag.ProjectionTypeFisheye), Is.EqualTo (CFComparisonResult.EqualTo), "ProjectionTypeFisheye");
				Assert.That (CMTag.Compare (CMTag.PackingTypeNone, CMTag.PackingTypeNone), Is.EqualTo (CFComparisonResult.EqualTo), "PackingTypeNone");
				Assert.That (CMTag.Compare (CMTag.PackingTypeSideBySide, CMTag.PackingTypeSideBySide), Is.EqualTo (CFComparisonResult.EqualTo), "PackingTypeSideBySide");
				Assert.That (CMTag.Compare (CMTag.PackingTypeOverUnder, CMTag.PackingTypeOverUnder), Is.EqualTo (CFComparisonResult.EqualTo), "PackingTypeOverUnder");

				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.MediaTypeVideo), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs MediaTypeVideo");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.MediaSubTypeMebx), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs MediaSubTypeMebx");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.MediaTypeAudio), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs MediaTypeAudio");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.MediaTypeMetadata), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs MediaTypeMetadata");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.StereoLeftEye), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs StereoLeftEye");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.StereoRightEye), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs StereoRightEye");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.StereoLeftAndRightEye), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs StereoLeftAndRightEye");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.StereoNone), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs StereoNone");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.StereoInterpretationOrderReversed), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs StereoInterpretationOrderReversed");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.ProjectionTypeRectangular), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs ProjectionTypeRectangular");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.ProjectionTypeEquirectangular), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs ProjectionTypeEquirectangular");
				if (TestRuntime.CheckXcodeVersion (16, 0)) {
					Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.ProjectionTypeHalfEquirectangular), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs ProjectionTypeHalfEquirectangular");
				} else {
					Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.ProjectionTypeHalfEquirectangular), Is.EqualTo (CFComparisonResult.EqualTo), "Invalid vs ProjectionTypeHalfEquirectangular");
				}
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.ProjectionTypeFisheye), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs ProjectionTypeFisheye");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.PackingTypeNone), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs PackingTypeNone");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.PackingTypeSideBySide), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs PackingTypeSideBySide");
				Assert.That (CMTag.Compare (CMTag.Invalid, CMTag.PackingTypeOverUnder), Is.EqualTo (CFComparisonResult.LessThan), "Invalid vs PackingTypeOverUnder");

				Assert.That (CMTag.Compare (CMTag.MediaTypeVideo, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "MediaTypeVideo vs Invalid");
				Assert.That (CMTag.Compare (CMTag.MediaSubTypeMebx, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "MediaSubTypeMebx vs Invalid");
				Assert.That (CMTag.Compare (CMTag.MediaTypeAudio, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "MediaTypeAudio vs Invalid");
				Assert.That (CMTag.Compare (CMTag.MediaTypeMetadata, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "MediaTypeMetadata vs Invalid");
				Assert.That (CMTag.Compare (CMTag.StereoLeftEye, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "StereoLeftEye vs Invalid");
				Assert.That (CMTag.Compare (CMTag.StereoRightEye, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "StereoRightEye vs Invalid");
				Assert.That (CMTag.Compare (CMTag.StereoLeftAndRightEye, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "StereoLeftAndRightEye vs Invalid");
				Assert.That (CMTag.Compare (CMTag.StereoNone, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "StereoNone vs Invalid");
				Assert.That (CMTag.Compare (CMTag.StereoInterpretationOrderReversed, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "StereoInterpretationOrderReversed vs Invalid");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeRectangular, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "ProjectionTypeRectangular vs Invalid");
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeEquirectangular, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "ProjectionTypeEquirectangular vs Invalid");
				if (TestRuntime.CheckXcodeVersion (16, 0)) {
					Assert.That (CMTag.Compare (CMTag.ProjectionTypeHalfEquirectangular, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "ProjectionTypeHalfEquirectangular vs Invalid");
				} else {
					Assert.That (CMTag.Compare (CMTag.ProjectionTypeHalfEquirectangular, CMTag.Invalid), Is.EqualTo (CFComparisonResult.EqualTo), "ProjectionTypeHalfEquirectangular vs Invalid");
				}
				Assert.That (CMTag.Compare (CMTag.ProjectionTypeFisheye, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "ProjectionTypeFisheye vs Invalid");
				Assert.That (CMTag.Compare (CMTag.PackingTypeNone, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "PackingTypeNone vs Invalid");
				Assert.That (CMTag.Compare (CMTag.PackingTypeSideBySide, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "PackingTypeSideBySide vs Invalid");
				Assert.That (CMTag.Compare (CMTag.PackingTypeOverUnder, CMTag.Invalid), Is.EqualTo (CFComparisonResult.GreaterThan), "PackingTypeOverUnder vs Invalid");
			});
		}

		void AssertTag (CMTag tag, CMTagCategory category, CMTagDataType dataType, ulong value, bool isValid, bool hasFloat64Value, double float64Value, bool hasOSTypeValue, uint osTypeValue, bool hasFlagsValue, ulong flagsValue, bool hasInt64Value, long int64Value, string message)
		{
			Assert.That (tag.Category, Is.EqualTo (category), $"{message}: Category");
			Assert.That (tag.DataType, Is.EqualTo (dataType), $"{message}: DataType");
			Assert.That (tag.Value, Is.EqualTo (value), $"{message}: Value");
			Assert.That (tag.IsValid, Is.EqualTo (isValid), $"{message}: IsValid");
			Assert.That (tag.HasFloat64Value, Is.EqualTo (hasFloat64Value), $"{message}: HasFloat64Value");
			if (hasFloat64Value)
				Assert.That (tag.Float64Value, Is.EqualTo (float64Value), $"{message}: Float64Value");
			Assert.That (tag.HasOSTypeValue, Is.EqualTo (hasOSTypeValue), $"{message}: HasOSTypeValue");
			if (hasOSTypeValue)
				Assert.That (tag.OSTypeValue, Is.EqualTo (osTypeValue), $"{message}: OSTypeValue ({AVFoundationEnumTest.FourCC (osTypeValue)}={osTypeValue} vs {AVFoundationEnumTest.FourCC (tag.OSTypeValue)}={tag.OSTypeValue})");
			Assert.That (tag.HasFlagsValue, Is.EqualTo (hasFlagsValue), $"{message}: HasFlagsValue");
			if (hasFlagsValue)
				Assert.That (tag.FlagsValue, Is.EqualTo (flagsValue), $"{message}: FlagsValue");
			Assert.That (tag.HasInt64Value, Is.EqualTo (hasInt64Value), $"{message}: HasInt64Value");
			if (hasInt64Value)
				Assert.That (tag.Int64Value, Is.EqualTo (int64Value), $"{message}: Int64Value");
		}

		[Test]
		public void Fields ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				AssertTag (CMTag.Invalid, CMTagCategory.Undefined, CMTagDataType.Invalid, 0, false, false, 0, false, 0, false, 0, false, 0, "Invalid");
				AssertTag (CMTag.MediaTypeVideo, CMTagCategory.MediaType, CMTagDataType.OSType, FourCC ("vide"), true, false, 0, true, FourCC ("vide"), false, 0, false, 0, "MediaTypeVideo");
				AssertTag (CMTag.MediaSubTypeMebx, CMTagCategory.MediaSubType, CMTagDataType.OSType, FourCC ("mebx"), true, false, 0, true, FourCC ("mebx"), false, 0, false, 0, "MediaSubTypeMebx");
				AssertTag (CMTag.MediaTypeAudio, CMTagCategory.MediaType, CMTagDataType.OSType, FourCC ("soun"), true, false, 0, true, FourCC ("soun"), false, 0, false, 0, "MediaTypeAudio");
				AssertTag (CMTag.MediaTypeMetadata, CMTagCategory.MediaType, CMTagDataType.OSType, FourCC ("meta"), true, false, 0, true, FourCC ("meta"), false, 0, false, 0, "MediaTypeMetadata");
				AssertTag (CMTag.StereoLeftEye, CMTagCategory.StereoView, CMTagDataType.Flags, 1, true, false, 0, false, 0, true, 1, false, 0, "StereoLeftEye");
				AssertTag (CMTag.StereoRightEye, CMTagCategory.StereoView, CMTagDataType.Flags, 2, true, false, 0, false, 0, true, 2, false, 0, "StereoRightEye");
				AssertTag (CMTag.StereoLeftAndRightEye, CMTagCategory.StereoView, CMTagDataType.Flags, 3, true, false, 0, false, 0, true, 3, false, 0, "StereoLeftAndRightEye");
				AssertTag (CMTag.StereoNone, CMTagCategory.StereoView, CMTagDataType.Flags, 0, true, false, 0, false, 0, true, 0, false, 0, "StereoNone");
				AssertTag (CMTag.StereoInterpretationOrderReversed, CMTagCategory.StereoViewInterpretation, CMTagDataType.Flags, 1, true, false, 0, false, 0, true, 1, false, 0, "StereoInterpretationOrderReversed");
				AssertTag (CMTag.ProjectionTypeRectangular, CMTagCategory.ProjectionType, CMTagDataType.OSType, FourCC ("rect"), true, false, 0, true, FourCC ("rect"), false, 0, false, 0, "ProjectionTypeRectangular");
				AssertTag (CMTag.ProjectionTypeEquirectangular, CMTagCategory.ProjectionType, CMTagDataType.OSType, FourCC ("equi"), true, false, 0, true, FourCC ("equi"), false, 0, false, 0, "ProjectionTypeEquirectangular");
				if (TestRuntime.CheckXcodeVersion (16, 0)) {
					AssertTag (CMTag.ProjectionTypeHalfEquirectangular, CMTagCategory.ProjectionType, CMTagDataType.OSType, FourCC ("hequ"), true, false, 0, true, FourCC ("hequ"), false, 0, false, 0, "ProjectionTypeHalfEquirectangular");
				} else {
					AssertTag (CMTag.ProjectionTypeHalfEquirectangular, CMTagCategory.Undefined, CMTagDataType.Invalid, 0, false, false, 0, false, 0, false, 0, false, 0, "Invalid");
				}
				AssertTag (CMTag.ProjectionTypeFisheye, CMTagCategory.ProjectionType, CMTagDataType.OSType, FourCC ("fish"), true, false, 0, true, FourCC ("fish"), false, 0, false, 0, "ProjectionTypeFisheye");
				AssertTag (CMTag.PackingTypeNone, CMTagCategory.PackingType, CMTagDataType.OSType, FourCC ("none"), true, false, 0, true, FourCC ("none"), false, 0, false, 0, "PackingTypeNone");
				AssertTag (CMTag.PackingTypeSideBySide, CMTagCategory.PackingType, CMTagDataType.OSType, FourCC ("side"), true, false, 0, true, FourCC ("side"), false, 0, false, 0, "PackingTypeSideBySide");
				AssertTag (CMTag.PackingTypeOverUnder, CMTagCategory.PackingType, CMTagDataType.OSType, FourCC ("over"), true, false, 0, true, FourCC ("over"), false, 0, false, 0, "PackingTypeOverUnder");
			});
		}

		[Test]
		public void ToStringTests ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				Assert.That (default (CMTag).ToString (), Is.EqualTo ("{category:''{INVALID}"), "Default");
				Assert.That (CMTag.Invalid.ToString (), Is.EqualTo ("{category:''{INVALID}"), "Invalid");
				Assert.That (CMTag.MediaTypeVideo.ToString (), Is.EqualTo ("{category:'mdia' value:'vide' <OSType>}"), "MediaTypeVideo");
				Assert.That (CMTag.MediaSubTypeMebx.ToString (), Is.EqualTo ("{category:'msub' value:'mebx' <OSType>}"), "MediaSubTypeMebx");
				Assert.That (CMTag.MediaTypeAudio.ToString (), Is.EqualTo ("{category:'mdia' value:'soun' <OSType>}"), "MediaTypeAudio");
				Assert.That (CMTag.MediaTypeMetadata.ToString (), Is.EqualTo ("{category:'mdia' value:'meta' <OSType>}"), "MediaTypeMetadata");
				Assert.That (CMTag.StereoLeftEye.ToString (), Is.EqualTo ("{category:'eyes' value:0x1 <flags>}"), "StereoLeftEye");
				Assert.That (CMTag.StereoRightEye.ToString (), Is.EqualTo ("{category:'eyes' value:0x2 <flags>}"), "StereoRightEye");
				Assert.That (CMTag.StereoLeftAndRightEye.ToString (), Is.EqualTo ("{category:'eyes' value:0x3 <flags>}"), "StereoLeftAndRightEye");
				Assert.That (CMTag.StereoNone.ToString (), Is.EqualTo ("{category:'eyes' value:0x0 <flags>}"), "StereoNone");
				Assert.That (CMTag.StereoInterpretationOrderReversed.ToString (), Is.EqualTo ("{category:'eyip' value:0x1 <flags>}"), "StereoInterpretationOrderReversed");
				Assert.That (CMTag.ProjectionTypeRectangular.ToString (), Is.EqualTo ("{category:'proj' value:'rect' <OSType>}"), "ProjectionTypeRectangular");
				Assert.That (CMTag.ProjectionTypeEquirectangular.ToString (), Is.EqualTo ("{category:'proj' value:'equi' <OSType>}"), "ProjectionTypeEquirectangular");
				if (TestRuntime.CheckXcodeVersion (16, 0)) {
					Assert.That (CMTag.ProjectionTypeHalfEquirectangular.ToString (), Is.EqualTo ("{category:'proj' value:'hequ' <OSType>}"), "ProjectionTypeHalfEquirectangular");
				} else {
					Assert.That (CMTag.ProjectionTypeHalfEquirectangular.ToString (), Is.EqualTo ("{category:''{INVALID}"), "ProjectionTypeHalfEquirectangular");
				}
				Assert.That (CMTag.ProjectionTypeFisheye.ToString (), Is.EqualTo ("{category:'proj' value:'fish' <OSType>}"), "ProjectionTypeFisheye");
				Assert.That (CMTag.PackingTypeNone.ToString (), Is.EqualTo ("{category:'pack' value:'none' <OSType>}"), "PackingTypeNone");
				Assert.That (CMTag.PackingTypeSideBySide.ToString (), Is.EqualTo ("{category:'pack' value:'side' <OSType>}"), "PackingTypeSideBySide");
				Assert.That (CMTag.PackingTypeOverUnder.ToString (), Is.EqualTo ("{category:'pack' value:'over' <OSType>}"), "PackingTypeOverUnder");
			});
		}

		[Test]
		public void Dictionary ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var roundTrip = new Action<CMTag, string> ((tag, message) => {
				var dict = tag.ToDictionary ();
				var deserializedTag = CMTag.Create (dict);
				Assert.That (CMTag.Equals (tag, deserializedTag), Is.EqualTo (true), message);
			});

			Assert.Multiple (() => {
				roundTrip (default (CMTag), "Default");
				roundTrip (CMTag.Invalid, "Invalid");
				roundTrip (CMTag.MediaTypeVideo, "MediaTypeVideo");
				roundTrip (CMTag.MediaSubTypeMebx, "MediaSubTypeMebx");
				roundTrip (CMTag.MediaTypeAudio, "MediaTypeAudio");
				roundTrip (CMTag.MediaTypeMetadata, "MediaTypeMetadata");
				roundTrip (CMTag.StereoLeftEye, "StereoLeftEye");
				roundTrip (CMTag.StereoRightEye, "StereoRightEye");
				roundTrip (CMTag.StereoLeftAndRightEye, "StereoLeftAndRightEye");
				roundTrip (CMTag.StereoNone, "StereoNone");
				roundTrip (CMTag.StereoInterpretationOrderReversed, "StereoInterpretationOrderReversed");
				roundTrip (CMTag.ProjectionTypeRectangular, "ProjectionTypeRectangular");
				roundTrip (CMTag.ProjectionTypeEquirectangular, "ProjectionTypeEquirectangular");
				roundTrip (CMTag.ProjectionTypeHalfEquirectangular, "ProjectionTypeHalfEquirectangular");
				roundTrip (CMTag.ProjectionTypeFisheye, "ProjectionTypeFisheye");
				roundTrip (CMTag.PackingTypeNone, "PackingTypeNone");
				roundTrip (CMTag.PackingTypeSideBySide, "PackingTypeSideBySide");
				roundTrip (CMTag.PackingTypeOverUnder, "PackingTypeOverUnder");
			});
		}

		[Test]
		public void Hash ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			Assert.Multiple (() => {
				Assert.That (default (CMTag).GetHashCode (), Is.Not.EqualTo (0), "Default");
				Assert.That (CMTag.Invalid.GetHashCode (), Is.Not.EqualTo (0), "Invalid");
				Assert.That (CMTag.MediaTypeVideo.GetHashCode (), Is.Not.EqualTo (0), "MediaTypeVideo");
				Assert.That (CMTag.MediaSubTypeMebx.GetHashCode (), Is.Not.EqualTo (0), "MediaSubTypeMebx");
				Assert.That (CMTag.MediaTypeAudio.GetHashCode (), Is.Not.EqualTo (0), "MediaTypeAudio");
				Assert.That (CMTag.MediaTypeMetadata.GetHashCode (), Is.Not.EqualTo (0), "MediaTypeMetadata");
				Assert.That (CMTag.StereoLeftEye.GetHashCode (), Is.Not.EqualTo (0), "StereoLeftEye");
				Assert.That (CMTag.StereoRightEye.GetHashCode (), Is.Not.EqualTo (0), "StereoRightEye");
				Assert.That (CMTag.StereoLeftAndRightEye.GetHashCode (), Is.Not.EqualTo (0), "StereoLeftAndRightEye");
				Assert.That (CMTag.StereoNone.GetHashCode (), Is.Not.EqualTo (0), "StereoNone");
				Assert.That (CMTag.StereoInterpretationOrderReversed.GetHashCode (), Is.Not.EqualTo (0), "StereoInterpretationOrderReversed");
				Assert.That (CMTag.ProjectionTypeRectangular.GetHashCode (), Is.Not.EqualTo (0), "ProjectionTypeRectangular");
				Assert.That (CMTag.ProjectionTypeEquirectangular.GetHashCode (), Is.Not.EqualTo (0), "ProjectionTypeEquirectangular");
				Assert.That (CMTag.ProjectionTypeHalfEquirectangular.GetHashCode (), Is.Not.EqualTo (0), "ProjectionTypeHalfEquirectangular");
				Assert.That (CMTag.ProjectionTypeFisheye.GetHashCode (), Is.Not.EqualTo (0), "ProjectionTypeFisheye");
				Assert.That (CMTag.PackingTypeNone.GetHashCode (), Is.Not.EqualTo (0), "PackingTypeNone");
				Assert.That (CMTag.PackingTypeSideBySide.GetHashCode (), Is.Not.EqualTo (0), "PackingTypeSideBySide");
				Assert.That (CMTag.PackingTypeOverUnder.GetHashCode (), Is.Not.EqualTo (0), "PackingTypeOverUnder");
			});
		}
	}
}
