//
// GKComponentSystem.cs: Implements some nicer methods for GKComponentSystem
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace GameplayKit {
	public partial class GKComponentSystem<TComponent> {

		/// <summary>Creates a new component system object with default values.</summary>
		///         <remarks>To be added.</remarks>
		public GKComponentSystem ()
			: this (GKState.GetClass (typeof (TComponent), "componentType"))
		{
		}

		/// <summary>Gets the System.Type that represents the component class of the components that can be contained in this component system.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Type? ComponentType {
			get { return Class.Lookup (ComponentClass); }
		}

		/// <param name="index">The 0-based index of the component to get.</param>
		/// <summary>Gets the component at the specified index from the array of components that are contained in the component system.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public TComponent this [nuint index] {
			get { return ObjectAtIndexedSubscript (index); }
		}
	}
}
