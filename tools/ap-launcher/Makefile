TOP=../..
include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

build:
	$(Q_BUILD) $(DOTNET) build /bl:$@.binlog *.csproj $(DOTNET_BUILD_VERBOSITY)

all-local:: build
