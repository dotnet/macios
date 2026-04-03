// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Types;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Converts anonymous Action&lt;...&gt;/Func&lt;...&gt; delegate types in method
/// parameters and property return types into custom named delegate declarations.
/// </summary>
[RegisterBefore (typeof (GenerateUsingStatementsMassager))]
public sealed class CustomDelegateMassager : Massager<CustomDelegateMassager> {
	readonly HashSet<string> usedNames = new HashSet<string> ();
	readonly List<(AstNode InsertBefore, DelegateDeclaration Delegate)> pendingDelegates = new ();

	public CustomDelegateMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override bool Initialize ()
	{
		return Binder.UseCustomDelegates;
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		if (HasVisited (methodDeclaration))
			return;

		MarkVisited (methodDeclaration);

		var parent = methodDeclaration.Parent as TypeDeclaration;
		if (parent is null)
			return;

		foreach (var param in methodDeclaration.Parameters.ToList ()) {
			if (param.Type is not DelegateType delegateType)
				continue;

			var delegateName = GenerateUniqueName (methodDeclaration.Name, param.Name.UCFirst ());
			var del = CreateDelegateDeclaration (delegateName, delegateType);
			pendingDelegates.Add ((parent, del));

			var newType = new SimpleType (delegateName);
			delegateType.CopyAnnotationsTo (newType);
			param.Type = newType;
		}

		if (methodDeclaration.ReturnType is DelegateType returnDelegateType) {
			var delegateName = GenerateUniqueName (methodDeclaration.Name, "Return");
			var del = CreateDelegateDeclaration (delegateName, returnDelegateType);
			pendingDelegates.Add ((parent, del));

			var newType = new SimpleType (delegateName);
			returnDelegateType.CopyAnnotationsTo (newType);
			methodDeclaration.ReturnType = newType;
		}
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration)
	{
		if (HasVisited (propertyDeclaration))
			return;

		MarkVisited (propertyDeclaration);

		if (propertyDeclaration.ReturnType is not DelegateType delegateType)
			return;

		var parent = propertyDeclaration.Parent as TypeDeclaration;
		if (parent is null)
			return;

		var delegateName = GenerateUniqueName (propertyDeclaration.Name, "Handler");
		var del = CreateDelegateDeclaration (delegateName, delegateType);
		pendingDelegates.Add ((parent, del));

		var newType = new SimpleType (delegateName);
		delegateType.CopyAnnotationsTo (newType);
		propertyDeclaration.ReturnType = newType;
	}

	public override void VisitSyntaxTree (SyntaxTree syntaxTree)
	{
		base.VisitSyntaxTree (syntaxTree);
		FlushPendingDelegates ();
	}

	public override void VisitNamespaceDeclaration (NamespaceDeclaration namespaceDeclaration)
	{
		base.VisitNamespaceDeclaration (namespaceDeclaration);
		FlushPendingDelegates ();
	}

	void FlushPendingDelegates ()
	{
		foreach (var (insertBefore, del) in pendingDelegates) {
			var container = insertBefore.Parent;
			if (container is SyntaxTree syntaxTree)
				syntaxTree.Members.InsertBefore (insertBefore, del);
			else if (container is NamespaceDeclaration ns)
				ns.Members.InsertBefore (insertBefore, del);
		}
		pendingDelegates.Clear ();
	}

	static DelegateDeclaration CreateDelegateDeclaration (string name, DelegateType delegateType)
	{
		var del = new DelegateDeclaration {
			Name = name
		};

		int i = 0;
		foreach (var typeArg in delegateType.TypeArguments) {
			var clone = typeArg.Clone ();
			del.Parameters.Add (new ParameterDeclaration (clone, String.Format ("arg{0}", i++)));
		}

		if (delegateType is FuncType) {
			// For Func<T1, T2, ..., TReturn>, the last type argument is the return type
			var returnParam = del.Parameters.LastOrNullObject ();
			var returnType = returnParam.Type;
			returnType.Remove ();
			returnParam.Remove ();
			del.ReturnType = returnType;
		} else {
			del.ReturnType = new PrimitiveType ("void");
		}

		return del;
	}

	string GenerateUniqueName (string memberName, string? suffix)
	{
		var baseName = memberName + suffix;
		var name = baseName;
		var counter = 2;
		while (!usedNames.Add (name))
			name = baseName + counter++;
		return name;
	}
}
