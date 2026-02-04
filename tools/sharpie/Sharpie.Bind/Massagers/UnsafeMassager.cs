// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Annotates method and property declarations with the 'unsafe' modifier if types have pointers.
/// </summary>
public sealed class UnsafeMassager : Massager<UnsafeMassager> {
	public UnsafeMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitComposedType (ComposedType composedType)
	{
		if (HasVisited (composedType.Parent))
			return;

		MarkVisited (composedType.Parent);

		if (composedType.PointerRank == 0)
			return;

		var entity = composedType.GetParent<EntityDeclaration> ();
		if (entity is not null)
			entity.Modifiers |= Modifiers.Unsafe;
	}
}
