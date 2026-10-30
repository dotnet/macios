// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Surface = IOSurface.IOSurface;
using IOSurface;

namespace MonoTouchFixtures.IOSurface {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class IOSurfaceTest {
		[DllImport (Constants.IOSurfaceLibrary)]
		static extern uint IOSurfaceGetID (IntPtr surface);

		[DllImport (Constants.libcLibrary)]
		static extern uint mach_task_self ();

		[DllImport (Constants.libcLibrary)]
		static extern int mach_port_deallocate (uint task, uint port);

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
		}

		static Surface CreateSurface ()
		{
			return new Surface (new IOSurfaceOptions {
				Width = 16,
				Height = 16,
				BytesPerElement = 4,
				BytesPerRow = 64,
			});
		}

		static void AssertSurface (Surface original, Surface? surface)
		{
			Assert.That (surface, Is.Not.Null, "Surface");
			if (surface is null)
				return;
			Assert.That (IOSurfaceGetID (surface.Handle), Is.EqualTo (IOSurfaceGetID (original.Handle)), "Id");
			Assert.That (surface.Width, Is.EqualTo (original.Width), "Width");
			Assert.That (surface.Height, Is.EqualTo (original.Height), "Height");
			Assert.That (original.Lock (0), Is.Zero, "Lock original");
			try {
				Marshal.WriteInt32 (original.BaseAddress, 0x12345678);
			} finally {
				Assert.That (original.Unlock (0), Is.Zero, "Unlock original");
			}
			Assert.That (surface.Lock (IOSurfaceLockOptions.ReadOnly), Is.Zero, "Lock surface");
			try {
				Assert.That (Marshal.ReadInt32 (surface.BaseAddress), Is.EqualTo (0x12345678), "Shared contents");
			} finally {
				Assert.That (surface.Unlock (IOSurfaceLockOptions.ReadOnly), Is.Zero, "Unlock surface");
			}
		}

		[Test]
		public void MachPort ()
		{
			using var original = CreateSurface ();
			var port = original.CreateMachPort ();
			Assert.That (port, Is.Not.Zero, "Port");
			try {
				using var surface = Surface.LookupFromMachPort (port);
				AssertSurface (original, surface);
				using var second = Surface.LookupFromMachPort (port);
				AssertSurface (original, second);
			} finally {
				Assert.That (mach_port_deallocate (mach_task_self (), port), Is.Zero, "Deallocate port");
			}
		}

		[Test]
		public void Lookup ()
		{
			using var original = CreateSurface ();
			using var surface = Surface.Lookup (IOSurfaceGetID (original.Handle));
			AssertSurface (original, surface);
		}

		[Test]
		public void InvalidLookup ()
		{
			Assert.That (Surface.Lookup (0), Is.Null, "Invalid id");
			Assert.That (Surface.LookupFromMachPort (0), Is.Null, "Invalid port");
		}

		[Test]
		public void Disposed ()
		{
			var surface = CreateSurface ();
			surface.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => surface.CreateMachPort (), "Mach port");
#if MONOMAC || __MACCATALYST__
			Assert.Throws<ObjectDisposedException> (() => surface.CreateXpcObject (), "XPC object");
#endif
		}

#if MONOMAC || __MACCATALYST__
		[Test]
		public void XpcObject ()
		{
			using var original = CreateSurface ();
			using var xpcObject = original.CreateXpcObject ();
			Assert.That (xpcObject, Is.Not.Null, "XPC object");
			if (xpcObject is null)
				return;
			using var surface = Surface.LookupFromXpcObject (xpcObject);
			AssertSurface (original, surface);
			using var second = Surface.LookupFromXpcObject (xpcObject);
			AssertSurface (original, second);
		}

		[Test]
		public void NullXpcObject ()
		{
			Assert.Throws<ArgumentNullException> (() => Surface.LookupFromXpcObject (null));
		}

		[Test]
		public void DisposedXpcObject ()
		{
			using var original = CreateSurface ();
			var xpcObject = original.CreateXpcObject ();
			Assert.That (xpcObject, Is.Not.Null, "XPC object");
			if (xpcObject is null)
				return;
			xpcObject.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => Surface.LookupFromXpcObject (xpcObject));
		}
#endif
	}
}
