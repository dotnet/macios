//
// Unit tests for UTType
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012, 2015 Xamarin Inc. All rights reserved.
//

using MobileCoreServices;

namespace MonoTouchFixtures.MobileCoreServices {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UTTypeTest {

		[Test]
		public void NSStringConstants ()
		{
			Assert.That (UTType.ExportedTypeDeclarationsKey, Is.Not.Null, "ExportedTypeDeclarationsKey");
			Assert.That (UTType.ImportedTypeDeclarationsKey, Is.Not.Null, "ImportedTypeDeclarationsKey");
			Assert.That (UTType.IdentifierKey, Is.Not.Null, "IdentifierKey");
			Assert.That (UTType.TagSpecificationKey, Is.Not.Null, "TagSpecificationKey");
			Assert.That (UTType.ConformsToKey, Is.Not.Null, "ConformsToKey");
			Assert.That (UTType.DescriptionKey, Is.Not.Null, "DescriptionKey");
			Assert.That (UTType.IconFileKey, Is.Not.Null, "IconFileKey");
			Assert.That (UTType.ReferenceURLKey, Is.Not.Null, "ReferenceURLKey");
			Assert.That (UTType.VersionKey, Is.Not.Null, "VersionKey");

			Assert.That (UTType.TagClassFilenameExtension, Is.Not.Null, "TagClassFilenameExtension");
			Assert.That (UTType.TagClassMIMEType, Is.Not.Null, "TagClassMIMEType");

			Assert.That (UTType.Item, Is.Not.Null, "Item");
			Assert.That (UTType.Content, Is.Not.Null, "Content");
			Assert.That (UTType.CompositeContent, Is.Not.Null, "CompositeContent");
			Assert.That (UTType.Application, Is.Not.Null, "Application");
			Assert.That (UTType.Message, Is.Not.Null, "Message");
			Assert.That (UTType.Contact, Is.Not.Null, "Contact");
			Assert.That (UTType.Archive, Is.Not.Null, "Archive");
			Assert.That (UTType.DiskImage, Is.Not.Null, "DiskImage");

			Assert.That (UTType.Data, Is.Not.Null, "Data");
			Assert.That (UTType.Directory, Is.Not.Null, "Directory");
			Assert.That (UTType.Resolvable, Is.Not.Null, "Resolvable");
			Assert.That (UTType.SymLink, Is.Not.Null, "SymLink");
			Assert.That (UTType.MountPoint, Is.Not.Null, "MountPoint");
			Assert.That (UTType.AliasFile, Is.Not.Null, "AliasFile");
			Assert.That (UTType.AliasRecord, Is.Not.Null, "AliasRecord");
			Assert.That (UTType.URL, Is.Not.Null, "URL");
			Assert.That (UTType.FileURL, Is.Not.Null, "FileURL");

			Assert.That (UTType.Text, Is.Not.Null, "Text");
			Assert.That (UTType.PlainText, Is.Not.Null, "PlainText");
			Assert.That (UTType.UTF8PlainText, Is.Not.Null, "UTF8PlainText");
			Assert.That (UTType.UTF16ExternalPlainText, Is.Not.Null, "UTF16ExternalPlainText");
			Assert.That (UTType.UTF16PlainText, Is.Not.Null, "UTF16PlainText");
			Assert.That (UTType.RTF, Is.Not.Null, "RTF");
			Assert.That (UTType.HTML, Is.Not.Null, "HTML");
			Assert.That (UTType.XML, Is.Not.Null, "XML");
			Assert.That (UTType.SourceCode, Is.Not.Null, "SourceCode");
			Assert.That (UTType.CSource, Is.Not.Null, "CSource");
			Assert.That (UTType.ObjectiveCSource, Is.Not.Null, "ObjectiveCSource");
			Assert.That (UTType.CPlusPlusSource, Is.Not.Null, "CPlusPlusSource");
			Assert.That (UTType.ObjectiveCPlusPlusSource, Is.Not.Null, "ObjectiveCPlusPlusSource");
			Assert.That (UTType.CHeader, Is.Not.Null, "CHeader");
			Assert.That (UTType.CPlusPlusHeader, Is.Not.Null, "CPlusPlusHeader");
			Assert.That (UTType.JavaSource, Is.Not.Null, "JavaSource");

			Assert.That (UTType.PDF, Is.Not.Null, "PDF");
			Assert.That (UTType.RTFD, Is.Not.Null, "RTFD");
			Assert.That (UTType.FlatRTFD, Is.Not.Null, "FlatRTFD");
			Assert.That (UTType.TXNTextAndMultimediaData, Is.Not.Null, "TXNTextAndMultimediaData");
			Assert.That (UTType.WebArchive, Is.Not.Null, "WebArchive");

			Assert.That (UTType.Image, Is.Not.Null, "Image");
			Assert.That (UTType.JPEG, Is.Not.Null, "JPEG");
			Assert.That (UTType.JPEG2000, Is.Not.Null, "JPEG2000");
			Assert.That (UTType.TIFF, Is.Not.Null, "TIFF");
			Assert.That (UTType.GIF, Is.Not.Null, "GIF");
			Assert.That (UTType.PNG, Is.Not.Null, "PNG");
			Assert.That (UTType.QuickTimeImage, Is.Not.Null, "QuickTimeImage");
			Assert.That (UTType.AppleICNS, Is.Not.Null, "AppleICNS");
			Assert.That (UTType.BMP, Is.Not.Null, "BMP");
			Assert.That (UTType.ICO, Is.Not.Null, "ICO");

			Assert.That (UTType.AudiovisualContent, Is.Not.Null, "AudiovisualContent");
			Assert.That (UTType.Movie, Is.Not.Null, "Movie");
			Assert.That (UTType.Video, Is.Not.Null, "Video");
			Assert.That (UTType.Audio, Is.Not.Null, "Audio");
			Assert.That (UTType.QuickTimeMovie, Is.Not.Null, "QuickTimeMovie");
			Assert.That (UTType.MPEG, Is.Not.Null, "MPEG");
			Assert.That (UTType.MPEG4, Is.Not.Null, "MPEG4");
			Assert.That (UTType.MP3, Is.Not.Null, "MP3");
			Assert.That (UTType.MPEG4Audio, Is.Not.Null, "MPEG4Audio");
			Assert.That (UTType.AppleProtectedMPEG4Audio, Is.Not.Null, "AppleProtectedMPEG4Audio");

			Assert.That (UTType.Folder, Is.Not.Null, "Folder");
			Assert.That (UTType.Volume, Is.Not.Null, "Volume");
			Assert.That (UTType.Package, Is.Not.Null, "Package");
			Assert.That (UTType.Bundle, Is.Not.Null, "Bundle");
			Assert.That (UTType.Framework, Is.Not.Null, "Framework");

			Assert.That (UTType.ApplicationBundle, Is.Not.Null, "ApplicationBundle");
			Assert.That (UTType.ApplicationFile, Is.Not.Null, "ApplicationFile");

			Assert.That (UTType.VCard, Is.Not.Null, "VCard");

			Assert.That (UTType.InkText, Is.Not.Null, "InkText");

			if (TestRuntime.CheckXcodeVersion (7, 0))
				Assert.That (UTType.SwiftSource, Is.Not.Null, "SwiftSource");
		}

