TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk
include ../common/Make.common

# assembly-preparer.csproj.inc contains the $(assembly_preparer_dependencies) variable used to determine if assembly-preparer needs to be rebuilt or not.
# Don't include (and thus try to rebuild) the .inc file when the target is
# generated-files, because MSBuild may invoke 'make generated-files' in parallel
# for multiple target frameworks, and the script that creates the .inc file
# doesn't handle concurrent invocations (it uses shared intermediate files).
ifneq ($(MAKECMDGOALS),generated-files)
assembly-preparer.csproj.inc: export BUILD_VERBOSITY=$(MSBUILD_VERBOSITY)
-include assembly-preparer.csproj.inc
endif

ASSEMBLY_PREPARER_NETCORE=bin/Debug/$(DOTNET_TFM)/assembly-preparer.dll
ASSEMBLY_PREPARER_NETSTD=bin/Debug/netstandard2.0/assembly-preparer.dll
ASSEMBLIES=$(ASSEMBLY_PREPARER_NETCORE) $(ASSEMBLY_PREPARER_NETSTD)

.build-stamp: $(assembly_preparer_dependencies)
	$(Q_BUILD) $(DOTNET) build /bl:$@.binlog *.csproj $(DOTNET_BUILD_VERBOSITY)
	$(Q) touch $(ASSEMBLIES)

$(ASSEMBLIES): .build-stamp

all-local:: .build-stamp

run-tests:
	$(Q_BUILD) $(DOTNET) test $(TOP)/tests/assembly-preparer/assembly-preparer-tests.csproj --logger "console;verbosity=detailed" $(DOTNET_BUILD_VERBOSITY) -bl:$@.binlog

generated-files: $(abspath ../common/SdkVersions.cs) $(abspath ../common/ProductConstants.cs)
