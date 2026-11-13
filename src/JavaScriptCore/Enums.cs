//
// Enums.cs: enums for JavaScriptCore
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc.

namespace JavaScriptCore {
	// untyped enum -> JSValueRef.h
	/// <summary>Identifies the type of a JavaScript value.</summary>
	public enum JSType {
		/// <summary>The unique undefined value.</summary>
		Undefined,
		/// <summary>The unique null value.</summary>
		Null,
		/// <summary>A primitive Boolean value, either <see langword="true"/> or <see langword="false"/>.</summary>
		Boolean,
		/// <summary>A primitive number value.</summary>
		Number,
		/// <summary>A primitive string value.</summary>
		String,
		/// <summary>An object value, represented by a <c>JSObjectRef</c>.</summary>
		Object,
		/// <summary>A primitive symbol value.</summary>
		[MacCatalyst (13, 1)]
		Symbol,
		/// <summary>A primitive BigInt value.</summary>
		[iOS (18, 0), MacCatalyst (18, 0), TV (18, 0), NoMac]
		BigInt,
	}

	// typedef unsigned -> JSObjectRef.h
	/// <summary>Enumerates attributes of properties.</summary>
	[Flags]
	public enum JSPropertyAttributes : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		ReadOnly = 1 << 1,
		/// <summary>To be added.</summary>
		DontEnum = 1 << 2,
		/// <summary>To be added.</summary>
		DontDelete = 1 << 3,
	}

	// typedef unsigned -> JSObjectRef.h
	/// <summary>Enumerates class attributes.</summary>
	[Flags]
	public enum JSClassAttributes : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		NoAutomaticPrototype = 1 << 1,
	}

	[iOS (18, 0), MacCatalyst (18, 0), TV (18, 0), Mac (15, 0)]
	public enum JSRelationCondition : uint {
		Undefined,
		Equal,
		GreaterThan,
		LessThan,
	}
}
