using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface UnsupportedOnAllSimulators {
	}

	[UnsupportedSimulator ("ios")]
	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface UnsupportedOnIosSimulatorOnly {
	}

	[SupportedSimulator ("ios17.0")]
	[SupportedSimulator ("tvos17.0")]
	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface SupportedOnSimulatorFromVersion {
	}

	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface NoSimulatorAttributes {
	}

	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface SimulatorAvailabilityMethods {
		[UnsupportedSimulator ("ios")]
		[UnsupportedSimulator ("tvos")]
		[Export ("unsupported")]
		void Unsupported ();

		[SupportedSimulator ("ios17.0")]
		[SupportedSimulator ("tvos17.0")]
		[Export ("supported")]
		void Supported ();

		[Export ("plain")]
		void Plain ();
	}

	// A simulator attribute placed on a smart-enum [Field] member must be propagated
	// to the generated *Extensions field accessor (e.g. CMSampleBufferAttachmentKey.Hdr10PlusPerFrameData).
	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	enum SmartEnumWithSimulatorField {
		[SupportedSimulator ("ios17.0")]
		[SupportedSimulator ("tvos17.0")]
		[Field ("SupportedSmartField", "__Internal")]
		Supported,

		[Field ("PlainSmartField", "__Internal")]
		Plain,
	}
}
