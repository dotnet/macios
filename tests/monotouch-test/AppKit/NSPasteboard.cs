#if __MACOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using AppKit;
using DataDetection;
using ObjCRuntime;
using Foundation;
using UniformTypeIdentifiers;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSPasteboardTests {
		[Test]
		public void DetectPatternsTests_WeaklyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSSet<NSString>? detectedPatterns = null;
				NSError? error = null;

				var hashSet = new NSSet<NSString> (new [] { NSPasteboardDetectionPattern.EmailAddress.GetConstant () });
				var callback = new NSPasteboardDetectPatternsHandler ((NSSet<NSString> detectedResult, NSError? errorResult) => {
					detectedPatterns = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectPatterns #1 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "WeaklyTyped DetectedPatterns #1 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (0), "WeaklyTyped DetectedPatterns #1 count");
				Assert.That (error, Is.Null, "WeaklyTyped DetectedPatterns #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectPatterns #2 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "WeaklyTyped DetectedPatterns #2 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (1), "WeaklyTyped DetectedPatterns #2 count");
				Assert.That (detectedPatterns.ToArray ().Select (v => v.ToString ()), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "WeaklyTyped DetectedPatterns #2 email");
				Assert.That (error, Is.Null, "WeaklyTyped DetectedPatterns #2 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectPatternTests_SomewhatStronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSSet<NSString>? detectedPatterns = null;
				NSError? error = null;

				var hashSet = new HashSet<NSPasteboardDetectionPattern> (new [] { NSPasteboardDetectionPattern.EmailAddress });
				var callback = new NSPasteboardDetectPatternsHandler ((NSSet<NSString>? detectedResult, NSError? errorResult) => {
					detectedPatterns = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectPatterns #1 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "SomewhatStronglyTyped DetectedPatterns #1 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (0), "SomewhatStronglyTyped DetectedPatterns #1 count");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectedPatterns #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectPatterns #2 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "SomewhatStronglyTyped DetectedPatterns #2 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (1), "SomewhatStronglyTyped DetectedPatterns #2 count");
				Assert.That (detectedPatterns.ToArray ().Select (v => v.ToString ()), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "SomewhatStronglyTyped DetectedPatterns #2 email");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectedPatterns #2 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectPatternTests_StronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				HashSet<NSPasteboardDetectionPattern>? detectedPatterns = null;
				NSError? error = null;

				var hashSet = new HashSet<NSPasteboardDetectionPattern> (new [] { NSPasteboardDetectionPattern.EmailAddress });
				var callback = new NSPasteboardDetectPatternsCompletionHandler ((HashSet<NSPasteboardDetectionPattern> detectedResult, NSError? errorResult) => {
					detectedPatterns = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectPatterns #1 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "StronglyTyped DetectedPatterns #1 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (0), "StronglyTyped DetectedPatterns #1 count");
				Assert.That (error, Is.Null, "StronglyTyped DetectedPatterns #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detectedPatterns = null;
				error = null;
				pasteboard.DetectPatterns (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectPatterns #2 wait");
				Assert.That (detectedPatterns, Is.Not.Null, "StronglyTyped DetectedPatterns #2 patterns");
				Assert.That ((int) detectedPatterns.Count, Is.EqualTo (1), "StronglyTyped DetectedPatterns #2 count");
				Assert.That (detectedPatterns, Does.Contain (NSPasteboardDetectionPattern.EmailAddress), "StronglyTyped DetectedPatterns #2 email");
				Assert.That (error, Is.Null, "StronglyTyped DetectedPatterns #2 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectValuesTests_WeaklyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSDictionary<NSString, NSObject>? detected = null;
				NSError? error = null;
				DDMatchEmailAddress matchedEmail;

				var callback = new NSSet<NSString> (new [] { NSPasteboardDetectionPattern.EmailAddress.GetConstant () });
				var hashSet = new NSPasteboardDetectValuesHandler ((NSDictionary<NSString, NSObject> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (callback, hashSet);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectValues #1 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectValues #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "WeaklyTyped DetectValues #1 count");
				Assert.That (error, Is.Null, "WeaklyTyped DetectValues #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (callback, hashSet);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped etectValues #2 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectValues #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "WeaklyTyped DetectValues #2 count");
				Assert.That (detected.Keys.Select (v => v.ToString ()).ToArray (), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "WeaklyTyped DetectValues #2 email");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("DDMatchEmailAddress"), "WeaklyTyped DetectValues #2 value");
				var array = ((NSArray) detected.Values.First ());
				matchedEmail = (DDMatchEmailAddress) array.First ();
				Assert.That (matchedEmail.EmailAddress, Is.EqualTo ("rolf@xamarin.com"), "WeaklyTyped DetectValues #2 match.EmailAddress");
				Assert.That (matchedEmail.Label, Is.Null, "WeaklyTyped DetectValues #2 match.Label");
				Assert.That (error, Is.Null, "WeaklyTyped DetectValues #2 error");

				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (callback, hashSet);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectValues #3 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectValues #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "WeaklyTyped DetectValues #3 count");
				Assert.That (detected.Keys.Select (v => v.ToString ()).ToArray (), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "WeaklyTyped DetectValues #3 email");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("DDMatchEmailAddress"), "WeaklyTyped DetectValues #3 value");
				Assert.That (error, Is.Null, "WeaklyTyped DetectValues #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectValuesTests_SomewhatStronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSDictionary<NSString, NSObject>? detected = null;
				NSError? error = null;
				DDMatchEmailAddress matchedEmail;

				var hashSet = new HashSet<NSPasteboardDetectionPattern> (new [] { NSPasteboardDetectionPattern.EmailAddress });
				var callback = new NSPasteboardDetectValuesHandler ((NSDictionary<NSString, NSObject> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectValues #1 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectValues #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "SomewhatStronglyTyped DetectValues #1 count");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectValues #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectValues #2 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectValues #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "SomewhatStronglyTyped DetectValues #2 count");
				Assert.That (detected.Keys.First ().ToString (), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "SomewhatStronglyTyped DetectValues #2 email");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("DDMatchEmailAddress"), "SomewhatStronglyTyped DetectValues #2 value");
				var array = ((NSArray) detected.Values.First ());
				matchedEmail = (DDMatchEmailAddress) array.First ();
				Assert.That (matchedEmail.EmailAddress, Does.Contain ("rolf@xamarin.com"), "SomewhatStronglyTyped DetectValues #2 match.EmailAddress");
				Assert.That (matchedEmail.Label, Is.Null, "SomewhatStronglyTyped DetectValues #2 match.Label");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectValues #2 error");

				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectValues #3 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectValues #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "SomewhatStronglyTyped DetectValues #3 count");
				Assert.That (detected.Keys.First ().ToString (), Does.Contain (NSPasteboardDetectionPattern.EmailAddress.GetConstant ().ToString ()), "SomewhatStronglyTyped DetectValues #3 email");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("DDMatchEmailAddress"), "SomewhatStronglyTyped DetectValues #3 value");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectValues #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectValuesTests_StronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				Dictionary<NSPasteboardDetectionPattern, DDMatch[]>? detected = null;
				NSError? error = null;
				DDMatch[] matches;
				DDMatch match;
				DDMatchEmailAddress matchedEmail;

				var hashSet = new HashSet<NSPasteboardDetectionPattern> (new [] { NSPasteboardDetectionPattern.EmailAddress });
				var callback = new NSPasteboardDetectValuesCompletionHandler ((Dictionary<NSPasteboardDetectionPattern, DDMatch[]> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectValues #1 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectValues #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "StronglyTyped DetectValues #1 count");
				Assert.That (error, Is.Null, "StronglyTyped DetectValues #1 error");

				pasteboard.SetStringForType ("rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectValues #2 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectValues #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "StronglyTyped DetectValues #2 count");
				Assert.That (detected.Keys, Does.Contain (NSPasteboardDetectionPattern.EmailAddress), "StronglyTyped DetectValues #2 email");
				matches = detected.Values.First ();
				Assert.That (matches.Length, Is.EqualTo (1), "StronglyTyped DetectValues #2 matches.Length");
				match = matches [0];
				Assert.That (match.MatchedString, Is.EqualTo ("rolf@xamarin.com"), "StronglyTyped DetectValues #2 match.MatchedString");
				matchedEmail = match as DDMatchEmailAddress;
				Assert.That (matchedEmail.EmailAddress, Is.EqualTo ("rolf@xamarin.com"), "StronglyTyped DetectValues #2 match.EmailAddress");
				Assert.That (matchedEmail.Label, Is.Null, "StronglyTyped DetectValues #2 match.Label");
				Assert.That (error, Is.Null, "StronglyTyped DetectValues #2 error");

				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectValues (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectValues #3 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectValues #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "StronglyTyped DetectValues #3 count");
				Assert.That (detected.Keys, Does.Contain (NSPasteboardDetectionPattern.EmailAddress), "StronglyTyped DetectValues #3 email");
				matches = detected.Values.First ();
				Assert.That (matches.Length, Is.EqualTo (1), "StronglyTyped DetectValues #3 matches.Length");
				match = matches [0];
				Assert.That (match.MatchedString, Is.EqualTo ("rolf@xamarin.com"), "StronglyTyped DetectValues #3 match.MatchedString");
				matchedEmail = match as DDMatchEmailAddress;
				Assert.That (matchedEmail.EmailAddress, Is.EqualTo ("rolf@xamarin.com"), "StronglyTyped DetectValues #3 match.EmailAddress");
				Assert.That (matchedEmail.Label, Is.Null, "StronglyTyped DetectValues #3 match.Label");
				Assert.That (error, Is.Null, "StronglyTyped DetectValues #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}


		[Test]
		public void DetectMetadataTests_WeaklyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSDictionary<NSString, NSObject>? detected = null;
				NSError? error = null;

				var hashSet = new NSSet<NSString> (new [] { NSPasteboardMetadataType.ContentType.GetConstant () });
				var callback = new NSPasteboardDetectMetadataHandler ((NSDictionary<NSString, NSObject> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectMetadata #1 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectMetadata #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "WeaklyTyped DetectMetadata #1 count");
				Assert.That (error, Is.Null, "WeaklyTyped DetectMetadata #1 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("file:///this/is/some/file.html", NSPasteboardType.FileUrl.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectMetadata #2 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectMetadata #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "WeaklyTyped DetectMetadata #2 count");
				Assert.That (detected.Keys.First ().ToString (), Does.Contain (NSPasteboardMetadataType.ContentType.GetConstant ().ToString ()), "WeaklyTyped DetectMetadata #2 email");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("public.html"), "WeaklyTyped DetectMetadata #2 value");
				var uttype = (UTType) detected.Values.First ();
				Assert.That (uttype.Identifier, Is.EqualTo ("public.html"), "WeaklyTyped DetectMetadata #2 uttype.Identifier");
				Assert.That (error, Is.Null, "WeaklyTyped DetectMetadata #2 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "WeaklyTyped DetectMetadata #3 wait");
				Assert.That (detected, Is.Not.Null, "WeaklyTyped DetectMetadata #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "WeaklyTyped DetectMetadata #3 count");
				Assert.That (error, Is.Not.Null, "WeaklyTyped DetectMetadata #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectMetadataTests_SomewhatStronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				NSDictionary<NSString, NSObject>? detected = null;
				NSError? error = null;

				var hashSet = new HashSet<NSPasteboardMetadataType> (new [] { NSPasteboardMetadataType.ContentType });
				var callback = new NSPasteboardDetectMetadataHandler ((NSDictionary<NSString, NSObject> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectMetadata #1 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectMetadata #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "SomewhatStronglyTyped DetectMetadata #1 count");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectMetadata #1 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("file:///this/is/some/file.html", NSPasteboardType.FileUrl.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectMetadata #2 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectMetadata #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "SomewhatStronglyTyped DetectMetadata #2 count");
				Assert.That (detected.Keys.First ().ToString (), Does.Contain (NSPasteboardMetadataType.ContentType.GetConstant ().ToString ()), "SomewhatStronglyTyped DetectMetadata #2 contenttype");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("public.html"), "SomewhatStronglyTyped DetectMetadata #2 value");
				var uttype = (UTType) detected.Values.First ();
				Assert.That (uttype.Identifier, Is.EqualTo ("public.html"), "SomewhatStronglyTyped DetectMetadata #2 uttype.Identifier");
				Assert.That (error, Is.Null, "SomewhatStronglyTyped DetectMetadata #2 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "SomewhatStronglyTyped DetectMetadata #3 wait");
				Assert.That (detected, Is.Not.Null, "SomewhatStronglyTyped DetectMetadata #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "SomewhatStronglyTyped DetectMetadata #3 count");
				Assert.That (error, Is.Not.Null, "SomewhatStronglyTyped DetectMetadata #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void DetectMetadataTests_StronglyTyped ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var pasteboard = NSPasteboard.CreateWithUniqueName ();
			try {
				var evt = new ManualResetEvent (false);
				Dictionary<NSPasteboardMetadataType, UTType>? detected = null;
				NSError? error = null;

				var hashSet = new HashSet<NSPasteboardMetadataType> (new [] { NSPasteboardMetadataType.ContentType });
				var callback = new NSPasteboardDetectMetadataCompletionHandler ((Dictionary<NSPasteboardMetadataType, UTType> detectedResult, NSError? errorResult) => {
					detected = detectedResult;
					error = errorResult;
					evt.Set ();
				});

				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectMetadata #1 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectMetadata #1 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "StronglyTyped DetectMetadata #1 count");
				Assert.That (error, Is.Null, "StronglyTyped DetectMetadata #1 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("file:///this/is/some/file.html", NSPasteboardType.FileUrl.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectMetadata #2 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectMetadata #2 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (1), "StronglyTyped DetectMetadata #2 count");
				Assert.That (detected.Keys.First (), Is.EqualTo (NSPasteboardMetadataType.ContentType), "StronglyTyped DetectMetadata #2 contenttype");
				Assert.That (detected.Values.First ().ToString (), Does.Contain ("public.html"), "StronglyTyped DetectMetadata #2 value");
				var uttype = detected.Values.First ();
				Assert.That (uttype.Identifier, Is.EqualTo ("public.html"), "StronglyTyped DetectMetadata #2 uttype.Identifier");
				Assert.That (error, Is.Null, "StronglyTyped DetectMetadata #2 error");

				pasteboard.ClearContents ();
				pasteboard.SetStringForType ("Send an email to rolf@xamarin.com", NSPasteboardType.String.GetConstant ());
				evt.Reset ();
				detected = null;
				error = null;
				pasteboard.DetectMetadata (hashSet, callback);
				Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "StronglyTyped DetectMetadata #3 wait");
				Assert.That (detected, Is.Not.Null, "StronglyTyped DetectMetadata #3 patterns");
				Assert.That ((int) detected.Count, Is.EqualTo (0), "StronglyTyped DetectMetadata #3 count");
				Assert.That (error, Is.Not.Null, "StronglyTyped DetectMetadata #3 error");
			} finally {
				pasteboard.ReleaseGlobally ();
			}
		}

		[Test]
		public void NSPasteboardTests_WriteObjectTests ()
		{
			NSPasteboard b = NSPasteboard.CreateWithUniqueName ();
			if (b is null)
				Assert.Inconclusive ("NSPasteboard could not be provided by the OS.");
			b.WriteObjects (new INSPasteboardWriting [] { (NSString) "asfd" });
#if NET
			b.WriteObjects (new INSPasteboardWriting [] { new MyPasteboard () });
#else
			b.WriteObjects (new NSPasteboardWriting [] { new MyPasteboard () });
#endif
			// from the docs: the lifetime of a unique pasteboard is not related to the lifetime of the creating app,
			// you must release a unique pasteboard by calling releaseGlobally to avoid possible leaks. 
			b.ReleaseGlobally ();
		}

#if NET
		class MyPasteboard2 : NSObject, INSPasteboardReading
#else
		class MyPasteboard2 : NSPasteboardReading
#endif
		{
#if !NET
			public override NSObject InitWithPasteboardPropertyList (NSObject propertyList, string type)
			{
				return new NSObject ();
			}
#endif
		}

#if NET
		class MyPasteboard : NSObject, INSPasteboardWriting
#else
		class MyPasteboard : NSPasteboardWriting
#endif
		{
#if NET
			NSObject INSPasteboardWriting.GetPasteboardPropertyListForType (string type)
#else
			public override NSObject GetPasteboardPropertyListForType (string type)
#endif
			{
				return new NSObject ();
			}

#if NET
			string [] INSPasteboardWriting.GetWritableTypesForPasteboard (NSPasteboard pasteboard)
#else
			public override string[] GetWritableTypesForPasteboard (NSPasteboard pasteboard)
#endif
			{
				return new string [] { };
			}

#if NET
			[Export ("writingOptionsForType:pasteboard:")]
			public NSPasteboardWritingOptions GetWritingOptionsForType (string type, NSPasteboard pasteboard)
#else
			public override NSPasteboardWritingOptions GetWritingOptionsForType (string type, NSPasteboard pasteboard)
#endif
			{
				return NSPasteboardWritingOptions.WritingPromised;
			}
		}
	}
}
#endif // __MACOS__
