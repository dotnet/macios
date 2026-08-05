#nullable enable

#if !__MACCATALYST__
using System.Threading.Tasks;

namespace VideoSubscriberAccount {

	/// <summary>Provides conversions between authentication scheme values and their native constants.</summary>
	public static partial class VSAccountProviderAuthenticationSchemeExtensions {

		// these are less common pattern so it's not automatically generated

		/// <summary>Gets the native constants for the specified authentication schemes.</summary>
		/// <param name="self">The authentication schemes to convert.</param>
		/// <returns>The native constant for each authentication scheme.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="self" /> is <see langword="null" />.</exception>
		public static NSString? [] GetConstants (this VSAccountProviderAuthenticationScheme [] self)
		{
			if (self is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (self));

			var array = new NSString? [self.Length];
			for (int n = 0; n < self.Length; n++)
				array [n] = self [n].GetConstant ();
			return array;
		}

		/// <summary>Gets the authentication schemes for the specified native constants.</summary>
		/// <param name="constants">The native constants to convert.</param>
		/// <returns>The authentication scheme for each native constant.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="constants" /> is <see langword="null" />.</exception>
		public static VSAccountProviderAuthenticationScheme [] GetValues (NSString [] constants)
		{
			if (constants is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (constants));

			var array = new VSAccountProviderAuthenticationScheme [constants.Length];
			for (int n = 0; n < constants.Length; n++)
				array [n] = GetValue (constants [n]);
			return array;
		}
	}
}
#endif // !__MACCATALYST__
