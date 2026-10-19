#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;

using Xamarin.MacDev.Tasks;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class DetectSigningIdentityTaskTests : TestBase {
		class ProfileLookupTask : DetectSigningIdentity {
			public IList<MobileProvision> Profiles { get; set; } = [];

			protected override IList<MobileProvision> GetMobileProvisions (MobileProvisionPlatform platform)
			{
				return Profiles;
			}

			public MobileProvision? ResolveProfile ()
			{
				return GetMobileProvision (MobileProvisionPlatform.iOS);
			}
		}

		static MobileProvision CreateProfile (string name, string uuid)
		{
			var plist = new PDictionary {
				{ "Name", new PString (name) },
				{ "UUID", new PString (uuid) },
			};
			return MobileProvision.LoadFromData (plist.ToByteArray (PropertyListFormat.Xml));
		}

		ProfileLookupTask CreateProfileLookupTask (string provisioningProfile, params MobileProvision [] profiles)
		{
			var task = CreateTask<ProfileLookupTask> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.ProvisioningProfile = provisioningProfile;
			task.Profiles = profiles;
			return task;
		}

		[Test]
		public void DuplicateProfileNames ()
		{
			var name = "VS: WildCard Development";
			var firstUuid = "11111111-1111-1111-1111-111111111111";
			var secondUuid = "22222222-2222-2222-2222-222222222222";
			var task = CreateProfileLookupTask (name, CreateProfile (name, firstUuid), CreateProfile (name, secondUuid));

			Assert.That (task.ResolveProfile (), Is.Null, "Profile");
			Assert.That (Engine.Logger.ErrorEvents, Has.Count.EqualTo (1), "Errors");
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo (
				$"Found 2 provisioning profiles matching the name '{name}'. Set the CodesignProvision property to the UUID of the intended profile instead: '{firstUuid}', '{secondUuid}'."), "Error message");
		}

		[TestCase (0)]
		[TestCase (1)]
		public void DuplicateProfileNamesWithUuid (int profileIndex)
		{
			var profiles = new [] {
				CreateProfile ("VS: WildCard Development", "11111111-1111-1111-1111-111111111111"),
				CreateProfile ("VS: WildCard Development", "22222222-2222-2222-2222-222222222222"),
			};
			var task = CreateProfileLookupTask (profiles [profileIndex].Uuid, profiles);

			Assert.That (task.ResolveProfile (), Is.SameAs (profiles [profileIndex]), "Profile");
			Assert.That (Engine.Logger.ErrorEvents, Is.Empty, "Errors");
		}

		[Test]
		public void ProfileUuidTakesPrecedenceOverName ()
		{
			var profile = CreateProfile ("Profile", "11111111-1111-1111-1111-111111111111");
			var task = CreateProfileLookupTask (profile.Uuid, CreateProfile (profile.Uuid, "22222222-2222-2222-2222-222222222222"), profile);

			Assert.That (task.ResolveProfile (), Is.SameAs (profile), "Profile");
			Assert.That (Engine.Logger.ErrorEvents, Is.Empty, "Errors");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void UniqueProfileName (bool duplicateInstallation)
		{
			var profile = CreateProfile ("Profile", "11111111-1111-1111-1111-111111111111");
			var profiles = duplicateInstallation ? new [] { profile, CreateProfile (profile.Name, profile.Uuid) } : new [] { profile };
			var task = CreateProfileLookupTask (profile.Name, profiles);

			Assert.That (task.ResolveProfile (), Is.SameAs (profile), "Profile");
			Assert.That (Engine.Logger.ErrorEvents, Is.Empty, "Errors");
		}

		[Test]
		public void ProfileNotFound ()
		{
			var task = CreateProfileLookupTask ("Missing", CreateProfile ("Other", "11111111-1111-1111-1111-111111111111"));

			Assert.That (task.ResolveProfile (), Is.Null, "Profile");
			Assert.That (Engine.Logger.ErrorEvents, Has.Count.EqualTo (1), "Errors");
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo ("The specified iOS provisioning profile 'Missing' could not be found"), "Error message");
		}

		DetectSigningIdentity CreateTask (string? tmpdir = null, ApplePlatform platform = ApplePlatform.iOS, bool simulator = true)
		{
			if (string.IsNullOrEmpty (tmpdir))
				tmpdir = Cache.CreateTemporaryDirectory ();

			var task = CreateTask<DetectSigningIdentity> ();
			task.AppBundleName = "AssemblyName";
			task.SdkIsSimulator = simulator;
			task.SdkPlatform = PlatformFrameworkHelper.GetSdkPlatform (platform, simulator);
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();

			return task;
		}

		[Test]
		public void Default ()
		{
			var dir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask (dir);

			ExecuteTask (task);

			Assert.That (task.DetectedAppId, Is.Null.Or.Empty, "DetectedAppId");
			Assert.That (task.DetectedCodeSigningKey, Is.EqualTo ("-"), "DetectedCodeSigningKey");
			Assert.That (task.DetectedCodesignAllocate, Is.EqualTo ($"{Xamarin.Tests.Configuration.XcodeLocation}/Toolchains/XcodeDefault.xctoolchain/usr/bin/codesign_allocate"), "DetectedCodesignAllocate");
			Assert.That (task.DetectedDistributionType, Is.EqualTo ("Any"), "DetectedDistributionType");
			Assert.That (task.DetectedProvisioningProfile, Is.Null.Or.Empty, "DetectedProvisioningProfile");
			Assert.That (task.HasEntitlements, Is.False, "HasEntitlements");
		}

		const string EmptyEntitlements1 = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
