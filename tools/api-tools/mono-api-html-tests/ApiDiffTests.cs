// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

using NUnit.Framework;

using Mono.ApiTools;

namespace MonoApiHtmlTests {

	[TestFixture]
	public class ApiDiffTests {

		static string CreateApiInfo (string classes)
		{
			return $@"<assemblies>
  <assembly name=""Microsoft.macOS"" version=""0.0.0.0"">
    <namespaces>
      <namespace name=""Security"">
        <classes>{classes}</classes>
      </namespace>
    </namespaces>
  </assembly>
</assemblies>";
		}

		// A new type that implements generic interfaces used to leak the
		// %LESSERTHANREPLACEMENT% / %GREATERTHANREPLACEMENT% placeholders into the
		// generated output (see the MultiplexedFormatter raw Write path).
		[Test]
		public void GenericInterfacesDoNotLeakPlaceholders ()
		{
			var source = CreateApiInfo ("");
			var target = CreateApiInfo (@"
          <class name=""AuthorizationRights"" type=""class"" sealed=""true"" base=""ObjCRuntime.DisposableObject"">
            <interfaces>
              <interface name=""System.Collections.Generic.IEnumerable`1[Security.AuthorizationRight]"" />
              <interface name=""System.Collections.Generic.IReadOnlyCollection`1[Security.AuthorizationRight]"" />
            </interfaces>
          </class>");

			var sourceFile = Path.GetTempFileName ();
			var targetFile = Path.GetTempFileName ();
			var htmlFile = Path.GetTempFileName ();
			var markdownFile = Path.GetTempFileName ();
			try {
				File.WriteAllText (sourceFile, source);
				File.WriteAllText (targetFile, target);

				var config = new ApiDiffFormattedConfig {
					HtmlOutput = htmlFile,
					MarkdownOutput = markdownFile,
				};
				ApiDiffFormatted.Generate (sourceFile, targetFile, config);

				var html = File.ReadAllText (htmlFile);
				var markdown = File.ReadAllText (markdownFile);

				Assert.That (html, Does.Not.Contain ("%LESSERTHANREPLACEMENT%"), "html LesserThan placeholder");
				Assert.That (html, Does.Not.Contain ("%GREATERTHANREPLACEMENT%"), "html GreaterThan placeholder");
				Assert.That (markdown, Does.Not.Contain ("%LESSERTHANREPLACEMENT%"), "markdown LesserThan placeholder");
				Assert.That (markdown, Does.Not.Contain ("%GREATERTHANREPLACEMENT%"), "markdown GreaterThan placeholder");

				Assert.That (html, Does.Contain ("IEnumerable&lt;AuthorizationRight&gt;"), "html generic");
				Assert.That (markdown, Does.Contain ("IEnumerable<AuthorizationRight>"), "markdown generic");
			} finally {
				File.Delete (sourceFile);
				File.Delete (targetFile);
				File.Delete (htmlFile);
				File.Delete (markdownFile);
			}
		}
	}
}
