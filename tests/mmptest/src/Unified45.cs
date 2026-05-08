using System;
using System.IO;
using NUnit.Framework;
using Xamarin.MMP.Tests;
using Xamarin.Bundler;
using System.Text;
using Xamarin.Tests;

namespace MonoTouchFixtures.Net45 {
	[TestFixture]
	public class Unified45 {
		[Test]
		public void ProtobufShouldSerializeAndDeserialize ()
		{
			var testFolder = Path.Combine (Configuration.TestProjectsDirectory, "Protobuf_Test", "Protobuf_Test");
			var testResults = testFolder + "/TestResult.txt";
			if (File.Exists (testResults))
				File.Delete (testResults);

			TI.BuildProject (testFolder + "/Protobuf_Test.csproj");

			TI.RunAndAssert (testFolder + "/bin/Debug/Protobuf_Test.app/Contents/MacOS/Protobuf_Test", Array.Empty<string> (), "Run");
			Assert.That (File.Exists (testResults), Is.True);

			using (TextReader reader = File.OpenText (testResults)) {
				var output = reader.ReadLine ();

				Assert.That (output, Is.EqualTo ("Test Passed"));
			}

			File.Delete (testResults);
		}

		[Test]
		public void Net45ShouldUseImmutableCollection ()
		{
			var testFolder = Path.Combine (Configuration.TestProjectsDirectory, "ImmutableCollection_Test", "ImmutableCollection_Test");

			TI.BuildProject (testFolder + "/ImmutableCollection_Test.csproj");

			TI.RunAndAssert (testFolder + "/bin/Debug/ImmutableCollection_Test.app/Contents/MacOS/ImmutableCollection_Test", Array.Empty<string> (), "Run");
		}

		[Test]
		public void BasicPCLTest ()
		{
			var testFolder = Path.Combine (Configuration.TestProjectsDirectory, "BasicPCLTest", "BasicPCLTest");
			var testResults = testFolder + "/TestResult.txt";
			if (File.Exists (testResults))
				File.Delete (testResults);

			TI.BuildProject (testFolder + "/BasicPCLTest.csproj");

			TI.RunAndAssert (testFolder + "/bin/Debug/BasicPCLTest.app/Contents/MacOS/BasicPCLTest", Array.Empty<string> (), "Run");
			Assert.That (File.Exists (testResults), Is.True);

			using (TextReader reader = File.OpenText (testResults)) {
				var output = reader.ReadToEnd ();

				Assert.That (output, Is.EqualTo ("{\n  \"MyArray\": [\n    \"Manual text\",\n    \"2000-05-23T00:00:00\"\n  ]\n}\n"));
			}

			File.Delete (testResults);
		}
	}
}
