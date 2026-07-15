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
			Assert.That (Directory.Exists (MonoTouchProjectBinPath), Is.False, "#1a");
			Assert.That (Directory.Exists (MonoTouchProjectObjPath), Is.False, "#1b");

			RunTarget (MonoTouchProject, TargetName.Build);
			RunTarget (MonoTouchProject, TargetName.Clean);
			Assert.That (Directory.GetDirectories (MonoTouchProjectBinPath, "*.dSYM", SearchOption.AllDirectories), Is.Empty, "#2a");
			Assert.That (Directory.GetFiles (MonoTouchProjectBinPath, "*.*", SearchOption.AllDirectories), Is.Empty, "#2b");
			Assert.That (Directory.Exists (MonoTouchProjectObjPath), Is.False, "#2c");
		}

		[Test]
		public void CleanLibrary ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);
			Configuration.AssertLegacyXamarinAvailable ();
			// .NET: we don't have a test that verifies that the Clean target works as expected, this needs to be added before we can remove this test.

			RunTarget (LibraryProject, TargetName.Clean);
			Assert.That (Directory.Exists (LibraryProjectBinPath), Is.False, "#1a");
			Assert.That (Directory.Exists (LibraryProjectObjPath), Is.False, "#1b");

			RunTarget (LibraryProject, TargetName.Build);
			RunTarget (LibraryProject, TargetName.Clean);
			Assert.That (Directory.GetFiles (LibraryProjectBinPath, "*.*", SearchOption.AllDirectories), Is.Empty, "#2a");
			Assert.That (Directory.Exists (LibraryProjectObjPath), Is.False, "#2b");
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

			Assert.That (File.Exists (optimisedFile), Is.True, "#1");
			if (shouldBeDifferent)
				Assert.That (File.ReadAllBytes (optimisedFile), Is.Not.EqualTo (File.ReadAllBytes (originalFile)), "#2a");
			else
				Assert.That (File.ReadAllBytes (optimisedFile), Is.EqualTo (File.ReadAllBytes (originalFile)), "#2b");
		}
	}
}
