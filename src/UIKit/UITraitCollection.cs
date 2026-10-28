// 
// UITraitCollection.cs: support for UITraitCollection
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System.ComponentModel;

#nullable enable

namespace UIKit {
	public partial class UITraitCollection {
		/// <summary>Creates a <see cref="UITraitCollection" /> with the specified <see cref="UIContentSizeCategory" />.</summary>
		/// <param name="category">The preferred content size category for the trait collection.</param>
		/// <returns>A new <see cref="UITraitCollection" /> configured with the specified content size category.</returns>
		public static UITraitCollection Create (UIContentSizeCategory category)
			=> FromPreferredContentSizeCategory (category.GetConstant ()!);

#if !XAMCORE_5_0
		/// <summary>This method is obsolete and always throws <see cref="NotSupportedException" />.</summary>
		/// <param name="mutations">Unused.</param>
		/// <returns>This method never returns.</returns>
		/// <exception cref="NotSupportedException">Always thrown. Use the overload that takes a <c>UITraitMutations</c> parameter instead.</exception>
		[Obsolete ("Use the overload that takes a 'UITraitMutations' parameter instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
		public virtual UITraitCollection GetTraitCollectionByModifyingTraits (Func<IUIMutableTraits> mutations)
		{
			// there's nothing useful this method can do.
			throw new NotSupportedException ($"Use the overload that takes a 'UITraitMutations' parameter instead.");
		}
#endif
	}
}
