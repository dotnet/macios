// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class RegistrarTests : BaseClass {
	[TestCase (RegistrarMode.Static)]
	[TestCase (RegistrarMode.ManagedStatic)]
	[TestCase (RegistrarMode.TrimmableStatic)]
	public void NSCellSubclassWarnings (RegistrarMode registrar)
	{
		var code = """
			using AppKit;
			using Foundation;
			using ObjCRuntime;

			class MissingBoth : NSCell {
			}

			class MissingConstructor : NSCell {
				public override NSObject Copy (NSZone zone) => base.Copy (zone);
			}

			class MissingCopy : NSCell {
				protected MissingCopy (NativeHandle handle) : base (handle) {}
			}

			class WrongConstructor : NSCell {
				protected WrongConstructor (NativeHandle handle, bool owns) : base (handle) {}
				public override NSObject Copy (NSZone zone) => base.Copy (zone);
			}

			class Complete : NSCell {
				protected Complete (NativeHandle handle) : base (handle) {}
				public override NSObject Copy (NSZone zone) => base.Copy (zone);
			}

			class ExportedCopy : NSCell {
				protected ExportedCopy (NativeHandle handle) : base (handle) {}
				[Export ("copyWithZone:")]
				public NSObject CopyCell (NSZone zone) => base.Copy (zone);
			}

			class InheritedCopy : Complete {
				protected InheritedCopy (NativeHandle handle) : base (handle) {}
			}

			class InheritedConstructor : Complete {
				public InheritedConstructor () : base (NativeHandle.Zero) {}
			}

			class IndirectSubclass : NSButtonCell {
			}

			abstract class AbstractCell : NSCell {
			}

			[Register ("WrappedCell", true)]
			class WrappedCell : NSCell {
			}

			class NotACell : NSObject {
			}
			""";

		var tmpdir = Xamarin.Cache.CreateTemporaryDirectory ();
		var config = $"""
			AssemblyName=Microsoft.macOS.dll
			CacheDirectory={tmpdir}
			TypeMapAssemblyName=_TypeMap
			TypeMapOutputDirectory={Path.Combine (tmpdir, "typemaps")}
			UnmanagedCallersOnlyMapPath={Path.Combine (tmpdir, "uco.txt")}
			""";
		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, p => p.Registrar = registrar, code, out _, extraConfig: config);
		AssertPrepare (preparer);
		if (registrar == RegistrarMode.Static)
			new RegistrarStep ().Process (preparer.Configuration.DerivedLinkContext);

		var logger = (TestLogger) preparer.Configuration.Logger;
		var expected = new List<string> ();
		foreach (var type in new [] { "MissingBoth", "MissingConstructor", "WrongConstructor", "InheritedConstructor", "IndirectSubclass" })
			expected.Add ($"4199: The NSCell subclass '{type}' does not have a constructor that takes a single ObjCRuntime.NativeHandle argument. Add this constructor so that copied native cells can be wrapped in managed instances.");
		foreach (var type in new [] { "MissingBoth", "MissingCopy", "IndirectSubclass" })
			expected.Add ($"4200: The NSCell subclass '{type}' does not implement Copy (NSZone). Override this method to copy the managed state when the native cell is copied.");
		Assert.That (logger.Warnings.Select (v => $"{v.Code}: {v.Message}"), Is.EquivalentTo (expected), "Warnings");
		Assert.That (logger.Errors, Is.Empty, "Errors");
	}

	[Test]
	public void ModelClassAsGenericArgument ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""sessionOptions"")]
			public NSDictionary<NSString, NSFileManagerDelegate> GetSessionOptions ()
			{
				throw new System.NotImplementedException ();
			}
		}
		";

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.ManagedStatic, code, out _);
		preparer.Prepare (out _);

		var logger = (TestLogger) preparer.Configuration.Logger;
		var exception = logger.Errors.Single (v => v.Code == 4192);
		Assert.That (exception.Message, Is.EqualTo ("The registrar cannot use the model class 'Foundation.NSFileManagerDelegate' as a generic type argument in the generic type 'Foundation.NSDictionary`2<Foundation.NSString,Foundation.NSFileManagerDelegate>'. Use the protocol interface instead of the model class."));
	}

	[Test]
	public void ProtocolInterfaceAsGenericArgument ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""sessionOptions"")]
			public NSDictionary<NSString, INSFileManagerDelegate> GetSessionOptions ()
			{
				throw new System.NotImplementedException ();
			}
		}
		";

		AssertPrepare (ApplePlatform.iOS, false, RegistrarMode.ManagedStatic, code, out _);
	}
}
