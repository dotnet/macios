
#nullable enable

namespace Darwin {
	/// <summary>Represents a POSIX time value with nanosecond precision.</summary>
	[StructLayout (LayoutKind.Sequential)]
	[NativeName ("timespec")]
	public struct TimeSpec {
		/// <summary>The number of whole seconds.</summary>
		public nint Seconds;
		/// <summary>The additional number of nanoseconds.</summary>
		public nint NanoSeconds;
	}
}
