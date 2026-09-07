#nullable enable

#if !__MACCATALYST__
using System.Threading.Tasks;

namespace VideoSubscriberAccount {

	public partial class VSAccountMetadataRequest {
		/// <summary>Gets or sets the authentication schemes supported by the account provider.</summary>
		/// <value>An array of supported authentication schemes.</value>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public VSAccountProviderAuthenticationScheme [] SupportedAuthenticationSchemes {
			get {
				return VSAccountProviderAuthenticationSchemeExtensions.GetValues (SupportedAuthenticationSchemesString);
			}
			set {
				var constants = value.GetConstants ();
				if (constants is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SupportedAuthenticationSchemesString = constants!;
			}
		}
	}
}
#endif // !__MACCATALYST__
