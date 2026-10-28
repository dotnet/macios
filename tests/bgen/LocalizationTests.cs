// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml.Linq;

using Xamarin.Tests;

namespace GeneratorTests {
	[TestFixture]
	public class LocalizationTests {
		[TestCase ("cs", "cs-CZ")]
		[TestCase ("de", "de-DE")]
		[TestCase ("es", "es-ES")]
		[TestCase ("fr", "fr-FR")]
		[TestCase ("it", "it-IT")]
		[TestCase ("ja", "ja-JP")]
		[TestCase ("ko", "ko-KR")]
		[TestCase ("pl", "pl-PL")]
		[TestCase ("pt-BR", "pt-BR")]
		[TestCase ("ru", "ru-RU")]
		[TestCase ("tr", "tr-TR")]
		[TestCase ("zh-Hans", "zh-CN")]
		[TestCase ("zh-Hant", "zh-TW")]
		public void SatelliteResources (string language, string cultureName)
		{
			var manager = new ResourceManager ("bgen.Resources", typeof (BindingTouch).Assembly);
			var culture = CultureInfo.GetCultureInfo (cultureName);
			var source = ReadResources (Path.Combine (Configuration.SourceRoot, "src", "Resources.resx"));
			var entries = ReadResources (Path.Combine (Configuration.SourceRoot, "src", "TranslatedAssemblies", $"Resources.{language}.resx"));

			foreach (var platform in Configuration.GetIncludedPlatforms ()) {
				var satellite = Path.Combine (Configuration.GetSdkRoot (platform), "lib", "bgen", language, "bgen.resources.dll");
				Assert.That (satellite, Does.Exist, $"Installed satellite: {platform}: {language}");
			}

			using var resources = manager.GetResourceSet (CultureInfo.GetCultureInfo (language), true, false)
				?? throw new InvalidOperationException ($"Missing bgen satellite resources for {language}.");

			foreach (var entry in entries) {
				Assert.That (resources.GetString (entry.Key), Is.EqualTo (entry.Value), $"{language}: {entry.Key}");
				Assert.That (manager.GetString (entry.Key, culture), Is.EqualTo (entry.Value), $"{cultureName}: {entry.Key}");
			}

			foreach (var entry in source) {
				if (!entries.ContainsKey (entry.Key))
					Assert.That (manager.GetString (entry.Key, culture), Is.EqualTo (entry.Value), $"English fallback: {cultureName}: {entry.Key}");
			}
		}

		static Dictionary<string, string> ReadResources (string path)
		{
			return XDocument.Load (path).Descendants ("data").ToDictionary (
				v => v.Attribute ("name")?.Value ?? throw new InvalidOperationException ($"Missing resource name in {path}."),
				v => v.Element ("value")?.Value ?? throw new InvalidOperationException ($"Missing resource value in {path}."));
		}
	}
}
