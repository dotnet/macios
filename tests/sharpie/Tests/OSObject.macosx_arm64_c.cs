using Foundation;

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants {
	// extern _Bool OS_OBJECT_HAVE_OBJC_SUPPORT__False;
	[Field ("OS_OBJECT_HAVE_OBJC_SUPPORT__False", "__Internal")]
	bool OS_OBJECT_HAVE_OBJC_SUPPORT__False { get; }

	// extern _Bool OS_OBJECT_USE_OBJC__False;
	[Field ("OS_OBJECT_USE_OBJC__False", "__Internal")]
	bool OS_OBJECT_USE_OBJC__False { get; }
}
