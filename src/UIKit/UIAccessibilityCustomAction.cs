//
// UIAccessibilityCustomAction.cs: Helpers for actions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin Inc
//

using CoreGraphics;

#nullable enable

namespace UIKit {

	/// <summary>
	/// A custom action to be presented to an assistive application.
	/// </summary>
	public partial class UIAccessibilityCustomAction {
		object? action;

		/// <summary>Creates a <see cref="UIAccessibilityCustomAction" /> with the specified <paramref name="name" />.</summary>
		/// <param name="name">The name of the action.</param>
		/// <param name="probe">A callback to invoke when the action is activated.</param>
		public UIAccessibilityCustomAction (string name, Func<UIAccessibilityCustomAction, bool> probe) : this (name, FuncBoolDispatcher.Selector, new FuncBoolDispatcher (probe))
		{
		}

		internal UIAccessibilityCustomAction (string name, Selector sel, FuncBoolDispatcher disp) : this (name, disp, sel)
		{
			action = disp;
			MarkDirty ();
		}

		// Use this for synchronous operations
		[Register ("__MonoMac_FuncBoolDispatcher")]
		internal sealed class FuncBoolDispatcher : NSObject {
			public const string SelectorName = "xamarinApplySelectorFunc:";
			public static readonly Selector Selector = new Selector (SelectorName);

			readonly Func<UIAccessibilityCustomAction, bool> probe;

			public FuncBoolDispatcher (Func<UIAccessibilityCustomAction, bool> probe)
			{
				ArgumentNullException.ThrowIfNull (probe);

				this.probe = probe;
				IsDirectBinding = false;
			}

			[Export (SelectorName)]
			[Preserve (Conditional = true)]
			public bool Probe (UIAccessibilityCustomAction customAction)
			{
				return probe (customAction);
			}
		}

	}
}
