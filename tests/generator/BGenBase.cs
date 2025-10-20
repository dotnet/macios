using System.Linq;
using System.IO;

using Xamarin.Tests;

namespace GeneratorTests {
	public class BGenBase {
		internal BGenTool BuildFile (Profile profile, params string [] filenames)
		{
			return BuildFile (profile, true, false, filenames);
		}

		internal BGenTool BuildFile (Profile profile, bool nowarnings, params string [] filenames)
		{
			return BuildFile (profile, nowarnings, false, filenames);
		}

		internal BGenTool BuildFile (Profile profile, bool nowarnings, bool processEnums, params string [] filenames)
		{
			return BuildFile (profile, nowarnings, processEnums, Enumerable.Empty<string> (), filenames);
		}

		internal BGenTool BuildFile (Profile profile, bool nowarnings, bool processEnums, IEnumerable<string> references, params string [] filenames)
		{
			return BuildFile (profile, nowarnings, (bgen) => {
				bgen.ProcessEnums = processEnums;
				bgen.References = references.ToList ();
			}, filenames);
		}

		internal BGenTool BuildFile (Profile profile, Action<BGenTool> configure, params string [] filenames)
		{
			return BuildFile (profile, true, configure, filenames);
		}

		internal BGenTool BuildFile (Profile profile, bool nowarnings, Action<BGenTool> configure, params string [] filenames)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.Defines = BGenTool.GetDefaultDefines (profile);
			configure (bgen);
			TestContext.Out.WriteLine (TestContext.CurrentContext.Test.FullName);
			foreach (var filename in filenames)
				TestContext.Out.WriteLine ($"\t{filename}");
			bgen.CreateTemporaryBinding (filenames.Select ((filename) => File.ReadAllText (Path.Combine (Configuration.SourceRoot, "tests", "generator", filename))).ToArray ());
			bgen.AssertExecute ("build");
			if (nowarnings)
				bgen.AssertNoWarnings ();
			return bgen;
		}
	}
}
