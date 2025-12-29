TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk
include ../common/Make.common

SHARPIE_VERSION_BUILD=0
SHARPIE_VERSION=$(XCODE_VERSION).$(SHARPIE_VERSION_BUILD).$(XCODE_BUMP_COMMIT_DISTANCE)

# Sharpie.Bind.csproj.inc contains the Sharpie.Bind_dependencies variable used to determine if Sharpie.Bind needs to be rebuilt or not.
Sharpie.Bind/Sharpie.Bind.csproj.inc: export BUILD_VERBOSITY=$(MSBUILD_VERBOSITY)
Sharpie.Bind/Sharpie.Bind.csproj.inc: export DOTNET:=$(DOTNET)
-include Sharpie.Bind/Sharpie.Bind.csproj.inc

all-local:: $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) build Sharpie.Bind.Tool/Sharpie.Bind.Tool.csproj $(DOTNET_BUILD_VERBOSITY) -bl:$@-build.binlog
	$(Q_BUILD) $(DOTNET) publish Sharpie.Bind.Tool/Sharpie.Bind.Tool.csproj $(DOTNET_PACK_VERBOSITY) -bl:$@-pack.binlog

run-tests: $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) test $(TOP)/tests/sharpie/Sharpie.Bind.Tests/Sharpie.Bind.Tests.csproj --logger "console;verbosity=detailed" $(DOTNET_BUILD_VERBOSITY) -bl:$@.binlog

pack: $(Sharpie.Bind_dependencies)
	$(Q_BUILD) $(DOTNET) pack Sharpie.Bind.Tool/Sharpie.Bind.Tool.csproj /p:Version=$(SHARPIE_VERSION) $(DOTNET_PACK_VERBOSITY) -bl:$@.binlog

