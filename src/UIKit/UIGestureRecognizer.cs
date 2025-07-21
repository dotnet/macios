// 
// UIGestureRecognizer: Implements some helper methods for UIGestureRecognizer
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2011-2013 Xamarin Inc. All rights reserved
//

using System;
using System.Collections.Generic;
using Foundation;
using ObjCRuntime;
#nullable enable

namespace UIKit {

	public partial class UIGestureRecognizer {
		//
		// Tracks the targets (NSObject, which we always enforce to be Token) to the Selector the point to, used when disposing
		//
		Dictionary<Token, IntPtr> recognizers = new Dictionary<Token, IntPtr> ();
		const string tsel = "target";
		internal const string parametrized_selector = "target:";

		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Initalizes a gesture recognizer.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public UIGestureRecognizer (Action action) : this (Selector.GetHandle (tsel), new ParameterlessDispatch (action))
		{
		}

		// Called by the Dispose() method, because this can run from a finalizer, we need to
		// (a) reference the handle, that we will release later, and (b) to remove the targets on the
		// UI thread.
		// Note: preserving this member allows us to re-enable the `Optimizable` binding flag
		[Preserve (Conditional = true)]
		void OnDispose ()
		{
			var copyOfRecognizers = recognizers;
			var savedHandle = Handle;
			recognizers = new Dictionary<Token, IntPtr> ();

			if (copyOfRecognizers.Count == 0)
				return;

			DangerousRetain (savedHandle);
			NSRunLoop.Main.BeginInvokeOnMainThread (() => {
				foreach (var kv in copyOfRecognizers)
					RemoveTarget (kv.Key, kv.Value);
				DangerousRelease (savedHandle);
			});
		}

		//
		// Signature swapped, this is only used so we can store the "token" in recognizers
		//
		/// <param name="sel">A selector that specifies the method that is implemented by the target to handle the gesture that is recognized by the receiver.</param>
		///         <param name="token">String constant to be used as a token.</param>
		///         <summary>Initalizes a gesture recognizer.</summary>
		///         <remarks>To be added.</remarks>
		public UIGestureRecognizer (Selector sel, Token token) : this (token, sel)
		{
			recognizers [token] = sel.Handle;
			MarkDirty ();
		}

		internal UIGestureRecognizer (IntPtr sel, Token token) : this (token, sel)
		{
			recognizers [token] = sel;
			MarkDirty ();
		}

		/// <summary>Represents an action that was added to a UIGestureRecognizer.</summary>
		///     <remarks>
		///
		///       An instance of this class is returned when you invoke the <see cref="UIKit.UIGestureRecognizer" />'s <see cref="UIKit.UIGestureRecognizer.AddTarget(Foundation.NSObject,ObjCRuntime.Selector)" /> method.
		///       The AddTarget returns this token as a mechanism for later
		///       unsubscribing this particular action from the recognizer using the <see cref="UIKit.UIGestureRecognizer.RemoveTarget(Foundation.NSObject,ObjCRuntime.Selector)" /> method.
		///
		///     </remarks>
		///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UIKit/__UIGestureRecognizerToken">Apple documentation for <c>__UIGestureRecognizerToken</c></related>
		[Register ("__UIGestureRecognizerToken")]
		public class Token : NSObject {
			/// <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			public Token ()
			{
				IsDirectBinding = false;
			}
		}

		[Register ("__UIGestureRecognizerGenericCB")]
		internal class Callback<T> : Token where T : UIGestureRecognizer {
			Action<T> action;

			internal Callback (Action<T> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (T sender) => action (sender);

		}

		/// <summary>Subtype of <see cref="UIKit.UIGestureRecognizer.Token" />, which is returned by <see cref="UIKit.UIView.AddGestureRecognizer(UIKit.UIGestureRecognizer)" />.</summary>
		///     <remarks>To be added.</remarks>
		///     <altmember cref="UIKit.UIGestureRecognizer.ParametrizedDispatch" />
		///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UIKit/__UIGestureRecognizerParameterlessToken">Apple documentation for <c>__UIGestureRecognizerParameterlessToken</c></related>
		[Register ("__UIGestureRecognizerParameterlessToken")]
		public class ParameterlessDispatch : Token {
			Action action;

			internal ParameterlessDispatch (Action action)
			{
				this.action = action;
			}

			/// <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			[Export ("target")]
			[Preserve (Conditional = true)]
			public void Activated ()
			{
				action ();
			}
		}

		/// <summary>Subtype of <see cref="UIKit.UIGestureRecognizer.Token" />.</summary>
		///     <remarks>To be added.</remarks>
		///     <altmember cref="UIKit.UIGestureRecognizer.ParameterlessDispatch" />
		///     <related type="externalDocumentation" href="https://developer.apple.com/reference/UIKit/__UIGestureRecognizerParametrizedToken">Apple documentation for <c>__UIGestureRecognizerParametrizedToken</c></related>
		[Register ("__UIGestureRecognizerParametrizedToken")]
		public class ParametrizedDispatch : Token {
			Action<UIGestureRecognizer> action;

