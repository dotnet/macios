//
// GKEntity.cs: Implements some nicer methods for GKEntity
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
	public partial class GKEntity {

		/// <param name="componentType">To be added.</param>
		///         <summary>Removes the element in <see cref="GameplayKit.GKEntity.Components" /> of the specified <paramref name="componentType" />.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveComponent (Type componentType)
		{
			RemoveComponent (GKState.GetClass (componentType, "componentType"));
		}

		/// <param name="componentType">To be added.</param>
		///         <summary>Retrieves the element in <see cref="GameplayKit.GKEntity.Components" /> of the specified <paramref name="componentType" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public GKComponent? GetComponent (Type componentType)
		{
			return GetComponent (GKState.GetClass (componentType, "componentType"));
		}
	}
}
