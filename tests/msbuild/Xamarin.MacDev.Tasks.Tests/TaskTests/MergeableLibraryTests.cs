#nullable enable

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin;
using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class MergeableLibraryTests : TestBase {

		static byte [] CreateMinimalMachODylib ()
		{
			var header = new byte [] {
				// Mach-O magic number for 64-bit (MH_MAGIC_64)
				0xCF, 0xFA, 0xED, 0xFE,
				// CPU type (CPU_TYPE_X86_64 = 0x01000007)
				0x07, 0x00, 0x00, 0x01,
				// CPU subtype
				0x03, 0x00, 0x00, 0x00,
				// File type (MH_DYLIB = 6)
				0x06, 0x00, 0x00, 0x00,
				// Number of load commands
				0x00, 0x00, 0x00, 0x00,
				// Size of load commands
				0x00, 0x00, 0x00, 0x00,
				// Flags
				0x00, 0x00, 0x00, 0x00,
				// Reserved (64-bit only)
				0x00, 0x00, 0x00, 0x00
			};
			return header;
		}

		static byte [] CreateMinimalMergeableDylib ()
		{
			// A Mach-O dylib with a single LC_ATOM_INFO load command (linkedit_data_command)
			var header = new byte [] {
				// Mach-O magic number for 64-bit (MH_MAGIC_64)
				0xCF, 0xFA, 0xED, 0xFE,
				// CPU type (CPU_TYPE_X86_64 = 0x01000007)
				0x07, 0x00, 0x00, 0x01,
				// CPU subtype
				0x03, 0x00, 0x00, 0x00,
				// File type (MH_DYLIB = 6)
				0x06, 0x00, 0x00, 0x00,
				// Number of load commands = 1
				0x01, 0x00, 0x00, 0x00,
				// Size of load commands = 16 (sizeof linkedit_data_command)
				0x10, 0x00, 0x00, 0x00,
				// Flags
				0x00, 0x00, 0x00, 0x00,
				// Reserved (64-bit only)
				0x00, 0x00, 0x00, 0x00,
				// LC_ATOM_INFO load command (linkedit_data_command)
				// cmd = 0x36 (LC_ATOM_INFO)
				0x36, 0x00, 0x00, 0x00,
				// cmdsize = 16
				0x10, 0x00, 0x00, 0x00,
				// dataoff = 0 (offset to atom data)
				0x00, 0x00, 0x00, 0x00,
				// datasize = 0 (size of atom data)
				0x00, 0x00, 0x00, 0x00,
			};
			return header;
		}

		static byte [] CreateMinimalStaticLib ()
		{
			// A minimal MH_OBJECT file (not a dylib)
			var header = new byte [] {
				// Mach-O magic number for 64-bit (MH_MAGIC_64)
				0xCF, 0xFA, 0xED, 0xFE,
				// CPU type (CPU_TYPE_X86_64 = 0x01000007)
				0x07, 0x00, 0x00, 0x01,
				// CPU subtype
				0x03, 0x00, 0x00, 0x00,
				// File type (MH_OBJECT = 1)
				0x01, 0x00, 0x00, 0x00,
				// Number of load commands
				0x00, 0x00, 0x00, 0x00,
				// Size of load commands
				0x00, 0x00, 0x00, 0x00,
				// Flags
				0x00, 0x00, 0x00, 0x00,
				// Reserved (64-bit only)
				0x00, 0x00, 0x00, 0x00
			};
			return header;
		}

		[Test]
		public void NonMergeableDylib_IsNotMergeable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var dylibPath = Path.Combine (tempDir, "test.dylib");
			File.WriteAllBytes (dylibPath, CreateMinimalMachODylib ());
			Assert.That (MachO.IsMergeableLibrary (dylibPath), Is.False, "Non-mergeable dylib should not be detected as mergeable");
		}

		[Test]
		public void MergeableDylib_IsMergeable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var dylibPath = Path.Combine (tempDir, "test_mergeable.dylib");
			File.WriteAllBytes (dylibPath, CreateMinimalMergeableDylib ());
			Assert.That (MachO.IsMergeableLibrary (dylibPath), Is.True, "Mergeable dylib should be detected as mergeable");
		}

		[Test]
		public void MergeableDylib_IsDynamicFramework ()
		{
			// A mergeable dylib is still a dynamic library
			var tempDir = Cache.CreateTemporaryDirectory ();
			var dylibPath = Path.Combine (tempDir, "test_mergeable.dylib");
			File.WriteAllBytes (dylibPath, CreateMinimalMergeableDylib ());
			Assert.That (MachO.IsDynamicFramework (dylibPath), Is.True, "Mergeable dylib should still be detected as a dynamic framework");
		}

		[Test]
		public void ObjectFile_IsNotMergeable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var objPath = Path.Combine (tempDir, "test.o");
			File.WriteAllBytes (objPath, CreateMinimalStaticLib ());
			Assert.That (MachO.IsMergeableLibrary (objPath), Is.False, "Object file should not be detected as mergeable");
		}

		[Test]
		public void MergeableFramework_IsMergeable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var frameworkDir = Path.Combine (tempDir, "TestMergeable.framework");
			Directory.CreateDirectory (frameworkDir);
			var executablePath = Path.Combine (frameworkDir, "TestMergeable");
			File.WriteAllBytes (executablePath, CreateMinimalMergeableDylib ());
			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.True, "Mergeable framework executable should be detected as mergeable");
		}

		[Test]
		public void NonMergeableFramework_IsNotMergeable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var frameworkDir = Path.Combine (tempDir, "TestNonMergeable.framework");
			Directory.CreateDirectory (frameworkDir);
			var executablePath = Path.Combine (frameworkDir, "TestNonMergeable");
			File.WriteAllBytes (executablePath, CreateMinimalMachODylib ());
			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.False, "Non-mergeable framework executable should not be detected as mergeable");
		}

		[Test]
		public void HasAtomInfo_Property ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();

			var mergeablePath = Path.Combine (tempDir, "mergeable.dylib");
			File.WriteAllBytes (mergeablePath, CreateMinimalMergeableDylib ());
			var mergeableFiles = MachO.Read (mergeablePath);
			foreach (var mf in mergeableFiles)
				Assert.That (mf.HasAtomInfo, Is.True, "Mergeable MachOFile should have atom info");

			var normalPath = Path.Combine (tempDir, "normal.dylib");
			File.WriteAllBytes (normalPath, CreateMinimalMachODylib ());
			var normalFiles = MachO.Read (normalPath);
			foreach (var mf in normalFiles)
				Assert.That (mf.HasAtomInfo, Is.False, "Normal MachOFile should not have atom info");
		}

		static void RunProcess (string filename, IList<string> arguments)
		{
			var rv = ExecutionHelper.Execute (filename, arguments, out var output, null, TimeSpan.FromSeconds (30));
			if (rv != 0)
				Assert.Fail ($"Process '{filename} {string.Join (" ", arguments)}' failed with exit code {rv}.\nOutput: {output}");
		}

		[Test]
		public void RealMergeableDylib_DetectedAndStrippable ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var sourcePath = Path.Combine (tempDir, "test.c");
			File.WriteAllText (sourcePath, "int mergeable_test_func (int a, int b) { return a + b; }");

			// Build a normal dylib and a mergeable dylib
			var normalDylib = Path.Combine (tempDir, "normal.dylib");
			var mergeableDylib = Path.Combine (tempDir, "mergeable.dylib");

			RunProcess ("xcrun", new [] { "clang", "-dynamiclib", "-o", normalDylib, sourcePath, "-arch", "arm64" });
			RunProcess ("xcrun", new [] { "clang", "-dynamiclib", "-o", mergeableDylib, sourcePath, "-arch", "arm64", "-Wl,-make_mergeable" });

			// Verify detection
			Assert.That (MachO.IsMergeableLibrary (normalDylib), Is.False, "Normal dylib should not be mergeable");
			Assert.That (MachO.IsMergeableLibrary (mergeableDylib), Is.True, "Mergeable dylib should be mergeable");

			// Both should be detected as dynamic
			Assert.That (MachO.IsDynamicFramework (normalDylib), Is.True, "Normal dylib should be dynamic");
			Assert.That (MachO.IsDynamicFramework (mergeableDylib), Is.True, "Mergeable dylib should be dynamic");

			// Strip atom info and verify
			var strippedDylib = Path.Combine (tempDir, "stripped.dylib");
			File.Copy (mergeableDylib, strippedDylib);
			RunProcess ("xcrun", new [] { "strip", "-no_atom_info", "-S", strippedDylib });

			Assert.That (MachO.IsMergeableLibrary (strippedDylib), Is.False, "Stripped dylib should not be mergeable");
			Assert.That (MachO.IsDynamicFramework (strippedDylib), Is.True, "Stripped dylib should still be dynamic");

			// Verify size reduction
			var mergeableSize = new FileInfo (mergeableDylib).Length;
			var strippedSize = new FileInfo (strippedDylib).Length;
			Assert.That (strippedSize, Is.LessThan (mergeableSize), "Stripped dylib should be smaller than mergeable dylib");
		}

		[Test]
		public void SymbolStrip_StripsAtomInfoFromFramework ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var sourcePath = Path.Combine (tempDir, "test.c");
			File.WriteAllText (sourcePath, "int mergeable_test_func (int a, int b) { return a + b; }");

			// Create a mergeable framework
			var frameworkDir = Path.Combine (tempDir, "TestMergeable.framework");
			Directory.CreateDirectory (frameworkDir);
			var executablePath = Path.Combine (frameworkDir, "TestMergeable");
			RunProcess ("xcrun", new [] { "clang", "-dynamiclib", "-o", executablePath, sourcePath, "-arch", "arm64", "-Wl,-make_mergeable", "-install_name", "@rpath/TestMergeable.framework/TestMergeable" });

			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.True, "Framework should be mergeable before stripping");

			// Run the SymbolStrip task with StripMergeableLibraries=true
			var task = CreateTask<SymbolStrip> ();
			var item = new TaskItem (executablePath);
			item.SetMetadata ("Kind", "Framework");
			task.Executable = new Microsoft.Build.Framework.ITaskItem [] { item };
			task.StripMergeableLibraries = true;
			ExecuteTask (task);

			// Verify atom info was removed
			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.False, "Framework should not be mergeable after SymbolStrip with StripMergeableLibraries=true");
			Assert.That (MachO.IsDynamicFramework (executablePath), Is.True, "Framework should still be dynamic after SymbolStrip");
		}

		[Test]
		public void SymbolStrip_PreservesAtomInfoWhenNotStripping ()
		{
			var tempDir = Cache.CreateTemporaryDirectory ();
			var sourcePath = Path.Combine (tempDir, "test.c");
			File.WriteAllText (sourcePath, "int mergeable_test_func (int a, int b) { return a + b; }");

			// Create a mergeable framework
			var frameworkDir = Path.Combine (tempDir, "TestMergeable.framework");
			Directory.CreateDirectory (frameworkDir);
			var executablePath = Path.Combine (frameworkDir, "TestMergeable");
			RunProcess ("xcrun", new [] { "clang", "-dynamiclib", "-o", executablePath, sourcePath, "-arch", "arm64", "-Wl,-make_mergeable", "-install_name", "@rpath/TestMergeable.framework/TestMergeable" });

			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.True, "Framework should be mergeable before stripping");

			// Run the SymbolStrip task with StripMergeableLibraries=false (debug mode)
			var task = CreateTask<SymbolStrip> ();
			var item = new TaskItem (executablePath);
			item.SetMetadata ("Kind", "Framework");
			task.Executable = new Microsoft.Build.Framework.ITaskItem [] { item };
			task.StripMergeableLibraries = false;
			ExecuteTask (task);

			// Verify atom info was preserved
			Assert.That (MachO.IsMergeableLibrary (executablePath), Is.True, "Framework should still be mergeable after SymbolStrip with StripMergeableLibraries=false");
			Assert.That (MachO.IsDynamicFramework (executablePath), Is.True, "Framework should still be dynamic after SymbolStrip");
		}
	}
}
