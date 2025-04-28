//
// UserDelegateTypeAttribute.cs: Attribute applied to block delegates
// to specify the type of the user-supplied delegate.
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2016 Xamarin Inc.
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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ObjCRuntime {

	// This attribute is emitted by the generator and used at runtime.
	// It's not supposed to be used by manually written code.
	/// <summary>
	///       <para>
	///         This attribute is used on delegates created by the binding generator to properly map between signatures for Objective-C blocks and their corresponding exposed managed delegates.
	///       </para>
	///       <para>
	///         When binding Objective-C blocks, the binding generator will create a managed delegate whose signature is equivalent
	///         to the corresponding block's signature for the required binding code. However, this signature isn't necessarily accurate
	///         enough to re-create the block's signature at runtime (which is needed in some circumstances). This attribute makes it
	///         possible to find the type of the corresponding managed delegate that is exposed by the generated bindings, which does
	///         have enough information to re-create the corresponding block's signature.
	///       </para>
	///     </summary>
	///     <remarks>
	///     </remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[AttributeUsage (AttributeTargets.Delegate | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class UserDelegateTypeAttribute : Attribute {
		/// <param name="userDelegateType">The exposed managed delegate type that corresponds to the delegate this attribute is applied to.</param>
		///         <summary>Initializes a new attribute with the specified managed delegate type.</summary>
		///         <remarks>
		///         </remarks>
		public UserDelegateTypeAttribute (Type userDelegateType)
		{
			UserDelegateType = userDelegateType;
		}

		/// <summary>The managed delegate type that corresponds to the delegate this attribute is applied to.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public Type UserDelegateType {
			get; private set;
		}
	}
}
