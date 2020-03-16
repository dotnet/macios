using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;

namespace Xamarin.MacDev.Tasks {
	public abstract class CreateBindingResourcePackageBase : Task {
		public string SessionId { get; set; }
		
		[Required]
		public string OutputPath { get; set; }
		
		[Required]		
		public ITaskItem[] NativeReferences { get; set; }
		
		[Required]
		public string ProjectDir { get; set; }
		
		[Required]
		public string BindingAssembly { get; set; }
		
		[Output]
		public ITaskItem Manifest { get; set; }
		
		public override bool Execute ()
		{
			// LinkWith must be migrated for NoBindingEmbedding styled binding projects
			if (NativeReferences.Length == 0) {
				Log.LogError (7068, null, MSBStrings.E7068);
				return false;
			}

			string bindingResourcePath = Path.Combine (ProjectDir, OutputPath, Path.ChangeExtension (Path.GetFileName (BindingAssembly), ".resources"));
			Log.LogMessage (MSBStrings.M0121, bindingResourcePath);

			Directory.CreateDirectory (bindingResourcePath);
			foreach (var nativeRef in NativeReferences)
				Xamarin.Bundler.FileCopier.UpdateDirectory (nativeRef.ItemSpec, bindingResourcePath);

			string manifestPath = CreateManifest (bindingResourcePath);

			Manifest = new TaskItem ("Manifest") { ItemSpec = manifestPath };

			return true;
		}

		string [] NativeReferenceAttributeNames = new string [] { "Kind", "ForceLoad", "SmartLink", "Frameworks", "WeakFrameworks", "LinkerFlags", "NeedsGccExceptionHandling", "IsCxx"};

		string CreateManifest (string resourcePath)
		{
			XmlWriterSettings settings = new XmlWriterSettings() {
				OmitXmlDeclaration = true,
				Indent = true,
				IndentChars = "\t",
			};

			string manifestPath = Path.Combine (resourcePath, "manifest");
			using (var writer = XmlWriter.Create (manifestPath, settings)) {
				writer.WriteStartElement ("BindingAssembly");

				foreach (var nativeRef in NativeReferences) {
					writer.WriteStartElement ("NativeReference");
					writer.WriteAttributeString ("Name", Path.GetFileName (nativeRef.ItemSpec));

					foreach (string attribute in NativeReferenceAttributeNames) {
						writer.WriteStartElement (attribute);
						writer.WriteString (nativeRef.GetMetadata (attribute));
						writer.WriteEndElement ();
					}

					writer.WriteEndElement ();
				}
				writer.WriteEndElement ();
			}
			return manifestPath;
		}
	}
}
