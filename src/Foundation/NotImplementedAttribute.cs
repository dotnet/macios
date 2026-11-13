//
// NotImplementedAttribute.cs
//
// Authors:
//   Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2017 Xamarin Inc.
//

#nullable enable

namespace Foundation {
	//
	// This is designed to be applied to setter methods in
	// a base class `Foo' when a `MutableFoo' exists.
	//
	// This allows the Foo.set_XXX to exists but throw an exception
	// but derived classes would then override the property
	//
	/// <summary>Marks a property setter as unsupported on the bound type.</summary>
	/// <remarks>The binding generator emits a setter that throws <see cref="NotSupportedException" />. A mutable subclass may override the property to support setting its value.</remarks>
	[AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
	public class NotImplementedAttribute : Attribute {
		/// <summary>Initializes the attribute with the default read-only property message.</summary>
		public NotImplementedAttribute () { }
		/// <summary>Initializes the attribute with a custom exception message.</summary>
		/// <param name="message">The message for the <see cref="NotSupportedException" /> thrown by the generated setter.</param>
		public NotImplementedAttribute (string message) { Message = message; }
		/// <summary>Gets or sets the exception message for the generated setter.</summary>
		/// <value>The custom message, or <see langword="null" /> to use the default message explaining that the property is read-only and a mutable subclass may support setting it.</value>
		public string? Message { get; set; }
	}
}
