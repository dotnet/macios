// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

#nullable enable

namespace Xamarin.Linker {
	// Holds the state for a per-assembly companion TypeMap assembly (_<Asm>.TypeMap.dll)
	// that's shared between ManagedRegistrarStep and TrimmableRegistrarStep when
	// HotReloadCompatibleBuild is enabled.
	internal sealed class RegistrarCompanionAssembly {
		// The companion assembly itself.
		public AssemblyDefinition Assembly;

		// The output path for the companion assembly.
		public string Path;

		// The constructor of the [IgnoresAccessChecksTo] attribute defined in the companion assembly.
		public MethodDefinition IgnoresAccessChecksToCtor;

		// The set of assemblies the companion needs access to (an [IgnoresAccessChecksTo] attribute
		// is emitted for each of these). The user assembly is always included.
		public HashSet<AssemblyDefinition> AccessesAssemblies = new ();

		// The '__Registrar_Callbacks__' type that holds the [UnmanagedCallersOnly] trampolines.
		// Created lazily by ManagedRegistrarStep when the first trampoline is emitted.
		public TypeDefinition? CallbacksType;

		public RegistrarCompanionAssembly (AssemblyDefinition assembly, string path, MethodDefinition ignoresAccessChecksToCtor)
		{
			Assembly = assembly;
			Path = path;
			IgnoresAccessChecksToCtor = ignoresAccessChecksToCtor;
		}

		// Returns the companion assembly for the given user assembly, creating it (an empty assembly
		// with an [IgnoresAccessChecksTo] attribute definition and access to the user assembly) if it
		// doesn't already exist. This must be called when no assembly is set as the current assembly
		// in the AppBundleRewriter (it temporarily sets and clears the current assembly itself).
		public static RegistrarCompanionAssembly GetOrCreate (LinkerConfiguration configuration, AssemblyDefinition userAssembly)
		{
			if (configuration.RegistrarCompanionAssemblies.TryGetValue (userAssembly, out var existing))
				return existing;

			var abr = configuration.AppBundleRewriter;
			var app = configuration.Application;
			var annotations = configuration.Context.Annotations;

			var copyFrom = abr.PlatformAssembly.MainModule;
			var moduleParameters = new ModuleParameters {
				Kind = copyFrom.Kind,
				Runtime = copyFrom.Runtime,
				Architecture = copyFrom.Architecture,
				AssemblyResolver = copyFrom.AssemblyResolver,
				MetadataResolver = copyFrom.MetadataResolver,
			};

			var name = new AssemblyNameDefinition ("_" + userAssembly.Name.Name + ".TypeMap", new Version (1, 0, 0, 0));
			var companion = AssemblyDefinition.CreateAssembly (name, name.Name, moduleParameters);
			var path = System.IO.Path.Combine (app.TypeMapOutputDirectory, name.Name + ".dll");
			annotations.SetAction (companion, annotations.GetAction (userAssembly));

			abr.SetCurrentAssembly (companion);

			/*
			 * [assembly: IgnoresAccessChecksTo ("...")]
			 */
			var ignoredAccessChecks = new TypeDefinition ("System.Runtime.CompilerServices", "IgnoresAccessChecksToAttribute", TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, abr.System_Attribute);
			var ignoredAccessChecksCtor = new MethodDefinition (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, abr.System_Void);
			ignoredAccessChecksCtor.AddParameter (abr.System_String); // assemblyName
			var il = ignoredAccessChecksCtor.Body.GetILProcessor ();
			il.Append (il.Create (OpCodes.Ldarg_0));
			il.Append (il.Create (OpCodes.Call, abr.System_Attribute__ctor));
			il.Append (il.Create (OpCodes.Ret));
			ignoredAccessChecks.Methods.Add (ignoredAccessChecksCtor);
			companion.MainModule.Types.Add (ignoredAccessChecks);

			abr.ClearCurrentAssembly ();

			var result = new RegistrarCompanionAssembly (companion, path, ignoredAccessChecksCtor);
			result.AccessesAssemblies.Add (userAssembly);
			configuration.RegistrarCompanionAssemblies [userAssembly] = result;
			return result;
		}
	}
}
