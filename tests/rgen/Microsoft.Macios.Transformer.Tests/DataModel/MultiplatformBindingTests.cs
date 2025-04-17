using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Transformer.Attributes;
using Microsoft.Macios.Transformer.DataModel;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Transformer.Tests.DataModel;

public class MultiplatformBindingTests {
	readonly Binding iosBinding;
	readonly Binding tvosBinding;
	readonly Binding maccatalystBinding;
	readonly Binding macosBinding;
	readonly MultiplatformBinding multiplatformBinding;

	public MultiplatformBindingTests ()
	{
		// build a number of bindings to test against, we will provide different members so that we can
		var info = new BindingInfo (
			baseTypeData: new (),
			bindingType: BindingType.Class);
		var className = "MyClass";
		var nameSpace = ImmutableArray.Create<string> ("NS");
		var fullyQualifiedSymbol = "NS.MyClass";

		iosBinding = new Binding (
			name: className,
			@namespace: nameSpace,
			fullyQualifiedSymbol: fullyQualifiedSymbol,
			bindingInfo: info,
			symbolAvailability: new (),
			attributes: []
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			],
			Constructors = [
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("init"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: [
						new Parameter (0, ReturnTypeForBool (), "flag"),
					]
				) {
					ExportMethodData = new ("initWithFlag:"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("initIOS"),
				},
			],
			Properties = [
				new (
					name: "Name",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("name")
				},
				new (
					name: "FrameworkConstant",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportFieldData = new ("FrameworkConstant"),
				},
				new (
					name: "iOS",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("ios")
				},
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "SetName",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name")
					]
				) {
					ExportMethodData = new ("withName:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetSurname",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "surname")
					]
				) {
					ExportMethodData = new ("withSurname:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetiOS",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "ios")
					]
				) {
					ExportMethodData = new ("withIOS:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
			],
			Events = [
				new (
					name: "Changed",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				),
				new (
					name: "iOSChanged",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				)
			],
		};

		tvosBinding = new Binding (
			name: className,
			@namespace: nameSpace,
			fullyQualifiedSymbol: fullyQualifiedSymbol,
			bindingInfo: info,
			symbolAvailability: new (),
			attributes: []
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			],
			Constructors = [
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("init"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: [
						new Parameter (0, ReturnTypeForBool (), "flag"),
					]
				) {
					ExportMethodData = new ("initWithFlag:"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("initTVOS"),
				},
			],
			Properties = [
				new (
					name: "Name",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("name")
				},
				new (
					name: "FrameworkConstant",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportFieldData = new ("FrameworkConstant"),
				},
				new (
					name: "tvOS",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("tvos")
				},
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "SetName",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name")
					]
				) {
					ExportMethodData = new ("withName:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetSurname",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "surname")
					]
				) {
					ExportMethodData = new ("withSurname:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetTVOS",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "ios")
					]
				) {
					ExportMethodData = new ("withTVOS:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
			],
			Events = [
				new (
					name: "Changed",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				),
				new (
					name: "tvOSChanged",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				)
			],
		};

		maccatalystBinding = new Binding (
			name: className,
			@namespace: nameSpace,
			fullyQualifiedSymbol: fullyQualifiedSymbol,
			bindingInfo: info,
			symbolAvailability: new (),
			attributes: []
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			],
			Constructors = [
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("init"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: [
						new Parameter (0, ReturnTypeForBool (), "flag"),
					]
				) {
					ExportMethodData = new ("initWithFlag:"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("initCatalyst"),
				},
			],
			Properties = [
				new (
					name: "Name",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("name")
				},
				new (
					name: "FrameworkConstant",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportFieldData = new ("FrameworkConstant"),
				},
				new (
					name: "Catalyst",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("catalyst")
				},
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "SetName",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name")
					]
				) {
					ExportMethodData = new ("withName:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetSurname",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "surname")
					]
				) {
					ExportMethodData = new ("withSurname:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetCatalyst",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "ios")
					]
				) {
					ExportMethodData = new ("withCatalyst:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
			],
			Events = [
				new (
					name: "Changed",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				),
				new (
					name: "CatalystChanged",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				)
			],
		};

		macosBinding = new Binding (
			name: className,
			@namespace: nameSpace,
			fullyQualifiedSymbol: fullyQualifiedSymbol,
			bindingInfo: info,
			symbolAvailability: new (),
			attributes: []
		) {
			Base = "object",
			Interfaces = ImmutableArray<string>.Empty,
			Attributes = [
				new ("ObjCBindings.BindingTypeAttribute<ObjCBindings.Class>")
			],
			UsingDirectives = new HashSet<string> { "ObjCBindings" },
			Modifiers = [
				SyntaxFactory.Token (SyntaxKind.PublicKeyword),
				SyntaxFactory.Token (SyntaxKind.PartialKeyword)
			],
			Constructors = [
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("init"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: [
						new Parameter (0, ReturnTypeForBool (), "flag"),
					]
				) {
					ExportMethodData = new ("initWithFlag:"),
				},
				new (
					type: className,
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword)
					],
					parameters: []
				) {
					ExportMethodData = new ("initMacOS"),
				},
			],
			Properties = [
				new (
					name: "Name",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("name")
				},
				new (
					name: "FrameworkConstant",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportFieldData = new ("FrameworkConstant"),
				},
				new (
					name: "macOS",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					attributes: [],
					accessors: [
						new (
							accessorKind: AccessorKind.Getter,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Setter,
							symbolAvailability: new (),
							attributes: []
						),
					]
				) {
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
					ExportPropertyData = new ExportData ("macos")
				},
			],
			Methods = [
				new (
					type: "NS.MyClass",
					name: "SetName",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "name")
					]
				) {
					ExportMethodData = new ("withName:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetSurname",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "surname")
					]
				) {
					ExportMethodData = new ("withSurname:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
				new (
					type: "NS.MyClass",
					name: "SetCatalyst",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					attributes: [],
					parameters: [
						new (position: 0, type: ReturnTypeForString (), name: "ios")
					]
				) {
					ExportMethodData = new ("withMacOS:"),
					Modifiers = [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
						SyntaxFactory.Token (SyntaxKind.PartialKeyword),
					],
				},
			],
			Events = [
				new (
					name: "Changed",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				),
				new (
					name: "macOSChanged",
					type: "System.EventHandler",
					symbolAvailability: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					accessors: [
						new (
							accessorKind: AccessorKind.Add,
							symbolAvailability: new (),
							attributes: []
						),
						new (
							accessorKind: AccessorKind.Remove,
							symbolAvailability: new (),
							attributes: []
						)
					]
				)
			],
		};
		multiplatformBinding = new MultiplatformBinding (
			(PlatformName.iOS, iosBinding),
			(PlatformName.TvOS, tvosBinding),
			(PlatformName.MacOSX, macosBinding),
			(PlatformName.MacCatalyst, maccatalystBinding)
		);
	}

	[Fact]
	public void ConstructorTest ()
	{
		var binding = new MultiplatformBinding (
			(PlatformName.iOS, iosBinding),
			(PlatformName.TvOS, tvosBinding),
			(PlatformName.MacOSX, macosBinding),
			(PlatformName.MacCatalyst, maccatalystBinding)
		);
		Assert.True (binding.HasPlatform (PlatformName.iOS));
		Assert.True (binding.HasPlatform (PlatformName.TvOS));
		Assert.True (binding.HasPlatform (PlatformName.MacCatalyst));
		Assert.True (binding.HasPlatform (PlatformName.MacOSX));

		binding = new MultiplatformBinding (
			(PlatformName.MacOSX, macosBinding),
			(PlatformName.MacCatalyst, maccatalystBinding)
		);

		Assert.False (binding.HasPlatform (PlatformName.iOS));
		Assert.False (binding.HasPlatform (PlatformName.TvOS));
		Assert.True (binding.HasPlatform (PlatformName.MacCatalyst));
		Assert.True (binding.HasPlatform (PlatformName.MacOSX));
	}

	[Fact]
	public void PropertySelectorsTest ()
	{
		var selectors = multiplatformBinding.PropertySelectors.ToArray ();
		// shared
		Assert.Contains ("name", selectors);
		Assert.Contains ("FrameworkConstant", selectors);
		// per platform
		Assert.Contains ("ios", selectors);
		Assert.Contains ("tvos", selectors);
		Assert.Contains ("macos", selectors);
		Assert.Contains ("catalyst", selectors);
	}

	[Fact]
	public void ConstructorSelectorsTest ()
	{
		var selectors = multiplatformBinding.ConstructorSelectors.ToArray ();
		// shared
		Assert.Contains ("init", selectors);
		Assert.Contains ("initWithFlag:", selectors);
		// per platform
		Assert.Contains ("initIOS", selectors);
		Assert.Contains ("initTVOS", selectors);
		Assert.Contains ("initCatalyst", selectors);
		Assert.Contains ("initMacOS", selectors);
	}

	[Fact]
	public void EventSelectorsTest ()
	{
		var selectors = multiplatformBinding.EventSelectors.ToArray ();
		// shared
		Assert.Contains ("Changed", selectors);
		// per platform
		Assert.Contains ("iOSChanged", selectors);
		Assert.Contains ("tvOSChanged", selectors);
		Assert.Contains ("CatalystChanged", selectors);
		Assert.Contains ("macOSChanged", selectors);
	}

	[Fact]
	public void MethodSelectorsTest ()
	{
		var selectors = multiplatformBinding.MethodSelectors.ToArray ();
		// shared
		Assert.Contains ("withName:", selectors);
		Assert.Contains ("withSurname:", selectors);
		// per platform
		Assert.Contains ("withIOS:", selectors);
		Assert.Contains ("withTVOS:", selectors);
		Assert.Contains ("withCatalyst:", selectors);
		Assert.Contains ("withMacOS:", selectors);
	}

}
