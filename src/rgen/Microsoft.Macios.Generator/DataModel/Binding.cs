// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Structure that represents a set of changes that were made by the user that need to be applied to the
/// generated code.
/// </summary>
[StructLayout (LayoutKind.Auto)]
readonly partial struct Binding {

	readonly string name = string.Empty;
	/// <summary>
	/// The name of the named type that generated the code change.
	/// </summary>
	public string Name => name;

	readonly ImmutableArray<string> namespaces = ImmutableArray<string>.Empty;
	/// <summary>
	/// The namespace that contains the named type that generated the code change.
	/// </summary>
	public ImmutableArray<string> Namespace => namespaces;

	readonly ImmutableArray<OuterClass> outerClasses = ImmutableArray<OuterClass>.Empty;

	/// <summary>
	/// The containing classes of the type, if it's a nested type.
	/// </summary>
	public ImmutableArray<OuterClass> OuterClasses => outerClasses;

	readonly ImmutableArray<string> interfaces = ImmutableArray<string>.Empty;
	/// <summary>
	/// The list of interfaces implemented by the class/interface.
	/// </summary>
	public ImmutableArray<string> Interfaces {
		get => interfaces;
		init => interfaces = value;
	}

	/// <summary>
	/// Fully qualified name of the symbol that the code changes are for.
	/// </summary>
	public string FullyQualifiedSymbol { get; }

	readonly string? baseClass = null;

	/// <summary>
	/// The fully qualified name of an interface/class base.
	/// </summary>
	public string? Base {
		get => baseClass;
		init => baseClass = value;

	}

	readonly SymbolAvailability availability = new ();
	/// <summary>
	/// The platform availability of the named type.
	/// </summary>
	public SymbolAvailability SymbolAvailability => availability;

	/// <summary>
	/// Changes to the attributes of the symbol.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; init; } = [];

	readonly IReadOnlySet<string> usingDirectives = ImmutableHashSet<string>.Empty;

	/// <summary>
	/// The using directive added in the named type declaration.
	/// </summary>
	public IReadOnlySet<string> UsingDirectives {
		get => usingDirectives;
		init => usingDirectives = value;
	}

	/// <summary>
	/// True if the code changes are for a static symbol.
	/// </summary>
	public bool IsStatic { get; private init; }

	/// <summary>
	/// True if the code changes are for a partial symbol.
	/// </summary>
	public bool IsPartial { get; private init; }

	/// <summary>
	/// True if the code changes are for an abstract symbol.
	/// </summary>
	public bool IsAbstract { get; private init; }

	readonly ImmutableArray<SyntaxToken> modifiers = [];
	/// <summary>
	/// Modifiers list.
	/// </summary>
	public ImmutableArray<SyntaxToken> Modifiers {
		get => modifiers;
		init {
			modifiers = value;
			IsStatic = value.Any (m => m.IsKind (SyntaxKind.StaticKeyword));
			IsPartial = value.Any (m => m.IsKind (SyntaxKind.PartialKeyword));
			IsAbstract = value.Any (m => m.IsKind (SyntaxKind.AbstractKeyword));
		}
	}

	readonly Dictionary<string, int> enumIndex = new ();
	readonly ImmutableArray<EnumMember> enumMembers = [];

	/// <summary>
	/// Changes to the enum members of the symbol.
	/// </summary>
	public ImmutableArray<EnumMember> EnumMembers {
		get => enumMembers;
		init {
			enumMembers = value;
			// populate the enum index for fast lookup using the symbol name
			for (var index = 0; index < enumMembers.Length; index++) {
				var member = enumMembers [index];
				if (member.Selector is null)
					continue;
				enumIndex [member.Selector] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the selectors for the enum members.
	/// </summary>
	public ImmutableArray<string> EnumMemberSelectors => [.. enumIndex.Keys];

	readonly Dictionary<string, int> propertyIndex = new ();
	readonly ImmutableArray<Property> properties = [];

	/// <summary>
	/// Changes to the properties of the symbol.
	/// </summary>
	public ImmutableArray<Property> Properties {
		get => properties;
		init {
			properties = value;
			// populate the property index for fast lookup using the symbol name
			for (var index = 0; index < properties.Length; index++) {
				var property = properties [index];
				// there are two type of properties, those that are fields and those that are properties
				if (property.Selector is null)
					continue;
				propertyIndex [property.Selector!] = index;
			}
		}
	}

	readonly Dictionary<string, int> parentPropertyIndex = new ();
	readonly ImmutableArray<Property> parentProperties = [];

	/// <summary>
	/// Gets the properties inherited from parent protocols.
	/// </summary>
	public ImmutableArray<Property> ParentProtocolProperties {
		get => parentProperties;
		init {
			parentProperties = value;
			// populate the property index for fast lookup using the symbol name
			for (var index = 0; index < parentProperties.Length; index++) {
				var property = parentProperties [index];
				// there are two type of properties, those that are fields and those that are properties
				if (property.Selector is null)
					continue;
				parentPropertyIndex [property.Selector!] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the selectors for the properties.
	/// </summary>
	public ImmutableArray<string> PropertySelectors => [.. propertyIndex.Keys];

	readonly Dictionary<string, int> strongDictPropertyIndex = new ();
	readonly ImmutableArray<Property> strongDictproperties = [];

	/// <summary>
	/// Changes to the properties of the symbol that is a StrongDictionary.
	/// </summary>
	public ImmutableArray<Property> StrongDictionaryProperties {
		get => strongDictproperties;
		init {
			strongDictproperties = value;
			// populate the property index for fast lookup using the symbol name
			for (var index = 0; index < strongDictproperties.Length; index++) {
				var property = strongDictproperties [index];
				// there are two type of properties, those that are fields and those that are properties
				if (property.Selector is null)
					continue;
				strongDictPropertyIndex [property.Selector!] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the strings that are used as keys in the StrongDictionary properties.
	/// </summary>
	public ImmutableArray<string> StrongDictionaryKeys => [.. strongDictPropertyIndex.Keys];

	readonly Dictionary<string, int> constructorIndex = new ();
	readonly ImmutableArray<Constructor> constructors = [];

	/// <summary>
	/// Changes to the constructors of the symbol.
	/// </summary>
	public ImmutableArray<Constructor> Constructors {
		get => constructors;
		init {
			constructors = value;
			// populate the constructor index for fast lookup using the symbol name
			for (var index = 0; index < constructors.Length; index++) {
				var constructor = constructors [index];
				if (constructor.Selector is null)
					continue;
				constructorIndex [constructor.Selector] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the selectors for the constructors.
	/// </summary>
	public ImmutableArray<string> ConstructorSelectors => [.. constructorIndex.Keys];

	readonly Dictionary<string, int> eventsIndex = new ();
	readonly ImmutableArray<Event> events = [];

	/// <summary>
	/// Changes to the events of the symbol.
	/// </summary>
	public ImmutableArray<Event> Events {
		get => events;
		init {
			events = value;
			// populate the event index for fast lookup using the symbol name
			for (var index = 0; index < events.Length; index++) {
				var eventItem = events [index];
				eventsIndex [eventItem.Name] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the selectors for the events.
	/// </summary>
	public ImmutableArray<string> EventSelectors => [.. eventsIndex.Keys];

	readonly Dictionary<string, int> methodIndex = new ();
	readonly ImmutableArray<Method> methods = [];

	/// <summary>
	/// Changes to the methods of a symbol.
	/// </summary>
	public ImmutableArray<Method> Methods {
		get => methods;
		init {
			methods = value;
			// populate the method index for fast lookup using the symbol name
			for (var index = 0; index < methods.Length; index++) {
				var method = methods [index];
				if (method.Selector is null)
					continue;
				methodIndex [method.Selector] = index;
			}
		}
	}

	readonly Dictionary<string, int> parentMethodIndex = new ();
	readonly ImmutableArray<Method> parentMethods = [];

	/// <summary>
	/// Gets the methods inherited from parent protocols.
	/// </summary>
	public ImmutableArray<Method> ParentProtocolMethods {
		get => parentMethods;
		init {
			parentMethods = value;
			// populate the method index for fast lookup using the symbol name
			for (var index = 0; index < parentMethods.Length; index++) {
				var method = parentMethods [index];
				if (method.Selector is null)
					continue;
				parentMethodIndex [method.Selector] = index;
			}
		}
	}

	/// <summary>
	/// Returns all the selectors for the methods.
	/// </summary>
	public ImmutableArray<string> MethodSelectors => [.. methodIndex.Keys];

	delegate bool SkipDelegate<in T> (T declarationSyntax, SemanticModel semanticModel);

	delegate bool TryCreateDelegate<in T, TR> (T declaration, RootContext context,
		[NotNullWhen (true)] out TR? change)
		where T : MemberDeclarationSyntax
		where TR : struct;

	static void GetMembers<T, TR> (TypeDeclarationSyntax baseDeclarationSyntax, RootContext context,
		SkipDelegate<T> skip, TryCreateDelegate<T, TR> tryCreate, out ImmutableArray<TR> members)
		where T : MemberDeclarationSyntax
		where TR : struct
	{
		var bucket = ImmutableArray.CreateBuilder<TR> ();
		var declarations = baseDeclarationSyntax.Members.OfType<T> ();
		foreach (var declaration in declarations) {
			if (skip (declaration, context.SemanticModel))
				continue;
			if (tryCreate (declaration, context, out var change))
				bucket.Add (change.Value);
		}

		members = bucket.ToImmutable ();
	}

	static bool TryGetFromIndex<T> (string selector, ImmutableArray<T> collection, Dictionary<string, int> index, [NotNullWhen (true)] out T? value)
		where T : struct
	{
		if (index.TryGetValue (selector, out var indexValue)) {
			value = collection [indexValue];
			return true;
		}

		value = null;
		return false;
	}

	/// <summary>
	/// Get the enum member that matches the field name.
	/// </summary>
	/// <param name="fieldName">The native field name.</param>
	/// <param name="enumMember">The enum member that matches the field name.</param>
	/// <returns>True if the enum member was found. False otherwise.</returns>
	public bool TryGetEnumValue (string fieldName, out EnumMember? enumMember)
		=> TryGetFromIndex (fieldName, enumMembers, enumIndex, out enumMember);

	/// <summary>
	/// Get the property that matches the selector.
	/// </summary>
	/// <param name="selector">The selector used for the property. It can also be a field name.</param>
	/// <param name="property">The property that matches the given selector/field name.</param>
	/// <returns>True if the property was found. False otherwise.</returns>
	public bool TryGetProperty (string selector, out Property? property)
		=> TryGetFromIndex (selector, properties, propertyIndex, out property);

	/// <summary>
	/// Get the constructor that matches the selector.
	/// </summary>
	/// <param name="selector">The selector used for the constructor.</param>
	/// <param name="constructor">The constructor that matches the given selector.</param>
	/// <returns>True if the constructor was found. False otherwise.</returns>
	public bool TryGetConstructor (string selector, out Constructor? constructor)
		=> TryGetFromIndex (selector, constructors, constructorIndex, out constructor);

	/// <summary>
	/// Get the event that matches the selector.
	/// </summary>
	/// <param name="selector">The selector used for the event.</param>
	/// <param name="event">The event that matches the given selector.</param>
	/// <returns>True if the event was found. False otherwise.</returns>
	public bool TryGetEvent (string selector, out Event? @event)
		=> TryGetFromIndex (selector, events, eventsIndex, out @event);

	/// <summary>
	/// Get The method that matches the selector.
	/// </summary>
	/// <param name="selector">The selector used for the method.</param>
	/// <param name="method">The method that matches the given selector.</param>
	/// <returns>True if the method was found. False otherwise.</returns>
	public bool TryGetMethod (string selector, out Method? method)
		=> TryGetFromIndex (selector, methods, methodIndex, out method);
}
