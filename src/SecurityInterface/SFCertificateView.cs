#nullable enable

using System;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	public partial class SFCertificateView {

		/// <summary>Gets or sets the certificate displayed by the view.</summary>
		public SecCertificate? Certificate {
			get => Runtime.GetINativeObject<SecCertificate> (_Certificate, owns: false);
			set {
				_Certificate = value.GetHandle ();
				GC.KeepAlive (value);
			}
		}

		/// <summary>Gets or sets the policies used to evaluate the displayed certificate.</summary>
		/// <remarks>Set this property to <see langword="null" /> to restore the default X.509 policy.</remarks>
		public SecPolicy []? Policies {
			get => _Policies == IntPtr.Zero ? null : NSArray.NonNullArrayFromHandleDropNullElements<SecPolicy> (_Policies);
			set {
				if (value is null) {
					_Policies = IntPtr.Zero;
					return;
				}
				if (value.Length == 0)
					throw new ArgumentException ("At least one policy is required.", nameof (value));
				using var array = NSArray.FromNativeObjects (value);
				_Policies = array.Handle;
				GC.KeepAlive (value);
			}
		}
	}
}
