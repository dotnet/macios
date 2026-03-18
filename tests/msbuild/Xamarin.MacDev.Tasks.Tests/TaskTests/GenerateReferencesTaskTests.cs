using System;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.MacDev.Tasks;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GenerateReferencesTaskTests : TestBase {

		static ITaskItem CreateSymbol (string identity, string symbolType, string symbolMode = "Default")
		{
			var item = new TaskItem (identity);
			item.SetMetadata ("SymbolType", symbolType);
			item.SetMetadata ("SymbolMode", symbolMode);
			return item;
		}

		#region Linker mode tests

		[Test]
		public void LinkerMode_PassesThroughSymbols ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Linker";
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
				CreateSymbol ("_OBJC_CLASS_$_UIView", "ObjectiveCClass"),
				CreateSymbol ("_my_field", "Field"),
			};

			ExecuteTask (task);

			Assert.AreEqual (3, task.NativeSymbols.Length, "Should pass through all symbols");
			Assert.AreEqual ("_my_function", task.NativeSymbols [0].ItemSpec);
			Assert.AreEqual ("Function", task.NativeSymbols [0].GetMetadata ("SymbolType"));
			Assert.AreEqual ("_OBJC_CLASS_$_UIView", task.NativeSymbols [1].ItemSpec);
			Assert.AreEqual ("ObjectiveCClass", task.NativeSymbols [1].GetMetadata ("SymbolType"));
			Assert.AreEqual ("_my_field", task.NativeSymbols [2].ItemSpec);
			Assert.AreEqual ("Field", task.NativeSymbols [2].GetMetadata ("SymbolType"));
			Assert.IsEmpty (task.ReferencesFile, "No references file in linker mode");
		}

		[Test]
		public void LinkerMode_FiltersIgnoredSymbols ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Linker";
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
				CreateSymbol ("_ignored_sym", "Function", "Ignore"),
				CreateSymbol ("_OBJC_CLASS_$_UIView", "ObjectiveCClass"),
			};

			ExecuteTask (task);

			Assert.AreEqual (2, task.NativeSymbols.Length, "Should filter out ignored symbols");
			Assert.AreEqual ("_my_function", task.NativeSymbols [0].ItemSpec);
			Assert.AreEqual ("_OBJC_CLASS_$_UIView", task.NativeSymbols [1].ItemSpec);
		}

		[Test]
		public void LinkerMode_EmptySymbols ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Linker";
			task.RequiredSymbols = Array.Empty<ITaskItem> ();

			ExecuteTask (task);

			Assert.IsEmpty (task.NativeSymbols, "No symbols to pass through");
			Assert.IsEmpty (task.ReferencesFile, "No references file");
		}

		#endregion

		#region Code mode tests

		[Test]
		public void CodeMode_GeneratesReferenceFile ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
				CreateSymbol ("_OBJC_CLASS_$_UIView", "ObjectiveCClass"),
				CreateSymbol ("_my_field", "Field"),
			};

			ExecuteTask (task);

			Assert.AreEqual (1, task.ReferencesFile.Length, "Should produce one reference file");
			var refFile = task.ReferencesFile [0].ItemSpec;
			Assert.AreEqual (Path.Combine (tmpDir, "reference.m"), refFile);
			Assert.IsTrue (File.Exists (refFile), "reference.m should exist");

			var content = File.ReadAllText (refFile);

			// Check declarations
			StringAssert.Contains ("#import <Foundation/Foundation.h>", content);
			StringAssert.Contains ("extern void * my_function;", content);
			StringAssert.Contains ("extern void * my_field;", content);
			StringAssert.Contains ("@interface UIView : NSObject @end", content);

			// Check referencing function
			StringAssert.Contains ("static void __xamarin_symbol_referencer ()", content);
			StringAssert.Contains ("void __xamarin_symbol_referencer ()", content);
			StringAssert.Contains ("value = my_function;", content);
			StringAssert.Contains ("value = my_field;", content);
			StringAssert.Contains ("value = [UIView class];", content);

			Assert.IsEmpty (task.NativeSymbols, "No native symbols output in code mode");
		}

		[Test]
		public void CodeMode_EmptySymbols_NoFile ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = Array.Empty<ITaskItem> ();

			ExecuteTask (task);

			Assert.IsEmpty (task.ReferencesFile, "No references file for empty symbols");
			Assert.IsFalse (File.Exists (Path.Combine (tmpDir, "reference.m")), "reference.m should not exist");
		}

		[Test]
		public void CodeMode_EmptySymbols_DeletesExistingFile ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var refPath = Path.Combine (tmpDir, "reference.m");
			File.WriteAllText (refPath, "old content");
			Assert.IsTrue (File.Exists (refPath), "Precondition: file exists");

			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = Array.Empty<ITaskItem> ();

			ExecuteTask (task);

			Assert.IsEmpty (task.ReferencesFile, "No references file for empty symbols");
			Assert.IsFalse (File.Exists (refPath), "reference.m should be deleted");
		}

		[Test]
		public void CodeMode_FiltersIgnoredSymbols ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
				CreateSymbol ("_ignored_sym", "Function", "Ignore"),
			};

			ExecuteTask (task);

			Assert.AreEqual (1, task.ReferencesFile.Length, "Should produce one reference file");
			var content = File.ReadAllText (task.ReferencesFile [0].ItemSpec);
			StringAssert.Contains ("my_function", content);
			StringAssert.DoesNotContain ("ignored_sym", content);
		}

		[Test]
		public void CodeMode_AllIgnored_DeletesFile ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var refPath = Path.Combine (tmpDir, "reference.m");
			File.WriteAllText (refPath, "old content");

			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_ignored_sym", "Function", "Ignore"),
			};

			ExecuteTask (task);

			Assert.IsEmpty (task.ReferencesFile, "No references file when all symbols are ignored");
			Assert.IsFalse (File.Exists (refPath), "reference.m should be deleted when all symbols are ignored");
		}

		[Test]
		public void CodeMode_WriteIfDifferent_DoesNotRewrite ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
			};

			ExecuteTask (task);

			var refPath = task.ReferencesFile [0].ItemSpec;
			var lastWrite1 = File.GetLastWriteTimeUtc (refPath);

			// Wait a bit to ensure timestamp difference would be visible
			System.Threading.Thread.Sleep (100);

			// Run again with same inputs
			var task2 = CreateTask<GenerateReferencesTask> ();
			task2.SymbolMode = "Code";
			task2.CacheDirectory = tmpDir;
			task2.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
			};

			ExecuteTask (task2);

			var lastWrite2 = File.GetLastWriteTimeUtc (task2.ReferencesFile [0].ItemSpec);
			Assert.AreEqual (lastWrite1, lastWrite2, "File should not be rewritten when content is identical");
		}

		[Test]
		public void CodeMode_CreatesDirectoryIfNeeded ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var subDir = Path.Combine (tmpDir, "nested", "cache");

			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = subDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
			};

			ExecuteTask (task);

			Assert.IsTrue (Directory.Exists (subDir), "Cache directory should be created");
			Assert.IsTrue (File.Exists (Path.Combine (subDir, "reference.m")), "reference.m should exist in nested dir");
		}

		#endregion

		#region Ignore mode tests

		[Test]
		public void IgnoreMode_ProducesNoOutput ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Ignore";
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
				CreateSymbol ("_OBJC_CLASS_$_UIView", "ObjectiveCClass"),
			};

			ExecuteTask (task);

			Assert.IsEmpty (task.NativeSymbols, "No native symbols in ignore mode");
			Assert.IsEmpty (task.ReferencesFile, "No references file in ignore mode");
		}

		#endregion

		#region Error handling tests

		[Test]
		public void InvalidSymbolMode_LogsError ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Invalid";
			task.RequiredSymbols = Array.Empty<ITaskItem> ();

			ExecuteTask (task, expectedErrorCount: 1);
		}

		[Test]
		public void CodeMode_InvalidSymbolType_LogsError ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_bad_symbol", "InvalidType"),
			};

			ExecuteTask (task, expectedErrorCount: 1);
		}

		#endregion

		#region Symbol name handling tests

		[Test]
		public void CodeMode_StripsUnderscorePrefix ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_my_function", "Function"),
			};

			ExecuteTask (task);

			var content = File.ReadAllText (task.ReferencesFile [0].ItemSpec);
			// The "_" prefix should be stripped for the .m file content
			StringAssert.Contains ("extern void * my_function;", content);
			StringAssert.Contains ("value = my_function;", content);
		}

		[Test]
		public void CodeMode_ObjCClassName_ExtractsCorrectly ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Code";
			task.CacheDirectory = tmpDir;
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_OBJC_CLASS_$_NSObject", "ObjectiveCClass"),
				CreateSymbol ("_OBJC_CLASS_$_UIViewController", "ObjectiveCClass"),
			};

			ExecuteTask (task);

			var content = File.ReadAllText (task.ReferencesFile [0].ItemSpec);
			StringAssert.Contains ("@interface NSObject : NSObject @end", content);
			StringAssert.Contains ("@interface UIViewController : NSObject @end", content);
			StringAssert.Contains ("value = [NSObject class];", content);
			StringAssert.Contains ("value = [UIViewController class];", content);
		}

		[Test]
		public void LinkerMode_PreservesIdentity ()
		{
			var task = CreateTask<GenerateReferencesTask> ();
			task.SymbolMode = "Linker";
			task.RequiredSymbols = new ITaskItem [] {
				CreateSymbol ("_OBJC_CLASS_$_UIView", "ObjectiveCClass"),
			};

			ExecuteTask (task);

			// In linker mode, the identity should be preserved as-is (with underscore prefix)
			Assert.AreEqual ("_OBJC_CLASS_$_UIView", task.NativeSymbols [0].ItemSpec,
				"Linker mode should preserve the full symbol identity");
		}

		#endregion
	}
}
