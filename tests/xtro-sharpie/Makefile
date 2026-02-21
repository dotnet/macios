TOP=../..

include $(TOP)/Make.config

SHARPIE ?= $(TOP)/tools/sharpie/Sharpie.Bind.Tool/bin/Release/osx-arm64/publish/Sharpie.Bind.Tool

XCODE=$(abspath $(XCODE_DEVELOPER_ROOT)/../..)

XTRO_DOTNET_PLATFORMS=$(DOTNET_PLATFORMS)

ANNOTATIONS_DIR=.
DOTNET_ANNOTATIONS_DIR=api-annotations-dotnet

all-local::

clean-local::
	rm -rf */bin */obj
	rm -f *.tmp

XTRO_REPORT=xtro-report/bin/Debug/xtro-report
XTRO_REPORT_EXEC=$(XTRO_REPORT)
XTRO_SANITY=xtro-sanity/bin/Debug/xtro-sanity
XTRO_SANITY_EXEC=$(XTRO_SANITY)
XTRO_SHARPIE=xtro-sharpie/bin/Debug/xtro-sharpie
XTRO_SHARPIE_EXEC=$(XTRO_SHARPIE)

build: $(XTRO_SHARPIE) $(XTRO_REPORT) $(XTRO_SANITY)

$(XTRO_SHARPIE): $(wildcard xtro-sharpie/*.cs) $(wildcard xtro-sharpie/*.csproj) $(XTRO_REPORT)
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-sharpie/xtro-sharpie.csproj /bl:xtro-sharpie.binlog $(DOTNET_BUILD_VERBOSITY)

$(XTRO_REPORT): $(wildcard xtro-report/*.cs) $(wildcard xtro-report/*.csproj) xtro-sharpie/Filter.cs Makefile
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-report/xtro-report.csproj /bl:xtro-report.binlog $(DOTNET_BUILD_VERBOSITY)

$(XTRO_SANITY): $(wildcard xtro-sanity/*.cs) $(wildcard xtro-sanity/*.csproj) $(wildcard $(TOP)/tools/common/*.cs) Makefile
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-sanity/xtro-sanity.csproj /bl:xtro-sanity.binlog $(DOTNET_BUILD_VERBOSITY)

define DotNetAssemblyPath
ifdef TESTS_USE_SYSTEM
X$(2)_DOTNET ?= $(DOTNET_DIR)/packs/$($(2)_NUGET_RUNTIME_MANAGED_NAME)/$($(2)_WORKLOAD_VERSION)/runtimes/$(DOTNET_$(2)_RUNTIME_IDENTIFIERS_NO_ARCH)/lib/$(DOTNET_TFM)/$(DOTNET_$(2)_ASSEMBLY_NAME).dll
else
X$(2)_DOTNET ?= $(DOTNET_DESTDIR)/$($(2)_NUGET_RUNTIME_MANAGED_NAME)/runtimes/$(DOTNET_$(2)_RUNTIME_IDENTIFIERS_NO_ARCH)/lib/$(DOTNET_TFM)/$(DOTNET_$(2)_ASSEMBLY_NAME).dll
endif
endef
$(foreach platform,$(XTRO_DOTNET_PLATFORMS),$(eval $(call DotNetAssemblyPath,$(platform),$(shell echo $(platform) | tr a-z A-Z))))

CORETELEPHONY_HEADERS = \
	-i CoreTelephony/CoreTelephonyDefines.h \
	-i CoreTelephony/CTCall.h \
	-i CoreTelephony/CTCallCenter.h \
	-i CoreTelephony/CTCarrier.h \
	-i CoreTelephony/CTCellularData.h \
	-i CoreTelephony/CTCellularPlanProperties.h \
	-i CoreTelephony/CTCellularPlanProvisioning.h \
	-i CoreTelephony/CTCellularPlanProvisioningRequest.h \
	-i CoreTelephony/CTCellularPlanStatus.h \
	-i CoreTelephony/CTSubscriber.h \
	-i CoreTelephony/CTSubscriberInfo.h \
	-i CoreTelephony/CTTelephonyNetworkInfo.h \

SWIFT_FRAMEWORKS = \
	CoreTransferable \
	LockedCameraCapture \
	TranslationUIProvider \

COMMON_IGNORED_FRAMEWORKS = \
	$(SWIFT_FRAMEWORKS) \
	_CoreNFC_UIKit \
	AssetsLibrary \
	BrowserKit \
	FactoryOTALogger \
	FactoryOTANetworkUtils \
	FactoryOTAWifiUtils \
	JavaNativeFoundation \
	Matter \
	ParavirtualizedGraphics \

IGNORED_IOS_FRAMEWORKS = \
	$(COMMON_IGNORED_FRAMEWORKS) \
	ClockKit \

IGNORED_TVOS_FRAMEWORKS = \
	$(COMMON_IGNORED_FRAMEWORKS) \

IGNORED_MACOS_FRAMEWORKS = \
	$(COMMON_IGNORED_FRAMEWORKS) \
	AccessorySetupKit \
	CompositorServices \
	DriverKit \
	Kerberos \
	IOUSBHost \
	Ruby \
	Tk \

IGNORED_MACCATALYST_FRAMEWORKS = \
	$(COMMON_IGNORED_FRAMEWORKS) \
	_CoreNFC_UIKit \
	AccessorySetupKit \
	AGL \
	AudioVideoBridging \
	CalendarStore \
	Carbon \
	ClockKit \
	CompositorServices \
	DiscRecordingUI \
	DriverKit \
	FSKit \
	GLKit \
	Hypervisor \
	ICADevices \
	InputMethodKit \
	InstallerPlugins \
	IOBluetooth \
	IOBluetoothUI \
	IOUSBHost \
	Kerberos \
	LDAP \
	Python \
	Quartz \
	QuickLookUI \
	Ruby \
	SecurityInterface \
	Tk \
	Virtualization \

COMMON_SHARPIE_ARGUMENTS = \
	-a arm64 \
	-modules true \
	--clang-resource-dir $(TOP)/tools/sharpie/clang \

IOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \
	--platform-assembly $(XIOS_DOTNET) \

TVOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	--platform-assembly $(XTVOS_DOTNET) \

MACCATALYST_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \
	--platform-assembly $(XMACCATALYST_DOTNET) \

MACOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \
	--platform-assembly $(XMACOS_DOTNET) \

IOS_PLATFORM=iphoneos$(IOS_SDK_VERSION)
TVOS_PLATFORM=appletvos$(TVOS_SDK_VERSION)
MACOS_PLATFORM=macosx$(MACOS_SDK_VERSION)
MACCATALYST_PLATFORM=ios$(MACCATALYST_SDK_VERSION)-macabi

XIOS_RID = ios-arm64
XTVOS_RID = tvos-arm64
XMACOS_RID = osx-arm64
XMACCATALYST_RID = maccatalyst-arm64

define DotNetAssembly
$(1).rsp: Makefile
	$$(Q_GEN) echo \
		-s $($(2)_PLATFORM) $$($(2)_SHARPIE_ARGUMENTS) $$(foreach framework,$$(IGNORED_$(2)_FRAMEWORKS),-exclude $$(framework)) \
		--output api/$(1) \
		> $$@

api/$(1)/ApiDefinition.cs: $(1).rsp
	$$(Q_GEN) $$(SHARPIE) sdk-db @$(1).rsp

gen-$(3): api/$(1)/ApiDefinition.cs
gen-all:: gen-$(3)
endef
$(foreach platform,$(XTRO_DOTNET_PLATFORMS),$(eval $(call DotNetAssembly,$(platform),$(shell echo $(platform) | tr a-z A-Z),$(shell echo $(platform) | tr A-Z a-z))))

ifeq ($(XCODE_IS_STABLE),false)
all: gen-all
endif

report-dotnet/index.html: $(XTRO_REPORT) .stamp-dotnet-classify
	$(Q) rm -rf report-dotnet
	$(Q_GEN) $(XTRO_REPORT_EXEC) $(DOTNET_ANNOTATIONS_DIR) report-dotnet

report-dotnet/report.zip: report-dotnet/index.html
	$(Q) rm -f "$@"
	$(Q) cd report-dotnet && zip -r9 report.zip . -x report.zip

dotnet-report: report-dotnet/index.html

report-short:
	JENKINS_SERVER_COOKIE=1 make report

define DotNetClassify
xtro-sharpie-$(1).rsp: Makefile $(1).rsp
	$$(Q_GEN) echo \
		--output-directory $$(DOTNET_ANNOTATIONS_DIR) --lib $(DOTNET_BCL_DIR) $$(X$(2)_DOTNET) --rsp $(abspath $(1).rsp) \
		> $$@

.stamp-dotnet-classify-$(1): $(XTRO_SHARPIE) $$(X$(2)_DOTNET) xtro-sharpie-$(1).rsp
	$$(Q) rm -f $$(DOTNET_ANNOTATIONS_DIR)/$(1)-*.raw
	$$(Q_GEN) $(XTRO_SHARPIE_EXEC) @xtro-sharpie-$(1).rsp
	$$(Q) touch $$@

dotnet-classify-$(1): .stamp-dotnet-classify-$(1)
DOTNET_CLASSIFY += .stamp-dotnet-classify-$(1)
endef
$(foreach platform,$(XTRO_DOTNET_PLATFORMS),$(eval $(call DotNetClassify,$(platform),$(shell echo $(platform) | tr a-z A-Z))))

.stamp-dotnet-classify: $(XTRO_SANITY) $(DOTNET_CLASSIFY)
	$(Q_GEN) $(XTRO_SANITY_EXEC) $(abspath $(DOTNET_ANNOTATIONS_DIR)) "$(ALL_DOTNET_PLATFORMS)" $(XTRO_DOTNET_PLATFORMS)
	$(Q) touch $@

dotnet-classify: .stamp-dotnet-classify

insane:
	XTRO_SANITY_SKIP=1 make all

remove-empty:
	find . -size 0 | xargs git rm

all: dotnet-report

remove-empty-files:
	find . -empty -exec git rm -f {} \;

U2TODO = u2todo/bin/Debug/u2todo.dll
$(U2TODO): $(wildcard u2todo/*.cs u2todo/*.csproj xtro-sharpie/Filter.cs)
	$(Q) unset MSBUILD_EXE_PATH && $(DOTNET) build u2todo/u2todo.csproj $(DOTNET_BUILD_VERBOSITY) /bl:$@.binlog
	$(Q) touch $@

unclassified2todo: $(U2TODO)
	cd $(DOTNET_ANNOTATIONS_DIR) && $(DOTNET) exec $(abspath $(U2TODO))

run-tests run-unit-tests:
	$(Q) $(MAKE) -C UnitTests $@

PROFILE_PATH:=/tmp/clangsharp-$(shell date "+%Y-%m-%d_%H-%M-%S").trace
profile: MacCatalyst.rsp
	dotnet publish $(TOP)/tools/sharpie/Sharpie.Bind.Tool
	xctrace record --output $(PROFILE_PATH) --template 'Time Profiler' --target-stdout - --launch -- $(TOP)/tools/sharpie/Sharpie.Bind.Tool/bin/Release/osx-arm64/publish/Sharpie.Bind.Tool sdk-db @MacCatalyst.rsp
	@echo "Created: $(PROFILE_PATH)"
