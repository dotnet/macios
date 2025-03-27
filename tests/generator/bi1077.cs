using Foundation;

namespace BI1077 {

	delegate void ADelegate (int a, int b, int c);
	delegate void BDelegate (int a, int b, int c);

	[BaseType (typeof (NSObject))]
	partial interface BI1059 {
		[Export ("a:")]
		[Async]
		void A (ADelegate callback);

		[Export ("B:")]
		[Async]
		void B (BDelegate callback);
	}

}
