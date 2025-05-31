//
// Unit tests for NSBundle
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012-2013 Xamarin Inc. All rights reserved.
//

using System;
using System.Net;
using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class BundleTest {

		NSBundle main = NSBundle.MainBundle;

		[Test]
		public void LocalizedString2 ()
		{
			string s = main.GetLocalizedString (null, "value");
			Assert.That (s, Is.EqualTo ("value"), "key");

			s = main.GetLocalizedString ("key", null);
			Assert.That (s, Is.EqualTo ("key"), "key");

			s = main.GetLocalizedString (null, null);
			Assert.That (s, Is.Empty, "all-null");
		}

		[Test]
		public void LocalizedString3 ()
		{
			string s = main.GetLocalizedString (null, "value", "table");
			Assert.That (s, Is.EqualTo ("value"), "key");

			s = NSBundle.MainBundle.GetLocalizedString ("key", null, "table");
			Assert.That (s, Is.EqualTo ("key"), "value");

			s = NSBundle.MainBundle.GetLocalizedString (null, "value", null);
			Assert.That (s, Is.EqualTo ("value"), "comment");

			s = main.GetLocalizedString (null, null, null);
			Assert.That (s, Is.Empty, "all-null");
		}

		// http://developer.apple.com/library/ios/#documentation/uikit/reference/NSBundle_UIKitAdditions/Introduction/Introduction.html

#if false // Disabling for now due to Xcode 9 does not support nibs if deployment target == 6.0
		[Test]
		public void LoadNibWithOptions ()
		{
#if MONOMAC
			NSArray objects;
			Assert.NotNull (main.LoadNibNamed ("EmptyNib", main, out objects));
#else
			Assert.NotNull (main.LoadNib ("EmptyNib", main, null));
#endif
		}
#endif

#if false
		// some selectors are only in AppKit but we included them in MonoTouch (and this match Apple documentation)
		// https://developer.apple.com/library/mac/#documentation/Cocoa/Reference/ApplicationKit/Classes/NSBundle_AppKitAdditions/Reference/Reference.html
		
		// I guess no one ever used them since they don't work...
		// commented (selectors removed from MT bindings) - can be re-enabled to test newer iOS releases
		[Test]
		public void PathForImageResource ()
		{
			Assert.Throws<ObjCException> (() => main.PathForImageResource ("basn3p08.png"));
		}

		[Test]
		public void PathForSoundResource ()
		{
			Assert.Throws<ObjCException> (() => main.PathForSoundResource ("basn3p08.png"));
		}

		[Test]
		public void LoadNib ()
		{
			Assert.Throws<ObjCException> (() => NSBundle.LoadNib (String.Empty, main));
		}
#endif
		[Test]
		public void Localizations ()
		{
			string [] locz = main.Localizations;
			Assert.That (locz.Length, Is.GreaterThanOrEqualTo (0), "Length");
		}

		[Test]
		public void PreferredLocalizations ()
		{
			string [] locz = main.PreferredLocalizations;
			var localizations = new string [] { "en-AU", "en-UK", "es", "es-AR", "es-ES" };
			Assert.That (locz.Length, Is.GreaterThanOrEqualTo (1), "Length");
			Assert.That (localizations, Contains.Item (locz [0]), $"PreferredLocalizations: {string.Join (";", locz)}");
		}

		[Test]
		public void AppStoreReceiptURL ()
		{
			if (!TestRuntime.CheckXcodeVersion (5, 0))
				Assert.Inconclusive ("Requires iOS7 or later");

			// The AppStoreReceiptUrl property may or may not return anything useful on the simulator, so run this only on device.
			TestRuntime.AssertDevice ();

			// on iOS8 device this now ends with "/StoreKit/sandboxReceipt" 
			// instead of "/StokeKit/receipt"
			Assert.That (main.AppStoreReceiptUrl.AbsoluteString, Does.EndWith ("eceipt"), "AppStoreReceiptUrl");
		}

		[Test]
		public void GetLocalizedString ()
		{
			// null values are fine
			using (var l = main.GetLocalizedString (null, null, null))
				Assert.That (l.Length, Is.EqualTo ((nint) 0), "null,null,null");

			// NoKey does not exists so the same string is returned
			using (var l = main.GetLocalizedString ("NoKey", null, null))
				Assert.That (l.ToString (), Is.EqualTo ("NoKey"), "string,null,null");
			using (var key = new NSString ("NoKey"))
			using (var l = main.GetLocalizedString (key, null, null))
				Assert.That (l.ToString (), Is.EqualTo ("NoKey"), "NString,null,null");

			// TestKey exists (Localizable.strings) and returns TestValue
			using (var l = main.GetLocalizedString ("TestKey", null, "Localizable"))
				Assert.That (l.ToString (), Is.EqualTo ("TestValue"), "string,null,null-2");
			using (var key = new NSString ("TestKey"))
			using (var l = main.GetLocalizedString (key, null, "Localizable"))
				Assert.That (l.ToString (), Is.EqualTo ("TestValue"), "NString,null,null-2");
		}
	}
}
