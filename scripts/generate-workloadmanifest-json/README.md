# generate-workloadmanifest-json

This script generates the `WorkloadManifest.json` file, which is a file that describes what's in a workload.

The generated manifest references the shared `Microsoft.NET.Runtime.MonoTargets.Sdk` packs directly for every supported .NET version. These build targets are still used with CoreCLR; extending the Apple runtime workloads would also inherit their host-platform restrictions and prevent installation on Linux.

For frameworks older than .NET 11, the mobile workloads additionally reference the Mono runtime and AOT tooling packs required by those frameworks. Host-specific AOT compiler aliases select the compiler on supported hosts without preventing workload installation on other hosts. .NET 11 and later do not install those Mono runtime or compiler packs.

More information about workload manifests can be found here:
https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workload-manifest.md
