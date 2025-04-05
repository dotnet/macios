#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace AudioUnit {
	public partial class AUParameter {
		/// <param name="value">The parameter value to represent as a string.</param>
		///         <summary>Returns the string representation of the parameter value that corresponds to <paramref name="value" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public string GetString (float? value)
		{
			unsafe {
				if (value is not null) {
					float f = value.Value;
					return this._GetString (new IntPtr (&f));
				} else {
					return this._GetString (IntPtr.Zero);
				}
			}
		}
	}
}
