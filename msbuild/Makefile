TOP = ..

include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

ifdef INCLUDE_IOS
BUILD_IOS=1
endif
ifdef INCLUDE_TVOS
BUILD_IOS=1
endif
ifdef INCLUDE_MACCATALYST
BUILD_IOS=1
endif

#
# New .targets and .props files are picked up automatically if they're in a directory with already
# existing .targets or .props files. If .targets or .props are added in a new directory, consider
# changing your mind and putting them with any existing .targets or .props files.
#

##
## Common definitions
##

MSBUILD_PRODUCTS             = 
MSBUILD_DIRECTORIES          =

ALL_SOURCES:= \
	$(shell git ls-files | sed 's/ /\\ /g') \
	$(wildcard $(XAMARIN_MACDEV_PATH)/Xamarin.MacDev/*.cs) \
	$(wildcard $(XAMARIN_MACDEV_PATH)/Xamarin.MacDev/*.csproj) \
	Versions.g.cs \
	$(wildcard $(TOP)/tools/common/*.cs) \

CONFIG      = Debug
TARGETFRAMEWORK = netstandard2.0
NETTARGETFRAMEWORK = $(DOTNET_TFM)
WINDOWSRUNTIMEIDENTIFIER = win

LOCALIZATION_LANGUAGES=cs de es fr it ja ko pl pt-BR ru tr zh-Hans zh-Hant
TRANSLATED_ASSEMBLIES = $(foreach locale,$(LOCALIZATION_LANGUAGES),$(locale)/Xamarin.MacDev.Tasks)

##
## iOS definitions
## 

IOS_WINDOWS_TARGETS =                                                      \
	$(wildcard Xamarin.iOS.Tasks.Windows/Xamarin.*.props)       \
	$(wildcard Xamarin.iOS.Tasks.Windows/Xamarin.*.targets)       \
	$(wildcard Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(NETTARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/Xamarin.*.props)       \
	$(wildcard Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(NETTARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/Xamarin.*.targets)       \

TASK_ASSEMBLIES = Xamarin.MacDev.Tasks
IOS_WINDOWS_TASK_ASSEMBLIES = Xamarin.iOS.Tasks.Windows
IOS_WINDOWS_DEPENDENCIES = System.Diagnostics.Tracer System.Formats.Asn1 System.Buffers System.Memory System.Numerics.Vectors System.Runtime.CompilerServices.Unsafe System.Security.Cryptography.ProtectedData System.Security.Cryptography.Pkcs Microsoft.Win32.Registry

PROPS_AND_TARGETS = \
	$(wildcard Xamarin.Shared/*.props)   \
	$(wildcard Xamarin.Shared/*.targets) \

MSBUILD_TASK_ASSEMBLIES += $(TASK_ASSEMBLIES)

##
## .NET targets ##
##

DOTNET_SHARED_ASSEMBLIES = \
	$(foreach dll,$(MSBUILD_TASK_ASSEMBLIES),$(dll).dll $(dll).pdb)

define InstallFiles

DOTNET_TARGETS += \
	$(foreach target,$(PROPS_AND_TARGETS),$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(notdir $(target))) \
	$(foreach target,$(DOTNET_SHARED_ASSEMBLIES),$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(TARGETFRAMEWORK)/$(notdir $(target))) \
	$(foreach dll,$(TRANSLATED_ASSEMBLIES),$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(TARGETFRAMEWORK)/$(dll).resources.dll) \
	.copy-stamp-$(1) \

DOTNET_DIRECTORIES_$(1) += \
	$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild \
	$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(TARGETFRAMEWORK) \
	$(foreach locale,$(LOCALIZATION_LANGUAGES),$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(TARGETFRAMEWORK)/$(locale)) \

DOTNET_DIRECTORIES += $$(DOTNET_DIRECTORIES_$(1))

$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/%: Xamarin.Shared/% | $(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild
	$$(Q) install -m 644 $$< $$@

$(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(TARGETFRAMEWORK)/%: Xamarin.MacDev.Tasks/bin/$(CONFIG)/$(TARGETFRAMEWORK)/% | $$(DOTNET_DIRECTORIES_$(1))
	$$(Q) install -m 644 $$< $$@

.copy-stamp-$(1): $$(wildcard Xamarin.MacDev.Tasks/bin/$(CONFIG)/$(NETTARGETFRAMEWORK)/*) $$(wildcard Xamarin.MacDev.Tasks/bin/$(CONFIG)/$(NETTARGETFRAMEWORK)/*/*) .build-stamp | $(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild
	$$(Q) rm -rf $(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(NETTARGETFRAMEWORK)/
	$$(Q) $(CP) -r Xamarin.MacDev.Tasks/bin/$(CONFIG)/$(NETTARGETFRAMEWORK) $(DOTNET_DESTDIR)/$($(2)_NUGET_SDK_NAME)/tools/msbuild/$(NETTARGETFRAMEWORK)
	$$(Q) touch $$@
endef

$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(call InstallFiles,$(platform),$(call uppercase,$(platform)))))

