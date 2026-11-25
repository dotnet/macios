# Cecil-based Tests

Even with the huge variety of tests that we have there's still a few things that are hard to test properly. Some of them can be tested by analyzing the assemblies (IL or metadata). So here we are...

The suite uses MSTest with the VSTest runner (`Microsoft.NET.Test.Sdk`). After building and installing the workloads, run `make run-tests` in this directory. Use `TEST_NAME=--filter=FullyQualifiedName~TestName` to select tests. CI collects TRX and HTML results.

Tests run sequentially because they share cached Cecil assemblies and metadata. Data-driven tests pass assembly paths, not Cecil objects, so discovery can enumerate individual cases without loading the assemblies or serializing Cecil metadata. Assemblies and symbols are loaded through the existing cache during execution. Tests that do not apply to the current platform, and the opt-in availability warning test, use MSTest's inconclusive outcome (reported as skipped by VSTest). Failure lists include all unexpected and fixed known failures in a single assertion message.

## Performance comparison

Measured on an Apple M1 (8 cores, 16 GB RAM), macOS 27.0.1, with .NET SDK `12.0.100-alpha.1.26523.103` and all four platforms enabled. The original NUnit binaries and the migrated MSTest binaries used the same installed platform assemblies and VSTest runner. Both versions produced the same 130 individual case outcomes: 126 passed and four skipped.

Each framework had one full-suite warm-up followed by five measured runs, alternating which framework ran first. Every run used a fresh `dotnet test <assembly>` process, without building, with the same console and TRX loggers. Discovery was measured separately with five `--list-tests` runs per framework. These are end-to-end wall-clock medians, including process startup:

| Project              | Framework    | Full suite | Discovery |
|----------------------|--------------|------------|-----------|
| `cecil-tests.csproj` | NUnit 4.6.1  | 35.75 s    | 0.849 s   |
| `cecil-tests.csproj` | MSTest 4.5.2 | 37.51 s    | 0.732 s   |

There was no clear full-suite speedup: MSTest's median was about 4.9% slower, but the ranges overlapped (NUnit: 33.47-47.00 s; MSTest: 32.11-41.09 s) and background machine load varied. Discovery saved about 0.12 s. This comparison measures the migration as a whole, including path-based data sources and native assertions, not an isolated test-framework microbenchmark.

## Test details

`SignListTest` checks the workload `.nupkg` files in `DOTNET_NUPKG_DIR` against `dotnet/Workloads/SignList.xml`, including files in nested ZIPs. Build or download the workload packages before running the full Cecil suite; CI copies downloaded packages into this directory when installing workloads. When all platforms are enabled, the test prints signing entries not used by any package without failing. Mach-O executables don't have a known file extension, so the test recognizes them by their Mach-O header. They must be listed in the sign list (except for the ones in an app bundle, which are signed with the bundle, the debug symbols in a `.dSYM`, which can't be signed, and static libraries, which aren't signed), and the files without a known extension that are matched by a `MacDeveloperSign` / `MacDeveloperSignHarden` entry must be Mach-O executables (for example, not a shell script or the debug symbols in a `.dSYM`). App bundles (`*.app` directories with a `Contents/Info.plist`, as on macOS) must be listed in a `MacDeveloperSignBundle` / `MacDeveloperSignHardenBundle` entry, which has the name of the bundle (such as `mlaunch.app`).

`OSPlatformGuardTest.RedundantGuards` checks implementation assemblies for OS-version guards at or below `SdkVersions.GetMinVersion` for the assembly's platform. It recognizes `SupportedOSPlatformGuard` attributes on methods and properties, conventional constant `major`/`minor`/`build`/`revision` arguments on unversioned guards, and the built-in `OperatingSystem.Is*VersionAtLeast` APIs. Dynamic arguments and jumps into argument sequences are not analyzed. Existing failures are tracked in `OSPlatformGuardTest.KnownFailures.cs` using `Helper.AssertFailures`.
