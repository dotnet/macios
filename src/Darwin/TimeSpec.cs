using System;
using System.Runtime.InteropServices;

using ObjCRuntime;

#nullable enable

namespace Darwin {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("timespec")]
	public struct TimeSpec {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Seconds;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint NanoSeconds;
	}
}
