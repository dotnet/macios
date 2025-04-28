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

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	/// <summary>Provides data for the <see cref="E:UIKit.UITextField.EndedWithReason" /> event.</summary>
	///     <remarks>
	///     </remarks>
	public partial class UITextFieldEditingEndedEventArgs : EventArgs {
		/// <param name="reason">To be added.</param>
		///         <summary>Initializes a new instance of the UITextFieldEditingEndedEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public UITextFieldEditingEndedEventArgs (UITextFieldDidEndEditingReason reason)
		{
			this.Reason = reason;
		}
		/// <summary>Gets or sets the reason why the edit was ended.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
			_UITextFieldDelegate del = Delegate as _UITextFieldDelegate;
			if (del is null) {
				del = (_UITextFieldDelegate) CreateInternalEventDelegateType ();
				Delegate = (IUITextFieldDelegate) del;
			}
			return del;
		}

#pragma warning disable 672
		[Register]
		internal class _UITextFieldDelegate : NSObject, IUITextFieldDelegate {
			public _UITextFieldDelegate () { IsDirectBinding = false; }

			internal EventHandler editingEnded;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidEndEditing:")]
			public void EditingEnded (UITextField textField)
			{
				EventHandler handler = editingEnded;
				if (handler is not null) {
					handler (textField, EventArgs.Empty);
				} else {
					// if this is executed before iOS10 and only the new API is used we'll raise the new event (if set)
					EventHandler<UITextFieldEditingEndedEventArgs> handler2 = editingEnded1;
					if (handler2 is not null) {
						var args = new UITextFieldEditingEndedEventArgs (UITextFieldDidEndEditingReason.Unknown);
						handler2 (textField, args);
					}
				}
			}

			internal EventHandler<UITextFieldEditingEndedEventArgs> editingEnded1;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidEndEditing:reason:")]
			public void EditingEnded (UITextField textField, UITextFieldDidEndEditingReason reason)
			{
				EventHandler<UITextFieldEditingEndedEventArgs> handler = editingEnded1;
				if (handler is not null) {
					var args = new UITextFieldEditingEndedEventArgs (reason);
					handler (textField, args);
				} else {
					// if this is executed on iOS10 (or late) and only the old API is used then we'll raise the old event (if set)
					EventHandler handler2 = editingEnded;
					if (handler2 is not null)
						handler2 (textField, EventArgs.Empty);
				}
			}

			internal EventHandler editingStarted;
			[Preserve (Conditional = true)]
			[Export ("textFieldDidBeginEditing:")]
			public void EditingStarted (UITextField textField)
			{
				EventHandler handler = editingStarted;
				if (handler is not null) {
					handler (textField, EventArgs.Empty);
				}
			}

			internal UITextFieldCondition shouldBeginEditing;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldBeginEditing:")]
			public bool ShouldBeginEditing (UITextField textField)
			{
				UITextFieldCondition handler = shouldBeginEditing;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldChange shouldChangeCharacters;
			[Preserve (Conditional = true)]
			[Export ("textField:shouldChangeCharactersInRange:replacementString:")]
			public bool ShouldChangeCharacters (UITextField textField, NSRange range, string replacementString)
			{
				UITextFieldChange handler = shouldChangeCharacters;
				if (handler is not null)
					return handler (textField, range, replacementString);
				return true;
			}

			internal UITextFieldCondition shouldClear;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldClear:")]
			public bool ShouldClear (UITextField textField)
			{
				UITextFieldCondition handler = shouldClear;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldCondition shouldEndEditing;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldEndEditing:")]
			public bool ShouldEndEditing (UITextField textField)
			{
				UITextFieldCondition handler = shouldEndEditing;
				if (handler is not null)
					return handler (textField);
				return true;
			}

			internal UITextFieldCondition shouldReturn;
			[Preserve (Conditional = true)]
			[Export ("textFieldShouldReturn:")]
			public bool ShouldReturn (UITextField textField)
			{
				UITextFieldCondition handler = shouldReturn;
				if (handler is not null)
					return handler (textField);
				return true;
			}
		}
#pragma warning restore 672

		/// <summary>Raised when editing has ended.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler Ended {
			add { EnsureUITextFieldDelegate ().editingEnded += value; }
			remove { EnsureUITextFieldDelegate ().editingEnded -= value; }
		}

		/// <summary>Event that is raised when editing ends.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler<UITextFieldEditingEndedEventArgs> EndedWithReason {
			add { EnsureUITextFieldDelegate ().editingEnded1 += value; }
			remove { EnsureUITextFieldDelegate ().editingEnded1 -= value; }
		}

		/// <summary>Raised when editing has started.</summary>
		/// <remarks>To be added.</remarks>
		public event EventHandler Started {
			add { EnsureUITextFieldDelegate ().editingStarted += value; }
			remove { EnsureUITextFieldDelegate ().editingStarted -= value; }
		}

		/// <summary>Delegate invoked by the object to get a value.</summary>
		///         <value />
		///         <remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
		public UITextFieldCondition ShouldBeginEditing {
			get { return EnsureUITextFieldDelegate ().shouldBeginEditing; }
			set { EnsureUITextFieldDelegate ().shouldBeginEditing = value; }
		}

		/// <summary>Delegate invoked by the object to get a value.</summary>
		///         <value>A delegate, usually a method, a anonymous method or a lambda function.</value>
		///         <remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
		public UITextFieldChange ShouldChangeCharacters {
			get { return EnsureUITextFieldDelegate ().shouldChangeCharacters; }
			set { EnsureUITextFieldDelegate ().shouldChangeCharacters = value; }
		}

		/// <summary>Delegate invoked by the object to get a value.</summary>
		///         <value>The delegate/method.</value>
		///         <remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
		public UITextFieldCondition ShouldClear {
			get { return EnsureUITextFieldDelegate ().shouldClear; }
			set { EnsureUITextFieldDelegate ().shouldClear = value; }
		}

		/// <summary>Delegate invoked by the object to get a value.</summary>
		///         <value />
		///         <remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
		public UITextFieldCondition ShouldEndEditing {
			get { return EnsureUITextFieldDelegate ().shouldEndEditing; }
			set { EnsureUITextFieldDelegate ().shouldEndEditing = value; }
		}

		/// <summary>Delegate invoked by the object to get a value.</summary>
		///         <value>A delegate that holds a method, an anonymous method or a lambda function.</value>
		///         <remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
		public UITextFieldCondition ShouldReturn {
			get { return EnsureUITextFieldDelegate ().shouldReturn; }
			set { EnsureUITextFieldDelegate ().shouldReturn = value; }
		}
	}
}

#endif // !COREBUILD
