# Test case for dynamic libraries

include ../shared.mk

XCFrameworkWithSymlinks.resources.zip: Makefile manifest ../../.libs/XTest.xcframework.zip
	$(Q) rm -f $@.tmp
	$(Q) cd ../../.libs && zip -9r --symlinks $(abspath $@.tmp) XTest.xcframework
	$(Q) zip -9 --symlinks "$(abspath $@.tmp)" manifest
	$(Q) mv "$@.tmp" "$@"

$(NUPKG_PATH): XCFrameworkWithSymlinks.resources.zip
