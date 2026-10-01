// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

#nullable enable

namespace Xamarin.Tests {

	public static class ResxAssertions {

		public static void AssertEntriesAreUniqueAndSorted (string path)
		{
			var document = XDocument.Load (path);
			var names = document.Root?.Elements ("data").Select (element => element.Attribute ("name")?.Value ?? "").ToArray () ?? [];
			var duplicates = names.GroupBy (name => name).Where (group => group.Count () > 1).Select (group => group.Key).ToArray ();

			Assert.That (duplicates, Is.Empty, $"{path} contains duplicate resource names.");

			int? previousNumber = null;
			string? previousName = null;
			foreach (var name in names) {
				if (!TryGetResourceNumber (name, out var number))
					continue;

				Assert.That (number, Is.GreaterThanOrEqualTo (previousNumber ?? number), $"{path} is not sorted: '{name}' must come before '{previousName}'.");
				previousNumber = number;
				previousName = name;
			}
		}

		static bool TryGetResourceNumber (string name, out int number)
		{
			var start = 0;
			while (start < name.Length && !char.IsDigit (name [start]))
				start++;

			var end = start;
			while (end < name.Length && char.IsDigit (name [end]))
				end++;

			return int.TryParse (name.Substring (start, end - start), NumberStyles.None, CultureInfo.InvariantCulture, out number);
		}
	}
}
