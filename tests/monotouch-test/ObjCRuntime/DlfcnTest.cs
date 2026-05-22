//
// Unit tests for Dlfcn
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DlfcnTest {

		// These tests exercise [Field]-backed properties from Apple frameworks.
		// The generated binding code calls Dlfcn.GetStringConstant / GetIntPtr / etc.
		// under the hood, which is what InlineDlfcnMethodsStep transforms.

		[Test]
		public void StringConstant_NSLocaleNotification ()
		{
			var value = NSLocale.CurrentLocaleDidChangeNotification;
			Assert.That (value, Is.Not.Null, "CurrentLocaleDidChangeNotification");
			Assert.That ((string) value, Is.EqualTo ("kCFLocaleCurrentLocaleDidChangeNotification"), "value");
		}

		[Test]
		public void StringConstant_NSBundleNotification ()
		{
			var value = NSBundle.BundleDidLoadNotification;
			Assert.That (value, Is.Not.Null, "BundleDidLoadNotification");
			Assert.That ((string) value, Is.EqualTo ("NSBundleDidLoadNotification"), "value");
		}

		[Test]
		public void StringConstant_NSUserDefaultsNotification ()
		{
			var value = NSUserDefaults.DidChangeNotification;
			Assert.That (value, Is.Not.Null, "DidChangeNotification");
			Assert.That ((string) value, Is.EqualTo ("NSUserDefaultsDidChangeNotification"), "value");
		}

		[Test]
		public void StringConstant_NSUndoManagerNotification ()
		{
			var value = NSUndoManager.CheckpointNotification;
			Assert.That (value, Is.Not.Null, "CheckpointNotification");
			Assert.That ((string) value, Is.EqualTo ("NSUndoManagerCheckpointNotification"), "value");
		}

		[Test]
		public void StringConstant_CachePointer ()
		{
			// Access several string constants multiple times to test caching behavior.
			// The binding code uses Dlfcn.CachePointer for repeated accesses.
			for (int i = 0; i < 3; i++) {
				var value = NSLocale.CurrentLocaleDidChangeNotification;
				Assert.That (value, Is.Not.Null, $"iteration {i}");
			}
		}

		[Test]
		public void OpenClose_libSystem ()
		{
			IntPtr handle = Dlfcn.dlopen ("/usr/lib/libSystem.dylib", 0);
			Assert.That (handle, Is.Not.EqualTo (IntPtr.Zero), "dlopen");
			var err = Dlfcn.dlclose (handle);
			var expected = 0;
#if !MONOMAC && !__MACCATALYST__
			if (Runtime.Arch == Arch.DEVICE && TestRuntime.CheckXcodeVersion (7, 0) && !TestRuntime.CheckXcodeVersion (10, 0)) {
				// Apple is doing some funky stuff with dlopen... this condition is to track if this change during betas
				expected = -1;
			}
#endif
			Assert.That (err, Is.EqualTo (expected), "dlclose");
		}

		[Test]
		public void GetVariables ()
		{
			const string symbol = "x_native_field";
			var handle = (IntPtr) Dlfcn.RTLD.Default;

			Assert.That (Dlfcn.dlsym (handle, symbol), Is.Not.EqualTo (IntPtr.Zero), "Symbol");

			var originalValue = Dlfcn.GetUInt64 (handle, symbol);
			Assert.Multiple (() => {
				unchecked {
					// the n(uint) and (U)IntPtr asserts only work in 64-bit, which is fine because we only care about 64-bit right now.
					Assert.That ((ushort) Dlfcn.GetInt16 (handle, symbol), Is.EqualTo ((ushort) 0x8899), "GetInt16");
					Assert.That ((uint) Dlfcn.GetInt32 (handle, symbol), Is.EqualTo ((uint) 0xeeff8899), "GetInt32");
					Assert.That ((ulong) Dlfcn.GetInt64 (handle, symbol), Is.EqualTo ((ulong) 0xaabbccddeeff8899), "GetInt64");
					Assert.That ((nuint) Dlfcn.GetNInt (handle, symbol), Is.EqualTo ((nuint) 0xaabbccddeeff8899), "GetNInt");
					Assert.That (Dlfcn.GetUInt16 (handle, symbol), Is.EqualTo ((ushort) 0x8899), "GetUInt16");
					Assert.That (Dlfcn.GetUInt32 (handle, symbol), Is.EqualTo ((uint) 0xeeff8899), "GetUInt32");
					Assert.That (Dlfcn.GetUInt64 (handle, symbol), Is.EqualTo ((ulong) 0xaabbccddeeff8899), "GetUInt64");
					Assert.That (Dlfcn.GetNUInt (handle, symbol), Is.EqualTo ((nuint) 0xaabbccddeeff8899), "GetNUInt");
					Assert.That (Dlfcn.GetNFloat (handle, symbol), Is.EqualTo ((nfloat) (-7.757653393002521E-103)), "GetNFloat");
					Assert.That (Dlfcn.GetDouble (handle, symbol), Is.EqualTo (-7.7576533930025207E-103d), "GetDouble");
					Assert.That ((nuint) Dlfcn.GetIntPtr (handle, symbol), Is.EqualTo ((nuint) 0xaabbccddeeff8899), "GetIntPtr"); // won't work in 32-bit, but we don't care about that anymore
					Assert.That (Dlfcn.GetUIntPtr (handle, symbol), Is.EqualTo ((nuint) 0xaabbccddeeff8899), "GetUIntPtr");
					Assert.That (Dlfcn.GetStruct<nint> (handle, symbol), Is.EqualTo ((nint) 0xaabbccddeeff8899), "GetStruct<nint>"); // won't work in 32-bit, but we don't care about that anymore
					Assert.That (Dlfcn.GetStruct<nuint> (handle, symbol), Is.EqualTo ((nuint) 0xaabbccddeeff8899), "GetStruct<nuint>"); // won't work in 32-bit, but we don't care about that anymore
					Assert.That (Dlfcn.GetStruct<long> (handle, symbol), Is.EqualTo ((long) 0xaabbccddeeff8899), "GetStruct<long>");
					Assert.That (Dlfcn.GetStruct<ulong> (handle, symbol), Is.EqualTo ((ulong) 0xaabbccddeeff8899), "GetStruct<ulong>");
					Assert.That (Dlfcn.GetStruct<int> (handle, symbol), Is.EqualTo ((int) 0xeeff8899), "GetStruct<int>");
					Assert.That (Dlfcn.GetStruct<uint> (handle, symbol), Is.EqualTo ((uint) 0xeeff8899), "GetStruct<uint>");
					Assert.That (Dlfcn.GetStruct<SomeValue> (handle, symbol).Value, Is.EqualTo ((ulong) 0xaabbccddeeff8899), "GetStruct<SomeValue>");
					Assert.That (Dlfcn.GetStruct<float> (handle, symbol), Is.EqualTo (-3.9541907E+28f), "GetStruct<float>");
					Assert.That (Dlfcn.GetStruct<double> (handle, symbol), Is.EqualTo (-7.7576533930025207E-103d), "GetStruct<double>");

#if !STATIC_NATIVE_SYMBOL_LOOKUP
					Assert.That (Dlfcn.GetStruct<ulong> (handle, "inexistent_symbol"), Is.EqualTo ((ulong) 0), "GetStruct<ulong> inexistent");
					Assert.That (Dlfcn.GetStruct<SomeValue> (handle, "inexistent_symbol").Value, Is.EqualTo ((ulong) 0), "GetStruct<SomeValue> inexistent");
#endif

					Dlfcn.SetInt16 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetInt16 (handle, symbol), Is.EqualTo ((short) 0x77), "SetInt16");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetInt32 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetInt32 (handle, symbol), Is.EqualTo ((int) 0x77), "SetInt32");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetInt64 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetInt64 (handle, symbol), Is.EqualTo ((long) 0x77), "SetInt64");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetNInt (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetNInt (handle, symbol), Is.EqualTo ((nint) 0x77), "SetNInt");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetUInt16 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetUInt16 (handle, symbol), Is.EqualTo ((ushort) 0x77), "SetUInt16");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetUInt32 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetUInt32 (handle, symbol), Is.EqualTo ((uint) 0x77), "SetUInt32");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetUInt64 (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetUInt64 (handle, symbol), Is.EqualTo ((ulong) 0x77), "SetUInt64");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetNUInt (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetNUInt (handle, symbol), Is.EqualTo ((nuint) 0x77), "SetNUInt");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetNFloat (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetNFloat (handle, symbol), Is.EqualTo ((nfloat) 0x77), "SetNFloat");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetDouble (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetDouble (handle, symbol), Is.EqualTo (0x77), "SetDouble");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetIntPtr (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetIntPtr (handle, symbol), Is.EqualTo ((nint) 0x77), "SetIntPtr");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);

					Dlfcn.SetUIntPtr (handle, symbol, 0x77);
					Assert.That (Dlfcn.GetUIntPtr (handle, symbol), Is.EqualTo ((nuint) 0x77), "SetUIntPtr");
					Dlfcn.SetUInt64 (handle, symbol, originalValue);
				}
			});
		}

