//
// Unit tests for DictionaryContainer
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Linq;

using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using ObjCRuntime;
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
				Assert.IsNull (wrapped.SByteField, "SByteField");
				var valueSByteField = SByte.MaxValue;
				wrapped.SByteField = valueSByteField;
				Assert.AreEqual (valueSByteField, wrapped.SByteField, "SByteField - set");
				wrapped.SByteField = null;
				Assert.IsNull (wrapped.SByteField, "SByteField - final");

				Assert.IsNull (wrapped.Int16Field, "Int16Field");
				var valueInt16Field = Int16.MaxValue;
				wrapped.Int16Field = valueInt16Field;
				Assert.AreEqual (valueInt16Field, wrapped.Int16Field, "Int16Field - set");
				wrapped.Int16Field = null;
				Assert.IsNull (wrapped.Int16Field, "Int16Field - final");

				Assert.IsNull (wrapped.Int32Field, "Int32Field");
				var valueInt32Field = Int32.MaxValue;
				wrapped.Int32Field = valueInt32Field;
				Assert.AreEqual (valueInt32Field, wrapped.Int32Field, "Int32Field - set");
				wrapped.Int32Field = null;
				Assert.IsNull (wrapped.Int32Field, "Int32Field - final");

				Assert.IsNull (wrapped.Int64Field, "Int64Field");
				var valueInt64Field = Int64.MaxValue;
				wrapped.Int64Field = valueInt64Field;
				Assert.AreEqual (valueInt64Field, wrapped.Int64Field, "Int64Field - set");
				wrapped.Int64Field = null;
				Assert.IsNull (wrapped.Int64Field, "Int64Field - final");

				Assert.IsNull (wrapped.ByteField, "ByteField");
				var valueByteField = Byte.MaxValue;
				wrapped.ByteField = valueByteField;
				Assert.AreEqual (valueByteField, wrapped.ByteField, "ByteField - set");
				wrapped.ByteField = null;
				Assert.IsNull (wrapped.ByteField, "ByteField - final");

				Assert.IsNull (wrapped.UInt16Field, "UInt16Field");
				var valueUInt16Field = UInt16.MaxValue;
				wrapped.UInt16Field = valueUInt16Field;
				Assert.AreEqual (valueUInt16Field, wrapped.UInt16Field, "UInt16Field - set");
				wrapped.UInt16Field = null;
				Assert.IsNull (wrapped.UInt16Field, "UInt16Field - final");

				Assert.IsNull (wrapped.UInt32Field, "UInt32Field");
				var valueUInt32Field = UInt32.MaxValue;
				wrapped.UInt32Field = valueUInt32Field;
				Assert.AreEqual (valueUInt32Field, wrapped.UInt32Field, "UInt32Field - set");
				wrapped.UInt32Field = null;
				Assert.IsNull (wrapped.UInt32Field, "UInt32Field - final");

				Assert.IsNull (wrapped.UInt64Field, "UInt64Field");
				var valueUInt64Field = UInt64.MaxValue;
				wrapped.UInt64Field = valueUInt64Field;
				Assert.AreEqual (valueUInt64Field, wrapped.UInt64Field, "UInt64Field - set");
				wrapped.UInt64Field = null;
				Assert.IsNull (wrapped.UInt64Field, "UInt64Field - final");

				Assert.IsNull (wrapped.NIntField, "NIntField");
				var valueNIntField = nint.MaxValue;
				wrapped.NIntField = valueNIntField;
				Assert.AreEqual (valueNIntField, wrapped.NIntField, "NIntField - set");
				wrapped.NIntField = null;
				Assert.IsNull (wrapped.NIntField, "NIntField - final");

				Assert.IsNull (wrapped.NUIntField, "NUIntField");
				var valueNUIntField = nuint.MaxValue;
				wrapped.NUIntField = valueNUIntField;
				Assert.AreEqual (valueNUIntField, wrapped.NUIntField, "NUIntField - set");
				wrapped.NUIntField = null;
				Assert.IsNull (wrapped.NUIntField, "NUIntField - final");

				Assert.IsNull (wrapped.SingleField, "SingleField");
				var valueSingleField = Single.MaxValue;
				wrapped.SingleField = valueSingleField;
				Assert.AreEqual (valueSingleField, wrapped.SingleField, "SingleField - set");
				wrapped.SingleField = null;
				Assert.IsNull (wrapped.SingleField, "SingleField - final");

				Assert.IsNull (wrapped.DoubleField, "DoubleField");
				var valueDoubleField = Double.MaxValue;
				wrapped.DoubleField = valueDoubleField;
				Assert.AreEqual (valueDoubleField, wrapped.DoubleField, "DoubleField - set");
				wrapped.DoubleField = null;
				Assert.IsNull (wrapped.DoubleField, "DoubleField - final");

				Assert.IsNull (wrapped.NFloatField, "NFloatField");
				var valueNFloatField = nfloat.MaxValue;
				wrapped.NFloatField = valueNFloatField;
				Assert.AreEqual (valueNFloatField, wrapped.NFloatField, "NFloatField - set");
				wrapped.NFloatField = null;
				Assert.IsNull (wrapped.NFloatField, "NFloatField - final");

				Assert.IsNull (wrapped.NSObjectField, "NSObjectField");
				var valueNSObjectField = (NSString) "NSObjectValue";
				wrapped.NSObjectField = valueNSObjectField;
				Assert.AreEqual (valueNSObjectField, wrapped.NSObjectField, "NSObjectField - set");
				wrapped.NSObjectField = null;
				Assert.IsNull (wrapped.NSObjectField, "NSObjectField - final");

				Assert.IsNull (wrapped.BooleanField, "BooleanField");
				var valueBooleanField = true;
				wrapped.BooleanField = valueBooleanField;
				Assert.AreEqual (valueBooleanField, wrapped.BooleanField, "BooleanField - set");
				wrapped.BooleanField = null;
				Assert.IsNull (wrapped.BooleanField, "BooleanField - final");

				Assert.IsNull (wrapped.NSStringField, "NSStringField");
				var valueNSStringField = (NSString) "NSStringValue";
				wrapped.NSStringField = valueNSStringField;
				Assert.AreEqual (valueNSStringField, wrapped.NSStringField, "NSStringField - set");
				wrapped.NSStringField = null;
				Assert.IsNull (wrapped.NSStringField, "NSStringField - final");

				Assert.IsNull (wrapped.NSDateField, "NSDateField");
				var valueNSDateField = (NSDate) new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.NSDateField = valueNSDateField;
				Assert.AreEqual (valueNSDateField, wrapped.NSDateField, "NSDateField - set");
				wrapped.NSDateField = null;
				Assert.IsNull (wrapped.NSDateField, "NSDateField - final");

				Assert.IsNull (wrapped.NSDictionaryField, "NSDictionaryField");
				var valueNSDictionaryField = new NSDictionary ();
				wrapped.NSDictionaryField = valueNSDictionaryField;
				Assert.AreEqual (valueNSDictionaryField, wrapped.NSDictionaryField, "NSDictionaryField - set");
				wrapped.NSDictionaryField = null;
				Assert.IsNull (wrapped.NSDictionaryField, "NSDictionaryField - final");

				Assert.IsNull (wrapped.NSStrongDictionaryField, "NSStrongDictionaryField");
				var valueNSStrongDictionaryField = new WrappedNSDictionary ();
				wrapped.NSStrongDictionaryField = valueNSStrongDictionaryField;
				Assert.AreEqual (valueNSStrongDictionaryField?.Dictionary?.ToString (), wrapped.NSStrongDictionaryField?.Dictionary?.ToString (), "NSStrongDictionaryField - set");
				wrapped.NSStrongDictionaryField = null;
				Assert.IsNull (wrapped.NSStrongDictionaryField, "NSStrongDictionaryField - final");

				Assert.IsNull (wrapped.StrongEnumField, "StrongEnumField");
				var valueStrongEnumField = StrongEnum.C;
				wrapped.StrongEnumField = valueStrongEnumField;
				Assert.AreEqual (valueStrongEnumField, wrapped.StrongEnumField, "StrongEnumField - set");
				wrapped.StrongEnumField = null;
				Assert.IsNull (wrapped.StrongEnumField, "StrongEnumField - final");

				Assert.IsNull (wrapped.NormalEnumField, "NormalEnumField");
				var valueNormalEnumField = NormalEnum.Z;
				wrapped.NormalEnumField = valueNormalEnumField;
				Assert.AreEqual (valueNormalEnumField, wrapped.NormalEnumField, "NormalEnumField - set");
				wrapped.NormalEnumField = null;
				Assert.IsNull (wrapped.NormalEnumField, "NormalEnumField - final");

				Assert.IsNull (wrapped.ArrayOfSByteField, "ArrayOfSByteField");
				var valueArrayOfSByteField = new sbyte [] { 1, 2, 3 };
				wrapped.ArrayOfSByteField = valueArrayOfSByteField;
				Assert.AreEqual (valueArrayOfSByteField, wrapped.ArrayOfSByteField, "ArrayOfSByteField - set");
				wrapped.ArrayOfSByteField = null;
				Assert.IsNull (wrapped.ArrayOfSByteField, "ArrayOfSByteField - final");

				Assert.IsNull (wrapped.ArrayOfInt16Field, "ArrayOfInt16Field");
				var valueArrayOfInt16Field = new short [] { 1, 2, 3 };
				wrapped.ArrayOfInt16Field = valueArrayOfInt16Field;
				Assert.AreEqual (valueArrayOfInt16Field, wrapped.ArrayOfInt16Field, "ArrayOfInt16Field - set");
				wrapped.ArrayOfInt16Field = null;
				Assert.IsNull (wrapped.ArrayOfInt16Field, "ArrayOfInt16Field - final");

				Assert.IsNull (wrapped.ArrayOfInt32Field, "ArrayOfInt32Field");
				var valueArrayOfInt32Field = new int [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt32Field = valueArrayOfInt32Field;
				Assert.AreEqual (valueArrayOfInt32Field, wrapped.ArrayOfInt32Field, "ArrayOfInt32Field - set");
				wrapped.ArrayOfInt32Field = null;
				Assert.IsNull (wrapped.ArrayOfInt32Field, "ArrayOfInt32Field - final");

				Assert.IsNull (wrapped.ArrayOfInt64Field, "ArrayOfInt64Field");
				var valueArrayOfInt64Field = new long [] { 1, 2, 3 }; ;
				wrapped.ArrayOfInt64Field = valueArrayOfInt64Field;
				Assert.AreEqual (valueArrayOfInt64Field, wrapped.ArrayOfInt64Field, "ArrayOfInt64Field - set");
				wrapped.ArrayOfInt64Field = null;
				Assert.IsNull (wrapped.ArrayOfInt64Field, "ArrayOfInt64Field - final");

				Assert.IsNull (wrapped.ArrayOfByteField, "ArrayOfByteField");
				var valueArrayOfByteField = new byte [] { 1, 2, 3 }; ;
				wrapped.ArrayOfByteField = valueArrayOfByteField;
				Assert.AreEqual (valueArrayOfByteField, wrapped.ArrayOfByteField, "ArrayOfByteField - set");
				wrapped.ArrayOfByteField = null;
				Assert.IsNull (wrapped.ArrayOfByteField, "ArrayOfByteField - final");

				Assert.IsNull (wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field");
				var valueArrayOfUInt16Field = new ushort [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt16Field = valueArrayOfUInt16Field;
				Assert.AreEqual (valueArrayOfUInt16Field, wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field - set");
				wrapped.ArrayOfUInt16Field = null;
				Assert.IsNull (wrapped.ArrayOfUInt16Field, "ArrayOfUInt16Field - final");

				Assert.IsNull (wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field");
				var valueArrayOfUInt32Field = new uint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt32Field = valueArrayOfUInt32Field;
				Assert.AreEqual (valueArrayOfUInt32Field, wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field - set");
				wrapped.ArrayOfUInt32Field = null;
				Assert.IsNull (wrapped.ArrayOfUInt32Field, "ArrayOfUInt32Field - final");

				Assert.IsNull (wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field");
				var valueArrayOfUInt64Field = new ulong [] { 1, 2, 3 }; ;
				wrapped.ArrayOfUInt64Field = valueArrayOfUInt64Field;
				Assert.AreEqual (valueArrayOfUInt64Field, wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field - set");
				wrapped.ArrayOfUInt64Field = null;
				Assert.IsNull (wrapped.ArrayOfUInt64Field, "ArrayOfUInt64Field - final");

				Assert.IsNull (wrapped.ArrayOfNIntField, "ArrayOfNIntField");
				var valueArrayOfNIntField = new nint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNIntField = valueArrayOfNIntField;
				Assert.AreEqual (valueArrayOfNIntField, wrapped.ArrayOfNIntField, "ArrayOfNIntField - set");
				wrapped.ArrayOfNIntField = null;
				Assert.IsNull (wrapped.ArrayOfNIntField, "ArrayOfNIntField - final");

				Assert.IsNull (wrapped.ArrayOfNUIntField, "ArrayOfNUIntField");
				var valueArrayOfNUIntField = new nuint [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNUIntField = valueArrayOfNUIntField;
				Assert.AreEqual (valueArrayOfNUIntField, wrapped.ArrayOfNUIntField, "ArrayOfNUIntField - set");
				wrapped.ArrayOfNUIntField = null;
				Assert.IsNull (wrapped.ArrayOfNUIntField, "ArrayOfNUIntField - final");

				Assert.IsNull (wrapped.ArrayOfSingleField, "ArrayOfSingleField");
				var valueArrayOfSingleField = new float [] { 1, 2, 3 }; ;
				wrapped.ArrayOfSingleField = valueArrayOfSingleField;
				Assert.AreEqual (valueArrayOfSingleField, wrapped.ArrayOfSingleField, "ArrayOfSingleField - set");
				wrapped.ArrayOfSingleField = null;
				Assert.IsNull (wrapped.ArrayOfSingleField, "ArrayOfSingleField - final");

				Assert.IsNull (wrapped.ArrayOfDoubleField, "ArrayOfDoubleField");
				var valueArrayOfDoubleField = new double [] { 1, 2, 3 }; ;
				wrapped.ArrayOfDoubleField = valueArrayOfDoubleField;
				Assert.AreEqual (valueArrayOfDoubleField, wrapped.ArrayOfDoubleField, "ArrayOfDoubleField - set");
				wrapped.ArrayOfDoubleField = null;
				Assert.IsNull (wrapped.ArrayOfDoubleField, "ArrayOfDoubleField - final");

				Assert.IsNull (wrapped.ArrayOfNFloatField, "ArrayOfNFloatField");
				var valueArrayOfNFloatField = new nfloat [] { 1, 2, 3 }; ;
				wrapped.ArrayOfNFloatField = valueArrayOfNFloatField;
				Assert.AreEqual (valueArrayOfNFloatField, wrapped.ArrayOfNFloatField, "ArrayOfNFloatField - set");
				wrapped.ArrayOfNFloatField = null;
				Assert.IsNull (wrapped.ArrayOfNFloatField, "ArrayOfNFloatField - final");

				Assert.IsNull (wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField");
				var valueArrayOfNSObjectField = new NSObject [] { (NSString) "Array1", NSDate.Now };
				wrapped.ArrayOfNSObjectField = valueArrayOfNSObjectField;
				Assert.AreEqual (valueArrayOfNSObjectField, wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField - set");
				wrapped.ArrayOfNSObjectField = null;
				Assert.IsNull (wrapped.ArrayOfNSObjectField, "ArrayOfNSObjectField - final");

				Assert.IsNull (wrapped.ArrayOfBooleanField, "ArrayOfBooleanField");
				var valueArrayOfBooleanField = new bool [] { true, false, true };
				wrapped.ArrayOfBooleanField = valueArrayOfBooleanField;
				Assert.AreEqual (valueArrayOfBooleanField, wrapped.ArrayOfBooleanField, "ArrayOfBooleanField - set");
				wrapped.ArrayOfBooleanField = null;
				Assert.IsNull (wrapped.ArrayOfBooleanField, "ArrayOfBooleanField - final");

				Assert.IsNull (wrapped.ArrayOfNSStringField, "ArrayOfNSStringField");
				var valueArrayOfNSStringField = new NSString [] { (NSString) "a", (NSString) "b", (NSString) "c" };
				wrapped.ArrayOfNSStringField = valueArrayOfNSStringField;
				Assert.AreEqual (valueArrayOfNSStringField, wrapped.ArrayOfNSStringField, "ArrayOfNSStringField - set");
				wrapped.ArrayOfNSStringField = null;
				Assert.IsNull (wrapped.ArrayOfNSStringField, "ArrayOfNSStringField - final");

				Assert.IsNull (wrapped.ArrayOfNSDateField, "ArrayOfNSDateField");
				var valueArrayOfNSDateField = new NSDate [] { NSDate.Now, NSDate.Now };
				wrapped.ArrayOfNSDateField = valueArrayOfNSDateField;
				Assert.AreEqual (valueArrayOfNSDateField, wrapped.ArrayOfNSDateField, "ArrayOfNSDateField - set");
				wrapped.ArrayOfNSDateField = null;
				Assert.IsNull (wrapped.ArrayOfNSDateField, "ArrayOfNSDateField - final");

				Assert.IsNull (wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField");
				var valueArrayOfNSDictionaryField = new NSDictionary [] { new NSDictionary (), new NSDictionary () };
				wrapped.ArrayOfNSDictionaryField = valueArrayOfNSDictionaryField;
				Assert.AreEqual (valueArrayOfNSDictionaryField, wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField - set");
				wrapped.ArrayOfNSDictionaryField = null;
				Assert.IsNull (wrapped.ArrayOfNSDictionaryField, "ArrayOfNSDictionaryField - final");

				Assert.IsNull (wrapped.ArrayOfStrongDictionaryField, "ArrayOfStrongDictionaryField");
				var valueArrayOfStrongDictionaryField = new WrappedNSDictionary [] { new WrappedNSDictionary (), new WrappedNSDictionary () };
				wrapped.ArrayOfStrongDictionaryField = valueArrayOfStrongDictionaryField;
				Assert.AreEqual (string.Join (";", valueArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ())), string.Join (";", wrapped.ArrayOfStrongDictionaryField.Select (v => v?.Dictionary?.ToString ())), "ArrayOfStrongDictionaryField - set");
				wrapped.ArrayOfStrongDictionaryField = null;
				Assert.IsNull (wrapped.ArrayOfStrongDictionaryField, "ArrayOfStrongDictionaryField - final");

				Assert.IsNull (wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField");
				var valueArrayOfStrongEnumField = new StrongEnum [] { StrongEnum.A, StrongEnum.B };
				wrapped.ArrayOfStrongEnumField = valueArrayOfStrongEnumField;
				Assert.AreEqual (valueArrayOfStrongEnumField, wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField - set");
				wrapped.ArrayOfStrongEnumField = null;
				Assert.IsNull (wrapped.ArrayOfStrongEnumField, "ArrayOfStrongEnumField - final");

				Assert.IsNull (wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField");
				var valueArrayOfNormalEnumField = new NormalEnum [] { NormalEnum.X, NormalEnum.Y };
				wrapped.ArrayOfNormalEnumField = valueArrayOfNormalEnumField;
				Assert.AreEqual (valueArrayOfNormalEnumField, wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField - set");
				wrapped.ArrayOfNormalEnumField = null;
				Assert.IsNull (wrapped.ArrayOfNormalEnumField, "ArrayOfNormalEnumField - final");

				Assert.IsNull (wrapped.StringField, "StringField");
				var valueStringField = "managed string";
				wrapped.StringField = valueStringField;
				Assert.AreEqual (valueStringField, wrapped.StringField, "StringField - set");
				wrapped.StringField = null;
				Assert.IsNull (wrapped.StringField, "StringField - final");

				Assert.IsNull (wrapped.DateTimeField, "DateTimeField");
				var valueDateTimeField = new DateTime (2025, 09, 01, 12, 45, 55, 23).ToUniversalTime ();
				wrapped.DateTimeField = valueDateTimeField;
				Assert.AreEqual (valueDateTimeField, wrapped.DateTimeField, "DateTimeField - set");
				wrapped.DateTimeField = null;
				Assert.IsNull (wrapped.DateTimeField, "DateTimeField - final");

				Assert.IsNull (wrapped.GenericNSDictionaryField, "GenericNSDictionaryField");
				var valueGenericNSDictionaryField = new NSDictionary<NSString, NSObject> ();
				wrapped.GenericNSDictionaryField = valueGenericNSDictionaryField;
				Assert.AreEqual (valueGenericNSDictionaryField, wrapped.GenericNSDictionaryField, "GenericNSDictionaryField - set");
				wrapped.GenericNSDictionaryField = null;
				Assert.IsNull (wrapped.GenericNSDictionaryField, "GenericNSDictionaryField - final");

				Assert.IsNull (wrapped.ArrayOfStringField, "ArrayOfStringField");
				var valueArrayOfStringField = new string [] { "abc", "def", "ghi" };
				wrapped.ArrayOfStringField = valueArrayOfStringField;
				Assert.AreEqual (valueArrayOfStringField, wrapped.ArrayOfStringField, "ArrayOfStringField - set");
				wrapped.ArrayOfStringField = null;
				Assert.IsNull (wrapped.ArrayOfStringField, "ArrayOfStringField - final");

				Assert.IsNull (wrapped.NSDataField, "NSDataField");
				var valueNSDataField = NSData.FromArray (new byte [] { 1, 2, 3 });
				wrapped.NSDataField = valueNSDataField;
				Assert.AreEqual (valueNSDataField, wrapped.NSDataField, "NSDataField - set");
				wrapped.NSDataField = null;
				Assert.IsNull (wrapped.NSDataField, "NSDataField - final");

				Assert.IsNull (wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field");
				var valueNSDataAsMatrix3Field = new NMatrix3 (1, 2, 3, 4, 5, 6, 7, 8, 9);
				wrapped.NSDataAsMatrix3Field = valueNSDataAsMatrix3Field;
				Assert.AreEqual (valueNSDataAsMatrix3Field, wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field - set");
				wrapped.NSDataAsMatrix3Field = null;
				Assert.IsNull (wrapped.NSDataAsMatrix3Field, "NSDataAsMatrix3Field - final");

				Assert.IsNull (wrapped.CGRectField, "CGRectField");
				var valueCGRectField = new CGRect (1, 2, 3, 4);
				wrapped.CGRectField = valueCGRectField;
				Assert.AreEqual (valueCGRectField, wrapped.CGRectField, "CGRectField - set");
				wrapped.CGRectField = null;
				Assert.IsNull (wrapped.CGRectField, "CGRectField - final");

				Assert.IsNull (wrapped.CGSizeField, "CGSizeField");
				var valueCGSizeField = new CGSize (5, 6);
				wrapped.CGSizeField = valueCGSizeField;
				Assert.AreEqual (valueCGSizeField, wrapped.CGSizeField, "CGSizeField - set");
				wrapped.CGSizeField = null;
				Assert.IsNull (wrapped.CGSizeField, "CGSizeField - final");

				Assert.IsNull (wrapped.CGPointField, "CGPointField");
				var valueCGPointField = new CGPoint (7, 8);
				wrapped.CGPointField = valueCGPointField;
				Assert.AreEqual (valueCGPointField, wrapped.CGPointField, "CGPointField - set");
				wrapped.CGPointField = null;
				Assert.IsNull (wrapped.CGPointField, "CGPointField - final");

				Assert.IsNull (wrapped.CMTimeField, "CMTimeField");
				var valueCMTimeField = new CMTime (123, 2);
				wrapped.CMTimeField = valueCMTimeField;
				Assert.AreEqual (valueCMTimeField, wrapped.CMTimeField, "CMTimeField - set");
				wrapped.CMTimeField = null;
				Assert.IsNull (wrapped.CMTimeField, "CMTimeField - final");

#if HAS_UIKIT
				Assert.IsNull (wrapped.UIEdgeInsetsField, "UIEdgeInsetsField");
				var valueUIEdgeInsetsField = new UIEdgeInsets (9, 8, 7, 6);
				wrapped.UIEdgeInsetsField = valueUIEdgeInsetsField;
				Assert.AreEqual (valueUIEdgeInsetsField, wrapped.UIEdgeInsetsField, "UIEdgeInsetsField - set");
				wrapped.UIEdgeInsetsField = null;
				Assert.IsNull (wrapped.UIEdgeInsetsField, "UIEdgeInsetsField - final");
#endif // HAS_UIKIT
			});
		}
	}
}
