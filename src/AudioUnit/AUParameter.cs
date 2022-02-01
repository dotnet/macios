using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace AudioUnit
{
#if NET
	[SupportedOSPlatform ("ios9.0")]
	[SupportedOSPlatform ("macos10.11")]
#else
	[iOS (9,0)]
	[Mac (10,11)]
#endif
	public partial class AUParameter
	{
		public string GetString (float? value)
		{
			unsafe {
				if (value != null && value.HasValue) {
					float f = value.Value;
					return this._GetString (new IntPtr (&f));
				}
				else {
					return this._GetString (IntPtr.Zero);
				}
			}
		}
	}
}
