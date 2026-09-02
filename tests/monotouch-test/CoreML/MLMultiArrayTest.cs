//
// Unit tests for MLMultiArrayConstraint
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

using CoreML;

namespace MonoTouchFixtures.CoreML {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MLMultiArrayTest {
		[Test]
		public void Ctors ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);

			NSError err;
			var shape = new nint [] { 1 };
			var strides = new nint [] { 0 };
			var nsshape = new NSNumber [] { NSNumber.FromNInt (1) };
			var nsstrides = new NSNumber [] { NSNumber.FromNInt (0) };

			using (var arr = new MLMultiArray (shape, MLMultiArrayDataType.Int32, out err)) {
				Assert.That (arr.Shape, Is.EqualTo (shape), "1 Shape");
				Assert.That (arr.DataType, Is.EqualTo (MLMultiArrayDataType.Int32), "1 DataType");
				Assert.That (err, Is.Null, "1 err");
			}

			using (var arr = new MLMultiArray (IntPtr.Zero, shape, MLMultiArrayDataType.Float32, strides, (v) => Marshal.FreeHGlobal (v), out err)) {
				Assert.That (arr.Shape, Is.EqualTo (shape), "2 Shape");
				Assert.That (arr.DataType, Is.EqualTo (MLMultiArrayDataType.Float32), "2 DataType");
				Assert.That (arr.Strides, Is.EqualTo (strides), "2 Strides");
				Assert.That (err, Is.Null, "2 err");
			}

			using (var arr = new MLMultiArray (IntPtr.Zero, nsshape, MLMultiArrayDataType.Double, nsstrides, (v) => Marshal.FreeHGlobal (v), out err)) {
				Assert.That (arr.Shape, Is.EqualTo (shape), "3 Shape");
				Assert.That (arr.DataType, Is.EqualTo (MLMultiArrayDataType.Double), "3 DataType");
				Assert.That (arr.Strides, Is.EqualTo (strides), "3 Strides");
				Assert.That (arr.DataPointer, Is.EqualTo (IntPtr.Zero), "3 DataPointer");
				Assert.That (err, Is.Null, "3 err");
			}

