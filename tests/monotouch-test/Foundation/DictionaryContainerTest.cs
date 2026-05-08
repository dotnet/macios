//
// Unit tests for DictionaryContainer
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System.Linq;

using CoreGraphics;
using CoreMedia;
using VideoToolbox;

using NUnit.Framework;

#if HAS_UIKIT
using UIKit;
#endif

using Bindings.Test;

namespace MonoTouchFixtures.Foundation {

	public class DictionaryContainerPoker : DictionaryContainer {

		public DictionaryContainerPoker ()
		{
		}


		public void SetArrayValue_ (NSString key, NSNumber [] values)
		{
			SetArrayValue (key, values);
		}

		public void SetArrayValue_ (NSString key, string [] values)
		{
			SetArrayValue (key, values);
		}

		public void SetArrayValue_<T> (NSString key, T [] values)
		{
			SetArrayValue<T> (key, values);
		}

		public void SetArrayValue_ (NSString key, INativeObject [] values)
		{
			SetArrayValue (key, values);
		}

		public void SetBooleanValue_ (NSString key, bool? value)
		{
			SetBooleanValue (key, value);
		}

		public void SetNumberValue_ (NSString key, int? value)
		{
			SetNumberValue (key, value);
		}

		public void SetNumberValue_ (NSString key, uint? value)
		{
			SetNumberValue (key, value);
		}

		public void SetNumberValue_ (NSString key, nint? value)
		{
			SetNumberValue (key, value);
		}

		public void SetNumberValue_ (NSString key, long? value)
		{
			SetNumberValue (key, value);
		}

		public void SetNumberValue_ (NSString key, nuint? value)
		{
			SetNumberValue (key, value);
		}

		public void SetStringValue_ (NSString key, string value)
		{
			SetStringValue (key, value);
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DictionaryContainerTest {

		[Test]
		public void Empty ()
		{
			var dc = new DictionaryContainerPoker ();
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "Count");
		}

		NSString key = new NSString ("key");

		[Test]
		public void SetArrayValue_NSNumber ()
		{
			var numbers = new NSNumber [] { (NSNumber) 0, (NSNumber) 1 };
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetArrayValue_ (null, numbers);
			}, "null key");

			dc.SetArrayValue_ (key, numbers);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");
			Assert.That ((int) (dc.Dictionary [key] as NSArray).Count, Is.EqualTo (2), "2");

			numbers = null;
			dc.SetArrayValue_ (key, numbers);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetArrayValue_String ()
		{
			var strings = new String [] { "xamarin", "monkeys" };
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetArrayValue_ (null, strings);
			}, "null key");

			dc.SetArrayValue_ (key, strings);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");
			Assert.That ((int) (dc.Dictionary [key] as NSArray).Count, Is.EqualTo (2), "2");

