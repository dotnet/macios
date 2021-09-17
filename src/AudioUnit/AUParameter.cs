using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace AudioUnit
{
#if !NET
	[iOS (9,0), Mac(10,11)]
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

		public void SetValue (float value, AUParameterObserverToken originator)
		{
			SetValue (value, originator.ObserverToken);
		}

		public void SetValue (float value, AUParameterObserverToken originator, ulong hostTime)
		{
			SetValue (value, originator.ObserverToken, hostTime);
		}
	}
}
