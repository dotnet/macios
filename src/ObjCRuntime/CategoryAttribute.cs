//
// CategoryAttribyute.cs:
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.

using System;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace ObjCRuntime {
	/// <include file="../../docs/api/ObjCRuntime/CategoryAttribute.xml" path="/Documentation/Docs[@DocId='T:ObjCRuntime.CategoryAttribute']/*" />
	[AttributeUsage (AttributeTargets.Class)]
	public class CategoryAttribute : Attribute {
		/// <param name="type">The Objective-C type to extend.   This must be a subclass of NSObject (or NSObject itself).</param>
		///         <summary>The type that this category extends.</summary>
		///         <remarks>
		///         </remarks>
		public CategoryAttribute (Type type)
		{
			Type = type;
		}

		/// <summary>The type that this category extends.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public Type Type { get; set; }
		/// <summary>The name of the category.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>This must be a valid Objective-C type name, but is otherwise unused.</para>
		///         </remarks>
		public string Name { get; set; }
	}
}
