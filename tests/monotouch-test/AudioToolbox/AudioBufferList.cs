
using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioBufferListTest {
		[Test]
		public unsafe void Usage ()
		{
			var buffer = new byte [1024];
			fixed (byte* ptr = buffer) {
				var list = (AudioBufferList*) ptr;
				Assert.That (list->Count, Is.EqualTo (0), "Count");
				Assert.Throws<ArgumentOutOfRangeException> (() => list->GetBuffer (0), "Item 0");
				Assert.Throws<ArgumentOutOfRangeException> (() => list->GetBuffer (-1), "Item -1");
				Assert.Throws<ArgumentOutOfRangeException> (() => list->GetBuffer (1), "Item 1");

				*(int*) ptr = 3;
				Assert.That (list->Count, Is.EqualTo (3), "Count B");
				for (var i = 0; i < 3; i++) {
					Assert.That (list->GetBuffer (i)->NumberChannels, Is.EqualTo (0), $"NumberChannels B#{i}");
					Assert.That (list->GetBuffer (i)->DataByteSize, Is.EqualTo (0), $"DataByteSize B#{i}");
					Assert.That (list->GetBuffer (i)->Data, Is.EqualTo ((nint) 0), $"Data B#{i}");

					list->GetBuffer (i)->NumberChannels = (i + 1) * 10;
					list->GetBuffer (i)->DataByteSize = (i + 1) * 100;
					list->GetBuffer (i)->Data = new IntPtr ((i + 1) * 1000);
				}
				Assert.Throws<ArgumentOutOfRangeException> (() => list->GetBuffer (-1), "Item -1 B");
				Assert.Throws<ArgumentOutOfRangeException> (() => list->GetBuffer (3), "Item 3 B");

				int* iptr = (int*) ptr;
				Assert.That (iptr [2 + 0 * 4], Is.EqualTo (10), "10"); // NumberChannels
				Assert.That (iptr [2 + 0 * 4 + 1], Is.EqualTo (100), "20"); // DataByteSize
				Assert.That (iptr [2 + 1 * 4], Is.EqualTo (20), "20"); // NumberChannels
				Assert.That (iptr [2 + 1 * 4 + 1], Is.EqualTo (200), "40"); // DataByteSize
				Assert.That (iptr [2 + 2 * 4], Is.EqualTo (30), "30"); // NumberChannels
				Assert.That (iptr [2 + 2 * 4 + 1], Is.EqualTo (300), "60"); // DataByteSize

				nint* nptr = (nint*) ptr;
				Assert.That (nptr [1 + 0 * 2 + 1], Is.EqualTo ((nint) 1000), "1000"); // Data
				Assert.That (nptr [1 + 1 * 2 + 1], Is.EqualTo ((nint) 2000), "2000"); // Data
				Assert.That (nptr [1 + 2 * 2 + 1], Is.EqualTo ((nint) 3000), "3000"); // Data
			}
		}
	}
}
