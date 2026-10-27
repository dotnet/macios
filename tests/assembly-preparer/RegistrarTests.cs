// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

public class RegistrarTests : BaseClass {
	static CustomAttribute CreateSuppression (ModuleDefinition module, string checkId, string attributeName = "UnconditionalSuppressMessageAttribute")
	{
		var attributeType = new TypeReference ("System.Diagnostics.CodeAnalysis", attributeName, module, module.TypeSystem.CoreLibrary);
		var constructor = new MethodReference (".ctor", module.TypeSystem.Void, attributeType) { HasThis = true };
		constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.String));
		constructor.Parameters.Add (new ParameterDefinition (module.TypeSystem.String));
		var attribute = new CustomAttribute (constructor);
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, "Registrar"));
		attribute.ConstructorArguments.Add (new CustomAttributeArgument (module.TypeSystem.String, checkId));
		return attribute;
	}

	[TestCase (ApplePlatform.iOS, "MT4179", true)]
	[TestCase (ApplePlatform.TVOS, "MT4179", true)]
	[TestCase (ApplePlatform.MacCatalyst, "MT4179", true)]
	[TestCase (ApplePlatform.MacOSX, "MM4179", true)]
	[TestCase (ApplePlatform.iOS, "MT4179:Abstract native object", true)]
	[TestCase (ApplePlatform.iOS, "MM4179", false)]
	[TestCase (ApplePlatform.MacOSX, "MT4179", false)]
	[TestCase (ApplePlatform.iOS, "MT4178", false)]
	[TestCase (ApplePlatform.iOS, "MT41790", false)]
	[TestCase (ApplePlatform.iOS, "IL4179", false)]
	[TestCase (ApplePlatform.iOS, "4179", false)]
	[TestCase (ApplePlatform.iOS, "", false)]
	public void SuppressionCheckId (ApplePlatform platform, string checkId, bool expected)
	{
		using var assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition ("Test", new Version (1, 0)), "Test", ModuleKind.Dll);
		var module = assembly.MainModule;
		var type = new TypeDefinition ("Test", "MyClass", TypeAttributes.Public, module.TypeSystem.Object);
		module.Types.Add (type);
		var method = new MethodDefinition ("Method", MethodAttributes.Public, module.TypeSystem.Void);
		type.Methods.Add (method);
		method.CustomAttributes.Add (CreateSuppression (module, checkId));
		var logger = new TestLogger { Platform = platform };

		Assert.That (ErrorHelper.IsWarningSuppressed (logger, 4179, method), Is.EqualTo (expected));
	}

	[TestCase ("method", true, false)]
	[TestCase ("property", false, true)]
	[TestCase ("getter", false, true)]
	[TestCase ("type", true, true)]
	[TestCase ("enclosing-type", true, true)]
	[TestCase ("assembly", true, true)]
	[TestCase ("module", true, true)]
	[TestCase ("global-method", true, false)]
	[TestCase ("global-property", false, true)]
	[TestCase ("global-type", true, true)]
	[TestCase ("other-method", false, false)]
	[TestCase ("other-global-method", false, false)]
	[TestCase ("invalid-scope", false, false)]
	[TestCase ("missing-target", false, false)]
	[TestCase ("suppress-message", false, false)]
	public void SuppressionScope (string scope, bool suppressMethod, bool suppressGetter)
	{
		using var assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition ("Test", new Version (1, 0)), "Test", ModuleKind.Dll);
		var module = assembly.MainModule;
		var outer = new TypeDefinition ("Test", "Outer", TypeAttributes.Public, module.TypeSystem.Object);
		module.Types.Add (outer);
		var type = new TypeDefinition ("", "MyClass", TypeAttributes.NestedPublic, module.TypeSystem.Object);
		outer.NestedTypes.Add (type);
		var method = new MethodDefinition ("Method", MethodAttributes.Public, module.TypeSystem.Void);
		type.Methods.Add (method);
		var otherMethod = new MethodDefinition ("OtherMethod", MethodAttributes.Public, module.TypeSystem.Void);
		type.Methods.Add (otherMethod);
		var getter = new MethodDefinition ("get_Property", MethodAttributes.Public | MethodAttributes.SpecialName, module.TypeSystem.Object) {
			IsGetter = true,
		};
		type.Methods.Add (getter);
		var property = new PropertyDefinition ("Property", PropertyAttributes.None, module.TypeSystem.Object) { GetMethod = getter };
		type.Properties.Add (property);
		var attribute = CreateSuppression (module, "MT4179", scope == "suppress-message" ? "SuppressMessageAttribute" : "UnconditionalSuppressMessageAttribute");
		ICustomAttributeProvider provider = scope switch {
			"property" => property,
			"getter" => getter,
			"type" => type,
			"enclosing-type" => outer,
			"module" => module,
			"other-method" => otherMethod,
			"assembly" or "global-method" or "global-property" or "global-type" or "other-global-method" or "invalid-scope" or "missing-target" => assembly,
			_ => method,
		};
		if (scope.StartsWith ("global-", StringComparison.Ordinal) || scope == "other-global-method" || scope == "invalid-scope" || scope == "missing-target") {
			var targetScope = scope == "global-type" ? "type" : scope == "invalid-scope" ? "namespace" : "member";
			var target = scope switch {
				"global-type" => "~T:Test.Outer",
				"global-property" => "~P:Test.Outer.MyClass.Property",
				"other-global-method" => "~M:Test.Outer.MyClass.OtherMethod",
				_ => "~M:Test.Outer.MyClass.Method",
			};
			attribute.Properties.Add (new CustomAttributeNamedArgument ("Scope", new CustomAttributeArgument (module.TypeSystem.String, targetScope)));
			if (scope != "missing-target")
				attribute.Properties.Add (new CustomAttributeNamedArgument ("Target", new CustomAttributeArgument (module.TypeSystem.String, target)));
		}
		provider.CustomAttributes.Add (attribute);
		var logger = new TestLogger { Platform = ApplePlatform.iOS };

		Assert.That (ErrorHelper.IsWarningSuppressed (logger, 4179, method), Is.EqualTo (suppressMethod), "Method");
		Assert.That (ErrorHelper.IsWarningSuppressed (logger, 4179, getter), Is.EqualTo (suppressGetter), "Getter");
	}

	[TestCase (false, false)]
	[TestCase (true, false)]
	[TestCase (true, true)]
	public void SuppressRegistrarWarning (bool suppress, bool warnAsError)
	{
		var attribute = suppress ? @"[System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage (""Registrar"", ""MT4179"")]" : "";
		var code = $@"
		using Foundation;
		using ObjCRuntime;

		abstract class NativeObject : INativeObject {{
			public NativeHandle Handle => default;
			protected NativeObject (NativeHandle handle, bool owns) {{ }}
		}}

		class MyClass : NSObject {{
			{attribute}
			[Export (""method:"")]
			public void Method (NativeObject value) {{ }}
		}}
		";
		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.ManagedStatic, code, out var testInfo, extraConfig: warnAsError ? "WarnAsError=4179" : "");
		AssertPrepare (preparer);
		var logger = (TestLogger) preparer.Configuration.Logger;
		Assert.That (logger.Errors, Is.Empty, "Preparation errors");
		Assert.That (logger.Warnings.Count (v => v.Code == 4179), Is.EqualTo (suppress ? 0 : 1));

		var context = preparer.Configuration.DerivedLinkContext;
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var method = assembly.MainModule.Types.Single (v => v.Name == "MyClass").Methods.Single (v => v.Name == "Method");
		var application = preparer.Configuration.Application;
		ErrorHelper.SetWarningLevel (application, ErrorHelper.WarningLevel.Error, 4179);
		var expectedLevel = suppress ? ErrorHelper.WarningLevel.Disable : ErrorHelper.WarningLevel.Error;
		Assert.That (ErrorHelper.CreateWarning (application, 4179, method, "Test warning").GetWarningLevel (application), Is.EqualTo (expectedLevel), "Warnings as errors");
		Assert.That (ErrorHelper.CreateError (application, 4179, method, "Test error").GetWarningLevel (application), Is.EqualTo (ErrorHelper.WarningLevel.Error), "Actual errors cannot be suppressed");

		// Simulate the trimmer removing the attribute and read the suppression from the original assembly.
		var resolver = new DefaultAssemblyResolver ();
		resolver.AddSearchDirectory (Path.GetDirectoryName (testInfo.InputPath));
		application.PreTrimAssemblyResolver = resolver;
		application.IsPostProcessingAssemblies = true;
		var suppression = method.CustomAttributes.SingleOrDefault (v => v.AttributeType.Name == "UnconditionalSuppressMessageAttribute");
		if (suppression is not null)
			method.CustomAttributes.Remove (suppression);
		var warning = ErrorHelper.CreateWarning (application, 4179, method, "Test warning after trimming");
		Assert.That (warning.GetWarningLevel (application), Is.EqualTo (expectedLevel), "Post-trim suppression");
		if (suppress) {
			ErrorHelper.Show (application, warning);
			Xamarin.Linker.LinkerConfiguration.Report (context, warning);
			Assert.That (logger.Warnings, Is.Empty, "Suppressed warnings are not logged");
			Assert.That (logger.Errors, Is.Empty, "Suppressed warnings are not promoted to errors");
		}
	}

	[TestCase (false)]
	[TestCase (true)]
	public void ModelClassAsGenericArgument (bool suppress)
	{
		var attribute = suppress ? @"[System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage (""Registrar"", ""MT4192"")]" : "";
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			" + attribute + @"
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
