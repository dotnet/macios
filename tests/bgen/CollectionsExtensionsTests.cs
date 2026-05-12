using System.Linq;

namespace GeneratorTests {

	[TestFixture]
	public class CollectionsExtensionsTests {

		[Test]
		public void Yield ()
			=> Assert.That ("test".Yield ().Count (), Is.EqualTo (1));

		[Test]
		public void DropLast ()
		{
			var array = new [] { "first", "second", "last" };
			var result = array.DropLast ();
			Assert.That (result.Length, Is.EqualTo (array.Length - 1), "Result Length");
			Assert.That (result.Contains (array.Last ()), Is.False, "Contains last item");
		}

	}
}