MSBUILD_DIRECTORIES += $(DOTNET_DIRECTORIES)

MSBUILD_PRODUCTS += $(DOTNET_TARGETS)

DOTNET_IOS_WINDOWS_OUTPUT_FILES =                                \
	$(foreach dll,$(IOS_WINDOWS_TASK_ASSEMBLIES),$(dll).*)       \
	$(foreach dll,$(IOS_WINDOWS_DEPENDENCIES),$(dll).*)          \
	Broker.zip                                                   \

DOTNET_IOS_WINDOWS_FILES = $(IOS_WINDOWS_TARGETS) $(foreach file,$(DOTNET_IOS_WINDOWS_OUTPUT_FILES),Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/$(file))
DOTNET_IOS_WINDOWS_FILES += Messaging/Xamarin.Messaging.Build/obj/$(CONFIG)/Build.zip

ifneq ($(filter iOS,$(DOTNET_PLATFORMS)),)
.copy-stamp-iOS: .copy-stamp-iOS-configure-mac

.copy-stamp-iOS-configure-mac: .build-stamp Xamarin.iOS.Tasks.Windows/Xamarin.ConfigureMac.targets
	$(Q) mkdir -p $(DOTNET_DESTDIR)/$(IOS_NUGET_SDK_NAME)/tools/msbuild/ConfigureMac
	$(Q) $(CP) Xamarin.iOS.Tasks.Windows/Xamarin.ConfigureMac.targets Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/Broker.zip Messaging/Xamarin.Messaging.Build/obj/$(CONFIG)/Build.zip $(DOTNET_DESTDIR)/$(IOS_NUGET_SDK_NAME)/tools/msbuild/
	$(Q) rsync -a --delete --delete-excluded --exclude='*.pdb' Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/ConfigureMac/ $(DOTNET_DESTDIR)/$(IOS_NUGET_SDK_NAME)/tools/msbuild/ConfigureMac/
	$(Q) touch $@
endif

.copy-windows-files: .build-stamp
	$(Q) mkdir -p $(DOTNET_DESTDIR)/$(IOS_NUGET_WINDOWS_SDK_NAME)/tools/msbuild/iOS/ConfigureMac
	$(Q) $(CP) $(DOTNET_IOS_WINDOWS_FILES) $(DOTNET_DESTDIR)/$(IOS_NUGET_WINDOWS_SDK_NAME)/tools/msbuild/iOS/
	$(Q) rsync -a --delete --delete-excluded --exclude='*.pdb' Xamarin.iOS.Tasks.Windows/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(WINDOWSRUNTIMEIDENTIFIER)/ConfigureMac/ $(DOTNET_DESTDIR)/$(IOS_NUGET_WINDOWS_SDK_NAME)/tools/msbuild/iOS/ConfigureMac/

.dotnet-windows: .build-stamp .copy-windows-files

# Build Xamarin.Messaging.Remote from a source checkout during normal builds:
#
#     export MESSAGING_REMOTE_REPO=/absolute/path/to/dotnet-macios-devtools
#     make -C msbuild all install
#
# Each build packs a fresh version and prints the package path and version.
# Unset MESSAGING_REMOTE_REPO to use the tracked version on the next build.
#
# Or use an existing local package for ConfigureMac in the installed SDKs:
#
#     make -C msbuild use-local-messaging-remote MESSAGING_REMOTE_PACKAGE=/path/to/Xamarin.Messaging.Remote.<version>.nupkg
#
# The target prints the staged package version. To check it, look for
# "Xamarin.Messaging.Remote/<version>" in Xamarin.iOS.Tasks.Windows/obj/project.assets.json.
# Use a normalized three-part version without build metadata that isn't on a
# configured feed, and don't use a package in its own NuGet cache directory.
# To go back to the tracked version, run:
#
#     rm -f msbuild/.build-stamp && make -C msbuild all install
ifneq ($(filter iOS,$(DOTNET_PLATFORMS)),)
use-local-messaging-remote:
	$(Q) DOTNET="$(DOTNET)" MAKE="$(MAKE)" ./stage-local-messaging-remote.sh "$(MESSAGING_REMOTE_PACKAGE)"
endif

ifndef NO_XCODE
all-local:: .dotnet-windows
dotnet:: .dotnet-windows
endif

##
## Common targets ##
##

# we must install locally during 'make all', because the F# build depends on the msbuild targets/assemblies.
all-local:: $(MSBUILD_PRODUCTS)
ifndef NO_XCODE
all-local:: .stamp-test-xml
endif

# I haven't found a way to execute ilrepack.exe on .NET (it will probably have to be built for .NET), so run it using Mono.

