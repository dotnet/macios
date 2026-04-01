using System;
using System.Linq;

using Mono.Cecil;

using Mono.Linker;
using Mono.Linker.Steps;
using Mono.Tuner;

using Xamarin.Bundler;
using Xamarin.Linker;

#nullable enable

namespace Xamarin.Linker.Steps;

public abstract class AssemblyModifierStep : ConfigurationAwareStep {
	private protected AppBundleRewriter abr => Configuration.AppBundleRewriter;

	protected override void TryProcessAssembly (AssemblyDefinition assembly)
	{
		var modified = false;

		abr.SetCurrentAssembly (assembly);
		foreach (var type in assembly.MainModule.Types)
			modified |= ProcessTypeImpl (type);

		if (modified)
			abr.SaveCurrentAssembly ();
		abr.ClearCurrentAssembly ();
	}

	protected virtual bool ProcessType (TypeDefinition type)
	{
		return false;
	}

	bool ProcessTypeImpl (TypeDefinition type)
	{
		var modified = ProcessType (type);
		if (type.HasNestedTypes) {
			foreach (var nested in type.NestedTypes)
				modified |= ProcessTypeImpl (nested);
		}
		return modified;
	}
}
