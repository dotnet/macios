// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.Macios.Generator.Tests;

public class BaseTestDataGenerator {
	public static string ReadFileAsString (string file, [CallerFilePath] string filePath = "", bool isGlobal = GeneratorConfiguration.UseGlobalNamespace)
	{
		var directoryPath = Path.GetDirectoryName (filePath);
		var fullPath = Path.Join (directoryPath, "Data", file);
		return isGlobal
			? File.ReadAllText (fullPath).Replace ("$GLOBAL$", "global::")
			: File.ReadAllText (fullPath).Replace ("$GLOBAL$", "");
	}
}
