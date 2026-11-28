include $(TOP)/mk/colors.mk
include $(TOP)/mk/xamarin.mk

.PHONY: check-versions print-versions reset reset-versions
check-versions print-versions reset reset-versions:
	@echo "No submodule versions to check or reset."
