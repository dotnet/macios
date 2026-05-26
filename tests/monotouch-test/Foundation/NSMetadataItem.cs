#if __MACOS__
using AppKit;

namespace Xamarin.Mac.Tests {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSMetadataItemTest {

		[Test]
		[Ignore ("Fails on bots, disabled until investigated")]
		public void CtorUrl ()
		{
			// 10.9 for NSMetadataItem initWithURL:
			TestRuntime.AssertXcodeVersion (5, 1);

			var url = NSBundle.MainBundle.BundleUrl;
			using (var mi = new NSMetadataItem (url)) {
				Assert.That (mi.DisplayName.ToString (), Is.EqualTo ("apitest"), "DisplayName");
				Assert.That (mi.FileSystemContentChangeDate, Is.Not.Null, "FileSystemContentChangeDate");
				Assert.That (mi.FileSystemCreationDate, Is.Not.Null, "FileSystemCreationDate");
				Assert.That (mi.FileSystemName.ToString (), Is.EqualTo ("apitest.app"), "FileSystemName");
				Assert.That (mi.FileSystemSize.UInt64Value, Is.GreaterThan (0), "FileSystemSize");
				Assert.That (mi.IsUbiquitous, Is.False, "IsUbiquitous");
				Assert.That (mi.Path.ToString (), Does.EndWith ("/apitest.app"), "Path");
				Assert.That (mi.UbiquitousItemHasUnresolvedConflicts, Is.False, "UbiquitousItemHasUnresolvedConflicts");
				Assert.That (mi.UbiquitousItemIsDownloading, Is.False, "UbiquitousItemIsDownloading");
				Assert.That (mi.UbiquitousItemIsUploaded, Is.False, "UbiquitousItemIsUploaded");
				Assert.That (mi.UbiquitousItemIsUploading, Is.False, "UbiquitousItemIsUploading");
				Assert.That (mi.UbiquitousItemPercentDownloaded, Is.EqualTo (0), "UbiquitousItemPercentDownloaded");
				Assert.That (mi.UbiquitousItemPercentUploaded, Is.EqualTo (0), "UbiquitousItemPercentUploaded");
				Assert.That (mi.Url, Is.Null, "Url");

				Assert.That (mi.ContentType.ToString (), Is.EqualTo ("com.apple.application-bundle"), "ContentType");
				Assert.That (mi.ContentTypeTree.Length, Is.GreaterThan (1), "ContentTypeTree");
				Assert.That (mi.UbiquitousItemDownloadingStatus, Is.EqualTo (NSItemDownloadingStatus.Unknown), "UbiquitousItemDownloadingStatus");
				Assert.That (mi.UbiquitousItemDownloadingError, Is.Null, "UbiquitousItemDownloadingError");
				Assert.That (mi.UbiquitousItemUploadingError, Is.Null, "UbiquitousItemUploadingError");
				Assert.That (mi.UbiquitousItemContainerDisplayName, Is.Null, "UbiquitousItemContainerDisplayName");
				Assert.That (mi.UbiquitousItemUrlInLocalContainer, Is.Null, "UbiquitousItemUrlInLocalContainer");

				// 10.10
				if (TestRuntime.CheckXcodeVersion (6, 0)) {
					Assert.That (mi.UbiquitousItemDownloadRequested, Is.False, "UbiquitousItemDownloadRequested");
					Assert.That (mi.UbiquitousItemIsExternalDocument, Is.False, "UbiquitousItemIsExternalDocument");
				}
			}
		}
	}
}
#endif // __MACOS__
