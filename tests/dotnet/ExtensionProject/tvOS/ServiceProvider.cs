using System;
using Foundation;
using TVServices;

namespace MyTVServicesExtension {
	[Register ("ServiceProvider")]
	public class ServiceProvider : NSObject, ITVTopShelfProvider {
		protected ServiceProvider (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public TVContentItem [] TopShelfItems {
			[Export ("topShelfItems")]
			get {
#pragma warning disable CA1422
				return new TVContentItem [] { new TVContentItem (new TVContentIdentifier ("identifier", null)) { Title = "title" } };
#pragma warning restore CA1422
			}
		}

		public TVTopShelfContentStyle TopShelfStyle {
			[Export ("topShelfStyle")]
			get {
				return TVTopShelfContentStyle.Inset;
			}
		}
	}
}