			strings = null;
			dc.SetArrayValue_ (key, strings);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetArrayValue_T_Enum ()
		{
			var enums = new NSStringEncoding [] { NSStringEncoding.ISOLatin1, NSStringEncoding.ISOLatin2 };
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetArrayValue_ (null, enums);
			}, "null key");

			dc.SetArrayValue_ (key, enums);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");
			Assert.That ((int) (dc.Dictionary [key] as NSArray).Count, Is.EqualTo (2), "2");

			enums = null;
			dc.SetArrayValue_ (key, enums);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetArrayValue_INativeObject ()
		{
			var native = new INativeObject [] { new CFString ("xamarin"), CFRunLoop.Main };
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetArrayValue_ (null, native);
			}, "null key");

			dc.SetArrayValue_ (key, native);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");
			Assert.That ((int) (dc.Dictionary [key] as NSArray).Count, Is.EqualTo (2), "2");

			native = null;
			dc.SetArrayValue_ (key, native);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetBooleanValue ()
		{
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetBooleanValue_ (null, true);
			}, "null key");

			dc.SetBooleanValue_ (key, true);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");

			dc.SetBooleanValue_ (key, null);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetNumberValue_Int32 ()
		{
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetNumberValue_ (null, int.MinValue);
			}, "null key");

			dc.SetNumberValue_ (key, int.MinValue);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");

			dc.SetNumberValue_ (key, (int?) null);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetNumberValue_UInt32 ()
		{
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetNumberValue_ (null, uint.MaxValue);
			}, "null key");

			dc.SetNumberValue_ (key, uint.MaxValue);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");

			dc.SetNumberValue_ (key, (uint?) null);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetNumberValue_Int64 ()
		{
			const long value = 2147483648L;
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetNumberValue_ (null, value);
			}, "null key");

			dc.SetNumberValue_ (key, value);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");
			Assert.That (((NSNumber) dc.Dictionary [key]).Int64Value, Is.EqualTo (value), "value");

			dc.SetNumberValue_ (key, (long?) null);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void SetStringValue ()
		{
			var dc = new DictionaryContainerPoker ();

			Assert.Throws<ArgumentNullException> (delegate
			{
				dc.SetStringValue_ (null, String.Empty);
			}, "null key");

			dc.SetStringValue_ (key, (NSString) "monkey");
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (1), "1");

			dc.SetStringValue_ (key, null);
			Assert.That ((int) dc.Dictionary.Count, Is.EqualTo (0), "0");
		}

		[Test]
		public void WrappedNSDictionary ()
		{
			var wrapped = new WrappedNSDictionary ();

			Assert.Multiple (() => {
				ClassicAssert.IsNull (wrapped.SByteField, "SByteField");
				var valueSByteField = SByte.MaxValue;
				wrapped.SByteField = valueSByteField;
				ClassicAssert.AreEqual (valueSByteField, wrapped.SByteField, "SByteField - set");
				wrapped.SByteField = null;
				ClassicAssert.IsNull (wrapped.SByteField, "SByteField - final");

				ClassicAssert.IsNull (wrapped.Int16Field, "Int16Field");
				var valueInt16Field = Int16.MaxValue;
				wrapped.Int16Field = valueInt16Field;
				ClassicAssert.AreEqual (valueInt16Field, wrapped.Int16Field, "Int16Field - set");
				wrapped.Int16Field = null;
				ClassicAssert.IsNull (wrapped.Int16Field, "Int16Field - final");

				ClassicAssert.IsNull (wrapped.Int32Field, "Int32Field");
				var valueInt32Field = Int32.MaxValue;
				wrapped.Int32Field = valueInt32Field;
				ClassicAssert.AreEqual (valueInt32Field, wrapped.Int32Field, "Int32Field - set");
				wrapped.Int32Field = null;
				ClassicAssert.IsNull (wrapped.Int32Field, "Int32Field - final");

				ClassicAssert.IsNull (wrapped.Int64Field, "Int64Field");
				var valueInt64Field = Int64.MaxValue;
				wrapped.Int64Field = valueInt64Field;
				ClassicAssert.AreEqual (valueInt64Field, wrapped.Int64Field, "Int64Field - set");
				wrapped.Int64Field = null;
				ClassicAssert.IsNull (wrapped.Int64Field, "Int64Field - final");

				ClassicAssert.IsNull (wrapped.ByteField, "ByteField");
				var valueByteField = Byte.MaxValue;
				wrapped.ByteField = valueByteField;
				ClassicAssert.AreEqual (valueByteField, wrapped.ByteField, "ByteField - set");
				wrapped.ByteField = null;
				ClassicAssert.IsNull (wrapped.ByteField, "ByteField - final");

				ClassicAssert.IsNull (wrapped.UInt16Field, "UInt16Field");
				var valueUInt16Field = UInt16.MaxValue;
				wrapped.UInt16Field = valueUInt16Field;
				ClassicAssert.AreEqual (valueUInt16Field, wrapped.UInt16Field, "UInt16Field - set");
				wrapped.UInt16Field = null;
				ClassicAssert.IsNull (wrapped.UInt16Field, "UInt16Field - final");

				ClassicAssert.IsNull (wrapped.UInt32Field, "UInt32Field");
				var valueUInt32Field = UInt32.MaxValue;
				wrapped.UInt32Field = valueUInt32Field;
				ClassicAssert.AreEqual (valueUInt32Field, wrapped.UInt32Field, "UInt32Field - set");
				wrapped.UInt32Field = null;
				ClassicAssert.IsNull (wrapped.UInt32Field, "UInt32Field - final");

				ClassicAssert.IsNull (wrapped.UInt64Field, "UInt64Field");
				var valueUInt64Field = UInt64.MaxValue;
				wrapped.UInt64Field = valueUInt64Field;
				ClassicAssert.AreEqual (valueUInt64Field, wrapped.UInt64Field, "UInt64Field - set");
				wrapped.UInt64Field = null;
				ClassicAssert.IsNull (wrapped.UInt64Field, "UInt64Field - final");

				ClassicAssert.IsNull (wrapped.NIntField, "NIntField");
				var valueNIntField = nint.MaxValue;
				wrapped.NIntField = valueNIntField;
				ClassicAssert.AreEqual (valueNIntField, wrapped.NIntField, "NIntField - set");
				wrapped.NIntField = null;
				ClassicAssert.IsNull (wrapped.NIntField, "NIntField - final");

				ClassicAssert.IsNull (wrapped.NUIntField, "NUIntField");
				var valueNUIntField = nuint.MaxValue;
				wrapped.NUIntField = valueNUIntField;
				ClassicAssert.AreEqual (valueNUIntField, wrapped.NUIntField, "NUIntField - set");
				wrapped.NUIntField = null;
				ClassicAssert.IsNull (wrapped.NUIntField, "NUIntField - final");

				ClassicAssert.IsNull (wrapped.SingleField, "SingleField");
				var valueSingleField = Single.MaxValue;
				wrapped.SingleField = valueSingleField;
				ClassicAssert.AreEqual (valueSingleField, wrapped.SingleField, "SingleField - set");
				wrapped.SingleField = null;
				ClassicAssert.IsNull (wrapped.SingleField, "SingleField - final");

				ClassicAssert.IsNull (wrapped.DoubleField, "DoubleField");
				var valueDoubleField = Double.MaxValue;
				wrapped.DoubleField = valueDoubleField;
				ClassicAssert.AreEqual (valueDoubleField, wrapped.DoubleField, "DoubleField - set");
				wrapped.DoubleField = null;
				ClassicAssert.IsNull (wrapped.DoubleField, "DoubleField - final");

				ClassicAssert.IsNull (wrapped.NFloatField, "NFloatField");
				var valueNFloatField = nfloat.MaxValue;
				wrapped.NFloatField = valueNFloatField;
				ClassicAssert.AreEqual (valueNFloatField, wrapped.NFloatField, "NFloatField - set");
				wrapped.NFloatField = null;
				ClassicAssert.IsNull (wrapped.NFloatField, "NFloatField - final");

				ClassicAssert.IsNull (wrapped.NSObjectField, "NSObjectField");
				var valueNSObjectField = (NSString) "NSObjectValue";
				wrapped.NSObjectField = valueNSObjectField;
				ClassicAssert.AreEqual (valueNSObjectField, wrapped.NSObjectField, "NSObjectField - set");
				wrapped.NSObjectField = null;
				ClassicAssert.IsNull (wrapped.NSObjectField, "NSObjectField - final");

				ClassicAssert.IsNull (wrapped.BooleanField, "BooleanField");
				var valueBooleanField = true;
				wrapped.BooleanField = valueBooleanField;
				ClassicAssert.AreEqual (valueBooleanField, wrapped.BooleanField, "BooleanField - set");
				wrapped.BooleanField = null;
				ClassicAssert.IsNull (wrapped.BooleanField, "BooleanField - final");

				ClassicAssert.IsNull (wrapped.NSStringField, "NSStringField");
				var valueNSStringField = (NSString) "NSStringValue";
				wrapped.NSStringField = valueNSStringField;
				ClassicAssert.AreEqual (valueNSStringField, wrapped.NSStringField, "NSStringField - set");
				wrapped.NSStringField = null;
				ClassicAssert.IsNull (wrapped.NSStringField, "NSStringField - final");

				ClassicAssert.IsNull (wrapped.NSDateField, "NSDateField");
				var valueNSDateField = (NSDate) new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.NSDateField = valueNSDateField;
				ClassicAssert.AreEqual (valueNSDateField, wrapped.NSDateField, "NSDateField - set");
				wrapped.NSDateField = null;
				ClassicAssert.IsNull (wrapped.NSDateField, "NSDateField - final");

				ClassicAssert.IsNull (wrapped.NSDictionaryField, "NSDictionaryField");
				var valueNSDictionaryField = new NSDictionary ();
				wrapped.NSDictionaryField = valueNSDictionaryField;
				ClassicAssert.AreEqual (valueNSDictionaryField, wrapped.NSDictionaryField, "NSDictionaryField - set");
				wrapped.NSDictionaryField = null;
				ClassicAssert.IsNull (wrapped.NSDictionaryField, "NSDictionaryField - final");

				ClassicAssert.IsNull (wrapped.NSStrongDictionaryField, "NSStrongDictionaryField");
				var valueNSStrongDictionaryField = new WrappedNSDictionary ();
				wrapped.NSStrongDictionaryField = valueNSStrongDictionaryField;
				ClassicAssert.AreEqual (valueNSStrongDictionaryField?.Dictionary?.ToString (), wrapped.NSStrongDictionaryField?.Dictionary?.ToString (), "NSStrongDictionaryField - set");
				wrapped.NSStrongDictionaryField = null;
				ClassicAssert.IsNull (wrapped.NSStrongDictionaryField, "NSStrongDictionaryField - final");

				ClassicAssert.IsNull (wrapped.StrongEnumField, "StrongEnumField");
				var valueStrongEnumField = StrongEnum.C;
				wrapped.StrongEnumField = valueStrongEnumField;
				ClassicAssert.AreEqual (valueStrongEnumField, wrapped.StrongEnumField, "StrongEnumField - set");
				wrapped.StrongEnumField = null;
				ClassicAssert.IsNull (wrapped.StrongEnumField, "StrongEnumField - final");

				ClassicAssert.IsNull (wrapped.NormalEnumField, "NormalEnumField");
				var valueNormalEnumField = NormalEnum.Z;
				wrapped.NormalEnumField = valueNormalEnumField;
				ClassicAssert.AreEqual (valueNormalEnumField, wrapped.NormalEnumField, "NormalEnumField - set");
				wrapped.NormalEnumField = null;
				ClassicAssert.IsNull (wrapped.NormalEnumField, "NormalEnumField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfSByteField, "ArrayOfSByteField");
				var valueArrayOfSByteField = new sbyte [] { 1, 2, 3 };
				wrapped.ArrayOfSByteField = valueArrayOfSByteField;
				ClassicAssert.AreEqual (valueArrayOfSByteField, wrapped.ArrayOfSByteField, "ArrayOfSByteField - set");
				wrapped.ArrayOfSByteField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfSByteField, "ArrayOfSByteField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfInt16Field, "ArrayOfInt16Field");
				var valueArrayOfInt16Field = new short [] { 1, 2, 3 };
				wrapped.ArrayOfInt16Field = valueArrayOfInt16Field;
				ClassicAssert.AreEqual (valueArrayOfInt16Field, wrapped.ArrayOfInt16Field, "ArrayOfInt16Field - set");
				wrapped.ArrayOfInt16Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfInt16Field, "ArrayOfInt16Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfInt32Field, "ArrayOfInt32Field");
				var valueArrayOfInt32Field = new int [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt32Field = valueArrayOfInt32Field;
				ClassicAssert.AreEqual (valueArrayOfInt32Field, wrapped.ArrayOfInt32Field, "ArrayOfInt32Field - set");
				wrapped.ArrayOfInt32Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfInt32Field, "ArrayOfInt32Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfInt64Field, "ArrayOfInt64Field");
				var valueArrayOfInt64Field = new long [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt64Field = valueArrayOfInt64Field;
				ClassicAssert.AreEqual (valueArrayOfInt64Field, wrapped.ArrayOfInt64Field, "ArrayOfInt64Field - set");
				wrapped.ArrayOfInt64Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfInt64Field, "ArrayOfInt64Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfByteField, "ArrayOfByteField");
				var valueArrayOfByteField = new byte [] { 1, 2, 3 }; ;
				wrapped.ArrayOfByteField = valueArrayOfByteField;
				ClassicAssert.AreEqual (valueArrayOfByteField, wrapped.ArrayOfByteField, "ArrayOfByteField - set");
				wrapped.ArrayOfByteField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfByteField, "ArrayOfByteField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field");
				var valueArrayOfUInt16Field = new ushort [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt16Field = valueArrayOfUInt16Field;
				ClassicAssert.AreEqual (valueArrayOfUInt16Field, wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field - set");
				wrapped.ArrayOfUInt16Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field");
				var valueArrayOfUInt32Field = new uint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt32Field = valueArrayOfUInt32Field;
				ClassicAssert.AreEqual (valueArrayOfUInt32Field, wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field - set");
				wrapped.ArrayOfUInt32Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field");
				var valueArrayOfUInt64Field = new ulong [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt64Field = valueArrayOfUInt64Field;
				ClassicAssert.AreEqual (valueArrayOfUInt64Field, wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field - set");
				wrapped.ArrayOfUInt64Field = null;
				ClassicAssert.IsNull (wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNIntField, "ArrayOfNIntField");
				var valueArrayOfNIntField = new nint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNIntField = valueArrayOfNIntField;
				ClassicAssert.AreEqual (valueArrayOfNIntField, wrapped.ArrayOfNIntField, "ArrayOfNIntField - set");
				wrapped.ArrayOfNIntField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNIntField, "ArrayOfNIntField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNUIntField, "ArrayOfNUIntField");
				var valueArrayOfNUIntField = new nuint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNUIntField = valueArrayOfNUIntField;
				ClassicAssert.AreEqual (valueArrayOfNUIntField, wrapped.ArrayOfNUIntField, "ArrayOfNUIntField - set");
				wrapped.ArrayOfNUIntField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNUIntField, "ArrayOfNUIntField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfSingleField, "ArrayOfSingleField");
				var valueArrayOfSingleField = new float [] { 1, 2, 3 }; ;
				wrapped.ArrayOfSingleField = valueArrayOfSingleField;
				ClassicAssert.AreEqual (valueArrayOfSingleField, wrapped.ArrayOfSingleField, "ArrayOfSingleField - set");
				wrapped.ArrayOfSingleField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfSingleField, "ArrayOfSingleField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfDoubleField, "ArrayOfDoubleField");
				var valueArrayOfDoubleField = new double [] { 1, 2, 3 }; ;
				wrapped.ArrayOfDoubleField = valueArrayOfDoubleField;
				ClassicAssert.AreEqual (valueArrayOfDoubleField, wrapped.ArrayOfDoubleField, "ArrayOfDoubleField - set");
				wrapped.ArrayOfDoubleField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfDoubleField, "ArrayOfDoubleField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNFloatField, "ArrayOfNFloatField");
				var valueArrayOfNFloatField = new nfloat [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNFloatField = valueArrayOfNFloatField;
				ClassicAssert.AreEqual (valueArrayOfNFloatField, wrapped.ArrayOfNFloatField, "ArrayOfNFloatField - set");
				wrapped.ArrayOfNFloatField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNFloatField, "ArrayOfNFloatField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField");
				var valueArrayOfNSObjectField = new NSObject [] { (NSString) "Array1", NSDate.Now };
				wrapped.ArrayOfNSObjectField = valueArrayOfNSObjectField;
				ClassicAssert.AreEqual (valueArrayOfNSObjectField, wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField - set");
				wrapped.ArrayOfNSObjectField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfBooleanField, "ArrayOfBooleanField");
				var valueArrayOfBooleanField = new bool [] { true, false, true };
				wrapped.ArrayOfBooleanField = valueArrayOfBooleanField;
				ClassicAssert.AreEqual (valueArrayOfBooleanField, wrapped.ArrayOfBooleanField, "ArrayOfBooleanField - set");
				wrapped.ArrayOfBooleanField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfBooleanField, "ArrayOfBooleanField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNSStringField, "ArrayOfNSStringField");
				var valueArrayOfNSStringField = new NSString [] { (NSString) "a", (NSString) "b", (NSString) "c" };
				wrapped.ArrayOfNSStringField = valueArrayOfNSStringField;
				ClassicAssert.AreEqual (valueArrayOfNSStringField, wrapped.ArrayOfNSStringField, "ArrayOfNSStringField - set");
				wrapped.ArrayOfNSStringField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNSStringField, "ArrayOfNSStringField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNSDateField, "ArrayOfNSDateField");
				var valueArrayOfNSDateField = new NSDate [] { NSDate.Now, NSDate.Now };
				wrapped.ArrayOfNSDateField = valueArrayOfNSDateField;
				ClassicAssert.AreEqual (valueArrayOfNSDateField, wrapped.ArrayOfNSDateField, "ArrayOfNSDateField - set");
				wrapped.ArrayOfNSDateField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNSDateField, "ArrayOfNSDateField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField");
				var valueArrayOfNSDictionaryField = new NSDictionary [] { new NSDictionary (), new NSDictionary () };
				wrapped.ArrayOfNSDictionaryField = valueArrayOfNSDictionaryField;
				ClassicAssert.AreEqual (valueArrayOfNSDictionaryField, wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField - set");
				wrapped.ArrayOfNSDictionaryField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfStrongDictionaryField, "ArrayOfStrongDictionaryField");
				var valueArrayOfStrongDictionaryField = new WrappedNSDictionary [] { new WrappedNSDictionary (), new WrappedNSDictionary () };
				wrapped.ArrayOfStrongDictionaryField = valueArrayOfStrongDictionaryField;
				ClassicAssert.AreEqual (string.Join (";", valueArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ())), string.Join (";", wrapped.ArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ())), "ArrayOfStrongDictionaryField - set");
				wrapped.ArrayOfStrongDictionaryField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfStrongDictionaryField, "ArrayOfStrongDictionaryField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField");
				var valueArrayOfStrongEnumField = new StrongEnum [] { StrongEnum.A, StrongEnum.B };
				wrapped.ArrayOfStrongEnumField = valueArrayOfStrongEnumField;
				ClassicAssert.AreEqual (valueArrayOfStrongEnumField, wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField - set");
				wrapped.ArrayOfStrongEnumField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField");
				var valueArrayOfNormalEnumField = new NormalEnum [] { NormalEnum.X, NormalEnum.Y };
				wrapped.ArrayOfNormalEnumField = valueArrayOfNormalEnumField;
				ClassicAssert.AreEqual (valueArrayOfNormalEnumField, wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField - set");
				wrapped.ArrayOfNormalEnumField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField - final");

				ClassicAssert.IsNull (wrapped.StringField, "StringField");
				var valueStringField = "managed string";
				wrapped.StringField = valueStringField;
				ClassicAssert.AreEqual (valueStringField, wrapped.StringField, "StringField - set");
				wrapped.StringField = null;
				ClassicAssert.IsNull (wrapped.StringField, "StringField - final");

				ClassicAssert.IsNull (wrapped.DateTimeField, "DateTimeField");
				var valueDateTimeField = new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.DateTimeField = valueDateTimeField;
				ClassicAssert.AreEqual (valueDateTimeField, wrapped.DateTimeField, "DateTimeField - set");
				wrapped.DateTimeField = null;
				ClassicAssert.IsNull (wrapped.DateTimeField, "DateTimeField - final");

				ClassicAssert.IsNull (wrapped.GenericNSDictionaryField, "GenericNSDictionaryField");
				var valueGenericNSDictionaryField = new NSDictionary<NSString, NSObject> ();
				wrapped.GenericNSDictionaryField = valueGenericNSDictionaryField;
				ClassicAssert.AreEqual (valueGenericNSDictionaryField, wrapped.GenericNSDictionaryField, "GenericNSDictionaryField - set");
				wrapped.GenericNSDictionaryField = null;
				ClassicAssert.IsNull (wrapped.GenericNSDictionaryField, "GenericNSDictionaryField - final");

				ClassicAssert.IsNull (wrapped.ArrayOfStringField, "ArrayOfStringField");
				var valueArrayOfStringField = new string [] { "abc", "def", "ghi" };
				wrapped.ArrayOfStringField = valueArrayOfStringField;
				ClassicAssert.AreEqual (valueArrayOfStringField, wrapped.ArrayOfStringField, "ArrayOfStringField - set");
				wrapped.ArrayOfStringField = null;
				ClassicAssert.IsNull (wrapped.ArrayOfStringField, "ArrayOfStringField - final");

				ClassicAssert.IsNull (wrapped.NSDataField, "NSDataField");
				var valueNSDataField = NSData.FromArray (new byte [] { 1, 2, 3 });
				wrapped.NSDataField = valueNSDataField;
				ClassicAssert.AreEqual (valueNSDataField, wrapped.NSDataField, "NSDataField - set");
				wrapped.NSDataField = null;
				ClassicAssert.IsNull (wrapped.NSDataField, "NSDataField - final");

				ClassicAssert.IsNull (wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field");
				var valueNSDataAsMatrix3Field = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
				wrapped.NSDataAsMatrix3Field = valueNSDataAsMatrix3Field;
				ClassicAssert.AreEqual (valueNSDataAsMatrix3Field, wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field - set");
				wrapped.NSDataAsMatrix3Field = null;
				ClassicAssert.IsNull (wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field - final");

				ClassicAssert.IsNull (wrapped.CGRectField, "CGRectField");
				var valueCGRectField = new CGRect (1, 2, 3, 4);
				wrapped.CGRectField = valueCGRectField;
				ClassicAssert.AreEqual (valueCGRectField, wrapped.CGRectField, "CGRectField - set");
				wrapped.CGRectField = null;
				ClassicAssert.IsNull (wrapped.CGRectField, "CGRectField - final");

				ClassicAssert.IsNull (wrapped.CGSizeField, "CGSizeField");
				var valueCGSizeField = new CGSize (5, 6);
				wrapped.CGSizeField = valueCGSizeField;
				ClassicAssert.AreEqual (valueCGSizeField, wrapped.CGSizeField, "CGSizeField - set");
				wrapped.CGSizeField = null;
				ClassicAssert.IsNull (wrapped.CGSizeField, "CGSizeField - final");

				ClassicAssert.IsNull (wrapped.CGPointField, "CGPointField");
				var valueCGPointField = new CGPoint (7, 8);
				wrapped.CGPointField = valueCGPointField;
				ClassicAssert.AreEqual (valueCGPointField, wrapped.CGPointField, "CGPointField - set");
				wrapped.CGPointField = null;
				ClassicAssert.IsNull (wrapped.CGPointField, "CGPointField - final");

				ClassicAssert.IsNull (wrapped.CMTimeField, "CMTimeField");
				var valueCMTimeField = new CMTime (123, 2);
				wrapped.CMTimeField = valueCMTimeField;
				ClassicAssert.AreEqual (valueCMTimeField, wrapped.CMTimeField, "CMTimeField - set");
				wrapped.CMTimeField = null;
				ClassicAssert.IsNull (wrapped.CMTimeField, "CMTimeField - final");

#if HAS_UIKIT
				ClassicAssert.IsNull (wrapped.UIEdgeInsetsField, "UIEdgeInsetsField");
				var valueUIEdgeInsetsField = new UIEdgeInsets (9, 8, 7, 6);
				wrapped.UIEdgeInsetsField = valueUIEdgeInsetsField;
				ClassicAssert.AreEqual (valueUIEdgeInsetsField, wrapped.UIEdgeInsetsField, "UIEdgeInsetsField - set");
				wrapped.UIEdgeInsetsField = null;
				ClassicAssert.IsNull (wrapped.UIEdgeInsetsField, "UIEdgeInsetsField - final");
#endif // HAS_UIKIT
			});
		}

		[Test]
		public void Matrix ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.Multiple (() => {
				var dict = new VTCompressionPropertyCameraCalibration ();

				ClassicAssert.IsNull (dict.IntrinsicMatrix, "IntrinsicMatrix");

				var matrix = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
				dict.IntrinsicMatrix = matrix;
				ClassicAssert.AreEqual (matrix, dict.IntrinsicMatrix, "IntrinsicMatrix 2");

				dict.IntrinsicMatrix = null;
				ClassicAssert.IsNull (dict.IntrinsicMatrix, "IntrinsicMatrix 3");
			});
		}

		[Test]
		public void FloatArray ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.Multiple (() => {
				var dict = new VTCompressionPropertyCameraCalibration ();

				ClassicAssert.IsNull (dict.LensDistortions, "LensDistortions");

				var array = new float [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				dict.LensDistortions = array;
				ClassicAssert.AreEqual (array, dict.LensDistortions, "LensDistortions 2");

				dict.LensDistortions = null;
				ClassicAssert.IsNull (dict.LensDistortions, "LensDistortions 3");
			});
		}
	}
}
