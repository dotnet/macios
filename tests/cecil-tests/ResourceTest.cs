// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Cecil.Tests {
	[TestFixture]
	public class ResourceTest {
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void ToolResourceManagersHaveEmbeddedResources (ApplePlatform platform)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			var toolsDirectory = Configuration.GetSdkRoot (platform);
			Assert.That (Directory.Exists (toolsDirectory), Is.True, $"SDK tools directory does not exist: {toolsDirectory}");

			var resourceManagers = 0;
			Assert.Multiple (() => {
				foreach (var path in Directory.GetFiles (toolsDirectory, "*.dll", SearchOption.AllDirectories).OrderBy (v => v, StringComparer.Ordinal)) {
					using var assembly = AssemblyDefinition.ReadAssembly (path);
					foreach (var module in assembly.Modules) {
						foreach (var type in module.GetTypes ()) {
							if (!type.CustomAttributes.Any (v => v.AttributeType.FullName == "System.CodeDom.Compiler.GeneratedCodeAttribute"
								&& v.ConstructorArguments.Count > 0
								&& v.ConstructorArguments [0].Value is string generator
								&& generator.EndsWith (".StronglyTypedResourceBuilder", StringComparison.Ordinal)))
								continue;

							resourceManagers++;
							CheckResourceManager (path, module, type);
						}
					}
				}
				Assert.That (resourceManagers, Is.GreaterThan (0), $"No generated resource managers found in {toolsDirectory}");
			});
		}

		static void CheckResourceManager (string path, ModuleDefinition module, TypeDefinition type)
		{
			var context = $"{path}: {type.FullName}";
			var getter = type.Properties.SingleOrDefault (v => v.Name == "ResourceManager")?.GetMethod;
			if (getter is null || !getter.HasBody) {
				Assert.Fail ($"{context}: generated ResourceManager getter is missing.");
				return;
			}

			var instructions = getter.Body.Instructions;
			var constructors = instructions.Where (v => v.OpCode == OpCodes.Newobj
				&& v.Operand is MethodReference method
				&& method.DeclaringType.FullName == "System.Resources.ResourceManager"
				&& method.Parameters.Count == 2
				&& method.Parameters [0].ParameterType.FullName == "System.String"
				&& method.Parameters [1].ParameterType.FullName == "System.Reflection.Assembly").ToArray ();
			if (constructors.Length != 1) {
				Assert.Fail ($"{context}: expected one ResourceManager(string, Assembly) constructor call, found {constructors.Length}.");
				return;
			}

			var index = instructions.IndexOf (constructors [0]);
			var getTypeIndex = index - 2;
			if (getTypeIndex >= 0
				&& instructions [getTypeIndex].Operand is MethodReference getTypeInfo
				&& getTypeInfo.FullName == "System.Reflection.TypeInfo System.Reflection.IntrospectionExtensions::GetTypeInfo(System.Type)")
				getTypeIndex--;
			// Generated accessors use new ResourceManager ("base name", typeof (Resources).Assembly).
			if (getTypeIndex < 2
				|| instructions [getTypeIndex - 2].OpCode != OpCodes.Ldstr
				|| instructions [getTypeIndex - 1].OpCode != OpCodes.Ldtoken
				|| instructions [getTypeIndex - 1].Operand is not TypeReference resourceType
				|| resourceType.FullName != type.FullName
				|| resourceType.Scope != module
				|| instructions [getTypeIndex].Operand is not MethodReference getType
				|| getType.FullName != "System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)"
				|| instructions [index - 1].Operand is not MethodReference getAssembly
				|| getAssembly.FullName != "System.Reflection.Assembly System.Type::get_Assembly()") {
				Assert.Fail ($"{context}: unrecognized ResourceManager initialization; update this test to validate its resource name and owning assembly.");
				return;
			}

			var resourceName = (string) instructions [getTypeIndex - 2].Operand + ".resources";
			var embeddedResources = module.Resources.OfType<EmbeddedResource> ().Select (v => v.Name).ToArray ();
			Assert.That (embeddedResources, Does.Contain (resourceName), $"{context}: missing embedded resource '{resourceName}'.");
		}
	}
}