ifneq ($(strip $(MESSAGING_REMOTE_REPO)$(wildcard obj/messaging-remote-repo)),)
.PHONY: force-messaging-remote-build
.build-stamp: force-messaging-remote-build
endif

.build-stamp: $(ALL_SOURCES)
ifneq ($(strip $(MESSAGING_REMOTE_REPO)),)
	$(Q) mkdir -p obj
	$(Q) touch obj/messaging-remote-repo
	$(Q) MESSAGING_REMOTE_REPO="$(MESSAGING_REMOTE_REPO)" DOTNET="$(DOTNET)" MAKE="$(MAKE)" ./stage-local-messaging-remote.sh --build
else
	$(Q) $(DOTNET) build "/bl:$@.binlog" $(XBUILD_VERBOSITY) /p:NuGetPackageRoot=$(abspath $(TOP)/packages)/
	$(Q) rm -f obj/messaging-remote-repo
endif
	$(Q) touch $@

# make all the target assemblies build when any of the sources have changed
$(foreach dll,$(MSBUILD_TASK_ASSEMBLIES),$(dll)/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(dll).dll $(dll)/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(dll).pdb): .build-stamp
$(foreach dll,$(TRANSLATED_ASSEMBLIES),Xamarin.MacDev.Tasks/bin/$(CONFIG)/$(TARGETFRAMEWORK)/$(dll).resources.dll): .build-stamp

# this is a directory-creating target.
# we sort to remove duplicates
$(sort $(MSBUILD_DIRECTORIES)):
	$(Q) mkdir -p $@

install-local:: $(MSBUILD_PRODUCTS)

.stamp-test-xml: $(wildcard */*.targets) $(wildcard */*.props)
	$(Q) xmllint --noout $^
	@echo Targets files are valid XML
	@touch $@

test run-test run-tests tests run-tests-msbuild:
	$(Q) $(MAKE) -C $(TOP)/tests run-tests-msbuild

clean-local::
	git clean -xfdq
	cd $(XAMARIN_MACDEV_PATH) && git clean -xfdq

Versions.g.cs: Makefile $(TOP)/Make.config.inc
	$(Q) printf "internal static class VersionConstants {\\n" > $@.tmp
	$(Q) printf "\\tpublic const string NuGetVersion = \"$($(call uppercase,$(firstword $(DOTNET_PLATFORMS)))_NUGET_VERSION)\";\\n" >> $@.tmp
	$(Q) echo $(foreach platform,$(ALL_DOTNET_PLATFORMS),"\\tpublic const string Microsoft_$(platform)_Version = \"$($(call uppercase,$(platform))_NUGET_VERSION_FULL)\";\\n") >> $@.tmp
	$(Q) echo $(foreach platform,$(ALL_DOTNET_PLATFORMS),"\\tpublic const string Microsoft_$(platform)_Standard_Version = \"$($(call uppercase,$(platform))_NUGET_VERSION_MAJOR).$($(call uppercase,$(platform))_NUGET_VERSION_MINOR).0.$($(call uppercase,$(platform))_NUGET_VERSION_PATCH)\";\\n") >> $@.tmp
	$(Q) printf "\\tpublic const string NuGetPrereleaseIdentifier = \"$(NUGET_PRERELEASE_IDENTIFIER)\";\\n" >> $@.tmp
	$(Q) printf "\\tpublic const string NuGetBuildMetadata = \"$(NUGET_BUILD_METADATA)\";\\n" >> $@.tmp
	$(Q) printf "\\tpublic const string Branch = \"$(CURRENT_BRANCH)\";\\n" >> $@.tmp
	$(Q) printf "\\tpublic const string Commit = \"$(CURRENT_HASH)\";\\n" >> $@.tmp
	$(Q) printf "}\\n" >> $@.tmp
	$(Q) mv $@.tmp $@

dotnet:: $(DOTNET_TARGETS)

.PHONY: install-remote-locally
install-remote-locally:
	PACKAGES_DIR="$(abspath $(DOTNET_NUPKG_DIR))" FEED_DIR="$(abspath $(DOTNET_DESTDIR)/feed)" CONFIG_FILE="$(abspath $(DOTNET_DESTDIR)/feed/NuGet.config)" "$(TOP)/tools/devops/automation/scripts/prepare-remote-nuget-config.sh"
	@# it doesn't matter which project we use, it just has to be *a* project.
	dotnet build /t:ConfigureMac -bl -p:ServerAddress=localhost -p:ServerUser="$(USER)" -tl:off -p:ResetRemoteCache=true -p:RemoteNuGetConfigFile="$(abspath $(DOTNET_DESTDIR)/feed/NuGet.config)" -v:diag "$(TOP)/tests/dotnet/MySimpleApp/iOS/MySimpleApp.csproj"
