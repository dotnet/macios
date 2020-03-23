using System;
using System.IO;

using NUnit.Framework;

using System.Threading.Tasks;
using Xharness.BCLTestImporter;

namespace Xharness.Tests.BCLTestImporter.Tests {
	public class BCLTestInfoPlistGeneratorTest {

		[Test]
		public void GenerateCodeNullTemplateFile ()
		{
			Assert.ThrowsAsync<ArgumentNullException> (() => 
				BCLTestInfoPlistGenerator.GenerateCodeAsync (null, "Project Name"));
		}

		[Test]
		public void GenerateCodeNullProjectName ()
		{
			var tmp = Path.GetTempFileName ();
			Assert.ThrowsAsync <ArgumentNullException> (() =>
				BCLTestInfoPlistGenerator.GenerateCodeAsync (File.Create (tmp), null));
			File.Delete (tmp);
		}

		[Test]
		public async Task GenerateCode ()
		{
			const string projectName = "MyTest";
			var fakeTemplate = $"{BCLTestInfoPlistGenerator.ApplicationNameReplacement}-{BCLTestInfoPlistGenerator.IndentifierReplacement}";
			var tmpPath = Path.GetTempPath ();
			var templatePath = Path.Combine (tmpPath, Path.GetRandomFileName());
			using (var file = new StreamWriter (templatePath, false)) { 
				await file.WriteAsync (fakeTemplate);
			}

			var result = await BCLTestInfoPlistGenerator.GenerateCodeAsync (File.OpenRead (templatePath), projectName);
			try {
				StringAssert.DoesNotContain (BCLTestInfoPlistGenerator.ApplicationNameReplacement, result);
				StringAssert.DoesNotContain (BCLTestInfoPlistGenerator.IndentifierReplacement, result);
				StringAssert.Contains (projectName, result);
			}
			finally {
				File.Delete (templatePath);
			}
		}
	}
}
