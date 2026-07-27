// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class ComputeExportAttributeRemovalStepTests : BaseClass {
	[TestCase (false, false, false)]
	[TestCase (null, false, true)]
	[TestCase (true, false, true)]
	[TestCase (null, true, false)]
	public void DynamicRegistration (bool? trimExportAttributes, bool dynamicRegistrationSupported, bool expectedRemoval)
	{
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, preparer => {
			preparer.Registrar = RegistrarMode.TrimmableStatic;
			preparer.TrimExportAttributes = trimExportAttributes;
			preparer.Optimizations.RemoveDynamicRegistrar = !dynamicRegistrationSupported;
			preparer.Optimizations.OptimizeBlockLiteralSetupBlock = true;
			preparer.Optimizations.StaticBlockToDelegateLookup = true;
		}, "public class C {}", out _);

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new ComputeExportAttributeRemovalStep ().Process (context);

		Assert.That (preparer.Configuration.Application.TrimExportAttributes, Is.EqualTo (expectedRemoval), "Removal");
	}

	[TestCase (true, true, true)]
	[TestCase (false, true, false)]
	[TestCase (true, false, false)]
	public void RequiredOptimizations (bool optimizeBlockLiteralSetupBlock, bool staticBlockToDelegateLookup, bool expectedRemoval)
	{
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, preparer => {
			preparer.Registrar = RegistrarMode.TrimmableStatic;
			preparer.TrimExportAttributes = true;
			preparer.Optimizations.RemoveDynamicRegistrar = true;
			preparer.Optimizations.OptimizeBlockLiteralSetupBlock = optimizeBlockLiteralSetupBlock;
			preparer.Optimizations.StaticBlockToDelegateLookup = staticBlockToDelegateLookup;
		}, "public class C {}", out _);

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new ComputeExportAttributeRemovalStep ().Process (context);

		Assert.That (preparer.Configuration.Application.TrimExportAttributes, Is.EqualTo (expectedRemoval), "Removal");
	}

	[Test]
	public void ExplicitBlocker ()
	{
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, preparer => {
			preparer.Registrar = RegistrarMode.TrimmableStatic;
			preparer.TrimExportAttributes = true;
			preparer.Optimizations.RemoveDynamicRegistrar = true;
			preparer.Optimizations.OptimizeBlockLiteralSetupBlock = true;
			preparer.Optimizations.StaticBlockToDelegateLookup = true;
		}, "public class C {}", out _);

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		preparer.Configuration.Application.TrimExportAttributesBlockers.Add ("a test blocker is active");
		new ComputeExportAttributeRemovalStep ().Process (context);

		Assert.That (preparer.Configuration.Application.TrimExportAttributes, Is.False, "Removal");
	}

	[Test]
	public void NSXpcInterfaceMethodInfoOverload ()
	{
		var code = """
			using System.Reflection;
			using Foundation;
			using ObjCRuntime;

			public class C {
				public void GetAllowedClasses (NSXpcInterface value, MethodInfo method)
				{
					value.GetAllowedClasses (method, 0, false);
				}

				public void SetAllowedClasses (NSXpcInterface value, MethodInfo method, NSSet<Class> classes)
				{
					value.SetAllowedClasses (method, classes, 0, false);
				}
			}
			""";

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, preparer => {
			preparer.Registrar = RegistrarMode.TrimmableStatic;
			preparer.TrimExportAttributes = true;
			preparer.Optimizations.RemoveDynamicRegistrar = true;
			preparer.Optimizations.OptimizeBlockLiteralSetupBlock = true;
			preparer.Optimizations.StaticBlockToDelegateLookup = true;
		}, code, out _);

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new RegistrarRemovalTrackingStep ().Process (context);
		new ComputeExportAttributeRemovalStep ().Process (context);

		Assert.That (preparer.Configuration.Application.TrimExportAttributes, Is.False, "Removal");
		Assert.That (preparer.Configuration.Application.TrimExportAttributesBlockers.Single (), Does.Contain ("NSXpcInterface"), "Blocker");
	}
}
