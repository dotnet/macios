#if HAS_COREMEDIAIO
#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOObjectPropertyAddressTest {

		[Test]
		public void DefaultConstructor ()
		{
			var address = new CMIOObjectPropertyAddress ();
			Assert.AreEqual ((uint) 0, address.Selector, "Selector");
			Assert.AreEqual ((uint) 0, address.Scope, "Scope");
			Assert.AreEqual ((uint) 0, address.Element, "Element");
		}

		[Test]
		public void Constructor_WithValues ()
		{
			var address = new CMIOObjectPropertyAddress (0x6F776E72, 0x676C6F62, 0);
			Assert.AreEqual ((uint) 0x6F776E72, address.Selector, "Selector");
			Assert.AreEqual ((uint) 0x676C6F62, address.Scope, "Scope");
			Assert.AreEqual ((uint) 0, address.Element, "Element");
		}

		[Test]
		public void Properties_RoundTrip ()
		{
			var address = new CMIOObjectPropertyAddress ();
			address.Selector = 42;
			address.Scope = 84;
			address.Element = 126;
			Assert.AreEqual ((uint) 42, address.Selector, "Selector");
			Assert.AreEqual ((uint) 84, address.Scope, "Scope");
			Assert.AreEqual ((uint) 126, address.Element, "Element");
		}

		[Test]
		public void StructSize ()
		{
			Assert.AreEqual (12, Marshal.SizeOf<CMIOObjectPropertyAddress> (), "Size should be 12 bytes (3 x uint)");
		}
	}
}
#endif // HAS_COREMEDIAIO
