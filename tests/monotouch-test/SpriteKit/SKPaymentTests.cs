#if __MACOS__
using System.Threading.Tasks;

using AppKit;
using StoreKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SKPaymentTests {
		[Test]
		public void SKPayment_PaymentWithProduct ()
		{
			SKProduct product = new SKProduct ();
			SKPayment payment = SKPayment.CreateFrom (product);
			Assert.IsNotNull (payment);
		}
	}
}

#endif // __MACOS__
