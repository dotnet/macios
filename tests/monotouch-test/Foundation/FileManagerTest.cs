//
// Unit tests for NSFileManager
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2011 Xamarin Inc. All rights reserved.
//

using System.Diagnostics;
using System.IO;
using System.Threading;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSFileManagerTest {
		// we might believe that Envioment.UserName os the same as NSFileManager.UserName, but it is not. On the simulator for
		// example, NSFileManager.UserName is an empty string while mono returns 'somebody'
		[Test]
		public void GetUserNameTest () => Assert.That (NSFileManager.UserName, Is.Not.Null);

		[Test]
		public void GetUserFullNameTest () => Assert.That (NSFileManager.FullUserName, Is.Not.Null); // cannot check the value since it depends on the enviroment

		[Test]
		public void GetHomeDirectoryTest () => Assert.That (NSFileManager.HomeDirectory, Is.Not.Null); // cannot check the value since it depends on the enviroment

		[Test]
		public void GetHomeDirectoryForUserTest () => Assert.That (NSFileManager.GetHomeDirectory (NSFileManager.UserName), Is.EqualTo (NSFileManager.HomeDirectory));

		[Test]
		public void TemporaryDirectoryTest () => Assert.That (NSFileManager.TemporaryDirectory, Is.Not.Null); // cannot check the value since it depends on the enviroment

		[Test]
		public void GetUrlForUbiquityContainer ()
		{
			NSFileManager fm = new NSFileManager ();
			if (TestRuntime.CheckXcodeVersion (4, 5) && fm.UbiquityIdentityToken is null) {
				// UbiquityIdentityToken is a fast way to check if iCloud is enabled
				Assert.Pass ("not iCloud enabled");
			}

			NSUrl c = null;
			Exception e = null;
			ManualResetEvent evt = new ManualResetEvent (false);

			new Thread (() => {
				try {
					// From Apple's documentaiton:
					// Important: Do not call this method from your app’s main thread. Because this method might take a nontrivial amount of time to set up 
					// iCloud and return the requested URL, you should always call it from a secondary thread. 
					c = fm.GetUrlForUbiquityContainer (null);
				} catch (Exception ex) {
					e = ex;
				} finally {
					evt.Set ();
				}
			}) {
				IsBackground = true,
			}.Start ();

			if (evt.WaitOne (TimeSpan.FromSeconds (15))) {
				if (e is not null)
					throw e;

				if (c is null)
					Assert.Pass ("not iCloud enabled"); // simulator or provisioning profile without iCloud enabled (old ones)
				else {
					Assert.That (c.ToString (), Does.StartWith ("file://localhost/private/var/mobile/Library/Mobile%20Documents").
												Or.StartWith ("file:///private/var/mobile/Library/Mobile%20Documents"));
				}
			} else {
				Assert.Pass ("iCloud is probably not enabled");
				// aborting is evil, so don't bother aborting the thread, just let it run its course
			}
		}

		[Test]
		public void GetSkipBackupAttribute ()
		{
			Assert.That (NSFileManager.GetSkipBackupAttribute (NSBundle.MainBundle.ExecutableUrl.ToString ()), Is.False, "MainBundle");

			var paths = NSSearchPath.GetDirectories (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			var filename = Path.Combine (paths [0], $"DoNotBackupMe-NSFileManager-{Process.GetCurrentProcess ().Id}");
			try {
				File.WriteAllText (filename, "not worth a bit");

				Assert.That (NSFileManager.GetSkipBackupAttribute (filename), Is.False, "DoNotBackupMe-0");

				NSFileManager.SetSkipBackupAttribute (filename, true);

				NSError error;
				Assert.That (NSFileManager.GetSkipBackupAttribute (filename, out error), Is.True, "DoNotBackupMe-1");
				Assert.That (error, Is.Null, "error-1");

				error = NSFileManager.SetSkipBackupAttribute (filename, false);
				Assert.That (NSFileManager.GetSkipBackupAttribute (filename), Is.False, "DoNotBackupMe-2");
				Assert.That (error, Is.Null, "error-2");
			} finally {
				// otherwise the attribute won't reset even if the file is overwritten
				File.Delete (filename);
			}
		}

		[Test]
		public void DefaultManager ()
		{
			// ICE on devices ? ref: http://forums.xamarin.com/discussion/6807/system-invalidcastexception-while-trying-to-get-nsfilemanager-defaultmanager
			Assert.That (NSFileManager.DefaultManager, Is.Not.Null, "DefaultManager");
		}

#if !MONOMAC // DocumentsDirectory and MyDocuments point to different locations on mac
		[Test]
		[Ignore ("DocumentsDirectory and MyDocuments point to different locations on mac")]
		public void DocumentDirectory ()
		{
			var path = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].Path;
			var file = Path.Combine (path, "myfile.txt");
			try {
				// because it's read-only MyDocuments != DocumentDirectory on tvOS as the former is required by a lot of code in the BCL
				if (!TestRuntime.IsTVOS) {
					File.WriteAllText (Path.Combine (path, "myfile.txt"), "woohoo");
					Assert.That (path, Is.EqualTo (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments)), "GetFolderPath");
				}
			} finally {
				File.Delete (file);
			}
		}

		// "Environment.SpecialFolder.Resources is empty string on mac"
		[Test]
		public void LibraryDirectory ()
		{
			var path = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User) [0].Path;
			var file = Path.Combine (path, "myfile.txt");
			try {
				File.WriteAllText (Path.Combine (path, "myfile.txt"), "woohoo");
				Assert.That (path, Is.EqualTo (Environment.GetFolderPath (Environment.SpecialFolder.Resources)), "GetFolderPath");
			} catch (UnauthorizedAccessException) {
				// DocumentDirectory cannot be written to on tvOS
				if (!TestRuntime.IsTVOS)
					throw;
			} finally {
				File.Delete (file);
			}
		}
#endif
	}
}
