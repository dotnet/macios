// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace UIKit {
	public partial class UIAction {
		/// <summary>Creates an action with the specified display properties, attributes, state, and handler.</summary>
		/// <param name="title">The action's title.</param>
		/// <param name="image">An image to display next to the action, or <see langword="null" />.</param>
		/// <param name="handler">The callback invoked when the user selects the action.</param>
		/// <param name="identifier">The action's identifier, or <see langword="null" /> to generate one automatically.</param>
		/// <param name="discoverabilityTitle">The title displayed in the keyboard shortcut overlay, or <see langword="null" />.</param>
		/// <param name="attributes">The action's attributes. The default is no attributes.</param>
		/// <param name="state">The action's state. The default is <see cref="UIMenuElementState.Off" />.</param>
		/// <returns>A new action configured with the specified values.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIAction Create (string title, UIImage? image, UIActionHandler handler, string? identifier = null, string? discoverabilityTitle = null, UIMenuElementAttributes attributes = 0, UIMenuElementState state = UIMenuElementState.Off)
		{
			var action = Create (title, image, identifier, handler);
			action.DiscoverabilityTitle = discoverabilityTitle;
			action.Attributes = attributes;
			action.State = state;
			return action;
		}
	}
}
