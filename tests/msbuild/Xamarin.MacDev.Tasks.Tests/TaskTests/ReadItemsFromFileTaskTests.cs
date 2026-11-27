// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.IO;

using Microsoft.Build.Utilities;
using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ReadItemsFromFileTaskTests : TestBase {
		[TestCase (false)]
		[TestCase (true)]
		public void ContainerMetadata (bool copyMetadata)
		{
			var path = Path.Combine (Cache.CreateTemporaryDirectory (), "codesign-bundle.items");
			var bundle = new TaskItem ("Extension.appex");
			bundle.SetMetadata ("BundleRelativePath", "PlugIns/");
			bundle.SetMetadata ("CodesignEntitlements", "extension.entitlements");
			var writer = CreateTask<WriteItemsToFile> ();
			writer.File = new TaskItem (path);
			writer.Items = new [] { bundle };
			writer.ItemName = "Bundle";
			writer.IncludeMetadata = true;
			ExecuteTask (writer);

			var file = new TaskItem (path);
			file.SetMetadata ("BundleRelativePath", "Contents/Extensions/");
			var reader = CreateTask<ReadItemsFromFile> ();
			reader.File = new [] { file };
			reader.MetadataToCopy = copyMetadata ? new [] { "BundleRelativePath" } : [];
			ExecuteTask (reader);

			Assert.That (reader.Items, Has.Length.EqualTo (1));
			var item = reader.Items [0];
			Assert.That (item.ItemSpec, Is.EqualTo ("Extension.appex"));
			Assert.That (item.GetMetadata ("BundleRelativePath"), Is.EqualTo (copyMetadata ? "Contents/Extensions/" : "PlugIns/"));
			Assert.That (item.GetMetadata ("CodesignEntitlements"), Is.EqualTo ("extension.entitlements"));
			Assert.That (item.GetMetadata ("SourceFile"), Is.EqualTo ("codesign-bundle.items"));
		}
	}
}
