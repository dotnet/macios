//
// securityinterface.cs: Bindings for the SecurityInterface framework (macOS only)
//
// Copyright 2025 Microsoft Corp.
//

#nullable enable

using System;
using AppKit;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

	/// <summary>Host view for authorization plugin mechanisms, subclassed to provide custom UI for the loginwindow authorization process.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFAuthorizationPluginView {

		[Internal]
		[Export ("initWithCallbacks:andEngineRef:")]
		NativeHandle _InitWithCallbacks (IntPtr callbacks, IntPtr engineRef);

		[Internal]
		[Export ("engineRef")]
		IntPtr _EngineRef { get; }

		[Internal]
		[Export ("callbacks")]
		IntPtr _Callbacks { get; }

		/// <summary>Called when the user presses a button in the UI.</summary>
		/// <param name="buttonType">The type of button that was pressed.</param>
		[Export ("buttonPressed:")]
		void ButtonPressed (SFButtonType buttonType);

		/// <summary>Gets the last error that occurred during the authorization process.</summary>
		[Export ("lastError")]
		[NullAllowed]
		NSError LastError { get; }

		/// <summary>Called after the view has been activated.</summary>
		[Export ("didActivate")]
		void DidActivate ();

		/// <summary>Called before the view activates, providing a dictionary of user information.</summary>
		/// <param name="userInformation">A dictionary containing user information, or <see langword="null" />.</param>
		[Export ("willActivateWithUser:")]
		void WillActivate ([NullAllowed] NSDictionary userInformation);

		/// <summary>Called after the view has been deactivated.</summary>
		[Export ("didDeactivate")]
		void DidDeactivate ();

		/// <summary>Gets the first view in the keyboard focus chain.</summary>
		[Export ("firstKeyView")]
		[NullAllowed]
		NSView FirstKeyView { get; }

		/// <summary>Gets the first responder for the view.</summary>
		[Export ("firstResponder")]
		[NullAllowed]
		NSResponder FirstResponder { get; }

		/// <summary>Gets the last view in the keyboard focus chain.</summary>
		[Export ("lastKeyView")]
		[NullAllowed]
		NSView LastKeyView { get; }

		/// <summary>Enables or disables the view.</summary>
		/// <param name="enabled"><see langword="true" /> to enable the view; <see langword="false" /> to disable it.</param>
		[Export ("setEnabled:")]
		void SetEnabled (bool enabled);

		/// <summary>Returns the view for the specified view type.</summary>
		/// <param name="viewType">The type of view to retrieve.</param>
		/// <returns>The <see cref="NSView" /> for the specified type, or <see langword="null" />.</returns>
		[Export ("viewForType:")]
		[return: NullAllowed]
		NSView GetView (SFViewType viewType);

		/// <summary>Displays the authorization plugin view.</summary>
		[Export ("displayView")]
		void DisplayView ();

		/// <summary>Enables or disables the specified button.</summary>
		/// <param name="buttonType">The button to enable or disable.</param>
		/// <param name="enabled"><see langword="true" /> to enable the button; <see langword="false" /> to disable it.</param>
		[Export ("setButton:enabled:")]
		void SetButton (SFButtonType buttonType, bool enabled);

		/// <summary>Updates the view to reflect current state.</summary>
		[Export ("updateView")]
		void UpdateView ();
	}

	/// <summary>Interface representing the protocol methods of <see cref="SFAuthorizationViewDelegate" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	interface ISFAuthorizationViewDelegate { }

	/// <summary>Delegate methods for responding to authorization state changes in an <see cref="SFAuthorizationView" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[Protocol (IsInformal = true, BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SFAuthorizationViewDelegate {

		/// <summary>Called when the authorization view has been authorized.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> that was authorized.</param>
		[Export ("authorizationViewDidAuthorize:")]
		void DidAuthorize (SFAuthorizationView view);

		/// <summary>Called when the authorization view has been deauthorized.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> that was deauthorized.</param>
		[Export ("authorizationViewDidDeauthorize:")]
		void DidDeauthorize (SFAuthorizationView view);

		/// <summary>Called to determine whether the authorization view should deauthorize.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> requesting deauthorization.</param>
		/// <returns><see langword="true" /> to allow deauthorization; otherwise, <see langword="false" />.</returns>
		[Export ("authorizationViewShouldDeauthorize:")]
		bool ShouldDeauthorize (SFAuthorizationView view);

		/// <summary>Called when the authorization view has created an authorization reference.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> that created the authorization.</param>
		[Export ("authorizationViewCreatedAuthorization:")]
		void CreatedAuthorization (SFAuthorizationView view);

		/// <summary>Called when the authorization view has released its authorization reference.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> that released the authorization.</param>
		[Export ("authorizationViewReleasedAuthorization:")]
		void ReleasedAuthorization (SFAuthorizationView view);

		/// <summary>Called when the authorization view has been hidden.</summary>
		/// <param name="view">The <see cref="SFAuthorizationView" /> that was hidden.</param>
		[Export ("authorizationViewDidHide:")]
		void DidHide (SFAuthorizationView view);
	}

	/// <summary>A view that displays a lock icon for controlling access to a privileged operation.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSView))]
	interface SFAuthorizationView {

		/// <summary>Initializes the view with the specified frame rectangle.</summary>
		/// <param name="frameRect">The frame rectangle for the view.</param>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CoreGraphics.CGRect frameRect);

		[Internal]
		[Export ("setString:")]
		void _SetAuthorizationString (IntPtr authorizationString);

		[Internal]
		[Export ("authorizationRights")]
		IntPtr _AuthorizationRights { get; set; }

		/// <summary>Gets the authorization object associated with this view, or <see langword="null" /> if not yet authorized.</summary>
		[Export ("authorization")]
		[NullAllowed]
		NSObject Authorization { get; }

		/// <summary>Updates the authorization status and lock icon state.</summary>
		/// <param name="sender">The object that initiated the update, or <see langword="null" />.</param>
		/// <returns><see langword="true" /> if the status was updated successfully; otherwise, <see langword="false" />.</returns>
		[Export ("updateStatus:")]
		bool UpdateStatus ([NullAllowed] NSObject sender);

		/// <summary>Enables or disables automatic status updates.</summary>
		/// <param name="autoupdate"><see langword="true" /> to enable auto-updating; <see langword="false" /> to disable it.</param>
		[Export ("setAutoupdate:")]
		void SetAutoupdate (bool autoupdate);

		/// <summary>Enables or disables automatic status updates with a specified interval.</summary>
		/// <param name="autoupdate"><see langword="true" /> to enable auto-updating; <see langword="false" /> to disable it.</param>
		/// <param name="interval">The interval in seconds between automatic updates.</param>
		[Export ("setAutoupdate:interval:")]
		void SetAutoupdate (bool autoupdate, double interval);

		/// <summary>Gets the current authorization state of the view.</summary>
		[Export ("authorizationState")]
		SFAuthorizationViewState AuthorizationState { get; }

		/// <summary>Gets or sets a value indicating whether the view is enabled.</summary>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Sets the authorization flags as a bitmask of AuthorizationFlags values.</summary>
		/// <remarks>The native API does not provide a corresponding getter.</remarks>
		/// <param name="flags">A bitmask of authorization flag values.</param>
		[Export ("setFlags:")]
		void SetFlags (AuthorizationFlags flags);

		/// <summary>Gets or sets the weak delegate that receives authorization state change notifications.</summary>
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>Gets or sets the delegate that receives authorization state change notifications.</summary>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISFAuthorizationViewDelegate Delegate { get; set; }

		/// <summary>Attempts to authorize.</summary>
		/// <param name="sender">The object that initiated the authorization, or <see langword="null" />.</param>
		/// <returns><see langword="true" /> if authorization succeeded; otherwise, <see langword="false" />.</returns>
		[Export ("authorize:")]
		bool Authorize ([NullAllowed] NSObject sender);

		/// <summary>Attempts to deauthorize.</summary>
		/// <param name="sender">The object that initiated the deauthorization, or <see langword="null" />.</param>
		/// <returns><see langword="true" /> if deauthorization succeeded; otherwise, <see langword="false" />.</returns>
		[Export ("deauthorize:")]
		bool Deauthorize ([NullAllowed] NSObject sender);
	}

	/// <summary>Delegate methods for the <see cref="SFCertificatePanel" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[Protocol (IsInformal = true, BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SFCertificatePanelDelegate {

		/// <summary>Called when the user clicks the help button in the certificate panel.</summary>
		/// <param name="sender">The <see cref="SFCertificatePanel" /> that sent the message.</param>
		/// <returns><see langword="true" /> if help was displayed; otherwise, <see langword="false" />.</returns>
		[Export ("certificatePanelShowHelp:")]
		bool ShowHelp (SFCertificatePanel sender);
	}

	/// <summary>A panel that displays one or more certificates, presented as a modal dialog or a sheet.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSPanel))]
	interface SFCertificatePanel {

		/// <summary>Gets the shared certificate panel instance.</summary>
		[Static]
		[Export ("sharedCertificatePanel")]
		SFCertificatePanel SharedCertificatePanel { get; }

		/// <summary>Displays the panel modally for the specified <see cref="SecTrust" /> object.</summary>
		/// <param name="trust">The <see cref="SecTrust" /> object containing the certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForTrust:showGroup:")]
		NSModalResponse RunModal (SecTrust trust, bool showGroup);

		/// <summary>Displays the panel modally for the specified array of certificates.</summary>
		/// <param name="certificates">An array of certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForCertificates:showGroup:")]
		NSModalResponse RunModal (SecCertificate [] certificates, bool showGroup);

		/// <summary>Displays the panel as a sheet for the specified <see cref="SecTrust" /> object.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="trust">The <see cref="SecTrust" /> object containing the certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:trust:showGroup:")]
		void BeginSheet (NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, SecTrust trust, bool showGroup);

		/// <summary>Displays the panel as a sheet for the specified array of certificates.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="certificates">An array of certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:certificates:showGroup:")]
		void BeginSheet (NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, SecCertificate [] certificates, bool showGroup);

		/// <summary>Gets or sets the policies used to evaluate the certificates.</summary>
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; set; }

		/// <summary>Sets the title of the default button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to use the default title.</param>
		[Export ("setDefaultButtonTitle:")]
		void SetDefaultButtonTitle ([NullAllowed] string title);

		/// <summary>Sets the title of the alternate button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to hide the button.</param>
		[Export ("setAlternateButtonTitle:")]
		void SetAlternateButtonTitle ([NullAllowed] string title);

		/// <summary>Gets or sets a value indicating whether the panel shows a help button.</summary>
		[Export ("showsHelp")]
		bool ShowsHelp { get; set; }

		/// <summary>Gets or sets the help anchor string for the help button.</summary>
		[Export ("helpAnchor")]
		[NullAllowed]
		string HelpAnchor { get; set; }

		/// <summary>Gets the <see cref="SFCertificateView" /> used to display certificate details.</summary>
		[NullAllowed]
		[Export ("certificateView")]
		SFCertificateView CertificateView { get; }
	}

	/// <summary>A panel for making trust decisions about certificates that cannot be verified.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (SFCertificatePanel))]
	interface SFCertificateTrustPanel {

		/// <summary>Gets the shared certificate trust panel instance.</summary>
		[Static]
		[Export ("sharedCertificateTrustPanel")]
		SFCertificateTrustPanel SharedCertificateTrustPanel { get; }

		/// <summary>Displays the panel modally for the specified <see cref="SecTrust" /> object with a descriptive message.</summary>
		/// <param name="trust">The <see cref="SecTrust" /> object to evaluate.</param>
		/// <param name="message">A message to display in the panel, or <see langword="null" />.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForTrust:message:")]
		NSModalResponse RunModal (SecTrust trust, [NullAllowed] string message);

		/// <summary>Displays the panel as a sheet for the specified <see cref="SecTrust" /> object with a message.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="trust">The <see cref="SecTrust" /> object to evaluate.</param>
		/// <param name="message">A message to display in the panel, or <see langword="null" />.</param>
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:trust:message:")]
		void BeginSheet (NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, SecTrust trust, [NullAllowed] string message);

		/// <summary>Gets or sets the informative text displayed in the panel.</summary>
		[Export ("informativeText")]
		[NullAllowed]
		string InformativeText { get; set; }
	}

	/// <summary>A view that displays the contents of a certificate, with support for disclosable details and trust editing.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSVisualEffectView))]
	interface SFCertificateView {

		/// <summary>Initializes the view with the specified frame rectangle.</summary>
		/// <param name="frameRect">The frame rectangle for the view.</param>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CoreGraphics.CGRect frameRect);

		[Internal]
		[Export ("certificate")]
		IntPtr _Certificate { get; set; }

		/// <summary>Gets or sets the policies used for trust evaluation.</summary>
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; set; }

		/// <summary>Gets or sets a value indicating whether the user can edit the trust settings.</summary>
		[Export ("editableTrust")]
		bool EditableTrust { [Bind ("isEditable")] get; set; }

		/// <summary>Gets or sets a value indicating whether trust information is displayed.</summary>
		[Export ("displayTrust")]
		bool TrustDisplayed { [Bind ("isTrustDisplayed")] get; set; }

		/// <summary>Saves the current trust settings to the user's trust database.</summary>
		[Export ("saveTrustSettings")]
		void SaveTrustSettings ();

		/// <summary>Gets or sets a value indicating whether certificate details are displayed.</summary>
		[Export ("displayDetails")]
		bool DetailsDisplayed { [Bind ("detailsDisplayed")] get; set; }

		/// <summary>Gets or sets a value indicating whether the details section is disclosed.</summary>
		[Export ("detailsDisclosed")]
		bool DetailsDisclosed { get; set; }

		/// <summary>Gets or sets a value indicating whether the policies section is disclosed.</summary>
		[Export ("policiesDisclosed")]
		bool PoliciesDisclosed { get; set; }

		/// <summary>Notification posted when the disclosure state of details or policies changes.</summary>
		[Notification]
		[Field ("SFCertificateViewDisclosureStateDidChange")]
		NSString DisclosureStateDidChangeNotification { get; }
	}

	/// <summary>Delegate methods for the <see cref="SFChooseIdentityPanel" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[Protocol (IsInformal = true, BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SFChooseIdentityPanelDelegate {

		/// <summary>Called when the user clicks the help button in the choose identity panel.</summary>
		/// <param name="sender">The <see cref="SFChooseIdentityPanel" /> that sent the message.</param>
		/// <returns><see langword="true" /> if help was displayed; otherwise, <see langword="false" />.</returns>
		[Export ("chooseIdentityPanelShowHelp:")]
		bool ShowHelp (SFChooseIdentityPanel sender);
	}

	/// <summary>A panel that lets the user choose a digital identity (certificate and private key pair) from a list.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSPanel))]
	interface SFChooseIdentityPanel {

		/// <summary>Gets the shared choose identity panel instance.</summary>
		[Static]
		[Export ("sharedChooseIdentityPanel")]
		SFChooseIdentityPanel SharedChooseIdentityPanel { get; }

		/// <summary>Displays the panel modally with the specified array of identities and a message.</summary>
		/// <param name="identities">An array of <see cref="SecIdentity" /> objects to display.</param>
		/// <param name="message">A message to display in the panel, or <see langword="null" />.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForIdentities:message:")]
		NSModalResponse RunModal (SecIdentity [] identities, [NullAllowed] string message);

		/// <summary>Displays the panel as a sheet with the specified identities and message.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="identities">An array of <see cref="SecIdentity" /> objects to display.</param>
		/// <param name="message">A message to display in the panel, or <see langword="null" />.</param>
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:identities:message:")]
		void BeginSheet (NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, SecIdentity [] identities, [NullAllowed] string message);

		/// <summary>Gets the identity that the user chose from the list.</summary>
		[Export ("identity")]
		[NullAllowed]
		SecIdentity Identity { get; }

		/// <summary>Gets or sets the policies used to evaluate the identities.</summary>
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; set; }

		/// <summary>Sets the title of the default button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to use the default title.</param>
		[Export ("setDefaultButtonTitle:")]
		void SetDefaultButtonTitle ([NullAllowed] string title);

		/// <summary>Sets the title of the alternate button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to hide the button.</param>
		[Export ("setAlternateButtonTitle:")]
		void SetAlternateButtonTitle ([NullAllowed] string title);

		/// <summary>Gets or sets a value indicating whether the panel shows a help button.</summary>
		[Export ("showsHelp")]
		bool ShowsHelp { get; set; }

		/// <summary>Gets or sets the help anchor string.</summary>
		[Export ("helpAnchor")]
		[NullAllowed]
		string HelpAnchor { get; set; }

		/// <summary>Gets or sets the informative text displayed in the panel.</summary>
		[Export ("informativeText")]
		[NullAllowed]
		string InformativeText { get; set; }

		/// <summary>Gets or sets the domain string used to filter identities.</summary>
		[Export ("domain")]
		[NullAllowed]
		string Domain { get; set; }
	}

	/// <summary>A save panel for creating a new keychain file.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSSavePanel))]
	[DisableDefaultCtor]
	interface SFKeychainSavePanel {

		/// <summary>Gets the shared keychain save panel instance.</summary>
		[Static]
		[Export ("sharedKeychainSavePanel")]
		SFKeychainSavePanel SharedKeychainSavePanel { get; }

		/// <summary>Displays the panel modally starting in the specified directory with a suggested filename.</summary>
		/// <param name="path">The directory to start in, or <see langword="null" /> for the default.</param>
		/// <param name="name">The suggested filename, or <see langword="null" />.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForDirectory:file:")]
		NSModalResponse RunModal ([NullAllowed] string path, [NullAllowed] string name);

		/// <summary>Sets the password for the new keychain.</summary>
		/// <param name="password">The password to use, or <see langword="null" />.</param>
		[Export ("setPassword:")]
		void SetPassword ([NullAllowed] string password);

		[Internal]
		[Export ("keychain")]
		IntPtr _Keychain { get; }

		/// <summary>Gets the last error that occurred during keychain creation, or <see langword="null" /> if no error.</summary>
		[Export ("error")]
		[NullAllowed]
		NSError Error { get; }

		/// <summary>Displays the panel as a sheet starting in the specified directory with a suggested filename.</summary>
		/// <param name="path">The directory to start in, or <see langword="null" /> for the default.</param>
		/// <param name="name">The suggested filename, or <see langword="null" />.</param>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		[Export ("beginSheetForDirectory:file:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet ([NullAllowed] string path, [NullAllowed] string name, [NullAllowed] NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
	}

	/// <summary>A panel for editing keychain settings such as lock-on-sleep and auto-lock interval.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[BaseType (typeof (NSPanel))]
	interface SFKeychainSettingsPanel {

		/// <summary>Gets the shared keychain settings panel instance.</summary>
		[Static]
		[Export ("sharedKeychainSettingsPanel")]
		SFKeychainSettingsPanel SharedKeychainSettingsPanel { get; }

		[Internal]
		[Export ("runModalForSettings:keychain:")]
		NSModalResponse _RunModalForSettings (ref SecKeychainSettings settings, IntPtr keychain);

		[Internal]
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:settings:keychain:")]
		void _BeginSheet ([NullAllowed] NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, ref SecKeychainSettings settings, IntPtr keychain);
	}

	/// <summary>Contains keys for the user information dictionary passed to authorization plugin views.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[Static]
	interface SFAuthorizationPluginViewKeys {

		/// <summary>Key for the user name value in the user information dictionary.</summary>
		[Field ("SFAuthorizationPluginViewUserNameKey")]
		NSString UserNameKey { get; }

		/// <summary>Key for the user short name value in the user information dictionary.</summary>
		[Field ("SFAuthorizationPluginViewUserShortNameKey")]
		NSString UserShortNameKey { get; }
	}

	/// <summary>Contains exception names raised by authorization plugin views.</summary>
	[NoiOS, NoTV, NoMacCatalyst]
	[Static]
	interface SFAuthorizationPluginViewExceptions {

		/// <summary>The name of the exception raised when an authorization plugin view cannot be displayed.</summary>
		[Field ("SFDisplayViewException")]
		NSString DisplayViewException { get; }
	}
}
