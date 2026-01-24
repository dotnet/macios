using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Xamarin.MacDev;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class TargetTests : TestBase {
		public TargetTests ()
			: base ("iPhoneSimulator")
		{
		}

		[Test]
		public void CleanExecutable ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we don't have a test that verifies that the Clean target works as expected, this needs to be added before we can remove this test.

			RunTarget (MonoTouchProject, TargetName.Clean);
			Assert.IsFalse (Directory.Exists (MonoTouchProjectBinPath), "#1a");
			Assert.IsFalse (Directory.Exists (MonoTouchProjectObjPath), "#1b");

			RunTarget (MonoTouchProject, TargetName.Build);
			RunTarget (MonoTouchProject, TargetName.Clean);
			Assert.IsEmpty (Directory.GetDirectories (MonoTouchProjectBinPath, "*.dSYM", SearchOption.AllDirectories), "#2a");
			Assert.IsEmpty (Directory.GetFiles (MonoTouchProjectBinPath, "*.*", SearchOption.AllDirectories), "#2b");
			Assert.IsFalse (Directory.Exists (MonoTouchProjectObjPath), "#2c");
		}

		[Test]
		public void CleanLibrary ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we don't have a test that verifies that the Clean target works as expected, this needs to be added before we can remove this test.

			RunTarget (LibraryProject, TargetName.Clean);
			Assert.IsFalse (Directory.Exists (LibraryProjectBinPath), "#1a");
			Assert.IsFalse (Directory.Exists (LibraryProjectObjPath), "#1b");

			RunTarget (LibraryProject, TargetName.Build);
			RunTarget (LibraryProject, TargetName.Clean);
			Assert.IsEmpty (Directory.GetFiles (LibraryProjectBinPath, "*.*", SearchOption.AllDirectories), "#2a");
			Assert.IsFalse (Directory.Exists (LibraryProjectObjPath), "#2b");
		}

		[Test]
		public void OptimizePngs_DefaultValue ()
		{
			OptimizePngs_Core (true);
		}

		[Test]
		public void OptimizePngs_True ()
		{
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we're currently not enabling png optimization (https://github.com/dotnet/macios/issues/20129), we need to enable that, and add a corresponding test, before we can remove this test.
			MonoTouchProjectInstance.SetProperty ("OptimizePNGs", "True");
			OptimizePngs_Core (true);
		}

		[Test]
		public void OptimizePngs_False ()
		{
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we're currently not enabling png optimization (https://github.com/dotnet/macios/issues/20129), we need to enable that, and add a corresponding test, before we can remove this test.
			MonoTouchProjectInstance.SetProperty ("OptimizePNGs", "False");
			OptimizePngs_Core (false);
		}

		void OptimizePngs_Core (bool shouldBeDifferent)
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we're currently not enabling png optimization (https://github.com/dotnet/macios/issues/20129), we need to enable that, and add a corresponding test, before we can remove this test.

			var originalFile = Path.Combine (MonoTouchProjectPath, "Resources", "image.png");
			var optimisedFile = Path.Combine (AppBundlePath, "image.png");

			RunTarget (MonoTouchProject, TargetName.Build);

			Assert.IsTrue (File.Exists (optimisedFile), "#1");
			if (shouldBeDifferent)
				CollectionAssert.AreNotEqual (File.ReadAllBytes (originalFile), File.ReadAllBytes (optimisedFile), "#2a");
			else
				CollectionAssert.AreEqual (File.ReadAllBytes (originalFile), File.ReadAllBytes (optimisedFile), "#2b");
		}
	}
}
