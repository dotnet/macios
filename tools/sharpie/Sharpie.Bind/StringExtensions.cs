// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public static class StringExtensions {
	/// <summary>
	/// Returns <paramref name="str"/> with its first character in
	/// upper case form using <see cref="System.Char.ToUpper"/>
	/// ("foo" becomes "Foo")
	/// </summary>
	[return: NotNullIfNotNull (nameof (str))]
	public static string? UCFirst (this string? str)
	{
		if (String.IsNullOrWhiteSpace (str))
			return str;

		return Char.ToUpperInvariant (str [0]) + str.Substring (1);
	}

	/// <summary>
	/// Returns a camel cased string if the given <paramref name="str"/> is
	/// of the form "UPPER_CASED_STRING" (resulting in "UpperCasedString").
	/// </summary>
	[return: NotNullIfNotNull (nameof (str))]
	public static string? ToCamelCaseFromUnderscoreUpperCase (this string? str)
	{
		if (String.IsNullOrEmpty (str))
			return str;

		var builder = new StringBuilder ();
		var partIndex = 0;

		foreach (var c in str) {
			if (Char.IsDigit (c) || Char.IsUpper (c))
				builder.Append (builder.Length == partIndex ? c : Char.ToLower (c));
			else if (c == '_')
				partIndex = builder.Length;
			else
				return str;
		}

		return builder.ToString ();
	}

	/// <summary>
	/// Returns the shared prefix string of a set of strings <paramref name="strings"/>.
	/// Given "Foo", "Fool", "Foster", returns "Fo".
	/// </summary>
	[return: NotNullIfNotNull (nameof (strings))]
	public static string? SharedPrefix (this IEnumerable<string>? strings)
	{
		return strings is null ? null : SharedPrefix (strings.ToArray ());
	}

	/// <summary>
	/// Returns the shared prefix string of a set of strings <paramref name="strings"/>.
	/// Given "Foo", "Fool", "Foster", returns "Fo".
	/// </summary>
	[return: NotNullIfNotNull (nameof (strings))]
	public static string? SharedPrefix (this string []? strings)
	{
		if (strings is null)
			return null;

		if (strings.Length == 0)
			return "";

		var first = strings [0];
		if (String.IsNullOrEmpty (first))
			return "";

		for (int length = 0; length < first.Length; length++) {
			var c = first [length];
			for (int i = 1; i < strings.Length; i++) {
				if (strings [i] is not null && (
					length >= strings [i].Length ||
					strings [i] [length] != c))
					return length == 0 ? "" : strings [i].Substring (0, length);
			}
		}

		return first;
	}
}
