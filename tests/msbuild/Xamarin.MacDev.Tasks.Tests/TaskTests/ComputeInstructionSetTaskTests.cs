#nullable enable
using System;
using NUnit.Framework;
using Xamarin.MacDev.Tasks;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ComputeInstructionSetTaskTests : TestBase {
		ComputeInstructionSet CreateTask (ApplePlatform platform = ApplePlatform.iOS)
		{
			var task = CreateTask<ComputeInstructionSet> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			return task;
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "12.0", "armv8-a")]     // iPhone 6s era (iOS 12)
		[TestCase (ApplePlatform.iOS, "13.0", "armv8-a")]     // iPhone 6s era (iOS 13)
		[TestCase (ApplePlatform.iOS, "14.0", "armv8-a")]     // iPhone 6s era (iOS 14)
		[TestCase (ApplePlatform.iOS, "15.0", "armv8-a")]     // iPhone 6s era (iOS 15)
		[TestCase (ApplePlatform.iOS, "16.0", "armv8-a")]     // iPad5 (A9) can run iOS 16
		[TestCase (ApplePlatform.iOS, "17.0", "armv8-a")]     // iPad7 (A10) can run iOS 17
		[TestCase (ApplePlatform.iOS, "18.0", "armv8.3-a")]   // iPhone XR and newer (iOS 18)
		public void TestiOSInstructionSets (ApplePlatform platform, string osVersion, string expectedInstructionSet)
		{
			var task = CreateTask (platform);
			task.SupportedOSPlatformVersion = osVersion;

			ExecuteTask (task);

			Assert.That (task.InstructionSet, Is.EqualTo (expectedInstructionSet), $"Instruction set for {platform} {osVersion}");
		}

		[Test]
		[TestCase (ApplePlatform.TVOS, "12.0", "armv8-a")]     // Apple TV 4
		[TestCase (ApplePlatform.TVOS, "13.0", "armv8-a")]     // Apple TV 4
		[TestCase (ApplePlatform.TVOS, "14.0", "armv8-a")]     // Apple TV 4
		[TestCase (ApplePlatform.TVOS, "15.0", "armv8-a")]     // Apple TV 4
		[TestCase (ApplePlatform.TVOS, "16.0", "armv8.1-a")]   // Apple TV 4K and newer
		[TestCase (ApplePlatform.TVOS, "17.0", "armv8.1-a")]   // Apple TV 4K and newer
		public void TesttvOSInstructionSets (ApplePlatform platform, string osVersion, string expectedInstructionSet)
		{
			var task = CreateTask (platform);
			task.SupportedOSPlatformVersion = osVersion;

			ExecuteTask (task);

			Assert.That (task.InstructionSet, Is.EqualTo (expectedInstructionSet), $"Instruction set for {platform} {osVersion}");
		}

		[Test]
		[TestCase (ApplePlatform.MacOSX, "10.15", "osx-x64", "x86-64-v2")]
		[TestCase (ApplePlatform.MacOSX, "11.0", "osx-x64", "x86-64-v2")]
		[TestCase (ApplePlatform.MacOSX, "11.0", "osx-arm64", "apple-m1")]
		[TestCase (ApplePlatform.MacOSX, "12.0", "osx-x64", "x86-64-v2")]
		[TestCase (ApplePlatform.MacOSX, "12.0", "osx-arm64", "apple-m1")]
		[TestCase (ApplePlatform.MacCatalyst, "14.0", "maccatalyst-x64", "x86-64-v2")]
		[TestCase (ApplePlatform.MacCatalyst, "14.0", "maccatalyst-arm64", "apple-m1")]
		[TestCase (ApplePlatform.MacCatalyst, "15.0", "maccatalyst-x64", "x86-64-v2")]
		[TestCase (ApplePlatform.MacCatalyst, "15.0", "maccatalyst-arm64", "apple-m1")]
		public void TestmacOSInstructionSets (ApplePlatform platform, string osVersion, string runtimeIdentifier, string expectedInstructionSet)
		{
			var task = CreateTask (platform);
			task.SupportedOSPlatformVersion = osVersion;
			task.RuntimeIdentifier = runtimeIdentifier;

			ExecuteTask (task);

			Assert.That (task.InstructionSet, Is.EqualTo (expectedInstructionSet), $"Instruction set for {platform} {osVersion} with {runtimeIdentifier}");
		}

		[Test]
		public void TestEmptyOSVersion ()
		{
			var task = CreateTask (ApplePlatform.iOS);
			task.SupportedOSPlatformVersion = "";

			ExecuteTask (task);

			Assert.That (task.InstructionSet, Is.EqualTo (""), "Instruction set should be empty for empty OS version");
		}

		[Test]
		public void TestInvalidOSVersion ()
		{
			var task = CreateTask (ApplePlatform.iOS);
			task.SupportedOSPlatformVersion = "invalid";

			ExecuteTask (task);

			Assert.That (task.InstructionSet, Is.EqualTo (""), "Instruction set should be empty for invalid OS version");
		}

		[Test]
		public void TestVeryOldOSVersion ()
		{
			var task = CreateTask (ApplePlatform.iOS);
			task.SupportedOSPlatformVersion = "9.0";

			// iOS 9 was supported by iPhone 4s (A5), but we don't have data for that era
			// The task should handle this gracefully
			ExecuteTask (task);
		}

		[Test]
		public void TestFutureOSVersion ()
		{
			var task = CreateTask (ApplePlatform.iOS);
			task.SupportedOSPlatformVersion = "99.0";

			ExecuteTask (task);

			// For a future OS version that no current device supports, we won't compute an instruction set
			// This is expected behavior - we can't predict future instruction sets
			Assert.That (task.InstructionSet, Is.Empty, "Should not compute instruction set for future OS version");
		}
	}
}
