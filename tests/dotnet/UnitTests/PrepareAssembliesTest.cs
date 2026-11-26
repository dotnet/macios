// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class PrepareAssembliesTest : TestBaseClass {
		[TestCase (false, false, "Target")]
		[TestCase (true, false, "Target")]
		[TestCase (true, true, "Target")]
		[TestCase (true, false, "IEmpty")]
		[TestCase (true, false, "Generic<>")]
		public void PreserveTypeInCopyUsedAssembly (bool preserve, bool allMembers, string typeName)
		{
			const ApplePlatform platform = ApplePlatform.MacOSX;
			const string runtimeIdentifier = "osx-arm64";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var tmpdir = Xamarin.Cache.CreateTemporaryDirectory ();
			var appDir = Path.Combine (tmpdir, "App");
			var targetDir = Path.Combine (tmpdir, "Target");
			Directory.CreateDirectory (appDir);
			Directory.CreateDirectory (targetDir);

			File.WriteAllText (Path.Combine (targetDir, "Target.csproj"), $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{Configuration.DotNetTfm}</TargetFramework>
		<IsTrimmable>false</IsTrimmable>
	</PropertyGroup>
</Project>");
			File.WriteAllText (Path.Combine (targetDir, "Target.cs"), @"
namespace Referenced;
public class Target {
	public int Value => 42;
}
public interface IEmpty {}
public class Generic<T> {}");

			var appProject = Path.Combine (appDir, "App.csproj");
			File.WriteAllText (appProject, $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{platform.ToFramework ()}</TargetFramework>
		<OutputType>Exe</OutputType>
		<ApplicationId>com.microsoft.preserve-copyused-test</ApplicationId>
	</PropertyGroup>
	<ItemGroup>
		<ProjectReference Include=""../Target/Target.csproj"" IsTrimmable=""false"" />
	</ItemGroup>
	<!-- IsTrimmable=false selects CopyUsed in the preparer, but the SDK currently passes copy to ILLink. -->
	<Target Name=""UseCopyUsed"" AfterTargets=""PrepareForILLink"">
		<ItemGroup>
			<ManagedAssemblyToLink Update=""@(ManagedAssemblyToLink)"" Condition=""'%(Filename)' == 'Target'"">
				<TrimMode>copyused</TrimMode>
			</ManagedAssemblyToLink>
		</ItemGroup>
	</Target>
</Project>");
			var attribute = preserve ? $"[assembly: Foundation.Preserve (typeof (Referenced.{typeName}), AllMembers = {allMembers.ToString ().ToLowerInvariant ()})]" : "";
			File.WriteAllText (Path.Combine (appDir, "Main.cs"), $@"
{attribute}
class MainClass {{
	static void Main () {{ System.Console.WriteLine (typeof (Foundation.NSObject)); }}
}}");

			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Configuration"] = "Release";
			properties ["LinkMode"] = "Full";
			var result = DotNet.AssertBuild (appProject, properties);
			AssertThatLinkerExecuted (result);

			var assemblyPath = Path.Combine (GetAppPath (appProject, platform, runtimeIdentifier, "Release"), GetRelativeAssemblyDirectory (platform), "Target.dll");
			Assert.That (File.Exists (assemblyPath), Is.EqualTo (preserve), "CopyUsed assembly retained");
			if (preserve) {
				using var target = AssemblyDefinition.ReadAssembly (assemblyPath);
				var type = target.MainModule.GetType ("Referenced.Target");
				Assert.That (type.Properties.Any (v => v.Name == "Value"), Is.True, "CopyUsed members retained");
				var preservedName = typeName == "Generic<>" ? "Generic`1" : typeName;
				Assert.That (target.MainModule.GetType ($"Referenced.{preservedName}"), Is.Not.Null, "Preserved type retained");
			}
		}

		[TestCase ("copy", true)]
		[TestCase ("copyused", false)]
		[TestCase ("copyused", true)]
		public void TypeForwarderInUntrimmedAssembly (string trimMode, bool rewrite)
		{
			const ApplePlatform platform = ApplePlatform.MacOSX;
			const string runtimeIdentifier = "osx-arm64";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var tmpdir = Xamarin.Cache.CreateTemporaryDirectory ();
			var appDir = Path.Combine (tmpdir, "App");
			var forwarderDir = Path.Combine (tmpdir, "Forwarder");
			var destinationDir = Path.Combine (tmpdir, "Destination");
			Directory.CreateDirectory (appDir);
			Directory.CreateDirectory (forwarderDir);
			Directory.CreateDirectory (destinationDir);

			File.WriteAllText (Path.Combine (destinationDir, "Destination.csproj"), $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{Configuration.DotNetTfm}</TargetFramework>
	</PropertyGroup>
</Project>");
			File.WriteAllText (Path.Combine (destinationDir, "Destination.cs"), "namespace Forwarded; public class Target {} public class Referenced {}");
			File.WriteAllText (Path.Combine (forwarderDir, "Forwarder.csproj"), $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{platform.ToFramework ()}</TargetFramework>
		<IsTrimmable>false</IsTrimmable>
	</PropertyGroup>
	<ItemGroup>
		<ProjectReference Include=""../Destination/Destination.csproj"" />
	</ItemGroup>
</Project>");
			File.WriteAllText (Path.Combine (forwarderDir, "Forwarder.cs"), @"
using System.Runtime.CompilerServices;
using Foundation;
using ObjCRuntime;

[assembly: TypeForwardedTo (typeof (Forwarded.Target))]
[assembly: TypeForwardedTo (typeof (Forwarded.Referenced))]
public class Marker {
	[Field (""NSFoundationVersionNumber"", ""Foundation"")]
	public static double Value => Dlfcn.GetDouble (0, ""NSFoundationVersionNumber"");
}");

			var appProject = Path.Combine (appDir, "App.csproj");
			File.WriteAllText (appProject, $@"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>{platform.ToFramework ()}</TargetFramework>
		<OutputType>Exe</OutputType>
		<ApplicationId>com.microsoft.forwarder-test</ApplicationId>
	</PropertyGroup>
	<ItemGroup>
		<ProjectReference Include=""../Forwarder/Forwarder.csproj"" IsTrimmable=""false"" />
	</ItemGroup>
	<Target Name=""SetForwarderPreparationMode"" BeforeTargets=""_PrepareAssembliesForPreparation"">
		<ItemGroup>
			<ResolvedFileToPublish Update=""@(ResolvedFileToPublish)"" Condition=""'%(Filename)' == 'Forwarder'"">
				<IsTrimmable>false</IsTrimmable>
				<TrimMode>{(trimMode == "copy" ? "copy" : "")}</TrimMode>
			</ResolvedFileToPublish>
		</ItemGroup>
	</Target>
	<Target Name=""SetForwarderLinkMode"" AfterTargets=""PrepareForILLink"">
		<ItemGroup>
			<ManagedAssemblyToLink Update=""@(ManagedAssemblyToLink)"" Condition=""'%(Filename)' == 'Forwarder'"">
				<TrimMode>{trimMode}</TrimMode>
			</ManagedAssemblyToLink>
		</ItemGroup>
	</Target>
</Project>");
			File.WriteAllText (Path.Combine (appDir, "Main.cs"), @"
class MainClass {
	static void Main () {
		System.Console.WriteLine (Marker.Value);
		System.Console.WriteLine (typeof (Forwarded.Target));
		System.Console.WriteLine (System.Type.GetType (""Forwarded.Referenced, Forwarder"", true));
	}
}");

			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Configuration"] = "Release";
			properties ["LinkMode"] = "Full";
			properties ["InlineDlfcnMethods"] = rewrite ? "strict" : "disabled";
			var result = DotNet.AssertBuild (appProject, properties);
			AssertThatLinkerExecuted (result);

			var preparedPath = Path.Combine (GetObjDir (appProject, platform, runtimeIdentifier, "Release"), "prepared-assemblies", "Forwarder.dll");
			Assert.That (File.Exists (preparedPath), Is.EqualTo (rewrite), "Untrimmed assembly rewritten by the preparer");
			var inputPath = Path.Combine (forwarderDir, "bin", "Release", platform.ToFramework (), "Forwarder.dll");
			using var prepared = AssemblyDefinition.ReadAssembly (rewrite ? preparedPath : inputPath);
			Assert.That (prepared.MainModule.Types.Any (v => v.Name == "Dlfcn"), Is.EqualTo (rewrite), "Dlfcn calls rewritten");
			Assert.That (prepared.MainModule.ExportedTypes.Any (v => v.FullName == "Forwarded.Target" && v.IsForwarder), Is.True, "Prepared forwarder");
			Assert.That (prepared.MainModule.ExportedTypes.Any (v => v.FullName == "Forwarded.Referenced" && v.IsForwarder), Is.True, "Prepared referenced forwarder");

			var assemblyPath = Path.Combine (GetAppPath (appProject, platform, runtimeIdentifier, "Release"), GetRelativeAssemblyDirectory (platform), "Forwarder.dll");
			using var linked = AssemblyDefinition.ReadAssembly (assemblyPath);
			// ILLink removes unreferenced forwarders from CopyUsed assemblies, whether or not they were rewritten.
			Assert.That (linked.MainModule.ExportedTypes.Any (v => v.FullName == "Forwarded.Target" && v.IsForwarder), Is.EqualTo (trimMode == "copy"), "Linked forwarder");
			Assert.That (linked.MainModule.ExportedTypes.Any (v => v.FullName == "Forwarded.Referenced" && v.IsForwarder), Is.True, "Linked referenced forwarder");
		}

		[Test]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "managed-static", "SdkOnly", "Release")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "dynamic", "None", "Debug")]
		public void IncrementalBuild (ApplePlatform platform, string runtimeIdentifiers, string registrar, string linkMode, string configuration)
		{
			// An incremental (second, no-source-change) build must not fail.
			// The PrepareAssemblies task must not run in a partial incremental build,
			// because the assembly-preparer needs the complete set of assemblies to
			// resolve inter-assembly references (otherwise it fails with MT4116/MT2362). See
			// https://github.com/dotnet/macios/issues/25938.
			var project = "MySimpleApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["MtouchLink"] = linkMode;
			properties ["Registrar"] = registrar;

			// The first (clean) build must succeed.
			DotNet.AssertBuild (project_path, properties);

			// The second (incremental) build, without any changes, must also succeed.
			var result = DotNet.AssertBuild (project_path, properties);
			var targets = BinLog.GetAllTargets (result.BinLogPath);
			AssertTargetNotExecuted (targets, "_ExecutePrepareAssemblies", "Incremental preparation");
			AssertTargetNotExecuted (targets, "_ExecutePostprocessAssemblies", "Incremental post-processing");

			var sourcePath = Path.Combine (Path.GetDirectoryName (project_path)!, "..", "AppDelegate.cs");
			Configuration.Touch (sourcePath);
			result = DotNet.AssertBuild (project_path, properties);
			targets = BinLog.GetAllTargets (result.BinLogPath);
			AssertTargetExecuted (targets, "_ExecutePrepareAssemblies", "Source change preparation");
			AssertTargetExecuted (targets, "_ExecutePostprocessAssemblies", "Source change post-processing");

			properties ["DynamicRegistrationSupported"] = "true";
			result = DotNet.AssertBuild (project_path, properties);
			targets = BinLog.GetAllTargets (result.BinLogPath);
			AssertTargetExecuted (targets, "_ExecutePrepareAssemblies", "Configuration change preparation");
			AssertTargetExecuted (targets, "_ExecutePostprocessAssemblies", "Configuration change post-processing");
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void NativeAotIncrementalBuild (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (project_path);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["PublishAot"] = "true";
			properties ["_IsPublishing"] = "true";
			properties ["Registrar"] = "trimmable-static";

			var firstBuild = DotNet.AssertBuild (project_path, properties);
			var firstBuildTargets = BinLog.GetAllTargets (firstBuild.BinLogPath);
			AssertTargetExecuted (firstBuildTargets, "_ExecutePostprocessAssembliesAfterIlc", "Post-ILC NativeAOT post-processing");

			var result = DotNet.AssertBuild (project_path, properties);
			var targets = BinLog.GetAllTargets (result.BinLogPath);
			AssertTargetNotExecuted (targets, "_ExecutePrepareAssemblies", "Incremental NativeAOT preparation");
			AssertTargetNotExecuted (targets, "_ExecutePostprocessAssembliesAfterIlc", "Incremental NativeAOT post-processing");
		}

		[TestCase ("trimmable-static", null, null, true)]
		[TestCase ("managed-static", null, null, false)]
		[TestCase ("trimmable-static", "false", null, false)]
		[TestCase ("trimmable-static", null, "true", false)]
		public void ExportAttributeRemovalEligibility (string registrar, string? trimExportAttributes, string? dynamicRegistrationSupported, bool expectedRemoval)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var project = "ExportAttributeRemovalApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Configuration"] = configuration;
			properties ["EnableAssemblyILStripping"] = "true";
			properties ["MtouchLink"] = registrar == "managed-static" ? "SdkOnly" : "Full";
			properties ["PublishReadyToRun"] = "false";
			properties ["Registrar"] = registrar;
			if (trimExportAttributes is not null)
				properties ["TrimExportAttributes"] = trimExportAttributes;
			if (dynamicRegistrationSupported is not null)
				properties ["DynamicRegistrationSupported"] = dynamicRegistrationSupported;

			var target = "Compile;_ComputePublishTrimmed;_ComputeLinkMode;_ComputeLinkerArguments;_PrepareAssemblies;_SetDynamicRegistrationSupportedFeature;_SetTrimExportAttributesFeature;_ComputeFrameworkFilesToPublish;_ComputeDynamicLibrariesToPublish;ComputeFilesToPublish;_ComputeStripAssemblyIL;_StripAssemblyIL";
			var assemblyDirectory = Path.Combine (GetObjDir (projectPath, platform, runtimeIdentifiers, configuration), "stripped");
			DotNet.AssertBuild (projectPath, properties, target: target);
			var platformAssemblyPath = Path.Combine (assemblyDirectory, Configuration.GetBaseLibraryName (platform));
			var appAssemblyPath = Path.Combine (assemblyDirectory, project + ".dll");

			AssertExportMetadata (platformAssemblyPath, appAssemblyPath, !expectedRemoval);

			if (expectedRemoval) {
				properties ["DynamicRegistrationSupported"] = "true";
				DotNet.AssertBuild (projectPath, properties, target: target);
				AssertExportMetadata (platformAssemblyPath, appAssemblyPath, true);
			}
		}

		static void AssertExportMetadata (string platformAssemblyPath, string appAssemblyPath, bool expected)
		{
			using var platformAssembly = AssemblyDefinition.ReadAssembly (platformAssemblyPath);
			var bundlePath = platformAssembly.MainModule.GetType ("Foundation.NSBundle").Properties.Single (v => v.Name == "BundlePath");
			AssertExport (bundlePath.GetMethod, expected, "direct-binding NSObject wrapper");

			var nsObjectDescription = platformAssembly.MainModule.GetType ("Foundation.NSObject").Properties.Single (v => v.Name == "Description");
			AssertExport (nsObjectDescription.GetMethod, expected, "wrapper ancestor with application subclasses");

			using var appAssembly = AssemblyDefinition.ReadAssembly (appAssemblyPath);
			var applicationType = appAssembly.MainModule.GetType ("ExportAttributeRemovalApp.ExportMetadataApplicationType");
			AssertAttribute (applicationType.Methods.Single (v => v.Name == "ApplicationExport"), "ExportAttribute", expected, "application export");
			AssertAttribute (applicationType.Methods.Single (v => v.Name == "ApplicationAction"), "ActionAttribute", expected, "application action");
			AssertAttribute (applicationType.Properties.Single (v => v.Name == "ApplicationOutlet"), "OutletAttribute", expected, "application outlet");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ExportAttributeRemovalWithNSXpcInterfaceUsage (bool explicitlyEnabled)
		{
			var platform = ApplePlatform.iOS;
			var runtimeIdentifiers = "iossimulator-arm64";
			var project = "ExportAttributeRemovalApp";
			var configuration = "Release";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out _, configuration: configuration);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["AdditionalDefineConstants"] = "EXPORT_ATTRIBUTE_REMOVAL_NSXPC";
			properties ["Configuration"] = configuration;
			properties ["MtouchLink"] = "Full";
			properties ["Registrar"] = "trimmable-static";
			properties ["DynamicRegistrationSupported"] = "false";
			if (explicitlyEnabled)
				properties ["TrimExportAttributes"] = "true";

			var target = "Compile;_ComputePublishTrimmed;_ComputeLinkMode;_ComputeLinkerArguments;_PrepareAssemblies";
			var expectedMessage = "Export attributes cannot be removed because the application uses an NSXpcInterface overload that obtains a selector from MethodInfo.";
			if (explicitlyEnabled) {
				for (var i = 0; i < 2; i++) {
					var result = DotNet.AssertBuildFailure (projectPath, properties, target: target);
					var errors = BinLog.GetBuildLogErrors (result.BinLogPath).ToArray ();
					AssertErrorMessages (errors, expectedMessage);
				}
			} else {
				var result = DotNet.AssertBuild (projectPath, properties, target: target);
				var warnings = BinLog.GetBuildLogWarnings (result.BinLogPath).FilterWarnings (platform).ToArray ();
				AssertWarningMessages (warnings, expectedMessage);
			}
		}

		static void AssertExport (ICustomAttributeProvider provider, bool expected, string message)
		{
			AssertAttribute (provider, "ExportAttribute", expected, message);
		}

		static void AssertAttribute (ICustomAttributeProvider provider, string name, bool expected, string message)
		{
			var actual = provider.CustomAttributes.Any (v => v.AttributeType.Namespace == "Foundation" && v.AttributeType.Name == name);
			Assert.That (actual, Is.EqualTo (expected), message);
		}
	}
}
