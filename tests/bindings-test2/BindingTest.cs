using System.Linq;

using Bindings.Test2;

namespace Xamarin.BindingTests2 {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class BindingTest {
		[Test]
		public void Test ()
		{
			Assert.That (CFunctions.getIntOfChocolate (), Is.EqualTo (42), "chocolate");
			Assert.That (Bindings.Test.CFunctions.theUltimateAnswer (), Is.EqualTo (42), "theUltimateAnswer");
		}
	}
}
