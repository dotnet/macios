#if !__TVOS__

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Photos;
using NUnit.Framework;

namespace MonoTouchFixtures.Photos {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public unsafe class PHLivePhotoEditingContextTest {

		[SetUp]
		public void Setup ()
		{
			if (!TestRuntime.CheckXcodeVersion (8, 0))
				Assert.Inconclusive ("Requires Xcode 8+ or later");
		}

		static NSError error_faker;

		static PHLivePhotoFrameProcessingBlock managed = (IPHLivePhotoFrame frame, ref NSError error) => {
			error = error_faker;
			return null;
		};

		delegate NativeHandle DPHLivePhotoFrameProcessingBlock2 (IntPtr block, NativeHandle frame, NativeHandle* error);

#if !MONOMAC
		// on macOS `initWithLivePhotoEditingInput:` returns `nil` and we throw
		[Test]
#endif
		public void Linker ()
		{
			using (var cei = new PHContentEditingInput ())
			using (var lpec = new PHLivePhotoEditingContext (cei)) {
				// not much but it means the linker cannot remove it
				Assert.Null (lpec.FrameProcessor, "FrameProcessor");
			}
		}

		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test pokes at internals, so it's expected to not be trimmer safe. It works though, so unless something changes, we're going to assume it's trimmer-compatible.")]
		[UnconditionalSuppressMessage ("Trimming", "IL2075", Justification = "This test pokes at internals, so it's expected to not be trimmer safe. It works though, so unless something changes, we're going to assume it's trimmer-compatible.")]
		[Test]
		public unsafe void FrameProcessingBlock2 ()
		{
			if (!Runtime.DynamicRegistrationSupported)
				Assert.Ignore ("This test requires support for the dynamic registrar to setup the block");

			var t = typeof (NSObject).Assembly.GetType ("ObjCRuntime.Trampolines+SDPHLivePhotoFrameProcessingBlock");
			Assert.NotNull (t, "SDPHLivePhotoFrameProcessingBlock2");

			var m = t.GetMethod ("Invoke", BindingFlags.Static | BindingFlags.NonPublic);
			Assert.NotNull (m, "Invoke");
			var d = m.CreateDelegate (typeof (DPHLivePhotoFrameProcessingBlock2));
			var fptr = m.MethodHandle.GetFunctionPointer ();
			var del = new DPHLivePhotoFrameProcessingBlock2 ((IntPtr a, NativeHandle b, NativeHandle* c) => (NativeHandle) global::Bindings.Test.CFunctions.x_call_func_3 (fptr, (IntPtr) a, (IntPtr) b, (IntPtr) (void*) c));

			using var bl = new BlockLiteral ((void*) fptr, managed, t, "Invoke");
			var block = &bl;
			var b = (IntPtr) block;

			// simulate a call that does not produce an error
			error_faker = null;
			Assert.That (del (b, NativeHandle.Zero, null), Is.EqualTo (NativeHandle.Zero), "1");

			// simulate a call that does produce an error
			error_faker = new NSError ((NSString) "domain", 42);
			NativeHandle ptr = NativeHandle.Zero;
			Assert.That (del (b, NativeHandle.Zero, &ptr), Is.EqualTo (NativeHandle.Zero), "2");
			Assert.That ((IntPtr) ptr, Is.EqualTo ((IntPtr) error_faker.Handle), "error 2");
		}
	}
}

#endif
