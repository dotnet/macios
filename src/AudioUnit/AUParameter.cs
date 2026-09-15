#nullable enable

namespace AudioUnit {
	public partial class AUParameter {
		/// <param name="value">The parameter value to represent as a string.</param>
		/// <summary>Returns the string representation of the parameter value that corresponds to <paramref name="value" />.</summary>
		/// <returns>A string representation of the specified parameter value, or the current value if <paramref name="value" /> is <see langword="null" />.</returns>
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
