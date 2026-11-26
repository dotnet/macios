using Mono.Cecil;

namespace Xamarin.Tests {
	[TestFixture]
	public class RegistrarTest : TestBaseClass {
		// The 'Registrar' property is computed inside the 'SelectRegistrar' target (not at evaluation
		// time), so its value is only recorded in the binlog when MSBuild's property tracking is
		// enabled. This isn't the case by default (in particular not on CI), so explicitly enable it
		// (just for the child 'dotnet build' process) for the tests in this file that need to look up
		// the resulting value of $(Registrar) in the binlog.
		static readonly Dictionary<string, string?> EnablePropertyTracking = new Dictionary<string, string?> {
			{ "MSBuildLogPropertyTracking", "1" },
		};

		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void DynamicRetainCountExport (ApplePlatform platform, string runtimeIdentifier)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GenerateProject (platform, nameof (DynamicRetainCountExport), runtimeIdentifier, out _);
			var projectDirectory = Path.GetDirectoryName (projectPath) ?? throw new InvalidOperationException ("The generated project path has no directory.");
			File.WriteAllText (Path.Combine (projectDirectory, "Main.cs"), """
				// Copyright (c) Microsoft Corporation.
				// Licensed under the MIT License.

				using System;
				using System.Linq;
				using System.Reflection;
				using System.Reflection.Emit;
				using System.Runtime.InteropServices;
				using Foundation;
				using ObjCRuntime;

				class Program {
					[DllImport ("/usr/lib/libobjc.dylib")]
					static extern IntPtr objc_allocateClassPair (IntPtr superclass, string name, IntPtr extraBytes);

					[DllImport ("/usr/lib/libobjc.dylib")]
					static extern void objc_registerClassPair (IntPtr cls);

					[DllImport ("/usr/lib/libobjc.dylib")]
					static extern IntPtr class_getInstanceMethod (IntPtr cls, IntPtr selector);

					[DllImport ("/usr/lib/libobjc.dylib")]
					static extern IntPtr method_getImplementation (IntPtr method);

					static void AssertRejected (Type type)
					{
						for (var attempt = 0; attempt < 2; attempt++) {
							try {
								Class.GetHandle (type);
								throw new InvalidOperationException ($"Registration of {type.Name} succeeded on attempt {attempt}.");
							} catch (AggregateException ex) when (ex.Flatten ().InnerExceptions.Any (v => v is RuntimeException error && error.Code == 4199)) {
							}
							if (Class.GetHandle (type.Name) != NativeHandle.Zero)
								throw new InvalidOperationException ($"The rejected native class {type.Name} was registered.");
						}
					}

					static void Main ()
					{
						var assembly = AssemblyBuilder.DefineDynamicAssembly (new AssemblyName ("RetainCountExports"), AssemblyBuilderAccess.Run);
						var module = assembly.DefineDynamicModule ("RetainCountExports");
						var builder = module.DefineType ("RejectedRetainCount", TypeAttributes.Public, typeof (NSObject));
						var method = builder.DefineMethod ("GetCount", MethodAttributes.Public, typeof (nuint), []);
						var exportConstructor = typeof (ExportAttribute).GetConstructor ([typeof (string)]) ?? throw new InvalidOperationException ("Missing ExportAttribute constructor.");
						method.SetCustomAttribute (new CustomAttributeBuilder (exportConstructor, ["retainCount"]));
						var il = method.GetILGenerator ();
						il.Emit (OpCodes.Ldc_I4_0);
						il.Emit (OpCodes.Conv_U);
						il.Emit (OpCodes.Ret);
						var type = builder.CreateType () ?? throw new InvalidOperationException ("Could not create the test type.");
						AssertRejected (type);
						var derived = module.DefineType ("RejectedDerivedRetainCount", TypeAttributes.Public, type).CreateType () ?? throw new InvalidOperationException ("Could not create the derived test type.");
						AssertRejected (derived);

						var cls = objc_allocateClassPair (Class.GetHandle (typeof (NSObject)), "RetainCountWrapper", IntPtr.Zero);
						if (cls == IntPtr.Zero)
							throw new InvalidOperationException ("Could not allocate the native wrapper class.");
						objc_registerClassPair (cls);
						var selector = Selector.GetHandle ("retainCount");
						var originalImplementation = method_getImplementation (class_getInstanceMethod (cls, selector));
						var wrapper = module.DefineType ("RetainCountWrapper", TypeAttributes.Public, typeof (NSObject));
						var registerConstructor = typeof (RegisterAttribute).GetConstructor ([typeof (string), typeof (bool)]) ?? throw new InvalidOperationException ("Missing RegisterAttribute constructor.");
						wrapper.SetCustomAttribute (new CustomAttributeBuilder (registerConstructor, ["RetainCountWrapper", true]));
						var wrapperMethod = wrapper.DefineMethod ("GetCount", MethodAttributes.Public, typeof (nuint), []);
						il = wrapperMethod.GetILGenerator ();
						il.Emit (OpCodes.Ldc_I4_0);
						il.Emit (OpCodes.Conv_U);
						il.Emit (OpCodes.Ret);
						var wrapperType = wrapper.CreateType () ?? throw new InvalidOperationException ("Could not create the wrapper test type.");
						var categoryClass = Class.GetHandle (typeof (NSFormatter));
						var categoryImplementation = method_getImplementation (class_getInstanceMethod (categoryClass, selector));
						var category = module.DefineType ("RejectedRetainCountCategory", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
						var categoryConstructor = typeof (CategoryAttribute).GetConstructor ([typeof (Type)]) ?? throw new InvalidOperationException ("Missing CategoryAttribute constructor.");
						category.SetCustomAttribute (new CustomAttributeBuilder (categoryConstructor, [typeof (NSFormatter)]));
						var extensionConstructor = typeof (System.Runtime.CompilerServices.ExtensionAttribute).GetConstructor ([]) ?? throw new InvalidOperationException ("Missing ExtensionAttribute constructor.");
						var validSelector = Selector.GetHandle ("retainCountCategoryRegression");
						if (class_getInstanceMethod (categoryClass, validSelector) != IntPtr.Zero)
							throw new InvalidOperationException ("The category test selector already exists.");
						var validMethod = category.DefineMethod ("ValidExport", MethodAttributes.Public | MethodAttributes.Static, typeof (nuint), [typeof (NSFormatter)]);
						validMethod.SetCustomAttribute (new CustomAttributeBuilder (exportConstructor, ["retainCountCategoryRegression"]));
						validMethod.SetCustomAttribute (new CustomAttributeBuilder (extensionConstructor, []));
						il = validMethod.GetILGenerator ();
						il.Emit (OpCodes.Ldc_I4_0);
						il.Emit (OpCodes.Conv_U);
						il.Emit (OpCodes.Ret);
						var categoryMethod = category.DefineMethod ("GetCount", MethodAttributes.Public | MethodAttributes.Static, typeof (nuint), [typeof (NSFormatter)]);
						categoryMethod.SetCustomAttribute (new CustomAttributeBuilder (exportConstructor, ["retainCount"]));
						categoryMethod.SetCustomAttribute (new CustomAttributeBuilder (extensionConstructor, []));
						il = categoryMethod.GetILGenerator ();
						il.Emit (OpCodes.Ldc_I4_0);
						il.Emit (OpCodes.Conv_U);
						il.Emit (OpCodes.Ret);
						AssertRejected (category.CreateType () ?? throw new InvalidOperationException ("Could not create the category test type."));
						if (method_getImplementation (class_getInstanceMethod (categoryClass, selector)) != categoryImplementation)
							throw new InvalidOperationException ("The category replaced the native retainCount implementation.");
						if (class_getInstanceMethod (categoryClass, validSelector) != IntPtr.Zero)
							throw new InvalidOperationException ("The rejected category installed its valid export.");

						var validCategory = module.DefineType ("ValidRetainCountCategory", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
						validCategory.SetCustomAttribute (new CustomAttributeBuilder (categoryConstructor, [typeof (NSFormatter)]));
						validMethod = validCategory.DefineMethod ("ValidExport", MethodAttributes.Public | MethodAttributes.Static, typeof (nuint), [typeof (NSFormatter)]);
						validMethod.SetCustomAttribute (new CustomAttributeBuilder (exportConstructor, ["retainCountCategoryRegression"]));
						validMethod.SetCustomAttribute (new CustomAttributeBuilder (extensionConstructor, []));
						il = validMethod.GetILGenerator ();
						il.Emit (OpCodes.Ldc_I4_0);
						il.Emit (OpCodes.Conv_U);
						il.Emit (OpCodes.Ret);
						Class.GetHandle (validCategory.CreateType () ?? throw new InvalidOperationException ("Could not create the valid category test type."));
						if (class_getInstanceMethod (categoryClass, validSelector) == IntPtr.Zero)
							throw new InvalidOperationException ("The valid category export was not installed.");

						var callback = wrapperType.GetMethod ("GetCount") ?? throw new InvalidOperationException ("Missing callback.");
						for (var attempt = 0; attempt < 2; attempt++) {
							try {
								Runtime.ConnectMethod (wrapperType, callback, new Selector ("retainCount"));
								throw new InvalidOperationException ("Connecting retainCount on a wrapper succeeded.");
							} catch (RuntimeException ex) when (ex.Code == 4199) {
							}
							if (method_getImplementation (class_getInstanceMethod (cls, selector)) != originalImplementation)
								throw new InvalidOperationException ("The native retainCount implementation was replaced.");
						}
						Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
					}
				}
				""");
			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["Registrar"] = "dynamic";
			properties ["MtouchLink"] = "None";
			properties ["UseMonoRuntime"] = "false";
			DotNet.AssertBuild (projectPath, properties);
			ExecuteProjectWithMagicWordAndAssert (projectPath, platform, runtimeIdentifier);
		}