			internal ParametrizedDispatch (Action<UIGestureRecognizer> action)
			{
				this.action = action;
			}

			/// <param name="sender">To be added.</param>
			///         <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (UIGestureRecognizer sender)
			{
				action (sender);
			}
		}

		/// <param name="action">The method to invoke when the gesture has been recognized.</param>
		///         <summary>Registers a new callback for when the gesture has been recognized.</summary>
		///         <returns>The returned token can be used later to remove this particular action from being invoked by the gesture recognizer.</returns>
		///         <remarks>To be added.</remarks>
		public Token AddTarget (Action action)
		{
			if (action is null)
				throw new ArgumentNullException ("action");

			var t = new ParameterlessDispatch (action);
			RegisterTarget (t, Selector.GetHandle (tsel));
			return t;
		}

		/// <param name="action">The method to invoke when the gesture has been recognized.</param>
		///         <summary>Registers a new callback for when the gesture has been recognized.</summary>
		///         <returns>The returned token can be used later to remove this particular action from being invoked by the gesture recognizer using the <see cref="UIKit.UIGestureRecognizer.RemoveTarget(Foundation.NSObject,ObjCRuntime.Selector)" /> method.</returns>
		///         <remarks>
		///         </remarks>
		public Token AddTarget (Action<NSObject> action)
		{
			if (action is null)
				throw new ArgumentNullException ("action");

			var t = new ParametrizedDispatch (action);
			RegisterTarget (t, Selector.GetHandle (parametrized_selector));
			return t;
		}

		void RegisterTarget (Token target, IntPtr sel)
		{
			AddTarget (target, sel);
			MarkDirty ();
			recognizers [target] = sel;
		}

		/// <param name="token">A Token returned by the AddTarget method.</param>
		///         <summary>Removes the callback method for the specified gesture being recognized, based on the token that was returned by AddTarget.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveTarget (Token token)
		{
			if (token is null)
				throw new ArgumentNullException ("token");
			if (recognizers is null)
				return;
			if (recognizers.Remove (token, out var sel))
				RemoveTarget (token, sel);
		}

		//
		// Used to enumerate all the registered handlers for this UIGestureRecognizer
		//
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerable<Token> GetTargets ()
		{
			var keys = recognizers?.Keys;
			if (keys is null)
				return Array.Empty<Token> ();
			return (IEnumerable<Token>) keys;
		}
	}

#if !TVOS
	public partial class UIRotationGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIRotationGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UIRotationGestureRecognizer (Action<UIRotationGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UIRotationGestureRecognizer> (action)) { }

	}
#endif

	public partial class UILongPressGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UILongPressGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UILongPressGestureRecognizer (Action<UILongPressGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UILongPressGestureRecognizer> (action)) { }

	}

	public partial class UITapGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UITapGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UITapGestureRecognizer (Action<UITapGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UITapGestureRecognizer> (action)) { }

	}

	public partial class UIPanGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIPanGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UIPanGestureRecognizer (Action<UIPanGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UIPanGestureRecognizer> (action)) { }

		internal UIPanGestureRecognizer (IntPtr sel, Token token) : base (token, sel) { }

	}

#if !TVOS
	public partial class UIPinchGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIPinchGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UIPinchGestureRecognizer (Action<UIPinchGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UIPinchGestureRecognizer> (action)) { }

	}
#endif

	public partial class UISwipeGestureRecognizer : UIGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UISwipeGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the gesture is recognized.</param>
		///         <summary>Constructs a gesture recognizer and provides a method to invoke when the gesture is recognized.</summary>
		///         <remarks>This overload allows the method that will be invoked to receive the recognizer that detected the gesture as a parameter.</remarks>
		public UISwipeGestureRecognizer (Action<UISwipeGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UISwipeGestureRecognizer> (action)) { }

	}

#if !TVOS
	public partial class UIScreenEdgePanGestureRecognizer : UIPanGestureRecognizer {
		/// <param name="action">Code to invoke when the screen edge pan gesture is recognized.</param>
		///         <summary>Initalizes a screen edge pan gesture recognizer.</summary>
		///         <remarks>To be added.</remarks>
		public UIScreenEdgePanGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">Code to invoke when the screen edge pan gesture is recognized.</param>
		///         <summary>Initializes a designated screen edge pan gesture recognizer.</summary>
		///         <remarks>To be added.</remarks>
		public UIScreenEdgePanGestureRecognizer (Action<UIScreenEdgePanGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UIScreenEdgePanGestureRecognizer> (action)) { }

	}

	public partial class UIHoverGestureRecognizer : UIGestureRecognizer {
		public UIHoverGestureRecognizer (Action<UIHoverGestureRecognizer> action) : base (Selector.GetHandle (UIGestureRecognizer.parametrized_selector), new Callback<UIHoverGestureRecognizer> (action)) { }

	}
#endif
}
