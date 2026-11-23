// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Foundation;
using ObjCRuntime;

namespace FastEnumeration {
	[BaseType (typeof (NSObject))]
	[NSFastEnumeration]
	interface UntypedCollection {
	}

	[BaseType (typeof (NSObject))]
	[NSFastEnumeration (typeof (NSString))]
	interface StringCollection {
	}

	[BaseType (typeof (StringCollection))]
	interface DerivedStringCollection {
	}

	[BaseType (typeof (StringCollection))]
	[NSFastEnumeration (typeof (NSString))]
	interface RepeatedStringCollection {
	}

	[BaseType (typeof (UntypedCollection))]
	[NSFastEnumeration (typeof (NSString))]
	interface TypedDerivedCollection {
	}

	[BaseType (typeof (NSObject))]
	[NSFastEnumeration ("T")]
	interface GenericCollection<T> where T : NSObject {
	}

	[BaseType (typeof (NSObject))]
	[NSFastEnumeration ("TKey")]
	interface GenericKeyCollection<TKey, TElement>
		where TKey : NSString
		where TElement : GameController.IGCPhysicalInputElement {
	}

	[Protocol]
	[NSFastEnumeration (typeof (NSString))]
	interface StringCollectionProtocol {
	}

	[BaseType (typeof (NSObject))]
	interface ProtocolCollection : StringCollectionProtocol {
	}

	[BaseType (typeof (NSObject))]
	[NSFastEnumeration (typeof (INSCopying))]
	interface ProtocolElementCollection {
	}

	[BaseType (typeof (NSObject))]
	interface ExternalProtocolCollection : ModelIO.IMDLObjectContainerComponent {
	}

	[BaseType (typeof (ModelIO.MDLMaterial))]
	[NSFastEnumeration (typeof (ModelIO.MDLMaterialProperty))]
	interface ExternalMaterialCollection {
	}
}
