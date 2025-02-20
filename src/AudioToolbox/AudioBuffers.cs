//
// AudioBuffers: AudioBufferList wrapper class
//
// Authors:
//   Miguel de Icaza
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2011, 2012 Xamarin Inc.
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace AudioToolbox {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CoreAudio.framework - CoreAudioTypes.h
	public class AudioBuffers : IDisposable, INativeObject {
		IntPtr address;
		readonly bool owns;

		/// <param name="address">Pointer to an existing C-based AudioBufferList.</param>
		///         <summary>Creates and AudioBuffers object that can be used to query and manipulate a native AudioBuffersList structure.</summary>
		///         <remarks>
		///         </remarks>
		public AudioBuffers (IntPtr address)
			: this (address, false)
		{
		}

		/// <param name="address">Pointer to an existing C-based AudioBufferList.</param>
		///         <param name="owns">Determines whether the user code owns the buffer pointed to by address, in that case, calling Dispose will release the buffer.</param>
		///         <summary>Creates and AudioBuffers object that can be used to query and manipulate a native AudioBuffersList structure.</summary>
		///         <summary>Creates and AudioBuffers object that can be used to query and manipulate a native AudioBuffersList structure.</summary>
		///         <remarks>
		/// 	  If you set owns to true, the structure pointed to by
		/// 	  "address" will be released when you call <see cref="M:AudioToolbox.AudioBuffers.Dispose(System.Boolean)" />.
		/// 	</remarks>
		public AudioBuffers (IntPtr address, bool owns)
		{
			if (address == IntPtr.Zero)
				throw new ArgumentException (nameof (address));

			this.address = address;
			this.owns = owns;
		}

		/// <param name="count">Number of buffers to create for this AudioBuffer.</param>
		///         <summary>Creates an AudioBuffers structure that can hold a fixed number of <see cref="T:AudioToolbox.AudioBuffer" /> structures.</summary>
		///         <remarks>
		/// 	  The allocated structure will be released when you call <see cref="M:AudioToolbox.AudioBuffers.Dispose(System.Boolean)" />.
		/// 	</remarks>
		public unsafe AudioBuffers (int count)
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException (nameof (count));

			//
			// AudioBufferList is a int + array of AudioBuffer (int + int + intptr).
			// However due to alignment, the array of AudioBuffer comes at position 8
			// in 64bit architectures, which is why we're using IntPtr.Size here
			// in order to calculate the total size / position of the AudioBuffer elements.
			//

			var size = IntPtr.Size + count * sizeof (AudioBuffer);
			address = Marshal.AllocHGlobal (size);
			owns = true;

			Marshal.WriteInt32 (address, 0, count);
			AudioBuffer* ptr = (AudioBuffer*) (((byte*) address) + IntPtr.Size);
			for (int i = 0; i < count; i++) {
				ptr->NumberChannels = 0;
				ptr->DataByteSize = 0;
				ptr->Data = IntPtr.Zero;
				ptr++;
			}
		}

		~AudioBuffers ()
		{
			Dispose (false);
		}

		/// <summary>Returns the number of buffers managed.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public unsafe int Count {
			get {
				return *(int*) address;
			}
		}

		public AudioBuffer this [int index] {
			get {
				if (index >= Count)
					throw new ArgumentOutOfRangeException (nameof (index));

				//
				// Decodes
				//
				// struct AudioBufferList
				// {
				//    UInt32      mNumberBuffers;
				//    AudioBuffer mBuffers[1]; // this is a variable length array of mNumberBuffers elements
				// }
				//
				unsafe {
					byte* baddress = (byte*) address;

					var ptr = baddress + IntPtr.Size + index * sizeof (AudioBuffer);
					return *(AudioBuffer*) ptr;
				}
			}
			set {
				if (index >= Count)
					throw new ArgumentOutOfRangeException (nameof (index));

				unsafe {
					byte* baddress = (byte*) address;
					var ptr = (AudioBuffer*) (baddress + IntPtr.Size + index * sizeof (AudioBuffer));
					*ptr = value;
				}
			}
		}

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public NativeHandle Handle {
			get { return address; }
		}

		public static explicit operator IntPtr (AudioBuffers audioBuffers)
		{
			return audioBuffers.address;
		}

		/// <param name="index">Index of the buffer to access.</param>
		///         <param name="data">Pointer to the data to set for the specified buffer.</param>
		///         <summary>Sets the data buffer for one of the audio buffers, without updating the buffer size.</summary>
		///         <remarks>
		///           <para>
		/// 	    You can use this method to swap out one of the buffers, without updating the size of the buffer. 
		/// 	  </para>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// //
		/// // Creating an AudioBuffers structure 
		/// //
		/// AudioBuffers SetupBuffers (int n = 2, int size = 4096)
		/// {
		///     var buffers = new AudioBuffers (n);
		///     for (int i = 0; i < n; i++)
		/// 	buffers.SetData (i, MyBuffers [0][i], size);
		///     return buffers;
		/// }
		///
		/// //
		/// // Swap the buffers
		/// //
		/// void SwapBuffers (AudioBuffers buffers, int bufferGroup)
		/// {
		///     for (int i = 0; i < buffers.Count; i++)
		///     	buffers.SetData (i, MyBuffers [bufferGroup][i]);
		/// }]]></code>
		///           </example>
		///         </remarks>
		public void SetData (int index, IntPtr data)
		{
			if (index >= Count)
				throw new ArgumentOutOfRangeException (nameof (index));

			unsafe {
				byte* baddress = (byte*) address;
				var ptr = (IntPtr*) (baddress + IntPtr.Size + index * sizeof (AudioBuffer) + sizeof (int) + sizeof (int));
				*ptr = data;
			}
		}

		/// <param name="index">Index of the buffer to access.</param>
		///         <param name="data">Pointer to the data to set for the specified buffer.</param>
		///         <param name="dataByteSize">Size of the buffer.</param>
		///         <summary>Sets the data buffer for one of the audio buffers.</summary>
		///         <remarks>
		///           <example>
		///             <code lang="c#"><![CDATA[
		/// //
		/// // Creating an AudioBuffers structure 
		/// //
		/// AudioBuffers SetupBuffers (int n = 2, int size = 4096)
		/// {
		///     var buffers = new AudioBuffers (n);
		///     for (int i = 0; i < n; i++){
		///         var buffer = Marshal.AllocHGlobal (size);
		/// 	buffers.SetData (i, buffer, size);
		///     }
		///     return buffers;
		/// }
		///
		/// void ReleaseBuffers (AudioBuffers buffers)
		/// {
		///     for (int i = 0; i < buffers.Count; i++){
		///         var buf = buffers [i];
		///         Marshal.ReleaseHGlobal (buf.Data);                
		///     }
		///     buffers.Dispose ();
		/// }]]></code>
		///           </example>
		///         </remarks>
		public void SetData (int index, IntPtr data, int dataByteSize)
		{
			if (index >= Count)
				throw new ArgumentOutOfRangeException (nameof (index));

			unsafe {
				byte* baddress = (byte*) address;
				var ptr = (int*) (baddress + IntPtr.Size + index * sizeof (AudioBuffer) + sizeof (int));
				*ptr = dataByteSize;
				ptr++;
				IntPtr* iptr = (IntPtr*) ptr;
				*iptr = data;
			}
		}

		/// <summary>Releases the resources used by the AudioBuffers object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioBuffers class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioBuffers ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/AudioToolbox/AudioBuffers.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioBuffers.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (owns && address != IntPtr.Zero) {
				Marshal.FreeHGlobal (address);
				address = IntPtr.Zero;
			}
		}
	}
}
