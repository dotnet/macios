// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// This step is only needed on .NET 10, where ILLink doesn't have the '--typemap-entry-assembly' option.
#if !NET11_0_OR_GREATER

using System.IO;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker {
	// ILLink only processes the type map attributes ([TypeMap], [TypeMapAssociation] and
	// [TypeMapAssemblyTarget]) if it knows which assembly is the type map entry assembly, and in .NET 10 the
	// only way for ILLink to figure that out is by looking at the root assembly - and only if the root
	// assembly is linked (it isn't when the link mode is 'SdkOnly' for instance, because then the root
	// assembly is just copied). When ILLink doesn't know about the type map entry assembly, it won't mark any
	// type map attributes, and it will thus remove all of them from every assembly it links - including the
	// type map assemblies the trimmable registrar generates, leaving the app with empty type maps at runtime.
	//
	// So tell ILLink about the entry assembly ourselves when ILLink didn't figure it out on its own (in
	// .NET 11+ we pass '--typemap-entry-assembly' to ILLink instead, so there this step isn't needed at all).
	public class SetTypeMapEntryAssemblyStep : ConfigurationAwareStep {
		protected override string Name { get; } = "SetTypeMapEntryAssembly";
		protected override int ErrorCode { get; } = 2530;

		protected override void TryProcess ()
		{
			base.TryProcess ();

			if (App.Registrar != RegistrarMode.TrimmableStatic)
				return;

			// The entry assembly accessors aren't part of the API ILLink exposes to custom steps, so we have to
			// use reflection to get to them.
			var annotations = Context.Annotations;
			var getEntryPointAssembly = annotations.GetType ().GetMethod ("GetEntryPointAssembly");
			var setEntryPointAssembly = annotations.GetType ().GetMethod ("SetEntryPointAssembly");
			if (getEntryPointAssembly is null || setEntryPointAssembly is null)
				throw ErrorHelper.CreateError (99, "Could not find the 'GetEntryPointAssembly' and/or the 'SetEntryPointAssembly' methods on the linker's annotation store.");

			// ILLink already knows about the entry assembly, nothing to do here.
			if (getEntryPointAssembly.Invoke (annotations, null) is not null)
				return;

			var entryAssemblyName = Path.GetFileNameWithoutExtension (App.AssemblyName);
			var entryAssembly = Context.GetLoadedAssembly (entryAssemblyName);
			if (entryAssembly is null)
				throw ErrorHelper.CreateError (99, $"Could not find the entry assembly '{entryAssemblyName}' among the assemblies loaded by the linker.");

			setEntryPointAssembly.Invoke (annotations, new object [] { entryAssembly });
		}
	}
}

#endif // !NET11_0_OR_GREATER
