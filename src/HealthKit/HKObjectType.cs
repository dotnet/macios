//
// HKObjectType.cs: methods that live technically in HKObjectType, but we are going to put
// in classes where they make more sense (they are factory methods on HKObejctType, taking
// strings, we are going to add factory methods in the relevant classes, that tkae the kind
// you want
//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com
//
// Copyright 2014-2015 Xamarin
//

#nullable enable

using System;
using Foundation;
namespace HealthKit {
#pragma warning disable CS0618 // Type or member is obsolete
	public partial class HKQuantityType {
		/// <param name="kind">To be added.</param>
		///         <summary>Creates and returns a quantity type for the specified identifier.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static HKQuantityType? Create (HKQuantityTypeIdentifier kind)
		{
			return HKObjectType.GetQuantityType (kind.GetConstant ()!);
		}
	}

	public partial class HKCategoryType {
		/// <param name="kind">To be added.</param>
		///         <summary>Creates and returns a <see cref="HealthKit.HKCategoryType" /> object of the specified <paramref name="kind" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static HKCategoryType? Create (HKCategoryTypeIdentifier kind)
		{
			return HKObjectType.GetCategoryType (kind.GetConstant ()!);
		}
	}

	public partial class HKCharacteristicType {
		/// <param name="kind">To be added.</param>
		///         <summary>Creates and returns a <see cref="HealthKit.HKCharacteristicType" /> for the specified <paramref name="kind" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static HKCharacteristicType? Create (HKCharacteristicTypeIdentifier kind)
		{
			return HKObjectType.GetCharacteristicType (kind.GetConstant ()!);
		}
	}

	public partial class HKCorrelationType {
		/// <param name="kind">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static HKCorrelationType? Create (HKCorrelationTypeIdentifier kind)
		{
			return HKObjectType.GetCorrelationType (kind.GetConstant ()!);
		}
	}
#pragma warning restore CS0618 // Type or member is obsolete

	public partial class HKDocumentType {
		/// <param name="kind">To be added.</param>
		///         <summary>Creates a new <see cref="HealthKit.HKDocumentType" /> from the specified type identifier.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static HKDocumentType? Create (HKDocumentTypeIdentifier kind)
		{
			var constant = kind.GetConstant ();
			if (constant is not null)
				return HKObjectType._GetDocumentType (constant);
			return null;
		}
	}
}
