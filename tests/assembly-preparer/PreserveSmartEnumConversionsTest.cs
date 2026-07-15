// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class PreserveSmartEnumConversionsTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void MarkedTest (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using CoreAnimation;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))]
			public CAToneMapMode RWProperty { get; set; }

			[BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))]
			public CAToneMapMode ROProperty { get { return default; } }
			
			[BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))]
			public CAToneMapMode WOProperty { set { } }

			[return: BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))]
			public CAToneMapMode Method1 () { return default; }

			public void Method2 ([BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))] CAToneMapMode p1) {}

			public void Method3 ([BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))] CAToneMapMode p1, [BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))] CAToneMapMode p2) {}

			[return: BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))]
			public CAToneMapMode Method4 ([BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))] CAToneMapMode p1, [BindAs (typeof (CAToneMapMode), OriginalType = typeof (NSString))] CAToneMapMode p2) { return default;}
		}";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var cctor = type.GetStaticConstructor ();
		Assert.That (cctor, Is.Null, "No static constructor should be needed.");

		void AssertHasDynamicDependency (ICustomAttributeProvider provider, string memberSignature, string typeName, string assemblyName)
		{
			var ddaAttributes = provider.CustomAttributes.Where (v => v.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute").ToArray ();
			var found = 0;
			foreach (var ca in ddaAttributes) {
				if (ca.ConstructorArguments.Count != 3)
					continue;
				var attribMemberSignature = (string) ca.ConstructorArguments [0].Value!;
				if (attribMemberSignature != memberSignature)
					continue;
				var attribTypeName = (string) ca.ConstructorArguments [1].Value!;
				if (attribTypeName != typeName)
					continue;
				var attribAssemblyName = (string) ca.ConstructorArguments [2].Value!;
				if (attribAssemblyName != assemblyName)
					continue;

				found++;
			}

			if (found == 1)
				return;

			var attributesAsString = ddaAttributes
				.Select (v => {
					switch (v.ConstructorArguments.Count) {
					case 3:
						return $"[DynamicDependency (\"{v.ConstructorArguments [0].Value}\", \"{v.ConstructorArguments [1].Value}\", \"{v.ConstructorArguments [2].Value}\")]";
					case 2:
						return $"[DynamicDependency (\"{v.ConstructorArguments [0].Value}\", \"{v.ConstructorArguments [1].Value}\")]";
					case 1:
						return $"[DynamicDependency (\"{v.ConstructorArguments [0].Value}\")]";
					default:
						return string.Join (", ", v.ConstructorArguments.Select (x => x.Value?.ToString () ?? "null"));
					}
				})
				.OrderBy (v => v)
				.ToArray ();

			string msg;
			if (found == 0) {
				if (attributesAsString.Length == 0) {
					msg = $"Expected [DynamicDependency (\"{memberSignature}\", \"{typeName}\", \"{assemblyName}\")] on {provider}, got no attributes.";
				} else {
					msg = $"Expected [DynamicDependency (\"{memberSignature}\", \"{typeName}\", \"{assemblyName}\")] on {provider}, got:\n\t{string.Join ("\n\t", attributesAsString)}";
				}
			} else {
				msg = $"Expected exactly one [DynamicDependency (\"{memberSignature}\", \"{typeName}\", \"{assemblyName}\")] on {provider}, got {found}:\n\t{string.Join ("\n\t", attributesAsString)}";
			}
			Console.WriteLine (msg);
			Assert.Fail (msg);
		}

		void AssertHasDynamicDependencies (ICustomAttributeProvider provider)
		{
			AssertHasDynamicDependency (provider, "GetConstant(CoreAnimation.CAToneMapMode)", "CoreAnimation.CAToneMapModeExtensions", $"Microsoft.{platform.AsString ()}");
			AssertHasDynamicDependency (provider, "GetValue(Foundation.NSString)", "CoreAnimation.CAToneMapModeExtensions", $"Microsoft.{platform.AsString ()}");
		}

		Assert.Multiple (() => {
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "get_RWProperty"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "set_RWProperty"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "get_ROProperty"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "set_WOProperty"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "Method1"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "Method2"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "Method3"));
			AssertHasDynamicDependencies (type.Methods.Single (v => v.Name == "Method4"));
		});
	}
}
