
namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DisposableObjectTest {
		class Subclassed : DisposableObject {
			public Subclassed () : base () { }
			public Subclassed (NativeHandle handle, bool owns) : base (handle, owns) { }
			public Subclassed (NativeHandle handle, bool owns, bool verify) : base (handle, owns, verify) { }

			public new NativeHandle Handle {
				get => base.Handle;
				set => base.Handle = value;
			}

			public new bool Owns { get => base.Owns; }
		}

		[Test]
		public void DefaultCtor ()
		{
			var obj = new Subclassed ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle");
			ClassicAssert.AreEqual (false, obj.Owns, "Owns");
		}

		[Test]
		public void CtorOwns ()
		{
			Subclassed obj;

			var ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, true), "Handle 1");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 1");

			ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, false), "Handle 2");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 2");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, true);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 3");
			ClassicAssert.AreEqual (true, obj.Owns, "Owns 3");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 3");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 3b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 4");
			ClassicAssert.AreEqual (false, obj.Owns, "Owns 4");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 4");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 4b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 4b");
		}

		[Test]
		public void CtorOwnsVerify ()
		{
			var obj = new Subclassed (NativeHandle.Zero, true, false);
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 1");
			ClassicAssert.AreEqual (true, obj.Owns, "Owns 1");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 1");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 1b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 1b");

			obj = new Subclassed (NativeHandle.Zero, false, false);
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 2");
			ClassicAssert.AreEqual (false, obj.Owns, "Owns 2");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 2");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 2b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 2b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, true, false);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 3");
			ClassicAssert.AreEqual (true, obj.Owns, "Owns 3");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 3");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 3b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false, false);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 4");
			ClassicAssert.AreEqual (false, obj.Owns, "Owns 4");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 4");


			var ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, true, true), "Handle 1V");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 1V");

			ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, false, true), "Handle 2V");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 2V");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, true, true);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 3V");
			ClassicAssert.AreEqual (true, obj.Owns, "Owns 3V");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 3V");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 3Vb");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3Vb");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false, true);
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "Handle 4V");
			ClassicAssert.AreEqual (false, obj.Owns, "Owns 4V");
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.GetCheckedHandle (), "GetCheckedHandle 4V");
			obj.Dispose ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle 4Vb");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 4Vb");
		}

		[Test]
		public void Handle ()
		{
			var obj = new Subclassed ();
			ClassicAssert.AreEqual (NativeHandle.Zero, obj.Handle, "Handle");
			var ex = Assert.Throws<Exception> (() => obj.Handle = NativeHandle.Zero, "SetHandle ex");
			obj.Handle = (NativeHandle) (IntPtr) 1;
			ClassicAssert.AreEqual ((NativeHandle) (IntPtr) 1, obj.Handle, "GetHandle");
		}
	}
}
