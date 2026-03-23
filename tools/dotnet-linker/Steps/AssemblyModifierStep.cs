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

	protected sealed override void TryProcessAssembly (AssemblyDefinition assembly)
	{
		var modified = false;

		abr.SetCurrentAssembly (assembly);
		modified |= ModifyAssembly (assembly);
		if (modified)
			abr.SaveCurrentAssembly ();
		abr.ClearCurrentAssembly ();
	}

	protected virtual bool ModifyAssembly (AssemblyDefinition assembly)
	{
		var modified = false;
		foreach (var type in assembly.MainModule.Types)
			modified |= ProcessTypeImpl (type);
		return modified;
	}

	protected virtual bool ProcessType (TypeDefinition type)
	{
		return false;
	}

	protected virtual bool ProcessMethod (MethodDefinition method)
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

	protected bool ProcessMethods (TypeDefinition type)
	{
		if (!type.HasMethods)
			return false;

		var modified = false;
		foreach (var method in type.Methods)
			modified |= ProcessMethod (method);
		return modified;
	}
}
