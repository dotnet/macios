using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace MapKit {

	/// <summary>This enum is used to select how to initialize a new instance of a <see cref="MKPointOfInterestFilter" />.</summary>
	public enum MKPointOfInterestFilterType {
		/// <summary>The specified categories are included.</summary>
		Including,
		/// <summary>The specified categories are excluded.</summary>
		Excluding,
	}

	public partial class MKPointOfInterestFilter {
		/// <summary>Create a new <see cref="MKPointOfInterestFilter" /> instance.</summary>
		/// <param name="categories">An array of categories for the filter to include.</param>
		public MKPointOfInterestFilter (MKPointOfInterestCategory [] categories) : this (categories, MKPointOfInterestFilterType.Including)
		{
		}

		/// <summary>Create a new <see cref="MKPointOfInterestFilter" /> instance.</summary>
		/// <param name="categories">An array of categories for the filter to include or exclude.</param>
		/// <param name="type">Specify whether <paramref name="categories" /> are included in or excluded from the filter.</param>
		public MKPointOfInterestFilter (MKPointOfInterestCategory [] categories, MKPointOfInterestFilterType type)
			: base (NSObjectFlag.Empty)
		{
			// two different `init*` would share the same C# signature
			switch (type) {
			case MKPointOfInterestFilterType.Including:
				InitializeHandle (_InitIncludingCategories (categories), "initExcludingCategories:");
				break;
			case MKPointOfInterestFilterType.Excluding:
				InitializeHandle (_InitExcludingCategories (categories), "initIncludingCategories:");
				break;
			default:
				throw new ArgumentException (nameof (type));
			}
		}
	}
}
