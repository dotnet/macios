using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;
using System.Xml.Linq;

using Xamarin.Messaging.Build.Client;

namespace Xamarin.MacDev.Tasks {
	public class ReadItemsFromFile : XamarinTask, ITaskCallback {
		static readonly XNamespace XmlNs = XNamespace.Get ("http://schemas.microsoft.com/developer/msbuild/2003");

		static readonly XName ItemGroupElementName = XmlNs + "ItemGroup";
		const string IncludeAttributeName = "Include";

		#region Inputs

		[Output]
		[Required]
		public ITaskItem [] File { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		#region Outputs

		[Output]
		public ITaskItem [] Items { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			var result = new List<ITaskItem> ();
			foreach (var file in File) {
				// XDocument.Load(string) takes a string to a URI, not a file path, so with certain characters that becomes a problem.
				// Just File.OpenRead instead and use the XDocument.Load(Stream) overload instead.
				using var stream = global::System.IO.File.OpenRead (file.ItemSpec);
				var document = XDocument.Load (stream);

				var items = document.Root
					.Elements (ItemGroupElementName)
					.SelectMany (element => element.Elements ())
					.Select (element => this.CreateItemFromElement (element, file.ItemSpec))
					.ToList ();
				result.AddRange (items);
			}

			if (Items is not null)
				result.AddRange (Items);

			Items = result.ToArray ();

			return true;
		}

		ITaskItem CreateItemFromElement (XElement element, string sourceFile)
		{
			var item = new TaskItem (element.Attribute (IncludeAttributeName).Value);

			foreach (var metadata in element.Elements ()) {
				item.SetMetadata (metadata.Name.LocalName, metadata.Value);
			}

			// Set the SourceFile metadata to the file from which this item was read.
			item.SetMetadata ("SourceFile", Path.GetFileName (sourceFile));

			return item;
		}

		public bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public bool ShouldCreateOutputFile (ITaskItem item) => false;

		public IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();
	}
}
