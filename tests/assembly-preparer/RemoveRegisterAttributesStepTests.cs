// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class RemoveRegisterAttributesStepTests : BaseClass {
	const string Code = """
		using Foundation;

		[Register ("NativeDirectWrapper", true)]
		public class DirectWrapper : NSObject {}

		[Register ("NativeSkippedWrapper", SkipRegistration = true)]
		public class SkippedWrapper : NSObject {}

		[Register ("NativeStubWrapper", true, IsStubClass = true)]
		public class StubWrapper : NSObject {}

		[Register ("NativeWrapperAncestor", true)]
		public class WrapperAncestor : NSObject {}

		[Register ("NativeApplicationSubclass")]
		public class ApplicationSubclass : WrapperAncestor {}

		[Register ("NativeNonNSObject", true)]
		public class NonNSObject {}

		public class Container {
			[Register ("NativeNestedWrapper", true)]
			public class NestedWrapper : NSObject {}
		}
		""";

	[TestCase (true, RegistrarMode.TrimmableStatic, "link", false, true)]
	[TestCase (false, RegistrarMode.TrimmableStatic, "link", false, false)]
	[TestCase (true, RegistrarMode.ManagedStatic, "link", false, false)]
	[TestCase (true, RegistrarMode.TrimmableStatic, "copy", false, false)]
	[TestCase (true, RegistrarMode.TrimmableStatic, "link", true, false)]
	public void Preparation (bool eligible, RegistrarMode registrar, string trimMode, bool dynamicRegistrationSupported, bool expectedRemoval)
	{
		var directory = Xamarin.Cache.CreateTemporaryDirectory ();
		var config = $"""
			AssemblyName=Microsoft.iOS.dll
			CanTrimRegistrarAttributes={eligible}
			DynamicRegistrationSupported={dynamicRegistrationSupported}
			TypeMapAssemblyName=_TypeMap
			TypeMapOutputDirectory={Path.Combine (directory, "typemaps")}
			""";
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => {
			p.Registrar = registrar;
			p.TrimExportAttributes = false;
			p.Optimizations.RemoveDynamicRegistrar = !dynamicRegistrationSupported;
		}, Code, out var info, extraConfig: config, testAssemblyTrimMode: trimMode);

		AssertPrepare (preparer);

		var directBinding = preparer.Configuration.DerivedLinkContext.IsDirectBindingValue;
		Assert.That (directBinding, Is.Not.Null, "Preparation-time type map");
		Assert.That (directBinding.Single (v => v.Key.Name == "DirectWrapper").Value, Is.True, "Direct wrapper");
		Assert.That (directBinding.Single (v => v.Key.Name == "SkippedWrapper").Value, Is.True, "Skipped wrapper");
		Assert.That (directBinding.Single (v => v.Key.Name == "WrapperAncestor").Value, Is.Null, "Wrapper ancestor");
		Assert.That (directBinding.Single (v => v.Key.Name == "ApplicationSubclass").Value, Is.False, "Application subclass");

		using var assembly = AssemblyDefinition.ReadAssembly (info.OutputPath);
		foreach (var name in new [] { "DirectWrapper", "SkippedWrapper", "StubWrapper", "Container/NestedWrapper" })
			AssertRegister (assembly.MainModule.GetType (name), !expectedRemoval);
		AssertRegister (assembly.MainModule.GetType ("WrapperAncestor"), true);
		AssertRegister (assembly.MainModule.GetType ("ApplicationSubclass"), true);
		AssertRegister (assembly.MainModule.GetType ("NonNSObject"), true);
	}

	[TestCase (XamarinRuntime.CoreCLR)]
	[TestCase (XamarinRuntime.NativeAOT)]
	public void PostprocessingRecoversRegistrationMetadata (XamarinRuntime runtime)
	{
		var directory = Xamarin.Cache.CreateTemporaryDirectory ();
		var config = $"""
			AssemblyName=Microsoft.iOS.dll
			CanTrimRegistrarAttributes=true
			DynamicRegistrationSupported=false
			Registrar=trimmable-static
			PrepareAssemblies=true
			CacheDirectory={directory}
			XamarinRuntime={runtime}
			TypeMapAssemblyName=_TypeMap
			TypeMapOutputDirectory={Path.Combine (directory, "typemaps")}
			UnmanagedCallersOnlyMapPath={Path.Combine (directory, "uco.txt")}
			""";
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => {
			p.Registrar = RegistrarMode.TrimmableStatic;
			p.TrimExportAttributes = false;
		}, Code, out _, extraConfig: config);
		AssertPrepare (preparer);

		var infos = preparer.Assemblies.Select (v => new AssemblyPreparerInfo (v.OutputPath, v.OutputPath, v.IsTrimmable, v.TrimMode))
			.Concat (preparer.AddedAssemblies.Select (v => new AssemblyPreparerInfo (v.Path, v.Path, true, "link")))
			.ToArray ();
		var configPath = Path.Combine (Path.GetFullPath (Path.Combine (preparer.IntermediateOutputPath, "..")), "config.txt");
		var logger = new TestLogger { Platform = ApplePlatform.iOS };
		using var postprocessor = new AssemblyPreparer (logger, infos, configPath) { Registrar = RegistrarMode.TrimmableStatic };
		var app = postprocessor.Configuration.Application;
		app.IsPostProcessingAssemblies = true;
		var resolver = new DefaultAssemblyResolver ();
		foreach (var path in preparer.Assemblies.Select (v => Path.GetDirectoryName (v.InputPath)).Distinct ()) {
			if (path is not null)
				resolver.AddSearchDirectory (path);
		}
		app.PreTrimAssemblyResolver = resolver;

		var context = postprocessor.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new CoreTypeMapStep ().Process (context);
		var directBinding = context.IsDirectBindingValue;
		Assert.That (directBinding, Is.Not.Null, "Postprocessing type map");
		Assert.That (directBinding.Single (v => v.Key.Name == "DirectWrapper").Value, Is.True, "Recovered wrapper state");
		Assert.That (directBinding.Single (v => v.Key.Name == "SkippedWrapper").Value, Is.True, "Recovered skip-registration state");

		new ManagedRegistrarStep ().Process (context);
		new RegistrarStep ().Process (context);
		Assert.That (logger.Errors, Is.Empty, "Postprocessing errors");
		Assert.That (postprocessor.Configuration.Exceptions, Is.Empty, "Postprocessing exceptions");

		var header = File.ReadAllText (Path.Combine (directory, "registrar.h"));
		var source = File.ReadAllText (Path.Combine (directory, "registrar.mm"));
		Assert.That (header, Does.Contain ("@interface NativeDirectWrapper : NSObject"), "Recovered native name and wrapper state");
		Assert.That (header, Does.Not.Contain ("NativeSkippedWrapper"), "Skipped wrapper is not registered");
		Assert.That (header, Does.Contain ("__attribute__((objc_class_stub))"), "Recovered stub declaration");
		Assert.That (source, Does.Contain ("[NativeStubWrapper class];"), "Recovered stub initialization");
	}

	static void AssertRegister (TypeDefinition type, bool expected)
	{
		Assert.That (type.CustomAttributes.Any (v => v.AttributeType.FullName == "Foundation.RegisterAttribute"), Is.EqualTo (expected), type.FullName);
	}
}
