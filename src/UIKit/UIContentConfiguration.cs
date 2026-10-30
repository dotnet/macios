// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace UIKit {
	public static partial class UIContentConfiguration_Extensions {
		/// <summary>Creates a content view and exposes its underlying <see cref="UIView" />.</summary>
		/// <param name="configuration">The configuration to create a content view from.</param>
		/// <param name="view">The view to add to a view hierarchy.</param>
		/// <returns>The content view's protocol interface, which can be used to update its configuration.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="configuration" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>The return value and <paramref name="view" /> refer to the same native object, but may be different managed wrappers.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst14.0")]
		[UnsupportedOSPlatform ("macos")]
		public static IUIContentView MakeContentView (this IUIContentConfiguration configuration, out UIView view)
		{
			ArgumentNullException.ThrowIfNull (configuration);
			var contentView = configuration.MakeContentView ();
			view = Runtime.GetNSObject<UIView> (contentView.GetNonNullHandle (nameof (contentView)), false)
				?? throw new InvalidOperationException ("The content configuration did not create a content view.");
			GC.KeepAlive (contentView);
			return contentView;
		}
	}
}
