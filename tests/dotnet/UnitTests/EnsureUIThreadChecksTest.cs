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
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "true")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "false")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "true")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64", "false")]
		public void UserSpecifiedValue (ApplePlatform platform, string runtimeIdentifiers, string ensureUIThreadChecks)
		{
			// When the user sets $(CheckForIllegalCrossThreadCalls), the value must be passed straight through to the
			// 'ObjCRuntime.Runtime.CheckForIllegalCrossThreadCalls' trimmer feature switch (which controls whether ILLink
			// stubs the [NS|UI]Application.EnsureUIThread method body).
			var project = "EnsureUIThreadApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "SdkOnly";
			properties ["LinkMode"] = "SdkOnly";
			properties ["CheckForIllegalCrossThreadCalls"] = ensureUIThreadChecks;
			var rv = DotNet.AssertBuild (project_path, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, "ObjCRuntime.Runtime.CheckForIllegalCrossThreadCalls");
			Assert.That (featureSwitch, Is.Not.Null, "The CheckForIllegalCrossThreadCalls feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (ensureUIThreadChecks), "The feature switch value must match the user-specified value.");

			AssertEnsureUIThreadBody (platform, appPath, checksKept: ensureUIThreadChecks == "true");
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

			var featureSwitch = GetRuntimeHostConfigurationOption (rv.BinLogPath, "ObjCRuntime.Runtime.CheckForIllegalCrossThreadCalls");
			Assert.That (featureSwitch, Is.Not.Null, "The CheckForIllegalCrossThreadCalls feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (expectedValue), "The feature switch value must match the expected default.");

			AssertEnsureUIThreadBody (platform, appPath, checksKept: expectedValue == "true");
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
