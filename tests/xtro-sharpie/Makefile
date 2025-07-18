TOP=../..

include $(TOP)/Make.config

# a 64bits mono is required because of the clang requirement
MONO ?= mono64 --debug
SHARPIE ?= sharpie

XCODE=$(abspath $(XCODE_DEVELOPER_ROOT)/../..)

XTRO_DOTNET_PLATFORMS=$(DOTNET_PLATFORMS)

ANNOTATIONS_DIR=.
DOTNET_ANNOTATIONS_DIR=api-annotations-dotnet

all-local::

clean-local::
	rm -rf */bin */obj
	rm -f *.tmp
	rm -rf *os*.pch*

XTRO_REPORT=xtro-report/bin/Debug/xtro-report
XTRO_REPORT_EXEC=$(XTRO_REPORT)
XTRO_SANITY=xtro-sanity/bin/Debug/xtro-sanity
XTRO_SANITY_EXEC=$(XTRO_SANITY)
XTRO_SHARPIE=xtro-sharpie/bin/Debug/xtro-sharpie.exe
XTRO_SHARPIE_EXEC=$(MONO) --debug $(XTRO_SHARPIE)

build: $(XTRO_SHARPIE) $(XTRO_REPORT) $(XTRO_SANITY)

$(XTRO_SHARPIE): $(wildcard xtro-sharpie/*.cs) $(wildcard xtro-sharpie/*.csproj) pch-info.proj $(XTRO_REPORT)
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-sharpie/xtro-sharpie.csproj /bl:xtro-sharpie.binlog $(DOTNET_BUILD_VERBOSITY)

$(XTRO_REPORT): $(wildcard xtro-report/*.cs) $(wildcard xtro-report/*.csproj) xtro-sharpie/Filter.cs Makefile
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-report/xtro-report.csproj /bl:xtro-report.binlog $(DOTNET_BUILD_VERBOSITY)

$(XTRO_SANITY): $(wildcard xtro-sanity/*.cs) $(wildcard xtro-sanity/*.csproj) $(wildcard $(TOP)/tools/common/*.cs) Makefile
	$(Q_GEN) unset MSBUILD_EXE_PATH && $(DOTNET) build xtro-sanity/xtro-sanity.csproj /bl:xtro-sanity.binlog $(DOTNET_BUILD_VERBOSITY)

CORETELEPHONY_HEADERS = \
	-i CoreTelephony/CoreTelephonyDefines.h \
	-i CoreTelephony/CTCarrier.h \
	-i CoreTelephony/CTCall.h \
	-i CoreTelephony/CTCallCenter.h \
	-i CoreTelephony/CTTelephonyNetworkInfo.h \
	-i CoreTelephony/CTSubscriber.h \
	-i CoreTelephony/CTSubscriberInfo.h \

SWIFT_FRAMEWORKS = \
	CoreTransferable \
	LockedCameraCapture \
	TranslationUIProvider \

COMMON_IGNORED_FRAMEWORKS = \
	$(SWIFT_FRAMEWORKS) \
	_CoreNFC_UIKit \
	AssetsLibrary \
	BrowserEngineCore \
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

IGNORED_MACCATALYST_FRAMEWORKS = \
	$(COMMON_IGNORED_FRAMEWORKS) \
	_CoreNFC_UIKit \
	AccessorySetupKit \
	AGL \
	AudioVideoBridging \
	CalendarStore \
	Carbon \
	ClockKit \
	DiscRecordingUI \
	FSKit \
	GLKit \
	ICADevices \
	InputMethodKit \
	InstallerPlugins \
	IOBluetooth \
	IOBluetoothUI \
	LDAP \
	Python \
	Quartz \
	QuickLookUI \
	SecurityInterface \
	Virtualization \

COMMON_SHARPIE_ARGUMENTS = \
	--xcode $(XCODE) \
	-a arm64 \
	-modules false \

IOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \

TVOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \

MACCATALYST_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \

MACOS_SHARPIE_ARGUMENTS = \
	$(COMMON_SHARPIE_ARGUMENTS) \
	$(CORETELEPHONY_HEADERS) \

IOS_PLATFORM=iphoneos$(IOS_SDK_VERSION)
TVOS_PLATFORM=appletvos$(TVOS_SDK_VERSION)
MACOS_PLATFORM=macosx$(MACOS_SDK_VERSION)
MACCATALYST_PLATFORM=ios$(MACCATALYST_SDK_VERSION)-macabi

XIOS_RID = ios-arm64
XTVOS_RID = tvos-arm64
XMACOS_RID = osx-arm64
XMACCATALYST_RID = maccatalyst-arm64

define DotNetAssembly
X$(2)_PCH = $($(2)_PLATFORM)-arm64.pch
$$(X$(2)_PCH): .stamp-check-sharpie
	$(SHARPIE) sdk-db -s $($(2)_PLATFORM) $$($(2)_SHARPIE_ARGUMENTS) $$(foreach framework,$$(IGNORED_$(2)_FRAMEWORKS),-exclude $$(framework))

pch:: $$(X$(2)_PCH)

ifdef TESTS_USE_SYSTEM
X$(2)_DOTNET ?= $(DOTNET_DIR)/packs/$($(2)_NUGET_RUNTIME_MANAGED_NAME)/$($(2)_WORKLOAD_VERSION)/runtimes/$(DOTNET_$(2)_RUNTIME_IDENTIFIERS_NO_ARCH)/lib/$(DOTNET_TFM)/$(DOTNET_$(2)_ASSEMBLY_NAME).dll
else
X$(2)_DOTNET ?= $(DOTNET_DESTDIR)/$($(2)_NUGET_RUNTIME_MANAGED_NAME)/runtimes/$(DOTNET_$(2)_RUNTIME_IDENTIFIERS_NO_ARCH)/lib/$(DOTNET_TFM)/$(DOTNET_$(2)_ASSEMBLY_NAME).dll
endif

$(3)-$($(2)_SDK_VERSION).g.cs: .stamp-check-sharpie $$(X$(2)_PCH)
	$$(Q_GEN) $$(SHARPIE) query -bind $$(X$(2)_PCH) > $$@.tmp
	$$(Q) mv $$@.tmp $$@

gen-$(3): $(3)-$$($(2)_SDK_VERSION).g.cs
gen-all:: gen-$(3)
endef
$(foreach platform,$(XTRO_DOTNET_PLATFORMS),$(eval $(call DotNetAssembly,$(platform),$(shell echo $(platform) | tr a-z A-Z),$(shell echo $(platform) | tr A-Z a-z))))

ifeq ($(XCODE_IS_STABLE),false)
all: gen-all
endif

pch-info.proj: Makefile
	$(Q) rm -f $@.tmp
	$(Q) printf "<Project>\\n" >> $@.tmp
	$(Q) printf "\\t<PropertyGroup>\\n" >> $@.tmp
	$(Q) printf "$(foreach platform,$(ALL_DOTNET_PLATFORMS),\\t\\t<$(platform)_PCH>$(X$(shell echo $(platform) | tr '[:lower:]' '[:upper:]')_PCH)</$(platform)_PCH>\\n)" >> $@.tmp
	$(Q) printf "$(foreach platform,$(ALL_DOTNET_PLATFORMS),\\t\\t<$(platform)_DLL>$(X$(shell echo $(platform) | tr '[:lower:]' '[:upper:]')_DOTNET)</$(platform)_DLL>\\n)" >> $@.tmp
	$(Q) printf "\\t</PropertyGroup>\\n" >> $@.tmp
	$(Q) printf "</Project>\\n" >> $@.tmp
	$(Q_GEN) mv $@.tmp $@

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
.stamp-dotnet-classify-$(1): $(XTRO_SHARPIE) $$(X$(2)_PCH) $$(X$(2)_DOTNET)
	$$(Q) rm -f $$(DOTNET_ANNOTATIONS_DIR)/$(1)-*.raw
	$$(Q_GEN) $(XTRO_SHARPIE_EXEC) --output-directory $$(DOTNET_ANNOTATIONS_DIR) --lib $(DOTNET_BCL_DIR) $$(X$(2)_PCH) $$(X$(2)_DOTNET)
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

.stamp-check-sharpie:
	@$(TOP)/system-dependencies.sh --ignore-all --enforce-sharpie
	@touch $@
	
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
