// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;

namespace Test {
	[BaseType (typeof (NSObject), Delegates = new [] { "WeakDelegate" }, Events = new [] { typeof (LegacyEventDelegate) })]
	interface EventSource {
		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		LegacyEventDelegate Delegate { get; set; }
	}

	[Model]
	[BaseType (typeof (NSObject))]
	interface LegacyEventDelegate {
		[Obsolete ("Use another callback.")]
		[Export ("legacyCallback:")]
		void LegacyCallback (EventSource sender);

		[Obsolete ("Use another callback.", true)]
		[Export ("legacyDecision:")]
		[DelegateName ("LegacyDecisionHandler"), DefaultValue (false)]
		bool LegacyDecision (EventSource sender);

		[Export ("currentCallback:")]
		void CurrentCallback (EventSource sender);
	}
}
