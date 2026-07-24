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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 5)]
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

		/// <summary>Returns the last error that occurred during the authorization process.</summary>
		/// <returns>An <see cref="NSError" /> describing the last error, or <see langword="null" /> if no error occurred.</returns>
		[Export ("lastError")]
		[return: NullAllowed]
		NSError GetLastError ();

		/// <summary>Called after the view has been activated.</summary>
		[Export ("didActivate")]
		void DidActivate ();

		/// <summary>Called before the view activates, providing a dictionary of user information.</summary>
		/// <param name="userInformation">A dictionary containing user information, or <see langword="null" />.</param>
		[Export ("willActivateWithUser:")]
		void WillActivateWithUser ([NullAllowed] NSDictionary userInformation);

		/// <summary>Called after the view has been deactivated.</summary>
		[Export ("didDeactivate")]
		void DidDeactivate ();

		/// <summary>Returns the first view in the keyboard focus chain.</summary>
		/// <returns>The first <see cref="NSView" /> in the key view loop, or <see langword="null" />.</returns>
		[Export ("firstKeyView")]
		[return: NullAllowed]
		NSView GetFirstKeyView ();

		/// <summary>Returns the first responder for the view.</summary>
		/// <returns>The first <see cref="NSResponder" />, or <see langword="null" />.</returns>
		[Export ("firstResponder")]
		[return: NullAllowed]
		NSResponder GetFirstResponder ();

		/// <summary>Returns the last view in the keyboard focus chain.</summary>
		/// <returns>The last <see cref="NSView" /> in the key view loop, or <see langword="null" />.</returns>
		[Export ("lastKeyView")]
		[return: NullAllowed]
		NSView GetLastKeyView ();

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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
	interface ISFAuthorizationViewDelegate { }

	/// <summary>Delegate methods for responding to authorization state changes in an <see cref="SFAuthorizationView" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
		[Export ("setAuthorizationRights:")]
		void _SetAuthorizationRights (IntPtr authorizationRights);

		[Internal]
		[Export ("authorizationRights")]
		IntPtr _AuthorizationRights { get; }

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

		/// <summary>Enables or disables the view.</summary>
		/// <param name="enabled"><see langword="true" /> to enable the view; <see langword="false" /> to disable it.</param>
		[Export ("setEnabled:")]
		void SetEnabled (bool enabled);

		/// <summary>Gets a value indicating whether the view is currently enabled.</summary>
		[Export ("isEnabled")]
		bool IsEnabled { get; }

		/// <summary>Sets the authorization flags as a bitmask of AuthorizationFlags values.</summary>
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

	/// <summary>Interface representing the protocol methods of <see cref="SFCertificatePanelDelegate" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 4)]
	interface ISFCertificatePanelDelegate { }

	/// <summary>Delegate methods for the <see cref="SFCertificatePanel" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 4)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
		[Mac (10, 4)]
		[Export ("runModalForTrust:showGroup:")]
		NSModalResponse RunModalForTrust (SecTrust trust, bool showGroup);

		/// <summary>Displays the panel modally for the specified array of certificates.</summary>
		/// <param name="certificates">An array of certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		/// <returns>The button code that was pressed to dismiss the panel.</returns>
		[Export ("runModalForCertificates:showGroup:")]
		NSModalResponse RunModalForCertificates (SecCertificate [] certificates, bool showGroup);

		/// <summary>Displays the panel as a sheet for the specified <see cref="SecTrust" /> object.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="trust">The <see cref="SecTrust" /> object containing the certificates to display.</param>
		/// <param name="showGroup">Whether to display the certificate group.</param>
		[Mac (10, 4)]
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

		/// <summary>Sets the policies used to evaluate the certificates.</summary>
		/// <param name="policies">An <see cref="NSArray" /> of SecPolicy objects, a single SecPolicy, or <see langword="null" />.</param>
		[Mac (10, 4)]
		[Internal]
		[Export ("setPolicies:")]
		void _SetPolicies ([NullAllowed] NSObject policies);

		/// <summary>Gets the policies used to evaluate the certificates.</summary>
		[Mac (10, 4)]
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; }

		/// <summary>Sets the title of the default button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to use the default title.</param>
		[Mac (10, 4)]
		[Export ("setDefaultButtonTitle:")]
		void SetDefaultButtonTitle ([NullAllowed] string title);

		/// <summary>Sets the title of the alternate button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to hide the button.</param>
		[Mac (10, 4)]
		[Export ("setAlternateButtonTitle:")]
		void SetAlternateButtonTitle ([NullAllowed] string title);

		/// <summary>Sets whether the panel shows a help button.</summary>
		/// <param name="showsHelp"><see langword="true" /> to show the help button; otherwise, <see langword="false" />.</param>
		[Mac (10, 4)]
		[Export ("setShowsHelp:")]
		void SetShowsHelp (bool showsHelp);

		/// <summary>Gets a value indicating whether the panel shows a help button.</summary>
		[Mac (10, 4)]
		[Export ("showsHelp")]
		bool ShowsHelp { get; }

		/// <summary>Sets the help anchor string for the help button.</summary>
		/// <param name="anchor">The help anchor string, or <see langword="null" />.</param>
		[Mac (10, 4)]
		[Export ("setHelpAnchor:")]
		void SetHelpAnchor ([NullAllowed] string anchor);

		/// <summary>Gets the help anchor string.</summary>
		[Mac (10, 4)]
		[Export ("helpAnchor")]
		[NullAllowed]
		string HelpAnchor { get; }

		/// <summary>Gets the <see cref="SFCertificateView" /> used to display certificate details.</summary>
		[Mac (10, 4)]
		[NullAllowed]
		[Export ("certificateView")]
		SFCertificateView CertificateView { get; }
	}

	/// <summary>A panel for making trust decisions about certificates that cannot be verified.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
		NSModalResponse RunModalForTrust (SecTrust trust, [NullAllowed] string message);

		/// <summary>Displays the panel as a sheet for the specified <see cref="SecTrust" /> object with a message.</summary>
		/// <param name="docWindow">The window to which the sheet is attached.</param>
		/// <param name="modalDelegate">The delegate that receives the did-end callback, or <see langword="null" />.</param>
		/// <param name="didEndSelector">The selector invoked when the sheet ends, or <see langword="null" />.</param>
		/// <param name="contextInfo">A pointer to context information passed to the callback.</param>
		/// <param name="trust">The <see cref="SecTrust" /> object to evaluate.</param>
		/// <param name="message">A message to display in the panel, or <see langword="null" />.</param>
		[Export ("beginSheetForWindow:modalDelegate:didEndSelector:contextInfo:trust:message:")]
		void BeginSheet (NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo, SecTrust trust, [NullAllowed] string message);

		/// <summary>Sets the informative text displayed in the panel.</summary>
		/// <param name="informativeText">The informative text string, or <see langword="null" />.</param>
		[Mac (10, 5)]
		[Export ("setInformativeText:")]
		void SetInformativeText ([NullAllowed] string informativeText);

		/// <summary>Gets the informative text displayed in the panel.</summary>
		[Mac (10, 5)]
		[Export ("informativeText")]
		[NullAllowed]
		string InformativeText { get; }
	}

	/// <summary>A view that displays the contents of a certificate, with support for disclosable details and trust editing.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
	[BaseType (typeof (NSVisualEffectView))]
	interface SFCertificateView {

		/// <summary>Initializes the view with the specified frame rectangle.</summary>
		/// <param name="frameRect">The frame rectangle for the view.</param>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CoreGraphics.CGRect frameRect);

		[Internal]
		[Export ("setCertificate:")]
		void _SetCertificate (IntPtr certificate);

		[Internal]
		[Export ("certificate")]
		IntPtr _Certificate { get; }

		/// <summary>Sets the policies used to evaluate the certificate trust.</summary>
		/// <param name="policies">An <see cref="NSArray" /> of SecPolicy objects, a single SecPolicy, or <see langword="null" />.</param>
		[Mac (10, 4)]
		[Internal]
		[Export ("setPolicies:")]
		void _SetPolicies ([NullAllowed] NSObject policies);

		/// <summary>Gets the policies used for trust evaluation.</summary>
		[Mac (10, 4)]
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; }

		/// <summary>Sets whether the user can edit the trust settings.</summary>
		/// <param name="editable"><see langword="true" /> to allow trust editing; otherwise, <see langword="false" />.</param>
		[Export ("setEditableTrust:")]
		void SetEditableTrust (bool editable);

		/// <summary>Gets a value indicating whether the trust settings are editable.</summary>
		[Export ("isEditable")]
		bool IsEditable { get; }

		/// <summary>Sets whether trust information is displayed.</summary>
		/// <param name="display"><see langword="true" /> to show trust information; otherwise, <see langword="false" />.</param>
		[Export ("setDisplayTrust:")]
		void SetDisplayTrust (bool display);

		/// <summary>Gets a value indicating whether trust information is currently displayed.</summary>
		[Export ("isTrustDisplayed")]
		bool IsTrustDisplayed { get; }

		/// <summary>Saves the current trust settings to the user's trust database.</summary>
		[Export ("saveTrustSettings")]
		void SaveTrustSettings ();

		/// <summary>Sets whether certificate details are displayed.</summary>
		/// <param name="display"><see langword="true" /> to show certificate details; otherwise, <see langword="false" />.</param>
		[Mac (10, 4)]
		[Export ("setDisplayDetails:")]
		void SetDisplayDetails (bool display);

		/// <summary>Gets a value indicating whether certificate details are displayed.</summary>
		[Mac (10, 4)]
		[Export ("detailsDisplayed")]
		bool DetailsDisplayed { get; }

		/// <summary>Sets whether the details section is disclosed (expanded).</summary>
		/// <param name="disclosed"><see langword="true" /> to expand the details section; otherwise, <see langword="false" />.</param>
		[Mac (10, 5)]
		[Export ("setDetailsDisclosed:")]
		void SetDetailsDisclosed (bool disclosed);

		/// <summary>Gets a value indicating whether the details section is disclosed.</summary>
		[Mac (10, 5)]
		[Export ("detailsDisclosed")]
		bool DetailsDisclosed { get; }

		/// <summary>Sets whether the policies section is disclosed (expanded).</summary>
		/// <param name="disclosed"><see langword="true" /> to expand the policies section; otherwise, <see langword="false" />.</param>
		[Mac (10, 5)]
		[Export ("setPoliciesDisclosed:")]
		void SetPoliciesDisclosed (bool disclosed);

		/// <summary>Gets a value indicating whether the policies section is disclosed.</summary>
		[Mac (10, 5)]
		[Export ("policiesDisclosed")]
		bool PoliciesDisclosed { get; }

		/// <summary>Notification posted when the disclosure state of details or policies changes.</summary>
		[Mac (10, 5)]
		[Notification]
		[Field ("SFCertificateViewDisclosureStateDidChange")]
		NSString DisclosureStateDidChangeNotification { get; }
	}

	/// <summary>Interface representing the protocol methods of <see cref="SFChooseIdentityPanelDelegate" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 4)]
	interface ISFChooseIdentityPanelDelegate { }

	/// <summary>Delegate methods for the <see cref="SFChooseIdentityPanel" />.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 4)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
		NSModalResponse RunModalForIdentities (SecIdentity [] identities, [NullAllowed] string message);

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

		/// <summary>Sets the policies used to evaluate the identities.</summary>
		/// <param name="policies">An <see cref="NSArray" /> of SecPolicy objects, a single SecPolicy, or <see langword="null" />.</param>
		[Mac (10, 4)]
		[Internal]
		[Export ("setPolicies:")]
		void _SetPolicies ([NullAllowed] NSObject policies);

		/// <summary>Gets the policies used to evaluate the identities.</summary>
		[Mac (10, 4)]
		[Internal]
		[Export ("policies")]
		IntPtr _Policies { get; }

		/// <summary>Sets the title of the default button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to use the default title.</param>
		[Mac (10, 4)]
		[Export ("setDefaultButtonTitle:")]
		void SetDefaultButtonTitle ([NullAllowed] string title);

		/// <summary>Sets the title of the alternate button.</summary>
		/// <param name="title">The button title, or <see langword="null" /> to hide the button.</param>
		[Mac (10, 4)]
		[Export ("setAlternateButtonTitle:")]
		void SetAlternateButtonTitle ([NullAllowed] string title);

		/// <summary>Sets whether the panel shows a help button.</summary>
		/// <param name="showsHelp"><see langword="true" /> to show the help button; otherwise, <see langword="false" />.</param>
		[Mac (10, 4)]
		[Export ("setShowsHelp:")]
		void SetShowsHelp (bool showsHelp);

		/// <summary>Gets a value indicating whether the panel shows a help button.</summary>
		[Mac (10, 4)]
		[Export ("showsHelp")]
		bool ShowsHelp { get; }

		/// <summary>Sets the help anchor string for the help button.</summary>
		/// <param name="anchor">The help anchor string, or <see langword="null" />.</param>
		[Mac (10, 4)]
		[Export ("setHelpAnchor:")]
		void SetHelpAnchor ([NullAllowed] string anchor);

		/// <summary>Gets the help anchor string.</summary>
		[Mac (10, 4)]
		[Export ("helpAnchor")]
		[NullAllowed]
		string HelpAnchor { get; }

		/// <summary>Sets the informative text displayed in the panel.</summary>
		/// <param name="informativeText">The informative text string, or <see langword="null" />.</param>
		[Mac (10, 5)]
		[Export ("setInformativeText:")]
		void SetInformativeText ([NullAllowed] string informativeText);

		/// <summary>Gets the informative text displayed in the panel.</summary>
		[Mac (10, 5)]
		[Export ("informativeText")]
		[NullAllowed]
		string InformativeText { get; }

		/// <summary>Sets the domain string used to filter identities.</summary>
		/// <param name="domainString">The domain string, or <see langword="null" />.</param>
		[Mac (10, 5)]
		[Export ("setDomain:")]
		void SetDomain ([NullAllowed] string domainString);

		/// <summary>Gets the domain string used to filter identities.</summary>
		[Mac (10, 5)]
		[Export ("domain")]
		[NullAllowed]
		string Domain { get; }
	}

	/// <summary>A table cell view used in the identity chooser panel to display identity and issuer information.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 13)]
	[BaseType (typeof (NSTableCellView))]
	interface SFChooseIdentityTableCellView {

		/// <summary>Initializes the cell view with the specified frame rectangle.</summary>
		/// <param name="frameRect">The frame rectangle for the cell view.</param>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CoreGraphics.CGRect frameRect);

		/// <summary>Gets or sets the text field that displays the certificate issuer name.</summary>
		[NullAllowed]
		[Export ("issuerTextField", ArgumentSemantic.Assign)]
		NSTextField IssuerTextField { get; set; }
	}

	/// <summary>A save panel for creating a new keychain file.</summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
	[BaseType (typeof (NSSavePanel))]
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
		NSModalResponse RunModalForDirectory ([NullAllowed] string path, [NullAllowed] string name);

		/// <summary>Sets the password for the new keychain.</summary>
		/// <param name="password">The password to use, or <see langword="null" />.</param>
		[Export ("setPassword:")]
		void SetPassword ([NullAllowed] string password);

		[Internal]
		[Export ("keychain")]
		IntPtr _Keychain { get; }

		/// <summary>Gets the last error that occurred during keychain creation, or <see langword="null" /> if no error.</summary>
		[Mac (10, 5)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 3)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 5)]
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
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 5)]
	[Static]
	interface SFAuthorizationPluginViewExceptions {

		/// <summary>The name of the exception raised when an authorization plugin view cannot be displayed.</summary>
		[Field ("SFDisplayViewException")]
		NSString DisplayViewException { get; }
	}
}
