//
// Attribute to link delegates to their marshallers
//
// Copyright 2013 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace ObjCRuntime {
	/// <summary>This attribute is used to notify the runtime which class is used to wrap Objective-C blocks into managed delegates.</summary>
	///     <remarks>
	///       <para>
	/// 	This type is used by the internals of Xamarin.iOS.
	///       </para>
	///       <para>
	/// 	This attribute is applied on parameters and is used by the
	/// 	Xamarin.iOS runtime to locate the helper class that is
	/// 	used to turn an Objective-C block into a managed delegate that can
	/// 	later be invoked by managed code to trigger a native block execution.
	///       </para>
	///     </remarks>
	[AttributeUsage (AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class BlockProxyAttribute : Attribute {
		/// <param name="t">Proxy type.</param>
		///         <summary>Specifies the type that is used to proxy blocks into managed delegates.</summary>
		///         <remarks>
		///         </remarks>
		public BlockProxyAttribute (Type t) { Type = t; }
		/// <summary>The type that is used to proxy an Objective-C block into this managed parameter.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public Type Type { get; set; }
	}

	/// <summary>This attribute is used to notify the runtime which class is used to wrap managed delegates into Objective-C blocks.</summary>
	///     <remarks>
	///       <para>
	///   This type is used by the internals of Xamarin.iOS.
	///       </para>
	///       <para>
	///   This attribute is applied on return types and is used by the
	///   Xamarin.iOS runtime to locate the helper class that is
	///   used to turn a managed delegate into an Objective-C block that can
	///   later be invoked by native code to trigger a managed execution.
	///       </para>
	///     </remarks>
	[AttributeUsage (AttributeTargets.ReturnValue, AllowMultiple = false)]
	public sealed class DelegateProxyAttribute : Attribute {
		/// <param name="delegateType">The delegate type</param>
		///         <summary>Specifies the delegate type that is used to proxy managed delegates into Objective-C blocks.</summary>
		///         <remarks>
		///         </remarks>
		public DelegateProxyAttribute (Type delegateType)
		{
			DelegateType = delegateType;
		}
		/// <summary>The delegate type that is used to proxy managed delegates into Objective-C blocks.</summary>
		///         <value>The delegate type that is used to proxy managed delegates into Objective-C blocks.</value>
		///         <remarks>
		///         </remarks>
		public Type DelegateType { get; set; }
	}
}
