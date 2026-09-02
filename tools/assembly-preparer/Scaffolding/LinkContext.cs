// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Mono.Cecil;
using Xamarin.Bundler;

namespace Mono.Linker;

public class LinkContext {
	Dictionary<string, string> custom_data = new Dictionary<string, string> ();

	AnnotationStore annotations = new AnnotationStore ();
	public AnnotationStore Annotations { get => annotations; }

	public List<AssemblyDefinition> Assemblies = new List<AssemblyDefinition> ();

	public AssemblyDefinition [] GetAssemblies () { return Assemblies.ToArray (); }

	public LinkerConfiguration Configuration { get; private set; }

	public LinkerConfiguration LinkerConfiguration { get => Configuration; }

	public LinkContext (LinkerConfiguration configuration)
	{
		Configuration = configuration;
	}

	public TypeDefinition Resolve (TypeReference type)
	{
		return Configuration.MetadataResolver.Resolve (type);
	}

	public AssemblyDefinition? GetLoadedAssembly (string name)
	{
		return Assemblies.SingleOrDefault (v => v.Name.Name == name);
	}

	public void SetCustomData (string key, string value)
	{
		custom_data [key] = value;
	}

	public bool TryGetCustomData (string key, [NotNullWhen (true)] out string? value)
	{
		return custom_data.TryGetValue (key, out value);
	}
}
