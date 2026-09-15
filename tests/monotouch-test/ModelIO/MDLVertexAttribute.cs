//
// MDLVertexAttribute Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

#if !MONOMAC
using UIKit;
#endif
#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;

using System.Numerics;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLVertexAttributeTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
		}

		[Test]
		public void Ctors ()
		{
			using (var obj = new MDLVertexAttribute ("name", MDLVertexFormat.Float3, 1, 2)) {
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Format, Is.EqualTo (MDLVertexFormat.Float3), "Format");
				Assert.That (obj.Offset, Is.EqualTo ((nuint) 1), "Offset");
				Assert.That (obj.BufferIndex, Is.EqualTo ((nuint) 2), "BufferIndex");
				Asserts.AreEqual (new Vector4 (0, 0, 0, 1), obj.InitializationValue, "InitializationValue");
			}
		}

		[Test]
		public void Properties ()
		{
			var V4 = new Vector4 (1, 2, 3, 4);

			using (var obj = new MDLVertexAttribute ("name", MDLVertexFormat.Float3, 1, 2)) {
				obj.Name = "new name";
				Assert.That (obj.Name, Is.EqualTo ("new name"), "Name");

				obj.Format = MDLVertexFormat.Float2;
				Assert.That (obj.Format, Is.EqualTo (MDLVertexFormat.Float2), "Format");

				obj.Offset = 4;
				Assert.That (obj.Offset, Is.EqualTo ((nuint) 4), "Offset");

				obj.BufferIndex = 9;
				Assert.That (obj.BufferIndex, Is.EqualTo ((nuint) 9), "BufferIndex");
			}

			using (var obj = new MDLVertexAttribute ("name", MDLVertexFormat.Float3, 1, 2)) {
				obj.InitializationValue = V4;
				Asserts.AreEqual (V4, obj.InitializationValue, "InitializationValue");
			}
		}
	}
}
