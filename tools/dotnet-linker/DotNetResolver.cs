using System;

using Mono.Cecil;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker {
	public class DotNetResolver : CoreResolver {
		public DotNetResolver (Application app)
		{
		}

		public override AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
		{
#if ASSEMBLY_PREPARER
			if (cache.TryGetValue (name.Name, out var assembly))
#else
			if (cache.TryGetValue (name.Name, out var assembly) && assembly.Name.FullName == name.FullName)
#endif
				return assembly;
			throw new NotImplementedException ($"Unable to resolve the assembly reference {name}");
		}
	}
}
