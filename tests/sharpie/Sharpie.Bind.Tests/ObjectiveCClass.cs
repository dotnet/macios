// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin;

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
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedBindings =
"""
using Foundation;

// @interface MyClass
interface MyClass
{
	// @property int P1;
	[Export ("P1")]
	int P1 { get; set; }
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
		binder.PlatformAssembly = Extensions.GetPlatformAssemblyPath (binder.Platform);
		binder.ClangResourceDirectory = Extensions.GetClangResourceDirectory ();
		var bindings = binder.BindInOrOut ();
		var expectedApiDefinitionBindings =
"""
using Foundation;

// @interface MyClass
interface MyClass
{
	// @property int P1;
	[Export ("P1")]
	int P1 { get; set; }
}

""";
		var expectedStructAndEnumsBindings =
		"""
		using System.Runtime.InteropServices;

		[StructLayout (LayoutKind.Sequential)]
		public struct MyStruct
		{
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
}
