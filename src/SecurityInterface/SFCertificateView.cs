#nullable enable

using System;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	public partial class SFCertificateView {

		/// <summary>Gets the policies used to evaluate the displayed certificate.</summary>
		public SecPolicy [] Policies {
			get {
				if (_Policies == IntPtr.Zero)
					throw new InvalidOperationException ("The native certificate view returned a null policies array.");
				return NSArray.NonNullArrayFromHandleDropNullElements<SecPolicy> (_Policies);
			}
		}

		/// <summary>Gets or sets the certificate displayed by the view.</summary>
		public SecCertificate? Certificate {
			get => Runtime.GetINativeObject<SecCertificate> (_Certificate, owns: false);
			set {
				_SetCertificate (value.GetHandle ());
				GC.KeepAlive (value);
			}
		}

		/// <summary>Sets the policies used to evaluate the displayed certificate.</summary>
		public void SetPolicies (params SecPolicy [] policies)
		{
			ArgumentNullException.ThrowIfNull (policies);
			if (policies.Length == 0)
				throw new ArgumentException ("At least one policy is required.", nameof (policies));
			using var array = NSArray.FromNativeObjects (policies);
			_SetPolicies (array);
			GC.KeepAlive (policies);
		}

		/// <summary>Resets the view to the default X.509 policy.</summary>
		public void ResetPolicies ()
		{
			_SetPolicies (null);
		}
	}
}
