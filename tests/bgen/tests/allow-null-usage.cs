// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable
#pragma warning error CS8602, CS8625

namespace AllowNullProperties {
	static class Usage {
#if !MONOMAC
		static void UseView (UIKit.UIViewController controller)
		{
			controller.View = null;
			controller.View.ToString ();
		}
#endif

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
