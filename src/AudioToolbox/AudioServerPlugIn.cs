// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACCATALYST__

namespace AudioToolbox {

	/// <summary>Provides registration support for Core Audio media device extensions.</summary>
	[SupportedOSPlatform ("maccatalyst27.0")]
	public static class AudioServerPlugIn {

		[SupportedOSPlatform ("maccatalyst27.0")]
		[DllImport (Constants.CoreAudioLibrary)]
		unsafe static extern OSStatus AudioServerPlugInRegisterMediaDeviceExtension (IntPtr inPlugIn, BlockLiteral* interruptionHandler);

		/// <summary>Registers an audio server plug-in that exposes a remote media output device.</summary>
		/// <param name="plugIn">The opaque native <c>AudioServerPlugInDriverRef</c> value, or <see cref="IntPtr.Zero" />.</param>
		/// <param name="interruptionHandler">A callback invoked when the connection to the audio server is interrupted, or <see langword="null" />.</param>
		/// <returns>An <c>OSStatus</c> value, where zero indicates success.</returns>
		/// <remarks>
		/// <para>The plug-in may expose a single output audio device whose transport type is remote screen or remote streaming.</para>
		/// <para>The caller is responsible for keeping the native plug-in interface and pointer storage valid for the duration of the registration.</para>
		/// </remarks>
		[SupportedOSPlatform ("maccatalyst27.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static OSStatus RegisterMediaDeviceExtension (IntPtr plugIn, Action? interruptionHandler)
		{
			unsafe {
				if (interruptionHandler is null)
					return AudioServerPlugInRegisterMediaDeviceExtension (plugIn, null);

				using var block = BlockStaticDispatchClass.CreateBlock (interruptionHandler);
				return AudioServerPlugInRegisterMediaDeviceExtension (plugIn, &block);
			}
		}
	}
}

#endif // __MACCATALYST__
