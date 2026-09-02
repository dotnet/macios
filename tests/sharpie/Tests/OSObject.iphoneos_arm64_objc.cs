using Foundation;

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern _Bool OS_OBJECT_HAVE_OBJC_SUPPORT__True;
	[Field ("OS_OBJECT_HAVE_OBJC_SUPPORT__True", "__Internal")]
	bool OS_OBJECT_HAVE_OBJC_SUPPORT__True { get; }

	// extern _Bool OS_OBJECT_USE_OBJC__True;
	[Field ("OS_OBJECT_USE_OBJC__True", "__Internal")]
	bool OS_OBJECT_USE_OBJC__True { get; }
}
