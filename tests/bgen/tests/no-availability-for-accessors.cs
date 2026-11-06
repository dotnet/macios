using System;

using Foundation;
using ObjCRuntime;

namespace NS {
	[BaseType (typeof (NSObject))]
	interface Whatever : IProtocol {
		[Export ("propA")]
		NSObject PropA {
			get;
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			set;
		}

		[Export ("propB")]
		NSObject PropB {
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			get;
			set;
		}
	}

	[Protocol]
	interface IProtocol {
		[Abstract]
		[Export ("iPropA")]
		NSObject IPropA {
			get;
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			set;
		}

		[Abstract]
		[Export ("iPropB")]
		NSObject IPropB {
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			get;
			set;
		}

		[Export ("iPropAOpt")]
		NSObject IPropAOpt {
			get;
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			set;
		}

		[Export ("iPropBOpt")]
		NSObject IPropBOpt {
			[NoiOS, NoMac, NoTV, NoMacCatalyst]
			get;
			set;
		}
	}
}
