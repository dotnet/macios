
using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioBufferListTest {
		[Test]
		public unsafe void Spans ()
		{
			var memory = System.Runtime.InteropServices.Marshal.AllocHGlobal (IntPtr.Size + 3 * sizeof (AudioBuffer));
			try {
				var list = (AudioBufferList*) memory;
				*(uint*) memory = 0;
				Assert.That (list->AsSpan ().Length, Is.EqualTo (0), "Empty writable span");
				Assert.That (list->AsReadOnlySpan ().Length, Is.EqualTo (0), "Empty read-only span");

				*(uint*) memory = 3;
				var writable = list->AsSpan ();
				var readOnly = list->AsReadOnlySpan ();
				Assert.That (writable.Length, Is.EqualTo (3), "Writable length");
				Assert.That (readOnly.Length, Is.EqualTo (3), "Read-only length");
				for (var i = 0; i < writable.Length; i++) {
					writable [i] = new AudioBuffer {
						NumberChannels = i + 1,
						DataByteSize = (i + 1) * 100,
						Data = (IntPtr) ((i + 1) * 1000),
					};
					Assert.That (readOnly [i].NumberChannels, Is.EqualTo (i + 1), $"Channels #{i}");
					Assert.That (readOnly [i].DataByteSize, Is.EqualTo ((i + 1) * 100), $"Size #{i}");
					Assert.That (readOnly [i].Data, Is.EqualTo ((IntPtr) ((i + 1) * 1000)), $"Data #{i}");
					fixed (AudioBuffer* element = &writable [i])
						Assert.That ((IntPtr) element, Is.EqualTo ((IntPtr) list->GetBuffer (i)), $"Address #{i}");
				}
				list->GetBuffer (1)->NumberChannels = 42;
				Assert.That (readOnly [1].NumberChannels, Is.EqualTo (42), "Native mutation");
				Assert.Throws<IndexOutOfRangeException> (() => { _ = list->AsSpan () [-1]; }, "Negative writable index");
				Assert.Throws<IndexOutOfRangeException> (() => { _ = list->AsSpan () [3]; }, "Writable upper bound");
				Assert.Throws<IndexOutOfRangeException> (() => { _ = list->AsReadOnlySpan () [-1]; }, "Negative read-only index");
				Assert.Throws<IndexOutOfRangeException> (() => { _ = list->AsReadOnlySpan () [3]; }, "Read-only upper bound");

				*(uint*) memory = uint.MaxValue;
				Assert.Throws<OverflowException> (() => { _ = list->AsSpan (); }, "Writable count overflow");
				Assert.Throws<OverflowException> (() => { _ = list->AsReadOnlySpan (); }, "Read-only count overflow");
			} finally {
				System.Runtime.InteropServices.Marshal.FreeHGlobal (memory);
			}
		}

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
