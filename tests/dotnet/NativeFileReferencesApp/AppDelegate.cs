using System;
using System.Runtime.InteropServices;

using Foundation;

namespace NativeFileReferencesApp {
	public class Program {
#if REMAPPED_PINVOKES
		const string FirstLibrary = "RemapMe";
		const string SecondLibrary = "RemapMe2";
#else
		const string FirstLibrary = "__Internal";
		const string SecondLibrary = "__Internal";
#endif

		// This comes from libtest.a
		[DllImport (FirstLibrary)]
		static extern int theUltimateAnswer ();

		// This comes from libtest2.a
		[DllImport (SecondLibrary)]
		static extern int getIntOfChocolate ();

		static int Main (string [] args)
		{
#if REMAPPED_PINVOKES && !NATIVEAOT
			NativeLibrary.SetDllImportResolver (typeof (Program).Assembly, (name, assembly, searchPath) => NativeLibrary.GetMainProgramHandle ());
#endif
			Console.WriteLine ($"libtest.a: {theUltimateAnswer ()}");
			Console.WriteLine ($"libtest2.a: {getIntOfChocolate ()}");

			GC.KeepAlive (typeof (NSObject)); // prevent linking away the platform assembly

			Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));

			return 0;
		}
	}
}
