using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ProcessRuntimeLibrariesTest : TestBase {
		[TestCase ("monovm", "static")]
		[TestCase ("coreclr", "invalid")]
		[TestCase ("nativeaot", "dylib")]
		public void InvalidRuntimeOrLinkMode (string runtime, string linkMode)
		{
			var task = CreateTask<ProcessRuntimeLibraries> ();
			task.DotNetRuntime = runtime;
			task.RuntimeIdentifier = "osx-x64";
			task.RuntimeLibLinkMode = linkMode;
			task.RuntimeNuGetPackageId = "runtime.osx-x64.Microsoft.NETCore.App";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_macOS_String;

			Assert.That (task.Execute (), Is.False, "Task execution");
			Assert.That (task.Log.HasLoggedErrors, Is.True, "Invalid runtime or link mode must report an error");
		}

		[Test]
		public void CoreCLR_dylib_macOS ()
		{
			var task = CreateTask<ProcessRuntimeLibraries> ();
			task.DotNetRuntime = "coreclr";
			task.ResolvedFileToPublish = new [] {
				CreateTaskItem ("/a/b/c/libclrjit.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "osx-x64" },
					{ "NuGetPackageId", "runtime.osx-x64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libgc.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "osx-x64" },
					{ "NuGetPackageId", "runtime.osx-x64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/System.Private.CoreLib.dll", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "osx-x64" },
					{ "NuGetPackageId", "runtime.osx-x64.Microsoft.NETCore.App" },
				}),
			};
			task.RuntimeIdentifier = "osx-x64";
			task.RuntimeLibLinkMode = "dylib";
			task.RuntimeNuGetPackageId = "runtime.osx-x64.Microsoft.NETCore.App";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_macOS_String;

			Assert.That (task.Execute (), Is.True, "Task execution");
			Assert.That (task.OutputResolvedFileToPublish.Length, Is.EqualTo (3), "Output count");
			Assert.That (task.OutputResolvedFileToPublish [0].ItemSpec, Is.EqualTo ("/a/b/c/System.Private.CoreLib.dll"), "System.Private.CoreLib.dll");
			Assert.That (task.OutputResolvedFileToPublish [1].ItemSpec, Is.EqualTo ("/a/b/c/libclrjit.dylib"), "libclrjit.dylib");
			Assert.That (task.OutputResolvedFileToPublish [2].ItemSpec, Is.EqualTo ("/a/b/c/libgc.dylib"), "libgc.dylib");
		}

		[Test]
		public void CoreCLR_dylib_iOS ()
		{
			var task = CreateTask<ProcessRuntimeLibraries> ();
			task.DotNetRuntime = "coreclr";
			task.ResolvedFileToPublish = new [] {
				CreateTaskItem ("/a/b/c/libclrjit.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libgc.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/System.Private.CoreLib.dll", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
			};
			task.RuntimeIdentifier = "ios-arm64";
			task.RuntimeLibLinkMode = "dylib";
			task.RuntimeNuGetPackageId = "runtime.ios-arm64.Microsoft.NETCore.App";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;

			Assert.That (task.Execute (), Is.True, "Task execution");
			Assert.That (task.OutputResolvedFileToPublish.Length, Is.EqualTo (2), "Output count");
			Assert.That (task.OutputResolvedFileToPublish [0].ItemSpec, Is.EqualTo ("/a/b/c/System.Private.CoreLib.dll"), "System.Private.CoreLib.dll");
			Assert.That (task.OutputResolvedFileToPublish [1].ItemSpec, Is.EqualTo ("/a/b/c/libgc.dylib"), "libgc.dylib");
		}

		[Test]
		public void CoreCLR_static_iOS ()
		{
			var task = CreateTask<ProcessRuntimeLibraries> ();
			task.DotNetRuntime = "coreclr";
			task.ResolvedFileToPublish = new [] {
				CreateTaskItem ("/a/b/c/System.Private.CoreLib.dll", new Dictionary<string, string> {
					{ "AssetType", "runtime" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libgc.a", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libgc.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libhelper.a", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/libmscordaccore.dylib", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "ios-arm64" },
					{ "NuGetPackageId", "runtime.ios-arm64.Microsoft.NETCore.App" },
				}),
			};
			task.RuntimeIdentifier = "ios-arm64";
			task.RuntimeLibLinkMode = "static";
			task.RuntimeNuGetPackageId = "runtime.ios-arm64.Microsoft.NETCore.App";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;

			Assert.That (task.Execute (), Is.True, "Task execution");
			Assert.That (task.OutputResolvedFileToPublish.Select (v => v.ItemSpec), Is.EqualTo (new [] { "/a/b/c/System.Private.CoreLib.dll" }), "Only the managed assembly is copied");
			Assert.That (task.DylibsToConvertToFrameworks.Select (v => v.ItemSpec), Is.EqualTo (new [] { "/a/b/c/libgc.dylib" }), "Frameworks");
			Assert.That (task.LinkWithRuntimeLibraries.Select (v => v.ItemSpec), Is.EqualTo (new [] { "/a/b/c/libhelper.a" }), "Static libraries");
		}

		[Test]
		public void NativeAOT ()
		{
			var task = CreateTask<ProcessRuntimeLibraries> ();
			task.DotNetRuntime = "nativeaot";
			task.ResolvedFileToPublish = new [] {
				CreateTaskItem ("/a/b/c/libnativeaot.a", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "osx-x64" },
					{ "NuGetPackageId", "runtime.osx-x64.Microsoft.NETCore.App" },
				}),
				CreateTaskItem ("/a/b/c/System.Private.CoreLib.dll", new Dictionary<string, string> {
					{ "AssetType", "native" },
					{ "RuntimeIdentifier", "osx-x64" },
					{ "NuGetPackageId", "runtime.osx-x64.Microsoft.NETCore.App" },
				}),
			};
			task.RuntimeIdentifier = "osx-x64";
			task.RuntimeLibLinkMode = "static";
			task.RuntimeNuGetPackageId = "runtime.osx-x64.Microsoft.NETCore.App";
			task.TargetFrameworkMoniker = TargetFramework.DotNet_macOS_String;

			Assert.That (task.Execute (), Is.True, "Task execution");
			Assert.That (task.OutputResolvedFileToPublish.Length, Is.EqualTo (1), "Output count");
			Assert.That (task.OutputResolvedFileToPublish [0].ItemSpec, Is.EqualTo ("/a/b/c/System.Private.CoreLib.dll"), "System.Private.CoreLib.dll");
			Assert.That (task.LinkWithRuntimeLibraries, Is.Empty, "NativeAOT owns native libraries");
		}

		static ITaskItem CreateTaskItem (string path, Dictionary<string, string> metadata)
		{
			var item = new TaskItem (path);
			foreach (var kvp in metadata)
				item.SetMetadata (kvp.Key, kvp.Value);
			return item;
		}
	}
}
