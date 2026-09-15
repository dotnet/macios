# Assembly preparer

This is a library that will modify assemblies when a project is built for a few reasons:

* Collect required information for a successful build
* Transform some code patterns so that they can be properly recognized and handled correctly by trimmers.
* Optimize some code patterns we can easily recognize
* Precompute some things at build time to be able to make apps smaller and run faster.

Currently it can:

* PreserveCodeBlockHandler: in some cases user assemblies might contain code
  created by the generator that's not trimmer safe; this handler will inject
  code to ensure that trimmers don't trim way some things that shouldn't be
  trimmed away.

## Design principles

* Easy to test (there's a unit test project, VSCode can run & debug its tests)
* Can be called from an MSBuild task (which means it currently needs to target `netstandard2.0`).
* Good error handling/reporting.
* We have two main scenarios:
	* Debug loop, where we shouldn't do more than absolutely necessary to make
	  debug builds as fast as possible. In particular, we'll only do whatever
	  is necessary for a correct build, and if possible, no assembly
	  modification, only information gathering.
	* Release builds, where we want to optimize as much as possible.
* To ease integration with existing custom linker steps, it's provides a
  very simplified API of ILLink's custom linker step API - much of it is
  stubbed out until it's needed.
* Until fully complete and both correctness and performance have been
  validated, it should be possible to use either custom linker steps or the
  assembly preparer, and as such any code that's used by both will have to
  keep working in both modes.

