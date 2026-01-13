//
// Unit tests for CGBitmapContext
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class BitmapContextTest {

		[Test]
		public void Ctor_CGBitmapFlags ()
		{
			// https://bugzilla.xamarin.com/show_bug.cgi?id=7441
			byte [] data = new byte [400];
			using (CGColorSpace space = CGColorSpace.CreateDeviceRGB ()) {
				// According to Apple "This value is equivalent to kCGImageAlphaNoneSkipLast" which is not true (at least in this "context" ;-)
				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.None); }, "None");

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.PremultipliedLast)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "PremultipliedLast");
				}

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.PremultipliedFirst)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "PremultipliedFirst");
				}

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.Last); }, "Last");

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.First); }, "First");

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.NoneSkipLast)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "NoneSkipLast");
				}

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.NoneSkipFirst)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "NoneSkipFirst");
				}

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGBitmapFlags.Only); }, "Only");
			}
		}

		[Test]
		public void Ctor_CGImageAlphaInfo ()
		{
			// https://bugzilla.xamarin.com/show_bug.cgi?id=7441
			byte [] data = new byte [400];
			using (CGColorSpace space = CGColorSpace.CreateDeviceRGB ()) {
				// According to Apple "This value is equivalent to kCGImageAlphaNoneSkipLast" which is not true (at least in this "context" ;-)
				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.None); }, "None");

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.PremultipliedLast)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "PremultipliedLast");
				}

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.PremultipliedFirst)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "PremultipliedFirst");
				}

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.Last); }, "Last");

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.First); }, "First");

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.NoneSkipLast)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "NoneSkipLast");
				}

				using (CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.NoneSkipFirst)) {
					Assert.That (c.Handle, Is.Not.EqualTo (IntPtr.Zero), "NoneSkipFirst");
				}

				Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.Only); }, "Only");
			}
		}

		[Test]
		public void Ctor_CGColorSpace_Null ()
		{
			byte [] data = new byte [400];
			// a null colorspace is not always accepted - that will return an invalid (IntPtr.Zero) handle and CGContext.set_Handle will throw
			Assert.Throws<Exception> (delegate { new CGBitmapContext (data, 10, 10, 8, 40, null, CGImageAlphaInfo.NoneSkipFirst); }, "null");

			// OTOH a null colorspace is possible with the valid parameters, e.g. bug #25600, so we can't throw a ANE blindly
			using (var context = new CGBitmapContext (null, 16, 32, 8, 0, null, CGImageAlphaInfo.Only)) {
				Assert.That (context.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
				Assert.Null (context.ColorSpace, "ColorSpace");
			}
		}

		[Test]
		public void ToImage ()
		{
			byte [] data = new byte [400];
			using (CGColorSpace space = CGColorSpace.CreateDeviceRGB ()) {
				CGBitmapContext c = new CGBitmapContext (data, 10, 10, 8, 40, space, CGImageAlphaInfo.PremultipliedLast);
				using (var img = c.ToImage ())
					Assert.NotNull (img, "ToImage");
				c.Dispose (); // Handle is now 0x0
				Assert.Null (c.ToImage (), "ToImage/Disposed");
			}
		}

		[Test]
		public void CreateAdaptive_1 ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			nuint width = 256;
			nuint height = 256;

			using (var pool = new NSAutoreleasePool ()) {
				using var context = CGBitmapContext.Create (width, height, (NSDictionary?) null, null, null, null, null);
				Assert.NotNull (context, "Context#1");
			}
		}

		[Test]
		public void CreateAdaptive_2 ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			nuint width = 256;
			nuint height = 256;

			var calledOnLockPointer = false;
			var calledOnUnlockPointer = false;
			var calledOnReleaseInfo = false;
			const int renderingBufferProviderSize = 512;

			var calledOnResolve = false;
			var calledOnAllocate = false;
			var calledOnFree = false;
			var calledOnError = false;

			using (var pool = new NSAutoreleasePool ()) {
				using var context = CGBitmapContext.Create (width, height, (CGAdaptiveOptions?) null,
					(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnResolve#2 info={info} parameters={parameters}");
						calledOnResolve = true;
						return true;
					},
					(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnAllocate#2 info={info} parameters={parameters}");
						calledOnAllocate = true;
						var renderingBufferProvider = CGRenderingBufferProvider.Create (IntPtr.Zero, renderingBufferProviderSize,
							lockPointer: (info) => {
								calledOnLockPointer = true;
								var rv = Marshal.AllocHGlobal (renderingBufferProviderSize);
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnLockPointer#2 (0x{info:x}) => 0x{rv:x}");
								return rv;
							},
							unlockPointer: (info, pointer) => {
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnUnlockPointer#2 (0x{info:x}, 0x{pointer:x})");
								calledOnUnlockPointer = true;
								Marshal.FreeHGlobal (pointer);
							},
							releaseInfo: (info) => {
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnReleaseInfo#2 (0x{info:x})");
								calledOnReleaseInfo = true;
							}
						);
						return renderingBufferProvider;
					},
					(CGRenderingBufferProvider renderingBufferProvider, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnFree#2 renderingBufferProvider={renderingBufferProvider} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
						calledOnFree = true;
					},
					(NSError error, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnError#2 error={error} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
						calledOnError = true;
					});

				Assert.NotNull (context, "Context#2");

				using var img = context.ToImage ();
				Assert.NotNull (img, "ToImage");
			}

			Assert.That (calledOnResolve, Is.True, "calledOnResolve#2");
			Assert.That (calledOnAllocate, Is.True, "calledOnAllocate#2");
			Assert.That (calledOnFree, Is.True, "calledOnFree#2");
			Assert.That (calledOnError, Is.False, "calledOnError#2");

			Assert.That (calledOnLockPointer, Is.True, "calledOnLockPointer#2");
			Assert.That (calledOnUnlockPointer, Is.True, "calledOnUnlockPointer#2");
			Assert.That (calledOnReleaseInfo, Is.False, "calledOnReleaseInfo#2");
		}

		[Test]
		public void CreateAdaptive_3 ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			nuint width = 256;
			nuint height = 256;

			var calledOnLockPointer = false;
			var calledOnUnlockPointer = false;
			var calledOnReleaseInfo = false;
			const int renderingBufferProviderSize = 512;

			var calledOnResolve = false;
			var calledOnAllocate = false;
			var calledOnFree = false;
			var calledOnError = false;
			var options = new CGAdaptiveOptions () {
				MaximumBitDepth = CGComponent.Float16Bit,
			};

			using (var pool = new NSAutoreleasePool ()) {
				using var context = CGBitmapContext.Create (width, height, options,
					(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnResolve#3 info={info} parameters={parameters}");
						calledOnResolve = true;
						return true;
					},
					(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnAllocate#3 info={info} parameters={parameters}");
						calledOnAllocate = true;
						var renderingBufferProvider = CGRenderingBufferProvider.Create (IntPtr.Zero, renderingBufferProviderSize,
							lockPointer: (info) => {
								calledOnLockPointer = true;
								var rv = Marshal.AllocHGlobal (renderingBufferProviderSize);
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnLockPointer#3 (0x{info:x}) => 0x{rv:x}");
								return rv;
							},
							unlockPointer: (info, pointer) => {
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnUnlockPointer#3 (0x{info:x}, 0x{pointer:x})");
								calledOnUnlockPointer = true;
								Marshal.FreeHGlobal (pointer);
							},
							releaseInfo: (info) => {
								// TestRuntime.NSLog ($"CreateAdaptive3 () OnReleaseInfo#3 (0x{info:x})");
								calledOnReleaseInfo = true;
							}
						);
						return renderingBufferProvider;
					},
					(CGRenderingBufferProvider renderingBufferProvider, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnFree#3 renderingBufferProvider={renderingBufferProvider} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
						calledOnFree = true;
					},
					(NSError error, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnError#3 error={error} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
						calledOnError = true;
					});

				Assert.NotNull (context, "Context#3");

				using var img = context.ToImage ();
				Assert.NotNull (img, "ToImage");
			}

			Assert.That (calledOnResolve, Is.True, "calledOnResolve#3");
			Assert.That (calledOnAllocate, Is.True, "calledOnAllocate#3");
			Assert.That (calledOnFree, Is.True, "calledOnFree#3");
			Assert.That (calledOnError, Is.False, "calledOnError#3");

			Assert.That (calledOnLockPointer, Is.True, "calledOnLockPointer#3");
			Assert.That (calledOnUnlockPointer, Is.True, "calledOnUnlockPointer#3");
			Assert.That (calledOnReleaseInfo, Is.False, "calledOnReleaseInfo#3");
		}

		[Test]
		public void CreateAdaptive_4 ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			nuint width = 256;
			nuint height = 256;

			var calledOnLockPointer = false;
			var calledOnUnlockPointer = false;
			var calledOnReleaseInfo = false;
			const int renderingBufferProviderSize = 512;

			using (var pool = new NSAutoreleasePool ()) {
				using (var renderingBufferProvider = CGRenderingBufferProvider.Create (IntPtr.Zero, renderingBufferProviderSize,
					lockPointer: (info) => {
						calledOnLockPointer = true;
						var rv = Marshal.AllocHGlobal (renderingBufferProviderSize);
						// TestRuntime.NSLog ($"CreateAdaptive () OnLockPointer#4 (0x{info:x}) => 0x{rv:x}");
						return rv;
					},
					unlockPointer: (info, pointer) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnUnlockPointer#4 (0x{info:x}, 0x{pointer:x})");
						calledOnUnlockPointer = true;
						Marshal.FreeHGlobal (pointer);
					},
					releaseInfo: (info) => {
						// TestRuntime.NSLog ($"CreateAdaptive () OnReleaseInfo#4 (0x{info:x})");
						calledOnReleaseInfo = true;
					}
				)) {
					Assert.That (renderingBufferProvider, Is.Not.Null, "RenderingBufferProvider");

					var calledOnResolve = false;
					var calledOnAllocate = false;
					var calledOnFree = false;
					var calledOnError = false;
					var options = new CGAdaptiveOptions () {
						MaximumBitDepth = CGComponent.Float16Bit,
					};

					using (var context = CGBitmapContext.Create (width, height, options,
						(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
							// TestRuntime.NSLog ($"CreateAdaptive () OnResolve#4 info={info} parameters={parameters}");
							calledOnResolve = true;
							return true;
						},
						(ref CGContentInfo info, ref CGBitmapParameters parameters) => {
							// TestRuntime.NSLog ($"CreateAdaptive () OnAllocate#4 info={info} parameters={parameters}");
							calledOnAllocate = true;
							return renderingBufferProvider;
						},
						(CGRenderingBufferProvider renderingBufferProvider, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
							// TestRuntime.NSLog ($"CreateAdaptive () OnFree#4 renderingBufferProvider={renderingBufferProvider} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
							calledOnFree = true;
						},
						(NSError error, ref CGContentInfo contentInfo, ref CGBitmapParameters bitmapParameters) => {
							// TestRuntime.NSLog ($"CreateAdaptive () OnError#4 error={error} contentInfo={contentInfo} bitmapParameters={bitmapParameters}");
							calledOnError = true;
						})) {

						Assert.NotNull (context, "Context#4");

						using var img = context.ToImage ();
						Assert.NotNull (img, "ToImage");
					}

					Assert.That (calledOnResolve, Is.True, "calledOnResolve#4");
					Assert.That (calledOnAllocate, Is.True, "calledOnAllocate#4");
					Assert.That (calledOnFree, Is.True, "calledOnFree#4");
					Assert.That (calledOnError, Is.False, "calledOnError#4");
				}
			}

			Assert.That (calledOnLockPointer, Is.True, "calledOnLockPointer#4");
			Assert.That (calledOnUnlockPointer, Is.True, "calledOnUnlockPointer#4");
			Assert.That (calledOnReleaseInfo, Is.True, "calledOnReleaseInfo#4");
		}
	}
}
