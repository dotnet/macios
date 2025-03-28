// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

class TrampolineEmitter (
	RootContext context,
	TabbedStringBuilder builder) {

	public string SymbolNamespace => "ObjCRuntime";
	public string SymbolName => "Trampolines";

	public bool TryEmit (IReadOnlySet<TypeInfo> trampolines,
		[NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		// TODO: actual trampoline generation, for the current time write the using statements, namespace, class
		// and some comments with the trampolines to emit
		diagnostics = null;

		builder.WriteLine ("using Foundation;");
		builder.WriteLine ("using ObjCBindings;");
		builder.WriteLine ("using ObjCRuntime;");
		builder.WriteLine ("using System;");

		builder.WriteLine ();
		builder.WriteLine ($"namespace ObjCRuntime;");
		builder.WriteLine ();

		using (var classBlock = builder.CreateBlock ($"static partial class {SymbolName}", true)) {
			classBlock.WriteLine ($"// Generate trampolines for compilation");
			_ = context.CurrentPlatform;
			foreach (var info in trampolines) {
				classBlock.WriteLine ($"// TODO: generate trampoline for {info.FullyQualifiedName}");
				classBlock.WriteLine ();
			}
		}

		return true;
	}
}
