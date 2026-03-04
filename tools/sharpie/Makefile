TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk
include ../common/Make.common

SHARPIE_VERSION_BUILD=0
SHARPIE_VERSION=$(XCODE_VERSION).$(SHARPIE_VERSION_BUILD).$(XCODE_BUMP_COMMIT_DISTANCE)$(NUGET_PRERELEASE_IDENTIFIER)

# Sharpie.Bind.csproj.inc contains the $(Sharpie.Bind_dependencies) variable used to determine if Sharpie.Bind needs to be rebuilt or not.
Sharpie.Bind/Sharpie.Bind.csproj.inc: export BUILD_VERBOSITY=$(MSBUILD_VERBOSITY)
Sharpie.Bind/Sharpie.Bind.csproj.inc: export DOTNET:=$(DOTNET)
-include Sharpie.Bind/Sharpie.Bind.csproj.inc

SHARPIE_BIND_TOOL_DEBUG=Sharpie.Bind.Tool/bin/Debug/osx-arm64/Sharpie.Bind.Tool

all-local:: $(SHARPIE_BIND_TOOL_DEBUG)
$(SHARPIE_BIND_TOOL_DEBUG): $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) build Sharpie.Bind.Tool/Sharpie.Bind.Tool.csproj $(DOTNET_BUILD_VERBOSITY) -bl:$@-build.binlog

SHARPIE_BIND_TOOL_NUGET_VERSION=$(if $(filter 0,$(XCODE_BUMP_COMMIT_DISTANCE)),$(XCODE_VERSION).$(SHARPIE_VERSION_BUILD)$(NUGET_PRERELEASE_IDENTIFIER),$(SHARPIE_VERSION))
SHARPIE_BIND_TOOL_NUPKG_NAME=Sharpie.Bind.Tool.$(SHARPIE_BIND_TOOL_NUGET_VERSION).nupkg
SHARPIE_BIND_TOOL_NUPKG=Sharpie.Bind.Tool/bin/Release/$(SHARPIE_BIND_TOOL_NUPKG_NAME)

pack: $(SHARPIE_BIND_TOOL_NUPKG)
$(SHARPIE_BIND_TOOL_NUPKG): $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) pack Sharpie.Bind.Tool/Sharpie.Bind.Tool.csproj "/p:Version=$(SHARPIE_VERSION)" "/p:CurrentBranch=$(CURRENT_BRANCH)" "/p:CurrentHash=$(CURRENT_HASH_LONG)" $(DOTNET_PACK_VERBOSITY) -bl:$@.binlog

all-local:: $(DOTNET_NUPKG_DIR)/$(SHARPIE_BIND_TOOL_NUPKG_NAME)
$(DOTNET_NUPKG_DIR)/$(SHARPIE_BIND_TOOL_NUPKG_NAME): $(SHARPIE_BIND_TOOL_NUPKG)
	$(Q) mkdir -p $(dir $@)
	$(Q) $(CP) $(dir $<)/*.nupkg $(dir $@)

run-tests: $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) test $(TOP)/tests/sharpie/Sharpie.Bind.Tests/Sharpie.Bind.Tests.csproj --logger "console;verbosity=detailed" $(DOTNET_BUILD_VERBOSITY) -bl:$@.binlog

generated-files: $(abspath ../common/SdkVersions.cs) $(abspath ../common/ProductConstants.cs)

show-version:
	@echo "        Sharpie.Bind.Tool $(SHARPIE_BIND_TOOL_NUGET_VERSION)"
