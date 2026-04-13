using Foundation;
using ObjCRuntime;

namespace NS {
	[Abstract]
	[BaseType (typeof (NSObject))]
	public interface MyObject<T> where T : NSObject {

		[Export ("abstractMember:")]
		void AbstractMember (T parameter);
	}
}