		[TestCase (ApplePlatform.iOS, "None", "partial-static")]
		[TestCase (ApplePlatform.iOS, "SdkOnly", "trimmable-static")]
		public void DefaultCoreCLRSimulatorRegistrar (ApplePlatform platform, string linkMode, string expectedRegistrar)
		{
			var runtimeIdentifiers = "iossimulator-arm64";

			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = linkMode;
			properties ["UseMonoRuntime"] = "false";

			var result = DotNet.AssertBuild (projectPath, properties, environmentVariables: EnablePropertyTracking);

			Assert.That (BinLog.TryFindPropertyValue (result.BinLogPath, "Registrar", out var registrar), Is.True, "Could not find the 'Registrar' property in the binlog.");
			Assert.That (registrar, Is.EqualTo (expectedRegistrar), "Registrar");
		}

		[TestCase (ApplePlatform.iOS)]
		public void ChangeDefaultCoreCLRSimulatorRegistrar (ApplePlatform platform)
		{
			var runtimeIdentifiers = "iossimulator-arm64";

			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath ("MySimpleApp", platform: platform);
			Clean (projectPath);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["MtouchLink"] = "None";
			properties ["UseMonoRuntime"] = "false";
			properties ["Registrar"] = "trimmable-static";
			DotNet.AssertBuild (projectPath, properties);

			properties.Remove ("Registrar");
			var result = DotNet.AssertBuild (projectPath, properties, environmentVariables: EnablePropertyTracking);

			Assert.That (BinLog.TryFindPropertyValue (result.BinLogPath, "Registrar", out var registrar), Is.True, "Could not find the 'Registrar' property in the binlog.");
			Assert.That (registrar, Is.EqualTo ("partial-static"), "Registrar");

			var targets = BinLog.GetAllTargets (result.BinLogPath);
			Assert.That (targets.Any (v => v.TargetName == "_ExecutePrepareAssemblies" && !v.Skipped), Is.True, "_ExecutePrepareAssemblies should execute when the registrar changes.");

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifiers);
			var registrarItemsPath = Path.Combine (objDir, "linker-items", "_RegistrarFile.items");
			Assert.That (registrarItemsPath, Does.Exist, "_RegistrarFile.items");
			Assert.That (File.ReadAllText (registrarItemsPath), Does.Not.Contain ("<_RegistrarFile Include="), "_RegistrarFile.items");
		}

		// This test does evil things that the AOT runtime complains about, so it only works when not running the AOT compiler (aka x64 when using Mono).
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		[TestCase (ApplePlatform.MacOSX, "osx-x64")]
		public void InvalidStaticRegistrarValidation (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MyRegistrarApp";
			var configuration = "Debug";

			Configuration.IgnoreIfIgnoredPlatform (platform);

			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var projectPath = GetProjectPath (project, platform: platform);
			Clean (projectPath);
			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["Registrar"] = "static";
			// enable the linker (so that the main assembly is modified)
			properties ["LinkMode"] = "full";
			properties ["MtouchLink"] = "full";

			DotNet.AssertBuild (projectPath, properties);

			var appDir = GetAppPath (projectPath, platform, runtimeIdentifiers, configuration);
			var asmDir = Path.Combine (appDir, GetRelativeAssemblyDirectory (platform));
			var appExecutable = Path.Combine (asmDir, project + ".dll");

			// Save the first version of the main assembly in memory
			var firstAssembly = File.ReadAllBytes (appExecutable);

			// Build again, including additional code
			properties ["AdditionalDefineConstants"] = "INCLUDED_ADDITIONAL_CODE";
			DotNet.AssertBuild (projectPath, properties);

			// Revert to the original version of the main assembly
			File.WriteAllBytes (appExecutable, firstAssembly);

			Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", "1");
			try {
				ExecuteProjectWithMagicWordAndAssert (projectPath, platform, runtimeIdentifiers);
			} finally {
				Environment.SetEnvironmentVariable ("XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", null);
			}
		}

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "26.0", true, true)]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64", "27.0", false, false)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", "26.0", false, true)]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64", "27.0", false, false)]
		public void UnavailablePlatformModelRegistrarOutput (ApplePlatform platform, string runtimeIdentifier, string deploymentTarget, bool expectNativeDeclaration, bool expectStaticProtocolLookup)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GenerateProject (platform, nameof (UnavailablePlatformModelRegistrarOutput), runtimeIdentifier, out _);
			var projectDir = Path.GetDirectoryName (projectPath)!;
			File.WriteAllText (Path.Combine (projectDir, "Main.cs"), """
				using System;
				using ObjCRuntime;
				using UIKit;

				class MainClass {
					static int Main ()
					{
				#pragma warning disable 618, CA1422
						GC.KeepAlive (typeof (UIAccelerometerDelegate));
						GC.KeepAlive (new Protocol (typeof (IUIAccelerometerDelegate)));
				#pragma warning restore 618, CA1422
						return 0;
					}
				}
				""");

			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["UseMonoRuntime"] = "false";
			properties ["Registrar"] = "static";
			properties ["SupportedOSPlatformVersion"] = deploymentTarget;
			DotNet.AssertBuild (projectPath, properties);

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifier);
			var registrarHeader = Directory.GetFiles (objDir, "registrar.h", SearchOption.AllDirectories).Single ();
			var registrarImplementation = Directory.GetFiles (objDir, "registrar.mm", SearchOption.AllDirectories).Single ();
			var headerContents = File.ReadAllText (registrarHeader);
			var implementationContents = File.ReadAllText (registrarImplementation);
			const string modelDeclaration = "UIAccelerometerDelegate : NSObject<UIAccelerometerDelegate>";

			if (expectNativeDeclaration) {
				Assert.That (headerContents, Does.Contain (modelDeclaration), "Native model declaration");
			} else {
				Assert.That (headerContents, Does.Not.Contain (modelDeclaration), "Native model declaration");
			}

			if (expectStaticProtocolLookup) {
				Assert.That (implementationContents, Does.Contain ("@protocol (UIAccelerometerDelegate)"), "Static protocol lookup");
			} else {
				Assert.That (implementationContents, Does.Not.Contain ("@protocol (UIAccelerometerDelegate)"), "Static protocol lookup");
				Assert.That (implementationContents, Does.Contain ("objc_getProtocol (\"UIAccelerometerDelegate\")"), "Dynamic protocol lookup");
			}
		}

		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void IncrementalTrimmableStaticRegistrar (ApplePlatform platform, string runtimeIdentifier)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifier);

			var projectPath = GenerateProject (platform, nameof (IncrementalTrimmableStaticRegistrar), runtimeIdentifier, out var appPath);
			var projectName = Path.GetFileNameWithoutExtension (projectPath);
			var sourcePath = Path.Combine (Path.GetDirectoryName (projectPath)!, "Main.cs");
			var source = """
				// Copyright (c) Microsoft Corporation.
				// Licensed under the MIT License.

				using System;
				using Foundation;
				using ObjCRuntime;

				[Register ("IncrementalRegistrarClass")]
				public class IncrementalRegistrarClass : NSObject {
					[Export ("value")]
					public string Value () => "VERSION";
					EXTRA_EXPORT
				}

				class Program {
					static void Main ()
					{
						using var instance = new IncrementalRegistrarClass ();
						var registeredClass = new Class (typeof (IncrementalRegistrarClass));
						if (registeredClass.Handle == NativeHandle.Zero || Class.Lookup (registeredClass) != typeof (IncrementalRegistrarClass) || instance.Value () != "VERSION")
							throw new InvalidOperationException ("The native class was not registered correctly.");
						Console.WriteLine (Environment.GetEnvironmentVariable ("MAGIC_WORD"));
					}
				}
				""";
			var properties = GetDefaultProperties (runtimeIdentifier);
			properties ["UseMonoRuntime"] = "false";
			properties ["Registrar"] = "trimmable-static";
			properties ["MtouchLink"] = "SdkOnly";

			var objDir = GetObjDir (projectPath, platform, runtimeIdentifier);
			var nativeExecutable = GetNativeExecutable (platform, appPath!);
			var appAssembly = Path.Combine (appPath!, GetRelativeAssemblyDirectory (platform), projectName + ".dll");
			var validationEnvironment = new Dictionary<string, string?> {
				{ "XAMARIN_VALIDATE_STATIC_REGISTRAR_CODE", "1" },
			};
			string? registrarContents = null;
			Guid previousMvid = Guid.Empty;
			DateTime registrarObjectTimestamp = default;
			DateTime assembliesObjectTimestamp = default;

			for (var version = 1; version <= 3; version++) {
				File.WriteAllText (sourcePath, source.Replace ("VERSION", version.ToString ()).Replace ("EXTRA_EXPORT", ""));
				var result = DotNet.AssertBuild (projectPath, properties);

				var registrarSource = Directory.GetFiles (objDir, "registrar.mm", SearchOption.AllDirectories).Single ();
				var assembliesObject = Directory.GetFiles (objDir, "registrar-assemblies.o", SearchOption.AllDirectories).Single ();
				var registrarObject = Directory.GetFiles (objDir, "registrar.o", SearchOption.AllDirectories).Single ();
				using var module = ModuleDefinition.ReadModule (appAssembly);
				var mvid = module.Mvid;
				Assert.That (Directory.GetFiles (objDir, "registrar-assemblies.mm", SearchOption.AllDirectories), Is.Empty, $"Version {version}: no assembly source to compile");
				Assert.That (File.ReadAllBytes (assembliesObject).AsSpan ().IndexOf (System.Text.Encoding.ASCII.GetBytes (mvid.ToString ())), Is.GreaterThanOrEqualTo (0), $"Version {version}: generated assembly object MVID");
				Assert.That (File.ReadAllText (registrarSource), Does.Not.Contain (mvid.ToString ()), $"Version {version}: stable registrar source");
				Assert.That (File.ReadAllBytes (nativeExecutable).AsSpan ().IndexOf (System.Text.Encoding.ASCII.GetBytes (mvid.ToString ())), Is.GreaterThanOrEqualTo (0), $"Version {version}: linked native MVID");
				ExecuteWithMagicWordAndAssert (platform, runtimeIdentifier, nativeExecutable, validationEnvironment);

				if (version == 1) {
					registrarContents = File.ReadAllText (registrarSource);
				} else {
					Assert.That (mvid, Is.Not.EqualTo (previousMvid), $"Version {version}: assembly MVID changed");
					Assert.That (File.ReadAllText (registrarSource), Is.EqualTo (registrarContents), $"Version {version}: registrar contents");
					Assert.That (File.GetLastWriteTimeUtc (registrarObject), Is.EqualTo (registrarObjectTimestamp), $"Version {version}: registrar object reused");
					Assert.That (File.GetLastWriteTimeUtc (assembliesObject), Is.GreaterThan (assembliesObjectTimestamp), $"Version {version}: assembly object regenerated");
					var targets = BinLog.GetAllTargets (result.BinLogPath);
					AssertTargetNotExecuted (targets, "_CompileNativeExecutable", $"Version {version}: no native compilation");
					AssertTargetExecuted (targets, "_LinkNativeExecutable", $"Version {version}: native relink");
				}

				previousMvid = mvid;
				registrarObjectTimestamp = File.GetLastWriteTimeUtc (registrarObject);
				assembliesObjectTimestamp = File.GetLastWriteTimeUtc (assembliesObject);
			}

			var unchanged = DotNet.AssertBuild (projectPath, properties);
			var currentAssembliesObject = Directory.GetFiles (objDir, "registrar-assemblies.o", SearchOption.AllDirectories).Single ();
			Assert.That (File.GetLastWriteTimeUtc (currentAssembliesObject), Is.EqualTo (assembliesObjectTimestamp), "Unchanged assembly object");
			AssertTargetNotExecuted (BinLog.GetAllTargets (unchanged.BinLogPath), "_CompileNativeExecutable", "Unchanged native sources");

			File.WriteAllText (sourcePath, source.Replace ("VERSION", "4").Replace ("EXTRA_EXPORT", "[Export (\"extraValue\")]\n\tpublic string ExtraValue () => \"extra\";"));
			DotNet.AssertBuild (projectPath, properties);
			var updatedRegistrarSource = Directory.GetFiles (objDir, "registrar.mm", SearchOption.AllDirectories).Single ();
			var updatedRegistrarObject = Directory.GetFiles (objDir, "registrar.o", SearchOption.AllDirectories).Single ();
			Assert.That (File.ReadAllText (updatedRegistrarSource), Does.Contain ("extraValue"), "New registered method");
			Assert.That (File.GetLastWriteTimeUtc (updatedRegistrarObject), Is.GreaterThan (registrarObjectTimestamp), "Registration change recompiles registrar");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifier, nativeExecutable, validationEnvironment);

			DotNet.Execute ("clean", projectPath, properties);
			Assert.That (File.Exists (currentAssembliesObject), Is.False, "Clean removes generated assembly object");
		}

		IEnumerable<TypeDefinition> AllTypes (ModuleDefinition module)
		{
			foreach (var type in module.Types) {
				yield return type;
				foreach (var t in InnerTypes (type))
					yield return t;
			}
		}

		IEnumerable<TypeDefinition> InnerTypes (TypeDefinition type)
		{
			if (type.HasNestedTypes) {
				foreach (var t in type.NestedTypes) {
					yield return t;
					foreach (var nt in InnerTypes (t))
						yield return nt;
				}
			}
		}
	}
}
