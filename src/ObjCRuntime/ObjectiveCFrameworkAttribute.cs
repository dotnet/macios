// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace ObjCRuntime {
	/// <summary>This attribute tells the runtime which Objective-C framework a given type belongs to.</summary>
	/// <remarks>This is used to determine which Objective-C frameworks to link with during the build.</remarks>
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ObjectiveCFrameworkAttribute : Attribute {
		/// <summary>Initializes a new <see cref="ObjectiveCFrameworkAttribute" /> attribute.</summary>
		/// <param name="framework">The name of the Objective-C framework.</param>
		public ObjectiveCFrameworkAttribute (string framework)
		{
			Framework = framework;
		}

		/// <summary>The name of the Objective-C framework.</summary>
		public string Framework { get; set; }
	}
}
