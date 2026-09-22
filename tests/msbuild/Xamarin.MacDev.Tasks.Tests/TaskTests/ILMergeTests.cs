// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

using Mono.Cecil;
using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {

	[TestFixture]
	public class ILMergeTests {

		[TestCase ("Microsoft.Extensions.DependencyInjection.Abstractions")]
		[TestCase ("Microsoft.Extensions.Logging.Abstractions")]
		public void MessagingDependencyIsMerged (string dependency)
		{
			using var assembly = AssemblyDefinition.ReadAssembly (typeof (CompileAppManifest).Assembly.Location);

			Assert.That (assembly.MainModule.AssemblyReferences.Any (reference => reference.Name == dependency), Is.False, $"{dependency} must be merged into Xamarin.MacDev.Tasks.dll for desktop MSBuild.");
		}
	}
}
