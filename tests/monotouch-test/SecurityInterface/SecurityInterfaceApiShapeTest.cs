#if __MACOS__
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnit.Framework;
using AppKit;
using Foundation;
using Security;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SecurityInterfaceApiShapeTest {

		[Test]
		public void StronglyTypedArrays ()
		{
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.RunModalForCertificates), new [] { typeof (SecCertificate []), typeof (bool) }), Is.Not.Null, "Certificates");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.RunModalForIdentities), new [] { typeof (SecIdentity []), typeof (string) }), Is.Not.Null, "Identities");
			Assert.That (typeof (SFCertificatePanel).GetMethod (nameof (SFCertificatePanel.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "Certificate policies");
			Assert.That (typeof (SFCertificateView).GetMethod (nameof (SFCertificateView.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "View policies");
			Assert.That (typeof (SFChooseIdentityPanel).GetMethod (nameof (SFChooseIdentityPanel.SetPolicies), new [] { typeof (SecPolicy []) }), Is.Not.Null, "Identity policies");
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
		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "The production factory has a DynamicDependency for the callback, and this test verifies that contract.")]
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
