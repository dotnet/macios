// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class ModelAttribute : GroupedAttribute {
	public static string ModelRemovedComment { get; } = "\n" +
		"  Check whether adding [Model] to this declaration is appropriate.\n" +
		"  [Model] is used to generate a C# class that implements this protocol,\n" +
		"  and might be useful for protocols that consumers are supposed to implement,\n" +
		"  since consumers can subclass the generated class instead of implementing\n" +
		"  the generated interface. If consumers are not supposed to implement this\n" +
		"  protocol, then [Model] is redundant and will generate code that will never\n" +
		"  be used.\n";

	public ModelAttribute () : base (1)
	{
		Type = AstType.Create ("Foundation.ModelAttribute");
	}
}
