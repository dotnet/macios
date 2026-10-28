TOP=../..
include $(TOP)/Make.config

ASSEMBLY=bin/Debug/netstandard2.0/Microsoft.Macios.Analyzers.dll
PACKS=$(foreach platform,$(DOTNET_PLATFORMS),$($(platform)_NUGET_SDK_NAME)) $(foreach platform,$(DOTNET_WINDOWS_PLATFORMS),$($(platform)_NUGET_WINDOWS_SDK_NAME))
INSTALLED_ASSEMBLIES=$(foreach pack,$(PACKS),$(DOTNET_DESTDIR)/$(pack)/analyzers/dotnet/cs/Microsoft.Macios.Analyzers.dll)

.build-stamp: $(wildcard *.cs *.csproj *.resx *.md) $(TOP)/Directory.Build.props
	$(Q_BUILD) $(DOTNET) build Microsoft.Macios.Analyzers.csproj $(DOTNET_BUILD_VERBOSITY) /bl:$@.binlog
	$(Q) touch $@

$(ASSEMBLY): .build-stamp

$(INSTALLED_ASSEMBLIES): $(ASSEMBLY)
	$(Q) mkdir -p $(dir $@)
	$(Q) install -m 644 $< $@

all-local:: $(INSTALLED_ASSEMBLIES)
install-local:: $(INSTALLED_ASSEMBLIES)

run-tests:
	$(Q) $(MAKE) -C $(TOP)/tests/analyzers run-tests
