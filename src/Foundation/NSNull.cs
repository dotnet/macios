
#nullable enable

namespace Foundation {
	public partial class NSNull {

		static NSNull? _null;

		/// <summary>
		/// Gets the singleton <see cref="NSNull"/> instance.
		/// </summary>
		/// <value>The singleton <see cref="NSNull"/> instance representing a null value in Objective-C collections.</value>
		/// <remarks>
		/// This property caches the native <see cref="NSNull"/> instance to avoid repeated native calls.
		/// </remarks>
		static public NSNull Null {
			get {
				if (_null is null)
					_null = _Null;
				return _null;
			}
		}
	}
}
