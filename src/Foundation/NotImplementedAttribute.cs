//
// NotImplementedAttribute.cs
//
// Authors:
//   Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2017 Xamarin Inc.
//

using System;

#nullable enable

namespace Foundation {
	//
	// This is designed to be applied to setter methods in
	// a base class `Foo' when a `MutableFoo' exists.
	//
	// This allows the Foo.set_XXX to exists but throw an exception
	// but derived classes would then override the property
	//
	[AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
	public class NotImplementedAttribute : Attribute {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NotImplementedAttribute () { }
		/// <param name="message">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NotImplementedAttribute (string message) { Message = message; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Message { get; set; }
	}
}
