// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using ApplicationServices;

namespace MonoTouchFixtures.ApplicationServices {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AXValueTest {
		[Test]
		public void Point ()
		{
			var expected = new CGPoint (1.25, -2.5);
			using var value = new AXValue (expected);
			Assert.That (value.ValueType, Is.EqualTo (AXValueType.CGPoint), "Type");
			Assert.That ((nuint) CFType.GetTypeID (value.Handle), Is.EqualTo (AXValue.GetTypeID ()), "Type ID");
			Assert.That (value.TryGetValue (out CGPoint actual), Is.True, "Get point");
			Assert.That (actual, Is.EqualTo (expected), "Point");
			Assert.That (TestRuntime.CFGetRetainCount (value.Handle), Is.EqualTo ((nint) 1), "Ownership");
		}

		[Test]
		public void Size ()
		{
			var expected = new CGSize (123.5, 456.75);
			using var value = new AXValue (expected);
			Assert.That (value.ValueType, Is.EqualTo (AXValueType.CGSize), "Type");
			Assert.That (value.TryGetValue (out CGSize actual), Is.True, "Get size");
			Assert.That (actual, Is.EqualTo (expected), "Size");
		}

		[Test]
		public void Rectangle ()
		{
			var expected = new CGRect (-1.5, 2.25, 300.5, 400.75);
			using var value = new AXValue (expected);
			Assert.That (value.ValueType, Is.EqualTo (AXValueType.CGRect), "Type");
			Assert.That (value.TryGetValue (out CGRect actual), Is.True, "Get rectangle");
			Assert.That (actual, Is.EqualTo (expected), "Rectangle");
		}

		[Test]
		public void Range ()
		{
			var expected = new CFRange ((long) int.MaxValue + 123, (long) int.MaxValue + 456);
			using var value = new AXValue (expected);
			Assert.That (value.ValueType, Is.EqualTo (AXValueType.CFRange), "Type");
			Assert.That (value.TryGetValue (out CFRange actual), Is.True, "Get range");
			Assert.That (actual.LongLocation, Is.EqualTo (expected.LongLocation), "Location");
			Assert.That (actual.LongLength, Is.EqualTo (expected.LongLength), "Length");
		}

		[Test]
		public void Error ()
		{
			using var value = new AXValue (AXError.AttributeUnsupported);
			Assert.That (value.ValueType, Is.EqualTo (AXValueType.AXError), "Type");
			Assert.That (value.TryGetValue (out AXError actual), Is.True, "Get error");
			Assert.That (actual, Is.EqualTo (AXError.AttributeUnsupported), "Error");
		}

		[Test]
		public void TypeMismatch ()
		{
			using var value = new AXValue (new CGPoint (1, 2));
			Assert.That (value.TryGetValue (out CGRect rectangle), Is.False, "Rectangle mismatch");
			Assert.That (rectangle, Is.EqualTo (default (CGRect)), "Rectangle");
			Assert.That (value.TryGetValue (out CGSize size), Is.False, "Size mismatch");
			Assert.That (size, Is.EqualTo (default (CGSize)), "Size");
			Assert.That (value.TryGetValue (out CFRange range), Is.False, "Range mismatch");
			Assert.That (range.LongLocation, Is.Zero, "Range location");
			Assert.That (range.LongLength, Is.Zero, "Range length");
			Assert.That (value.TryGetValue (out AXError error), Is.False, "Error mismatch");
			Assert.That (error, Is.EqualTo (default (AXError)), "Error");
			using var errorValue = new AXValue (AXError.NoValue);
			Assert.That (errorValue.TryGetValue (out CGPoint point), Is.False, "Point mismatch");
			Assert.That (point, Is.EqualTo (default (CGPoint)), "Point");
		}

		[Test]
		public void Disposed ()
		{
			var value = new AXValue (new CGPoint (1, 2));
			value.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => _ = value.ValueType, "Value type");
			Assert.Throws<ObjectDisposedException> (() => value.TryGetValue (out CGPoint point), "Get value");
		}
	}
}
#endif
