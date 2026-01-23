// Copyright 2011-2013 Xamarin Inc. All rights reserved.

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Linker;
using Mono.Tuner;

using Xamarin.Tuner;

namespace Xamarin.Linker.Steps {

	public class ApplyPreserveAttribute : ApplyPreserveAttributeBase {
		// We need to run the ApplyPreserveAttribute step even if we're only linking sdk assemblies, because even
		// though we know that sdk assemblies will never have Preserve attributes, user assemblies may have
		// [assembly: LinkSafe] attributes, which means we treat them as sdk assemblies and those may have
		// Preserve attributes.
		public override bool IsActiveFor (AssemblyDefinition assembly)
		{
			return Annotations.GetAction (assembly) == AssemblyAction.Link;
		}

		protected override void Process (AssemblyDefinition assembly)
		{
			base.Process (assembly);
			ProcessAssemblyAttributes (assembly);
		}

		void ProcessAssemblyAttributes (AssemblyDefinition assembly)
		{
			if (!assembly.HasCustomAttributes)
				return;

			foreach (var attribute in assembly.CustomAttributes) {
				if (!attribute.Constructor.DeclaringType.Is (Namespaces.Foundation, "PreserveAttribute"))
					continue;

				if (!attribute.HasConstructorArguments)
					continue;
				var tr = (attribute.ConstructorArguments [0].Value as TypeReference);
				if (tr is null)
					continue;

				// we do not call `this.ProcessType` since
				// (a) we're potentially processing a different assembly and `is_active` represent the current one
				// (b) it will try to fetch the [Preserve] attribute on the type (and it's not there) as `base` would
				var type = tr.Resolve ();

				PreserveType (type, attribute);
			}
		}

		protected override bool IsPreservedAttribute (ICustomAttributeProvider provider, CustomAttribute attribute, out bool removeAttribute)
		{
			removeAttribute = false;
			TypeReference type = attribute.Constructor.DeclaringType;

			if (type.Name == "PreserveAttribute") {
				// there's no need to keep the [Preserve] attribute in the assembly once it was processed
				removeAttribute = true;
				return true;
			}
			return false;
		}
	}
}
