# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

TOP=../../../../../..

include $(TOP)/Make.config
include $(TOP)/mk/colors.mk
include $(TOP)/scripts/run-audio-unit-extension-tests/fragment.mk

# This file is meant to be included from
# tests/monotouch-test/dotnet/extensions/audio-unit/<platform>/Makefile.

BINLOG_TIMESTAMP:=$(shell date +%Y-%m-%d-%H%M%S)
AUVAL_ARGUMENTS?=-v aufx test Xmrn
RUN_TIMEOUT_SECONDS?=600
BUILD_PARAMETERS+=$(BUILD_ARGUMENTS)
ifneq ($(TEST_FILTER),)
BUILD_PARAMETERS+=/p:MonotouchExtensionTestName=$(TEST_FILTER)
endif

ifeq ($(PLATFORM),)
PLATFORM=$(shell basename "$(CURDIR)")
endif

LOGFILENAME:=$(TMPDIR)/monotouch-test/extensions/audio-unit/$(PLATFORM)-$(shell date +%Y-%m-%d--%H:%M:%S).log

ifeq ($(CONFIG),)
ifneq ($(XAMARIN_RUNTIME),)
CONFIG=$(XAMARIN_RUNTIME)
else
CONFIG=Debug
endif
endif
CONFIG_ARGUMENT=/p:Configuration=$(CONFIG)

ifeq ($(RID),)
ifeq ($(PLATFORM),MacCatalyst)
ifeq ($(CONFIG),Release)
RID=maccatalyst-x64;maccatalyst-arm64
else ifneq ($(UNIVERSAL),)
RID=maccatalyst-x64;maccatalyst-arm64
else ifeq ($(shell arch),arm64)
RID=maccatalyst-arm64
else
RID=maccatalyst-x64
endif
else ifeq ($(PLATFORM),macOS)
ifeq ($(CONFIG),Release)
RID=osx-x64;osx-arm64
else ifneq ($(UNIVERSAL),)
RID=osx-x64;osx-arm64
else ifeq ($(shell arch),arm64)
RID=osx-arm64
else
RID=osx-x64
endif
else ifeq ($(PLATFORM),iOS)
RID=iossimulator-arm64
else ifeq ($(PLATFORM),tvOS)
RID=tvossimulator-arm64
else
RID=unknown-platform-$(PLATFORM)
endif
endif

ifneq ($(UNIVERSAL),)
UNIVERSAL_ARGUMENT=/p:UniversalBuild=true
endif

ifneq ($(XAMARIN_RUNTIME),)
USE_MONO_RUNTIME_ARGUMENT=/p:UseMonoRuntime=$(if $(subst MonoVM,,$(XAMARIN_RUNTIME)),false,true)
endif

ifneq ($(findstring ;,$(RID)),)
RID_ARGUMENT=/p:RuntimeIdentifiers=$(RID)
PATH_RID=
else
RID_ARGUMENT=/p:RuntimeIdentifier=$(RID)
PATH_RID=$(RID)/
endif

CONTAINER_PROJECT=$(abspath $(CURDIR))/ContainerApp/ContainerApp.csproj
APP_PATH=$(abspath $(CURDIR))/ContainerApp/bin/$(CONFIG)/$(DOTNET_TFM)-$(shell echo $(PLATFORM) | tr 'A-Z' 'a-z')/$(PATH_RID)ContainerApp.app
EXTENSION_PATH=$(APP_PATH)/Contents/PlugIns/AppExtension.appex
EXECUTABLE=$(APP_PATH)/Contents/MacOS/ContainerApp
LSREGISTER=/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister
prepare:
	@# nothing to do here right now

build: prepare
	$(Q) echo "Building extension test project: $(COLOR_GRAY)$(CONTAINER_PROJECT)$(COLOR_CLEAR) [$(CONFIG) $(RID)]"
	$(Q) rm -rf "$(abspath $(CURDIR))/AppExtension/bin" "$(abspath $(CURDIR))/AppExtension/obj" "$(abspath $(CURDIR))/ContainerApp/bin" "$(abspath $(CURDIR))/ContainerApp/obj"
	$(Q) $(DOTNET) build "$(CONTAINER_PROJECT)" "/bl:$(abspath build-$(BINLOG_TIMESTAMP).binlog)" $(DOTNET_BUILD_VERBOSITY) $(BUILD_PARAMETERS) $(CONFIG_ARGUMENT) $(RID_ARGUMENT) $(UNIVERSAL_ARGUMENT) $(USE_MONO_RUNTIME_ARGUMENT)
	$(Q) echo "Build completed."

register-extension: build
	$(Q) "$(LSREGISTER)" -f "$(APP_PATH)"
	$(Q) pluginkit -a "$(EXTENSION_PATH)"

run: $(RUN_AUDIO_UNIT_EXTENSION_TESTS)
	$(Q) echo "Running monotouch-test from the audio unit extension: $(COLOR_GRAY)$(EXTENSION_PATH)$(COLOR_CLEAR)"
	$(Q) echo "Writing output to: $(COLOR_GRAY)$(LOGFILENAME)$(COLOR_CLEAR)"
	$(Q) $(RUN_AUDIO_UNIT_EXTENSION_TESTS_EXEC) \
		--platform "$(PLATFORM)" \
		--rid "$(RID)" \
		--config "$(CONFIG)" \
		--app "$(APP_PATH)" \
		--extension "$(EXTENSION_PATH)" \
		--executable "$(EXECUTABLE)" \
		--log-file "$(LOGFILENAME)" \
		--timeout-seconds "$(RUN_TIMEOUT_SECONDS)" \
		--lsregister "$(LSREGISTER)" \
		$(if $(TEST_FILTER),--test-filter "$(TEST_FILTER)")

run-bare: run

print-app-path:
	@echo $(APP_PATH)

print-extension-path:
	@echo $(EXTENSION_PATH)

print-executable:
	@echo $(EXECUTABLE)

clean:
	rm -Rf AppExtension/bin AppExtension/obj ContainerApp/bin ContainerApp/obj *.binlog
