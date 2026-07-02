// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class UnpackLibraryResourcesTests : TestBase {

		static string CreateAssemblyWithResource (string directory, string resourceName, byte [] content)
		{
			var asmPath = Path.Combine (directory, "TestLib.dll");

			var metadataBuilder = new MetadataBuilder ();

			metadataBuilder.AddModule (
				0,
				metadataBuilder.GetOrAddString ("TestLib.dll"),
				metadataBuilder.GetOrAddGuid (Guid.NewGuid ()),
				default,
				default);

			metadataBuilder.AddAssembly (
				metadataBuilder.GetOrAddString ("TestLib"),
				new Version (1, 0, 0, 0),
				default,
				default,
				default,
				AssemblyHashAlgorithm.None);

			// Add the embedded resource
			var resourceBlob = metadataBuilder.GetOrAddBlob (content);
			metadataBuilder.AddManifestResource (
				ManifestResourceAttributes.Public,
				metadataBuilder.GetOrAddString (resourceName),
				default,
				0);

			var metadataRootBuilder = new MetadataRootBuilder (metadataBuilder);

			// Build the PE with the resource data
			var peHeaderBuilder = new PEHeaderBuilder (imageCharacteristics: Characteristics.Dll);
			var resourceBlobBuilder = new BlobBuilder ();
			// Resource format: 4-byte length prefix followed by the data
			resourceBlobBuilder.WriteInt32 (content.Length);
			resourceBlobBuilder.WriteBytes (content);

			var peBuilder = new ManagedPEBuilder (
				peHeaderBuilder,
				metadataRootBuilder,
				ilStream: new BlobBuilder (),
				managedResources: resourceBlobBuilder);

			var blobBuilder = new BlobBuilder ();
			peBuilder.Serialize (blobBuilder);

			using var fs = new FileStream (asmPath, FileMode.Create, FileAccess.Write);
			blobBuilder.WriteContentTo (fs);

			return asmPath;
		}

		[Test]
		public void PathTraversal_IsRejected ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var prefix = "monotouch";
			// Mangled resource name: ".._sEvil.txt" unmangles to "../Evil.txt" (path traversal)
			var resourceName = $"__{prefix}_content_.._sEvil.txt";
			var assemblyPath = CreateAssemblyWithResource (tmpdir, resourceName, new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = prefix;
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("would extract to"));
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("outside"));

			// Verify the file was NOT written outside the target directory
			var escapedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestLib", "Evil.txt");
			Assert.That (File.Exists (escapedPath), Is.False, "File should not have been extracted outside target directory");
		}

		[Test]
		public void ValidResource_IsExtracted ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var prefix = "monotouch";
			// Mangled resource name: "sub_sfile.txt" unmangles to "sub/file.txt" (valid path)
			var resourceName = $"__{prefix}_content_sub_sfile.txt";
			var assemblyPath = CreateAssemblyWithResource (tmpdir, resourceName, new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = prefix;
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 0);

			var extractedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestLib", "content", "sub", "file.txt");
			Assert.That (File.Exists (extractedPath), Is.True, $"File should have been extracted to {extractedPath}");
		}
	}
}
