#nullable enable

#if !__MACCATALYST__
using System;
using System.Threading.Tasks;
using Foundation;

namespace VideoSubscriberAccount {

	public static partial class VSAccountProviderAuthenticationSchemeExtensions {

		// these are less common pattern so it's not automatically generated

		/// <param name="self">The instance on which this method operates.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString? [] GetConstants (this VSAccountProviderAuthenticationScheme [] self)
		{
			if (self is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (self));

			var array = new NSString? [self.Length];
			for (int n = 0; n < self.Length; n++)
				array [n] = self [n].GetConstant ();
			return array;
		}

		/// <param name="constants">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
