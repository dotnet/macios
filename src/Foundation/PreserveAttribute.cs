//
// PreserveAttribute.cs
//
// Authors:
//   Jb Evain
//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2012,2015 Xamarin Inc.
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
//

using System;

namespace Foundation {

	[AttributeUsage (
		AttributeTargets.Assembly
		| AttributeTargets.Class
		| AttributeTargets.Struct
		| AttributeTargets.Enum
		| AttributeTargets.Constructor
		| AttributeTargets.Method
		| AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Event
		| AttributeTargets.Interface
		| AttributeTargets.Delegate,
		AllowMultiple = true)]
	public sealed class PreserveAttribute : Attribute {

		/// <summary>Ensures that all members of this type are preserved.</summary>
		///         <remarks>All members of this type, including fields, properties, methods, subclasses are preserved during linking.</remarks>
		public bool AllMembers;
		/// <summary>Flags the method as a method to preserve during linking if the container class is pulled in.</summary>
		///         <remarks>
		///           <para>
		/// 	    If the Conditional value is set on a Preserve attribute on a method, then the method will be preserved if the containing NSObject is kept after the linker has done its job.
		/// 	  </para>
		///           <para>
		/// 	    You would typically use this for callbacks that you know will be called in your code dynamically (for example with a selector invocation from Objective-C) since a static linker would not be able to infer that this particual method is required.
		/// 	  </para>
		///         </remarks>
		public bool Conditional;

		/// <summary>Instruct the MonoTouch linker to preserve the decorated code</summary>
		///         <remarks>By default the linker, when enabled, will remove all the code that is not directly used by the application.</remarks>
		public PreserveAttribute ()
		{
		}

		/// <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PreserveAttribute (Type type)
		{
		}
	}
}
