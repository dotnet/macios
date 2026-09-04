// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;

using Mono.Cecil;
using Mono.Cecil.Cil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class HotReloadCompatibleTest : TestBaseClass {
		[TestCase ("Debug", "true", 1)]
		[TestCase ("Release", "false", 0)]
		public void FeatureSwitch (string configuration, string expectedValue, int expectedConstructorCalls)
		{
			const ApplePlatform platform = ApplePlatform.MacCatalyst;
			const string runtimeIdentifier = "maccatalyst-arm64";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GetProjectPath ("EnsureUIThreadApp", runtimeIdentifiers: runtimeIdentifier, platform: platform, out var appPath, configuration: configuration);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Configuration"] = configuration;
			properties ["MtouchLink"] = "SdkOnly";
			properties ["LinkMode"] = "SdkOnly";
			properties ["Registrar"] = "trimmable-static";

			var result = DotNet.AssertBuild (projectPath, properties);

			var featureSwitch = GetRuntimeHostConfigurationOption (result.BinLogPath, "ObjCRuntime.Runtime.HotReloadCompatible");
			Assert.That (featureSwitch, Is.Not.Null, "The HotReloadCompatible feature switch must be set.");
			Assert.That (featureSwitch?.GetMetadata ("Value"), Is.EqualTo (expectedValue), "Feature switch value");

			var platformAssembly = Path.Combine (appPath, GetRelativeAssemblyDirectory (platform), Configuration.GetBaseLibraryName (platform));
			using var assembly = AssemblyDefinition.ReadAssembly (platformAssembly, new ReaderParameters { ReadingMode = ReadingMode.Immediate });
			var runtimeType = assembly.MainModule.Types.Single (v => v.FullName == "ObjCRuntime.Runtime");
			Assert.That (CountCalls (runtimeType, "ConstructNSObject", 5, "GetIntPtrConstructor"), Is.EqualTo (expectedConstructorCalls), "NSObject constructor calls");
			Assert.That (CountCalls (runtimeType, "ConstructINativeObject", 7, "GetIntPtr_BoolConstructor"), Is.EqualTo (expectedConstructorCalls), "INativeObject constructor calls");
		}

		static int CountCalls (TypeDefinition type, string callerName, int parameterCount, string calledMethodName)
		{
			var caller = type.Methods.Single (method => method.Name == callerName && method.HasGenericParameters && method.Parameters.Count == parameterCount);
			return caller.Body.Instructions.Count (instruction => instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference method && method.Name == calledMethodName);
		}

		static ITaskItem? GetRuntimeHostConfigurationOption (string binLogPath, string name)
		{
			ITaskItem? rv = null;
			foreach (var args in BinLog.ReadBuildEvents (binLogPath)) {
				if (args is not TaskParameterEventArgs tpea || tpea.Kind != TaskParameterMessageKind.AddItem || tpea.ItemType != "RuntimeHostConfigurationOption")
					continue;
				foreach (var item in tpea.Items) {
					if (item is ITaskItem taskItem && taskItem.ItemSpec == name)
						rv = taskItem;
				}
			}
			return rv;
		}
	}
}
