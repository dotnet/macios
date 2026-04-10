#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace CoreMediaIO {

	/// <summary>Represents an AVC command to be sent to a CoreMediaIO device.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMIODeviceAVCCommand {
		IntPtr mCommand;
		uint mCommandLength;
		IntPtr mResponse;
		uint mResponseLength;
		uint mResponseUsed;

#if !COREBUILD
		/// <summary>Gets or sets a pointer to the buffer containing the AVC command bytes.</summary>
		public IntPtr Command {
			get => mCommand;
			set => mCommand = value;
		}

		/// <summary>Gets or sets the size (in bytes) of the command buffer.</summary>
		public uint CommandLength {
			get => mCommandLength;
			set => mCommandLength = value;
		}

		/// <summary>Gets or sets a pointer to the buffer for returning the response bytes.</summary>
		public IntPtr Response {
			get => mResponse;
			set => mResponse = value;
		}

		/// <summary>Gets or sets the size (in bytes) of the response buffer.</summary>
		public uint ResponseLength {
			get => mResponseLength;
			set => mResponseLength = value;
		}

		/// <summary>Gets the actual number of response bytes returned.</summary>
		public uint ResponseUsed {
			get => mResponseUsed;
		}
#endif // !COREBUILD
	}
}
