#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace PassKit {

	public partial class PKContactFieldsExtensions {

		/// <param name="set">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public PKContactFields GetValue (NSSet set)
		{
			if (set is null)
				return PKContactFields.None;
			return PKContactFieldsExtensions.ToFlags (set.ToArray<NSString> ());
		}

		/// <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public NSSet GetSet (PKContactFields values)
		{
			return new NSMutableSet (values.ToArray ());
		}
	}

	public partial class PKPaymentRequest {

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
#endif
		public PKContactFields RequiredBillingContactFields {
			get { return PKContactFieldsExtensions.GetValue (WeakRequiredBillingContactFields); }
			set { WeakRequiredBillingContactFields = PKContactFieldsExtensions.GetSet (value); }
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
#endif
		public PKContactFields RequiredShippingContactFields {
			get { return PKContactFieldsExtensions.GetValue (WeakRequiredShippingContactFields); }
			set { WeakRequiredShippingContactFields = PKContactFieldsExtensions.GetSet (value); }
		}
	}
}
