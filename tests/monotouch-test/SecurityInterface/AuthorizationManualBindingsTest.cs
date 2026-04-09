#if __MACOS__
using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using ObjCRuntime;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AuthorizationEngineTest {

		[Test]
		public void Create_Zero_ReturnsNull ()
		{
			var engine = AuthorizationEngine.Create (NativeHandle.Zero);
			Assert.That (engine, Is.Null, "Zero handle should return null");
		}

		[Test]
		public void Create_NonZero_ReturnsWrapper ()
		{
			// Use a fake non-zero handle to verify the wrapper creation path
			var fakeHandle = new NativeHandle ((IntPtr) 0x12345678);
			var engine = AuthorizationEngine.Create (fakeHandle);
			Assert.That (engine, Is.Not.Null, "Non-zero handle should create a wrapper");
			Assert.That (engine!.Handle, Is.EqualTo (fakeHandle), "Handle should match");
			// Don't dispose — this is a fake handle, not a real CF object
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public unsafe class AuthorizationCallbacksTest {

		[StructLayout (LayoutKind.Sequential)]
		struct FakeCallbacksNative {
			public uint Version;
			// All remaining fields are function pointers — set to zero for testing
			public IntPtr SetResult;
			public IntPtr RequestInterrupt;
			public IntPtr DidDeactivate;
			public IntPtr GetContextValue;
			public IntPtr SetContextValue;
			public IntPtr GetHintValue;
			public IntPtr SetHintValue;
			public IntPtr GetArguments;
			public IntPtr GetSessionId;
			public IntPtr GetImmutableHintValue;
			public IntPtr GetLAContext;
			public IntPtr GetTokenIdentities;
			public IntPtr GetTKTokenWatcher;
			public IntPtr RemoveHintValue;
			public IntPtr RemoveContextValue;
		}

		[Test]
		public void Create_Zero_StoresHandle ()
		{
			var callbacks = new AuthorizationCallbacks (NativeHandle.Zero);
			Assert.That ((IntPtr) callbacks.Handle, Is.EqualTo (IntPtr.Zero), "Zero handle should be stored");
		}

		[Test]
		public void Create_NonZero_ReturnsWrapper ()
		{
			var native = new FakeCallbacksNative { Version = 7 };
			var ptr = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, ptr, false);
				var callbacks = new AuthorizationCallbacks (ptr);
				Assert.That (callbacks, Is.Not.Null, "Should create a wrapper");
				Assert.That (callbacks.Handle, Is.EqualTo ((NativeHandle) ptr), "Handle should match");
			} finally {
				Marshal.FreeHGlobal (ptr);
			}
		}

		[Test]
		public void Version_ReadsCorrectly ()
		{
			var native = new FakeCallbacksNative { Version = 42 };
			var ptr = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, ptr, false);
				var callbacks = new AuthorizationCallbacks (ptr);
				Assert.That (callbacks.Version, Is.EqualTo (42u), "Version should read 42");
			} finally {
				Marshal.FreeHGlobal (ptr);
			}
		}

		[Test]
		public void Version_DifferentValues ()
		{
			var native = new FakeCallbacksNative { Version = 1 };
			var ptr = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, ptr, false);
				var callbacks = new AuthorizationCallbacks (ptr);
				Assert.That (callbacks.Version, Is.EqualTo (1u), "Version should be 1");

				// Update the native memory and verify the wrapper reads the new value
				native.Version = 99;
				Marshal.StructureToPtr (native, ptr, false);
				Assert.That (callbacks.Version, Is.EqualTo (99u), "Version should update to 99");
			} finally {
				Marshal.FreeHGlobal (ptr);
			}
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AuthorizationRightsRoundTripTest {

		[Test]
		public void FromHandle_NullReturnsNull ()
		{
			var rights = AuthorizationRights.FromHandle (NativeHandle.Zero);
			Assert.That (rights, Is.Null, "Zero handle should return null");
		}

		[Test]
		public void NativeHandle_RoundTrip ()
		{
			// Create rights, get their handle, then read them back via FromHandle
			using var original = new AuthorizationRights (
				new AuthorizationRight ("com.example.right1", new byte [] { 0xAA, 0xBB }, 5),
				new AuthorizationRight ("com.example.right2")
			);

			var handle = original.Handle;
			Assert.That (handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle should be valid");

			// Read back from the same handle (simulates what the bgen getter does)
			using var readBack = AuthorizationRights.FromHandle (handle);
			Assert.That (readBack, Is.Not.Null, "FromHandle should return non-null");
			Assert.That (readBack!.Count, Is.EqualTo (2), "Count should match");

			Assert.That (readBack [0].Name, Is.EqualTo ("com.example.right1"), "Name[0]");
			Assert.That (readBack [0].Value, Is.EqualTo (new byte [] { 0xAA, 0xBB }), "Value[0]");
			Assert.That (readBack [0].Flags, Is.EqualTo (5u), "Flags[0]");

			Assert.That (readBack [1].Name, Is.EqualTo ("com.example.right2"), "Name[1]");
			Assert.That (readBack [1].Value, Is.Null, "Value[1] should be null");
			Assert.That (readBack [1].Flags, Is.EqualTo (0u), "Flags[1]");
		}

		[Test]
		public void LargeRightsSet ()
		{
			var items = new AuthorizationRight [100];
			for (int i = 0; i < 100; i++)
				items [i] = new AuthorizationRight ($"com.example.right{i}", new byte [] { (byte) i }, (uint) i);

			using var rights = new AuthorizationRights (items);
			Assert.That (rights.Count, Is.EqualTo (100), "Count");

			using var readBack = AuthorizationRights.FromHandle (rights.Handle);
			Assert.That (readBack!.Count, Is.EqualTo (100), "ReadBack Count");
			Assert.That (readBack [50].Name, Is.EqualTo ("com.example.right50"), "Name[50]");
			Assert.That (readBack [50].Value, Is.EqualTo (new byte [] { 50 }), "Value[50]");
			Assert.That (readBack [50].Flags, Is.EqualTo (50u), "Flags[50]");
		}

		[Test]
		public void UnicodeRightNames ()
		{
			using var rights = new AuthorizationRights (
				new AuthorizationRight ("com.example.日本語テスト"),
				new AuthorizationRight ("com.example.émojis🎉")
			);

			using var readBack = AuthorizationRights.FromHandle (rights.Handle);
			Assert.That (readBack! [0].Name, Is.EqualTo ("com.example.日本語テスト"), "Unicode name[0]");
			Assert.That (readBack [1].Name, Is.EqualTo ("com.example.émojis🎉"), "Unicode name[1]");
		}

		[Test]
		public void EmptyValueVsNullValue ()
		{
			using var rights = new AuthorizationRights (
				new AuthorizationRight ("with-empty", new byte [0]),
				new AuthorizationRight ("with-null", null)
			);

			using var readBack = AuthorizationRights.FromHandle (rights.Handle);
			Assert.That (readBack! [0].Value, Is.Null, "Empty byte array should read back as null (zero length)");
			Assert.That (readBack [1].Value, Is.Null, "Null value should remain null");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SFAuthorizationPluginViewManualTest {

		[Test]
		public void SFAuthorizationPluginView_CallbacksProperty_Null ()
		{
			// We can't construct a real SFAuthorizationPluginView without a valid engine+callbacks
			// from the authorization plugin host. But we can verify the type exists and is constructible
			// via the ObjC runtime by checking the class handle.
			var classHandle = global::ObjCRuntime.Class.GetHandle ("SFAuthorizationPluginView");
			Assert.That (classHandle, Is.Not.EqualTo (IntPtr.Zero), "SFAuthorizationPluginView ObjC class should exist");
		}
	}
}
#endif // __MACOS__
