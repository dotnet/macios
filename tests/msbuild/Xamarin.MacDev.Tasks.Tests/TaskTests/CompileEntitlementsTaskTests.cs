using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

using Xamarin.MacDev;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	class CustomCompileEntitlements : CompileEntitlements {
		protected override MobileProvision GetMobileProvision (MobileProvisionPlatform platform, string uuid)
		{
			if (File.Exists (ProvisioningProfile))
				return MobileProvision.LoadFromFile (ProvisioningProfile);

			return null!;
		}
	}

	[TestFixture]
	public class CompileEntitlementsTaskTests : TestBase {
		CustomCompileEntitlements CreateEntitlementsTask (out string compiledEntitlements)
		{
			return CreateEntitlementsTask (out compiledEntitlements, out var _);
		}

		CustomCompileEntitlements CreateEntitlementsTask (string entitlementsFile, out string compiledEntitlements)
		{
			return CreateEntitlementsTask (entitlementsFile, out compiledEntitlements, out var _);
		}

		CustomCompileEntitlements CreateEntitlementsTask (out string compiledEntitlements, out string archivedEntitlements, string provisioningProfile = "profile.mobileprovision")
		{
			return CreateEntitlementsTask ("Entitlements.plist", out compiledEntitlements, out archivedEntitlements, provisioningProfile);
		}

		CustomCompileEntitlements CreateEntitlementsTask (string entitlementsFile, out string compiledEntitlements, out string archivedEntitlements, string provisioningProfile = "profile.mobileprovision")
		{
			var task = CreateTask<CustomCompileEntitlements> ();

			task.AppBundleDir = AppBundlePath;
			task.BundleIdentifier = "com.xamarin.MySingleView";
			task.CompiledEntitlements = new TaskItem (Path.Combine (MonoTouchProjectObjPath, "Entitlements.xcent"));
			task.Entitlements = GetResourcePath (entitlementsFile);
			if (!string.IsNullOrEmpty (provisioningProfile))
				task.ProvisioningProfile = GetResourcePath (provisioningProfile);
			task.SdkPlatform = "iPhoneOS";
			task.SdkVersion = "6.1";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;

			compiledEntitlements = task.CompiledEntitlements.ItemSpec;
			archivedEntitlements = Path.Combine (AppBundlePath, "archived-expanded-entitlements.xcent");

			DeleteDirectory (Path.Combine (MonoTouchProjectPath, "bin"));
			DeleteDirectory (Path.Combine (MonoTouchProjectPath, "obj"));

			return task;
		}

		string GetResourcePath (string resource)
		{
			return Path.Combine (Path.GetDirectoryName (GetType ().Assembly.Location)!, "Resources", resource);
		}

		void DeleteDirectory (string directory)
		{
			if (!Directory.Exists (directory))
				return;
			Directory.Delete (directory, true);
		}

		bool AreEntitlementValidationFailuresAnError {
			get {
				return Xamarin.Tests.Configuration.GetDotNetVersion ().Major > 10;
			}
		}

		void ExecuteAndCheckValidationErrors (Task task, params string [] expectedMessages)
		{
			IList<BuildEventArgs> buildEvents;
			var expectErrors = expectedMessages.Any (v => v.StartsWith ("error:", StringComparison.Ordinal)) || AreEntitlementValidationFailuresAnError;
			var expectedErrorCount = 0;
			var expectedWarningCount = 0;
			if (expectErrors) {
				ExecuteTask (task, expectedMessages.Length);
				buildEvents = Engine.Logger.ErrorEvents.Cast<BuildEventArgs> ().ToList ();
				expectedErrorCount = expectedMessages.Length;
			} else {
				ExecuteTask (task, 0);
				buildEvents = Engine.Logger.WarningsEvents.Cast<BuildEventArgs> ().ToList ();
				expectedWarningCount = expectedMessages.Length;
			}

			if (expectedErrorCount != Engine.Logger.ErrorEvents.Count) {
				Assert.Fail ($"Expected {expectedErrorCount} errors, but got {Engine.Logger.ErrorEvents.Count} errors.\nExpected errors:\n\t{string.Join ("\n\t", expectedMessages)}\nActual errors:\n\t{string.Join ("\n\t", Engine.Logger.ErrorEvents.Select (e => e.Message))}");
			}
			if (expectedWarningCount != Engine.Logger.WarningsEvents.Count) {
				Assert.Fail ($"Expected {expectedWarningCount} warnings, but got {Engine.Logger.WarningsEvents.Count} warnings.\nExpected warnings:\n\t{string.Join ("\n\t", expectedMessages)}\nActual warnings:\n\t{string.Join ("\n\t", Engine.Logger.WarningsEvents.Select (e => e.Message))}");
			}

			for (var i = 0; i < expectedMessages.Length; i++) {
				var expectedMessage = expectedMessages [i];
				if (expectedMessage.StartsWith ("error:", StringComparison.Ordinal))
					expectedMessage = expectedMessage [6..];
				string? expectedCode = null;
				if (expectedMessage.Length > 6 && expectedMessage [6] == ':') {
					expectedCode = expectedMessage [0..6];
					expectedMessage = expectedMessage [7..];
				}
				Assert.AreEqual (expectedMessage, buildEvents [i].Message, $"Error message #{i + 1}");
				if (expectedCode is not null) {
					string actualCode;
					if (buildEvents [i] is BuildErrorEventArgs beea) {
						actualCode = beea.Code;
					} else if (buildEvents [i] is BuildWarningEventArgs bwea) {
						actualCode = bwea.Code;
					} else {
						actualCode = "?";
					}
					Assert.That (actualCode, Is.EqualTo (expectedCode), $"Error code #{i + 1}");
				}
			}
		}

		[Test (Description = "Xambug #46298")]
		public void ValidateEntitlement ()
		{
			var task = CreateEntitlementsTask (out var compiledEntitlements, out var archivedEntitlements);
			ExecuteAndCheckValidationErrors (task,
				"The app requests the entitlement 'com.apple.developer.associated-domains', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement.",
				"The app requests the entitlement 'com.apple.developer.pass-type-identifiers', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement.",
				"The app requests the entitlement 'com.apple.developer.ubiquity-kvstore-identifier', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement.");

			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsTrue (compiled.Get<PBoolean> (EntitlementKeys.GetTaskAllow)?.Value, "#1");
			Assert.AreEqual ("32UV7A8CDE.com.xamarin.MySingleView", compiled.Get<PString> ("application-identifier")?.Value, "#2");
			Assert.AreEqual ("Z8CSQKJE7R", compiled.Get<PString> ("com.apple.developer.team-identifier")?.Value, "#3");
			Assert.AreEqual ("applinks:*.xamarin.com", compiled.GetAssociatedDomains ().ToStringArray ().First (), "#4");
			Assert.AreEqual ("Z8CSQKJE7R.*", compiled.GetPassBookIdentifiers ().ToStringArray ().First (), "#5");
			Assert.AreEqual ("Z8CSQKJE7R.com.xamarin.MySingleView", compiled.GetUbiquityKeyValueStore (), "#6");
			Assert.AreEqual ("32UV7A8CDE.com.xamarin.MySingleView", compiled.GetKeychainAccessGroups ().ToStringArray ().First (), "#7");

			var archived = PDictionary.FromFile (archivedEntitlements);
			Assert.IsTrue (compiled.ContainsKey ("application-identifier"), "archived");
		}

		[TestCase ("Invalid", null, "Unknown type 'Invalid' for the entitlement 'com.xamarin.custom.entitlement' specified in the CustomEntitlements item group. Expected 'Remove', 'Boolean', 'String', or 'StringArray'.")]
		[TestCase ("Boolean", null, "Invalid value '' for the entitlement 'com.xamarin.custom.entitlement' of type 'Boolean' specified in the CustomEntitlements item group. Expected 'true' or 'false'.")]
		[TestCase ("Boolean", "invalid", "Invalid value 'invalid' for the entitlement 'com.xamarin.custom.entitlement' of type 'Boolean' specified in the CustomEntitlements item group. Expected 'true' or 'false'.")]
		[TestCase ("Remove", "invalid", "Invalid value 'invalid' for the entitlement 'com.xamarin.custom.entitlement' of type 'Remove' specified in the CustomEntitlements item group. Expected no value at all.")]
		public void InvalidCustomEntitlements (string type, string? value, string errorMessage)
		{
			var dict = new Dictionary<string, string> {
				{ "Type", type }
			};
			if (value is not null)
				dict ["Value"] = value;

			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.xamarin.custom.entitlement", dict)
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.CustomEntitlements = customEntitlements;
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			ExecuteTask (task, expectedErrorCount: 1);
			Assert.AreEqual (errorMessage, Engine.Logger.ErrorEvents [0].Message, "Error message");
		}

		[Test]
		[TestCase ("a-string-value")]
		[TestCase ("")]
		[TestCase (null)]
		public void CustomEntitlemements_String (string value)
		{
			var dict = new Dictionary<string, string> {
				{ "Type", "String" },
				{ "Value", value },
			};
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.xamarin.custom.entitlement", dict)
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.CustomEntitlements = customEntitlements;
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.AreEqual (value ?? string.Empty, compiled.GetString ("com.xamarin.custom.entitlement")?.Value, "#1");
			Assert.IsTrue (Engine.Logger.MessageEvents.Any (v => v.Message.Contains ("The app requests the entitlement 'com.xamarin.custom.entitlement', but provisioning profile WildCardMacAppDevelopment does not grant this entitlement. This is probably not OK.")), "custom entitlement");
		}

		[Test]
		public void CustomEntitlemements_StringArray ()
		{
			var dict = new Dictionary<string, string> {
				{ "Type", "StringArray" },
				{ "Value", "A;B;C" },
			};
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.xamarin.custom.entitlement", dict)
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.InjectDefaultPlatformEntitlements = "false";
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			var array = compiled.GetArray ("com.xamarin.custom.entitlement");
			Assert.NotNull (array, "array");
			Assert.AreEqual (new string [] { "A", "B", "C" }, array.ToStringArray (), "array contents");
			Assert.IsTrue (Engine.Logger.MessageEvents.Any (v => v.Message.Contains ("The app requests the entitlement 'com.xamarin.custom.entitlement', but provisioning profile WildCardMacAppDevelopment does not grant this entitlement. This is probably not OK.")), "custom entitlement");
		}

		[Test]
		[TestCase (",")]
		[TestCase ("😁")]
		public void CustomEntitlemements_StringArray_CustomSeparator (string separator)
		{
			var dict = new Dictionary<string, string> {
				{ "Type", "StringArray" },
				{ "Value", $"A;B;C{separator}D{separator}E" },
				{ "ArraySeparator", separator },
			};
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.xamarin.custom.entitlement", dict)
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.CustomEntitlements = customEntitlements;
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			var array = compiled.GetArray ("com.xamarin.custom.entitlement");
			Assert.NotNull (array, "array");
			Assert.AreEqual (new string [] { "A;B;C", "D", "E" }, array.ToStringArray (), "array contents");
		}

		[Test]
		public void AllowJit_Default ()
		{
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsFalse (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
		}

		[Test]
		public void AllowJit_True ()
		{
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.apple.security.cs.allow-jit", new Dictionary<string, string> { {  "Type", "Boolean" }, { "Value", "true" } }),
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsTrue (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
			Assert.IsTrue (compiled.Get<PBoolean> (EntitlementKeys.AllowExecutionOfJitCode)?.Value, "#2");
		}

		[Test]
		public void AllowJit_False ()
		{
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.apple.security.cs.allow-jit", new Dictionary<string, string> { {  "Type", "Boolean" }, { "Value", "false" } }),
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements, out var archivedEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsTrue (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
			Assert.IsFalse (compiled.Get<PBoolean> (EntitlementKeys.AllowExecutionOfJitCode)?.Value, "#2");

			Assert.That (archivedEntitlements, Does.Not.Exist, "No archived entitlements");
		}

		[Test]
		public void AllowJit_None ()
		{
			var customEntitlements = new TaskItem [] {
				new TaskItem ("com.apple.security.cs.allow-jit", new Dictionary<string, string> { {  "Type", "Remove" } }),
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
			task.ProvisioningProfile = GetResourcePath ("WildCardMacAppDevelopment.provisionprofile");
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsFalse (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
		}

		[Test]
		public void AppIdentifierPrefix ()
		{
			var customEntitlements = new TaskItem [] {
				new TaskItem ("keychain-access-groups", new Dictionary<string, string> { {  "Type", "String" }, { "Value", "$(AppIdentifierPrefix)org.xamarin" } }),
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements, out var archivedEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=ios";
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsFalse (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
			var kag = ((PString?) compiled ["keychain-access-groups"])?.Value;
			Assert.That (kag, Is.EqualTo ("32UV7A8CDE.org.xamarin"), "value 1");

			var archived = PDictionary.FromFile (archivedEntitlements)!;
			Assert.IsTrue (archived.ContainsKey ("keychain-access-groups"), "archived");
			var archivedKag = ((PString?) archived ["keychain-access-groups"])?.Value;
			Assert.That (archivedKag, Is.EqualTo ("32UV7A8CDE.org.xamarin"), "archived value 1");
		}

		[Test]
		public void TeamIdentifierPrefix ()
		{
			var customEntitlements = new TaskItem [] {
				new TaskItem ("keychain-access-groups", new Dictionary<string, string> { {  "Type", "String" }, { "Value", "$(TeamIdentifierPrefix)org.xamarin" } }),
			};
			var task = CreateEntitlementsTask ("EmptyEntitlements.plist", out var compiledEntitlements, out var archivedEntitlements);
			task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=ios";
			task.CustomEntitlements = customEntitlements;
			ExecuteTask (task);
			var compiled = PDictionary.FromFile (compiledEntitlements)!;
			Assert.IsFalse (compiled.ContainsKey (EntitlementKeys.AllowExecutionOfJitCode), "#1");
			var kag = ((PString?) compiled ["keychain-access-groups"])?.Value;
			Assert.That (kag, Is.EqualTo ("Z8CSQKJE7R.org.xamarin"), "value 1");

			var archived = PDictionary.FromFile (archivedEntitlements)!;
			Assert.IsTrue (archived.ContainsKey ("keychain-access-groups"), "archived");
			var archivedKag = ((PString?) archived ["keychain-access-groups"])?.Value;
			Assert.That (archivedKag, Is.EqualTo ("Z8CSQKJE7R.org.xamarin"), "archived value 1");
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_Invalid (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("invalid", [
				"error:MT7138:Invalid value 'invalid' for the 'ValidateEntitlements' property. Valid values are: 'disable', 'warn' or 'error'."
			], mode);
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_NotInProfile_Default (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("", [
				"MT7140:The app requests the entitlement 'aps-environment', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement."
			], mode);
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_NotInProfile_Error (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("error", [
				"MT7140:The app requests the entitlement 'aps-environment', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement."
			], mode);
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_NotInProfile_Warning (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("warn", [
				"MT7140:The app requests the entitlement 'aps-environment', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement."
			], mode);
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_NoProfile_Error (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("error", [
				"MT7139:The app requests the entitlement 'aps-environment', but no provisioning profile has been specified. Please specify the name of the provisioning profile to use with the 'CodesignProvision' property in the project file."
			], mode, mobileProvision: string.Empty);
		}

		List<BuildWarningEventArgs> GetWarningsSkippingIrrelevantWarnings ()
		{
			return Engine.Logger.WarningsEvents.Where (v => {
				Console.WriteLine ($"Message: {v.Message}");
				return v.Message != ("Cannot expand $(AppIdentifierPrefix) in Entitlements.plist without a provisioning profile for key 'application-identifier' with value '$(AppIdentifierPrefix)$(CFBundleIdentifier)'.");
			}).ToList ();
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_NoProfile_Warning (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("warn", [
				"MT7139:The app requests the entitlement 'aps-environment', but no provisioning profile has been specified. Please specify the name of the provisioning profile to use with the 'CodesignProvision' property in the project file."
			], mode, mobileProvision: string.Empty);
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_MismatchedValue_Error (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("error", [
				"MT7137:The app requests the entitlement 'aps-environment' with the value 'production', but the provisioning profile 'APS Development Environment Profile' grants it for the value 'development'."
			], mode, mobileProvision: "APS_Development_Environment_Profile.mobileprovision");
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_MismatchedValue_Warning (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("warn", [
				"MT7137:The app requests the entitlement 'aps-environment' with the value 'production', but the provisioning profile 'APS Development Environment Profile' grants it for the value 'development'."
			], mode, mobileProvision: "APS_Development_Environment_Profile.mobileprovision");
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_MatchingValue_NoError (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("error", [], mode, mobileProvision: "APS_Development_Environment_Profile.mobileprovision", apsEnvironmentValue: "development");
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_MatchingValue_NoWarning (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("warn", [], mode, mobileProvision: "APS_Development_Environment_Profile.mobileprovision", apsEnvironmentValue: "development");
		}

		[Test]
		[TestCase (EntitlementsMode.InCustomEntitlements)]
		[TestCase (EntitlementsMode.InFile)]
		public void ValidateEntitlements_Disabled (EntitlementsMode mode)
		{
			ValidateEntitlementsImpl ("disable", [], mode);
		}

		[Test]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		public void ValidateEntitlementsDesktopOnly_Disable_Fail (string entitlement, EntitlementsMode mode, ApplePlatform platform)
		{
			ValidateEntitlementsImpl (platform, "disable", [], mode, entitlement: entitlement, "Boolean", "true");
		}

		[Test]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		public void ValidateEntitlementsDesktopOnly_Warn_Fail (string entitlement, EntitlementsMode mode, ApplePlatform platform)
		{
			ValidateEntitlementsImpl (platform, "warn", [
				$"MT7151:The app requests the entitlement '{entitlement}', but this entitlement is not allowed on the current platform ({platform.AsString ()}). It's only allowed on: macOS, MacCatalyst."
			], mode, entitlement: entitlement, "Boolean", "true");
		}

		[Test]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.iOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.TVOS)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.TVOS)]
		public void ValidateEntitlementsDesktopOnly_Error_Fail (string entitlement, EntitlementsMode mode, ApplePlatform platform)
		{
			ValidateEntitlementsImpl (platform, "error", [
				$"MT7151:The app requests the entitlement '{entitlement}', but this entitlement is not allowed on the current platform ({platform.AsString ()}). It's only allowed on: macOS, MacCatalyst."
			], mode, entitlement: entitlement, "Boolean", "true");
		}

		[Test]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.MacCatalyst)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.MacCatalyst)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.MacCatalyst)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.MacCatalyst)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InCustomEntitlements, ApplePlatform.MacOSX)]
		[TestCase ("com.apple.security.personal-information.location", EntitlementsMode.InFile, ApplePlatform.MacOSX)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InCustomEntitlements, ApplePlatform.MacOSX)]
		[TestCase ("com.apple.security.personal-information.calendars", EntitlementsMode.InFile, ApplePlatform.MacOSX)]
		public void ValidateEntitlementsDesktopOnly_Pass (string entitlement, EntitlementsMode mode, ApplePlatform platform)
		{
			ValidateEntitlementsImpl (platform, "disable", [], mode, entitlement, "Boolean", "true");
			ValidateEntitlementsImpl (platform, "warn", [], mode, entitlement, "Boolean", "true", mobileProvision: string.Empty);
			ValidateEntitlementsImpl (platform, "error", [], mode, entitlement, "Boolean", "true", mobileProvision: string.Empty);
		}

		[Test]
		[TestCase ("com.apple.developer.applesignin", EntitlementsMode.InCustomEntitlements, ApplePlatform.iOS)]
		[TestCase ("com.apple.developer.applesignin", EntitlementsMode.InFile, ApplePlatform.iOS)]
		public void ValidateStringArrayEntitlement (string entitlement, EntitlementsMode mode, ApplePlatform platform)
		{
			ValidateEntitlementsImpl (platform, "disable", [], mode, entitlement, "String", "Default");
			ValidateEntitlementsImpl (platform, "warn", [], mode, entitlement, "String", "Default", mobileProvision: "Apple_Signin_Test_Profile.mobileprovision");
			ValidateEntitlementsImpl (platform, "error", [], mode, entitlement, "String", "Default", mobileProvision: "Apple_Signin_Test_Profile.mobileprovision");

			ValidateEntitlementsImpl (platform, "disable", [], mode, entitlement, "String", "Default");
			ValidateEntitlementsImpl (platform, "warn", [
				"MT7140:The app requests the entitlement 'com.apple.developer.applesignin', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement."
			], mode, entitlement, "String", "Default");
			ValidateEntitlementsImpl (platform, "error", [
				"MT7140:The app requests the entitlement 'com.apple.developer.applesignin', but the provisioning profile 'iOS Team Provisioning Profile: *' doesn't contain this entitlement."
			], mode, entitlement, "String", "Default");

			// We're currently not validating the value of string array entitlements, so there are no errors for these checks
			ValidateEntitlementsImpl (platform, "disable", [], mode, entitlement, "String", "InvalidValue");
			ValidateEntitlementsImpl (platform, "warn", [], mode, entitlement, "String", "InvalidValue", mobileProvision: "Apple_Signin_Test_Profile.mobileprovision");
			ValidateEntitlementsImpl (platform, "error", [], mode, entitlement, "String", "InvalidValue", mobileProvision: "Apple_Signin_Test_Profile.mobileprovision");
		}

		public enum EntitlementsMode {
			None,
			InFile,
			InCustomEntitlements,
		}

		CompileEntitlements ValidateEntitlementsImpl (string validateEntitlements, string [] expectedErrors, EntitlementsMode entitlementsMode, string mobileProvision = "profile.mobileprovision", string apsEnvironmentValue = "production")
		{
			return ValidateEntitlementsImpl (ApplePlatform.iOS, validateEntitlements, expectedErrors, entitlementsMode, "aps-environment", "String", apsEnvironmentValue, mobileProvision);
		}

		CompileEntitlements ValidateEntitlementsImpl (ApplePlatform platform, string validateEntitlements, string [] expectedErrors, EntitlementsMode entitlementsMode, string entitlement, string type, string value, string mobileProvision = "profile.mobileprovision")
		{
			var task = CreateTask<CustomCompileEntitlements> ();
			Engine.Logger.Clear ();

			var dir = Cache.CreateTemporaryDirectory ();
			task.AppBundleDir = Path.Combine (dir, "TestApp.app");
			task.BundleIdentifier = "com.xamarin.compileentitlementstasktest";
			task.CompiledEntitlements = new TaskItem (Path.Combine (dir, "Entitlements.xcent"));
			switch (entitlementsMode) {
			case EntitlementsMode.InFile:
				var path = Path.Combine (dir, "Entitlements.plist");
				string plist;
				switch (type.ToLowerInvariant ()) {
				case "boolean":
					value = string.Equals (value, "true", StringComparison.OrdinalIgnoreCase) ? "<true />" : "<false />";
					plist = $"<plist version=\"1.0\"><dict><key>{entitlement}</key>{value}</dict></plist>";
					break;
				default:
					plist = $"<plist version=\"1.0\"><dict><key>{entitlement}</key><{type.ToLowerInvariant ()}>{value}</{type.ToLowerInvariant ()}></dict></plist>";
					break;
				}
				File.WriteAllText (path, plist);
				task.Entitlements = path;
				break;
			case EntitlementsMode.InCustomEntitlements:
				var entitlementItem = new TaskItem (entitlement);
				entitlementItem.SetMetadata ("Type", type);
				entitlementItem.SetMetadata ("Value", value);
				task.CustomEntitlements = new [] { entitlementItem };
				task.InjectDefaultPlatformEntitlements = "false";
				break;
			}
			if (!string.IsNullOrEmpty (mobileProvision))
				task.ProvisioningProfile = GetResourcePath (mobileProvision);
			switch (platform) {
			case ApplePlatform.iOS:
				task.SdkPlatform = "iPhoneOS";
				task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=ios";
				break;
			case ApplePlatform.TVOS:
				task.SdkPlatform = "AppleTVOS";
				task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=tvos";
				break;
			case ApplePlatform.MacOSX:
				task.SdkPlatform = "MacOSX";
				task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=macos";
				break;
			case ApplePlatform.MacCatalyst:
				task.SdkPlatform = "MacCatalyst";
				task.TargetFrameworkMoniker = ".NETCoreApp,Version=v6.0,Profile=maccatalyst";
				break;
			default:
				throw new NotImplementedException ();
			}
			task.ValidateEntitlements = validateEntitlements;
			ExecuteAndCheckValidationErrors (task, expectedErrors);
			return task;
		}
	}
}
