// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace AllowNullProperties {
	static class Usage {
#if !MONOMAC
		static void UseView (UIKit.UIViewController controller)
		{
			controller.View = null;
			controller.View.ToString ();
		}

		static void UseUIKit (UIKit.UIView view, UIKit.UILabel label, UIKit.UICellAccessoryCustomView accessory, Foundation.NSObject accessibility)
		{
			view.TintColor = null;
			view.TintColor.ToString ();
			label.Font = null;
			label.Font.ToString ();
			accessory.Position = null;
			accessory.Position ([]);
			accessibility.AccessibilityUserInputLabels = null;
			accessibility.AccessibilityUserInputLabels.ToString ();
		}
#else
		static void UseAppKit (AppKit.NSAlert alert, AppKit.NSObjectController controller)
		{
			alert.Icon = null;
			alert.Icon.ToString ();
			controller.ObjectClass = null;
			controller.ObjectClass.ToString ();
			AppKit.NSAppearance.CurrentAppearance = null;
			AppKit.NSAppearance.CurrentAppearance.ToString ();
		}
#endif

		static void UseFrameworks (Foundation.NSDateFormatter formatter, Foundation.NSNumberFormatter numbers, AVFoundation.AVCaptureVideoDataOutput output, Metal.MTLRenderPassDescriptor renderPass, SpriteKit.SKSpriteNode sprite)
		{
			formatter.DateFormat = null;
			formatter.DateFormat.ToString ();
			formatter.MonthSymbols = null;
			formatter.MonthSymbols.ToString ();
			numbers.PositivePrefix = null;
			numbers.PositivePrefix.ToString ();
			output.WeakVideoSettings = null;
			output.WeakVideoSettings.ToString ();
			output.CompressedVideoSetting = null;
			output.CompressedVideoSetting.ToString ();
			renderPass.DepthAttachment = null;
			renderPass.DepthAttachment.ToString ();
			sprite.Color = null;
			sprite.Color.ToString ();
		}

		static void Use (Properties properties, IPropertiesProtocol protocol, ProtocolImplementation implementation)
		{
			properties.Object = null;
			properties.Text = null;
			properties.Objects = null;
			properties.Strings = null;
			properties.Callback = null;
			properties.WrappedObject = null;
			protocol.Object = null;
			protocol.OptionalObject = null;
			implementation.Object = null;

			properties.Object.ToString ();
			properties.Text.ToString ();
			properties.Objects.ToString ();
			properties.Strings.ToString ();
			properties.Callback ();
			properties.WrappedObject.ToString ();
			protocol.Object.ToString ();
			protocol.OptionalObject.ToString ();
			implementation.Object.ToString ();
		}
	}
}
