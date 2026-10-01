#if __MACOS__
#nullable enable

using System;
using System.Runtime.CompilerServices;
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

		[UnmanagedCallersOnly]
		static int SetResultStub (IntPtr engine, AuthorizationResult result)
		{
			lastEngine = engine;
			lastResult = result;
			return 17;
		}

		[Test]
		public void Accessors ()
		{
			var memory = stackalloc byte [sizeof (AuthorizationCallbacks)];
			new Span<byte> (memory, sizeof (AuthorizationCallbacks)).Clear ();
			*(uint*) memory = 4;
			var setResult = (delegate* unmanaged<IntPtr, AuthorizationResult, int>) &SetResultStub;
			*(IntPtr*) (memory + IntPtr.Size) = (IntPtr) setResult;
			*(IntPtr*) (memory + 15 * IntPtr.Size) = (IntPtr) 0x5678;
			var callbacks = (AuthorizationCallbacks*) memory;

			Assert.That (sizeof (AuthorizationCallbacks), Is.EqualTo (16 * IntPtr.Size), "Size");
			Assert.That (callbacks->Version, Is.EqualTo (4), "Version");
			Assert.That ((IntPtr) callbacks->SetResult, Is.EqualTo ((IntPtr) setResult), "SetResult");
			Assert.That ((IntPtr) callbacks->RemoveContextValue, Is.EqualTo ((IntPtr) 0x5678), "RemoveContextValue");
			Assert.That (callbacks->SetResult ((IntPtr) 0x1234, AuthorizationResult.Allow), Is.EqualTo (17), "Status");
			Assert.That (lastEngine, Is.EqualTo ((IntPtr) 0x1234), "Engine");
			Assert.That (lastResult, Is.EqualTo (AuthorizationResult.Allow), "Result");
		}

		[Test]
		public void VersionedAccessors ()
		{
			var memory = stackalloc byte [sizeof (AuthorizationCallbacks)];
			new Span<byte> (memory, sizeof (AuthorizationCallbacks)).Clear ();
			*(IntPtr*) (memory + 8 * IntPtr.Size) = (IntPtr) 0x1001;
			*(IntPtr*) (memory + 9 * IntPtr.Size) = (IntPtr) 0x1002;
			*(IntPtr*) (memory + 10 * IntPtr.Size) = (IntPtr) 0x1003;
			*(IntPtr*) (memory + 11 * IntPtr.Size) = (IntPtr) 0x1004;
			*(IntPtr*) (memory + 13 * IntPtr.Size) = (IntPtr) 0x1005;
			*(IntPtr*) (memory + 14 * IntPtr.Size) = (IntPtr) 0x1006;
			var callbacks = (AuthorizationCallbacks*) memory;

			*(uint*) memory = 0;
			Assert.That ((IntPtr) callbacks->GetArguments, Is.EqualTo ((IntPtr) 0x1001), "Version 0 arguments");
			Assert.That ((IntPtr) callbacks->GetSessionId, Is.EqualTo ((IntPtr) 0x1002), "Version 0 session ID");
			Assert.That ((IntPtr) callbacks->GetImmutableHintValue, Is.EqualTo (IntPtr.Zero), "Version 0 immutable hint");
			*(uint*) memory = 1;
			Assert.That ((IntPtr) callbacks->GetImmutableHintValue, Is.EqualTo ((IntPtr) 0x1003), "Version 1 immutable hint");
			Assert.That ((IntPtr) callbacks->GetLAContext, Is.EqualTo (IntPtr.Zero), "Version 1 LA context");
			*(uint*) memory = 2;
			Assert.That ((IntPtr) callbacks->GetLAContext, Is.EqualTo ((IntPtr) 0x1004), "Version 2 LA context");
			Assert.That ((IntPtr) callbacks->GetTKTokenWatcher, Is.EqualTo (IntPtr.Zero), "Version 2 token watcher");
			*(uint*) memory = 3;
			Assert.That ((IntPtr) callbacks->GetTKTokenWatcher, Is.EqualTo ((IntPtr) 0x1005), "Version 3 token watcher");
			Assert.That ((IntPtr) callbacks->RemoveHintValue, Is.EqualTo (IntPtr.Zero), "Version 3 remove hint");
			*(uint*) memory = 4;
			Assert.That ((IntPtr) callbacks->RemoveHintValue, Is.EqualTo ((IntPtr) 0x1006), "Version 4 remove hint");
		}

		[Test]
		public void PluginView_NullCallbacksThrows ()
		{
			using var engine = AuthorizationEngine.Create ((NativeHandle) (IntPtr) 0x1234);
			Assert.That (engine, Is.Not.Null, "Engine");
			if (engine is null)
				return;
			Assert.Throws<ArgumentNullException> (() => new SFAuthorizationPluginView (null, engine));
		}

		[Test]
		public void PluginView_NullEngineThrows ()
		{
			Assert.Throws<ArgumentNullException> (() => new SFAuthorizationPluginView ((AuthorizationCallbacks*) 0x1234, null));
		}
	}
}
#endif // __MACOS__
