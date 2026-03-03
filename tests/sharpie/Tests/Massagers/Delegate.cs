using Foundation;
using ObjCRuntime;

// @interface PrimaryDelegate
interface PrimaryDelegate {
}

// @interface SecondaryDelegate
interface SecondaryDelegate {
}

// @interface DelegateConsumer
interface DelegateConsumer {
	[Wrap ("WeakDelegate")]
	PrimaryDelegate Delegate { get; set; }

	// @property (assign) PrimaryDelegate * delegate;
	[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
	NSObject WeakDelegate { get; set; }

	[Wrap ("WeakSecondaryDelegate")]
	SecondaryDelegate SecondaryDelegate { get; set; }

	// @property (assign) SecondaryDelegate * secondaryDelegate;
	[NullAllowed, Export ("secondaryDelegate", ArgumentSemantic.Assign)]
	NSObject WeakSecondaryDelegate { get; set; }
}
