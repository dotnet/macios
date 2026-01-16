using System;
using System.IO;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class DetectSdkLocationsTaskTests : TestBase {
		[Test]
		public void InvalidXamarinSdkRoot ()
		{
			Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.iOS);
			var task = CreateTask<DetectSdkLocations> ();
			task.XamarinSdkRoot = "XYZ";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			task.Execute ();

			Assert.AreEqual ("XYZ", task.XamarinSdkRoot, "#1");
		}
	}
}
