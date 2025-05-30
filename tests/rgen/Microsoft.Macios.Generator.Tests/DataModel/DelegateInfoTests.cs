// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class DelegateInfoTests : BaseGeneratorTestClass {

	class TestDataFromMethodDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string actionNoParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Action cb) {}
	}
}
";

			yield return [
				actionNoParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForAction (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForVoid (),
									parameters: []
								)
							),
							name: "cb"
						)
					]
				)
			];

			const string actionSingleParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Action<string> cb) {}
	}
}
";

			yield return [
				actionSingleParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForAction (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForVoid (),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "obj"
										),
									]
								),
								"string"),
							name: "cb"
						)
					]
				)
			];

			const string actionSingleNullableParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Action<string?> cb) {}
	}
}
";

			yield return [
				actionSingleNullableParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForAction (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForVoid (),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (isNullable: true),
											name: "obj"
										),
									]
								),
								"string?"),
							name: "cb"
						)
					]
				)
			];

			const string actionMultiParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Action<string, string> cb) {}
	}
}
";

			yield return [
				actionMultiParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForAction (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForVoid (),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "arg1"
										),
										new (
											position: 1,
											type: ReturnTypeForString (),
											name: "arg2"
										),
									]
								),
								"string", "string"),
							name: "cb"
						)
					]
				)
			];

			const string funcSingleParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Func<string, string> cb) {}
	}
}
";

			yield return [
				funcSingleParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForFunc (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForString (),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "arg"
										),
									]
								),
								"string", "string"),
							name: "cb"
						)
					]
				)
			];

			const string funcMultiParam = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod (Func<string, string, string> cb) {}
	}
}
";

			yield return [
				funcMultiParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForFunc (
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForString (),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "arg1"
										),
										new (
											position: 1,
											type: ReturnTypeForString (),
											name: "arg2"
										),
									]
								),
								"string", "string", "string"),
							name: "cb"
						)
					]
				)
			];

			const string customDelegate = @"
using System;

namespace NS {
	public class MyClass {
		public delegate int? Callback(string name, string? middleName, params string[] surname);

		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				customDelegate,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "name"
										),
										new (
											position: 1,
											type: ReturnTypeForString (isNullable: true),
											name: "middleName"
										),
										new (
											position: 2,
											type: ReturnTypeForArray ("string", isBlittable: false),
											name: "surname"
										) {
											IsParams = true,
										},
									]
								)
							),
							name: "cb"
						)
					]
				)
			];

			const string customDelegateForcedType = @"
using System;
using Foundation;
using ObjCBindings;

namespace NS {

	public class MyNSObject : NSObject {
	}

	public class MyClass {
		public delegate int? Callback([ForcedType] MyNSObject name);

		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				customDelegateForcedType,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForNSObject (nsObjectName: "NS.MyNSObject"),
											name: "name"
										) {
											ForcedType = new (),
										},
									]
								)
							),
							name: "cb"
						)
					]
				)
			];

			const string customDelegateCcallback = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS {

	public class MyNSObject : NSObject {
	}

	public class MyClass {
		public delegate int? Callback([ForcedType] MyNSObject name);

		public void MyMethod ([CCallback] Callback cb) {}
	}
}
";

			yield return [
				customDelegateCcallback,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForNSObject (nsObjectName: "NS.MyNSObject"),
											name: "name"
										) {
											ForcedType = new (),
										},
									]
								) {
									IsCCallback = true,
								}
							),
							name: "cb"
						) {
							Attributes = [
								new ("ObjCRuntime.CCallbackAttribute")
							]
						}
					]
				)
			];

			const string customDelegateBlockcallback = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS {

	public class MyNSObject : NSObject {
	}

	public class MyClass {
		public delegate int? Callback([ForcedType] MyNSObject name);

		public void MyMethod ([BlockCallback] Callback cb) {}
	}
}
";

			yield return [
				customDelegateBlockcallback,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForNSObject (nsObjectName: "NS.MyNSObject"),
											name: "name"
										) {
											ForcedType = new (),
										},
									]
								) {
									IsBlockCallback = true,
								}
							),
							name: "cb"
						) {
							Attributes = [
								new ("ObjCRuntime.BlockCallbackAttribute")
							]
						}
					]
				)
			];

			const string customDelegateKeywordParam = @"
using System;

namespace NS {
	public class MyClass {
		public delegate int? Callback(string @event, string? middleName, params string[] surname);

		public void MyMethod (Callback cb) {}
	}
}
";

			yield return [
				customDelegateKeywordParam,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForString (),
											name: "@event"
										),
										new (
											position: 1,
											type: ReturnTypeForString (isNullable: true),
											name: "middleName"
										),
										new (
											position: 2,
											type: ReturnTypeForArray ("string", isBlittable: false),
											name: "surname"
										) {
											IsParams = true,
										},
									]
								)
							),
							name: "cb"
						)
					]
				)
			];

			const string customDelegateBindFrom = @"
using System;
using Foundation;
using ObjCRuntime;
using ObjCBindings;

namespace NS {

	public class MyNSObject : NSObject {
	}

	public class MyClass {
		public delegate int? Callback([BindFrom (typeof(NSNumber))] int value);

		public void MyMethod ([BlockCallback] Callback cb) {}
	}
}
";

			yield return [
				customDelegateBindFrom,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: [
						new (
							position: 0,
							type: ReturnTypeForDelegate (
								"NS.MyClass.Callback",
								delegateInfo: new (
									name: "Invoke",
									returnType: ReturnTypeForInt (isNullable: true),
									parameters: [
										new (
											position: 0,
											type: ReturnTypeForInt (),
											name: "value"
										) {
											BindAs = new (ReturnTypeForNSObject ("Foundation.NSNumber")),
										},
									]
								) {
									IsBlockCallback = true,
								}
							),
							name: "cb"
						) {
							Attributes = [
								new ("ObjCRuntime.BlockCallbackAttribute")
							]
						}
					]
				)
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFromMethodDeclaration>]
	void FromMethodDeclaration (ApplePlatform platform, string inputText, Method expected)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		Assert.Equal (expected, changes);
	}
}
