using System;

using Foundation;
using ObjCRuntime;

namespace issue10015Tests {

	[BaseType (typeof (NSObject))]
	interface Widget {

		[Export ("myAction")]
		Action<Action<bool>> MyAction ();
	}
}
