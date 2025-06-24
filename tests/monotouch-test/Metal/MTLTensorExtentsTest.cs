using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[Preserve (AllMembers = true)]
	public class MTLTensorExtentsTest {
		[Test]
		public void Constructor ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);
			TestRuntime.AssertDevice (); // only works on device

			using (var obj = new MTLTensorExtents (null)) {
				Assert.That (obj.Rank, Is.EqualTo ((nuint) 0), "#0");
			}

			using (var obj = new MTLTensorExtents (1)) {
				Assert.That (obj.Rank, Is.EqualTo ((nuint) 1), "#1");
				Assert.That (obj.GetExtent (0), Is.EqualTo ((nint) 1), "#1b");
			}

			using (var obj = new MTLTensorExtents (1, 2)) {
				Assert.That (obj.Rank, Is.EqualTo ((nuint) 2), "#2");
				Assert.That (obj.GetExtent (0), Is.EqualTo ((nint) 1), "#2ba");
				Assert.That (obj.GetExtent (1), Is.EqualTo ((nint) 2), "#2bb");
			}

			using (var obj = new MTLTensorExtents (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)) {
				Assert.That (obj.Rank, Is.EqualTo ((nuint) 16), "#3");
				for (var i = 0; i < 16; i++)
					Assert.That (obj.GetExtent ((nuint) i), Is.EqualTo ((nint) (i + 1)), $"#3-{i}");
			}

			var ex = Assert.Throws<Exception> (() => {
				var obj = new MTLTensorExtents (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
				GC.KeepAlive (obj);
			}, "#4");
			Assert.That (ex.Message, Does.StartWith ("Could not initialize an instance of the type 'Metal.MTLTensorExtents': the native 'initWithRank:values:' method returned nil."), "#4 Message");
		}
	}
}
