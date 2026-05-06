// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

[AttributeUsage (AttributeTargets.Class)]
public sealed class RegisterBeforeAttribute : System.Attribute {
	public System.Type Type { get; private set; }

	public RegisterBeforeAttribute (System.Type type)
	{
		if (!typeof (MassagerBase).IsAssignableFrom (type))
			throw new ArgumentException (type.FullName + " is not a MassagerBase", "type");

		Type = type;
	}
}

[AttributeUsage (AttributeTargets.Class)]
public sealed class RegisterAfterAttribute : System.Attribute {
	public System.Type Type { get; private set; }

	public RegisterAfterAttribute (System.Type type)
	{
		if (!typeof (MassagerBase).IsAssignableFrom (type))
			throw new ArgumentException (type.FullName + " is not a MassagerBase", "type");

		Type = type;
	}
}

public abstract class MassagerBase : DepthFirstAstVisitor {
	public ObjectiveCBinder Binder { get; private set; }

	public MassagerBase (ObjectiveCBinder binder)
	{
		Binder = binder;
	}

	public virtual bool Initialize ()
	{
		return true;
	}
}

public abstract class Massager<T> : MassagerBase where T : MassagerBase {
	static readonly HashSet<AstNode> visitedNodes = new HashSet<AstNode> ();

	public Massager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	/// <summary>
	/// Determines if any instance has visited the specified node.
	/// </summary>
	/// <returns><c>true</c> if any instance has visited the node; otherwise, <c>false</c>.</returns>
	public static bool HasVisited (AstNode node)
	{
		return visitedNodes.Contains (node);
	}

	/// <summary>
	/// Marks the specified node as visited for any and all instances of the massager.
	/// </summary>
	protected static void MarkVisited (AstNode node)
	{
		visitedNodes.Add (node);
	}
}
