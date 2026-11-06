using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mono.Cecil;

#nullable enable

namespace Cecil.Tests {
	[TestClass]
	public class OpenTKTest {
		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformAssemblyTestData), typeof (Helper))]
		// https://github.com/dotnet/macios/issues/9724
		public void BeGone (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			var assembly = info.Assembly;
			var found = new HashSet<string> ();
			foreach (var type in assembly.MainModule.Types) {
				if (type.Namespace?.StartsWith ("OpenTK", StringComparison.Ordinal) == true) {
					found.Add (type.FullName);
				}
			}

			Assert.IsEmpty (found, string.Join ("\n", found));
		}
	}
}
