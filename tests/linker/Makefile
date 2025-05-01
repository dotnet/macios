TOP = ../..

include $(TOP)/Make.config

build-all:
	$(Q) $(MAKE) build-all -C "dont link/dotnet"
	$(Q) $(MAKE) build-all -C "link all/dotnet"
	$(Q) $(MAKE) build-all -C "link sdk/dotnet"
	$(Q) $(MAKE) build-all -C "trimmode copy/dotnet"
	$(Q) $(MAKE) build-all -C "trimmode link/dotnet"