#pragma warning disable CS0649 // Field 'DlfcnTest.SomeValue.Value' is never assigned to, and will always have its default value 0
		struct SomeValue {
			public ulong Value;
		}
#pragma warning restore CS0649

		[Test]
		public void FieldProperty_CGRect ()
		{
			Assert.Multiple (() => {
				// CGRect.Null is backed by [Field("CGRectNull")] which calls Dlfcn.GetCGRect.
				var value = global::CoreGraphics.CGRect.Null;
				Assert.That (value.X, Is.EqualTo (nfloat.PositiveInfinity), "CGRectNull.X");
				Assert.That (value.Y, Is.EqualTo (nfloat.PositiveInfinity), "CGRectNull.Y");
				Assert.That (value.Width, Is.EqualTo ((nfloat) 0), "CGRectNull.Width");
				Assert.That (value.Height, Is.EqualTo ((nfloat) 0), "CGRectNull.Height");

				var infinite = global::CoreGraphics.CGRect.Infinite;
				Assert.That (infinite.X, Is.EqualTo (nfloat.MinValue / 2), "CGRectInfinite.X");
				Assert.That (infinite.Y, Is.EqualTo (nfloat.MinValue / 2), "CGRectInfinite.Y");
				Assert.That (infinite.Width, Is.EqualTo (nfloat.MaxValue), "CGRectInfinite.Width");
				Assert.That (infinite.Height, Is.EqualTo (nfloat.MaxValue), "CGRectInfinite.Height");
			});
		}
	}
}
