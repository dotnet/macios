TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

# The spawner is a host tool: it always runs on the macOS build machine, and is
# needed regardless of which platforms are enabled. Don't use the generic
# per-RID rules from mk/rules.mk, because those are only defined for the
# platforms that are enabled - so when macOS is disabled (which is the case for
# most of the test jobs in CI) the .libs/osx-*/spawner.o rules don't exist, and
# the build fails with "No rule to make target '.libs/osx-arm64/spawner.o'".
# For the same reason we compute the macOS SDK path directly instead of relying
# on $(macos_SDK).

SPAWNER_ARCHS=arm64 x86_64
SPAWNER_SDK=$(shell $(XCODE_DEVELOPER_ROOT)/usr/bin/xcodebuild -version -sdk macosx Path 2>/dev/null)

define SpawnerTemplate
.libs/$(1):
	$$(Q) mkdir -p $$@

.libs/$(1)/spawner.o: spawner.c | .libs/$(1)
	$$(Q_CC) $$(CLANG) -arch $(1) -isysroot $$(SPAWNER_SDK) -c $$< -o $$@

.libs/$(1)/spawner: .libs/$(1)/spawner.o
	$$(Q_CCLD) $$(CLANG) -arch $(1) -isysroot $$(SPAWNER_SDK) $$< -o $$@
endef
$(foreach arch,$(SPAWNER_ARCHS),$(eval $(call SpawnerTemplate,$(arch))))

spawner: $(foreach arch,$(SPAWNER_ARCHS),.libs/$(arch)/spawner)
	$(Q_LIPO) $(LIPO) $^ -create -output $@
	$(Q) chmod +x $@

all-local:: spawner
