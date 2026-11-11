// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace Metal {

	public partial class MTLCaptureDescriptor {

		/// <summary>Attempts to get the <see cref="CaptureObject" /> as an <see cref="IMTLDevice" />.</summary>
		/// <param name="value">When this method returns <see langword="true" />, contains the capture object as an <see cref="IMTLDevice" />; otherwise, <see langword="null" />.</param>
		/// <returns><see langword="true" /> if <see cref="CaptureObject" /> is non-null and conforms to <see cref="IMTLDevice" />; otherwise, <see langword="false" />.</returns>
		public bool TryGetCaptureObject ([NotNullWhen (true)] out IMTLDevice? value)
		{
			if (CaptureObject is { } obj && obj.TryAsProtocol (out value))
				return true;

			value = null;
			return false;
		}

		/// <summary>Attempts to get the <see cref="CaptureObject" /> as an <see cref="IMTLCommandQueue" />.</summary>
		/// <param name="value">When this method returns <see langword="true" />, contains the capture object as an <see cref="IMTLCommandQueue" />; otherwise, <see langword="null" />.</param>
		/// <returns><see langword="true" /> if <see cref="CaptureObject" /> is non-null and conforms to <see cref="IMTLCommandQueue" />; otherwise, <see langword="false" />.</returns>
		public bool TryGetCaptureObject ([NotNullWhen (true)] out IMTLCommandQueue? value)
		{
			if (CaptureObject is { } obj && obj.TryAsProtocol (out value))
				return true;

			value = null;
			return false;
		}

		/// <summary>Attempts to get the <see cref="CaptureObject" /> as an <see cref="IMTLCaptureScope" />.</summary>
		/// <param name="value">When this method returns <see langword="true" />, contains the capture object as an <see cref="IMTLCaptureScope" />; otherwise, <see langword="null" />.</param>
		/// <returns><see langword="true" /> if <see cref="CaptureObject" /> is non-null and conforms to <see cref="IMTLCaptureScope" />; otherwise, <see langword="false" />.</returns>
		public bool TryGetCaptureObject ([NotNullWhen (true)] out IMTLCaptureScope? value)
		{
			if (CaptureObject is { } obj && obj.TryAsProtocol (out value))
				return true;

			value = null;
			return false;
		}
	}
}
