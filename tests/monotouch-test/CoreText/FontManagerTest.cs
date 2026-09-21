//
// Unit tests for CTFontManager
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System.IO;
using CoreText;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using System.Linq;

namespace MonoTouchFixtures.CoreText {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontManagerTest {

		static string pacifico_ttf_path;
		static string tamarin_pdf_path;
		static string non_existent_path;

		[SetUp]
		public void SetUp ()
		{
			pacifico_ttf_path = NSBundle.MainBundle.PathForResource ("Pacifico", "ttf");
			if (!File.Exists (pacifico_ttf_path))
				Assert.Fail ($"Could not find the font file {pacifico_ttf_path}");

			tamarin_pdf_path = NSBundle.MainBundle.PathForResource ("Tamarin", "pdf");
			if (!File.Exists (tamarin_pdf_path))
				Assert.Fail ($"Could not find the PDF file {tamarin_pdf_path}");

			non_existent_path = Path.GetFullPath ("NonExistent.ttf");
			if (File.Exists (non_existent_path))
				Assert.Fail ($"This file should not exists {non_existent_path}");
		}

		[Test]
		public void RegisterTTF ()
		{
			using (var url = NSUrl.FromFilename (pacifico_ttf_path)) {
				var err = CTFontManager.RegisterFontsForUrl (url, CTFontManagerScope.Process);
				Assert.That (err, Is.Null, "err 1");
				err = CTFontManager.UnregisterFontsForUrl (url, CTFontManagerScope.Process);
				Assert.That (err, Is.Null, "err 2");
			}

			using (var url = NSUrl.FromFilename (non_existent_path)) {
				var err = CTFontManager.RegisterFontsForUrl (url, CTFontManagerScope.Process);
				// xcode 11 beta 4 stopped reporting errors
				// Assert.That (err, Is.Not.Null, "err 3");
				err = CTFontManager.UnregisterFontsForUrl (url, CTFontManagerScope.Process);
#if MONOMAC || __MACCATALYST__
				if (TestRuntime.CheckXcodeVersion (12, 2))
					Assert.That (err, Is.Not.Null, "err 4");
				else
					Assert.That (err, Is.Null, "err 4");
#else
				Assert.That (err, Is.Not.Null, "err 4");
#endif
			}
		}

