// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

// create a protocol class for the tests, otherwise the generator will not generate the events since we are expecting a
// rgen decorated interface

[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<Protocol>]
public partial interface IRgenNSKeyedArchiverDelegate {

	// not event args, should not be needed
	[Export<Method> ("archiver:didEncodeObject:", Flags = ObjCBindings.Method.Event, EventArgsType = typeof (NSObject))]
	void EncodedObject (NSKeyedArchiver archiver, NSObject obj);

	// two params, expect to use the second one as the event args type
	[Export<Method> ("archiver:didEncodeObject:", Flags = ObjCBindings.Method.Event)]
	void EncodedObject2 (NSKeyedArchiver archiver, NSObject obj);

	// not event args, should not be needed
	[Export<Method> ("archiverDidFinish:", Flags = ObjCBindings.Method.Event)]
	void Finished (NSKeyedArchiver archiver);

	// not event args, should not be needed
	[Export<Method> ("archiverDidFinish:", Flags = ObjCBindings.Method.Event, EventArgsType = typeof (NSObject))]
	void WillEncode (NSKeyedArchiver archiver, NSObject obj);

	// not event args, should not be needed
	[Export<Method> ("archiverWillFinish:", Flags = ObjCBindings.Method.Event)]
	void Finishing (NSKeyedArchiver archiver);

	// use a name, the class should be generated with the two NSObjects as properties
	[Export<Method> ("archiver:willReplaceObject:withObject:", Flags = ObjCBindings.Method.Event, EventArgsTypeName = "NSArchiveReplaceEventArgs")]
	void ReplacingObject (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);

	// use a typeof named tuple
	[Export<Method> ("archiver:willReplaceObject:withObject:", Flags = ObjCBindings.Method.Event, EventArgsType = typeof ((NSObject OldObject, NSObject NewObject)))]
	void ReplacingObject2 (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);

	// use a nameless tuple
	[Export<Method> ("archiver:willReplaceObject:withObject:", Flags = ObjCBindings.Method.Event, EventArgsType = typeof ((NSObject, NSObject)))]
	void ReplacingObject3 (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);

	// use a nameless tuple without declaring it
	[Export<Method> ("archiver:willReplaceObject:withObject:", Flags = ObjCBindings.Method.Event)]
	void ReplacingObject4 (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);
}

[BindingType<Class>]
public partial class EventTests {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("delegate",
		ArgumentSemantic.Weak,
		Flags = Property.WeakDelegate | Property.CreateEvents,
		StrongDelegateType = typeof (IRgenNSKeyedArchiverDelegate))]
	public virtual partial NSObject? WeakDelegate { get; set; }
}
