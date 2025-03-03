TOP=../..
include $(TOP)/Make.config

# Example TEST_FILTER:
#    TEST_FILTER="--filter FullyQualifiedName~BuildMyCocoaApp"
# Docs: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test#filter-option-details

build-bindings-analyzer-tests:
	$(Q) $(DOTNET) build Microsoft.Macios.Bindings.Analyzer.Tests/Microsoft.Macios.Bindings.Analyzer.Tests.csproj $(DOTNET_BUILD_VERBOSITY)

run-bindings-analyzer-tests:
	$(Q) $(DOTNET) test Microsoft.Macios.Bindings.Analyzer.Tests/Microsoft.Macios.Bindings.Analyzer.Tests.csproj -l:"console;verbosity=normal" $(TEST_FILTER)

build-generator-tests:
	$(Q) $(DOTNET) build Microsoft.Macios.Generator.Tests/Microsoft.Macios.Generator.Tests.csproj $(DOTNET_BUILD_VERBOSITY)

run-generator-tests:
	$(Q) $(DOTNET) test Microsoft.Macios.Generator.Tests/Microsoft.Macios.Generator.Tests.csproj -l:"console;verbosity=normal" $(TEST_FILTER)

build-transformer-tests:
	$(Q) $(DOTNET) build Microsoft.Macios.Transformer.Tests/Microsoft.Macios.Transformer.Tests.csproj $(DOTNET_BUILD_VERBOSITY)

run-transformer-tests:
	$(Q) $(DOTNET) test Microsoft.Macios.Transformer.Tests/Microsoft.Macios.Transformer.Tests.csproj -l:"console;verbosity=normal" $(TEST_FILTER)

build build-all: build-bindings-analyzer-tests build-generator-tests build-transformer-tests

run-tests run test: run-bindings-analyzer-tests run-generator-tests run-transformer-tests
