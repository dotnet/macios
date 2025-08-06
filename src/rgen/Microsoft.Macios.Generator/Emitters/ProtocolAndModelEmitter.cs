// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// An emitter that combines the functionality of <see cref="ProtocolEmitter"/> and <see cref="ProtocolWrapperEmitter"/>
/// to emit both the protocol interface and its corresponding wrapper class.
/// </summary>
class ProtocolAndModelEmitter : ICodeEmitter {
	readonly ProtocolEmitter protocolEmitter = new ();
	readonly ProtocolWrapperEmitter protocolWrapperEmitter = new ();
	readonly ModelEmitter modelEmitter = new ();

	/// <inheritdoc />
	public string GetSymbolName (in Binding binding) => protocolEmitter.GetSymbolName (binding);

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements {
		get {
			foreach (var ns in protocolEmitter.UsingStatements) {
				yield return ns;
			}
			foreach (var ns in protocolWrapperEmitter.UsingStatements) {
				yield return ns;
			}
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		// try to emit the protocol first
		if (!protocolEmitter.TryEmit (bindingContext, out diagnostics)) {
			return false;
		}

		// add space and add the wrapper emitter
		bindingContext.Builder.WriteLine ();

		if (!protocolWrapperEmitter.TryEmit (bindingContext, out diagnostics)) {
			return false;
		}

		var bindingData = (BindingTypeData<ObjCBindings.Protocol>) bindingContext.Changes.BindingInfo;

		// check if the protocol was marked to have a model
		if (!bindingData.Flags.HasFlag (ObjCBindings.Protocol.CreateModel))
			return true;

		bindingContext.Builder.WriteLine ();
		if (!modelEmitter.TryEmit (bindingContext, out diagnostics)) {
			return false;
		}

		return true;
	}

}
