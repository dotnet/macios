//
// RequiresSuperAttribute.cs:
//
// Authors:
//   Sebastien Pouliot  <sebastien.pouliot@microsoft.com>
//
// Copyright 2017 Xamarin Inc.

using System;
using Foundation;

namespace ObjCRuntime {

	// https://clang.llvm.org/docs/AttributeReference.html#objc-requires-super-clang-objc-requires-super
	/// <summary>This attribute is applied to methods that must call their base implementation when they're overridden.</summary>
	///     <remarks>
	///       <para>This is the managed equivalent of clang's <format type="text/html"><a href="https://clang.llvm.org/docs/AttributeReference.html#objc-requires-super">objc_requires_super</a></format> attribute, and is applied to managed methods that bind such native methods.</para>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RequiresSuperAttribute : AdviceAttribute {
		/// <summary>Initializes a new instance of the RequiresSuper attribute.</summary>
		///         <remarks>
		///         </remarks>
		public RequiresSuperAttribute ()
			: base ("Overriding this method requires a call to the overriden method.")
		{
		}
	}
}
