// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public class BindingException : Exception {
	public BindingMessage BindingMessage { get; private set; }

	public BindingException (BindingMessage message)
	 : base (message.ToString ())
	{
		BindingMessage = message;
	}
}
