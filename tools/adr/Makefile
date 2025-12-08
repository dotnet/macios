TOP=../..

include $(TOP)/Make.config
include $(TOP)/mk/xamarin.mk

DOWNLOAD_STAMP_FILE=.download-$(ADR_NUGET_VERSION).stamp

$(DOWNLOAD_STAMP_FILE):
	$(Q) $(DOTNET) restore download-adr.csproj /bl:$@.binlog $(MSBUILD_VERBOSITY) /p:AdrNuGetVersion=$(ADR_NUGET_VERSION) /p:AdrRuntimeIdentifier=$(ADR_RUNTIME_IDENTIFIER)
	$(Q) touch $@

clean-local::
	$(Q) rm -rf .*.stamp obj .*.binlog

TARGETS += $(DOWNLOAD_STAMP_FILE)

all-local:: $(TARGETS)
install-local:: $(TARGETS)
download: $(DOWNLOAD_STAMP_FILE)
