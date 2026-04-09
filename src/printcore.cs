using System;
using AppKit;
using Foundation;
using ObjCRuntime;

namespace PrintCore {

	/// <summary>
	/// Protocol interface for <see cref="PDEPanel" />, which handles a single print dialog panel.
	/// </summary>
	interface IPDEPanel { }

	/// <summary>
	/// Protocol interface for <see cref="PDEPlugIn" />, a factory that creates print dialog extension panels.
	/// </summary>
	interface IPDEPlugIn { }

	/// <summary>
	/// Protocol interface for <see cref="PDEPlugInCallbackProtocol" />, which provides printing dialog extensions
	/// with information about the current printer and print job.
	/// </summary>
	interface IPDEPlugInCallbackProtocol { }

	/// <summary>
	/// Represents a single print dialog panel that manages its own UI, settings persistence, and PPD option handling.
	/// </summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface PDEPanel {
		/// <summary>
		/// Called when the panel is about to be displayed. Use this to perform any final preparations,
		/// such as lazily initializing the user interface.
		/// </summary>
		[Abstract]
		[Export ("willShow")]
		void WillShow ();

		/// <summary>
		/// Called before the panel is removed from view. Return <see langword="false" /> to prevent
		/// hiding (for example, if the user's input is invalid), or <see langword="true" /> to allow it.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if the panel may be hidden; otherwise, <see langword="false" />.
		/// </returns>
		[Abstract]
		[Export ("shouldHide")]
		bool ShouldHide ();

		/// <summary>
		/// Writes the current values from the panel's UI controls to the permanent store
		/// (print settings or page format, depending on the panel type).
		/// </summary>
		/// <param name="error">On failure, contains an <see cref="NSError" /> describing what went wrong.</param>
		/// <returns>
		/// <see langword="true" /> if the values were saved successfully; otherwise, <see langword="false" />.
		/// </returns>
		[Abstract]
		[Export ("saveValuesAndReturnError:")]
		bool SaveValues ([NullAllowed] out NSError error);

		/// <summary>
		/// Refreshes the panel's UI elements by reading values from the page format or print settings.
		/// This is called before the panel is first shown and whenever the user selects a new preset.
		/// </summary>
		/// <param name="error">On failure, contains an <see cref="NSError" /> describing what went wrong.</param>
		/// <returns>
		/// <see langword="true" /> if the values were restored successfully; otherwise, <see langword="false" />.
		/// </returns>
		[Abstract]
		[Export ("restoreValuesAndReturnError:")]
		bool RestoreValues ([NullAllowed] out NSError error);

		/// <summary>
		/// Returns the PPD option keys that this panel is responsible for displaying.
		/// Any unclaimed keys will receive dynamically generated UI in the Printer Features panel.
		/// </summary>
		/// <returns>
		/// An array of PPD option key strings, or <see langword="null" /> if the panel claims no keys.
		/// </returns>
		[Export ("supportedPPDOptionKeys")]
		[return: NullAllowed]
		string [] GetSupportedPpdOptionKeys ();

		/// <summary>
		/// Notifies the panel that a PPD option choice was changed externally (for example, during
		/// conflict resolution). The panel should update its internal data for the specified key.
		/// </summary>
		/// <param name="option">The PPD main key whose value changed.</param>
		/// <param name="choice">The new PPD choice value for <paramref name="option" />.</param>
		[Abstract]
		[Export ("PPDOptionKeyValueDidChange:ppdChoice:")]
		void PpdOptionKeyValueDidChange (string option, string choice);

		/// <summary>
		/// Returns the panel's <see cref="NSView" /> to be embedded into the print or page setup dialog.
		/// </summary>
		/// <returns>
		/// The <see cref="NSView" /> for this panel, or <see langword="null" /> if no view is available.
		/// </returns>
		[Abstract]
		[Export ("panelView")]
		[return: NullAllowed]
		NSView GetPanelView ();

		/// <summary>
		/// Returns the localized name of this panel, which appears in the panel menu within the print dialog.
		/// </summary>
		/// <returns>A localized string representing the panel's display name.</returns>
		[Abstract]
		[Export ("panelName")]
		string GetPanelName ();

		/// <summary>
		/// Returns the panel's kind identifier string. This is used to allow overriding built-in system panels.
		/// Return a unique string if the panel does not override any system panel.
		/// </summary>
		/// <returns>A string identifying the kind of this panel.</returns>
		[Abstract]
		[Export ("panelKind")]
		string GetPanelKind ();

		/// <summary>
		/// Returns a dictionary of localized key-value pairs describing the panel's current settings,
		/// suitable for display in a summary view (for example, key "Page Range", value "All").
		/// </summary>
		/// <returns>
		/// A dictionary mapping setting names to their localized values, or <see langword="null" /> if no summary is available.
		/// </returns>
		[Abstract]
		[Export ("summaryInfo")]
		[return: NullAllowed]
		NSDictionary<NSString, NSString> GetSummaryInfo ();