		[Test]
		public void GetPreferredTag ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			Assert.That (UTType.GetPreferredTag (UTType.PDF, UTType.TagClassFilenameExtension), Is.Not.Null, "GetPreferredTag");
		}

		[Test]
		public void GetDeclaration ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			Assert.That (UTType.GetDeclaration (UTType.PDF), Is.Not.Null);
		}

		[Test]
		public void GetDeclaringBundleURL ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			Assert.That (UTType.GetDeclaringBundleUrl (UTType.PDF), Is.Not.Null);
		}

		[Test]
		public void CreatePreferredIdentifier ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			string [] extensions = new [] { ".html", ".css", ".jpg", ".js", ".otf" };
			// random failure reported in #36708 (on some iPad2 only)
			for (int i = 0; i < 100; i++) {
				foreach (var ext in extensions) {
					var result = UTType.CreatePreferredIdentifier (UTType.TagClassMIMEType, ext, null);
					Assert.That (result, Is.Not.Null, ext + i.ToString ());
				}
			}
		}

		[Test]
		public void Equals ()
		{
			Assert.That (UTType.Equals (null, null), Is.True, "null-null");
			Assert.That (UTType.Equals (null, UTType.PDF), Is.False, "null-PDF");
			Assert.That (UTType.Equals (UTType.PDF, null), Is.False, "PDF-null");
			Assert.That (UTType.Equals (UTType.PDF, UTType.PDF), Is.True, "PDF-PDF");
		}

		[Test]
		public void CreateAllIdentifiers ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.CreateAllIdentifiers (UTType.TagClassFilenameExtension, "pdf", null);
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void CopyAllTags ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.CopyAllTags (UTType.PDF, UTType.TagClassFilenameExtension);
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void GetDescription ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.GetDescription (UTType.PDF);
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}
	}
}
