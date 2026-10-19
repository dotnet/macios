
namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NativeHandleTest {
		class NativeObject : INativeObject {
			public NativeHandle Handle { get; set; }
		}

		[Test]
		public void GetNonNullHandle ()
		{
			INativeObject value = null;
			var exception = Assert.Throws<ArgumentNullException> (() => value.GetNonNullHandle ("argument"));
			Assert.That (exception.ParamName, Is.EqualTo ("argument"), "Null argument name");

			var nativeObject = new NativeObject ();
			Assert.Throws<ObjectDisposedException> (() => nativeObject.GetNonNullHandle ("argument"), "Zero handle");

			nativeObject.Handle = (IntPtr) 42;
			Assert.That (nativeObject.GetNonNullHandle ("argument"), Is.EqualTo (nativeObject.Handle), "Valid handle");
		}

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