			using (var arr = new MLMultiArray (nsshape, MLMultiArrayDataType.Int32, out err)) {
				Assert.That (arr.Shape, Is.EqualTo (shape), "4 Shape");
				Assert.That (arr.DataType, Is.EqualTo (MLMultiArrayDataType.Int32), "4 DataType");
				Assert.That (err, Is.Null, "4 err");
			}
		}

		[Test]
		public void Indexers ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);

			NSError err;
			var shape = new nint [] { 10 };
			using (var arr = new MLMultiArray (shape, MLMultiArrayDataType.Int32, out err)) {
				Assert.That (err, Is.Null, "err");
				Assert.That (arr.Count, Is.EqualTo ((nint) 10), "Count");
				Assert.That (arr.Shape, Is.EqualTo (new nint [] { 10 }), "Shape");
				Assert.That (arr.Strides, Is.EqualTo (new nint [] { 1 }), "Strides");

				arr [0] = 0; // MLMultiArray's elements aren't zero-initialized
				Assert.That (arr [0].Int32Value, Is.EqualTo (0), "a");
				Assert.That (arr [new nint [] { 0 }].Int32Value, Is.EqualTo (0), "b");
				Assert.That (arr [new NSNumber [] { NSNumber.FromNInt (0) }].Int32Value, Is.EqualTo (0), "c nint");
				Assert.That (arr [new NSNumber [] { NSNumber.FromInt32 (0) }].Int32Value, Is.EqualTo (0), "c int32");
				Assert.That (arr [new NSNumber [] { NSNumber.FromByte (0) }].Int32Value, Is.EqualTo (0), "c byte");
				Assert.That (arr [new NSNumber [] { NSNumber.FromFloat (0) }].Int32Value, Is.EqualTo (0), "c float");

				Assert.That (arr.GetObject (0).Int32Value, Is.EqualTo (0), "GetObject a");
				Assert.That (arr.GetObject (new nint [] { 0 }).Int32Value, Is.EqualTo (0), "GetObject b");
				Assert.That (arr.GetObject (new NSNumber [] { NSNumber.FromNInt (0) }).Int32Value, Is.EqualTo (0), "GetObject c nint");
				Assert.That (arr.GetObject (new NSNumber [] { NSNumber.FromInt32 (0) }).Int32Value, Is.EqualTo (0), "GetObject c int32");
				Assert.That (arr.GetObject (new NSNumber [] { NSNumber.FromByte (0) }).Int32Value, Is.EqualTo (0), "GetObject c byte");
				Assert.That (arr.GetObject (new NSNumber [] { NSNumber.FromFloat (0) }).Int32Value, Is.EqualTo (0), "GetObject c float");

				arr [1] = NSNumber.FromInt32 (1);
				arr [new nint [] { 2 }] = NSNumber.FromInt32 (2);
				arr [new NSNumber [] { NSNumber.FromUInt16 (3) }] = NSNumber.FromInt32 (3);
				arr.SetObject (NSNumber.FromInt32 (4), 4);
				arr.SetObject (NSNumber.FromInt32 (5), new nint [] { 5 });
				arr.SetObject (NSNumber.FromInt32 (6), new NSNumber [] { NSNumber.FromSByte (6) });

				Assert.That (arr [1].Int32Value, Is.EqualTo (1), "1");
				Assert.That (arr [2].Int32Value, Is.EqualTo (2), "2");
				Assert.That (arr [3].Int32Value, Is.EqualTo (3), "3");
				Assert.That (arr [4].Int32Value, Is.EqualTo (4), "4");
				Assert.That (arr [5].Int32Value, Is.EqualTo (5), "5");
				Assert.That (arr [6].Int32Value, Is.EqualTo (6), "6");
			}

			// multi-dimensional
			shape = new nint [] { 7, 7, 7 };
			using (var arr = new MLMultiArray (shape, MLMultiArrayDataType.Int32, out err)) {
				Assert.That (err, Is.Null, "err");
				Assert.That (arr.Count, Is.EqualTo (shape [0] * shape [1] * shape [2]), "Count");

				arr [0, 0, 0] = 0; // MLMultiArray's elements aren't zero-initialized
				Assert.That (arr [0, 0, 0].Int32Value, Is.EqualTo (0), "a");
				Assert.That (arr [new nint [] { 0, 0, 0 }].Int32Value, Is.EqualTo (0), "b");
				Assert.That (arr [new NSNumber [] { NSNumber.FromNInt (0), NSNumber.FromNInt (0), NSNumber.FromNInt (0) }].Int32Value, Is.EqualTo (0), "c nint");

				Assert.That (arr.GetObject (0, 0, 0).Int32Value, Is.EqualTo (0), "GetObject a");
				Assert.That (arr.GetObject (new nint [] { 0, 0, 0 }).Int32Value, Is.EqualTo (0), "GetObject b");
				Assert.That (arr.GetObject (new NSNumber [] { NSNumber.FromNInt (0), NSNumber.FromNInt (0), NSNumber.FromNInt (0) }).Int32Value, Is.EqualTo (0), "GetObject c nint");

				arr [1, 1, 1] = NSNumber.FromInt32 (1);
				arr [new nint [] { 2, 2, 2 }] = NSNumber.FromInt32 (2);
				arr [new NSNumber [] { NSNumber.FromUInt16 (3), NSNumber.FromUInt16 (3), NSNumber.FromUInt16 (3) }] = NSNumber.FromInt32 (3);
				arr.SetObject (NSNumber.FromInt32 (4), 4, 4, 4);
				arr.SetObject (NSNumber.FromInt32 (5), new nint [] { 5, 5, 5 });
				arr.SetObject (NSNumber.FromInt32 (6), new NSNumber [] { NSNumber.FromSByte (6), NSNumber.FromSByte (6), NSNumber.FromSByte (6) });

				Assert.That (arr [1, 1, 1].Int32Value, Is.EqualTo (1), "1");
				Assert.That (arr [2, 2, 2].Int32Value, Is.EqualTo (2), "2");
				Assert.That (arr [3, 3, 3].Int32Value, Is.EqualTo (3), "3");
				Assert.That (arr [4, 4, 4].Int32Value, Is.EqualTo (4), "4");
				Assert.That (arr [5, 5, 5].Int32Value, Is.EqualTo (5), "5");
				Assert.That (arr [6, 6, 6].Int32Value, Is.EqualTo (6), "6");
			}
		}
	}
}
