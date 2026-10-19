// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;

using Mono.Cecil;
using Mono.Cecil.Cil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class EnsureUIThreadChecksTest : TestBaseClass {
		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true", "CheckForIllegalCrossThreadCalls", "Debug")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false", "CheckForIllegalCrossThreadCalls", "Debug")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "true", "CheckForIllegalCrossThreadCalls", "Debug")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "false", "CheckForIllegalCrossThreadCalls", "Debug")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true", "CheckForEventAndDelegateMismatches", "Release")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false", "CheckForEventAndDelegateMismatches", "Debug")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "true", "CheckForEventAndDelegateMismatches", "Release")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "false", "CheckForEventAndDelegateMismatches", "Debug")]
		public void UserSpecifiedValue (ApplePlatform platform, string runtimeIdentifiers, string enabled, string property, string configuration)
		{
			var project = "EnsureUIThreadApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "SdkOnly";
			properties ["LinkMode"] = "SdkOnly";
			properties ["Configuration"] = configuration;
			properties [property] = enabled;
			// Mismatch checks must be independent of debugger support.
			if (property == "CheckForEventAndDelegateMismatches")
				properties ["DebuggerSupport"] = enabled == "true" ? "false" : "true";
			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, $"ObjCRuntime.Runtime.{property}");
			Assert.That (featureSwitch, Is.Not.Null, $"The {property} feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (enabled), "The feature switch value must match the user-specified value.");

			AssertChecks (platform, appPath, property, checksKept: enabled == "true");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "Debug", "true")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "Release", "false")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "Debug", "true")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "Release", "false")]
		public void DefaultValue (ApplePlatform platform, string runtimeIdentifiers, string configuration, string expectedValue)
		{
			// By default the UI thread checks are kept in debug builds and removed in release builds.
			var project = "EnsureUIThreadApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "SdkOnly";
			properties ["LinkMode"] = "SdkOnly";
			properties ["Configuration"] = configuration;

			var rv = DotNet.AssertBuild (project_path, properties);

			foreach (var property in new [] { "CheckForIllegalCrossThreadCalls", "CheckForEventAndDelegateMismatches" }) {
				var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, $"ObjCRuntime.Runtime.{property}");
				Assert.That (featureSwitch, Is.Not.Null, $"The {property} feature switch must be set.");
				Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (expectedValue), "The feature switch value must match the expected default.");

				AssertChecks (platform, appPath, property, checksKept: expectedValue == "true");
			}
		}

		void AssertChecks (ApplePlatform platform, string appPath, string property, bool checksKept)
		{
			if (property == "CheckForIllegalCrossThreadCalls") {
				AssertEnsureUIThreadBody (platform, appPath, checksKept);
				return;
			}

			var platformAssembly = Path.Combine (appPath, GetRelativeAssemblyDirectory (platform), Configuration.GetBaseLibraryName (platform));
			Assert.That (platformAssembly, Does.Exist, "The platform assembly must exist in the app bundle.");

			using var ad = AssemblyDefinition.ReadAssembly (platformAssembly, new ReaderParameters { ReadingMode = ReadingMode.Deferred });
			var typeName = platform == ApplePlatform.MacOSX ? "AppKit.NSApplication" : "UIKit.UIApplication";
			var type = ad.MainModule.Types.Single (v => v.FullName == typeName);
			var checkField = type.Fields.SingleOrDefault (f => f.Name == property);
			if (checksKept)
				Assert.That (checkField, Is.Not.Null, $"{typeName}.{property} field must be kept.");
			else
				Assert.That (checkField, Is.Null, $"{typeName}.{property} field must be trimmed away.");
			foreach (var methodName in new [] { "EnsureEventAndDelegateAreNotMismatched", "EnsureDelegateAssignIsNotOverwritingInternalDelegate" }) {
				var method = type.Methods.SingleOrDefault (m => m.Name == methodName);
				if (checksKept) {
					Assert.That (method, Is.Not.Null, $"{typeName}.{methodName} must be kept.");
					Assert.That (method?.Body.Instructions.Any (i => i.OpCode == OpCodes.Throw), Is.True, $"{typeName}.{methodName} must still throw.");
				} else if (method is not null) {
					Assert.That (method.Body.Instructions.All (i => i.OpCode == OpCodes.Nop || i.OpCode == OpCodes.Ret), Is.True, $"{typeName}.{methodName} must be a no-op.");
				}
			}
		}

		// Inspects the linked platform assembly in the app bundle and verifies that the UI thread check is kept
		// or removed. The public [NS|UI]Application.EnsureUIThread entry point delegates to the internal
		// ObjCRuntime.Runtime.EnsureUIThread method, which performs the actual thread check (and throw). When the
		// checks are removed, the entry point is stubbed to a no-op via ILLink substitutions, which makes
		// Runtime.EnsureUIThread unreferenced (and thus trimmed away) together with the
		// CheckForIllegalCrossThreadCalls field.
		void AssertEnsureUIThreadBody (ApplePlatform platform, string appPath, bool checksKept)
		{
			var platformAssembly = Path.Combine (appPath, GetRelativeAssemblyDirectory (platform), Configuration.GetBaseLibraryName (platform));
			Assert.That (platformAssembly, Does.Exist, "The platform assembly must exist in the app bundle.");

			using var ad = AssemblyDefinition.ReadAssembly (platformAssembly, new ReaderParameters { ReadingMode = ReadingMode.Deferred });
			var typeName = platform == ApplePlatform.MacOSX ? "AppKit.NSApplication" : "UIKit.UIApplication";
			var type = ad.MainModule.Types.Single (v => v.FullName == typeName);
			var runtimeType = ad.MainModule.Types.Single (v => v.FullName == "ObjCRuntime.Runtime");

			var hasRuntimeCheck = runtimeType.Methods.Any (m => m.Name == "EnsureUIThread" && m.Parameters.Count == 0);
			var checkField = type.Fields.SingleOrDefault (f => f.Name == "CheckForIllegalCrossThreadCalls");
			if (checksKept) {
				var runtimeMethod = runtimeType.Methods.Single (m => m.Name == "EnsureUIThread" && m.Parameters.Count == 0);
				var throwCount = runtimeMethod.Body.Instructions.Count (i => i.OpCode == OpCodes.Throw);
				Assert.That (throwCount, Is.GreaterThan (0), "The ObjCRuntime.Runtime.EnsureUIThread body must still throw when the UI thread checks are kept.");
				Assert.That (checkField, Is.Not.Null, $"The {typeName}.CheckForIllegalCrossThreadCalls field must be present when the UI thread checks are kept.");
			} else {
				Assert.That (hasRuntimeCheck, Is.False, "The ObjCRuntime.Runtime.EnsureUIThread method must be trimmed away when the UI thread checks are removed.");
				Assert.That (checkField, Is.Null, $"The {typeName}.CheckForIllegalCrossThreadCalls field must be trimmed away when the UI thread checks are removed.");
			}
		}

		// Returns the last RuntimeHostConfigurationOption item with the given name (ItemSpec) added during the build.
		static ITaskItem? GetRuntimeHostConfigurationOption (string binLogPath, string name)
		{
			ITaskItem? rv = null;
			foreach (var args in BinLog.ReadBuildEvents (binLogPath)) {
				if (args is not TaskParameterEventArgs tpea)
					continue;
				if (tpea.Kind != TaskParameterMessageKind.AddItem)
					continue;
				if (tpea.ItemType != "RuntimeHostConfigurationOption")
					continue;
				foreach (var item in tpea.Items) {
					if (item is ITaskItem taskItem && taskItem.ItemSpec == name)
						rv = taskItem;
				}
			}
			return rv;
		}
	}
}
