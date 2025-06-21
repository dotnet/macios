TOP=../../../..
include $(TOP)/tests/common/shared-dotnet.mk

clean-hook::
	rm -rf $(TOP)/tests/dotnet/LibraryWithResources/*/bin $(TOP)/tests/dotnet/LibraryWithResources/*/obj
	rm -rf $(TOP)/tests/dotnet/SecondLibraryWithResources/*/bin $(TOP)/tests/dotnet/SecondLibraryWithResources/*/obj
