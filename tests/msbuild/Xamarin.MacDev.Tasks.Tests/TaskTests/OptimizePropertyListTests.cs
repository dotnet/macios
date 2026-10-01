// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class OptimizePropertyListTests : TestBase {
		[Test]
		public void ConvertsXmlPropertyListToBinary ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var input = Path.Combine (directory, "input.plist");
			var output = Path.Combine (directory, "nested", "output.plist");
			var plist = new PDictionary {
				{ "Value", new PString ("Expected") },
			};
			plist.Save (input);

			var task = CreateTask<OptimizePropertyList> ();
			task.Input = new TaskItem (input);
			task.Output = new TaskItem (output);

			ExecuteTask (task);

			var optimized = PDictionary.OpenFile (output, out var isBinary);
			Assert.Multiple (() => {
				Assert.That (isBinary, Is.True, "Binary format");
				Assert.That (optimized.GetString ("Value").Value, Is.EqualTo ("Expected"), "Value");
			});
		}
	}
}
