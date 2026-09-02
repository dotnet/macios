//
// Unit tests for NSUrl
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012-2013 Xamarin Inc. All rights reserved.
//

using System.Diagnostics;
using System.IO;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlTest {

		[Test]
		public void Fields ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			Assert.That (NSUrl.IsExcludedFromBackupKey.ToString (), Is.EqualTo ("NSURLIsExcludedFromBackupKey"), "IsExcludedFromBackupKey");
		}

		[Test]
		public void IsExcludedFromBackupKey ()
		{
			//TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 8, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false); // 10.8 fails DoNotBackupMe-1
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false); // 10.10 fails DoNotBackupMe-1

			// NOTE: this test was failing with either NullReferenceException or InvalidCastException
			// when we used CFBoolean as a NSObject (i.e. CFBoolean.TrueObject). The test order execution
			// was important to track this down

			NSObject value;
			Assert.That (NSBundle.MainBundle.ExecutableUrl.TryGetResource (NSUrl.IsExcludedFromBackupKey, out value), Is.True, "MainBundle");
			Assert.That (value, Is.TypeOf (typeof (NSNumber)), "NSNumber");
			Assert.That ((int) (value as NSNumber), Is.EqualTo (0), "0");

			var paths = NSSearchPath.GetDirectories (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			var filename = Path.Combine (paths [0], $"DoNotBackupMe-NSUrl-{Process.GetCurrentProcess ().Id}");
			try {
				File.WriteAllText (filename, "not worth a bit");
				using (NSUrl url = NSUrl.FromFilename (filename)) {
					Assert.That (url.TryGetResource (NSUrl.IsExcludedFromBackupKey, out value), Is.True);
					Assert.That ((int) (value as NSNumber), Is.EqualTo (0), "DoNotBackupMe-0");

					url.SetResource (NSUrl.IsExcludedFromBackupKey, (NSNumber) 1);

					Assert.That (url.TryGetResource (NSUrl.IsExcludedFromBackupKey, out value), Is.True);
					Assert.That ((int) (value as NSNumber), Is.EqualTo (1), "DoNotBackupMe-1");

					NSError error;
					NSDictionary dict = url.GetResourceValues (new NSString [] { NSUrl.IsExcludedFromBackupKey }, out error);
					Assert.That (error, Is.Null, "error");
					Assert.That (dict.Keys [0], Is.EqualTo (NSUrl.IsExcludedFromBackupKey), "Key");
					Assert.That ((int) (dict.Values [0] as NSNumber), Is.EqualTo (1), "Value");
				}
			} finally {
				// otherwise the attribute won't reset even if the file is overwritten
				File.Delete (filename);
			}
		}

		const string bad_uri = "http://localhost/page?query={bad}";
		const string good_uri = "http://localhost/page?query=%7Bgood%7D";

		[Test]
		public void FromString ()
		{
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				using (var url = NSUrl.FromString (bad_uri))
					Assert.That (bad_uri, Is.Not.Null, "invalid");
			} else {
				Assert.That (NSUrl.FromString (bad_uri), Is.Null, "invalid");
			}

			using (var url = NSUrl.FromString (good_uri)) {
				Assert.That (url.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");

				Assert.That (url.PathExtension, Is.EqualTo (String.Empty), "PathExtension-1");

				Assert.That (url.ToString (), Is.Not.Null, "ToString"); // see #4763
			}

			using (var url = NSUrl.FromString ("file.extension")) {
				Assert.That (url.PathExtension, Is.EqualTo ("extension"), "PathExtension-2");
			}
		}

		[Test]
		public void Ctor_string ()
		{
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				using (var url = new NSUrl (bad_uri))
					Assert.That (url.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			} else {
				Assert.Throws<Exception> (() => new NSUrl (bad_uri), "exception");
			}

			using (var url = new NSUrl (good_uri)) {
				Assert.That (url.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public void Unicode_6597 ()
		{
			const string bug6597 = "http://www.bing.com/images/search?q=雅詩蘭黛";

			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				Assert.That (NSUrl.FromString (bug6597), Is.Not.Null, "1");

				using (var url = new NSUrl (bug6597))
					Assert.That (url, Is.Not.Null, "exception");
			} else {
				// does not work - From* static methods returns null for invalid URL
				Assert.That (NSUrl.FromString (bug6597), Is.Null, "1");

				// does not work - handle is null (as a .NET .ctor can't return null like ObjC init can do)
				Assert.Throws<Exception> (() => new NSUrl (bug6597), "exception");
			}


			// works
			using (var s = new NSString (bug6597))
			using (var url3 = NSUrl.FromObject (s)) {
				Assert.That (url3.Handle, Is.Not.EqualTo (IntPtr.Zero), "3");
				Assert.That (url3.ToString (), Is.EqualTo (bug6597), "ToString");
			}
		}

		[Test]
		public void Bug13069 ()
		{
			string url = "http://username:password@google.com:8080/path?query=value.ext";
			var uri = new Uri (url);
			Assert.That (url.ToString (), Is.EqualTo (url), "Uri.ToString");
			var ns1 = (NSUrl) uri;
			Assert.That (ns1.ToString (), Is.EqualTo (url), "implicit NSUrl.ToString");
			var ns2 = new NSUrl (uri.ToString ());
			Assert.That (ns2.ToString (), Is.EqualTo (url), "created NSUrl.ToString");
		}

		[Test]
		public void InitWithSpaces ()
		{
			// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=14307
			// note: add space in file name - otherwise there was no space in the file on devices
			// device: /var/mobile/Applications/2042876B-F875-4A67-94EE-286B6A81FDCF/monotouchtest.app/Hand.wav
			// simulator: /Users/poupou/Library/Application Support/iPhone Simulator/6.0/Applications/DCFB542F-1D37-4ADC-9046-BB0D26ABB3A3/monotouchtest.app
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand Left.wav");

			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				using (var url = NSUrl.FromString (file))
					Assert.That (url, Is.Not.Null, "1");
			} else {
				// initWithString: will fail with spaces
				Assert.Throws<Exception> (() => new NSUrl (file), "1");
			}

			using (var url2 = new NSUrl (file, false)) {
				// initFileURLWithPath:isDirectory: will always works (both sim or devices)
				Assert.That (url2.Handle, Is.Not.EqualTo (IntPtr.Zero), "2");
			}
		}

		[Test]
		public void Copy ()
		{
			using (var url = NSUrl.FromString ("http://www.xamarin.com")) {
				// NSObject.Copy works because NSUrl conforms to NSCopying
				using (var copy = (NSUrl) url.Copy ()) {
					Assert.That (copy.AbsoluteString, Is.EqualTo ("http://www.xamarin.com"), "AbsoluteString");
				}
			}
		}

		[Test]
		public void MutableCopy ()
		{
			using (var url = NSUrl.FromString ("http://www.xamarin.com")) {
				// NSObject.MutableCopy does NOT works because NSUrl does NOT conforms to NSMutableCopying
				Assert.Throws<InvalidOperationException> (delegate
				{
					url.MutableCopy ();
				}, "MutableCopy");
			}
		}

		[Test]
		public void Equals ()
		{
			using (var url1 = NSUrl.FromString ("http://www.xamarin.com/ios"))
			using (var url2 = NSUrl.FromString ("http://www.xamarin.com"))
			using (var url3 = new NSUrl ("http://www.xamarin.com")) {
				Assert.That (url1.GetHashCode (), Is.Not.EqualTo (url2.GetHashCode ()), "GetHashCode 1-2");
				// created differently but still identical
				Assert.That (url2.GetHashCode (), Is.EqualTo (url3.GetHashCode ()), "GetHashCode 2-3");

				// NSObject
				Assert.That (url1.Equals ((NSObject) url2), Is.False, "Equals(NSObject) 1-2");
				Assert.That (url2.Equals ((NSObject) url3), Is.True, "Equals(NSObject) 2-3");
				Assert.That (url1.Equals ((NSObject) null), Is.False, "Equals(NSObject) null");

				// NSUrl / IEquatable<NSUrl>
				Assert.That (url1.Equals (url2), Is.False, "Equals(NSUrl) 1-2");
				Assert.That (url2.Equals (url3), Is.True, "Equals(NSUrl) 2-3");
				Assert.That (url1.Equals ((NSUrl) null), Is.False, "Equals(NSUrl) null");
			}
		}

		class BadCustomUrl : NSUrl {

			public BadCustomUrl (string url) : base (url)
			{
				Comment = "Bad";
			}

			public string Comment { get; set; }

			public bool DirectBinding {
				get { return IsDirectBinding; }
			}
		}

		class GoodCustomUrl : NSUrl {

			public GoodCustomUrl (string url) : base (url)
			{
				Comment = "Good";
			}

			public string Comment { get; set; }

			public bool DirectBinding {
				get { return IsDirectBinding; }
			}

			public override int GetHashCode ()
			{
				return base.GetHashCode () ^ Comment.GetHashCode ();
			}

			public override bool Equals (object t)
			{
				var url = (t as GoodCustomUrl);
				if (url is null)
					return false;
				return base.Equals (t) && Comment == url.Comment;
			}
		}

		[Test]
		public void SubclassEquality ()
		{
			using (var url1 = NSUrl.FromString ("http://www.xamarin.com"))
			using (var url2 = new BadCustomUrl ("http://www.xamarin.com"))
			using (var url3 = new GoodCustomUrl ("http://www.xamarin.com")) {
				Assert.That (url1.GetHashCode (), Is.Not.EqualTo (url2.GetHashCode ()), "GetHashCode 1-2");
				Assert.That (url2.GetHashCode (), Is.Not.EqualTo (url3.GetHashCode ()), "GetHashCode 2-3");

				Assert.That (url2.DirectBinding, Is.False, "DirectBinding 2");
				Assert.That (url3.DirectBinding, Is.False, "DirectBinding 3");
				Assert.That (url2.GetHashCode (), Is.Not.EqualTo (url3.GetHashCode ()), "GetHashCode 2-3");

				// NSObject
				Assert.That (url1.Equals ((NSObject) url2), Is.False, "Equals(NSObject) 1-2");
				Assert.That (url2.Equals ((NSObject) url3), Is.False, "Equals(NSObject) 2-3");

				// NSUrl / IEquatable<NSUrl>
				Assert.That (url1.Equals (url2), Is.False, "Equals(NSUrl) 1-2");
				Assert.That (url2.Equals (url3), Is.False, "Equals(NSUrl) 2-3");

				// System.Object
				Assert.That (url1.Equals ((object) url2), Is.False, "Equals(object) 1-2");
				Assert.That (url2.Equals ((object) url3), Is.False, "Equals(object) 2-3");
			}
		}

		[Test]
		public void Invalid_29510 ()
		{
			string bad = "Server 1/Custom View/Analog Schedule!@#$%^&&%$#@";

#pragma warning disable SYSLIB0013 // warning SYSLIB0013: 'Uri.EscapeUriString(string)' is obsolete: 'Uri.EscapeUriString can corrupt the Uri string in some cases. Consider using Uri.EscapeDataString for query string components instead.'
			string bad_url = Uri.EscapeUriString (bad);
#pragma warning restore
			if (TestRuntime.CheckXcodeVersion (15, 0)) {
				using (var url = NSUrl.FromString (bad_url))
					Assert.That (bad_url, Is.Not.Null, "bad");
			} else {
				Assert.That (NSUrl.FromString (bad_url), Is.Null, "bad");
			}

			string converted = ((NSString) bad).CreateStringByAddingPercentEscapes (NSStringEncoding.UTF8);
			using (var url = NSUrl.FromString (converted)) {
				Assert.That (url.AbsoluteString, Is.EqualTo ("Server%201/Custom%20View/Analog%20Schedule!@%23$%25%5E&&%25$%23@"), "good");
			}
		}

		[Test]
		public void TestEqualOperatorSameInstace ()
		{
			using (var url = NSUrl.FromString ("http://www.xamarin.com"))
#pragma warning disable CS1718 // warning CS1718: Comparison made to same variable; did you mean to compare something else?
				Assert.That (url == url, Is.True);
#pragma warning restore

		}

		[Test]
		public void TestEqualOperatorNull ()
		{
			using (var url = NSUrl.FromString ("http://www.xamarin.com")) {
				Assert.That (url is null, Is.False, "url is null");
				Assert.That (null == url, Is.False, "null == url");
			}
		}

		[Test]
		public void TestEqualOperator ()
		{
			using (var url1 = NSUrl.FromString ("http://www.xamarin.com"))
			using (var url2 = NSUrl.FromString ("http://www.xamarin.com/foo"))
				Assert.That (url1.IsEqual (url2), Is.EqualTo (url1 == url2));
		}

		[Test]
		public void TestNotEqualOperatorNull ()
		{
			using (var url = NSUrl.FromString ("http://www.xamarin.com")) {
				Assert.That (url is not null, Is.True, "url is not null");
				Assert.That (null != url, Is.True, "null != url");
			}
		}

		[Test]
		public void TestNotEqualOperator ()
		{
			using (var url1 = NSUrl.FromString ("http://www.xamarin.com"))
			using (var url2 = NSUrl.FromString ("http://www.xamarin.com/foo"))
				Assert.That (!url1.IsEqual (url2), Is.EqualTo (url1 != url2));
		}

		[TestCase ("http://microsoft.com/", UriKind.Absolute)]
		[TestCase ("https://microsoft.com/", UriKind.Absolute)]
		[TestCase ("https://microsoft.com/some/path", UriKind.Absolute)]
		[TestCase ("https://microsoft.com/page?value=foo", UriKind.Absolute)]
		[TestCase ("relative", UriKind.Relative)]
		[TestCase ("relative/to/some/page", UriKind.Relative)]
		[TestCase ("relative?value=foo", UriKind.Relative)]
		public void ImplicitOperatorRoundTrip (string value, UriKind kind)
		{
			var nsurl = new NSUrl (value);
			Assert.That (((NSUrl) (Uri) nsurl).ToString (), Is.EqualTo (nsurl.ToString ()), "RoundTrip NSUrl");

			var url = new Uri (value, kind);
			Assert.That (((Uri) (NSUrl) url).ToString (), Is.EqualTo (url.ToString ()), "RoundTrip Uri");
		}

		[Test]
		public void FromNullString ()
		{
			Assert.That (NSUrl.FromString (null), Is.Null);
		}
	}
}
