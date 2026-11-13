// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Linker.Steps;

namespace AssemblyPreparerTests;

public class DisposeFieldClearsStepTests : BaseClass {
	[TestCase (true)]
	[TestCase (false)]
	public void RestoreFieldClearOnlyWhenFieldSurvivesTrimming (bool keepField)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			NSObject cached;

			[BindingImpl (BindingImplOptions.Optimizable)]
			protected override void Dispose (bool disposing) {
				base.Dispose (disposing);
				if (Handle == IntPtr.Zero)
					cached = null;
			}
		}
		";

		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, p => p.Registrar = RegistrarMode.Dynamic, code, out _);
		AssertPrepare (preparer);

		var context = preparer.Configuration.DerivedLinkContext;
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var type = assembly.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "Dispose");
		Assert.That (method.Body.Instructions.Any (v => v.OpCode.Code == Code.Stfld), Is.False, "Field store is delayed");
		Assert.That (method.Body.Instructions.Any (v => v.OpCode.Code == Code.Ldstr && v.Operand is string marker && marker.StartsWith ("__macios_dispose_field__:", StringComparison.Ordinal)), Is.True, "Field marker");

		if (!keepField)
			type.Fields.Remove (type.Fields.Single (v => v.Name == "cached"));

		preparer.Configuration.Application.IsPostProcessingAssemblies = true;
		new RestoreDisposeFieldClearsStep ().Process (context);

		var hasFieldStore = method.Body.Instructions.Any (v => v.OpCode.Code == Code.Stfld && v.Operand is FieldReference field && field.Name == "cached");
		Assert.That (hasFieldStore, Is.EqualTo (keepField), "Field store restored only if field survives trimming");
		Assert.That (method.Body.Instructions.Any (v => v.OpCode.Code == Code.Ldstr && v.Operand is string marker && marker.StartsWith ("__macios_dispose_field__:", StringComparison.Ordinal)), Is.False, "No field markers remain");
	}
}
