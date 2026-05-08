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
				ClassicAssert.NotNull (mi.FileSystemContentChangeDate, "FileSystemContentChangeDate");
				ClassicAssert.NotNull (mi.FileSystemCreationDate, "FileSystemCreationDate");
				Assert.That (mi.FileSystemName.ToString (), Is.EqualTo ("apitest.app"), "FileSystemName");
				Assert.That (mi.FileSystemSize.UInt64Value, Is.GreaterThan (0), "FileSystemSize");
				ClassicAssert.False (mi.IsUbiquitous, "IsUbiquitous");
				Assert.That (mi.Path.ToString (), Does.EndWith ("/apitest.app"), "Path");
				ClassicAssert.False (mi.UbiquitousItemHasUnresolvedConflicts, "UbiquitousItemHasUnresolvedConflicts");
				ClassicAssert.False (mi.UbiquitousItemIsDownloading, "UbiquitousItemIsDownloading");
				ClassicAssert.False (mi.UbiquitousItemIsUploaded, "UbiquitousItemIsUploaded");
				ClassicAssert.False (mi.UbiquitousItemIsUploading, "UbiquitousItemIsUploading");
				Assert.That (mi.UbiquitousItemPercentDownloaded, Is.EqualTo (0), "UbiquitousItemPercentDownloaded");
				Assert.That (mi.UbiquitousItemPercentUploaded, Is.EqualTo (0), "UbiquitousItemPercentUploaded");
				ClassicAssert.Null (mi.Url, "Url");

				Assert.That (mi.ContentType.ToString (), Is.EqualTo ("com.apple.application-bundle"), "ContentType");
				Assert.That (mi.ContentTypeTree.Length, Is.GreaterThan (1), "ContentTypeTree");
				Assert.That (mi.UbiquitousItemDownloadingStatus, Is.EqualTo (NSItemDownloadingStatus.Unknown), "UbiquitousItemDownloadingStatus");
				ClassicAssert.Null (mi.UbiquitousItemDownloadingError, "UbiquitousItemDownloadingError");
				ClassicAssert.Null (mi.UbiquitousItemUploadingError, "UbiquitousItemUploadingError");
				ClassicAssert.Null (mi.UbiquitousItemContainerDisplayName, "UbiquitousItemContainerDisplayName");
				ClassicAssert.Null (mi.UbiquitousItemUrlInLocalContainer, "UbiquitousItemUrlInLocalContainer");

				// 10.10
				if (TestRuntime.CheckXcodeVersion (6, 0)) {
					ClassicAssert.False (mi.UbiquitousItemDownloadRequested, "UbiquitousItemDownloadRequested");
					ClassicAssert.False (mi.UbiquitousItemIsExternalDocument, "UbiquitousItemIsExternalDocument");
				}
			}
		}
	}
}
#endif // __MACOS__
