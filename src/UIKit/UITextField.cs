//
// UITextField.cs: Extensions to UITextField
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
//

#if !COREBUILD

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace UIKit {

	/// <summary>Provides data for the <see cref="UIKit.UITextField.EndedWithReason" /> event.</summary>
	public partial class UITextFieldEditingEndedEventArgs : EventArgs {
		/// <summary>Initializes a new instance of the <see cref="UITextFieldEditingEndedEventArgs" /> class.</summary>
		/// <param name="reason">The reason the editing ended.</param>
		public UITextFieldEditingEndedEventArgs (UITextFieldDidEndEditingReason reason)
		{
			this.Reason = reason;
		}
		/// <summary>Gets or sets the reason why the edit ended.</summary>
		public UITextFieldDidEndEditingReason Reason { get; set; }
	}

	/// <param name="textField">To be added.</param>
	///     <param name="range">To be added.</param>
	///     <param name="replacementString">To be added.</param>
	///     <summary>A delegate used to respond to changes on the UITextField.</summary>
	///     <returns>To be added.</returns>
	///     <remarks>To be added.</remarks>
	public delegate bool UITextFieldChange (UITextField textField, NSRange range, string replacementString);

	/// <param name="textField">To be added.</param>
	///     <summary>A delegate used to get the condition for a UITextField.</summary>
	///     <returns>To be added.</returns>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/MapDemo">Example_ContentControls</related>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/MonoCatalog-MonoDevelop/">monocatalog</related>
	public delegate bool UITextFieldCondition (UITextField textField);

	/// <summary>Delegate that determines whether text changes should be allowed in a UITextField for multiple ranges.</summary>
	/// <param name="textField">The text field that contains the text to be modified.</param>
	/// <param name="ranges">An array of <see cref="NSValue" /> objects containing <see cref="NSRange" /> values representing the character ranges to be replaced.</param>
	/// <param name="replacementString">The replacement text for the specified ranges. When typing, this typically contains the single new character entered, but may contain multiple characters if pasting. An empty string indicates character deletion.</param>
	/// <returns>Returns true if the text changes should be allowed; otherwise, false to prevent the modification.</returns>
	/// <remarks>
	/// <para>This delegate method is called when the text field needs to validate text changes across multiple ranges simultaneously.</para>
	/// <para>Use this method to implement custom text validation logic, such as restricting input to specific formats or preventing certain types of content.</para>
	/// <para>This is the multiple-range version of the standard <see cref="UIKit.UITextFieldDelegate.ShouldChangeCharacters (UITextField, NSValue [], string)" /> method.</para>
	/// </remarks>
	public delegate bool UITextFieldChanges (UITextField textField, NSValue [] ranges, string replacementString);

	public partial class UITextField : IUITextInputTraits {

		internal virtual Type GetInternalEventDelegateType {
			get { return typeof (_UITextFieldDelegate); }
		}

		internal virtual _UITextFieldDelegate CreateInternalEventDelegateType ()
		{
			return (_UITextFieldDelegate) (new _UITextFieldDelegate ());
		}

		internal _UITextFieldDelegate EnsureUITextFieldDelegate ()
		{
			if (Delegate is not null)
				UIApplication.EnsureEventAndDelegateAreNotMismatched (Delegate, GetInternalEventDelegateType);
			var del = Delegate as _UITextFieldDelegate;
			if (del is null) {
				del = (_UITextFieldDelegate) CreateInternalEventDelegateType ();
				Delegate = (IUITextFieldDelegate) del;
			}
			return del;
		}

		[Register]
		internal class _UITextFieldDelegate : NSObject, IUITextFieldDelegate {
			public _UITextFieldDelegate () { IsDirectBinding = false; }

			internal EventHandler? editingEnded;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidEndEditing:")]
			public void EditingEnded (UITextField textField)
			{
				var handler = editingEnded;
				if (handler is not null) {
					handler (textField, EventArgs.Empty);
				} else {
					// if this is executed before iOS10 and only the new API is used we'll raise the new event (if set)
					var handler2 = editingEnded1;
					if (handler2 is not null) {
						var args = new UITextFieldEditingEndedEventArgs (UITextFieldDidEndEditingReason.Unknown);
						handler2 (textField, args);
					}
				}
			}

			internal EventHandler<UITextFieldEditingEndedEventArgs>? editingEnded1;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidEndEditing:reason:")]
			public void EditingEnded (UITextField textField, UITextFieldDidEndEditingReason reason)
			{
				var handler = editingEnded1;
				if (handler is not null) {
					var args = new UITextFieldEditingEndedEventArgs (reason);
					handler (textField, args);
				} else {
					// if this is executed on iOS10 (or late) and only the old API is used then we'll raise the old event (if set)
					var handler2 = editingEnded;
					if (handler2 is not null)
						handler2 (textField, EventArgs.Empty);
				}
			}

			internal EventHandler? editingStarted;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidBeginEditing:")]
			public void EditingStarted (UITextField textField)
			{
				var handler = editingStarted;
				if (handler is not null) {
					handler (textField, EventArgs.Empty);
				}
			}

			internal UITextFieldCondition? shouldBeginEditing;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldBeginEditing:")]
			public bool ShouldBeginEditing (UITextField textField)
			{
				var handler = shouldBeginEditing;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldChange? shouldChangeCharacters;
			[Preserve (Conditional = true)]
			[Export ("textField:shouldChangeCharactersInRange:replacementString:")]
			public bool ShouldChangeCharacters (UITextField textField, NSRange range, string replacementString)
			{
				var handler = shouldChangeCharacters;
				if (handler is not null)
					return handler (textField, range, replacementString);
				return true;
			}

			internal UITextFieldCondition? shouldClear;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldClear:")]
			public bool ShouldClear (UITextField textField)
			{
				var handler = shouldClear;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldCondition? shouldEndEditing;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldEndEditing:")]
			public bool ShouldEndEditing (UITextField textField)
			{
				var handler = shouldEndEditing;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldCondition? shouldReturn;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldReturn:")]
			public bool ShouldReturn (UITextField textField)
			{
				var handler = shouldReturn;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal EventHandler? didChangeSelection;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidChangeSelection:")]
			public void DidChangeSelection (UITextField textField)
			{
				var handler = didChangeSelection;
				if (handler is not null)
					handler (textField, EventArgs.Empty);
			}

			internal UITextFieldChanges? shouldChangeCharactersInRanges;
			[Preserve (Conditional = true)]
			[Export ("textField:shouldChangeCharactersInRanges:replacementString:")]
			bool ShouldChangeCharacters (UITextField textField, NSValue [] ranges, string replacementString)
			{
				var handler = shouldChangeCharactersInRanges;
				if (handler is not null)
					return handler (textField, ranges, replacementString);
				return true;
			}
		}

		/// <summary>Raised when editing has ended.</summary>
		public event EventHandler Ended {
			add { EnsureUITextFieldDelegate ().editingEnded += value; }
			remove { EnsureUITextFieldDelegate ().editingEnded -= value; }
		}

		/// <summary>Event that is raised when editing ends.</summary>
		public event EventHandler<UITextFieldEditingEndedEventArgs> EndedWithReason {
			add { EnsureUITextFieldDelegate ().editingEnded1 += value; }
			remove { EnsureUITextFieldDelegate ().editingEnded1 -= value; }
		}

		/// <summary>Raised when editing has started.</summary>
		public event EventHandler Started {
			add { EnsureUITextFieldDelegate ().editingStarted += value; }
			remove { EnsureUITextFieldDelegate ().editingStarted -= value; }
		}

		/// <summary>Raised when the text field wants to know if editing should begin.</summary>
		public UITextFieldCondition? ShouldBeginEditing {
			get { return EnsureUITextFieldDelegate ().shouldBeginEditing; }
			set { EnsureUITextFieldDelegate ().shouldBeginEditing = value; }
		}

		/// <summary>Raised when the text field wants to know if the text in the text field should be changed.</summary>
		public UITextFieldChange? ShouldChangeCharacters {
			get { return EnsureUITextFieldDelegate ().shouldChangeCharacters; }
			set { EnsureUITextFieldDelegate ().shouldChangeCharacters = value; }
		}

		/// <summary>Raised when the text field wants to know if the text in the text field should be cleared.</summary>
		public UITextFieldCondition? ShouldClear {
			get { return EnsureUITextFieldDelegate ().shouldClear; }
			set { EnsureUITextFieldDelegate ().shouldClear = value; }
		}

		/// <summary>Raised when the text field wants to know if editing should end.</summary>
		public UITextFieldCondition? ShouldEndEditing {
			get { return EnsureUITextFieldDelegate ().shouldEndEditing; }
			set { EnsureUITextFieldDelegate ().shouldEndEditing = value; }
		}

		/// <summary>Raised when the 'Return' key was pressed, and the text field wants to know whether it should process this key press or not using the default behavior.</summary>
		public UITextFieldCondition? ShouldReturn {
			get { return EnsureUITextFieldDelegate ().shouldReturn; }
			set { EnsureUITextFieldDelegate ().shouldReturn = value; }
		}

		/// <summary>Raised when the text selection changed.</summary>
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public event EventHandler DidChangeSelection {
			add { EnsureUITextFieldDelegate ().didChangeSelection += value; }
			remove { EnsureUITextFieldDelegate ().didChangeSelection -= value; }
		}

		/// <summary>Raised when the text field wants to know if the text in the text field should be changed for multiple ranges.</summary>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		public UITextFieldChanges? ShouldChangeCharactersInRanges {
			get { return EnsureUITextFieldDelegate ().shouldChangeCharactersInRanges; }
			set { EnsureUITextFieldDelegate ().shouldChangeCharactersInRanges = value; }
		}

		// The following events are already here from the UITextInput protocol, so no need to implement the ones from UITextFieldDelegate:
		// * WillPresentEditMenu
		// * WillDismissEditMenu
		// * GetEditMenu
		// * InsertInputSuggestion
	}
}

#endif // !COREBUILD
