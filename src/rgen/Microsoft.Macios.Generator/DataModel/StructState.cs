// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Represents the initialization state of a struct.
/// </summary>
public enum StructState {
	/// <summary>
	/// The struct is in its default, uninitialized state.
	/// </summary>
	Default,
	/// <summary>
	/// The struct has been initialized.
	/// </summary>
	Initialized,
}
