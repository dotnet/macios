
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
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle");
			Assert.That (obj.Owns, Is.EqualTo (false), "Owns");
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
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 3");
			Assert.That (obj.Owns, Is.EqualTo (true), "Owns 3");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 3");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 3b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false);
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 4");
			Assert.That (obj.Owns, Is.EqualTo (false), "Owns 4");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 4");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 4b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 4b");
		}

		[Test]
		public void CtorOwnsVerify ()
		{
			var obj = new Subclassed (NativeHandle.Zero, true, false);
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 1");
			Assert.That (obj.Owns, Is.EqualTo (true), "Owns 1");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 1");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 1b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 1b");

			obj = new Subclassed (NativeHandle.Zero, false, false);
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 2");
			Assert.That (obj.Owns, Is.EqualTo (false), "Owns 2");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 2");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 2b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 2b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, true, false);
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 3");
			Assert.That (obj.Owns, Is.EqualTo (true), "Owns 3");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 3");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 3b");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3b");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false, false);
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 4");
			Assert.That (obj.Owns, Is.EqualTo (false), "Owns 4");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 4");


			var ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, true, true), "Handle 1V");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 1V");

			ex = Assert.Throws<Exception> (() => obj = new Subclassed (NativeHandle.Zero, false, true), "Handle 2V");
			Assert.That (ex.Message, Does.Contain ("Could not initialize an instance of the type"), "Ex 2V");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, true, true);
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 3V");
			Assert.That (obj.Owns, Is.EqualTo (true), "Owns 3V");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 3V");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 3Vb");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 3Vb");

			obj = new Subclassed ((NativeHandle) (IntPtr) 1, false, true);
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "Handle 4V");
			Assert.That (obj.Owns, Is.EqualTo (false), "Owns 4V");
			Assert.That (obj.GetCheckedHandle (), Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetCheckedHandle 4V");
			obj.Dispose ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle 4Vb");
			Assert.Throws<ObjectDisposedException> (() => obj.GetCheckedHandle (), "GetCheckedHandle 4Vb");
		}

		[Test]
		public void Handle ()
		{
			var obj = new Subclassed ();
			Assert.That (obj.Handle, Is.EqualTo (NativeHandle.Zero), "Handle");
			var ex = Assert.Throws<Exception> (() => obj.Handle = NativeHandle.Zero, "SetHandle ex");
			obj.Handle = (NativeHandle) (IntPtr) 1;
			Assert.That (obj.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 1), "GetHandle");
		}
	}
}