		[Test]
		public void RegisterFonts_Null ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.RegisterFonts (null, CTFontManagerScope.Process, true, null), "null array");
			Assert.Throws<ArgumentException> (() => CTFontManager.RegisterFonts (new NSUrl [] { null }, CTFontManagerScope.Process, true, null), "null element");
		}

		[Test]
		public void UnregisterFonts_Null ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.UnregisterFonts (null, CTFontManagerScope.Process, null), "null array");
			Assert.Throws<ArgumentException> (() => CTFontManager.UnregisterFonts (new NSUrl [] { null }, CTFontManagerScope.Process, null), "null element");
		}

		[Test]
		public void RegisterFonts_NoCallback ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using (var url = NSUrl.FromFilename (pacifico_ttf_path)) {
				var array = new [] { url };
				CTFontManager.RegisterFonts (array, CTFontManagerScope.Process, true, null);
				CTFontManager.UnregisterFonts (array, CTFontManagerScope.Process, null);
			}

			using (var url = NSUrl.FromFilename (non_existent_path)) {
				var array = new [] { url };
				CTFontManager.RegisterFonts (array, CTFontManagerScope.Process, true, null);
				CTFontManager.UnregisterFonts (array, CTFontManagerScope.Process, null);
			}
		}

		static bool SuccessDone (NSError [] errors, bool done)
		{
			Assert.That (errors.Length, Is.EqualTo (0), "errors");
			Assert.That (done, Is.True, "done");
			return true;
		}

		static bool FailureDone (NSError [] errors, bool done)
		{
			Assert.That (errors.Length, Is.EqualTo (1), "errors");
			Assert.That (errors [0].UserInfo.TryGetValue (CTFontManagerErrorKeys.FontUrlsKey, out var urls), Is.True, "FontUrlsKey");
			Assert.That ((urls as NSArray).GetItem<NSUrl> (0).AbsoluteString.EndsWith ("NonExistent.ttf", StringComparison.Ordinal), Is.True, "NonExistent");
			Assert.That (done, Is.True, "done");
			return true;
		}

		[Test]
		public void RegisterFonts_WithCallback ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using (var url = NSUrl.FromFilename (pacifico_ttf_path)) {
				var array = new [] { url };
				CTFontManager.RegisterFonts (array, CTFontManagerScope.Process, true, SuccessDone);
				CTFontManager.UnregisterFonts (array, CTFontManagerScope.Process, SuccessDone);
			}

			// xcode 11 beta 4 stopped reporting errors
			// using (var url = NSUrl.FromFilename (non_existent_path)) {
			// 	var array = new [] { url };
			// 	CTFontManager.RegisterFonts (array, CTFontManagerScope.Process, true, FailureDone);
			// 	CTFontManager.UnregisterFonts (array, CTFontManagerScope.Process, FailureDone);
			// }
		}

		[Test]
		public void RegisterTTFs ()
		{
			using (var url = NSUrl.FromFilename (pacifico_ttf_path)) {
				var err = CTFontManager.RegisterFontsForUrl (new [] { url }, CTFontManagerScope.Process);
				Assert.That (err, Is.Null, "err 1");
				err = CTFontManager.UnregisterFontsForUrl (new [] { url }, CTFontManagerScope.Process);
				Assert.That (err, Is.Null, "err 2");
			}

			using (var url = NSUrl.FromFilename (non_existent_path)) {
				var err = CTFontManager.RegisterFontsForUrl (new [] { url }, CTFontManagerScope.Process);
				// xcode 11 beta 4 stopped reporting errors
				// Assert.That (err, Is.Not.Null, "err 3");
				// Assert.That (err.Length, Is.EqualTo (1), "err 3 l");
				// Assert.That (err [0], Is.Not.Null, "err 3[0]");
				err = CTFontManager.UnregisterFontsForUrl (new [] { url }, CTFontManagerScope.Process);
#if MONOMAC || __MACCATALYST__
				if (TestRuntime.CheckXcodeVersion (12, 2)) {
					Assert.That (err, Is.Not.Null, "err 4");
					Assert.That (err.Length, Is.EqualTo (1), "err 4 l");
					Assert.That (err [0], Is.Not.Null, "err 4[0]");
				} else
					Assert.That (err, Is.Null, "err 4");
#else
				Assert.That (err, Is.Not.Null, "err 4");
				Assert.That (err.Length, Is.EqualTo (1), "err 4 l");
				Assert.That (err [0], Is.Not.Null, "err 4[0]");
#endif
			}
		}

		[Test]
		public void RegisterFontDescriptors_Null ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.RegisterFontDescriptors (null, CTFontManagerScope.Process, true, null), "null array");
			Assert.Throws<ArgumentException> (() => CTFontManager.RegisterFontDescriptors (new CTFontDescriptor [] { null }, CTFontManagerScope.Process, true, null), "null element");
		}

		[Test]
		public void UnregisterFontDescriptors_Null ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.UnregisterFontDescriptors (null, CTFontManagerScope.Process, null), "null array");
			Assert.Throws<ArgumentException> (() => CTFontManager.UnregisterFontDescriptors (new CTFontDescriptor [] { null }, CTFontManagerScope.Process, null), "null element");
		}

		[Test]
		public void RegisterFontDescriptors_NoCallback ()
		{
			TestRuntime.AssertXcodeVersion (11, 1); // Introduced in iOS 13.0, but with a bug that makes it crash. Apple fixed it for iOS 13.1
			CTFontDescriptorAttributes fda = new CTFontDescriptorAttributes () {
				FamilyName = "Courier",
				StyleName = "Bold",
				Size = 16.0f
			};
			using (CTFontDescriptor fd = new CTFontDescriptor (fda)) {
				var array = new [] { fd };
				CTFontManager.RegisterFontDescriptors (array, CTFontManagerScope.Process, true, null);
				CTFontManager.UnregisterFontDescriptors (array, CTFontManagerScope.Process, null);
			}
		}

		[Test]
		public void RegisterFontDescriptors_WithCallback ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			CTFontDescriptorAttributes fda = new CTFontDescriptorAttributes () {
				FamilyName = "Courier",
				StyleName = "Bold",
				Size = 16.0f
			};
			Exception? ex = null;
			using (CTFontDescriptor fd = new CTFontDescriptor (fda)) {
				var array = new [] { fd };
				CTFontManager.RegisterFontDescriptors (array, CTFontManagerScope.Process, true, (NSError [] errors, bool done) => {
					try {
						Assert.That (done, Is.True, "done: RegisterFontDescriptors");
					} catch (Exception e) {
						ex = e;
					}
					return true;
				});
				Assert.That (ex, Is.Null.Or.Not.Null, "RegisterFontDescriptors");

				CTFontManager.UnregisterFontDescriptors (array, CTFontManagerScope.Process, (NSError [] errors, bool done) => {
					try {
						Assert.That (done, Is.True, "done: UnregisterFontDescriptors");
					} catch (Exception e) {
						ex = e;
					}
					return true;
				});
				Assert.That (ex, Is.Null.Or.Not.Null, "UnregisterFontDescriptors");
			}
		}

		[Test]
		public void GetFontsNullUrl ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.GetFonts (null));
		}

		[Test]
		public void GetFontsPresent ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			var url = NSUrl.FromFilename (pacifico_ttf_path);
			var err = CTFontManager.RegisterFontsForUrl (url, CTFontManagerScope.Process);
			Assert.That (err, Is.Null, "Register error");

			// method under test
			var fonts = CTFontManager.GetFonts (url);
			Assert.That (fonts.Length, Is.EqualTo (1));
			Assert.That (fonts [0].GetAttributes ().Name?.ToString (), Is.EqualTo ("Pacifico"));

			err = CTFontManager.UnregisterFontsForUrl (url, CTFontManagerScope.Process);
			Assert.That (err, Is.Null, "Unregister error");
		}

		[Test]
		public void GetFontsMissing ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			using (var url = NSUrl.FromFilename (non_existent_path)) {
				var fonts = CTFontManager.GetFonts (url);
				Assert.That (fonts.Length, Is.EqualTo (0));
			}
		}

		[Test]
		public void CreateFontDescriptor ()
		{
			Assert.Throws<ArgumentNullException> (() => CTFontManager.CreateFontDescriptor (null), "null");

			using (var data = NSData.FromFile (pacifico_ttf_path))
				Assert.That (CTFontManager.CreateFontDescriptor (data), Is.Not.Null, "font");

			using (var data = NSData.FromFile (tamarin_pdf_path))
				Assert.That (CTFontManager.CreateFontDescriptor (data), Is.Null, "not a font");
		}

		[Test]
		public void CreateFontDescriptors ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.Throws<ArgumentNullException> (() => CTFontManager.CreateFontDescriptors (null), "null");

			using (var data = NSData.FromFile (pacifico_ttf_path)) {
				var fds = CTFontManager.CreateFontDescriptors (data);
				Assert.That (fds.Length, Is.EqualTo (1), "font");
			}

			using (var data = NSData.FromFile (tamarin_pdf_path)) {
				var fds = CTFontManager.CreateFontDescriptors (data);
				Assert.That (fds.Length, Is.EqualTo (0), "not font(s)");
			}
		}

#if __IOS__ && !__MACCATALYST__
		// Running this on Mac Catalyst prints "CTFontManagerRequestFonts not implemented for plaform or scope" to the terminal
		[Test]
		public void RequestFonts ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			CTFontDescriptorAttributes fda = new CTFontDescriptorAttributes () {
				FamilyName = "Courier",
				StyleName = "Bold",
				Size = 16.0f
			};
			using (CTFontDescriptor fd = new CTFontDescriptor (fda)) {
				Assert.Throws<ArgumentNullException> (() => CTFontManager.RequestFonts (new [] { fd }, null), "null");

				var callback = false;
				CTFontManager.RequestFonts (new [] { fd }, (unresolved) => {
					Assert.That (unresolved.Length, Is.EqualTo (0), "all resolved");
					callback = true;
				});
				Assert.That (callback, Is.True, "callback");
			}
		}
#endif
	}
}
