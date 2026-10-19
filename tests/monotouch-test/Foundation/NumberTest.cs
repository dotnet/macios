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
			Assert.Throws<NullReferenceException> (() => NSNumber.ToBool (NativeHandle.Zero), "Bool");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToByte (NativeHandle.Zero), "Byte");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToSByte (NativeHandle.Zero), "SByte");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToInt16 (NativeHandle.Zero), "Int16");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToUInt16 (NativeHandle.Zero), "UInt16");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToInt32 (NativeHandle.Zero), "Int32");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToUInt32 (NativeHandle.Zero), "UInt32");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToInt64 (NativeHandle.Zero), "Int64");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToUInt64 (NativeHandle.Zero), "UInt64");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToFloat (NativeHandle.Zero), "Float");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToDouble (NativeHandle.Zero), "Double");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToNFloat (NativeHandle.Zero), "NFloat");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToNInt (NativeHandle.Zero), "NInt");
			Assert.Throws<NullReferenceException> (() => NSNumber.ToNUInt (NativeHandle.Zero), "NUInt");
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
