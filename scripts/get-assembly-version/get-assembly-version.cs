using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

if (args.Length == 0) {
	Console.Error.WriteLine ("The path to the assembly is required.");
	return 1;
}

if (args.Length > 1) {
	Console.Error.WriteLine ("Only one assembly is supported.");
	return 1;
}

var dll = args [0];

using var fs = new FileStream (dll, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
using var peReader = new PEReader (fs);

var mr = peReader.GetMetadataReader ();
var version = mr.GetAssemblyDefinition ().Version;
// This script is used to get the assembly version before comparing two assemblies for API compatibility.
// If we're building a release build, and the two assemblies have the same Major + Minor version, then we
// must do a strict API compatibility check, where no API additions are allowed. This is why we only care
// about Major + Minor here.
var versionString = $"{version.Major}.{version.Minor}";
Console.WriteLine (versionString);

return 0;
