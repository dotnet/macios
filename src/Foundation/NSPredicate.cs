// Copyright 2014, Xamarin Inc. All rights reserved.

#if !COREBUILD

namespace Foundation {

	public partial class NSPredicate {

		/// <param name="predicateFormat">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSPredicate FromFormat (string predicateFormat)
		{
			return _FromFormat (predicateFormat, null);
		}

		// a single `nil` is a valid parameter, not to be confused with no parameters
		/// <param name="predicateFormat">To be added.</param>
		///         <param name="argument">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSPredicate FromFormat (string predicateFormat, NSObject argument)
		{
			return _FromFormat (predicateFormat, new NSObject [] { argument });
		}

		/// <param name="predicateFormat">To be added.</param>
		///         <param name="arguments">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSPredicate FromFormat (string predicateFormat, params NSObject [] arguments)
		{
			return _FromFormat (predicateFormat, arguments);
		}
	}
}

#endif
