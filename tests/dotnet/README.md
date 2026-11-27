# .net tests

## size-comparison

To install the latest `appcompare` tool do:

```bash
$ dotnet tool install --global appcompare
```

You can update it to the latest version by running:

```bash
$ dotnet tool update --global appcompare
```

The current directory might point to a different and incompatible dotnet
SDK, resulting in an error. However running the command from a different
location (outside the repo) should work.

### Easier Analysis

Assembly IL stripping is no longer performed by the Apple SDK build targets. The
legacy `--nostrip` option and `EnableAssemblyILStripping=false` are harmless;
`--nostrip:false` and `EnableAssemblyILStripping=true` are unsupported. Native
symbol stripping and IL trimming are separate operations.
