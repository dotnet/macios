// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Removes default constructors from interfaces since the binding tool
/// always generates these. Annotates interfaces with [DisableDefaultCtor] if
/// the default constructor in Objective C is unavailable, which is a common
/// pattern from Apple.
/// </summary>
public sealed class DefaultConstructorMassager : Massager<DefaultConstructorMassager> {
	public DefaultConstructorMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		if (HasVisited (methodDeclaration))
			return;

		MarkVisited (methodDeclaration);

		// not a default constructor
		if (!(methodDeclaration is ObjCConstructorMethodDeclaration) || methodDeclaration.Parameters.Count > 0)
			return;

		// annotate the interface with [DisableDefaultCtor] if unavailable in Obj-C
		var native = methodDeclaration.Annotation<ObjCMethodDecl> ();
		if (native is not null && (native.Attrs.IsUnavailableAttr () ||
				native.Attrs.IsAnyAvailabilityAttributeUnavailable ()))
			methodDeclaration.GetParent<TypeDeclaration> ().AddAttribute (new DisableDefaultCtorAttribute ());

		// remove the explicit default ctor from the interface since it will be generated for us
		if (methodDeclaration.PrevSibling is NativeCodeReferenceComment)
			methodDeclaration.PrevSibling.Remove ();
		methodDeclaration.Remove ();
	}
}
