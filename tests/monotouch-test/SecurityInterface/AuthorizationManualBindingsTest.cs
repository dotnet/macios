#if __MACOS__
#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NUnit.Framework;
using LocalAuthentication;
using ObjCRuntime;
using Security;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AuthorizationEngineTest {

		[Test]
		public void Create_Zero_ReturnsNull ()
		{
			Assert.That (AuthorizationEngine.Create (NativeHandle.Zero), Is.Null, "Zero");
		}

		[Test]
		public void Create_NonZero_DoesNotRetain ()
		{
			using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
			Assert.That (engine, Is.Not.Null, "Engine");
			if (engine is null)
				return;
			Assert.That (engine.Handle, Is.EqualTo ((NativeHandle) (IntPtr) 0x1234), "Handle");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public unsafe class AuthorizationCallbacksTest {
		static IntPtr lastEngine;
		static AuthorizationResult lastResult;
		static IntPtr contextValue;

		[StructLayout (LayoutKind.Sequential)]
		struct FakeCallbacksNative {
			internal uint Version;
			internal IntPtr SetResult;
			internal IntPtr RequestInterrupt;
			internal IntPtr DidDeactivate;
			internal IntPtr GetContextValue;
			internal IntPtr SetContextValue;
			internal IntPtr GetHintValue;
			internal IntPtr SetHintValue;
			internal IntPtr GetArguments;
			internal IntPtr GetSessionId;
			internal IntPtr GetImmutableHintValue;
			internal IntPtr GetLAContext;
			internal IntPtr GetTokenIdentities;
			internal IntPtr GetTKTokenWatcher;
			internal IntPtr RemoveHintValue;
			internal IntPtr RemoveContextValue;
		}

		[StructLayout (LayoutKind.Sequential)]
		struct AuthorizationValueLayout {
			internal nuint Length;
			internal IntPtr Data;
		}

		[UnmanagedCallersOnly]
		static int SetResultStub (IntPtr engine, AuthorizationResult result)
		{
			lastEngine = engine;
			lastResult = result;
			return 17;
		}

		[UnmanagedCallersOnly]
		static int GetContextValueStub (IntPtr engine, IntPtr key, AuthorizationContextFlags* flags, IntPtr* value)
		{
			lastEngine = engine;
			*flags = AuthorizationContextFlags.Sticky;
			*value = contextValue;
			return 0;
		}

		[UnmanagedCallersOnly]
		static int EngineOnlyStub (IntPtr engine)
		{
			lastEngine = engine;
			return 21;
		}

		[UnmanagedCallersOnly]
		static int SetContextValueStub (IntPtr engine, IntPtr key, AuthorizationContextFlags flags, IntPtr value)
		{
			lastEngine = engine;
			return 22;
		}

		[UnmanagedCallersOnly]
		static int GetValueStub (IntPtr engine, IntPtr key, IntPtr* value)
		{
			lastEngine = engine;
			*value = IntPtr.Zero;
			return 23;
		}

		[UnmanagedCallersOnly]
		static int SetValueStub (IntPtr engine, IntPtr key, IntPtr value)
		{
			lastEngine = engine;
			return 24;
		}

		[UnmanagedCallersOnly]
		static int GetPointerStub (IntPtr engine, IntPtr* value)
		{
			lastEngine = engine;
			*value = IntPtr.Zero;
			return 25;
		}

		[UnmanagedCallersOnly]
		static int GetTokenIdentitiesStub (IntPtr engine, IntPtr context, IntPtr* value)
		{
			lastEngine = engine;
			*value = IntPtr.Zero;
			return 26;
		}

		[UnmanagedCallersOnly]
		static int RemoveValueStub (IntPtr engine, IntPtr key)
		{
			lastEngine = engine;
			return 27;
		}

		[Test]
		public void Create_ZeroReturnsNull ()
		{
			Assert.That (AuthorizationCallbacks.Create (NativeHandle.Zero), Is.Null, "Zero");
		}

		[Test]
		public void NativeLayout ()
		{
			Assert.That (Marshal.SizeOf<FakeCallbacksNative> (), Is.EqualTo (128), "Size");
			Assert.That (Marshal.OffsetOf<FakeCallbacksNative> (nameof (FakeCallbacksNative.Version)).ToInt32 (), Is.EqualTo (0), "Version");
			Assert.That (Marshal.OffsetOf<FakeCallbacksNative> (nameof (FakeCallbacksNative.SetResult)).ToInt32 (), Is.EqualTo (8), "SetResult");
			Assert.That (Marshal.OffsetOf<FakeCallbacksNative> (nameof (FakeCallbacksNative.RemoveContextValue)).ToInt32 (), Is.EqualTo (120), "RemoveContextValue");
		}

		[Test]
		public void Version ()
		{
			var native = new FakeCallbacksNative { Version = 4 };
			var pointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, pointer, false);
				var callbacks = AuthorizationCallbacks.Create (pointer);
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				if (callbacks is null)
					return;
				Assert.That (callbacks.Version, Is.EqualTo (4u), "Version");
			} finally {
				Marshal.FreeHGlobal (pointer);
			}
		}

		[Test]
		public void SetResult ()
		{
			var native = new FakeCallbacksNative {
				Version = 4,
				SetResult = (IntPtr) (delegate* unmanaged<IntPtr, AuthorizationResult, int>) &SetResultStub,
			};
			var pointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, pointer, false);
				var callbacks = AuthorizationCallbacks.Create (pointer);
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				if (callbacks is null)
					return;
				using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
				Assert.That (engine, Is.Not.Null, "Engine");
				if (engine is null)
					return;
				var status = callbacks.SetResult (engine, AuthorizationResult.Allow);
				Assert.That (status, Is.EqualTo (17), "Status");
				Assert.That (lastEngine, Is.EqualTo ((IntPtr) 0x1234), "Engine");
				Assert.That (lastResult, Is.EqualTo (AuthorizationResult.Allow), "Result");
			} finally {
				Marshal.FreeHGlobal (pointer);
			}
		}

		[Test]
		public void GetContextValue_CopiesValue ()
		{
			var data = Marshal.AllocHGlobal (3);
			var valuePointer = Marshal.AllocHGlobal (Marshal.SizeOf<AuthorizationValueLayout> ());
			contextValue = valuePointer;
			var callbacksPointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.Copy (new byte [] { 1, 2, 3 }, 0, data, 3);
				Marshal.StructureToPtr (new AuthorizationValueLayout { Length = 3, Data = data }, contextValue, false);
				Marshal.StructureToPtr (new FakeCallbacksNative {
					Version = 4,
					GetContextValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, int>) &GetContextValueStub,
				}, callbacksPointer, false);

				var callbacks = AuthorizationCallbacks.Create (callbacksPointer);
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				if (callbacks is null)
					return;
				using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x5678);
				Assert.That (engine, Is.Not.Null, "Engine");
				if (engine is null)
					return;
				var status = callbacks.GetContextValue (engine, "test-key", out var flags, out var value);
				Assert.That (status, Is.EqualTo (0), "Status");
				Assert.That (flags, Is.EqualTo (AuthorizationContextFlags.Sticky), "Flags");
				Assert.That (value, Is.EqualTo (new byte [] { 1, 2, 3 }), "Value");
			} finally {
				contextValue = IntPtr.Zero;
				Marshal.FreeHGlobal (valuePointer);
				Marshal.FreeHGlobal (callbacksPointer);
				Marshal.FreeHGlobal (data);
			}
		}

		[Test]
		public void OptionalCallback_VersionGuard ()
		{
			var native = new FakeCallbacksNative { Version = 3 };
			var pointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, pointer, false);
				var callbacks = AuthorizationCallbacks.Create (pointer);
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				if (callbacks is null)
					return;
				using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
				Assert.That (engine, Is.Not.Null, "Engine");
				if (engine is null)
					return;
				Assert.Throws<NotSupportedException> (() => callbacks.RemoveHintValue (engine, "key"));
			} finally {
				Marshal.FreeHGlobal (pointer);
			}
		}

		[Test]
		public void MissingCallback_Throws ()
		{
			var native = new FakeCallbacksNative { Version = 4 };
			var pointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, pointer, false);
				var callbacks = AuthorizationCallbacks.Create (pointer);
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				if (callbacks is null)
					return;
				using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
				Assert.That (engine, Is.Not.Null, "Engine");
				if (engine is null)
					return;
				Assert.Throws<InvalidOperationException> (() => callbacks.RequestInterrupt (engine));
			} finally {
				Marshal.FreeHGlobal (pointer);
			}
		}

		[Test]
		public void AllCallbackSlots_Invoke ()
		{
			var native = new FakeCallbacksNative {
				Version = 4,
				SetResult = (IntPtr) (delegate* unmanaged<IntPtr, AuthorizationResult, int>) &SetResultStub,
				RequestInterrupt = (IntPtr) (delegate* unmanaged<IntPtr, int>) &EngineOnlyStub,
				DidDeactivate = (IntPtr) (delegate* unmanaged<IntPtr, int>) &EngineOnlyStub,
				GetContextValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, int>) &GetContextValueStub,
				SetContextValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, IntPtr, int>) &SetContextValueStub,
				GetHintValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, IntPtr*, int>) &GetValueStub,
				SetHintValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, IntPtr, int>) &SetValueStub,
				GetArguments = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr*, int>) &GetPointerStub,
				GetSessionId = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr*, int>) &GetPointerStub,
				GetImmutableHintValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, IntPtr*, int>) &GetValueStub,
				GetLAContext = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr*, int>) &GetPointerStub,
				GetTokenIdentities = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, IntPtr*, int>) &GetTokenIdentitiesStub,
				GetTKTokenWatcher = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr*, int>) &GetPointerStub,
				RemoveHintValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, int>) &RemoveValueStub,
				RemoveContextValue = (IntPtr) (delegate* unmanaged<IntPtr, IntPtr, int>) &RemoveValueStub,
			};
			var pointer = Marshal.AllocHGlobal (Marshal.SizeOf<FakeCallbacksNative> ());
			try {
				Marshal.StructureToPtr (native, pointer, false);
				var callbacks = AuthorizationCallbacks.Create (pointer);
				using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
				using var context = new LAContext ();
				Assert.That (callbacks, Is.Not.Null, "Callbacks");
				Assert.That (engine, Is.Not.Null, "Engine");
				if (callbacks is null || engine is null)
					return;

				Assert.That (callbacks.RequestInterrupt (engine), Is.EqualTo (21), "RequestInterrupt");
				Assert.That (callbacks.DidDeactivate (engine), Is.EqualTo (21), "DidDeactivate");
				Assert.That (callbacks.SetContextValue (engine, "key", AuthorizationContextFlags.None, []), Is.EqualTo (22), "SetContextValue");
				Assert.That (callbacks.GetHintValue (engine, "key", out _), Is.EqualTo (23), "GetHintValue");
				Assert.That (callbacks.SetHintValue (engine, "key", []), Is.EqualTo (24), "SetHintValue");
				Assert.That (callbacks.GetArguments (engine, out _), Is.EqualTo (25), "GetArguments");
				Assert.That (callbacks.GetSessionId (engine, out _), Is.EqualTo (25), "GetSessionId");
				Assert.That (callbacks.GetImmutableHintValue (engine, "key", out _), Is.EqualTo (23), "GetImmutableHintValue");
				Assert.That (callbacks.GetLAContext (engine, out _), Is.EqualTo (25), "GetLAContext");
				Assert.That (callbacks.GetTokenIdentities (engine, context, out _), Is.EqualTo (26), "GetTokenIdentities");
				Assert.That (callbacks.GetTokenWatcher (engine, out _), Is.EqualTo (25), "GetTokenWatcher");
				Assert.That (callbacks.RemoveHintValue (engine, "key"), Is.EqualTo (27), "RemoveHintValue");
				Assert.That (callbacks.RemoveContextValue (engine, "key"), Is.EqualTo (27), "RemoveContextValue");
				Assert.That (lastEngine, Is.EqualTo ((IntPtr) 0x1234), "Engine");
			} finally {
				Marshal.FreeHGlobal (pointer);
			}
		}
	}
}
#endif // __MACOS__
