// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Massagers;

using Type = System.Type;

namespace Sharpie.Bind;

public sealed class BindingMassager {
	public ObjectiveCBinder Binder { get; }

	public BindingMassager (ObjectiveCBinder binder)
	{
		Binder = binder;
	}

	static readonly List<Type> defaultMassagers = new List<Type> {
		typeof (CustomDelegateMassager),
		typeof (DefaultConstructorMassager),
		typeof (DelegateMassager),
		typeof (MethodToPropertyMassager),
		typeof (ProtocolMethodMassager),
		typeof (NullabilityMassager),
		typeof (NSArrayMassager),
		typeof (ObjCTypeMassager),
		typeof (NSStringMassager),
		typeof (ArgumentSemanticMassager),
		typeof (UnsafeMassager),
		typeof (AnonymousStructMassager),
		typeof (UnionStructMassager),
		typeof (AvailabilityMassager),
		typeof (EnumNameMassager),
		typeof (AttributeMassager),
		typeof (GroupedAttributeMassager),
		typeof (PlatformTypeMappingMassager),
		typeof (GenerateUsingStatementsMassager),
		typeof (SortUsingStatementsMassager)
	};

	List<MassagerBase>? _massagers;
	List<MassagerBase> massagers {
		get {
			if (_massagers is null)
				_massagers = defaultMassagers.Select (v => CreateMassager (Binder, v)).ToList ();
			return _massagers;
		}
	}

	int FindMassager (Type massagerType)
		=> massagers.FindIndex (m => m.GetType () == massagerType);
	int FindMassager (string name)
		=> massagers.FindIndex (m => m.GetType () == CreateMassager (Binder, name).GetType ());

	public bool ExcludeMassager (string name)
	{
		var index = FindMassager (name);
		if (index >= 0) {
			massagers.RemoveAt (index);
			return true;
		}

		return false;
	}

	static MassagerBase CreateMassager (ObjectiveCBinder binder, Type type)
	{
		return CreateMassager (binder, type.Name);
	}

	static MassagerBase CreateMassager (ObjectiveCBinder binder, string name)
	{
		const string ns = "Sharpie.Bind.Massagers.";

		if (!name.EndsWith ("Massager", StringComparison.Ordinal))
			name += "Massager";

		if (name.StartsWith (ns + "."))
			name = name.Substring (ns.Length + 1);

		switch (name) {
		case nameof (CustomDelegateMassager):
			return new CustomDelegateMassager (binder);
		case nameof (DefaultConstructorMassager):
			return new DefaultConstructorMassager (binder);
		case nameof (DelegateMassager):
			return new DelegateMassager (binder);
		case nameof (MethodToPropertyMassager):
			return new MethodToPropertyMassager (binder);
		case nameof (ProtocolMethodMassager):
			return new ProtocolMethodMassager (binder);
		case nameof (NullabilityMassager):
			return new NullabilityMassager (binder);
		case nameof (NSArrayMassager):
			return new NSArrayMassager (binder);
		case nameof (ObjCTypeMassager):
			return new ObjCTypeMassager (binder);
		case nameof (NSStringMassager):
			return new NSStringMassager (binder);
		case nameof (ArgumentSemanticMassager):
			return new ArgumentSemanticMassager (binder);
		case nameof (UnsafeMassager):
			return new UnsafeMassager (binder);
		case nameof (AnonymousStructMassager):
			return new AnonymousStructMassager (binder);
		case nameof (UnionStructMassager):
			return new UnionStructMassager (binder);
		case nameof (AvailabilityMassager):
			return new AvailabilityMassager (binder);
		case nameof (EnumNameMassager):
			return new EnumNameMassager (binder);
		case nameof (AttributeMassager):
			return new AttributeMassager (binder);
		case nameof (GroupedAttributeMassager):
			return new GroupedAttributeMassager (binder);
		case nameof (PlatformTypeMappingMassager):
			return new PlatformTypeMappingMassager (binder);
		case nameof (GenerateUsingStatementsMassager):
			return new GenerateUsingStatementsMassager (binder);
		case nameof (SortUsingStatementsMassager):
			return new SortUsingStatementsMassager (binder);
		case nameof (ExplicitBaseTypeNameMassager):
			return new ExplicitBaseTypeNameMassager (binder);
		default:
			throw new Exception ($"Unknown massager: {name}");
		}
	}

	public bool RegisterMassager (string name)
	{
		var massager = CreateMassager (Binder, name);
		if (FindMassager (massager.GetType ()) < 0)
			RegisterMassager (massager);

		return true;
	}

	public void RegisterMassager (MassagerBase massager)
	{
		if (massager is null)
			throw new ArgumentNullException ("massager");

		var registerBefore = massager.GetType ().GetCustomAttribute<RegisterBeforeAttribute> ();
		var registerAfter = massager.GetType ().GetCustomAttribute<RegisterAfterAttribute> ();

		if (registerBefore is not null && registerAfter is not null)
			throw new InvalidOperationException ("a massager cannot have both a " +
				"[RegisterBefore] and [RegisterAfter] attribute");

		for (int i = 0; (registerBefore is not null || registerAfter is not null) && i < massagers.Count; i++) {
			var type = massagers [i].GetType ();

			if (registerBefore is not null && type == registerBefore.Type) {
				massagers.Insert (i, massager);
				return;
			}

			if (registerAfter is not null && type == registerAfter.Type) {
				massagers.Insert (i + 1, massager);
				return;
			}
		}

		massagers.Add (massager);
	}

	public void Massage (AstNode astNode)
	{
		if (astNode is null)
			throw new ArgumentNullException (nameof (astNode));

		foreach (var massager in massagers) {
			if (massager.Initialize ())
				astNode.AcceptVisitor (massager);
		}
	}
}
