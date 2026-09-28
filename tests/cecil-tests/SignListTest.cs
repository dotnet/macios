// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using NUnit.Framework;

using Xamarin.Tests;

#nullable enable

namespace Cecil.Tests {

	[TestFixture]
	public class SignListTest {
		const int MaxZipDepth = 8;

		static readonly string [] signingGroups = { "Skip", "FirstParty", "ThirdParty", "MacDeveloperSign" };

		[Test]
		public void WorkloadPackagesHaveSigningEntries ()
		{
			var root = Configuration.SourceRoot;
			var signList = Path.Combine (root, "dotnet", "Workloads", "SignList.xml");
			var packageDir = Environment.GetEnvironmentVariable ("MACIOS_SIGNLIST_NUPKG_DIR");
			if (string.IsNullOrEmpty (packageDir))
				packageDir = Path.Combine (root, "_build", "nupkgs");

			Assert.That (Directory.Exists (packageDir), Is.True, $"Package directory '{packageDir}' does not exist. Build the workload packages or set MACIOS_SIGNLIST_NUPKG_DIR to a directory containing them.");
			var packages = Directory.GetFiles (packageDir, "*.nupkg", SearchOption.TopDirectoryOnly);
			Assert.That (packages, Is.Not.Empty, $"No nupkgs found in '{packageDir}'.");

			var patterns = ReadPatterns (signList);
			var missing = new List<string> ();
			foreach (var package in packages) {
				using var archive = ZipFile.OpenRead (package);
				CheckArchive (archive, Path.GetFileName (package), "", patterns, missing, 0);
			}
			Assert.That (missing, Is.Empty, $"Files missing from {signList}:\n{string.Join ("\n", missing)}");
		}

		static List<Regex> ReadPatterns (string signList)
		{
			var document = XDocument.Load (signList);
			return ReadPatterns (document, signList);
		}

		static List<Regex> ReadPatterns (XDocument document, string signList)
		{
			var patterns = new List<Regex> ();
			foreach (var group in signingGroups) {
				foreach (var item in document.Descendants (group)) {
					var include = (string?) item.Attribute ("Include");
					Assert.That (include, Is.Not.Null.And.Not.Empty, $"{group} entry without Include in {signList}");
					Assert.That (include, Does.Not.Contain ("/"), $"Use Windows-style separators in {signList}: {include}");
					var expression = Regex.Escape (include!).Replace (@"\*", ".*").Replace (@"\?", ".");
					patterns.Add (new Regex ($"(^|\\\\){expression}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
				}
			}
			Assert.That (patterns, Is.Not.Empty, $"No signing entries found in {signList}");
			return patterns;
		}

		static void CheckArchive (ZipArchive archive, string archiveName, string zipPath, IReadOnlyList<Regex> patterns, List<string> missing, int depth)
		{
			foreach (var entry in archive.Entries) {
				if (entry.Name.Length == 0)
					continue;

				var name = entry.FullName.Replace ('/', '\\');
				if (entry.Name.EndsWith (".zip", StringComparison.OrdinalIgnoreCase)) {
					Assert.That (depth, Is.LessThan (MaxZipDepth), $"Nested ZIP depth exceeds {MaxZipDepth}: {archiveName}!{zipPath}{entry.FullName}");
					using var stream = entry.Open ();
					using var nested = new ZipArchive (stream, ZipArchiveMode.Read);
					CheckArchive (nested, archiveName, zipPath + entry.FullName + "!", patterns, missing, depth + 1);
				} else if (name.EndsWith (".dll", StringComparison.OrdinalIgnoreCase)
					|| name.EndsWith (".exe", StringComparison.OrdinalIgnoreCase)
					|| name.EndsWith (".dylib", StringComparison.OrdinalIgnoreCase)) {
					if (!patterns.Any (pattern => pattern.IsMatch (name)))
						missing.Add ($"{archiveName}!{zipPath}{entry.FullName}");
				}
			}
		}

		[Test]
		public void SigningPatternAndNestedZipRegression ()
		{
			var patterns = ReadPatterns (XDocument.Parse ("""
				<Project><ItemGroup>
				  <FirstParty Include="System.*.dll" />
				  <FirstParty Include="tools\msbuild\net11.0\ILLink.Tasks.dll" />
				</ItemGroup></Project>
				"""), "test.xml");
			using var innerStream = new MemoryStream ();
			using (var inner = new ZipArchive (innerStream, ZipArchiveMode.Create, true)) {
				inner.CreateEntry ("MonoBundle/System.Drawing.dll");
				inner.CreateEntry ("MonoBundle/Missing.dylib");
			}
			innerStream.Position = 0;
			using var outerStream = new MemoryStream ();
			using (var outer = new ZipArchive (outerStream, ZipArchiveMode.Create, true)) {
				outer.CreateEntry ("tools/msbuild/net11.0/ILLink.Tasks.dll");
				using var nested = outer.CreateEntry ("app.zip").Open ();
				innerStream.CopyTo (nested);
			}
			outerStream.Position = 0;
			using var package = new ZipArchive (outerStream, ZipArchiveMode.Read);
			var missing = new List<string> ();
			CheckArchive (package, "test.nupkg", "", patterns, missing, 0);
			Assert.That (missing, Is.EqualTo (new [] { "test.nupkg!app.zip!MonoBundle/Missing.dylib" }));
		}

		[Test]
		public void RejectUnixSeparatorsInSignList ()
		{
			var document = XDocument.Parse ("""<Project><ItemGroup><FirstParty Include="tools/msbuild/Build.dll" /></ItemGroup></Project>""");
			Assert.Throws<AssertionException> (() => ReadPatterns (document, "test.xml"));
		}

		[TestCase (MaxZipDepth, false)]
		[TestCase (MaxZipDepth + 1, true)]
		public void NestedZipDepth (int zipDepth, bool exceedsLimit)
		{
			byte [] contents;
			using (var stream = new MemoryStream ()) {
				using (var archive = new ZipArchive (stream, ZipArchiveMode.Create, true))
					archive.CreateEntry ("Signed.dll");
				contents = stream.ToArray ();
			}
			for (var i = 0; i < zipDepth; i++) {
				using var stream = new MemoryStream ();
				using (var archive = new ZipArchive (stream, ZipArchiveMode.Create, true)) {
					using var entry = archive.CreateEntry ("nested.zip").Open ();
					entry.Write (contents);
				}
				contents = stream.ToArray ();
			}

			using var packageStream = new MemoryStream (contents);
			using var package = new ZipArchive (packageStream, ZipArchiveMode.Read);
			var patterns = ReadPatterns (XDocument.Parse ("""<Project><ItemGroup><FirstParty Include="Signed.dll" /></ItemGroup></Project>"""), "test.xml");
			var missing = new List<string> ();
			if (exceedsLimit) {
				Assert.Throws<AssertionException> (() => CheckArchive (package, "test.nupkg", "", patterns, missing, 0));
			} else {
				CheckArchive (package, "test.nupkg", "", patterns, missing, 0);
				Assert.That (missing, Is.Empty);
			}
		}
	}
}
