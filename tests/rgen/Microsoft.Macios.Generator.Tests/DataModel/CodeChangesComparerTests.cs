using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class CodeChangesComparerTests : BaseGeneratorTestClass {
	readonly CodeChangesEqualityComparer comparer = new ();

	[Fact]
	public void CompareDifferentFullyQualifiedSymbol ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name1");
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name2");
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentBindingType ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name");
		var changes2 = new CodeChanges (BindingType.Unknown, "name");
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentAttributesLength ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name");
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			Attributes = [
				new AttributeCodeChange ("name", ["arg1", "arg2"])
			]
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentAttributes ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			Attributes = [
				new AttributeCodeChange ("name", ["arg1", "arg2"])
			],
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			Attributes = [
				new AttributeCodeChange ("name2", ["arg1", "arg2"])
			],
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMembersLength ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name");
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [
				new EnumMember ("name", [])
			],
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMembers ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [
				new EnumMember ("name", [])
			],
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [
				new EnumMember ("name2", [])
			],
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentPropertyLength ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = []
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					])
			]
		};

		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSamePropertiesDiffOrder ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			]
		};
		Assert.True (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentProperties ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			]
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentEventsLength ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			]
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSameEventsDiffOrder ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			]
		};

		Assert.True (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentEvents ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.InternalKeyword),
					],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			]
		};
		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMethodsLength ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "TryGetString",
					returnType: "bool",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "string?", "example") {
							IsNullable = true,
							ReferenceKind = ReferenceKind.Out,
						},
					]
				),
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: "void",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "NS.CustomType", "input")
					]
				)
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "TryGetString",
					returnType: "bool",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "string?", "example") {
							IsNullable = true,
							ReferenceKind = ReferenceKind.Out,
						},
					]
				),
			]
		};

		Assert.False (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareSameMethodsDiffOrder ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "TryGetString",
					returnType: "bool",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "string?", "example") {
							IsNullable = true,
							ReferenceKind = ReferenceKind.Out,
						},
					]
				),
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: "void",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "NS.CustomType", "input")
					]
				)
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			],
			Methods = [
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: "void",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "NS.CustomType", "input")
					]
				),
				new (
					type: "NS.MyClass",
					name: "TryGetString",
					returnType: "bool",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "string?", "example") {
							IsNullable = true,
							ReferenceKind = ReferenceKind.Out,
						},
					]
				),
			]
		};

		Assert.True (comparer.Equals (changes1, changes2));
	}

	[Fact]
	public void CompareDifferentMethods ()
	{
		var changes1 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
			],
			Methods = [
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: "void",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "NS.CustomType", "input")
					]
				),
			]
		};
		var changes2 = new CodeChanges (BindingType.SmartEnum, "name") {
			EnumMembers = [],
			Properties = [
				new (
					name: "Name",
					type: "Utils.MyClass",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios18.0"]),
						], []),
					]),
				new (
					name: "Surname",
					type: "string",
					attributes: [
						new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios"]),
					],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (AccessorKind.Getter, [
							new ("System.Runtime.Versioning.SupportedOSPlatformAttribute", ["ios17.0"]),
						], []),
						new (AccessorKind.Setter, [], []),
					]),
			],
			Events = [
				new (
					name: "MyEvent2",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
						new (AccessorKind.Remove, [], []),
					]),
				new (
					name: "MyEvent",
					type: "System.EventHandler",
					attributes: [],
					modifiers: [],
					accessors: [
						new (AccessorKind.Add, [], []),
					]),
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "TryGetString",
					returnType: "bool",
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (0, "string?", "example") {
							IsNullable = true,
							ReferenceKind = ReferenceKind.Out,
						},
					]
				),
			]
		};

		Assert.False (comparer.Equals (changes1, changes2));
	}
}
