using System;

using CoreSpotlight;
using Foundation;

namespace SpotlightImportExtensionTest {
	[Register ("ImportExtension")]
	public class ImportExtension : CSImportExtension {
		public ImportExtension (IntPtr handle) : base (handle)
		{
		}

		public override bool Update (CSSearchableItemAttributeSet attributes, NSUrl contentUrl, out NSError error)
		{
			error = null;
			attributes.Title = "Test Spotlight Import";
			attributes.ContentDescription = "Imported by test extension";
			return true;
		}
	}
}
