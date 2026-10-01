// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Collections.Generic;

using Microsoft.Build.Utilities;
using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ComputeHashForItemsTaskTests : TestBase {
		[Test]
		public void MetadataBoundariesAffectHash ()
		{
			var first = new TaskItem ("aString", new Dictionary<string, string> {
				{ "Type", "String" },
				{ "Value", "b" },
			});
			var second = new TaskItem ("a", new Dictionary<string, string> {
				{ "Type", "String" },
				{ "Value", "Stringb" },
			});
			var task = CreateTask<ComputeHashForItems> ();
			task.Input = [first, second];
			task.InputMetadata = [
				new TaskItem ("Identity"),
				new TaskItem ("Type"),
				new TaskItem ("Value"),
				new TaskItem ("ArraySeparator"),
			];
			task.OutputMetadata = "Hash";

			ExecuteTask (task);

			Assert.That (first.GetMetadata ("Hash"), Is.Not.EqualTo (second.GetMetadata ("Hash")));
		}
	}
}
