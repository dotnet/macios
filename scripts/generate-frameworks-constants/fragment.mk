include $(TOP)/scripts/template.mk
$(eval $(call TemplateScript,GENERATE_FRAMEWORKS_CONSTANTS,generate-frameworks-constants,$(TOP)/tools/common/Frameworks.cs $(TOP)/tools/common/ApplePlatform.cs $(TOP)/tools/common/SdkVersions.cs $(TOP)/tools/common/StringUtils.cs $(TOP)/tools/common/NullableAttributes.cs))
