// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;

using CoreMedia;
using Foundation;
using NUnit.Framework;
using ObjCRuntime;
using VideoToolbox;

namespace MonoTouchFixtures.VideoToolbox;

[TestFixture]
[Preserve (AllMembers = true)]
public class VTCompressionPropertiesTest {
	[SetUp]
	public void SetUp ()
	{
		TestRuntime.AssertXcodeVersion (27, 0);
	}

	[Test]
	public void LogTransferFunctionDefaultValues ()
	{
		using var dictionary = new NSMutableDictionary ();
		var properties = new VTCompressionProperties (dictionary);

		Assert.That (properties.LogTransferFunction, Is.Null, "Strong");
		Assert.That (properties.WeakLogTransferFunction, Is.Null, "Weak");
		Assert.That (dictionary.ContainsKey (VTCompressionPropertyKey.LogTransferFunction), Is.False, "Key");
	}

	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog, "com.apple.rec2020.apple-log")]
	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog2, "com.apple.apple-wide-gamut.apple-log")]
	public void LogTransferFunction (CMFormatDescriptionLogTransferFunction value, string expected)
	{
		using var dictionary = new NSMutableDictionary ();
		var properties = new VTCompressionProperties (dictionary);

		properties.LogTransferFunction = value;

		Assert.That (properties.LogTransferFunction, Is.EqualTo (value), "Strong");
		Assert.That (properties.WeakLogTransferFunction, Is.EqualTo (expected), "Weak");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction], Is.InstanceOf<NSString> (), "Native type");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction].ToString (), Is.EqualTo (expected), "Native value");
		Assert.That (dictionary.Count, Is.EqualTo ((nuint) 1), "Count");
	}

	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog, "com.apple.rec2020.apple-log")]
	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog2, "com.apple.apple-wide-gamut.apple-log")]
	public void WeakLogTransferFunction (CMFormatDescriptionLogTransferFunction value, string expected)
	{
		using var dictionary = new NSMutableDictionary ();
		var properties = new VTCompressionProperties (dictionary);

		properties.WeakLogTransferFunction = expected;

		Assert.That (properties.WeakLogTransferFunction, Is.EqualTo (expected), "Weak");
		Assert.That (properties.LogTransferFunction, Is.EqualTo (value), "Strong");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction], Is.InstanceOf<NSString> (), "Native type");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction].ToString (), Is.EqualTo (expected), "Native value");
		Assert.That (dictionary.Count, Is.EqualTo ((nuint) 1), "Count");
	}

	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog, "com.apple.rec2020.apple-log")]
	[TestCase (CMFormatDescriptionLogTransferFunction.AppleLog2, "com.apple.apple-wide-gamut.apple-log")]
	public void LogTransferFunctionFromDictionary (CMFormatDescriptionLogTransferFunction value, string expected)
	{
		using var nativeValue = new NSString (expected);
		using var dictionary = NSDictionary.FromObjectAndKey (nativeValue, VTCompressionPropertyKey.LogTransferFunction);
		var properties = new VTCompressionProperties (dictionary);

		Assert.That (properties.LogTransferFunction, Is.EqualTo (value), "Strong");
		Assert.That (properties.WeakLogTransferFunction, Is.EqualTo (expected), "Weak");
	}

	[TestCase (true)]
	[TestCase (false)]
	public void LogTransferFunctionClearTwice (bool weak)
	{
		using var dictionary = new NSMutableDictionary ();
		var properties = new VTCompressionProperties (dictionary) {
			LogTransferFunction = CMFormatDescriptionLogTransferFunction.AppleLog,
			RealTime = true,
		};

		if (weak) {
			properties.WeakLogTransferFunction = null;
			properties.WeakLogTransferFunction = null;
		} else {
			properties.LogTransferFunction = null;
			properties.LogTransferFunction = null;
		}

		Assert.That (properties.LogTransferFunction, Is.Null, "Strong");
		Assert.That (properties.WeakLogTransferFunction, Is.Null, "Weak");
		Assert.That (dictionary.ContainsKey (VTCompressionPropertyKey.LogTransferFunction), Is.False, "Key");
		Assert.That (properties.RealTime, Is.True, "Unrelated property");
		Assert.That (dictionary.Count, Is.EqualTo ((nuint) 1), "Count");
	}

	[TestCase ("com.example.log")]
	[TestCase ("")]
	public void WeakLogTransferFunctionUnknownValue (string value)
	{
		using var dictionary = new NSMutableDictionary ();
		var properties = new VTCompressionProperties (dictionary) {
			WeakLogTransferFunction = value,
		};

		Assert.That (properties.WeakLogTransferFunction, Is.EqualTo (value), "Weak");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction], Is.InstanceOf<NSString> (), "Native type");
		Assert.That (dictionary [VTCompressionPropertyKey.LogTransferFunction].ToString (), Is.EqualTo (value), "Native value");
		Assert.Throws<NotSupportedException> (() => _ = properties.LogTransferFunction, "Strong");
		Assert.That (properties.WeakLogTransferFunction, Is.EqualTo (value), "Unchanged");

		properties.LogTransferFunction = null;
		Assert.That (properties.WeakLogTransferFunction, Is.Null, "Cleared");
		Assert.That (dictionary.ContainsKey (VTCompressionPropertyKey.LogTransferFunction), Is.False, "Key");
	}
}
