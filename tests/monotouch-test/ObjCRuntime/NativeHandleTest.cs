
namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeHandleTest {
		[Test]
		public unsafe void Operators ()
		{
			IntPtr value = new IntPtr (0xdadf00d);

			Assert.That (((NativeHandle) value).Handle, Is.EqualTo (value), "IntPtr -> NativeHandle");
			Assert.That ((IntPtr) new NativeHandle (value), Is.EqualTo (value), "NativeHandle -> IntPtr");
			Assert.That (((NativeHandle) ((void*) value)).Handle, Is.EqualTo (value), "void* -> NativeHandle");
			Assert.That ((IntPtr) (void*) new NativeHandle (value), Is.EqualTo (value), "NativeHandle -> void*");
		}
	}
}
