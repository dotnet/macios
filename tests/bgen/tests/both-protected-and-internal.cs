using Foundation;

namespace iosbinding {

	[BaseType (typeof (NSObject))]
	interface SomeType {
		[Export ("method")]
		[Internal]
		[Protected]
		void Method ();
	}
}
