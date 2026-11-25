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

		[TestCase (false)]
		[TestCase (true)]
		public void PreservesValuesAfterLongFormBoolean (bool value)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var input = Path.Combine (directory, "input.plist");
			var output = Path.Combine (directory, "output.plist");
			var boolean = value ? "true" : "false";
			File.WriteAllText (input, $"""
				<?xml version="1.0" encoding="UTF-8"?>
				<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
				<plist version="1.0">
				<dict>
					<key>Before</key>
					<string>First</string>
					<key>Boolean</key>
					<{boolean}></{boolean}>
					<key>After</key>
					<string>Last</string>
				</dict>
				</plist>
				""");

			var task = CreateTask<OptimizePropertyList> ();
			task.Input = new TaskItem (input);
			task.Output = new TaskItem (output);

			ExecuteTask (task);

			var optimized = PDictionary.OpenFile (output, out var isBinary);
			Assert.Multiple (() => {
				Assert.That (isBinary, Is.True, "Binary format");
				Assert.That (optimized.Count, Is.EqualTo (3), "Dictionary count");
				Assert.That (optimized.GetString ("Before").Value, Is.EqualTo ("First"), "Before");
				Assert.That (optimized.Get<PBoolean> ("Boolean")?.Value, Is.EqualTo (value), "Boolean");
				Assert.That (optimized.GetString ("After")?.Value, Is.EqualTo ("Last"), "After");
			});
		}
	}
}
