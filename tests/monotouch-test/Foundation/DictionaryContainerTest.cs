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
				Assert.That (wrapped.SByteField, Is.Null, "SByteField");
				var valueSByteField = SByte.MaxValue;
				wrapped.SByteField = valueSByteField;
				Assert.That (wrapped.SByteField, Is.EqualTo (valueSByteField), "SByteField - set");
				wrapped.SByteField = null;
				Assert.That (wrapped.SByteField, Is.Null, "SByteField - final");

				Assert.That (wrapped.Int16Field, Is.Null, "Int16Field");
				var valueInt16Field = Int16.MaxValue;
				wrapped.Int16Field = valueInt16Field;
				Assert.That (wrapped.Int16Field, Is.EqualTo (valueInt16Field), "Int16Field - set");
				wrapped.Int16Field = null;
				Assert.That (wrapped.Int16Field, Is.Null, "Int16Field - final");

				Assert.That (wrapped.Int32Field, Is.Null, "Int32Field");
				var valueInt32Field = Int32.MaxValue;
				wrapped.Int32Field = valueInt32Field;
				Assert.That (wrapped.Int32Field, Is.EqualTo (valueInt32Field), "Int32Field - set");
				wrapped.Int32Field = null;
				Assert.That (wrapped.Int32Field, Is.Null, "Int32Field - final");

				Assert.That (wrapped.Int64Field, Is.Null, "Int64Field");
				var valueInt64Field = Int64.MaxValue;
				wrapped.Int64Field = valueInt64Field;
				Assert.That (wrapped.Int64Field, Is.EqualTo (valueInt64Field), "Int64Field - set");
				wrapped.Int64Field = null;
				Assert.That (wrapped.Int64Field, Is.Null, "Int64Field - final");

				Assert.That (wrapped.ByteField, Is.Null, "ByteField");
				var valueByteField = Byte.MaxValue;
				wrapped.ByteField = valueByteField;
				Assert.That (wrapped.ByteField, Is.EqualTo (valueByteField), "ByteField - set");
				wrapped.ByteField = null;
				Assert.That (wrapped.ByteField, Is.Null, "ByteField - final");

				Assert.That (wrapped.UInt16Field, Is.Null, "UInt16Field");
				var valueUInt16Field = UInt16.MaxValue;
				wrapped.UInt16Field = valueUInt16Field;
				Assert.That (wrapped.UInt16Field, Is.EqualTo (valueUInt16Field), "UInt16Field - set");
				wrapped.UInt16Field = null;
				Assert.That (wrapped.UInt16Field, Is.Null, "UInt16Field - final");

				Assert.That (wrapped.UInt32Field, Is.Null, "UInt32Field");
				var valueUInt32Field = UInt32.MaxValue;
				wrapped.UInt32Field = valueUInt32Field;
				Assert.That (wrapped.UInt32Field, Is.EqualTo (valueUInt32Field), "UInt32Field - set");
				wrapped.UInt32Field = null;
				Assert.That (wrapped.UInt32Field, Is.Null, "UInt32Field - final");

				Assert.That (wrapped.UInt64Field, Is.Null, "UInt64Field");
				var valueUInt64Field = UInt64.MaxValue;
				wrapped.UInt64Field = valueUInt64Field;
				Assert.That (wrapped.UInt64Field, Is.EqualTo (valueUInt64Field), "UInt64Field - set");
				wrapped.UInt64Field = null;
				Assert.That (wrapped.UInt64Field, Is.Null, "UInt64Field - final");

				Assert.That (wrapped.NIntField, Is.Null, "NIntField");
				var valueNIntField = nint.MaxValue;
				wrapped.NIntField = valueNIntField;
				Assert.That (wrapped.NIntField, Is.EqualTo (valueNIntField), "NIntField - set");
				wrapped.NIntField = null;
				Assert.That (wrapped.NIntField, Is.Null, "NIntField - final");

				Assert.That (wrapped.NUIntField, Is.Null, "NUIntField");
				var valueNUIntField = nuint.MaxValue;
				wrapped.NUIntField = valueNUIntField;
				Assert.That (wrapped.NUIntField, Is.EqualTo (valueNUIntField), "NUIntField - set");
				wrapped.NUIntField = null;
				Assert.That (wrapped.NUIntField, Is.Null, "NUIntField - final");

				Assert.That (wrapped.SingleField, Is.Null, "SingleField");
				var valueSingleField = Single.MaxValue;
				wrapped.SingleField = valueSingleField;
				Assert.That (wrapped.SingleField, Is.EqualTo (valueSingleField), "SingleField - set");
				wrapped.SingleField = null;
				Assert.That (wrapped.SingleField, Is.Null, "SingleField - final");

				Assert.That (wrapped.DoubleField, Is.Null, "DoubleField");
				var valueDoubleField = Double.MaxValue;
				wrapped.DoubleField = valueDoubleField;
				Assert.That (wrapped.DoubleField, Is.EqualTo (valueDoubleField), "DoubleField - set");
				wrapped.DoubleField = null;
				Assert.That (wrapped.DoubleField, Is.Null, "DoubleField - final");

				Assert.That (wrapped.NFloatField, Is.Null, "NFloatField");
				var valueNFloatField = nfloat.MaxValue;
				wrapped.NFloatField = valueNFloatField;
				Assert.That (wrapped.NFloatField, Is.EqualTo (valueNFloatField), "NFloatField - set");
				wrapped.NFloatField = null;
				Assert.That (wrapped.NFloatField, Is.Null, "NFloatField - final");

				Assert.That (wrapped.NSObjectField, Is.Null, "NSObjectField");
				var valueNSObjectField = (NSString) "NSObjectValue";
				wrapped.NSObjectField = valueNSObjectField;
				Assert.That (wrapped.NSObjectField, Is.EqualTo (valueNSObjectField), "NSObjectField - set");
				wrapped.NSObjectField = null;
				Assert.That (wrapped.NSObjectField, Is.Null, "NSObjectField - final");

				Assert.That (wrapped.BooleanField, Is.Null, "BooleanField");
				var valueBooleanField = true;
				wrapped.BooleanField = valueBooleanField;
				Assert.That (wrapped.BooleanField, Is.EqualTo (valueBooleanField), "BooleanField - set");
				wrapped.BooleanField = null;
				Assert.That (wrapped.BooleanField, Is.Null, "BooleanField - final");

				Assert.That (wrapped.NSStringField, Is.Null, "NSStringField");
				var valueNSStringField = (NSString) "NSStringValue";
				wrapped.NSStringField = valueNSStringField;
				Assert.That (wrapped.NSStringField, Is.EqualTo (valueNSStringField), "NSStringField - set");
				wrapped.NSStringField = null;
				Assert.That (wrapped.NSStringField, Is.Null, "NSStringField - final");

				Assert.That (wrapped.NSDateField, Is.Null, "NSDateField");
				var valueNSDateField = (NSDate) new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.NSDateField = valueNSDateField;
				Assert.That (wrapped.NSDateField, Is.EqualTo (valueNSDateField), "NSDateField - set");
				wrapped.NSDateField = null;
				Assert.That (wrapped.NSDateField, Is.Null, "NSDateField - final");

				Assert.That (wrapped.NSDictionaryField, Is.Null, "NSDictionaryField");
				var valueNSDictionaryField = new NSDictionary ();
				wrapped.NSDictionaryField = valueNSDictionaryField;
				Assert.That (wrapped.NSDictionaryField, Is.EqualTo (valueNSDictionaryField), "NSDictionaryField - set");
				wrapped.NSDictionaryField = null;
				Assert.That (wrapped.NSDictionaryField, Is.Null, "NSDictionaryField - final");

				Assert.That (wrapped.NSStrongDictionaryField, Is.Null, "NSStrongDictionaryField");
				var valueNSStrongDictionaryField = new WrappedNSDictionary ();
				wrapped.NSStrongDictionaryField = valueNSStrongDictionaryField;
				Assert.That (wrapped.NSStrongDictionaryField?.Dictionary?.ToString (), Is.EqualTo (valueNSStrongDictionaryField?.Dictionary?.ToString ()), "NSStrongDictionaryField - set");
				wrapped.NSStrongDictionaryField = null;
				Assert.That (wrapped.NSStrongDictionaryField, Is.Null, "NSStrongDictionaryField - final");

				Assert.That (wrapped.StrongEnumField, Is.Null, "StrongEnumField");
				var valueStrongEnumField = StrongEnum.C;
				wrapped.StrongEnumField = valueStrongEnumField;
				Assert.That (wrapped.StrongEnumField, Is.EqualTo (valueStrongEnumField), "StrongEnumField - set");
				wrapped.StrongEnumField = null;
				Assert.That (wrapped.StrongEnumField, Is.Null, "StrongEnumField - final");

				Assert.That (wrapped.NormalEnumField, Is.Null, "NormalEnumField");
				var valueNormalEnumField = NormalEnum.Z;
				wrapped.NormalEnumField = valueNormalEnumField;
				Assert.That (wrapped.NormalEnumField, Is.EqualTo (valueNormalEnumField), "NormalEnumField - set");
				wrapped.NormalEnumField = null;
				Assert.That (wrapped.NormalEnumField, Is.Null, "NormalEnumField - final");

				Assert.That (wrapped.ArrayOfSByteField, Is.Null, "ArrayOfSByteField");
				var valueArrayOfSByteField = new sbyte [] { 1, 2, 3 };
				wrapped.ArrayOfSByteField = valueArrayOfSByteField;
				Assert.That (wrapped.ArrayOfSByteField, Is.EqualTo (valueArrayOfSByteField), "ArrayOfSByteField - set");
				wrapped.ArrayOfSByteField = null;
				Assert.That (wrapped.ArrayOfSByteField, Is.Null, "ArrayOfSByteField - final");

				Assert.That (wrapped.ArrayOfInt16Field, Is.Null, "ArrayOfInt16Field");
				var valueArrayOfInt16Field = new short [] { 1, 2, 3 };
				wrapped.ArrayOfInt16Field = valueArrayOfInt16Field;
				Assert.That (wrapped.ArrayOfInt16Field, Is.EqualTo (valueArrayOfInt16Field), "ArrayOfInt16Field - set");
				wrapped.ArrayOfInt16Field = null;
				Assert.That (wrapped.ArrayOfInt16Field, Is.Null, "ArrayOfInt16Field - final");

				Assert.That (wrapped.ArrayOfInt32Field, Is.Null, "ArrayOfInt32Field");
				var valueArrayOfInt32Field = new int [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt32Field = valueArrayOfInt32Field;
				Assert.That (wrapped.ArrayOfInt32Field, Is.EqualTo (valueArrayOfInt32Field), "ArrayOfInt32Field - set");
				wrapped.ArrayOfInt32Field = null;
				Assert.That (wrapped.ArrayOfInt32Field, Is.Null, "ArrayOfInt32Field - final");

				Assert.That (wrapped.ArrayOfInt64Field, Is.Null, "ArrayOfInt64Field");
				var valueArrayOfInt64Field = new long [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt64Field = valueArrayOfInt64Field;
				Assert.That (wrapped.ArrayOfInt64Field, Is.EqualTo (valueArrayOfInt64Field), "ArrayOfInt64Field - set");
				wrapped.ArrayOfInt64Field = null;
				Assert.That (wrapped.ArrayOfInt64Field, Is.Null, "ArrayOfInt64Field - final");

				Assert.That (wrapped.ArrayOfByteField, Is.Null, "ArrayOfByteField");
				var valueArrayOfByteField = new byte [] { 1, 2, 3 }; ;
				wrapped.ArrayOfByteField = valueArrayOfByteField;
				Assert.That (wrapped.ArrayOfByteField, Is.EqualTo (valueArrayOfByteField), "ArrayOfByteField - set");
				wrapped.ArrayOfByteField = null;
				Assert.That (wrapped.ArrayOfByteField, Is.Null, "ArrayOfByteField - final");

				Assert.That (wrapped.ArrayOfUInt16Field, Is.Null, "ArrayOfUInt16Field");
				var valueArrayOfUInt16Field = new ushort [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt16Field = valueArrayOfUInt16Field;
				Assert.That (wrapped.ArrayOfUInt16Field, Is.EqualTo (valueArrayOfUInt16Field), "ArrayOfUInt16Field - set");
				wrapped.ArrayOfUInt16Field = null;
				Assert.That (wrapped.ArrayOfUInt16Field, Is.Null, "ArrayOfUInt16Field - final");

				Assert.That (wrapped.ArrayOfUInt32Field, Is.Null, "ArrayOfUInt32Field");
				var valueArrayOfUInt32Field = new uint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt32Field = valueArrayOfUInt32Field;
				Assert.That (wrapped.ArrayOfUInt32Field, Is.EqualTo (valueArrayOfUInt32Field), "ArrayOfUInt32Field - set");
				wrapped.ArrayOfUInt32Field = null;
				Assert.That (wrapped.ArrayOfUInt32Field, Is.Null, "ArrayOfUInt32Field - final");

				Assert.That (wrapped.ArrayOfUInt64Field, Is.Null, "ArrayOfUInt64Field");
				var valueArrayOfUInt64Field = new ulong [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt64Field = valueArrayOfUInt64Field;
				Assert.That (wrapped.ArrayOfUInt64Field, Is.EqualTo (valueArrayOfUInt64Field), "ArrayOfUInt64Field - set");
				wrapped.ArrayOfUInt64Field = null;
				Assert.That (wrapped.ArrayOfUInt64Field, Is.Null, "ArrayOfUInt64Field - final");

				Assert.That (wrapped.ArrayOfNIntField, Is.Null, "ArrayOfNIntField");
				var valueArrayOfNIntField = new nint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNIntField = valueArrayOfNIntField;
				Assert.That (wrapped.ArrayOfNIntField, Is.EqualTo (valueArrayOfNIntField), "ArrayOfNIntField - set");
				wrapped.ArrayOfNIntField = null;
				Assert.That (wrapped.ArrayOfNIntField, Is.Null, "ArrayOfNIntField - final");

				Assert.That (wrapped.ArrayOfNUIntField, Is.Null, "ArrayOfNUIntField");
				var valueArrayOfNUIntField = new nuint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNUIntField = valueArrayOfNUIntField;
				Assert.That (wrapped.ArrayOfNUIntField, Is.EqualTo (valueArrayOfNUIntField), "ArrayOfNUIntField - set");
				wrapped.ArrayOfNUIntField = null;
				Assert.That (wrapped.ArrayOfNUIntField, Is.Null, "ArrayOfNUIntField - final");

				Assert.That (wrapped.ArrayOfSingleField, Is.Null, "ArrayOfSingleField");
				var valueArrayOfSingleField = new float [] { 1, 2, 3 }; ;
				wrapped.ArrayOfSingleField = valueArrayOfSingleField;
				Assert.That (wrapped.ArrayOfSingleField, Is.EqualTo (valueArrayOfSingleField), "ArrayOfSingleField - set");
				wrapped.ArrayOfSingleField = null;
				Assert.That (wrapped.ArrayOfSingleField, Is.Null, "ArrayOfSingleField - final");

				Assert.That (wrapped.ArrayOfDoubleField, Is.Null, "ArrayOfDoubleField");
				var valueArrayOfDoubleField = new double [] { 1, 2, 3 }; ;
				wrapped.ArrayOfDoubleField = valueArrayOfDoubleField;
				Assert.That (wrapped.ArrayOfDoubleField, Is.EqualTo (valueArrayOfDoubleField), "ArrayOfDoubleField - set");
				wrapped.ArrayOfDoubleField = null;
				Assert.That (wrapped.ArrayOfDoubleField, Is.Null, "ArrayOfDoubleField - final");

				Assert.That (wrapped.ArrayOfNFloatField, Is.Null, "ArrayOfNFloatField");
				var valueArrayOfNFloatField = new nfloat [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNFloatField = valueArrayOfNFloatField;
				Assert.That (wrapped.ArrayOfNFloatField, Is.EqualTo (valueArrayOfNFloatField), "ArrayOfNFloatField - set");
				wrapped.ArrayOfNFloatField = null;
				Assert.That (wrapped.ArrayOfNFloatField, Is.Null, "ArrayOfNFloatField - final");

				Assert.That (wrapped.ArrayOfNSObjectField, Is.Null, "ArrayOfNSObjectField");
				var valueArrayOfNSObjectField = new NSObject [] { (NSString) "Array1", NSDate.Now };
				wrapped.ArrayOfNSObjectField = valueArrayOfNSObjectField;
				Assert.That (wrapped.ArrayOfNSObjectField, Is.EqualTo (valueArrayOfNSObjectField), "ArrayOfNSObjectField - set");
				wrapped.ArrayOfNSObjectField = null;
				Assert.That (wrapped.ArrayOfNSObjectField, Is.Null, "ArrayOfNSObjectField - final");

				Assert.That (wrapped.ArrayOfBooleanField, Is.Null, "ArrayOfBooleanField");
				var valueArrayOfBooleanField = new bool [] { true, false, true };
				wrapped.ArrayOfBooleanField = valueArrayOfBooleanField;
				Assert.That (wrapped.ArrayOfBooleanField, Is.EqualTo (valueArrayOfBooleanField), "ArrayOfBooleanField - set");
				wrapped.ArrayOfBooleanField = null;
				Assert.That (wrapped.ArrayOfBooleanField, Is.Null, "ArrayOfBooleanField - final");

				Assert.That (wrapped.ArrayOfNSStringField, Is.Null, "ArrayOfNSStringField");
				var valueArrayOfNSStringField = new NSString [] { (NSString) "a", (NSString) "b", (NSString) "c" };
				wrapped.ArrayOfNSStringField = valueArrayOfNSStringField;
				Assert.That (wrapped.ArrayOfNSStringField, Is.EqualTo (valueArrayOfNSStringField), "ArrayOfNSStringField - set");
				wrapped.ArrayOfNSStringField = null;
				Assert.That (wrapped.ArrayOfNSStringField, Is.Null, "ArrayOfNSStringField - final");

				Assert.That (wrapped.ArrayOfNSDateField, Is.Null, "ArrayOfNSDateField");
				var valueArrayOfNSDateField = new NSDate [] { NSDate.Now, NSDate.Now };
				wrapped.ArrayOfNSDateField = valueArrayOfNSDateField;
				Assert.That (wrapped.ArrayOfNSDateField, Is.EqualTo (valueArrayOfNSDateField), "ArrayOfNSDateField - set");
				wrapped.ArrayOfNSDateField = null;
				Assert.That (wrapped.ArrayOfNSDateField, Is.Null, "ArrayOfNSDateField - final");

				Assert.That (wrapped.ArrayOfNSDictionaryField, Is.Null, "ArrayOfNSDictionaryField");
				var valueArrayOfNSDictionaryField = new NSDictionary [] { new NSDictionary (), new NSDictionary () };
				wrapped.ArrayOfNSDictionaryField = valueArrayOfNSDictionaryField;
				Assert.That (wrapped.ArrayOfNSDictionaryField, Is.EqualTo (valueArrayOfNSDictionaryField), "ArrayOfNSDictionaryField - set");
				wrapped.ArrayOfNSDictionaryField = null;
				Assert.That (wrapped.ArrayOfNSDictionaryField, Is.Null, "ArrayOfNSDictionaryField - final");

				Assert.That (wrapped.ArrayOfStrongDictionaryField, Is.Null, "ArrayOfStrongDictionaryField");
				var valueArrayOfStrongDictionaryField = new WrappedNSDictionary [] { new WrappedNSDictionary (), new WrappedNSDictionary () };
				wrapped.ArrayOfStrongDictionaryField = valueArrayOfStrongDictionaryField;
				Assert.That (string.Join (";", wrapped.ArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ())), Is.EqualTo (string.Join (";", valueArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ()))), "ArrayOfStrongDictionaryField - set");
				wrapped.ArrayOfStrongDictionaryField = null;
				Assert.That (wrapped.ArrayOfStrongDictionaryField, Is.Null, "ArrayOfStrongDictionaryField - final");

				Assert.That (wrapped.ArrayOfStrongEnumField, Is.Null, "ArrayOfStrongEnumField");
				var valueArrayOfStrongEnumField = new StrongEnum [] { StrongEnum.A, StrongEnum.B };
				wrapped.ArrayOfStrongEnumField = valueArrayOfStrongEnumField;
				Assert.That (wrapped.ArrayOfStrongEnumField, Is.EqualTo (valueArrayOfStrongEnumField), "ArrayOfStrongEnumField - set");
				wrapped.ArrayOfStrongEnumField = null;
				Assert.That (wrapped.ArrayOfStrongEnumField, Is.Null, "ArrayOfStrongEnumField - final");

				Assert.That (wrapped.ArrayOfNormalEnumField, Is.Null, "ArrayOfNormalEnumField");
				var valueArrayOfNormalEnumField = new NormalEnum [] { NormalEnum.X, NormalEnum.Y };
				wrapped.ArrayOfNormalEnumField = valueArrayOfNormalEnumField;
				Assert.That (wrapped.ArrayOfNormalEnumField, Is.EqualTo (valueArrayOfNormalEnumField), "ArrayOfNormalEnumField - set");
				wrapped.ArrayOfNormalEnumField = null;
				Assert.That (wrapped.ArrayOfNormalEnumField, Is.Null, "ArrayOfNormalEnumField - final");

				Assert.That (wrapped.StringField, Is.Null, "StringField");
				var valueStringField = "managed string";
				wrapped.StringField = valueStringField;
				Assert.That (wrapped.StringField, Is.EqualTo (valueStringField), "StringField - set");
				wrapped.StringField = null;
				Assert.That (wrapped.StringField, Is.Null, "StringField - final");

				Assert.That (wrapped.DateTimeField, Is.Null, "DateTimeField");
				var valueDateTimeField = new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.DateTimeField = valueDateTimeField;
				Assert.That (wrapped.DateTimeField, Is.EqualTo (valueDateTimeField), "DateTimeField - set");
				wrapped.DateTimeField = null;
				Assert.That (wrapped.DateTimeField, Is.Null, "DateTimeField - final");

				Assert.That (wrapped.GenericNSDictionaryField, Is.Null, "GenericNSDictionaryField");
				var valueGenericNSDictionaryField = new NSDictionary<NSString, NSObject> ();
				wrapped.GenericNSDictionaryField = valueGenericNSDictionaryField;
				Assert.That (wrapped.GenericNSDictionaryField, Is.EqualTo (valueGenericNSDictionaryField), "GenericNSDictionaryField - set");
				wrapped.GenericNSDictionaryField = null;
				Assert.That (wrapped.GenericNSDictionaryField, Is.Null, "GenericNSDictionaryField - final");

				Assert.That (wrapped.ArrayOfStringField, Is.Null, "ArrayOfStringField");
				var valueArrayOfStringField = new string [] { "abc", "def", "ghi" };
				wrapped.ArrayOfStringField = valueArrayOfStringField;
				Assert.That (wrapped.ArrayOfStringField, Is.EqualTo (valueArrayOfStringField), "ArrayOfStringField - set");
				wrapped.ArrayOfStringField = null;
				Assert.That (wrapped.ArrayOfStringField, Is.Null, "ArrayOfStringField - final");

				Assert.That (wrapped.NSDataField, Is.Null, "NSDataField");
				var valueNSDataField = NSData.FromArray (new byte [] { 1, 2, 3 });
				wrapped.NSDataField = valueNSDataField;
				Assert.That (wrapped.NSDataField, Is.EqualTo (valueNSDataField), "NSDataField - set");
				wrapped.NSDataField = null;
				Assert.That (wrapped.NSDataField, Is.Null, "NSDataField - final");

				Assert.That (wrapped.NSDataAsMatrix3Field, Is.Null, "NSDataAsMatrix3Field");
				var valueNSDataAsMatrix3Field = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
				wrapped.NSDataAsMatrix3Field = valueNSDataAsMatrix3Field;
				Assert.That (wrapped.NSDataAsMatrix3Field, Is.EqualTo (valueNSDataAsMatrix3Field), "NSDataAsMatrix3Field - set");
				wrapped.NSDataAsMatrix3Field = null;
				Assert.That (wrapped.NSDataAsMatrix3Field, Is.Null, "NSDataAsMatrix3Field - final");

				Assert.That (wrapped.CGRectField, Is.Null, "CGRectField");
				var valueCGRectField = new CGRect (1, 2, 3, 4);
				wrapped.CGRectField = valueCGRectField;
				Assert.That (wrapped.CGRectField, Is.EqualTo (valueCGRectField), "CGRectField - set");
				wrapped.CGRectField = null;
				Assert.That (wrapped.CGRectField, Is.Null, "CGRectField - final");

				Assert.That (wrapped.CGSizeField, Is.Null, "CGSizeField");
				var valueCGSizeField = new CGSize (5, 6);
				wrapped.CGSizeField = valueCGSizeField;
				Assert.That (wrapped.CGSizeField, Is.EqualTo (valueCGSizeField), "CGSizeField - set");
				wrapped.CGSizeField = null;
				Assert.That (wrapped.CGSizeField, Is.Null, "CGSizeField - final");

				Assert.That (wrapped.CGPointField, Is.Null, "CGPointField");
				var valueCGPointField = new CGPoint (7, 8);
				wrapped.CGPointField = valueCGPointField;
				Assert.That (wrapped.CGPointField, Is.EqualTo (valueCGPointField), "CGPointField - set");
				wrapped.CGPointField = null;
				Assert.That (wrapped.CGPointField, Is.Null, "CGPointField - final");

				Assert.That (wrapped.CMTimeField, Is.Null, "CMTimeField");
				var valueCMTimeField = new CMTime (123, 2);
				wrapped.CMTimeField = valueCMTimeField;
				Assert.That (wrapped.CMTimeField, Is.EqualTo (valueCMTimeField), "CMTimeField - set");
				wrapped.CMTimeField = null;
				Assert.That (wrapped.CMTimeField, Is.Null, "CMTimeField - final");

#if HAS_UIKIT
				Assert.That (wrapped.UIEdgeInsetsField, Is.Null, "UIEdgeInsetsField");
				var valueUIEdgeInsetsField = new UIEdgeInsets (9, 8, 7, 6);
				wrapped.UIEdgeInsetsField = valueUIEdgeInsetsField;
				Assert.That (wrapped.UIEdgeInsetsField, Is.EqualTo (valueUIEdgeInsetsField), "UIEdgeInsetsField - set");
				wrapped.UIEdgeInsetsField = null;
				Assert.That (wrapped.UIEdgeInsetsField, Is.Null, "UIEdgeInsetsField - final");
#endif // HAS_UIKIT
			});
		}

		[Test]
		public void Matrix ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.Multiple (() => {
				var dict = new VTCompressionPropertyCameraCalibration ();

				Assert.That (dict.IntrinsicMatrix, Is.Null, "IntrinsicMatrix");

				var matrix = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
				dict.IntrinsicMatrix = matrix;
				Assert.That (dict.IntrinsicMatrix, Is.EqualTo (matrix), "IntrinsicMatrix 2");

				dict.IntrinsicMatrix = null;
				Assert.That (dict.IntrinsicMatrix, Is.Null, "IntrinsicMatrix 3");
			});
		}

		[Test]
		public void FloatArray ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.Multiple (() => {
				var dict = new VTCompressionPropertyCameraCalibration ();

				Assert.That (dict.LensDistortions, Is.Null, "LensDistortions");

				var array = new float [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				dict.LensDistortions = array;
				Assert.That (dict.LensDistortions, Is.EqualTo (array), "LensDistortions 2");

				dict.LensDistortions = null;
				Assert.That (dict.LensDistortions, Is.Null, "LensDistortions 3");
			});
		}
	}
}
