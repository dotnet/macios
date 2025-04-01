//
// Copyright 2014 - 2015, Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

using ObjCRuntime;

#nullable enable

namespace Foundation {

	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ProtocolAttribute : Attribute {

		public ProtocolAttribute () { }

		/// <summary>The type of a specific managed type that can be used to wrap an instane of this protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>Objective-C protocols are bound as interfaces in managed code, but sometimes a class is needed (in certain
		///           scenarios our Objective-C-managed bridge have the pointer to an instance of a native object and we only know that it
		///           implements a particular protocol; in that case we might need a managed type that can wrap this instance, because the
		///           actual type of the object may not formally implement the interface).</remarks>
		public Type? WrapperType { get; set; }
		/// <summary>The name of the protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name { get; set; }
		/// <summary>Whether the Objective-C protocol is an informal protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>An informal protocol is the old name for an Objective-C category.</remarks>
		public bool IsInformal { get; set; }
		// In which SDK version this protocol switched from being informal (i.e. a category) to a formal protocol.
		// System.Version is not a valid type for attributes, so we're using a string instead.
		string? informal_until;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? FormalSince {
			get {
				return informal_until;
			}
			set {
				if (value is not null)
					Version.Parse (value); // This will throw an exception with invalid input, which is what we want.
				informal_until = value;
			}
		}

#if !XAMCORE_5_0
		/// <summary>
		///	  <para>This property indicates whether the binding generator will generate backwards-compatible code for the protocol in question.</para>
		///	  <para>In particular, if this property is true, then the binding generator will generate extension methods for optional members and <see cref="ProtocolMemberAttribute" /> attributes on the protocol interface for all protocol members.</para>
		/// </summary>
		/// <remarks>This property is by default true.</remarks>
		public bool BackwardsCompatibleCodeGeneration { get; set; } = true;
#endif
	}

	[AttributeUsage (AttributeTargets.Interface, AllowMultiple = true)]
	public sealed class ProtocolMemberAttribute : Attribute {
		public ProtocolMemberAttribute () { }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsRequired { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsProperty { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsStatic { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Selector { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type? ReturnType { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type? ReturnTypeDelegateProxy { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type []? ParameterType { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool []? ParameterByRef { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type? []? ParameterBlockProxy { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsVariadic { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type? PropertyType { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? GetterSelector { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? SetterSelector { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ArgumentSemantic ArgumentSemantic { get; set; }
	}

	/// <summary>This attribute is added by the binding generator to members that bind required protocol members.</summary>
	[AttributeUsage (AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class RequiredMemberAttribute : Attribute {
	}

	// There's already an OptionalAttribute in System.Runtime.InteropServices, so I went with
	// "OptionalMemberAttribute" - and in that case it prefered "RequiredMemberAttribute" instead
	// of "RequiredAttribute" just to keep the symmetry.
	/// <summary>This attribute is added by the binding generator to members that bind optional protocol members.</summary>
	[AttributeUsage (AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class OptionalMemberAttribute : Attribute {
	}
}
