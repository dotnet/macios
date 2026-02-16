#if IOS
using System.Runtime.CompilerServices;

#nullable enable

namespace UIKit {
	public partial class UIPasteboard {
		const string selImages = "images";
		const string selSetImages_ = "setImages:";

		UIImage [] GetImageArray (IntPtr handle)
		{
			using (var array = new NSArray (handle)) {
				var ret = new UIImage [array.Count];
				for (uint i = 0; i < ret.Length; i++) {
					var obj = Runtime.GetNSObject (array.ValueAt (i));

					if (obj is NSData data) {
						ret [i] = new UIImage (data);
					} else if (obj is UIImage img) {
						ret [i] = img;
					} else if (obj is not null) {
						throw new System.InvalidOperationException ("Unexpected object type in UIPasteboard images array: " + obj.GetType ().FullName);
					}
				}

				return ret;
			}
		}

		// iOS6 introduced a breaking change in UIPasteboard.Images - they don't
		// return images anymore, they return NSData objects (contrary to what
		// their documentation says). Transparently create UIImage objects from
		// the NSData objects whenever required, so that we can keep our existing
		// API and not make users change their code.

		/// <summary>Gets or sets the images on the pasteboard.</summary>
		[CompilerGenerated]
		public virtual UIImage [] Images {
			[Export ("images", ArgumentSemantic.Copy)]
			get {
				global::UIKit.UIApplication.EnsureUIThread ();
				UIImage [] ret;
				if (IsDirectBinding) {
					ret = GetImageArray (ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (selImages)));
				} else {
					ret = GetImageArray (ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle (selImages)));
				}
				return ret;
			}

			[Export ("setImages:", ArgumentSemantic.Copy)]
			set {
				global::UIKit.UIApplication.EnsureUIThread ();
				var nsa_value = NSArray.FromNSObjects (value);
				var nsa_valueHandle = nsa_value.Handle;

				if (IsDirectBinding) {
					ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle (selSetImages_), nsa_valueHandle);
				} else {
					ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle (selSetImages_), nsa_valueHandle);
				}

				nsa_value.Dispose ();
			}
		}
	}
}

#endif // IOS
