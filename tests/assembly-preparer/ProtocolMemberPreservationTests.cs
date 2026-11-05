// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Rocks;
using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class ProtocolMemberPreservationTests : BaseClass {
	[TestCase (RegistrarMode.TrimmableStatic, false, null, true)]
	[TestCase (RegistrarMode.TrimmableStatic, false, false, true)]
	[TestCase (RegistrarMode.TrimmableStatic, true, null, false)]
	[TestCase (RegistrarMode.ManagedStatic, false, null, false)]
	[TestCase (RegistrarMode.Dynamic, true, null, false)]
	public void ProtocolMemberDependencies (RegistrarMode registrar, bool dynamicRegistrationSupported, bool? trimExportAttributes, bool expectedRemoval)
	{
		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, preparer => {
			preparer.Registrar = registrar;
			preparer.TrimExportAttributes = trimExportAttributes;
			preparer.Optimizations.RemoveDynamicRegistrar = !dynamicRegistrationSupported;
			preparer.Optimizations.OptimizeBlockLiteralSetupBlock = true;
			preparer.Optimizations.StaticBlockToDelegateLookup = true;
		}, "public class C {}", out _);

		AssertPrepare (preparer);

		var assembly = preparer.Configuration.Assemblies.Single (v => v.Name.Name == "Microsoft.macOS");
		var type = assembly.MainModule.GetType ("AppKit.INSApplicationDelegate");
		var cctor = type.GetStaticConstructor ();
		Assert.That (cctor, Is.Not.Null, "Protocol constructor");
		var dependencies = cctor.CustomAttributes.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute").ToArray ();
		var memberDependencies = dependencies.Where (v => v.ConstructorArguments.Count == 1).ToArray ();
		if (expectedRemoval) {
			Assert.That (memberDependencies, Is.Empty, "Protocol member dependencies");
		} else {
			Assert.That (memberDependencies.Select (v => v.ConstructorArguments [0].Value), Does.Contain ("GetHandler(AppKit.NSApplication,Intents.INIntent)"), "Intents dependency");
			Assert.That (memberDependencies.Select (v => v.ConstructorArguments [0].Value), Does.Contain ("UserDidAcceptCloudKitShare(AppKit.NSApplication,CloudKit.CKShareMetadata)"), "CloudKit dependency");
		}
		Assert.That (dependencies.Where (v => v.ConstructorArguments.Count == 2).Select (v => ((TypeReference) v.ConstructorArguments [1].Value).FullName),
			Does.Contain ("AppKit.NSApplicationDelegateWrapper"), "Wrapper dependency");
	}

	[Test]
	public void PreserveOtherConstructors ()
	{
		var code = """
			using System;
			using System.Diagnostics.CodeAnalysis;
			using Foundation;
			using ObjCRuntime;

			[Protocol]
			public interface ICustomProtocol {
				[DynamicDependency ("Method")]
				static ICustomProtocol () { GC.KeepAlive (null); }
				void Method () { }
			}

			public class NonProtocol {
				[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
				[DynamicDependency ("Method")]
				static NonProtocol () { GC.KeepAlive (null); }
				public void Method () { }
			}
			""";

		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, preparer => {
			preparer.Registrar = RegistrarMode.TrimmableStatic;
			preparer.Optimizations.RemoveDynamicRegistrar = true;
		}, code, out _);
		AssertPrepare (preparer);

		var assembly = preparer.Configuration.Assemblies.Single (v => v.Name.Name == "Test");
		foreach (var name in new [] { "ICustomProtocol", "NonProtocol" }) {
			var cctor = assembly.MainModule.GetType (name).GetStaticConstructor ();
			Assert.That (cctor, Is.Not.Null, name);
			Assert.That (cctor.CustomAttributes.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute").Select (v => v.ConstructorArguments [0].Value),
				Does.Contain ("Method"), name);
		}
	}
}
