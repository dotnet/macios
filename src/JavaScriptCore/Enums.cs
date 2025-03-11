//
// Enums.cs: enums for JavaScriptCore
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc.

using System;

using ObjCRuntime;

namespace JavaScriptCore {
	// untyped enum -> JSValueRef.h
	/// <summary>An enumeration whose values specify JavaScript types, including <c>Object</c> and <c>undefined</c>.</summary>
	public enum JSType {
		/// <summary>To be added.</summary>
		Undefined,
		/// <summary>To be added.</summary>
		Null,
		/// <summary>To be added.</summary>
		Boolean,
		/// <summary>To be added.</summary>
		Number,
		/// <summary>To be added.</summary>
		String,
		/// <summary>To be added.</summary>
		Object,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Symbol,
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
