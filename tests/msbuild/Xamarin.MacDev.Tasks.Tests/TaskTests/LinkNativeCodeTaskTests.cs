// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Utilities;

using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class LinkNativeCodeTaskTests : TestBase {
		[TestCase ("DeclaredAgeRange")]
		[TestCase ("native/DeclaredAgeRange")]
		[TestCase ("native/DeclaredAgeRange.dylib")]
		public void InvalidFrameworkReference (string path)
		{
			var reference = new TaskItem (path);
			reference.SetMetadata ("Kind", "Framework");
			reference.SetMetadata ("Frameworks", "DeclaredAgeRange");

			var task = CreateTask<LinkNativeCode> ();
			task.TargetArchitectures = "ARM64";
			task.NativeReferences = [reference];

			ExecuteTask (task, 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT7201"), "Error code");
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo ($"The native framework reference '{path}' must be a path to a .framework directory or its library. To link a system framework, use LinkerArgument items instead: '-framework' followed by the framework name."));
		}

		[TestCase ("DeclaredAgeRange.framework", "DeclaredAgeRange.framework")]
		[TestCase ("DeclaredAgeRange.framework/DeclaredAgeRange", "DeclaredAgeRange.framework")]
		[TestCase ("native/DeclaredAgeRange.framework", "native/DeclaredAgeRange.framework")]
		[TestCase ("native/DeclaredAgeRange.framework/DeclaredAgeRange", "native/DeclaredAgeRange.framework")]
		[TestCase ("/native/DeclaredAgeRange.framework/DeclaredAgeRange", "/native/DeclaredAgeRange.framework")]
		[TestCase ("native/DeclaredAgeRange.FRAMEWORK/DeclaredAgeRange", "native/DeclaredAgeRange.FRAMEWORK")]
		public void ValidFrameworkReference (string path, string expectedFramework)
		{
			var reference = new TaskItem (path);
			reference.SetMetadata ("Kind", "Framework");

			var options = new LinkerOptions ();
			options.BuildNativeReferenceFlags (new TaskLoggingHelper (Engine, "LinkNativeCode"), [reference]);

			Assert.That (Engine.Logger.ErrorEvents, Is.Empty, "Errors");
			Assert.That (options.Frameworks, Is.EquivalentTo (new [] { expectedFramework }), "Frameworks");
		}
	}
}
