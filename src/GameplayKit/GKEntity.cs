//
// GKEntity.cs: Implements some nicer methods for GKEntity
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace GameplayKit {
	public partial class GKEntity {

		/// <summary>Removes the element in <see cref="GameplayKit.GKEntity.Components" /> of the specified <paramref name="componentType" />.</summary>
		/// <param name="componentType">The component type.</param>
		public void RemoveComponent (Type componentType)
		{
			RemoveComponent (GKState.GetClass (componentType, "componentType"));
		}

		/// <summary>Retrieves the element in <see cref="GameplayKit.GKEntity.Components" /> of the specified <paramref name="componentType" />.</summary>
		/// <param name="componentType">The component type.</param>
		public GKComponent? GetComponent (Type componentType)
		{
			return GetComponent (GKState.GetClass (componentType, "componentType"));
		}
	}
}
