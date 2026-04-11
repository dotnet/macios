namespace PdfKit;

partial class PdfViewAnnotationHitEventArgs : NSNotificationEventArgs {
	// This property needs a manual binding, because it's not using a constant string value,
	// it's using a literal string value (as per Apple's documentation).
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public PdfKit.PdfAnnotation? AnnotationHit {
		get {
			var userinfo = Notification.UserInfo;
			if (userinfo is null)
				return null;

			using var key = new TransientCFString ("PDFAnnotationHit");
			var value = userinfo.LowlevelObjectForKey (key);
			return Runtime.GetNSObject<PdfKit.PdfAnnotation> (value);
		}
	}
}
