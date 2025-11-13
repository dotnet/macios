TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

.libs/osx-arm64/spawner: .libs/osx-arm64/spawner.o
	$(Q_CCLD) $(CLANG) -arch $(DOTNET_osx-arm64_ARCHITECTURES) $< -o$@ -isysroot $(macos_SDK)

.libs/osx-x64/spawner: .libs/osx-x64/spawner.o
	$(Q_CCLD) $(CLANG) -arch $(DOTNET_osx-x64_ARCHITECTURES) $< -o$@ -isysroot $(macos_SDK)

spawner: .libs/osx-arm64/spawner .libs/osx-x64/spawner
	$(Q_LIPO) $(LIPO) $^ -create -output $@
	$(Q) chmod +x $@

all-local:: spawner
