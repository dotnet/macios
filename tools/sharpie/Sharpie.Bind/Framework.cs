// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

namespace Sharpie.Bind;

public sealed class FrameworkException : Exception {
	public FrameworkException (Framework framework, string message, params string [] args)
		: base (String.Format ("{0}: {1}", framework.Name, String.Format (message, args)))
	{
	}
}

public sealed class Framework {
	public required Sdk Sdk { get; set; }
	public required string Name { get; set; }
	public required string Location { get; set; }
	public string UmbrellaHeaderName { get; set; } = "";
	public string ModuleMap { get; set; } = "";

	public string ClangSearchDirectory {
		get { return Path.GetDirectoryName (Location)!; }
	}

	public string LibraryFile {
		get { return Path.Combine (Location, Name); }
	}

	public string HeadersDirectory {
		get { return Path.Combine (Location, "Headers"); }
	}

	public string UmbrellaHeaderFile {
		get { return Path.Combine (HeadersDirectory, (UmbrellaHeaderName ?? Name) + ".h"); }
	}

	public string ModulesDirectory {
		get { return Path.Combine (Location, "Modules"); }
	}

	public string ModuleMapFile {
		get { return Path.Combine (ModulesDirectory, "module.modulemap"); }
	}

	public static Framework Load (Sdk sdk, string frameworkDirectory)
	{
		var framework = new Framework {
			Sdk = sdk,
			Location = frameworkDirectory,
			Name = Path.GetFileNameWithoutExtension (frameworkDirectory),
		};

		if (!Directory.Exists (framework.Location))
			throw new FrameworkException (framework,
				"framework path does not exist: {0}", framework.Location);

		if (File.Exists (framework.ModuleMapFile))
			return framework;

		if (!File.Exists (framework.UmbrellaHeaderFile))
			throw new FrameworkException (framework,
				"Umbrella header file does not exist: {0}", framework.UmbrellaHeaderFile);

		return framework;
	}
}
