//
// Copyright 2011, Kenneth J. Pouncey
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

namespace ScriptingBridge {
	// AE.framework/Headers/AEDataModel.h:typedef SInt32 AESendMode;
	[Flags]
	public enum AESendMode : int {
		/// <summary>To be added.</summary>
		NoReply = 0x00000001,
		/// <summary>To be added.</summary>
		QueueReply = 0x00000002,
		/// <summary>To be added.</summary>
		WaitReply = 0x00000003,
		/// <summary>To be added.</summary>
		DontReconnect = 0x00000080,
		/// <summary>To be added.</summary>
		WantReceipt = 0x00000200,
		/// <summary>To be added.</summary>
		NeverInteract = 0x00000010,
		/// <summary>To be added.</summary>
		CanInteract = 0x00000020,
		/// <summary>To be added.</summary>
		AlwaysInteract = 0x00000030,
		/// <summary>To be added.</summary>
		CanSwitchLayer = 0x00000040,
		/// <summary>To be added.</summary>
		DontRecord = 0x00001000,
		/// <summary>To be added.</summary>
		DontExecute = 0x00002000,
		/// <summary>To be added.</summary>
		ProcessNonReplyEvents = 0x00008000,
	}

	// LaunchServices.framework/Headers/LSOpen.h:typedef OptionBits LSLaunchFlags;
	// DirectoryService.framework/Headers/DirServicesTypes.h:typedef UInt32 OptionBits;
	[Flags]
	public enum LSLaunchFlags : uint {
		/// <summary>To be added.</summary>
		LaunchDefaults = 0x00000001,
		/// <summary>To be added.</summary>
		LaunchAndPrint = 0x00000002,
		/// <summary>To be added.</summary>
		LaunchReserved2 = 0x00000004,
		/// <summary>To be added.</summary>
		LaunchReserved3 = 0x00000008,
		/// <summary>To be added.</summary>
		LaunchReserved4 = 0x00000010,
		/// <summary>To be added.</summary>
		LaunchReserved5 = 0x00000020,
		/// <summary>To be added.</summary>
		LaunchAndDisplayErrors = 0x00000040,
		/// <summary>To be added.</summary>
		LaunchInhibitBGOnly = 0x00000080,
		/// <summary>To be added.</summary>
		LaunchDontAddToRecents = 0x00000100,
		/// <summary>To be added.</summary>
		LaunchDontSwitch = 0x00000200,
		/// <summary>To be added.</summary>
		LaunchNoParams = 0x00000800,
		/// <summary>To be added.</summary>
		LaunchAsync = 0x00010000,
		/// <summary>To be added.</summary>
		LaunchStartClassic = 0x00020000,
		/// <summary>To be added.</summary>
		LaunchInClassic = 0x00040000,
		/// <summary>To be added.</summary>
		LaunchNewInstance = 0x00080000,
		/// <summary>To be added.</summary>
		LaunchAndHide = 0x00100000,
		/// <summary>To be added.</summary>
		LaunchAndHideOthers = 0x00200000,
		/// <summary>To be added.</summary>
		LaunchHasUntrustedContents = 0x00400000,
	}

}