</dict>
</plist>";

		const string EmptyEntitlements2 = @"<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
</dict>
</plist>";

		const string EmptyEntitlements3 = @"<plist version=""1.0"">
<dict>
</dict>
</plist>";

		const string EmptyEntitlements4 = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
<!-- comment here! -->
</dict>
</plist>";

		const string NonEmptyEntitlements1 = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
    <key>com.apple.security.app-sandbox</key>
    <true/>
</dict>
</plist>";

		public class EntitlementTestCase {
			public string Name = string.Empty;
			public string Entitlements = string.Empty;
			public bool Required;
			public bool IsSimulator;
			public bool? CodesignRequireProvisioningProfile;
			public bool? ExpectedProvisioningProfile;

			public override string ToString ()
			{
				return Name;
			}
		}

		static EntitlementTestCase [] GetEntitlementsTestCases ()
		{
			return new EntitlementTestCase []
			{
				// simulator
				new EntitlementTestCase { Name = nameof (EmptyEntitlements1), Entitlements = EmptyEntitlements1, IsSimulator = true },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements2), Entitlements = EmptyEntitlements2, IsSimulator = true },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements3), Entitlements = EmptyEntitlements3, IsSimulator = true },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements4), Entitlements = EmptyEntitlements4, IsSimulator = true },
				new EntitlementTestCase { Name = nameof (NonEmptyEntitlements1), Entitlements = NonEmptyEntitlements1, IsSimulator = true, ExpectedProvisioningProfile = true },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements1) + "_Required", Entitlements = EmptyEntitlements1, IsSimulator = true, CodesignRequireProvisioningProfile = true },
				new EntitlementTestCase { Name = nameof (NonEmptyEntitlements1) + "_NotRequired", Entitlements = NonEmptyEntitlements1, IsSimulator = true, CodesignRequireProvisioningProfile = false },
				// device
				new EntitlementTestCase { Name = nameof (EmptyEntitlements1) + "_Device", Entitlements = EmptyEntitlements1, IsSimulator = false },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements2) + "_Device", Entitlements = EmptyEntitlements2, IsSimulator = false },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements3) + "_Device", Entitlements = EmptyEntitlements3, IsSimulator = false },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements4) + "_Device", Entitlements = EmptyEntitlements4, IsSimulator = false },
				new EntitlementTestCase { Name = nameof (NonEmptyEntitlements1) + "_Device", Entitlements = NonEmptyEntitlements1, IsSimulator = false },
				new EntitlementTestCase { Name = nameof (EmptyEntitlements1) + "_Required_Device", Entitlements = EmptyEntitlements1, IsSimulator = false, CodesignRequireProvisioningProfile = true },
				new EntitlementTestCase { Name = nameof (NonEmptyEntitlements1) + "_NotRequired_Device", Entitlements = NonEmptyEntitlements1, IsSimulator = false, CodesignRequireProvisioningProfile = false, ExpectedProvisioningProfile = false },
			};
		}

		[Test]
		[TestCaseSource (nameof (GetEntitlementsTestCases))]
		public void EmptyEntitlements (EntitlementTestCase testCase)
		{
			var dir = Cache.CreateTemporaryDirectory ();
			var entitlementsPath = Path.Combine (dir, "Entitlements.plist");
			File.WriteAllText (entitlementsPath, testCase.Entitlements);

			var task = CreateTask (dir, simulator: testCase.IsSimulator);
			task.BundleIdentifier = "com.tests.emptyentitlements";
			task.CodesignEntitlements = new TaskItem (entitlementsPath);
			task.SdkIsSimulator = testCase.IsSimulator;
			if (testCase.CodesignRequireProvisioningProfile.HasValue)
				task.CodesignRequireProvisioningProfile = testCase.CodesignRequireProvisioningProfile.Value.ToString ();

			ExecuteTask (task);

			bool requiresProvisioningProfile;
			if (testCase.ExpectedProvisioningProfile.HasValue) {
				requiresProvisioningProfile = testCase.ExpectedProvisioningProfile.Value;
			} else {
				requiresProvisioningProfile = testCase.CodesignRequireProvisioningProfile == true || !testCase.IsSimulator;
			}

			if (requiresProvisioningProfile) {
				Assert.That (task.DetectedAppId, Does.EndWith (".com.tests.emptyentitlements"), "DetectedAppId");
				Assert.That (task.DetectedProvisioningProfile, Is.Not.Null.And.Not.Empty, "DetectedProvisioningProfile");
			} else {
				Assert.That (task.DetectedAppId, Is.EqualTo ("com.tests.emptyentitlements"), "DetectedAppId");
				Assert.That (task.DetectedProvisioningProfile, Is.Null.Or.Empty, "DetectedProvisioningProfile");
			}
			if (testCase.IsSimulator || !requiresProvisioningProfile) {
				Assert.That (task.DetectedCodeSigningKey, Is.EqualTo ("-"), "DetectedCodeSigningKey");
				Assert.That (task.DetectedDistributionType, Is.EqualTo ("Any"), "DetectedDistributionType");
			} else {
				Assert.That (task.DetectedCodeSigningKey, Has.Length.EqualTo ("20D63576DE3EA7BE419C18997CF948D759B43D53".Length), "DetectedCodeSigningKey");
				Assert.That (task.DetectedDistributionType, Is.EqualTo ("Development").Or.EqualTo ("AppStore").Or.EqualTo ("Any"), "DetectedDistributionType");
			}
			Assert.That (task.DetectedCodesignAllocate, Is.EqualTo ($"{Xamarin.Tests.Configuration.XcodeLocation}/Toolchains/XcodeDefault.xctoolchain/usr/bin/codesign_allocate"), "DetectedCodesignAllocate");
		}

		[Test]
		public void CustomEntitlements ()
		{
			var dir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask (dir);
			task.CustomEntitlements = new ITaskItem [] { new TaskItem ("keychain-access-group") };
			ExecuteTask (task);

			Assert.That (task.DetectedAppId, Is.Not.Null.And.Not.Empty, "DetectedAppId");
			Assert.That (task.DetectedCodeSigningKey, Is.EqualTo ("-"), "DetectedCodeSigningKey");
			Assert.That (task.DetectedCodesignAllocate, Is.EqualTo ($"{Xamarin.Tests.Configuration.XcodeLocation}/Toolchains/XcodeDefault.xctoolchain/usr/bin/codesign_allocate"), "DetectedCodesignAllocate");
			Assert.That (task.DetectedDistributionType, Is.EqualTo ("Any"), "DetectedDistributionType");
			Assert.That (task.DetectedProvisioningProfile, Is.Not.Null.And.Not.Empty, "DetectedProvisioningProfile");
			Assert.That (task.HasEntitlements, Is.True, "HasEntitlements");
		}

		[Test]
		public void Simulator ()
		{
			var dir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask (dir);
			task.BundleIdentifier = "com.xamarin.simulatortest";
			task.SdkIsSimulator = true;
			task.CustomEntitlements = new ITaskItem [] { new TaskItem ("keychain-access-group") };
			ExecuteTask (task);

			Assert.Multiple (() => {
				Assert.That (task.DetectedAppId, Does.EndWith ("." + task.BundleIdentifier), "DetectedAppId");
				Assert.That (task.DetectedCodeSigningKey, Is.EqualTo ("-"), "DetectedCodeSigningKey");
				Assert.That (task.DetectedCodesignAllocate, Is.EqualTo ($"{Xamarin.Tests.Configuration.XcodeLocation}/Toolchains/XcodeDefault.xctoolchain/usr/bin/codesign_allocate"), "DetectedCodesignAllocate");
				Assert.That (task.DetectedDistributionType, Is.EqualTo ("Any"), "DetectedDistributionType");
				Assert.That (task.DetectedProvisioningProfile, Is.Not.Null.And.Not.Empty, "DetectedProvisioningProfile");
				Assert.That (task.HasEntitlements, Is.True, "HasEntitlements");
			});
		}
	}
}
