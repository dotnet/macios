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
		/// <exception cref="ArgumentNullException"><paramref name="title" /> or <paramref name="handler" /> is <see langword="null" />.</exception>
		/// <exception cref="NotSupportedException">This constructor is invoked from a managed subclass.</exception>
		/// <remarks>This constructor uses UIKit's action factory and does not support managed subclasses.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public UIAction (string title, UIImage? image, UIActionHandler handler, string? identifier = null, string? discoverabilityTitle = null, UIMenuElementAttributes attributes = 0, UIMenuElementState state = UIMenuElementState.Off)
			: base (CreateHandle (title, image, identifier, handler))
		{
			if (GetType () != typeof (UIAction))
				throw new NotSupportedException ("This UIAction constructor does not support managed subclasses.");

			DiscoverabilityTitle = discoverabilityTitle;
			Attributes = attributes;
			State = state;
		}

		static unsafe NativeHandle CreateHandle (string title, UIImage? image, string? identifier, UIActionHandler handler)
		{
			ArgumentNullException.ThrowIfNull (title);
			ArgumentNullException.ThrowIfNull (handler);
			UIApplication.EnsureUIThread ();

			using var nativeTitle = new NSString (title);
			using var nativeIdentifier = identifier is null ? null : new NSString (identifier);
			using var block = Trampolines.SDUIActionHandler.CreateBlock (handler);
			BlockLiteral* blockPointer = &block;
			var handle = Messaging.NativeHandle_objc_msgSend_NativeHandle_NativeHandle_NativeHandle_NativeHandle (
				class_ptr, Selector.GetHandle ("actionWithTitle:image:identifier:handler:"),
				nativeTitle.Handle, image.GetHandle (), nativeIdentifier.GetHandle (), (IntPtr) blockPointer);
			GC.KeepAlive (image);
			return handle;
		}

		/// <summary>Creates an action with the specified display properties, attributes, state, and handler.</summary>
		/// <param name="title">The action's title.</param>
		/// <param name="image">An image to display next to the action, or <see langword="null" />.</param>
		/// <param name="handler">The callback invoked when the user selects the action.</param>
		/// <param name="attributes">The action's attributes. The default is no attributes.</param>
		/// <param name="state">The action's state. The default is <see cref="UIMenuElementState.Off" />.</param>
		/// <param name="identifier">The action's identifier, or <see langword="null" /> to generate one automatically.</param>
		/// <param name="discoverabilityTitle">The title displayed in the keyboard shortcut overlay, or <see langword="null" />.</param>
		/// <returns>A new action configured with the specified values.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIAction Create (string title, UIImage? image, UIActionHandler handler, UIMenuElementAttributes attributes = 0, UIMenuElementState state = UIMenuElementState.Off, string? identifier = null, string? discoverabilityTitle = null)
		{
			var action = Create (title, image, identifier, handler);
			action.DiscoverabilityTitle = discoverabilityTitle;
			action.Attributes = attributes;
			action.State = state;
			return action;
		}
	}
}
