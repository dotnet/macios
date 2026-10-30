// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;

namespace AudioToolbox {

	/// <summary>Create workgroup intervals for use with audio realtime threads.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class AudioWorkInterval {
#if !COREBUILD
		[DllImport (Constants.AudioToolboxLibrary)]
		static extern IntPtr AudioWorkIntervalCreate (IntPtr name, uint clock, IntPtr attr);

		/// <summary>Create a workgroup interval for use with audio realtime threads.</summary>
		/// <param name="name">The name of the workgroup interval.</param>
		/// <param name="clock">The clock used for the interval's timestamps.</param>
		/// <returns>The new workgroup interval, or <see langword="null" /> if it couldn't be created.</returns>
		public static OSWorkgroupInterval? Create (string name, OSClockId clock = OSClockId.MachAbsoluteTime)
		{
			ArgumentNullException.ThrowIfNull (name);
			using var namePtr = new TransientString (name, TransientString.Encoding.Ansi);
			var handle = AudioWorkIntervalCreate (namePtr, (uint) clock, IntPtr.Zero);
			return handle == IntPtr.Zero ? null : new OSWorkgroupInterval (handle, true);
		}
#endif // !COREBUILD
	}
}
