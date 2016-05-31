﻿using System;
using NUnit.Framework;

#if !XAMCORE_2_0
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
#else
using AppKit;
using ObjCRuntime;
using Foundation;
#endif

namespace Xamarin.Mac.Tests
{
	public class NSTextFieldTests
	{
		NSTextField textField;

		[SetUp]
		public void SetUp ()
		{
			textField = new NSTextField ();
		}

		[Test]
		public void NSTextFieldShouldChangePlaceholderString ()
		{
			Asserts.EnsureYosemite ();

			var placeholder = textField.PlaceholderString;
			textField.PlaceholderString = "Test";

			Assert.IsFalse (textField.PlaceholderString == placeholder, "NSTextFieldShouldChangePlaceholderString - Failed to set the PlaceholderString property");
		}

		[Test]
		public void NSTextFieldShouldChangePlaceholderAttributedString ()
		{
			Asserts.EnsureYosemite ();

			var placeholder = textField.PlaceholderAttributedString;
			textField.PlaceholderAttributedString = new NSAttributedString ("Test");

			Assert.IsFalse (textField.PlaceholderAttributedString == placeholder, "NSTextFieldShouldChangePlaceholderAttributedString - Failed to set the PlaceholderAttributedString property");
		}
	}
}