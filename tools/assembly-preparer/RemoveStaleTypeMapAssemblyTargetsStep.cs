// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;

using Xamarin.Bundler;

namespace MonoTouch.Tuner {
	// The trimmable static registrar creates one type map assembly per assembly in the app, and references
	// each of them with a [TypeMapAssemblyTarget<TUniverse>] attribute in the root type map assembly.
	//
	// The trimmer may end up removing every type from a type map assembly (if none of the corresponding
	// Objective-C types are used by the app), in which case the trimmer deletes the assembly altogether -
	// but it doesn't remove the corresponding [TypeMapAssemblyTarget<TUniverse>] attributes. The runtime
	// will then try to load an assembly that doesn't exist, and the app crashes at startup.
	//
	// So remove any [TypeMapAssemblyTarget<TUniverse>] attributes that point to assemblies that no longer exist.
	public class RemoveStaleTypeMapAssemblyTargetsStep : ConfigurationAwareStep {
		protected override string Name { get; } = "RemoveStaleTypeMapAssemblyTargets";
		protected override int ErrorCode { get; } = 2560;

		const string TypeMapAssemblyTargetAttribute = "TypeMapAssemblyTargetAttribute`1";

		protected override void TryProcess ()
		{
			var configuration = Configuration;
			var abr = configuration.AppBundleRewriter;
			var existingAssemblies = new HashSet<string> (configuration.Assemblies.Select (v => v.Name.Name));

			foreach (var assembly in configuration.Assemblies) {
				if (!assembly.HasCustomAttributes)
					continue;

				var staleAttributes = assembly.CustomAttributes.Where (ca => IsStale (ca, existingAssemblies)).ToList ();
				if (staleAttributes.Count == 0)
					continue;

				abr.SetCurrentAssembly (assembly);
				foreach (var ca in staleAttributes) {
					configuration.Log ($"Removing the [{TypeMapAssemblyTargetAttribute}] attribute pointing to '{ca.ConstructorArguments [0].Value}' from {assembly.Name.Name}, because that assembly doesn't exist anymore.");
					assembly.CustomAttributes.Remove (ca);
				}
				abr.SaveCurrentAssembly ();
				abr.ClearCurrentAssembly ();
			}
		}

		static bool IsStale (CustomAttribute ca, HashSet<string> existingAssemblies)
		{
			var attributeType = ca.AttributeType;
			if (attributeType.Name != TypeMapAssemblyTargetAttribute)
				return false;
			if (attributeType.Namespace != "System.Runtime.InteropServices")
				return false;
			if (!ca.HasConstructorArguments || ca.ConstructorArguments.Count != 1)
				return false;
			if (ca.ConstructorArguments [0].Value is not string assemblyName)
				return false;

			// The value is an assembly name, which may or may not be fully qualified.
			var comma = assemblyName.IndexOf (',');
			if (comma >= 0)
				assemblyName = assemblyName.Substring (0, comma);

			return !existingAssemblies.Contains (assemblyName.Trim ());
		}
	}
}
