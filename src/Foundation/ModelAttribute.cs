//
// Copyright 2009-2010, Novell, Inc.
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

#nullable enable

namespace Foundation {

	/// <summary>Flags a class or interface as a model, indicating that only explicitly overridden methods are exposed to the Objective-C runtime.</summary>
	/// <remarks>
	///   <para>Objective-C protocols are like interfaces, but they support optional methods, that is, not all of the methods need to be implemented for the protocol to work.</para>
	///   <para>The [Model] attribute is applied to a class that can have optional methods, and it is typically used for declaring Objective-C delegates or data models that have a number of optional methods. The runtime treats classes with the Model attribute applied specially: only when a user overrides methods in a class is the actual override exposed to the Objective-C world as existing.</para>
	///   <para>This attribute is used in all of the models and delegate classes to allow the user to only implement the methods that they are interested in.</para>
	/// </remarks>
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ModelAttribute : Attribute {

		/// <summary>Initializes a new instance of the <see cref="ModelAttribute" /> class.</summary>
		public ModelAttribute () { }

		/// <summary>Gets or sets the Objective-C type name for the model.</summary>
		/// <value>The Objective-C type name for the model.</value>
		/// <remarks>
		///   <para>This value is only taken into account when used in binding code / projects.</para>
		/// </remarks>
		public string? Name { get; set; }
	}
}
