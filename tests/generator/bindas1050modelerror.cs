using System;
using System.Drawing;
using Foundation;
using CoreAnimation;

namespace BindAs1050ModelErrorTests {

	[Model]
	[BaseType (typeof (NSObject))]
	interface MyFooClass {

		[return: BindAs (typeof (CAScroll? []))]
		[Export ("getScrollArrayEnum2:")]
		NSNumber [] GetScrollArrayFooEnumArray2 ([BindAs (typeof (CAScroll []))] NSNumber [] arg1);
		
	}
}