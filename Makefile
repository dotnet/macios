TOP=.
SUBDIRS=builds
include $(TOP)/Make.config
include $(TOP)/mk/versions.mk

# On Linux, skip directories that require native compilation or macOS platform
ifndef IS_LINUX
SUBDIRS += runtime
endif

SUBDIRS += src

ifndef ONLY_SHARPIE
SUBDIRS += msbuild
endif

SUBDIRS += tools

ifndef IS_LINUX
SUBDIRS += dotnet
endif

#
# Common
#

all-local:: check-system
install-local::

.PHONY: world
world: check-system
	@$(MAKE) reset-versions
	@$(MAKE) all -j8
	@$(MAKE) install -j8
	@echo "Build is done, the following workloads were built:"
	@$(DOTNET) workload list

.PHONY: check-system
check-system:
	@./system-dependencies.sh
	$(Q) $(MAKE) show-versions

show-versions:
	@echo "Building:"
	@echo "    The .NET NuGet(s):"
	@$(foreach platform,$(DOTNET_PLATFORMS),echo "        Microsoft.$(platform) $($(shell echo $(platform) | tr 'a-z' 'A-Z')_NUGET_VERSION_FULL)";)
	@$(MAKE) -C tools/sharpie show-version

all-local:: global.json

# This tells NuGet to use the exact same dotnet version we've configured in Make.config
global.json: $(TOP)/dotnet.config Makefile $(GIT_DIRECTORY)/HEAD $(GIT_DIRECTORY)/index
	$(Q_GEN) \
		printf "{\n" > $@; \
		printf "  \"sdk\": {\n" >> $@; \
		printf "    \"version\": \"$(DOTNET_VERSION)\",\n" >> $@; \
		printf "    \"paths\": [\n" >> $@; \
		printf "      \"builds/downloads/dotnet\",\n" >> $@; \
		printf "      \"\$$host\$$\"\n" >> $@; \
		printf "    ],\n" >> $@; \
		printf "    \"errorMessage\": \"The .NET SDK could not be found, please run 'make dotnet -C builds'.\"\n" >> $@; \
		printf "  },\n" >> $@; \
		printf "  \"tools\": {\n    \"dotnet\": \"$(DOTNET_VERSION)\"\n  },\n" >> $@; \
		printf "  \"msbuild-sdks\": {\n    \"Microsoft.DotNet.Arcade.Sdk\": \"$(ARCADE_VERSION)\"\n  }\n" >> $@; \
		printf "}\n" >> $@

install-hook::
	$(Q) if ! git diff --exit-code global.json; then \
		echo "Error: global.json has changed: please commit the changes."; \
		exit 1; \
	fi

dotnet-install-system:
	$(Q) $(MAKE) -C dotnet install-system

fix-xcode-select:
	sudo xcode-select -s $(XCODE_DEVELOPER_ROOT)

fix-xcode-first-run:
	$(XCODE_DEVELOPER_ROOT)/usr/bin/xcodebuild -runFirstLaunch

install-dotnet:
	@echo "Figuring out package link..."
	@export PKG=$$(make -C builds print-dotnet-pkg-urls); \
	echo "Downloading $$(basename $$PKG)..."; \
	curl -LO "$$PKG"; \
	echo "Installing $$(basename $$PKG)..."; \
	time sudo installer -pkg "$$(basename $$PKG)" -target / -verbose -dumplog

git-clean-all:
	@echo "$(COLOR_RED)Cleaning and resetting all dependencies. This is a destructive operation.$(COLOR_CLEAR)"
	@echo "$(COLOR_RED)You have 5 seconds to cancel (Ctrl-C) if you wish.$(COLOR_CLEAR)"
	@sleep 5
	@echo "Cleaning macios..."
	@git clean -xffdq
	@echo "Cleaning submodules..."
	@git submodule foreach -q --recursive 'git clean -xffdq && git reset --hard -q'
	@echo "Done"

SUBDIRS += tests

