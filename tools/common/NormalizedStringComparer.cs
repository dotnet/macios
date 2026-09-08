// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Xamarin.Bundler;

#nullable enable

public sealed class NormalizedStringComparer : IEqualityComparer<string> {
	public static readonly NormalizedStringComparer OrdinalIgnoreCase = new NormalizedStringComparer (StringComparer.OrdinalIgnoreCase);

	StringComparer comparer;

	public NormalizedStringComparer (StringComparer comparer)
	{
		this.comparer = comparer;
	}

	public bool Equals (string? x, string? y)
	{
		// From what I gather it doesn't matter which normalization form
		// is used, but I chose Form D because HFS normalizes to Form D.
		if (x is not null)
			x = x.Normalize (System.Text.NormalizationForm.FormD);
		if (y is not null)
			y = y.Normalize (System.Text.NormalizationForm.FormD);
		return comparer.Equals (x, y);
	}

	public int GetHashCode (string obj)
	{
		return comparer.GetHashCode (obj.Normalize (System.Text.NormalizationForm.FormD));
	}
}
