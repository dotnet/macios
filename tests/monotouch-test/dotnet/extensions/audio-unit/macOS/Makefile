# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

include ../shared.mk

# To run monotouch-test from the audio unit extension with MonoVM:
#    make run-monovm
# To run monotouch-test from the audio unit extension with CoreCLR:
#    make run-coreclr
#
# Any arguments to 'dotnet build' can be set using the BUILD_PARAMETERS variable.
# Set TEST_FILTER to run a specific test instead of the full suite.

FORWARDED_MAKE_VARIABLES=TEST_FILTER="$(TEST_FILTER)" BUILD_ARGUMENTS="$(BUILD_ARGUMENTS)" BUILD_PARAMETERS="$(BUILD_PARAMETERS)" RID="$(RID)" UNIVERSAL="$(UNIVERSAL)" RUN_TIMEOUT_SECONDS="$(RUN_TIMEOUT_SECONDS)"

build-monovm:
	$(Q) $(MAKE) XAMARIN_RUNTIME=MonoVM CONFIG=MonoVM $(FORWARDED_MAKE_VARIABLES) build

build-coreclr:
	$(Q) $(MAKE) XAMARIN_RUNTIME=CoreCLR CONFIG=CoreCLR $(FORWARDED_MAKE_VARIABLES) build

run-monovm:
	$(Q) $(MAKE) XAMARIN_RUNTIME=MonoVM CONFIG=MonoVM $(FORWARDED_MAKE_VARIABLES) build
	$(Q) $(MAKE) XAMARIN_RUNTIME=MonoVM CONFIG=MonoVM $(FORWARDED_MAKE_VARIABLES) run

run-coreclr:
	$(Q) $(MAKE) XAMARIN_RUNTIME=CoreCLR CONFIG=CoreCLR $(FORWARDED_MAKE_VARIABLES) build
	$(Q) $(MAKE) XAMARIN_RUNTIME=CoreCLR CONFIG=CoreCLR $(FORWARDED_MAKE_VARIABLES) run
