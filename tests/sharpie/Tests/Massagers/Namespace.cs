using Foundation;
using ObjCRuntime;

namespace CatOverflow {
	// @interface SuperCats
	interface SuperCats {
		// -(void)meow;
		[Export ("meow")]
		void Meow ();
	}

	[Native]
	public enum CatBehavior : long {
		CatBehaviorEvil
	}
}
