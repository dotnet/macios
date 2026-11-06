using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class CreateBindingResourceTaskTests : TestBase {
		CreateBindingResourcePackage ExecuteTask (string compress, bool symlinks, out string tmpdir)
		{
			tmpdir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<CreateBindingResourcePackage> ();
			task.Compress = compress;
			task.BindingResourcePath = Path.Combine (tmpdir, "CreateBindingResourceTaskTest");
			task.IntermediateOutputPath = Path.Combine (tmpdir, "IntermediateOutputPath");
			task.NativeReferences = CreateNativeReferences (tmpdir, symlinks);

			var currentDir = Environment.CurrentDirectory;
			try {
				Environment.CurrentDirectory = tmpdir;
				ExecuteTask (task);
			} finally {
				Environment.CurrentDirectory = currentDir;
			}

			return task;
		}

		[Test]
		[TestCase (true, false)]
		[TestCase (false, true)]
		[TestCase (false, false)]
		public void Compressed (bool symlinks, bool useSystemIOCompression)
		{
			var usedSystemIOCompression = Environment.GetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION");
			try {
				Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", useSystemIOCompression ? "1" : null);

				var task = ExecuteTask ("true", symlinks, out var tmpdir);

				var zipFile = task.BindingResourcePath + ".zip";
				Assert.That (zipFile, Does.Exist, "Zip existence");

				var extracted = Path.Combine (tmpdir, "Extracted");
				Extract (zipFile, extracted);
				AssertResourceDirectory (extracted, symlinks);
			} finally {
				Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", usedSystemIOCompression);
			}
		}

		[Test]
		[TestCase (true)]
		[TestCase (false)]
		public void Uncompressed (bool symlinks)
		{
			var task = ExecuteTask ("false", symlinks, out var tmpdir);

			AssertResourceDirectory (task.BindingResourcePath, symlinks);
		}

		[Test]
		[TestCase (true)]
		[TestCase (false)]
		public void Auto (bool symlinks)
		{
			var task = ExecuteTask ("auto", symlinks, out var tmpdir);

			var compressed = true;
			string extracted;
			if (compressed) {
				var zipFile = task.BindingResourcePath + ".zip";
				Assert.That (zipFile, Does.Exist, "Zip existence");

				extracted = Path.Combine (tmpdir, "Extracted");
				Extract (zipFile, extracted);
			} else {
				extracted = task.BindingResourcePath;
			}
			AssertResourceDirectory (extracted, symlinks);
		}

		void Extract (string zipArchive, string targetDirectory)
		{
			var unzipArguments = new List<string> ();
			unzipArguments.Add ("-d");
			unzipArguments.Add (targetDirectory);
			unzipArguments.Add (zipArchive);
			var rv = Execution.RunAsync ("unzip", unzipArguments).Result;
			Assert.That (rv.ExitCode, Is.EqualTo (0), "ExitCode\n" + rv.Output.MergedOutput);
		}

		[TestCase (true, false)]
		[TestCase (false, false)]
		[TestCase (false, true)]
		public void CopiesDebugSymbols (bool xcframework, bool dynamicLibrary)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var slice = Path.Combine (tmpdir, "Test.xcframework", "ios-arm64");
			var nativeName = dynamicLibrary ? "libTest.dylib" : "Test.framework";
			var symbolName = nativeName + ".dSYM";
			var framework = Path.Combine (xcframework ? slice : tmpdir, nativeName);
			var symbols = xcframework ? Path.Combine (slice, "dSYMs", "Test.framework.dSYM") : framework + ".dSYM";
			var dwarfDirectory = Path.Combine (symbols, "Contents", "Resources", "DWARF");
			Directory.CreateDirectory (dwarfDirectory);
			File.WriteAllText (Path.Combine (symbols, "Contents", "Info.plist"), "vendor plist");
			var dwarf = Path.Combine (dwarfDirectory, "Test");
			File.WriteAllText (dwarf, "vendor debug symbols");
			var outputDirectory = Path.Combine (tmpdir, "output");
			var outputSymbol = Path.Combine (outputDirectory, symbolName, "Contents", "Resources", "DWARF", "Test");

			var root = ProjectRootElement.Create (Engine.ProjectCollection);
			root.AddProperty ("_TaskAssemblyName", typeof (Ditto).Assembly.Location);
			root.AddProperty ("_PostProcess", "true");
			root.AddProperty ("IsMacEnabled", "true");
			root.AddProperty ("_OutputDsymLocation", outputDirectory + Path.DirectorySeparatorChar);
			root.AddImport (Path.Combine (Configuration.RootPath, "msbuild", "Xamarin.Shared", "Xamarin.Shared.targets"));
			root.AddTarget ("_PreparePostProcessing");
			root.AddItem ("NativeReference", framework);
			var nativePath = dynamicLibrary ? "App.app/libTest.dylib" : "App.app/Frameworks/Test.framework/Test";
			var item = root.AddItem ("_PostProcessingItem", nativePath);
			item.AddMetadata ("ItemSourcePath", dynamicLibrary ? framework : Path.Combine (framework, "Test"));
			item.AddMetadata ("dSYMSourcePath", framework + ".dSYM");
			item.AddMetadata ("NoDSymUtil", "true");
			var generated = root.AddItem ("_GenerateDSymItems", nativePath);
			generated.AddMetadata ("DSymName", dynamicLibrary ? "libTest.dSYM" : symbolName);
			generated.AddMetadata ("dSYMSourcePath", framework + ".dSYM");
			var verify = root.AddTarget ("VerifySymbols");
			verify.DependsOnTargets = "_CollectBindingResourceDebugSymbols;_CopyXCFrameworkDSyms";
			var error = verify.AddTask ("Error");
			error.Condition = $"'@(_BindingResourceDebugSymbols->Count())' != '{(xcframework ? 0 : 2)}'";
			error.SetParameter ("Text", "Binding resource inputs must include all adjacent debug symbol files.");
			error = verify.AddTask ("Error");
			error.Condition = "'@(_GenerateDSymItems)' != ''";
			error.SetParameter ("Text", "Vendor symbols must not be regenerated.");
			error = verify.AddTask ("Error");
			error.Condition = "'@(FileWrites)' == ''";
			error.SetParameter ("Text", "Copied symbols must be tracked in FileWrites.");
			var project = new Project (root);
			var logger = new Microsoft.Build.Logging.ConsoleLogger (LoggerVerbosity.Minimal);

			Assert.That (project.Build ("VerifySymbols", new [] { logger }), Is.True, "Copy vendor symbols");
			Assert.That (File.ReadAllText (outputSymbol), Is.EqualTo ("vendor debug symbols"), "Copied vendor symbols with NoDSymUtil=true");
			Assert.That (File.ReadAllText (Path.Combine (outputDirectory, symbolName, "Contents", "Info.plist")), Is.EqualTo ("vendor plist"), "Copied vendor plist");

			File.WriteAllText (dwarf, "updated vendor debug symbols");
			File.SetLastWriteTimeUtc (dwarf, File.GetLastWriteTimeUtc (outputSymbol).AddSeconds (2));
			Assert.That (project.Build ("_CopyXCFrameworkDSyms", new [] { logger }), Is.True, "Incremental copy");
			Assert.That (File.ReadAllText (outputSymbol), Is.EqualTo ("updated vendor debug symbols"), "Changed DWARF with unchanged Info.plist");

			File.Delete (outputSymbol);
			Assert.That (project.Build ("_CopyXCFrameworkDSyms", new [] { logger }), Is.True, "Missing output");
			Assert.That (File.ReadAllText (outputSymbol), Is.EqualTo ("updated vendor debug symbols"), "Restore missing DWARF");

			var app = Path.Combine (outputDirectory, "App.app");
			Directory.CreateDirectory (Path.Combine (app, "Frameworks", "Test.framework"));
			if (dynamicLibrary)
				File.WriteAllText (Path.Combine (app, nativeName), "native binary");
			File.WriteAllText (Path.Combine (app, "Info.plist"), @"<plist version=""1.0""><dict>
	<key>CFBundleIdentifier</key><string>com.example.test</string>
	<key>CFBundleName</key><string>App</string>
	<key>CFBundleShortVersionString</key><string>1.0</string>
	<key>CFBundleVersion</key><string>1</string>
</dict></plist>");
			var archive = CreateTask<Archive> ();
			archive.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			archive.AppBundleDir = new TaskItem (app);
			archive.OutputPath = outputDirectory;
			archive.ProjectName = "App";
			archive.ArchiveDir = Path.Combine (tmpdir, "App.xcarchive");
			ExecuteTask (archive);
			var archivedSymbol = Path.Combine (archive.ArchiveDir, "dSYMs", symbolName, "Contents", "Resources", "DWARF", "Test");
			Assert.That (File.ReadAllText (archivedSymbol), Is.EqualTo ("updated vendor debug symbols"), "Archived vendor symbols");
		}

		[Test]
		public void CompressedFrameworkDebugSymbols ([Values] bool hasSymbols, [Values ("direct", "true", "false")] string compressPackage)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var framework = Path.Combine (tmpdir, "Test.framework");
			Directory.CreateDirectory (framework);
			File.WriteAllText (Path.Combine (framework, "Test"), "native binary");
			var inputs = new List<string> { framework };
			if (hasSymbols) {
				var symbols = framework + ".dSYM";
				Directory.CreateDirectory (symbols);
				File.WriteAllText (Path.Combine (symbols, "symbols"), "vendor debug symbols");
				inputs.Add (symbols);
			}
			var resolve = CreateTask<ResolveNativeReferences> ();
			var zip = framework + ".zip";
			Assert.That (CompressionHelper.TryCompress (resolve, zip, inputs, false, tmpdir), Is.True, "Compress framework");
			resolve.Architectures = "arm64";
			resolve.FrameworksDirectory = "Frameworks";
			resolve.IntermediateOutputPath = Path.Combine (tmpdir, "resolved");
			resolve.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			if (compressPackage == "direct") {
				resolve.NativeReferences = new ITaskItem [] { new TaskItem (zip) };
			} else {
				var package = CreateTask<CreateBindingResourcePackage> ();
				package.Compress = compressPackage;
				package.BindingResourcePath = Path.Combine (tmpdir, "Binding.resources");
				package.IntermediateOutputPath = Path.Combine (tmpdir, "obj");
				package.NativeReferences = new ITaskItem [] { new TaskItem (zip) };
				ExecuteTask (package);
				resolve.References = new ITaskItem [] { new TaskItem (Path.Combine (tmpdir, "Binding.dll")) };
			}
			ExecuteTask (resolve);
			Assert.That (resolve.NativeFrameworks, Has.Length.EqualTo (1), "Resolved framework");
			var nativeFrameworks = resolve.NativeFrameworks ?? throw new InvalidOperationException ("No native references were resolved.");
			var extractedSymbols = Path.Combine (Path.GetDirectoryName (nativeFrameworks [0].ItemSpec) + ".dSYM", "symbols");
			if (hasSymbols)
				Assert.That (File.ReadAllText (extractedSymbols), Is.EqualTo ("vendor debug symbols"), "Extracted symbols");
			else
				Assert.That (extractedSymbols, Does.Not.Exist, "Optional symbols");
		}

		[Test]
		public void DebugSymbols ([Values ("true", "false", "auto")] string compress, [Values ("Test.framework", "libTest.dylib", "libTest.a")] string nativeReference)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var nativePath = Path.Combine (tmpdir, nativeReference);
			if (nativeReference.EndsWith (".framework", StringComparison.Ordinal)) {
				Directory.CreateDirectory (nativePath);
				File.WriteAllText (Path.Combine (nativePath, "Test"), "native binary");
			} else {
				File.WriteAllText (nativePath, "native binary");
			}
			var symbolDirectory = Path.Combine (nativePath + ".dSYM", "Contents", "Resources", "DWARF");
			Directory.CreateDirectory (symbolDirectory);
			var symbolPath = Path.Combine (symbolDirectory, "Test");
			File.WriteAllText (symbolPath, "vendor debug symbols");

			var task = CreateTask<CreateBindingResourcePackage> ();
			task.Compress = compress;
			task.BindingResourcePath = Path.Combine (tmpdir, "Binding.resources");
			task.IntermediateOutputPath = Path.Combine (tmpdir, "obj");
			task.NativeReferences = new ITaskItem [] { new TaskItem (nativePath) };
			Assert.That (task.GetAdditionalItemsToBeCopied ().Select (v => v.ItemSpec), Does.Contain (symbolPath), "Remote inputs");
			ExecuteTask (task);

			var resources = task.BindingResourcePath;
			if (compress != "false") {
				resources = Path.Combine (tmpdir, "extracted");
				Extract (task.BindingResourcePath + ".zip", resources);
			}
			var packagedSymbol = Path.Combine (resources, nativeReference + ".dSYM", "Contents", "Resources", "DWARF", "Test");
			Assert.That (File.ReadAllText (packagedSymbol), Is.EqualTo ("vendor debug symbols"), "Packaged symbols");
			Assert.That (File.ReadAllText (Path.Combine (resources, "manifest")), Does.Not.Contain (".dSYM"), "Symbols aren't native references");
			if (compress == "false")
				Assert.That (task.PackagedFiles.Select (v => v.ItemSpec), Does.Contain (packagedSymbol), "FileWrites");

			var resolve = CreateTask<ResolveNativeReferences> ();
			resolve.Architectures = "arm64";
			resolve.FrameworksDirectory = "Frameworks";
			resolve.IntermediateOutputPath = Path.Combine (tmpdir, "resolved");
			resolve.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			resolve.References = new ITaskItem [] { new TaskItem (Path.Combine (tmpdir, "Binding.dll")) };
			ExecuteTask (resolve);

			Assert.That (resolve.NativeFrameworks, Has.Length.EqualTo (1), "Resolved native references");
			var nativeFrameworks = resolve.NativeFrameworks ?? throw new InvalidOperationException ("No native references were resolved.");
			var library = nativeFrameworks [0].ItemSpec;
			var resolvedSymbols = (nativeReference.EndsWith (".framework", StringComparison.Ordinal) ? Path.GetDirectoryName (library) : library) + ".dSYM";
			var resolvedSymbol = Path.Combine (resolvedSymbols, "Contents", "Resources", "DWARF", "Test");
			Assert.That (File.ReadAllText (resolvedSymbol), Is.EqualTo ("vendor debug symbols"), "Resolved symbols");
			if (compress != "false")
				Assert.That (resolve.TouchedFiles.Select (v => v.ItemSpec), Does.Contain (resolvedSymbol), "Extracted FileWrites");
		}

		[Test]
		public void XCFrameworkDebugSymbols ([Values] bool compressPackage, [Values] bool compressXCFramework, [Values] bool simulator)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var xcframework = Path.Combine (tmpdir, "Test.xcframework");
			Directory.CreateDirectory (xcframework);
			File.WriteAllText (Path.Combine (xcframework, "Info.plist"), @"<plist version=""1.0""><dict>
	<key>CFBundlePackageType</key><string>XFWK</string>
	<key>AvailableLibraries</key><array>
		<dict>
			<key>LibraryIdentifier</key><string>ios-arm64</string>
			<key>LibraryPath</key><string>Test.framework</string>
			<key>SupportedPlatform</key><string>ios</string>
			<key>SupportedArchitectures</key><array><string>arm64</string></array>
			<key>DebugSymbolsPath</key><string>dSYMs</string>
		</dict>
		<dict>
			<key>LibraryIdentifier</key><string>ios-arm64-simulator</string>
			<key>LibraryPath</key><string>Test.framework</string>
			<key>SupportedPlatform</key><string>ios</string>
			<key>SupportedPlatformVariant</key><string>simulator</string>
			<key>SupportedArchitectures</key><array><string>arm64</string></array>
			<key>DebugSymbolsPath</key><string>dSYMs</string>
		</dict>
	</array>
</dict></plist>");
			var symbolRelativePath = Path.Combine ("dSYMs", "Test.framework.dSYM", "Contents", "Resources", "DWARF", "Test");
			foreach (var slice in new [] { "ios-arm64", "ios-arm64-simulator" }) {
				var framework = Path.Combine (xcframework, slice, "Test.framework");
				Directory.CreateDirectory (framework);
				File.WriteAllText (Path.Combine (framework, "Test"), slice);
				var symbolDirectory = Path.Combine (xcframework, slice, "dSYMs", "Test.framework.dSYM", "Contents", "Resources", "DWARF");
				Directory.CreateDirectory (symbolDirectory);
				var symbol = Path.Combine (symbolDirectory, "Test");
				File.WriteAllText (symbol, slice + " symbols");
			}

			var package = CreateTask<CreateBindingResourcePackage> ();
			package.Compress = compressPackage ? "true" : "false";
			package.BindingResourcePath = Path.Combine (tmpdir, "Binding.resources");
			package.IntermediateOutputPath = Path.Combine (tmpdir, "obj");
			if (compressXCFramework) {
				Assert.That (CompressionHelper.TryCompress (package, xcframework + ".zip", new [] { xcframework }, false, tmpdir), Is.True, "Compress XCFramework");
				xcframework += ".zip";
			}
			package.NativeReferences = new ITaskItem [] { new TaskItem (xcframework) };
			ExecuteTask (package);

			var resolve = CreateTask<ResolveNativeReferences> ();
			resolve.Architectures = "arm64";
			resolve.FrameworksDirectory = "Frameworks";
			resolve.IntermediateOutputPath = Path.Combine (tmpdir, "resolved");
			resolve.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
			resolve.SdkIsSimulator = simulator;
			resolve.References = new ITaskItem [] { new TaskItem (Path.Combine (tmpdir, "Binding.dll")) };
			ExecuteTask (resolve);

			Assert.That (resolve.NativeFrameworks, Has.Length.EqualTo (1), "Resolved native references");
			var nativeFrameworks = resolve.NativeFrameworks ?? throw new InvalidOperationException ("No native references were resolved.");
			var sliceDirectory = Path.GetDirectoryName (Path.GetDirectoryName (nativeFrameworks [0].ItemSpec)) ?? throw new InvalidOperationException ("No slice directory.");
			var expectedSlice = simulator ? "ios-arm64-simulator" : "ios-arm64";
			Assert.That (File.ReadAllText (Path.Combine (sliceDirectory, symbolRelativePath)), Is.EqualTo (expectedSlice + " symbols"), "Selected slice symbols");
			if (compressPackage && !compressXCFramework) {
				var otherSlice = simulator ? "ios-arm64" : "ios-arm64-simulator";
				Assert.That (Path.Combine (sliceDirectory, "..", otherSlice), Does.Not.Exist, "Don't extract other slices");
			}
		}

		void AssertResourceDirectory (string directory, bool symlinks)
		{
			var allFiles = Directory.GetFileSystemEntries (directory, "*", SearchOption.AllDirectories).OrderBy (v => v).Select (v => v.Substring (directory.Length + 1)).ToArray ();
			foreach (var file in allFiles)
				Console.WriteLine (file);
			if (symlinks) {
				Assert.That (allFiles.Length, Is.EqualTo (7), "Length");
			} else {
				Assert.That (allFiles.Length, Is.EqualTo (5), "Length");
			}
			Assert.That (File.ReadAllText (Path.Combine (directory, "A.txt")), Is.EqualTo ("ABCDEFGHIJKLMAAA"), "A.txt");
			Assert.That (File.ReadAllText (Path.Combine (directory, "B.txt")), Is.EqualTo ("ABCDEFGHIJKLMBBB"), "B.txt");
			Assert.That (File.ReadAllText (Path.Combine (directory, "C.framework/C.txt")), Is.EqualTo ("ABCDEFGHIJKLMCCC"), "C.txt");
			if (symlinks) {
				var linkToCPath = Path.Combine (directory, "C.framework/LinkToC.txt");
				Assert.That (File.ReadAllText (linkToCPath), Is.EqualTo ("ABCDEFGHIJKLMCCC"), "LinkToC.txt");
				Assert.That (PathUtils.IsSymlink (linkToCPath), Is.True, "LinkToC.txt - IsSymlink");
				Assert.That (PathUtils.GetSymlinkTarget (linkToCPath), Is.EqualTo ("C.txt"), "LinkToC.txt - IsSymlink target");

				var linkToNowherePath = Path.Combine (directory, "C.framework/LinkToNowhere.txt");
				Assert.Throws<FileNotFoundException> (() => File.ReadAllText (linkToNowherePath), "LinkToNowhere.txt");
				Assert.That (PathUtils.GetSymlinkTarget (linkToNowherePath), Is.EqualTo ("Nowhere.txt"), "LinkToNowhere.txt - IsSymlink target");
			}

			var manifest = @"<BindingAssembly>
	<NativeReference Name=""A.txt"">
		<ForceLoad></ForceLoad>
		<Frameworks></Frameworks>
		<IsCxx></IsCxx>
		<Kind></Kind>
		<LinkerFlags></LinkerFlags>
		<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
		<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
		<SmartLink></SmartLink>
		<WeakFrameworks></WeakFrameworks>
	</NativeReference>
	<NativeReference Name=""B.txt"">
		<ForceLoad></ForceLoad>
		<Frameworks></Frameworks>
		<IsCxx></IsCxx>
		<Kind></Kind>
		<LinkerFlags></LinkerFlags>
		<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
		<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
		<SmartLink></SmartLink>
		<WeakFrameworks></WeakFrameworks>
	</NativeReference>
	<NativeReference Name=""C.framework"">
		<ForceLoad></ForceLoad>
		<Frameworks></Frameworks>
		<IsCxx></IsCxx>
		<Kind></Kind>
		<LinkerFlags></LinkerFlags>
		<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
		<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
		<SmartLink></SmartLink>
		<WeakFrameworks></WeakFrameworks>
	</NativeReference>
</BindingAssembly>";
			Assert.That (File.ReadAllText (Path.Combine (directory, "manifest")), Is.EqualTo (manifest), "Manifest");
		}

		ITaskItem [] CreateNativeReferences (string tmpdir, bool symlinks)
		{
			var rv = new List<ITaskItem> ();

			// Full path
			var fn = Path.Combine (tmpdir, "A.txt");
			File.WriteAllText (fn, "ABCDEFGHIJKLMAAA");
			rv.Add (new TaskItem (fn));

			// Relative path
			fn = Path.Combine (tmpdir, "B.txt");
			File.WriteAllText (fn, "ABCDEFGHIJKLMBBB");
			rv.Add (new TaskItem (Path.GetFileName (fn)));

			// Directory with symlink
			var dir = Path.Combine (tmpdir, "C.framework");
			Directory.CreateDirectory (dir);
			rv.Add (new TaskItem (dir));
			File.WriteAllText (Path.Combine (dir, "C.txt"), "ABCDEFGHIJKLMCCC");
			if (symlinks) {
				PathUtils.CreateSymlink (Path.Combine (dir, "LinkToC.txt"), "C.txt");
				PathUtils.CreateSymlink (Path.Combine (dir, "LinkToNowhere.txt"), "Nowhere.txt");
			}

			return rv.ToArray ();
		}

	}
}
