// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;

using Xamarin.Utils;
using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ResolveAppExtensionReferencesTests : TestBase {
		[Test]
		public void MixedExtensions (
			[Values (ApplePlatform.iOS, ApplePlatform.TVOS, ApplePlatform.MacOSX, ApplePlatform.MacCatalyst)] ApplePlatform platform)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var contents = platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst ? "Contents" : "";
			var task = CreateTask<ResolveAppExtensionReferences> ();
			task.AppBundlePath = Path.Combine (directory, "Container.app");
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			task.AppExtensionReferences = new [] {
				CreateExtension (directory, "Legacy", contents, "NSExtension"),
				CreateExtension (directory, "Modern", contents, "EXAppExtensionAttributes"),
				new TaskItem (Path.Combine (directory, "Service.xpc")),
			};
			task.AppExtensionReferences [1].SetMetadata ("IsAdditionalAppExtension", "true");
			Assert.That (task.GetAdditionalItemsToBeCopied ().Select (v => v.ItemSpec),
				Is.EqualTo (new [] { Path.Combine (task.AppExtensionReferences [1].ItemSpec, contents, "Info.plist") }), "Upload only native extension files");

			ExecuteTask (task);
			Assert.That (task.ResolvedAppExtensionReferences.Select (v => v.GetMetadata ("ContainerName")), Is.EqualTo (new [] { "PlugIns", "Extensions", "XPCServices" }));
			for (var i = 0; i < task.AppExtensionReferences.Length; i++) {
				var item = task.ResolvedAppExtensionReferences [i];
				Assert.That (item.ItemSpec, Is.EqualTo (task.AppExtensionReferences [i].ItemSpec), "Source path");
				Assert.That (item.GetMetadata ("BundleRelativePath"), Is.EqualTo (Path.Combine (contents, item.GetMetadata ("ContainerName")) + Path.DirectorySeparatorChar), "Bundle relative path");
				if (i < 2) {
					Assert.That (item.GetMetadata ("CodesignEntitlements"), Is.EqualTo ("custom.entitlements"), "Preserved metadata");
					Assert.That (item.GetMetadata ("ManifestPath"), Is.EqualTo (Path.Combine (item.ItemSpec, contents, "Info.plist")), "Manifest path");
					Assert.That (item.GetMetadata ("OutputFiles"), Is.EqualTo (Path.Combine (task.AppBundlePath, contents, item.GetMetadata ("ContainerName"), Path.GetFileName (item.ItemSpec), contents, "Info.plist")), "Output files");
				}
			}
		}

		[TestCase ("missing")]
		[TestCase ("unreadable")]
		[TestCase ("neither")]
		[TestCase ("both")]
		[TestCase ("wrong-type")]
		public void InvalidManifest (string kind)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var item = CreateExtension (directory, "Invalid", "", "NSExtension");
			var path = Path.Combine (item.ItemSpec, "Info.plist");
			var plist = new PDictionary ();
			switch (kind) {
			case "missing":
				File.Delete (path);
				break;
			case "unreadable":
				File.WriteAllText (path, "not a plist");
				break;
			case "both":
				plist ["NSExtension"] = new PDictionary ();
				plist ["EXAppExtensionAttributes"] = new PDictionary ();
				plist.Save (path);
				break;
			case "wrong-type":
				plist ["NSExtension"] = "not a dictionary";
				plist.Save (path);
				break;
			default:
				plist.Save (path);
				break;
			}

			var task = CreateTask<ResolveAppExtensionReferences> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.AppExtensionReferences = new [] { item };
			ExecuteTask (task, expectedErrorCount: 1);
			Assert.That (task.ResolvedAppExtensionReferences, Is.Empty);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain (path), "Error identifies the manifest");
			if (kind != "missing" && kind != "unreadable")
				Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT7236"), "Error code");
		}

		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.MacOSX)]
		public void CopyAndSignTargets (ApplePlatform platform)
		{
			if (!OperatingSystem.IsMacOS ())
				Assert.Ignore ("Copying app extensions requires ditto on macOS.");

			var directory = Cache.CreateTemporaryDirectory ();
			var contents = platform == ApplePlatform.MacOSX ? "Contents" : "";
			var legacy = CreateExtension (Path.Combine (directory, "Legacy", "build"), "Legacy", contents, "NSExtension");
			var modern = CreateExtension (Path.Combine (directory, "Modern", "build"), "Modern", contents, "NSExtension");
			var headers = Path.Combine (modern.ItemSpec, contents, "Frameworks", "Native.framework", "Headers");
			Directory.CreateDirectory (headers);
			File.WriteAllText (Path.Combine (headers, "Native.h"), "header");
			var project = Path.Combine (directory, "Placement.proj");
			File.WriteAllText (project, """
				<Project>
				  <PropertyGroup>
				    <_ComputedTargetFrameworkMoniker>.NETCoreApp,Version=v11.0,Profile=$(TestPlatform)</_ComputedTargetFrameworkMoniker>
				  </PropertyGroup>
				  <Import Project="$(Repo)/msbuild/Xamarin.Shared/Xamarin.Shared.targets" />
				  <ItemGroup>
				    <AdditionalAppExtensions Include="$(MSBuildThisFileDirectory)Legacy" Condition="'$(RemoveLegacy)' != 'true'">
				      <Name>Legacy</Name>
				      <BuildOutput>build</BuildOutput>
				      <CodesignEntitlements>legacy.entitlements</CodesignEntitlements>
				    </AdditionalAppExtensions>
				    <AdditionalAppExtensions Include="$(MSBuildThisFileDirectory)Modern">
				      <Name>Modern</Name>
				      <BuildOutput>build</BuildOutput>
				      <CodesignEntitlements>modern.entitlements</CodesignEntitlements>
				    </AdditionalAppExtensions>
				  </ItemGroup>
				  <Target Name="_ResolveAppExtensionReferences" />
				  <Target Name="_DetectSigningIdentity" />
				  <Target Name="_GenerateBundleName" />
				  <Target Name="Restore" />
				  <Target Name="Check" DependsOnTargets="_CopyAppExtensionsToBundle;_CollectAdditionalAppExtensionCodesigningData;_StripFrameworkHeaders;_RegisterEmbeddedAppExtensionFileWrites">
				    <WriteItemsToFile File="$(MSBuildThisFileDirectory)signing.items" ItemName="Bundle" Items="@(_CodesignBundle)" IncludeMetadata="true" Overwrite="true" />
				    <WriteItemsToFile File="$(MSBuildThisFileDirectory)writes.items" ItemName="File" Items="@(FileWrites)" Overwrite="true" />
				  </Target>
				</Project>
				""");
			var app = Path.Combine (directory, "Container.app");
			var properties = new Dictionary<string, string> {
				{ "Repo", Configuration.SourceRoot },
				{ "_TaskAssemblyName", typeof (ResolveAppExtensionReferences).Assembly.Location },
				{ "_TaskRuntime", "NET" },
				{ "IsMacEnabled", "true" },
				{ "TestPlatform", platform == ApplePlatform.MacOSX ? "macos" : "ios" },
				{ "_AppBundleName", "Container" },
				{ "AppBundleExtension", ".app" },
				{ "_AppBundlePath", app + Path.DirectorySeparatorChar },
				{ "AppBundleDir", app },
				{ "StripFrameworkHeaders", "true" },
				{ "_AppExtensionRoot", Path.Combine (app, contents) + Path.DirectorySeparatorChar },
				{ "_AppCodeSignatureRelativePath", string.IsNullOrEmpty (contents) ? "" : contents + Path.DirectorySeparatorChar },
				{ "DeviceSpecificIntermediateOutputPath", Path.Combine (directory, "obj") + Path.DirectorySeparatorChar },
			};
			void Build () => DotNet.AssertBuild (project, properties, target: "Check");
			Build ();
			var legacyCopy = Path.Combine (app, contents, "PlugIns", "Legacy.appex");
			var modernCopy = Path.Combine (app, contents, "PlugIns", "Modern.appex");
			Assert.That (legacyCopy, Does.Exist, "Legacy copy");
			Assert.That (modernCopy, Does.Exist, "Initial modern copy");

			var manifest = new PDictionary {
				{ "EXAppExtensionAttributes", new PDictionary () },
			};
			manifest.Save (Path.Combine (modern.ItemSpec, contents, "Info.plist"));
			Build ();
			Assert.That (modernCopy, Does.Not.Exist, "Old container cleaned");
			modernCopy = Path.Combine (app, contents, "Extensions", "Modern.appex");
			var copiedManifest = Path.Combine (modernCopy, contents, "Info.plist");
			Assert.That (copiedManifest, Does.Exist, "New container");
			var timestamp = File.GetLastWriteTimeUtc (copiedManifest);
			Build ();
			Assert.That (File.GetLastWriteTimeUtc (copiedManifest), Is.EqualTo (timestamp), "No-op build must not copy again");
			Assert.That (Path.Combine (modernCopy, contents, "Frameworks", "Native.framework", "Headers"), Does.Not.Exist, "Framework headers stripped");

			var reader = CreateTask<ReadItemsFromFile> ();
			reader.File = new [] { new TaskItem (Path.Combine (directory, "signing.items")) };
			ExecuteTask (reader);
			Assert.That (reader.Items.Select (v => v.ItemSpec.Replace ('\\', '/')), Is.EqualTo (new [] {
				Path.Combine ("Container.app", contents, "PlugIns", "Legacy.appex"),
				Path.Combine ("Container.app", contents, "Extensions", "Modern.appex"),
			}), "Signing paths match placement");
			Assert.That (reader.Items.Select (v => v.GetMetadata ("CodesignEntitlements")), Is.EqualTo (new [] { "legacy.entitlements", "modern.entitlements" }));
			Assert.That (reader.Items.Select (v => v.GetMetadata ("InputFiles")), Is.All.Empty, "Signing data excludes copy inputs");
			reader.File = new [] { new TaskItem (Path.Combine (directory, "writes.items")) };
			reader.Items = [];
			ExecuteTask (reader);
			Assert.That (reader.Items.Select (v => v.ItemSpec), Does.Contain (copiedManifest), "FileWrites contains concrete copied paths");
			Assert.That (reader.Items.Select (v => v.ItemSpec), Has.None.Contains ("*"), "FileWrites expands wildcards");

			File.Delete (copiedManifest);
			Build ();
			Assert.That (copiedManifest, Does.Exist, "Missing output restored");
			properties ["RemoveLegacy"] = "true";
			Build ();
			Assert.That (legacyCopy, Does.Not.Exist, "Removed reference cleaned");
		}

		static ITaskItem CreateExtension (string directory, string name, string contents, string key)
		{
			var path = Path.Combine (directory, name + ".appex");
			Directory.CreateDirectory (Path.Combine (path, contents));
			var plist = new PDictionary ();
			plist [key] = new PDictionary ();
			plist.Save (Path.Combine (path, contents, "Info.plist"));
			var item = new TaskItem (path);
			item.SetMetadata ("CodesignEntitlements", "custom.entitlements");
			return item;
		}
	}
}
