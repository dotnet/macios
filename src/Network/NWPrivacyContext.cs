#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;
using OS_nw_privacy_context = System.IntPtr;
using OS_nw_resolver_config = System.IntPtr;
using OS_nw_proxy_config = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWPrivacyContext : NativeObject {

		public static NWPrivacyContext Default =>
			new NWPrivacyContext (NWPrivacyContextConstants._DefaultContext, false);

		[Preserve (Conditional = true)]
		internal NWPrivacyContext (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe OS_nw_privacy_context nw_privacy_context_create (IntPtr description);

		static unsafe OS_nw_privacy_context nw_privacy_context_create (string description)
		{
			using var descriptionPtr = new TransientString (description);
			return nw_privacy_context_create (descriptionPtr);
		}

		public NWPrivacyContext (string description)
			: base (nw_privacy_context_create (description), true) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_privacy_context_flush_cache (OS_nw_privacy_context privacyContext);

		public void FlushCache ()
			=> nw_privacy_context_flush_cache (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_privacy_context_disable_logging (OS_nw_privacy_context privacyContext);

		public void DisableLogging ()
			=> nw_privacy_context_disable_logging (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_privacy_context_require_encrypted_name_resolution (OS_nw_privacy_context privacyContext, byte requireEncryptedNameResolution, OS_nw_resolver_config fallbackResolverConfig);

		public void RequireEncryptedNameResolution (bool requireEncryptedNameResolution, NWResolverConfig? fallbackResolverConfig)
		{
			nw_privacy_context_require_encrypted_name_resolution (GetCheckedHandle (), requireEncryptedNameResolution.AsByte (), fallbackResolverConfig.GetHandle ());
			GC.KeepAlive (requireEncryptedNameResolution);
			GC.KeepAlive (fallbackResolverConfig);
		}

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_privacy_context_add_proxy (OS_nw_privacy_context privacy_context, OS_nw_proxy_config proxy_config);

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public void AddProxy (NWProxyConfig proxyConfig)
		{
			if (proxyConfig is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyConfig));
			nw_privacy_context_add_proxy (GetCheckedHandle (), proxyConfig.GetCheckedHandle ());
			GC.KeepAlive (proxyConfig);
		}

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_privacy_context_clear_proxies (OS_nw_privacy_context privacy_context);

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public void ClearProxies ()
		{
			nw_privacy_context_clear_proxies (GetCheckedHandle ());
		}

	}

}
