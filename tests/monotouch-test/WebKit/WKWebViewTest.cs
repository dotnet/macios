// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if HAS_WEBKIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CoreGraphics;
using WebKit;

namespace MonoTouchFixtures.WebKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class WKWebViewTest {
		const string switchName = "WebKit.WKWebView.EnableMemoryPressure";
		const int expectedPressure = 32 * 1024 * 1024;
		bool previousSwitch;

		[UnsafeAccessor (UnsafeAccessorKind.Field, Name = "memoryPressure")]
		static extern ref int GetMemoryPressure (WKWebView view);

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern IntPtr SendIntPtr (IntPtr receiver, IntPtr selector);

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern IntPtr InitializeWebView (IntPtr receiver, IntPtr selector, CGRect frame, IntPtr configuration);

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern void SendVoid (IntPtr receiver, IntPtr selector);

		[SetUp]
		public void SetUp ()
		{
			previousSwitch = !AppContext.TryGetSwitch (switchName, out var enabled) || enabled;
		}

		[TearDown]
		public void TearDown ()
		{
			AppContext.SetSwitch (switchName, previousSwitch);
		}

		[TestCase (true)]
		[TestCase (false)]
		public void ManagedConstruction (bool enabled)
		{
			AppContext.SetSwitch (switchName, enabled);
			using var configuration = new WKWebViewConfiguration ();
			using var view = new WKWebView (CGRect.Empty, configuration);
			Assert.That (GetMemoryPressure (view), Is.EqualTo (enabled ? expectedPressure : 0), "Initial pressure");

			AppContext.SetSwitch (switchName, !enabled);
			view.Dispose ();
			Assert.That (GetMemoryPressure (view), Is.Zero, "Disposal after switch change");
			view.Dispose ();
			Assert.That (GetMemoryPressure (view), Is.Zero, "Repeated disposal");
		}

		[TestCase (true)]
		[TestCase (false)]
		public void NativeConstruction (bool enabled)
		{
			AppContext.SetSwitch (switchName, enabled);
			using var configuration = new WKWebViewConfiguration ();
			var allocated = SendIntPtr (Class.GetHandle ("WKWebView"), Selector.GetHandle ("alloc"));
			var handle = InitializeWebView (allocated, Selector.GetHandle ("initWithFrame:configuration:"), CGRect.Empty, configuration.Handle);
			Assert.That (handle, Is.Not.EqualTo (IntPtr.Zero), "Native initialization");
			try {
				using var view = Runtime.GetNSObject<WKWebView> (handle);
				Assert.That (GetMemoryPressure (view), Is.EqualTo (enabled ? expectedPressure : 0), "Native-created peer");
				Assert.That (Runtime.GetNSObject<WKWebView> (handle), Is.SameAs (view), "Reusing peer");
				Assert.That (GetMemoryPressure (view), Is.EqualTo (enabled ? expectedPressure : 0), "No duplicate pressure");
			} finally {
				SendVoid (handle, Selector.GetHandle ("release"));
			}
		}

		[TestCase (true)]
		[TestCase (false)]
		public void RepeatedInitialization (bool enabled)
		{
			AppContext.SetSwitch (switchName, enabled);
			using var configuration = new WKWebViewConfiguration ();
			using var view = new TestWebView (configuration);
			AppContext.SetSwitch (switchName, !enabled);
			view.InitializeAgain ();
			Assert.That (GetMemoryPressure (view), Is.EqualTo (enabled ? expectedPressure : 0), "Original accounting and switch decision");
		}

		[Test]
		public void DeferredRelease ()
		{
			AppContext.SetSwitch (switchName, true);
			using var configuration = new WKWebViewConfiguration ();
			using var view = new TestWebView (configuration);
			Assert.That (GetMemoryPressure (view), Is.EqualTo (expectedPressure), "Before finalizer path");
			view.QueueRelease ();
			Assert.That (GetMemoryPressure (view), Is.Zero, "Pressure removed before deferred native release");
			view.Dispose ();
			Assert.That (GetMemoryPressure (view), Is.Zero, "No duplicate removal");
		}

		[MethodImpl (MethodImplOptions.NoInlining)]
		static WeakReference<WKWebView> CreateCollectibleView ()
		{
			using var configuration = new WKWebViewConfiguration ();
			var view = new WKWebView (CGRect.Empty, configuration);
			Assert.That (GetMemoryPressure (view), Is.EqualTo (expectedPressure), "Before collection");
			return new WeakReference<WKWebView> (view, trackResurrection: true);
		}

		[Test]
		public void Finalization ()
		{
			AppContext.SetSwitch (switchName, true);
			var reference = CreateCollectibleView ();
			Assert.That (TestRuntime.RunAsync (TimeSpan.FromSeconds (10), () => {
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				return !reference.TryGetTarget (out var view) || GetMemoryPressure (view) == 0;
			}), Is.True, "Finalization releases memory pressure");
		}

		[Register ("MemoryPressureTestWebView")]
		[Preserve (AllMembers = true)]
		class TestWebView : WKWebView {
			public TestWebView (WKWebViewConfiguration configuration) : base (CGRect.Empty, configuration)
			{
			}

			public TestWebView (NativeHandle handle) : base (handle)
			{
			}

			public void InitializeAgain ()
			{
				InitializeHandle (Handle);
			}

			public void QueueRelease ()
			{
				Dispose (false);
			}
		}
	}
}

#endif
