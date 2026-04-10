#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace CoreMediaIO {

	/// <summary>Represents an Audio Video Control (AVC) command to be sent to a CoreMediaIO device.</summary>
	/// <remarks>
	/// <para>The AVC protocol is used for device control over IEEE 1394 (FireWire) connections.</para>
	/// <para>The <see cref="Command" /> and <see cref="Response" /> fields point to caller-owned byte buffers.
	/// These buffers must remain valid for the duration of the native call that uses this struct.</para>
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[NativeName ("CMIODeviceAVCCommand")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMIODeviceAvcCommand {
		IntPtr mCommand;
		uint mCommandLength;
		IntPtr mResponse;
		uint mResponseLength;
		uint mResponseUsed;

#if !COREBUILD
		/// <summary>Gets or sets a pointer to the buffer containing the AVC command bytes to send.</summary>
		public IntPtr Command {
			get => mCommand;
			set => mCommand = value;
		}

		/// <summary>Gets or sets the size (in bytes) of the <see cref="Command" /> buffer.</summary>
		public uint CommandLength {
			get => mCommandLength;
			set => mCommandLength = value;
		}

		/// <summary>Gets or sets a pointer to the buffer that will receive the AVC response bytes.</summary>
		public IntPtr Response {
			get => mResponse;
			set => mResponse = value;
		}

		/// <summary>Gets or sets the size (in bytes) of the <see cref="Response" /> buffer.</summary>
		public uint ResponseLength {
			get => mResponseLength;
			set => mResponseLength = value;
		}

		/// <summary>Gets the actual number of response bytes written to the <see cref="Response" /> buffer.</summary>
		public uint ResponseUsed {
			get => mResponseUsed;
		}
#endif // !COREBUILD
	}
}
