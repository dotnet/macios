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
			ClassicAssert.NotNull (UTType.ExportedTypeDeclarationsKey, "ExportedTypeDeclarationsKey");
			ClassicAssert.NotNull (UTType.ImportedTypeDeclarationsKey, "ImportedTypeDeclarationsKey");
			ClassicAssert.NotNull (UTType.IdentifierKey, "IdentifierKey");
			ClassicAssert.NotNull (UTType.TagSpecificationKey, "TagSpecificationKey");
			ClassicAssert.NotNull (UTType.ConformsToKey, "ConformsToKey");
			ClassicAssert.NotNull (UTType.DescriptionKey, "DescriptionKey");
			ClassicAssert.NotNull (UTType.IconFileKey, "IconFileKey");
			ClassicAssert.NotNull (UTType.ReferenceURLKey, "ReferenceURLKey");
			ClassicAssert.NotNull (UTType.VersionKey, "VersionKey");

			ClassicAssert.NotNull (UTType.TagClassFilenameExtension, "TagClassFilenameExtension");
			ClassicAssert.NotNull (UTType.TagClassMIMEType, "TagClassMIMEType");

			ClassicAssert.NotNull (UTType.Item, "Item");
			ClassicAssert.NotNull (UTType.Content, "Content");
			ClassicAssert.NotNull (UTType.CompositeContent, "CompositeContent");
			ClassicAssert.NotNull (UTType.Application, "Application");
			ClassicAssert.NotNull (UTType.Message, "Message");
			ClassicAssert.NotNull (UTType.Contact, "Contact");
			ClassicAssert.NotNull (UTType.Archive, "Archive");
			ClassicAssert.NotNull (UTType.DiskImage, "DiskImage");

			ClassicAssert.NotNull (UTType.Data, "Data");
			ClassicAssert.NotNull (UTType.Directory, "Directory");
			ClassicAssert.NotNull (UTType.Resolvable, "Resolvable");
			ClassicAssert.NotNull (UTType.SymLink, "SymLink");
			ClassicAssert.NotNull (UTType.MountPoint, "MountPoint");
			ClassicAssert.NotNull (UTType.AliasFile, "AliasFile");
			ClassicAssert.NotNull (UTType.AliasRecord, "AliasRecord");
			ClassicAssert.NotNull (UTType.URL, "URL");
			ClassicAssert.NotNull (UTType.FileURL, "FileURL");

			ClassicAssert.NotNull (UTType.Text, "Text");
			ClassicAssert.NotNull (UTType.PlainText, "PlainText");
			ClassicAssert.NotNull (UTType.UTF8PlainText, "UTF8PlainText");
			ClassicAssert.NotNull (UTType.UTF16ExternalPlainText, "UTF16ExternalPlainText");
			ClassicAssert.NotNull (UTType.UTF16PlainText, "UTF16PlainText");
			ClassicAssert.NotNull (UTType.RTF, "RTF");
			ClassicAssert.NotNull (UTType.HTML, "HTML");
			ClassicAssert.NotNull (UTType.XML, "XML");
			ClassicAssert.NotNull (UTType.SourceCode, "SourceCode");
			ClassicAssert.NotNull (UTType.CSource, "CSource");
			ClassicAssert.NotNull (UTType.ObjectiveCSource, "ObjectiveCSource");
			ClassicAssert.NotNull (UTType.CPlusPlusSource, "CPlusPlusSource");
			ClassicAssert.NotNull (UTType.ObjectiveCPlusPlusSource, "ObjectiveCPlusPlusSource");
			ClassicAssert.NotNull (UTType.CHeader, "CHeader");
			ClassicAssert.NotNull (UTType.CPlusPlusHeader, "CPlusPlusHeader");
			ClassicAssert.NotNull (UTType.JavaSource, "JavaSource");

			ClassicAssert.NotNull (UTType.PDF, "PDF");
			ClassicAssert.NotNull (UTType.RTFD, "RTFD");
			ClassicAssert.NotNull (UTType.FlatRTFD, "FlatRTFD");
			ClassicAssert.NotNull (UTType.TXNTextAndMultimediaData, "TXNTextAndMultimediaData");
			ClassicAssert.NotNull (UTType.WebArchive, "WebArchive");

			ClassicAssert.NotNull (UTType.Image, "Image");
			ClassicAssert.NotNull (UTType.JPEG, "JPEG");
			ClassicAssert.NotNull (UTType.JPEG2000, "JPEG2000");
			ClassicAssert.NotNull (UTType.TIFF, "TIFF");
			ClassicAssert.NotNull (UTType.GIF, "GIF");
			ClassicAssert.NotNull (UTType.PNG, "PNG");
			ClassicAssert.NotNull (UTType.QuickTimeImage, "QuickTimeImage");
			ClassicAssert.NotNull (UTType.AppleICNS, "AppleICNS");
			ClassicAssert.NotNull (UTType.BMP, "BMP");
			ClassicAssert.NotNull (UTType.ICO, "ICO");

			ClassicAssert.NotNull (UTType.AudiovisualContent, "AudiovisualContent");
			ClassicAssert.NotNull (UTType.Movie, "Movie");
			ClassicAssert.NotNull (UTType.Video, "Video");
			ClassicAssert.NotNull (UTType.Audio, "Audio");
			ClassicAssert.NotNull (UTType.QuickTimeMovie, "QuickTimeMovie");
			ClassicAssert.NotNull (UTType.MPEG, "MPEG");
			ClassicAssert.NotNull (UTType.MPEG4, "MPEG4");
			ClassicAssert.NotNull (UTType.MP3, "MP3");
			ClassicAssert.NotNull (UTType.MPEG4Audio, "MPEG4Audio");
			ClassicAssert.NotNull (UTType.AppleProtectedMPEG4Audio, "AppleProtectedMPEG4Audio");

			ClassicAssert.NotNull (UTType.Folder, "Folder");
			ClassicAssert.NotNull (UTType.Volume, "Volume");
			ClassicAssert.NotNull (UTType.Package, "Package");
			ClassicAssert.NotNull (UTType.Bundle, "Bundle");
			ClassicAssert.NotNull (UTType.Framework, "Framework");

			ClassicAssert.NotNull (UTType.ApplicationBundle, "ApplicationBundle");
			ClassicAssert.NotNull (UTType.ApplicationFile, "ApplicationFile");

			ClassicAssert.NotNull (UTType.VCard, "VCard");

			ClassicAssert.NotNull (UTType.InkText, "InkText");

			if (TestRuntime.CheckXcodeVersion (7, 0))
				ClassicAssert.NotNull (UTType.SwiftSource, "SwiftSource");
		}

		[Test]
		public void GetPreferredTag ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			ClassicAssert.NotNull (UTType.GetPreferredTag (UTType.PDF, UTType.TagClassFilenameExtension), "GetPreferredTag");
		}

		[Test]
		public void GetDeclaration ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			ClassicAssert.NotNull (UTType.GetDeclaration (UTType.PDF));
		}

		[Test]
		public void GetDeclaringBundleURL ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			ClassicAssert.NotNull (UTType.GetDeclaringBundleUrl (UTType.PDF));
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
					ClassicAssert.NotNull (result, ext + i.ToString ());
				}
			}
		}

		[Test]
		public void Equals ()
		{
			ClassicAssert.True (UTType.Equals (null, null), "null-null");
			ClassicAssert.False (UTType.Equals (null, UTType.PDF), "null-PDF");
			ClassicAssert.False (UTType.Equals (UTType.PDF, null), "PDF-null");
			ClassicAssert.True (UTType.Equals (UTType.PDF, UTType.PDF), "PDF-PDF");
		}

		[Test]
		public void CreateAllIdentifiers ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.CreateAllIdentifiers (UTType.TagClassFilenameExtension, "pdf", null);
			ClassicAssert.NotNull (result, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void CopyAllTags ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.CopyAllTags (UTType.PDF, UTType.TagClassFilenameExtension);
			ClassicAssert.NotNull (result, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}

		[Test]
		public void GetDescription ()
		{
			TestRuntime.AssertIfSimulatorThenARM64 ();

			var result = UTType.GetDescription (UTType.PDF);
			ClassicAssert.NotNull (result, "result");
			Assert.That (result.Length, Is.GreaterThan (0), "Length");
		}
	}
}
