#if __MACOS__
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnit.Framework;
using AppKit;
using Foundation;
using ObjCRuntime;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SecurityInterfaceApiShapeTest {

		[Test]
		public void StronglyTypedArrays ()
		{
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.RunModal), new [] { typeof (SecCertificate []), typeof (bool) }), Is.Not.Null, "Certificates");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.RunModal), new [] { typeof (SecIdentity []), typeof (string) }), Is.Not.Null, "Identities");
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "Certificate policies");
			Assert.That (typeof (SFCertificateView).GetMethod (nameof (SFCertificateView.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "View policies");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "Identity policies");
		}

		[Test]
		public void RunModalOverloads ()
		{
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.RunModal), new [] { typeof (SecTrust), typeof (bool) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Certificate trust");
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.RunModal), new [] { typeof (SecCertificate []), typeof (bool) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Certificates");
			Assert.That (typeof (SFCertificateTrustPanel).GetMethod (nameof (SFCertificateTrustPanel.RunModal), new [] { typeof (SecTrust), typeof (string) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Trust");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.RunModal), new [] { typeof (SecIdentity []), typeof (string) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Identities");
			Assert.That (typeof (SFKeychainSavePanel).GetMethod (nameof (SFKeychainSavePanel.RunModal), new [] { typeof (string), typeof (string) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Save panel");
			Assert.That (typeof (SFKeychainSettingsPanel).GetMethod (nameof (SFKeychainSettingsPanel.RunModal), new [] { typeof (SecKeychainSettings).MakeByRefType (), typeof (SecKeychain) })?.ReturnType, Is.EqualTo (typeof (NSModalResponse)), "Settings panel");
		}

		[Test]
		public void ActionSheetOverloads ()
		{
			var actionType = typeof (Action<NSModalResponse>);
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.BeginSheet), new [] { typeof (NSWindow), typeof (SecTrust), typeof (bool), actionType }), Is.Not.Null, "Certificate trust");
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.BeginSheet), new [] { typeof (NSWindow), typeof (SecCertificate []), typeof (bool), actionType }), Is.Not.Null, "Certificates");
			Assert.That (typeof (SFCertificateTrustPanel).GetMethod (nameof (SFCertificateTrustPanel.BeginSheet), new [] { typeof (NSWindow), typeof (SecTrust), typeof (string), actionType }), Is.Not.Null, "Trust");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.BeginSheet), new [] { typeof (NSWindow), typeof (SecIdentity []), typeof (string), actionType }), Is.Not.Null, "Identities");
			Assert.That (typeof (SFKeychainSavePanel).GetMethod (nameof (SFKeychainSavePanel.BeginSheet), new [] { typeof (string), typeof (string), typeof (NSWindow), actionType }), Is.Not.Null, "Save panel");
			Assert.That (typeof (SFKeychainSettingsPanel).GetMethod (nameof (SFKeychainSettingsPanel.BeginSheet), new [] { typeof (NSWindow), typeof (SecKeychainSettings).MakeByRefType (), typeof (SecKeychain), actionType }), Is.Not.Null, "Settings panel");
		}

		[Test]
		public void ManualPropertiesAreStronglyTyped ()
		{
			Assert.That (typeof (SFAuthorizationPluginView).GetProperty (nameof (SFAuthorizationPluginView.EngineRef))?.PropertyType, Is.EqualTo (typeof (AuthorizationEngine)), "EngineRef");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.Certificate))?.PropertyType, Is.EqualTo (typeof (SecCertificate)), "Certificate");
			Assert.That (typeof (SFKeychainSavePanel).GetProperty (nameof (SFKeychainSavePanel.Keychain))?.PropertyType, Is.EqualTo (typeof (SecKeychain)), "Keychain");
		}

		[Test]
		public void Properties ()
		{
			Assert.That (typeof (SFAuthorizationPluginView).GetProperty (nameof (SFAuthorizationPluginView.LastError)), Is.Not.Null, "LastError");
			Assert.That (typeof (SFAuthorizationPluginView).GetProperty (nameof (SFAuthorizationPluginView.FirstKeyView)), Is.Not.Null, "FirstKeyView");
			Assert.That (typeof (SFAuthorizationPluginView).GetProperty (nameof (SFAuthorizationPluginView.FirstResponder)), Is.Not.Null, "FirstResponder");
			Assert.That (typeof (SFAuthorizationPluginView).GetProperty (nameof (SFAuthorizationPluginView.LastKeyView)), Is.Not.Null, "LastKeyView");
			Assert.That (typeof (SFAuthorizationView).GetProperty (nameof (SFAuthorizationView.Enabled))?.CanWrite, Is.True, "Enabled");
			Assert.That (typeof (SFCertificatePanel).GetProperty (nameof (SFCertificatePanel.ShowsHelp))?.CanWrite, Is.True, "Certificate ShowsHelp");
			Assert.That (typeof (SFCertificatePanel).GetProperty (nameof (SFCertificatePanel.HelpAnchor))?.CanWrite, Is.True, "Certificate HelpAnchor");
			Assert.That (typeof (SFCertificateTrustPanel).GetProperty (nameof (SFCertificateTrustPanel.InformativeText))?.CanWrite, Is.True, "Trust InformativeText");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.EditableTrust))?.CanWrite, Is.True, "EditableTrust");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.TrustDisplayed))?.CanWrite, Is.True, "TrustDisplayed");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.DetailsDisplayed))?.CanWrite, Is.True, "DetailsDisplayed");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.DetailsDisclosed))?.CanWrite, Is.True, "DetailsDisclosed");
			Assert.That (typeof (SFCertificateView).GetProperty (nameof (SFCertificateView.PoliciesDisclosed))?.CanWrite, Is.True, "PoliciesDisclosed");
			Assert.That (typeof (SFChooseIdentityPanel).GetProperty (nameof (SFChooseIdentityPanel.ShowsHelp))?.CanWrite, Is.True, "Identity ShowsHelp");
			Assert.That (typeof (SFChooseIdentityPanel).GetProperty (nameof (SFChooseIdentityPanel.HelpAnchor))?.CanWrite, Is.True, "Identity HelpAnchor");
			Assert.That (typeof (SFChooseIdentityPanel).GetProperty (nameof (SFChooseIdentityPanel.InformativeText))?.CanWrite, Is.True, "Identity InformativeText");
			Assert.That (typeof (SFChooseIdentityPanel).GetProperty (nameof (SFChooseIdentityPanel.Domain))?.CanWrite, Is.True, "Identity Domain");
		}

		[Test]
		public void AuthorizationCallbacksShape ()
		{
			Assert.That (typeof (INativeObject).IsAssignableFrom (typeof (AuthorizationCallbacks)), Is.False, "INativeObject");
			Assert.That (typeof (IDisposable).IsAssignableFrom (typeof (AuthorizationCallbacks)), Is.True, "IDisposable");
			Assert.That (typeof (AuthorizationCallbacks).GetProperty ("Handle"), Is.Null, "Handle");
		}

		[Test]
		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "The test intentionally reflects over the internal dispatcher type.")]
		[UnconditionalSuppressMessage ("Trimming", "IL2075", Justification = "The test intentionally reflects over the internal dispatcher methods.")]
		public void SheetDispatcher_IsOneShot ()
		{
			var dispatcherType = typeof (SFCertificatePanel).Assembly.GetType ("SecurityInterface.SecurityInterfaceSheetDidEndDispatcher");
			Assert.That (dispatcherType, Is.Not.Null, "Dispatcher type");
			if (dispatcherType is null)
				return;

			var create = dispatcherType.GetMethod ("Create", BindingFlags.Static | BindingFlags.NonPublic);
			var didEnd = dispatcherType.GetMethod ("DidEnd", BindingFlags.Instance | BindingFlags.Public);
			Assert.That (create, Is.Not.Null, "Create");
			Assert.That (didEnd, Is.Not.Null, "DidEnd");
			if (create is null || didEnd is null)
				return;

			var count = 0;
			var response = default (NSModalResponse);
			Action<NSModalResponse> action = value => {
				count++;
				response = value;
			};
			var dispatcher = create.Invoke (null, new object [] { action });
			Assert.That (dispatcher, Is.Not.Null, "Dispatcher");
			if (dispatcher is null)
				return;

			var arguments = new object? [] { null, (nint) NSModalResponse.OK, IntPtr.Zero };
			didEnd.Invoke (dispatcher, arguments);
			didEnd.Invoke (dispatcher, arguments);
			Assert.That (count, Is.EqualTo (1), "Count");
			Assert.That (response, Is.EqualTo (NSModalResponse.OK), "Response");
		}
	}
}
#endif // __MACOS__
