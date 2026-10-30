//
// Unit tests for NSDecimal
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014-2015 Xamarin Inc. All rights reserved.
//

using System.Globalization;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DecimalTest {

		[Test]
		public void ArithmeticInputs ()
		{
			NSDecimal left = 12;
			NSDecimal right = 3;
			var expectedLeft = left;
			var expectedRight = right;
#if XAMCORE_5_0
			Assert.That (NSDecimal.Compare (in left, in right), Is.EqualTo (NSComparisonResult.Descending), "Compare");
			Assert.That (NSDecimal.Add (out var sum, in left, in right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Add");
			Assert.That (NSDecimal.Subtract (out var difference, in left, in right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Subtract");
			Assert.That (NSDecimal.Multiply (out var product, in left, in right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Multiply");
			Assert.That (NSDecimal.Divide (out var quotient, in left, in right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Divide");
			Assert.That (NSDecimal.Power (out var power, in right, 2, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Power");
			Assert.That (NSDecimal.MultiplyByPowerOf10 (out var scaled, in right, 2, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "MultiplyByPowerOf10");
			NSDecimal.Round (out var rounded, in left, 0, NSRoundingMode.Plain);
#else
			Assert.That (NSDecimal.Compare (ref left, ref right), Is.EqualTo (NSComparisonResult.Descending), "Compare");
			Assert.That (NSDecimal.Add (out var sum, ref left, ref right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Add");
			Assert.That (NSDecimal.Subtract (out var difference, ref left, ref right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Subtract");
			Assert.That (NSDecimal.Multiply (out var product, ref left, ref right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Multiply");
			Assert.That (NSDecimal.Divide (out var quotient, ref left, ref right, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Divide");
			Assert.That (NSDecimal.Power (out var power, ref right, 2, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "Power");
			Assert.That (NSDecimal.MultiplyByPowerOf10 (out var scaled, ref right, 2, NSRoundingMode.Plain), Is.EqualTo (NSCalculationError.None), "MultiplyByPowerOf10");
			NSDecimal.Round (out var rounded, ref left, 0, NSRoundingMode.Plain);
#endif
			Assert.That ((int) sum, Is.EqualTo (15), "Sum");
			Assert.That ((int) difference, Is.EqualTo (9), "Difference");
			Assert.That ((int) product, Is.EqualTo (36), "Product");
			Assert.That ((int) quotient, Is.EqualTo (4), "Quotient");
			Assert.That ((int) power, Is.EqualTo (9), "Power result");
			Assert.That ((int) scaled, Is.EqualTo (300), "Scaled");
			Assert.That ((int) rounded, Is.EqualTo (12), "Rounded");
			Assert.That (left, Is.EqualTo (expectedLeft), "Unchanged left");
			Assert.That (right, Is.EqualTo (expectedRight), "Unchanged right");
		}

		[Test]
		public void CastFloat ()
		{
			float f = 0.7f;
			NSDecimal nsd = (NSDecimal) f;
			// We call NSNumberFormatter to adjust the test according to the iOS (not .NET) culture,
			// since NSDecimal.ToString () takes culture into account. .NET CultureInfo does not handle cases
			// like "en-HU"
			if (TestRuntime.CheckXcodeVersion (4, 5) && (CultureInfo.CurrentCulture.Name != "ar-AE")) {
				// interestingly the call to `NSDecimal.NSDecimalString (ref this, NSLocale.CurrentLocale.Handle);` does not consider the current locale on iOS 5.1
				var expected = NSNumberFormatter.LocalizedStringFromNumbernumberStyle ((NSNumber) f, NSNumberFormatterStyle.Decimal);
				Assert.That (nsd.ToString (), Is.EqualTo (expected), "ToString");
			}
			Assert.That (f, Is.EqualTo ((float) nsd), "float-rountrip");
		}

		[Test]
		public void CastDecimal ()
		{
			decimal m = 0.7m;
			NSDecimal nsd = (NSDecimal) m;
			// We call NSNumberFormatter to adjust the test according to the iOS (not .NET) culture,
			// since NSDecimal.ToString () takes culture into account. .NET CultureInfo does not handle cases
			// like "en-HU"
			// note: there's no NSNumber / Decimal conversions
			if (TestRuntime.CheckXcodeVersion (4, 5) && (CultureInfo.CurrentCulture.Name != "ar-AE")) {
				// interestingly the call to `NSDecimal.NSDecimalString (ref this, NSLocale.CurrentLocale.Handle);` does not consider the current locale on iOS 5.1
				var expected = NSNumberFormatter.LocalizedStringFromNumbernumberStyle ((NSNumber) 0.7d, NSNumberFormatterStyle.Decimal);
				Assert.That (nsd.ToString (), Is.EqualTo (expected), "ToString");
			}
			Assert.That (m, Is.EqualTo ((decimal) nsd), "decimal-rountrip");
		}

		[Test]
		public void CastDouble ()
		{
			double d = 0.7d;
			NSDecimal nsd = (NSDecimal) d;
			// We call NSNumberFormatter to adjust the test according to the iOS (not .NET) culture,
			// since NSDecimal.ToString () takes culture into account. .NET CultureInfo does not handle cases
			// like "en-HU"
			if (TestRuntime.CheckXcodeVersion (4, 5) && (CultureInfo.CurrentCulture.Name != "ar-AE")) {
				// interestingly the call to `NSDecimal.NSDecimalString (ref this, NSLocale.CurrentLocale.Handle);` does not consider the current locale on iOS 5.1
				var expected = NSNumberFormatter.LocalizedStringFromNumbernumberStyle ((NSNumber) d, NSNumberFormatterStyle.Decimal);
				Assert.That (nsd.ToString (), Is.EqualTo (expected), "ToString");
			}
			Assert.That (d, Is.EqualTo ((double) nsd), "double-rountrip");
		}

		[Test]
		public void CastInt ()
		{
			int i = 42;
			NSDecimal nsd = (NSDecimal) i;
			Assert.That (nsd.ToString (), Is.EqualTo ("42"), "ToString");
			Assert.That (i, Is.EqualTo ((int) nsd), "int-rountrip");
		}
	}
}
