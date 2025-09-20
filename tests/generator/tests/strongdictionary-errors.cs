using System;
using Foundation;

namespace BackingField {
	[BackingFieldType (typeof (nint))]
	enum NSIntegerFieldType {
		[Field ("AField", "__Internal")]
		A,
	}

	[BackingFieldType (typeof (nuint))]
	enum NSUIntegerFieldType {
		[Field ("BField", "__Internal")]
		B,
	}

	[BackingFieldType (typeof (NSNumber))]
	enum NSNumberFieldType {
		[Field ("CField", "__Internal")]
		C,
	}

	[BackingFieldType (typeof (Int32))]
	enum Int32FieldType {
		[Field ("DField", "__Internal")]
		D,
	}

	[BackingFieldType (typeof (Int64))]
	enum Int64FieldType {
		[Field ("EField", "__Internal")]
		E,
	}

	[BackingFieldType (typeof (UInt32))]
	enum UInt32FieldType {
		[Field ("FField", "__Internal")]
		F,
	}

	[BackingFieldType (typeof (UInt64))]
	enum UInt64FieldType {
		[Field ("GField", "__Internal")]
		G,
	}

	[BackingFieldType (typeof (NSString))]
	enum NSStringFieldType {
		[Field ("ZField", "__Internal")]
		Z,
	}

	[Partial]
	interface StrongDictionaryKeys {
		[Field ("StrongDictionaryNSIntegerKey", "__Internal")]
		NSString NSIntegerValueKey { get; }

		[Field ("StrongDictionaryNSUIntegerKey", "__Internal")]
		NSString NSUIntegerValueKey { get; }

		[Field ("StrongDictionaryNSNumberKey", "__Internal")]
		NSString NSNumberValueKey { get; }

		[Field ("StrongDictionaryInt32Key", "__Internal")]
		NSString Int32ValueKey { get; }

		[Field ("StrongDictionaryInt64Key", "__Internal")]
		NSString Int64ValueKey { get; }

		[Field ("StrongDictionaryUInt32Key", "__Internal")]
		NSString UInt32ValueKey { get; }

		[Field ("StrongDictionaryUInt64Key", "__Internal")]
		NSString UInt64ValueKey { get; }

		[Field ("StrongDictionaryNSStringKey", "__Internal")]
		NSString NSStringValueKey { get; }

		[Field ("StrongDictionaryArrayOfNSIntegerKey", "__Internal")]
		NSString ArrayOfNSIntegerValueKey { get; }

		[Field ("StrongDictionaryArrayOfNSUIntegerKey", "__Internal")]
		NSString ArrayOfNSUIntegerValueKey { get; }

		[Field ("StrongDictionaryArrayOfNSNumberKey", "__Internal")]
		NSString ArrayOfNSNumberValueKey { get; }

		[Field ("StrongDictionaryArrayOfInt32Key", "__Internal")]
		NSString ArrayOfInt32ValueKey { get; }

		[Field ("StrongDictionaryArrayOfInt64Key", "__Internal")]
		NSString ArrayOfInt64ValueKey { get; }

		[Field ("StrongDictionaryArrayOfUInt32Key", "__Internal")]
		NSString ArrayOfUInt32ValueKey { get; }

		[Field ("StrongDictionaryArrayOfUInt64Key", "__Internal")]
		NSString ArrayOfUInt64ValueKey { get; }

		[Field ("StrongDictionaryArrayOfNSStringKey", "__Internal")]
		NSString ArrayOfNSStringValueKey { get; }
	}

	[StrongDictionary ("StrongDictionaryKeys")]
	interface StrongDictionary {
		NSIntegerFieldType NSIntegerValue { get; }
		NSUIntegerFieldType NSUIntegerValue { get; }
		NSNumberFieldType NSNumberValue { get; }
		Int32FieldType Int32Value { get; }
		Int64FieldType Int64Value { get; }
		UInt32FieldType UInt32Value { get; }
		UInt64FieldType UInt64Value { get; }
		NSStringFieldType NSStringValue { get; }

		NSIntegerFieldType [] ArrayOfNSIntegerValue { get; }
		NSUIntegerFieldType [] ArrayOfNSUIntegerValue { get; }
		NSNumberFieldType [] ArrayOfNSNumberValue { get; }
		Int32FieldType [] ArrayOfInt32Value { get; }
		Int64FieldType [] ArrayOfInt64Value { get; }
		UInt32FieldType [] ArrayOfUInt32Value { get; }
		UInt64FieldType [] ArrayOfUInt64Value { get; }
		NSStringFieldType [] ArrayOfNSStringValue { get; }
	}
}
