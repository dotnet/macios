# Various tests to be executed prior to releases

## Test permissions

There are a number of tests that acccess the file system and the bluetooth. For these tests to correctly execute you have to ensure that
the terminal application that you are using to execute the tests has access to the full filesystem and the bluetooth. If you do not do this
step the macOS tests will crash.

## Types of Tests

### Unit Tests

Most of the projects are using NUnit[Lite] and looks like unit tests.
They are meant to be executed on the target: simulator, devices, macOS.

In reality most of them are regression tests - but that does not change
the need to execute and continually expand them to cover new code.

### Introspection Tests

Introspection tests are executed on target (both simulator and device for
iOS) or a specific version of OSX. The application proceed to analyze itself
using:

* `System.Reflection` for managed code; and
* the ObjectiveC runtime library for native code

and compare the results. E.g. if using .NET reflection it can see a binding
for a `NSBundle` type then it should be able to find a native `NSBundle` 
type using the ObjC runtime functions. Otherwise an error is raised...

Since the application analyze itself it must contains everything we wish
to test. That's why the introspection tests are part of the `dontlink.app`
application (for iOS) and the dontlink-mac project (for OSX).

Pros

* They always tell the truth, which can differ from documentation

Cons

* Incomplete - Not everything is encoded in the metadata / executable;
* Too complete - Not every truth is good to be known (or published)

### Extrospection Tests ###

Extrospection tests takes data from some outside sources and see if our
implementation match the information, e.g.

* Header files from the SDK;
* Rules, like Gendarme or FxCop;

Since this is done externally there's no need to run them on the devices,
simulator or even a specific version of OSX.

Pro

* There is more data available, e.g. information lost when compiling

Con

* The data might not represent the truth (errors, false positives...)

# Test Suites

## *-tests : where * is the assembly name, e.g. monotouch

Use the project defaults for linking, i.e. 

* "Don't link" for simulator

* "Link SDK assemblies only" for devices

## linker/dontlink

* regression testing without using the linker

* both simulator and devices are set to "Don't link"

## linker/linkall

* regression testing using the linker on the all assemblies

* "Link all assemblies" for both simulator/devices

## linker/linksdk

* regression testing using the linker on the SDK assemblies

* "Link SDK assemblies only" for both simulator/devices

# Common make targets

Each test project has a `dotnet` directory, with a subdirectory for each platform we support.

Use the makefile in each of those subdirectories to run the corresponding test suite.

To build the test in question:

    $ make build

To run the test in question:

	$ make run

Some tests have a list of variations that can be build and run, to get a list of all the variations:

    $ make list-variations

And then (note that VARIATION has to be set for both 'make build' and 'make run'):

    $ make build VARIATION=variation
    $ make run VARIATION=variation

