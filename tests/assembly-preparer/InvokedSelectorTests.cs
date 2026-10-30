// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class InvokedSelectorTests : BaseClass {
	[TestCase (null, false, false)]
	[TestCase (null, true, true)]
	[TestCase (true, false, true)]
	[TestCase (true, true, true)]
	[TestCase (false, false, false)]
	[TestCase (false, true, false)]
	public void Detection (bool? enabled, bool referenced, bool expected)
	{
		var code = referenced
			? "public class C { public object Get () => ObjCRuntime.Runtime.InvokedSelector; }"
			: "public class C {}";
		var output = Path.Combine (Xamarin.Cache.CreateTemporaryDirectory (), "msbuild.txt");
		using var preparer = CreatePreparer (ApplePlatform.MacCatalyst, true, p => {
			p.Configuration.Application.EnableInvokedSelectorTracking = enabled;
			p.Configuration.Application.AreAnyAssembliesTrimmed = false;
		}, code, out _, extraConfig: $"MSBuildOutputFile={output}");

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new DetectApiUsageStep ().Process (context);

		Assert.That (preparer.Configuration.Application.EnableInvokedSelectorTracking, Is.EqualTo (expected), "Tracking");
		var logger = (TestLogger) preparer.Configuration.Logger;
		Assert.That (logger.Errors.Any (v => v.Code == 2388), Is.EqualTo (enabled == false && referenced), "Disabled feature error");
		Assert.That (preparer.Configuration.Application.DynamicRegistrationSupported, Is.True, "Untrimmed dynamic registrar");
		preparer.Configuration.FlushOutputForMSBuild ();
		Assert.That (File.ReadAllLines (output), Does.Contain ($"EnableInvokedSelectorTracking={expected.ToString ().ToLowerInvariant ()}"), "Persisted decision");
	}

	[Test]
	public void UnrelatedGetterDoesNotEnableTracking ()
	{
		var code = """
			namespace ObjCRuntime {
				public static class Runtime {
					public static object InvokedSelector => null;
				}
			}
			public class C {
				public object Get () => ObjCRuntime.Runtime.InvokedSelector;
			}
			""";
		using var preparer = CreatePreparer (ApplePlatform.MacCatalyst, true, null, code, out _);
		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new DetectApiUsageStep ().Process (context);
		Assert.That (preparer.Configuration.Application.EnableInvokedSelectorTracking, Is.False);
	}

	[TestCase (RegistrarMode.Static, false)]
	[TestCase (RegistrarMode.Static, true)]
	[TestCase (RegistrarMode.ManagedStatic, false)]
	[TestCase (RegistrarMode.ManagedStatic, true)]
	[TestCase (RegistrarMode.TrimmableStatic, false)]
	[TestCase (RegistrarMode.TrimmableStatic, true)]
	public void NativeRegistrarGeneration (RegistrarMode registrar, bool enabled)
	{
		var code = """
			using Foundation;
			using ObjCRuntime;
			public class SelectorTarget : NSObject {
				[Export ("init")]
				public SelectorTarget () {}

				[Export ("invoke:")]
				public void Invoke (NSObject sender) {}

				[Export ("value")]
				public int Value () => 42;
			}
			""";
		var directory = Xamarin.Cache.CreateTemporaryDirectory ();
		using var preparer = CreatePreparer (ApplePlatform.MacCatalyst, true, p => {
			p.Registrar = registrar;
			p.Configuration.Application.EnableInvokedSelectorTracking = enabled;
			// Code generation doesn't require the installed Xcode to match the bindings.
			p.Configuration.Application.SdkVersion = SdkVersions.GetVersion (ApplePlatform.MacCatalyst);
		}, code, out _, extraConfig: $"CacheDirectory={directory}\nTypeMapAssemblyName=_TypeMap\nTypeMapOutputDirectory={directory}");
		AssertPrepare (preparer);

		var source = Path.Combine (directory, "registrar.mm");
		new RegistrarStep ().Process (preparer.Configuration.DerivedLinkContext);
		Assert.That (((TestLogger) preparer.Configuration.Logger).Errors, Is.Empty, "Registrar errors");
		var contents = File.ReadAllText (source);
		Assert.That (contents.Contains ("xamarin_set_invoked_selector (_cmd)"), Is.EqualTo (enabled), "Selector entry");
		Assert.That (contents.Contains ("xamarin_set_invoked_selector (previous_selector)"), Is.EqualTo (enabled), "Selector restoration");
		Assert.That (contents.Contains ("@catch (id exception)"), Is.EqualTo (enabled), "Exception-safe restoration");
	}

	[TestCase (false)]
	[TestCase (true)]
	public void PartialStaticRegistrar (bool enabled)
	{
		using var preparer = CreatePreparer (ApplePlatform.MacCatalyst, true, p => {
			p.Registrar = RegistrarMode.PartialStatic;
			p.Configuration.Application.EnableInvokedSelectorTracking = enabled;
		}, "public class C {}", out _, extraConfig: "PartialStaticRegistrarLibrary=platform-registrar.a");
		new RegistrarStep ().Process (preparer.Configuration.DerivedLinkContext);
		Assert.That (((TestLogger) preparer.Configuration.Logger).Errors, Is.Empty, "Registrar errors");
		Assert.That (preparer.Configuration.RegistrationMethods.Count, Is.EqualTo (enabled ? 0 : 1), "Prebuilt registrar");
	}
}
