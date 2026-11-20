//
// Unit tests for NSNumber
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NumberTest {

		[Test]
		public void ToValue ()
		{
			NSNumber [] numbers = [
				new NSNumber (true),
				new NSNumber (false),
				new NSNumber (long.MinValue),
				new NSNumber (long.MaxValue),
				new NSNumber (ulong.MaxValue),
				new NSNumber (-123.75),
				new NSNumber (123.75f),
				new NSNumber (double.NaN),
				new NSNumber (double.PositiveInfinity),
				new NSNumber (double.NegativeInfinity),
				new NSDecimalNumber ("123.75"),
			];
			foreach (var number in numbers) {
				using (number) {
					var handle = number.Handle;
					Assert.That (NSNumber.ToBool (handle), Is.EqualTo (number.BoolValue), "Bool");
					Assert.That (NSNumber.ToByte (handle), Is.EqualTo (number.ByteValue), "Byte");
					Assert.That (NSNumber.ToSByte (handle), Is.EqualTo (number.SByteValue), "SByte");
					Assert.That (NSNumber.ToInt16 (handle), Is.EqualTo (number.Int16Value), "Int16");
					Assert.That (NSNumber.ToUInt16 (handle), Is.EqualTo (number.UInt16Value), "UInt16");
					Assert.That (NSNumber.ToInt32 (handle), Is.EqualTo (number.Int32Value), "Int32");
					Assert.That (NSNumber.ToUInt32 (handle), Is.EqualTo (number.UInt32Value), "UInt32");
					Assert.That (NSNumber.ToInt64 (handle), Is.EqualTo (number.Int64Value), "Int64");
					Assert.That (NSNumber.ToUInt64 (handle), Is.EqualTo (number.UInt64Value), "UInt64");
					Assert.That (NSNumber.ToFloat (handle), Is.EqualTo (number.FloatValue), "Float");
					Assert.That (NSNumber.ToDouble (handle), Is.EqualTo (number.DoubleValue), "Double");
					Assert.That ((double) NSNumber.ToNFloat (handle), Is.EqualTo ((double) number.NFloatValue), "NFloat");
					Assert.That (NSNumber.ToNInt (handle), Is.EqualTo (number.NIntValue), "NInt");
					Assert.That (NSNumber.ToNUInt (handle), Is.EqualTo (number.NUIntValue), "NUInt");
					Assert.That (NSNumber.ToNullableBool (handle), Is.EqualTo (number.BoolValue), "NullableBool");
					Assert.That (NSNumber.ToNullableByte (handle), Is.EqualTo (number.ByteValue), "NullableByte");
					Assert.That (NSNumber.ToNullableSByte (handle), Is.EqualTo (number.SByteValue), "NullableSByte");
					Assert.That (NSNumber.ToNullableInt16 (handle), Is.EqualTo (number.Int16Value), "NullableInt16");
					Assert.That (NSNumber.ToNullableUInt16 (handle), Is.EqualTo (number.UInt16Value), "NullableUInt16");
					Assert.That (NSNumber.ToNullableInt32 (handle), Is.EqualTo (number.Int32Value), "NullableInt32");
					Assert.That (NSNumber.ToNullableUInt32 (handle), Is.EqualTo (number.UInt32Value), "NullableUInt32");
					Assert.That (NSNumber.ToNullableInt64 (handle), Is.EqualTo (number.Int64Value), "NullableInt64");
					Assert.That (NSNumber.ToNullableUInt64 (handle), Is.EqualTo (number.UInt64Value), "NullableUInt64");
					Assert.That (NSNumber.ToNullableFloat (handle), Is.EqualTo (number.FloatValue), "NullableFloat");
					Assert.That (NSNumber.ToNullableDouble (handle), Is.EqualTo (number.DoubleValue), "NullableDouble");
					Assert.That ((double?) NSNumber.ToNullableNFloat (handle), Is.EqualTo ((double) number.NFloatValue), "NullableNFloat");
					Assert.That (NSNumber.ToNullableNInt (handle), Is.EqualTo (number.NIntValue), "NullableNInt");
					Assert.That (NSNumber.ToNullableNUInt (handle), Is.EqualTo (number.NUIntValue), "NullableNUInt");
					Assert.That (number.Handle, Is.EqualTo (handle), "The existing wrapper must not be disposed");
				}
			}
		}

		[Test]
		public void ToValueArray ()
		{
			using var first = new NSNumber (1.25f);
			using var second = new NSNumber (-2.5f);
			using var array = NSArray.FromNSObjects (first, second);
			var values = NSArray.ArrayFromHandleFunc<float> (array.Handle, NSNumber.ToFloat);
			Assert.That (values, Is.EqualTo (new float [] { 1.25f, -2.5f }), "Values");
			Assert.That (first.Handle, Is.Not.EqualTo (NativeHandle.Zero), "First wrapper");
			Assert.That (second.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Second wrapper");
		}

		[Test]
		public void ToValueDoesNotAllocate ()
		{
			using var number = new NSNumber (123.75);
			var handle = number.Handle;
			NSNumber.ToDouble (handle);
			Assert.That (number.Handle, Is.EqualTo (handle), "The existing wrapper must not be disposed");
			for (var i = 0; i < 100; i++)
				NSNumber.ToDouble (handle);
			var before = GC.GetAllocatedBytesForCurrentThread ();
			for (var i = 0; i < 1000; i++)
				NSNumber.ToDouble (handle);
			var allocated = GC.GetAllocatedBytesForCurrentThread () - before;
			Assert.That (allocated, Is.EqualTo (0), "Managed allocations");
			Assert.That (number.Handle, Is.EqualTo (handle), "The existing wrapper must not be disposed");
		}

		[Test]
		public void ToValueNull ()
		{
			Assert.That (NSNumber.ToBool (NativeHandle.Zero), Is.EqualTo (default (bool)), "Bool");
			Assert.That (NSNumber.ToByte (NativeHandle.Zero), Is.EqualTo (default (byte)), "Byte");
			Assert.That (NSNumber.ToSByte (NativeHandle.Zero), Is.EqualTo (default (sbyte)), "SByte");
			Assert.That (NSNumber.ToInt16 (NativeHandle.Zero), Is.EqualTo (default (short)), "Int16");
			Assert.That (NSNumber.ToUInt16 (NativeHandle.Zero), Is.EqualTo (default (ushort)), "UInt16");
			Assert.That (NSNumber.ToInt32 (NativeHandle.Zero), Is.EqualTo (default (int)), "Int32");
			Assert.That (NSNumber.ToUInt32 (NativeHandle.Zero), Is.EqualTo (default (uint)), "UInt32");
			Assert.That (NSNumber.ToInt64 (NativeHandle.Zero), Is.EqualTo (default (long)), "Int64");
			Assert.That (NSNumber.ToUInt64 (NativeHandle.Zero), Is.EqualTo (default (ulong)), "UInt64");
			Assert.That (NSNumber.ToFloat (NativeHandle.Zero), Is.EqualTo (default (float)), "Float");
			Assert.That (NSNumber.ToDouble (NativeHandle.Zero), Is.EqualTo (default (double)), "Double");
			Assert.That ((double) NSNumber.ToNFloat (NativeHandle.Zero), Is.EqualTo (0d), "NFloat");
			Assert.That (NSNumber.ToNInt (NativeHandle.Zero), Is.EqualTo (default (nint)), "NInt");
			Assert.That (NSNumber.ToNUInt (NativeHandle.Zero), Is.EqualTo (default (nuint)), "NUInt");
			Assert.That (NSNumber.ToNullableBool (NativeHandle.Zero), Is.Null, "NullableBool");
			Assert.That (NSNumber.ToNullableByte (NativeHandle.Zero), Is.Null, "NullableByte");
			Assert.That (NSNumber.ToNullableSByte (NativeHandle.Zero), Is.Null, "NullableSByte");
			Assert.That (NSNumber.ToNullableInt16 (NativeHandle.Zero), Is.Null, "NullableInt16");
			Assert.That (NSNumber.ToNullableUInt16 (NativeHandle.Zero), Is.Null, "NullableUInt16");
			Assert.That (NSNumber.ToNullableInt32 (NativeHandle.Zero), Is.Null, "NullableInt32");
			Assert.That (NSNumber.ToNullableUInt32 (NativeHandle.Zero), Is.Null, "NullableUInt32");
			Assert.That (NSNumber.ToNullableInt64 (NativeHandle.Zero), Is.Null, "NullableInt64");
			Assert.That (NSNumber.ToNullableUInt64 (NativeHandle.Zero), Is.Null, "NullableUInt64");
			Assert.That (NSNumber.ToNullableFloat (NativeHandle.Zero), Is.Null, "NullableFloat");
			Assert.That (NSNumber.ToNullableDouble (NativeHandle.Zero), Is.Null, "NullableDouble");
			Assert.That (NSNumber.ToNullableNFloat (NativeHandle.Zero), Is.Null, "NullableNFloat");
			Assert.That (NSNumber.ToNullableNInt (NativeHandle.Zero), Is.Null, "NullableNInt");
			Assert.That (NSNumber.ToNullableNUInt (NativeHandle.Zero), Is.Null, "NullableNUInt");
		}

		[Test]
		public void CompareTo ()
		{
			NSNumber n0 = (NSNumber) 0;
			NSNumber n1 = (NSNumber) 1;
			NSNumber n1f = (NSNumber) 1.0f;
			NSNumber n2 = (NSNumber) 2.0f;

			Assert.That (n1.Compare (n1f), Is.EqualTo ((nint) 0), "Compare-a");
			Assert.That (n0.Compare (n1f), Is.LessThan ((nint) 0), "Compare-b");
			Assert.That (n2.Compare (n0), Is.GreaterThan ((nint) 0), "Compare-c");

			// IComparable
			Assert.That (n1.CompareTo ((object) n1f), Is.EqualTo (0), "CompareTo-a");
			Assert.That (n0.CompareTo ((object) n1f), Is.LessThan (0), "CompareTo-b");
			Assert.That (n2.CompareTo ((object) n0), Is.GreaterThan (0), "CompareTo-c");

			// IComparable<NSNumber>
			Assert.That (n1.CompareTo (n1f), Is.EqualTo (0), "CompareTo<NSNumber>-a");
			Assert.That (n0.CompareTo (n1f), Is.LessThan (0), "CompareTo<NSNumber>-b");
			Assert.That (n2.CompareTo (n0), Is.GreaterThan (0), "CompareTo<NSNumber>-c");
		}

		[Test]
		public void CtorNSCoder ()
		{
			// NSNumber conforms to NSCoding - so it's .ctor(NSCoder) is usable
			using (var n = new NSNumber (-1))
			using (var d = new NSMutableData ()) {
				using (var a = new NSKeyedArchiver (d)) {
					n.EncodeTo (a);
					a.FinishEncoding ();
				}
				using (var u = new NSKeyedUnarchiver (d))
				using (var n2 = new NSNumber (u)) {
					// so we can re-create an instance from it
					Assert.That (n.Int32Value, Is.EqualTo (-1), "Value");
				}
			}
		}

		[Test]
		public void Singleton ()
		{
			var a = new NSNumber (true);
			var b = new NSNumber (true);
			a.Dispose ();
			Assert.That (b.Handle, Is.Not.EqualTo (IntPtr.Zero));
		}

		[Test]
		public void Equals ()
		{
			using (var a = new NSNumber (1))
			using (var b = new NSNumber (1d)) {
				// Two objects that are equal return hash codes that are equal.
				Assert.That (a.Equals (b), Is.True, "Equals(NSNumber)");
				Assert.That (b.Equals ((object) a), Is.True, "Equals(Object)");
				Assert.That (a.GetHashCode (), Is.EqualTo (b.GetHashCode ()), "GetHashCode");
			}
		}
	}
}
