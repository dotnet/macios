// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

using Mono.Cecil;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class AssemblyTests {
		[Test]
		public void MergedAssemblyReferences ()
		{
			var expectedReferences = new [] {
				"Microsoft.Build",
				"Microsoft.Build.Framework",
				"Microsoft.Build.Tasks.Core",
				"Microsoft.Build.Utilities.Core",
				"Microsoft.Win32.Registry",
				"System",
				"System.Core",
				"System.Xml",
				"System.Xml.Linq",
				"Xamarin.Localization.MSBuild",
				"mscorlib",
				"netstandard",
			};
			using var assembly = AssemblyDefinition.ReadAssembly (typeof (CompileAppManifest).Assembly.Location);
			var actualReferences = assembly.MainModule.AssemblyReferences.Select (v => v.Name);

			Assert.That (actualReferences, Is.EquivalentTo (expectedReferences));
		}
	}
}
