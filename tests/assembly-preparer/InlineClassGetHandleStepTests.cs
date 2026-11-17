// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

public class InlineClassGetHandleStepTests : BaseClass {
	[TestCase (ApplePlatform.MacCatalyst)]
	[TestCase (ApplePlatform.iOS)]
	[TestCase (ApplePlatform.TVOS)]
	[TestCase (ApplePlatform.MacOSX)]
	public void StaticRegistrarIncludesSwiftStubMetadata (ApplePlatform platform)
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		[Register (""SwiftStub"", true, IsStubClass = true)]
		public class SwiftStub : NSObject {
			public static NativeHandle Lookup () => Class.GetHandle (""SwiftStub"");
		}
		";

		var typeMapPath = Path.Combine (Xamarin.Cache.CreateTemporaryDirectory (), "type-map.txt");
		AssertPrepare (platform, RegistrarMode.Static, code, out var assembly,
			extraConfig: $"InlineClassGetHandle=strict\nTypeMapFilePath={typeMapPath}");
		using (assembly) {
			Assert.That (File.ReadAllText (typeMapPath), Does.Match (@"(?m)^Class=SwiftStub\|.*\|IsWrapper=True\|IsStubClass=True\r?$"), "Swift stub metadata for native code generation");
			var methods = assembly.MainModule.GetType ("ObjCRuntime.Class").Methods;
			Assert.That (methods.Select (v => v.PInvokeInfo?.EntryPoint), Does.Contain ("xamarin_Class_GetHandle_SwiftStub_Native"), "Inlined class lookup");
		}
	}
}