		/// <summary>
		/// Determines whether the system's default printing help should be shown. Return <see langword="false" />
		/// if the panel implements its own custom help; return <see langword="true" /> (or leave unimplemented)
		/// to use the default printing help.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> to show default help; <see langword="false" /> if custom help is handled by the panel.
		/// </returns>
		[Export ("shouldShowHelp")]
		bool ShouldShowHelp ();

		/// <summary>
		/// Called to determine whether printing should proceed based on the current panel state.
		/// Return <see langword="false" /> to prevent printing. Most panels do not need to implement this.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if printing should proceed; <see langword="false" /> to cancel the print operation.
		/// </returns>
		[Export ("shouldPrint")]
		bool ShouldPrint ();

		/// <summary>
		/// Called when the print dialog is about to close. Use this for cleanup tasks such as removing
		/// notification observers.
		/// </summary>
		/// <param name="userCanceled">
		/// <see langword="true" /> if the user canceled the dialog; <see langword="false" /> if the user confirmed.
		/// </param>
		[Export ("printWindowWillClose:")]
		void PrintWindowWillClose (bool userCanceled);
	}

	/// <summary>
	/// Factory for creating <see cref="PDEPanel" /> instances. The factory receives the type of printer panel
	/// needed (page setup or print dialog) along with the current printer, and uses this information to
	/// determine which panels should be created.
	/// </summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface PDEPlugIn {
		/// <summary>
		/// Returns an array of <see cref="IPDEPanel" /> instances for the specified panel type and host context.
		/// </summary>
		/// <param name="pdeType">
		/// The type of print panels to create (for example, <c>kGeneralPageSetupDialogTypeIDStr</c>
		/// for page setup or <c>kGeneralPrintDialogTypeIDStr</c> for the print dialog).
		/// </param>
		/// <param name="host">
		/// A print-system-provided instance implementing <see cref="IPDEPlugInCallbackProtocol" />,
		/// used to obtain the current printer, print settings, or page format.
		/// </param>
		/// <returns>
		/// An array of <see cref="IPDEPanel" /> instances, or <see langword="null" /> if no panels
		/// should be used for the given type.
		/// </returns>
		[Abstract]
		[Export ("PDEPanelsForType:withHostInfo:")]
		[return: NullAllowed]
		IPDEPanel [] GetPanels (string pdeType, IPDEPlugInCallbackProtocol host);
	}

	/// <summary>
	/// Callback protocol implemented by the print system that allows printing dialog extensions to obtain
	/// information about the current printer and print job, including session, settings, page format,
	/// printer, and PPD data.
	/// </summary>
	[NoiOS, NoTV, NoMacCatalyst, Mac (10, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface PDEPlugInCallbackProtocol {
		/// <summary>
		/// Returns the current print session.
		/// </summary>
		/// <returns>The current <see cref="PMPrintSession" />, or <see langword="null" /> if unavailable.</returns>
		[Abstract]
		[Export ("printSession")]
		[return: NullAllowed]
		PMPrintSession GetPrintSession ();

		/// <summary>
		/// Returns the print settings being modified by the dialog.
		/// </summary>
		/// <returns>
		/// The current <see cref="PMPrintSettings" />, or <see langword="null" /> if the dialog
		/// is not operating on print settings (for example, a page setup dialog).
		/// </returns>
		[Abstract]
		[Export ("printSettings")]
		[return: NullAllowed]
		PMPrintSettings GetPrintSettings ();

		/// <summary>
		/// Returns the page format used by the dialog.
		/// </summary>
		/// <returns>
		/// The current <see cref="PMPageFormat" />, or <see langword="null" /> if the dialog is
		/// operating without a page format.
		/// </returns>
		[Abstract]
		[Export ("pageFormat")]
		[return: NullAllowed]
		PMPageFormat GetPageFormat ();

		/// <summary>
		/// Returns the current printer. For a page setup dialog this is the default printer;
		/// for a print dialog it is the currently selected printer.
		/// </summary>
		/// <returns>The current <see cref="PMPrinter" />.</returns>
		[Abstract]
		[Export ("PMPrinter")]
		PMPrinter GetPrinter ();

		/// <summary>
		/// Returns a pointer to the CUPS PPD structure (<c>ppd_file_s</c>) for the current printer.
		/// Use CUPS PPD functions to query and set options on the returned pointer.
		/// </summary>
		/// <returns>
		/// A pointer to the native <c>ppd_file_s</c> CUPS PPD structure for the current printer.
		/// </returns>
		[Abstract]
		[Export ("ppdFile")]
		IntPtr GetPpdFile ();

		/// <summary>
		/// Notifies the print system that the plugin intends to change a PPD option choice.
		/// The print system validates the change and returns whether it is allowed.
		/// </summary>
		/// <param name="option">The PPD main key identifying the option to change.</param>
		/// <param name="choice">The desired PPD choice value for <paramref name="option" />.</param>
		/// <returns>
		/// <see langword="true" /> if the change is allowed; <see langword="false" /> if the change
		/// conflicts with another setting and should not proceed.
		/// </returns>
		[Abstract]
		[Export ("willChangePPDOptionKeyValue:ppdChoice:")]
		bool WillChangePpdOptionKeyValue (string option, string choice);
	}
}
