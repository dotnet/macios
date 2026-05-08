
namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeHandleTest {
		[Test]
		public unsafe void Operators ()
		{
			IntPtr value = new IntPtr (0xdadf00d);

			ClassicAssert.AreEqual (value, ((NativeHandle) value).Handle, "IntPtr -> NativeHandle");
			ClassicAssert.AreEqual (value, (IntPtr) new NativeHandle (value), "NativeHandle -> IntPtr");
			ClassicAssert.AreEqual (value, ((NativeHandle) ((void*) value)).Handle, "void* -> NativeHandle");
			ClassicAssert.AreEqual (value, (IntPtr) (void*) new NativeHandle (value), "NativeHandle -> void*");
		}
	}
}
