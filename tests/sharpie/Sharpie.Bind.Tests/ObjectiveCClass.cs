// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Sharpie.Bind.Tests;

public class Tests {
	[Test]
	public void ErrorNoNSObject ()
	{
		var binder = new BindTool ();
		var code =
		"""
        @interface MyClass : NSObject  {
        }
            @property int Pi1;
        @end
        int main () { return 0; }
        """;
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var tmpfile = Path.Combine (tmpdir, "test.m");
		File.WriteAllText (tmpfile, code);
		binder.SourceFile = tmpfile;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		bindings.AssertErrors ((5, $"Compilation failed with error: cannot find interface declaration for 'NSObject', superclass of 'MyClass'", tmpfile, 1));
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void ObjectiveCClass ()
	{
		var binder = new BindTool ();
		var code =
		"""
        @interface MyClass {
        }
            @property int P1;
        @end
        """;
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var tmpfile = Path.Combine (tmpdir, "test.m");
		File.WriteAllText (tmpfile, code);
		binder.SplitDocuments = false;
		binder.SourceFile = tmpfile;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

// @interface MyClass
interface MyClass {
	// @property int P1;
	[Export ("P1")]
	int P1 { get; set; }
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework ()
	{
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "MyFramework.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		Directory.CreateDirectory (headersDir);
		var umbrellaHeader = Path.Combine (headersDir, "MyFramework.h");
		File.WriteAllText (umbrellaHeader,
		"""
		@interface MyClass {
		}
			@property int P1;
		@end
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

namespace MyFramework {
	// @interface MyClass
	interface MyClass {
		// @property int P1;
		[Export ("P1")]
		int P1 { get; set; }
	}
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ImpliedNamespace ()
	{
		// Verify that the namespace is set to the framework name when not explicitly specified.
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "CustomKit.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		Directory.CreateDirectory (headersDir);
		var umbrellaHeader = Path.Combine (headersDir, "CustomKit.h");
		File.WriteAllText (umbrellaHeader,
		"""
		@interface Widget {
		}
			@property int Width;
		@end
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

namespace CustomKit {
	// @interface Widget
	interface Widget {
		// @property int Width;
		[Export ("Width")]
		int Width { get; set; }
	}
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ExplicitNamespace ()
	{
		// Verify that an explicitly specified namespace overrides the framework name.
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "MyFramework.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		Directory.CreateDirectory (headersDir);
		var umbrellaHeader = Path.Combine (headersDir, "MyFramework.h");
		File.WriteAllText (umbrellaHeader,
		"""
		@interface MyClass {
		}
			@property int P1;
		@end
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.Namespace = "CustomNamespace";
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

namespace CustomNamespace {
	// @interface MyClass
	interface MyClass {
		// @property int P1;
		[Export ("P1")]
		int P1 { get; set; }
	}
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ErrorBothSourceFileAndFramework ()
	{
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "MyFramework.framework");
		Directory.CreateDirectory (frameworkDir);
		var tmpfile = Path.Combine (tmpdir, "test.m");
		File.WriteAllText (tmpfile, "@interface MyClass @end");
		binder.SourceFile = tmpfile;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		bindings.AssertErrors ((16, "Cannot specify both a source framework and a source file.", null, null));
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ErrorNonExistentFramework ()
	{
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "NonExistent.framework");
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		bindings.AssertErrors ((17, $"The framework '{frameworkDir}' doesn't exist.", null, null));
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ErrorNoUmbrellaHeaderOrModuleMap ()
	{
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "Empty.framework");
		Directory.CreateDirectory (frameworkDir);
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var fullPath = Path.GetFullPath (frameworkDir);
		bindings.AssertErrors ((18, $"The framework '{fullPath}' does not have an umbrella header or module map.", null, null));
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_SdkFromInfoPlist ()
	{
		// Verify that the Sdk value is computed from the framework's Info.plist when not explicitly set.
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "MyFramework.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		Directory.CreateDirectory (headersDir);
		var umbrellaHeader = Path.Combine (headersDir, "MyFramework.h");
		File.WriteAllText (umbrellaHeader,
		"""
		@interface MyClass {
		}
			@property int P1;
		@end
		""");
		// Create an Info.plist with DTSDKName
		var infoPlist = Path.Combine (frameworkDir, "Info.plist");
		File.WriteAllText (infoPlist,
		"""
		<?xml version="1.0" encoding="UTF-8"?>
		<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
		<plist version="1.0">
		<dict>
			<key>DTSDKName</key>
			<string>macosx15.0</string>
		</dict>
		</plist>
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.MacOSX);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (ApplePlatform.MacOSX);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		// Don't set binder.Sdk - it should be read from the Info.plist
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

namespace MyFramework {
	// @interface MyClass
	interface MyClass {
		// @property int P1;
		[Export ("P1")]
		int P1 { get; set; }
	}
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_SdkVersionStripped ()
	{
		// Verify that the version number is stripped from the DTSDKName value (e.g. "macosx11.0" → "macosx").
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "MyFramework.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		Directory.CreateDirectory (headersDir);
		var umbrellaHeader = Path.Combine (headersDir, "MyFramework.h");
		File.WriteAllText (umbrellaHeader,
		"""
		@interface MyClass {
		}
			@property int P1;
		@end
		""");
		var infoPlist = Path.Combine (frameworkDir, "Info.plist");
		File.WriteAllText (infoPlist,
		"""
		<?xml version="1.0" encoding="UTF-8"?>
		<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
		<plist version="1.0">
		<dict>
			<key>DTSDKName</key>
			<string>macosx11.0</string>
		</dict>
		</plist>
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (ApplePlatform.MacOSX);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (ApplePlatform.MacOSX);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		// The SDK should be "macosx" (version stripped), not "macosx11.0" which doesn't exist.
		Assert.That (binder.Sdk, Is.EqualTo ("macosx"), "SDK version should be stripped");
		bindings.AssertSuccess (null);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void BindFramework_ModuleMap ()
	{
		// Verify that a framework with a module map can be bound,
		// and that the framework search path (-F) is automatically configured.
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var frameworkDir = Path.Combine (tmpdir, "ModKit.framework");
		var headersDir = Path.Combine (frameworkDir, "Headers");
		var modulesDir = Path.Combine (frameworkDir, "Modules");
		Directory.CreateDirectory (headersDir);
		Directory.CreateDirectory (modulesDir);
		var header = Path.Combine (headersDir, "ModKit.h");
		File.WriteAllText (header,
		"""
		@interface MKWidget {
		}
			@property int Height;
		@end
		""");
		File.WriteAllText (Path.Combine (modulesDir, "module.modulemap"),
		"""
		framework module ModKit {
			umbrella header "ModKit.h"
			export *
			module * { export * }
		}
		""");
		binder.SplitDocuments = false;
		binder.SourceFramework = frameworkDir;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		// Do NOT manually set ClangArguments with -F — it should be auto-added.
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

namespace ModKit {
	// @interface MKWidget
	interface MKWidget {
		// @property int Height;
		[Export ("Height")]
		int Height { get; set; }
	}
}

""";
		bindings.AssertSuccess (expectedBindings);
		bindings.AssertNoWarnings ();
	}

	[Test]
	public void SplitDocuments ()
	{
		var binder = new BindTool ();
		var code =
		"""
		struct MyStruct {
			int X;
			int Y;
		};
		@interface MyClass {
		}
			@property int P1;
		@end
		""";
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var tmpfile = Path.Combine (tmpdir, "test.m");
		File.WriteAllText (tmpfile, code);
		binder.SourceFile = tmpfile;
		binder.OutputDirectory = tmpdir;
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedApiDefinitionBindings =
"""
using Foundation;

// @interface MyClass
interface MyClass {
	// @property int P1;
	[Export ("P1")]
	int P1 { get; set; }
}

""";
		var expectedStructAndEnumsBindings =
		"""
		using System.Runtime.InteropServices;

		[StructLayout (LayoutKind.Sequential)]
		public struct MyStruct {
			public int X;

			public int Y;
		}
		
		""";
		bindings.AssertSuccess (null);
		bindings.AssertNoWarnings ();
		Assert.That (bindings.AdditionalFiles.Count, Is.EqualTo (2), "Additional files");
		Assert.That (bindings.AdditionalFiles ["ApiDefinition.cs"].Trim (), Is.EqualTo (expectedApiDefinitionBindings.Trim ()), "Api definition");
		Assert.That (bindings.AdditionalFiles ["StructsAndEnums.cs"].Trim (), Is.EqualTo (expectedStructAndEnumsBindings.Trim ()), "Struct and enums");
	}

	[Test]
	public void Scope_RelativePath ()
	{
		// Verify that --scope with a relative path works correctly
		// (the relative path should be resolved to absolute before matching).
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var subdir = Path.Combine (tmpdir, "headers");
		Directory.CreateDirectory (subdir);

		// Create a header in the scoped directory
		var scopedHeader = Path.Combine (subdir, "InScope.h");
		File.WriteAllText (scopedHeader,
		"""
		@interface InScopeClass {
		}
			@property int Value;
		@end
		""");

		// Create an umbrella header that includes both
		var mainHeader = Path.Combine (tmpdir, "main.h");
		File.WriteAllText (mainHeader, $"#import \"{scopedHeader}\"\n");

		binder.SplitDocuments = false;
		binder.SourceFile = mainHeader;
		binder.OutputDirectory = tmpdir;

		// Use a relative scope path (the bug was that this produced empty output)
		var oldCwd = Environment.CurrentDirectory;
		try {
			Environment.CurrentDirectory = tmpdir;
			binder.DirectoriesInScope.Add (Path.GetFullPath ("headers"));
		} finally {
			Environment.CurrentDirectory = oldCwd;
		}

		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

// @interface InScopeClass
interface InScopeClass {
	// @property int Value;
	[Export ("Value")]
	int Value { get; set; }
}

""";
		bindings.AssertSuccess (expectedBindings);
	}

	[Test]
	public void Scope_FiltersOutOfScopeDeclarations ()
	{
		// Verify that declarations from headers outside the scope directory are not bound.
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var scopedDir = Path.Combine (tmpdir, "scoped");
		var unscopedDir = Path.Combine (tmpdir, "unscoped");
		Directory.CreateDirectory (scopedDir);
		Directory.CreateDirectory (unscopedDir);

		var scopedHeader = Path.Combine (scopedDir, "Scoped.h");
		File.WriteAllText (scopedHeader,
		"""
		@interface ScopedClass {
		}
			@property int A;
		@end
		""");

		var unscopedHeader = Path.Combine (unscopedDir, "Unscoped.h");
		File.WriteAllText (unscopedHeader,
		"""
		@interface UnscopedClass {
		}
			@property int B;
		@end
		""");

		var mainHeader = Path.Combine (tmpdir, "main.h");
		File.WriteAllText (mainHeader, $"#import \"{scopedHeader}\"\n#import \"{unscopedHeader}\"\n");

		binder.SplitDocuments = false;
		binder.SourceFile = mainHeader;
		binder.OutputDirectory = tmpdir;
		binder.DirectoriesInScope.Add (scopedDir);
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();

		// Only ScopedClass should be in the output, not UnscopedClass
		var expectedBindings =
"""
using Foundation;

// @interface ScopedClass
interface ScopedClass {
	// @property int A;
	[Export ("A")]
	int A { get; set; }
}

""";
		bindings.AssertSuccess (expectedBindings);
	}

	[Test]
	public void Scope_PrefixDoesNotFalseMatch ()
	{
		// Verify that a scope of "/foo/bar" does not match "/foo/barbaz/header.h"
		// (the scope must be a proper directory prefix with separator).
		var binder = new BindTool ();
		var tmpdir = Cache.CreateTemporaryDirectory ();
		var scopedDir = Path.Combine (tmpdir, "scope");
		var falseMatchDir = Path.Combine (tmpdir, "scopeextra");
		Directory.CreateDirectory (scopedDir);
		Directory.CreateDirectory (falseMatchDir);

		var scopedHeader = Path.Combine (scopedDir, "Good.h");
		File.WriteAllText (scopedHeader,
		"""
		@interface GoodClass {
		}
			@property int X;
		@end
		""");

		var falseMatchHeader = Path.Combine (falseMatchDir, "Bad.h");
		File.WriteAllText (falseMatchHeader,
		"""
		@interface BadClass {
		}
			@property int Y;
		@end
		""");

		var mainHeader = Path.Combine (tmpdir, "main.h");
		File.WriteAllText (mainHeader, $"#import \"{scopedHeader}\"\n#import \"{falseMatchHeader}\"\n");

		binder.SplitDocuments = false;
		binder.SourceFile = mainHeader;
		binder.OutputDirectory = tmpdir;
		binder.DirectoriesInScope.Add (scopedDir);
		Configuration.IgnoreIfIgnoredPlatform (binder.Platform);
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();

		// Only GoodClass should appear (from "scope/"), not BadClass (from "scopeextra/")
		var expectedBindings =
"""
using Foundation;

// @interface GoodClass
interface GoodClass {
	// @property int X;
	[Export ("X")]
	int X { get; set; }
}

""";
		bindings.AssertSuccess (expectedBindings);
	}
}
