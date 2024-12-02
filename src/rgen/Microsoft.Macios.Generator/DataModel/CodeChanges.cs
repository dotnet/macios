using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Structure that represents a set of changes that were made by the user that need to be applied to the
/// generated code.
/// </summary>
readonly struct CodeChanges {
	/// <summary>
	/// Represents the type of binding that the code changes are for.
	/// </summary>
	public BindingType BindingType { get; } = BindingType.Unknown;

	/// <summary>
	/// Fully qualified name of the symbol that the code changes are for.
	/// </summary>
	public string FullyQualifiedSymbol { get; }

	/// <summary>
	/// Changes to the attributes of the symbol.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; init; } = [];

	readonly ImmutableArray<EnumMember> enumMembers = [];

	/// <summary>
	/// Changes to the enum members of the symbol.
	/// </summary>
	public ImmutableArray<EnumMember> EnumMembers {
		get => enumMembers;
		init => enumMembers = value;
	}

	readonly ImmutableArray<Property> properties = [];

	/// <summary>
	/// Changes to the properties of the symbol.
	/// </summary>
	public ImmutableArray<Property> Properties {
		get => properties;
		init => properties = value;
	}

	readonly ImmutableArray<Constructor> constructors = [];

	/// <summary>
	/// Changes to the constructors of the symbol.
	/// </summary>
	public ImmutableArray<Constructor> Constructors {
		get => constructors;
		init => constructors = value;
	}

	readonly ImmutableArray<Event> events = [];

	/// <summary>
	/// Changes to the events of the symbol.
	/// </summary>
	public ImmutableArray<Event> Events {
		get => events;
		init => events = value;
	}

	readonly ImmutableArray<Method> methods = [];

	/// <summary>
	/// Changes to the methods of a symbol.
	/// </summary>
	public ImmutableArray<Method> Methods {
		get => methods;
		init => methods = value;
	}

	/// <summary>
	/// Decide if an enum value should be ignored as a change.
	/// </summary>
	/// <param name="enumMemberDeclarationSyntax">The enum declaration under test.</param>
	/// <param name="semanticModel">The semantic model of the compilation.</param>
	/// <returns>True if the enum value should be ignored. False otherwise.</returns>
	internal static bool Skip (EnumMemberDeclarationSyntax enumMemberDeclarationSyntax, SemanticModel semanticModel)
	{
		// for smart enums, we are only interested in the field that has a Field<EnumValue> attribute
		return !enumMemberDeclarationSyntax.HasAttribute (semanticModel, AttributesNames.EnumFieldAttribute);
	}

	/// <summary>
	/// Decide if a property should be ignored as a change.
	/// </summary>
	/// <param name="propertyDeclarationSyntax">The property declaration under test.</param>
	/// <param name="semanticModel">The semantic model of the compilation.</param>
	/// <returns>True if the property should be ignored. False otherwise.</returns>
	internal static bool Skip (PropertyDeclarationSyntax propertyDeclarationSyntax, SemanticModel semanticModel)
	{
		// valid properties are: 
		// 1. Partial
		// 2. One of the following:
		//	  1. Field properties
		//    2. Exported properties
		if (propertyDeclarationSyntax.Modifiers.Any (SyntaxKind.PartialKeyword)) {
			return !propertyDeclarationSyntax.HasAtLeastOneAttribute (semanticModel,
				AttributesNames.ExportFieldAttribute, AttributesNames.ExportPropertyAttribute);
		}

		return true;
	}

	internal static bool Skip (ConstructorDeclarationSyntax constructorDeclarationSyntax, SemanticModel semanticModel)
	{
		// TODO: we need to confirm this when we have support from the roslyn team.
		return false;
	}

	internal static bool Skip (EventDeclarationSyntax eventDeclarationSyntax, SemanticModel semanticModel)
	{
		// TODO: we need to confirm this when we have support from the roslyn team.
		return false;
	}

	internal static bool Skip (MethodDeclarationSyntax methodDeclarationSyntax, SemanticModel semanticModel)
	{
		// Valid methods are:
		// 1. Partial
		// 2. Contain the export attribute
		if (methodDeclarationSyntax.Modifiers.Any (SyntaxKind.PartialKeyword)) {
			return !methodDeclarationSyntax.HasAttribute (semanticModel, AttributesNames.ExportMethodAttribute);
		}

		return true;
	}

	delegate bool SkipDelegate<in T> (T declarationSyntax, SemanticModel semanticModel);

	delegate bool TryCreateDelegate<in T, TR> (T declaration, SemanticModel semanticModel,
		[NotNullWhen (true)] out TR? change)
		where T : MemberDeclarationSyntax
		where TR : struct;

	static void GetMembers<T, TR> (TypeDeclarationSyntax baseDeclarationSyntax, SemanticModel semanticModel,
		SkipDelegate<T> skip, TryCreateDelegate<T, TR> tryCreate, out ImmutableArray<TR> members)
		where T : MemberDeclarationSyntax
		where TR : struct
	{
		var bucket = ImmutableArray.CreateBuilder<TR> ();
		var declarations = baseDeclarationSyntax.Members.OfType<T> ();
		foreach (var declaration in declarations) {
			if (skip (declaration, semanticModel))
				continue;
			if (tryCreate (declaration, semanticModel, out var change))
				bucket.Add (change.Value);
		}

		members = bucket.ToImmutable ();
	}

	/// <summary>
	/// Internal constructor added for testing purposes.
	/// </summary>
	/// <param name="bindingType">The type of binding for the given code changes.</param>
	/// <param name="fullyQualifiedSymbol">The fully qualified name of the symbol.</param>
	internal CodeChanges (BindingType bindingType, string fullyQualifiedSymbol)
	{
		BindingType = bindingType;
		FullyQualifiedSymbol = fullyQualifiedSymbol;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="CodeChanges"/> struct for a given enum declaration.
	/// </summary>
	/// <param name="enumDeclaration">The enum declaration that triggered the change.</param>
	/// <param name="semanticModel">The semantic model of the compilation.</param>
	CodeChanges (EnumDeclarationSyntax enumDeclaration, SemanticModel semanticModel)
	{
		BindingType = BindingType.SmartEnum;
		FullyQualifiedSymbol = enumDeclaration.GetFullyQualifiedIdentifier ();
		Attributes = enumDeclaration.GetAttributeCodeChanges (semanticModel);
		var bucket = ImmutableArray.CreateBuilder<EnumMember> ();
		// loop over the fields and add those that contain a FieldAttribute
		var enumValueDeclaration = enumDeclaration.Members.OfType<EnumMemberDeclarationSyntax> ();
		foreach (var val in enumValueDeclaration) {
			if (Skip (val, semanticModel))
				continue;
			var memberName = val.Identifier.ToFullString ().Trim ();
			var attributes = val.GetAttributeCodeChanges (semanticModel);
			bucket.Add (new (memberName, attributes));
		}

		EnumMembers = bucket.ToImmutable ();
	}

	/// <summary>
	/// Creates a new instance of the <see cref="CodeChanges"/> struct for a given class declaration.
	/// </summary>
	/// <param name="classDeclaration">The class declaration that triggered the change.</param>
	/// <param name="semanticModel">The semantic model of the compilation.</param>
	CodeChanges (ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
	{
		BindingType = BindingType.Class;
		FullyQualifiedSymbol = classDeclaration.GetFullyQualifiedIdentifier ();
		Attributes = classDeclaration.GetAttributeCodeChanges (semanticModel);

		// use the generic method to get the members, we are using an out param to try an minimize the number of times
		// the value types are copied
		GetMembers<ConstructorDeclarationSyntax, Constructor> (classDeclaration, semanticModel, Skip,
			Constructor.TryCreate, out constructors);
		GetMembers<PropertyDeclarationSyntax, Property> (classDeclaration, semanticModel, Skip, Property.TryCreate,
			out properties);
		GetMembers<EventDeclarationSyntax, Event> (classDeclaration, semanticModel, Skip, Event.TryCreate, out events);
		GetMembers<MethodDeclarationSyntax, Method> (classDeclaration, semanticModel, Skip, Method.TryCreate,
			out methods);
	}

	/// <summary>
	/// Creates a new instance of the <see cref="CodeChanges"/> struct for a given interface declaration.
	/// </summary>
	/// <param name="interfaceDeclaration">The interface declaration that triggered the change.</param>
	/// <param name="semanticModel">The semantic model of the compilation.</param>
	CodeChanges (InterfaceDeclarationSyntax interfaceDeclaration, SemanticModel semanticModel)
	{
		BindingType = BindingType.Protocol;
		FullyQualifiedSymbol = interfaceDeclaration.GetFullyQualifiedIdentifier ();
		Attributes = interfaceDeclaration.GetAttributeCodeChanges (semanticModel);
		// we do not init the constructors, we use the default empty array

		GetMembers<PropertyDeclarationSyntax, Property> (interfaceDeclaration, semanticModel, Skip, Property.TryCreate,
			out properties);
		GetMembers<EventDeclarationSyntax, Event> (interfaceDeclaration, semanticModel, Skip, Event.TryCreate,
			out events);
		GetMembers<MethodDeclarationSyntax, Method> (interfaceDeclaration, semanticModel, Skip, Method.TryCreate,
			out methods);
	}

	/// <summary>
	/// Create a CodeChange from the provide base type declaration syntax. If the syntax is not supported,
	/// it will return null.
	/// </summary>
	/// <param name="baseTypeDeclarationSyntax">The declaration syntax whose change we want to calculate.</param>
	/// <param name="semanticModel">The semantic model related to the syntax tree that contains the node.</param>
	/// <returns>A code change or null if it could not be calculated.</returns>
	public static CodeChanges? FromDeclaration (BaseTypeDeclarationSyntax baseTypeDeclarationSyntax,
		SemanticModel semanticModel)
		=> baseTypeDeclarationSyntax switch {
			EnumDeclarationSyntax enumDeclarationSyntax => new CodeChanges (enumDeclarationSyntax, semanticModel),
			InterfaceDeclarationSyntax interfaceDeclarationSyntax => new CodeChanges (interfaceDeclarationSyntax,
				semanticModel),
			ClassDeclarationSyntax classDeclarationSyntax => new CodeChanges (classDeclarationSyntax, semanticModel),
			_ => null
		};

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ("Changes: {");
		sb.Append ($"BindingType: {BindingType}, ");
		sb.Append ($"FullyQualifiedSymbol: {FullyQualifiedSymbol}, ");
		sb.Append ("Attributes: [");
		sb.AppendJoin (", ", Attributes);
		sb.Append ("], EnumMembers: [");
		sb.AppendJoin (", ", EnumMembers);
		sb.Append ("], Constructors: [");
		sb.AppendJoin (", ", Constructors);
		sb.Append ("], Properties: [");
		sb.AppendJoin (", ", Properties);
		sb.Append ("], Methods: [");
		sb.AppendJoin (", ", Methods);
		sb.Append ("], Events: [");
		sb.AppendJoin (", ", Events);
		sb.Append ('}');
		return sb.ToString ();
	}
}
