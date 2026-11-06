TOP=../..
include $(TOP)/Make.config

build:
	$(Q) $(DOTNET) build Microsoft.Macios.Analyzers.Tests.csproj $(DOTNET_BUILD_VERBOSITY)

run-tests:
	$(Q) $(DOTNET) test Microsoft.Macios.Analyzers.Tests.csproj $(DOTNET_BUILD_VERBOSITY)
