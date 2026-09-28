# Cecil-based Tests

Even with the huge variety of tests that we have there's still a few things that are hard to test properly. Some of them can be tested by analyzing the assemblies (IL or metadata). So here we are...

`SignListTest` checks the workload `.nupkg` files in `_build/nupkgs` against `dotnet/Workloads/SignList.xml`, including files in nested ZIPs. Build the workload packages before running the full Cecil suite. To check downloaded packages instead, set `MACIOS_SIGNLIST_NUPKG_DIR` to the directory containing the `.nupkg` files.
