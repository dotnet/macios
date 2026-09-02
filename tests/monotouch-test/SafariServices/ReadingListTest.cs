//
// Unit tests for SSReadingList
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using System.IO;
using SafariServices;
using UIKit;
using Xamarin.Utils;

namespace MonoTouchFixtures.SafariServices {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ReadingListTest {

		string local_file = Path.Combine (NSBundle.MainBundle.ResourcePath, "Hand.wav");

		[Test]
		[Ignore ("This test adds two entries every time it's executed to the global reading list in Safari. For people who use their reading lists this becomes slightly annoying.")]
		public void DefaultReadingList ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			NSError error;
			using (var http = new NSUrl ("http://www.xamarin.com"))
			using (var local = new NSUrl (local_file, false))
			using (var rl = SSReadingList.DefaultReadingList) {
				Assert.That (rl.Add (http, "title", "preview text", out error), Is.True, "Add-1");
				Assert.That (error, Is.Null, "error-1");

				Assert.That (rl.Add (http, null, null, out error), Is.True, "Add-2");
				Assert.That (error, Is.Null, "error-2");

				Assert.That (rl.Add (local, null, null, out error), Is.False, "Add-3");
				Assert.That (error.Domain, Is.EqualTo ((string) SSReadingListError.UrlSchemeNotAllowed.GetDomain ()), "Domain");
				Assert.That (error.Code, Is.EqualTo ((nint) (int) SSReadingListError.UrlSchemeNotAllowed), "Code");

				try {
					throw new NSErrorException (error);
				} catch (NSErrorException ns) {
					Assert.That (ns.Error.Code, Is.EqualTo (error.Code), "Code");
					Assert.That (ns.Error.Domain, Is.EqualTo (error.Domain), "Domain");
					Assert.That (ns.Message, Is.EqualTo (error.Description), "Message");
				} catch (Exception e) {
					Assert.Fail (e.ToString ());
				}
			}
		}

		[Test]
		public void SupportsUrl ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 7, 0, throwIfOtherPlatform: false);

			Assert.That (SSReadingList.SupportsUrl (null), Is.False, "null");

			using (var http = new NSUrl ("http://www.xamarin.com"))
				Assert.That (SSReadingList.SupportsUrl (http), Is.True, "http");

			using (var local = new NSUrl (local_file, false))
				Assert.That (SSReadingList.SupportsUrl (local), Is.False, "local");
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
