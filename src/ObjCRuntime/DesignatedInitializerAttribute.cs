//
// DesignatedInitializerAttribute.cs:
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2015 Xamarin Inc.

using System;

namespace ObjCRuntime {

	// For bindings the attribute is used on interfaces, which means we must be able to decorated methods
	// not only constructors
	/// <summary>This attribute is used to mark managed constructors that bind Objective-C initializers marked with the NS_DESIGNATED_INITIALIZER attribute.</summary>
	///     <remarks>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Constructor | AttributeTargets.Method)]
	public class DesignatedInitializerAttribute : Attribute {
		/// <summary>Initializes new DesignatedInitializer attribute.</summary>
		///         <remarks>
		///         </remarks>
		public DesignatedInitializerAttribute ()
		{
		}
	}
}
