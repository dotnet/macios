// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MonoTouch.Tuner;
using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class RegistrarTests : BaseClass {
	[TestCase (ApplePlatform.iOS)]
	[TestCase (ApplePlatform.MacCatalyst)]
	public void TrimmedModelProtocolConformance (ApplePlatform platform)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;
		using UIKit;

		[Protocol (Name = ""MyProtocol"")]
		public interface IMyProtocol {
		}

		[Protocol (Name = ""MyTrimmedProtocol"", WrapperType = typeof (MyTrimmedProtocolWrapper))]
		public interface IMyTrimmedProtocol : INativeObject, IDisposable {
		}

		internal sealed class MyTrimmedProtocolWrapper : BaseWrapper, IMyTrimmedProtocol {
			public MyTrimmedProtocolWrapper (IntPtr handle, bool owns) : base (handle, owns) { }
		}

		[Protocol (Name = ""MyRetainedProtocol"", WrapperType = typeof (MyRetainedProtocolWrapper))]
		public interface IMyRetainedProtocol : INativeObject, IDisposable {
		}

		internal sealed class MyRetainedProtocolWrapper : BaseWrapper, IMyRetainedProtocol {
			public MyRetainedProtocolWrapper (IntPtr handle, bool owns) : base (handle, owns) { }
		}

		[ObjectiveCFramework (""TestFramework"")]
		[Protocol (Name = ""MyExternalProtocol"", WrapperType = typeof (MyExternalProtocolWrapper))]
		public interface IMyExternalProtocol : INativeObject, IDisposable {
		}

		internal sealed class MyExternalProtocolWrapper : BaseWrapper, IMyExternalProtocol {
			public MyExternalProtocolWrapper (IntPtr handle, bool owns) : base (handle, owns) { }
		}

		public class MyClass : UIWindowSceneDelegate, IMyProtocol, IMyTrimmedProtocol, IMyRetainedProtocol, IMyExternalProtocol {
		}
		";
		var tempDir = Xamarin.Cache.CreateTemporaryDirectory ();
		var config = $@"
		AssemblyName=Microsoft.{platform.AsString ()}.dll
		CacheDirectory={tempDir}
		TypeMapAssemblyName=_TypeMap
		TypeMapOutputDirectory={Path.Combine (tempDir, "typemaps")}
		UnmanagedCallersOnlyMapPath={Path.Combine (tempDir, "uco.txt")}
		";
		using var preparer = CreatePreparer (platform, p => p.Registrar = RegistrarMode.TrimmableStatic,
			code, out _, extraConfig: config, hotReloadCompatibleBuild: true);
		AssertPrepare (preparer);

		var postDir = Path.Combine (tempDir, "post");
		var infos = preparer.Assemblies.Select (v => new AssemblyPreparerInfo (v.OutputPath, Path.Combine (postDir, Path.GetFileName (v.OutputPath)), v.OriginalInputPath, v.IsTrimmable, v.TrimMode))
			.Concat (preparer.AddedAssemblies.Select (v => new AssemblyPreparerInfo (v.Path, Path.Combine (postDir, Path.GetFileName (v.Path)), true, "link")))
			.ToArray ();
		var configPath = Path.Combine (Path.GetFullPath (Path.Combine (preparer.IntermediateOutputPath, "..")), "config.txt");
		var logger = new TestLogger { Platform = platform };
		using var postprocessor = new AssemblyPreparer (logger, infos, configPath) { Registrar = RegistrarMode.TrimmableStatic };
		postprocessor.Configuration.Application.IsPostProcessingAssemblies = true;
		var resolver = new DefaultAssemblyResolver ();
		foreach (var directory in preparer.Assemblies.Select (v => Path.GetDirectoryName (v.InputPath) ?? throw new InvalidOperationException (v.InputPath)).Distinct ())
			resolver.AddSearchDirectory (directory);
		postprocessor.Configuration.Application.PreTrimAssemblyResolver = resolver;

		var context = postprocessor.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		var platformAssembly = context.GetAssemblies ().Single (v => v.Name.Name == $"Microsoft.{platform.AsString ()}");
		platformAssembly.MainModule.GetType ("UIKit.UIWindowSceneDelegate").Interfaces.Clear ();
		var userAssembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		userAssembly.MainModule.GetType ("MyClass").Interfaces.Clear ();
		userAssembly.MainModule.Types.Remove (userAssembly.MainModule.GetType ("IMyProtocol"));
		userAssembly.MainModule.Types.Remove (userAssembly.MainModule.GetType ("IMyTrimmedProtocol"));
		userAssembly.MainModule.Types.Remove (userAssembly.MainModule.GetType ("IMyExternalProtocol"));
		new ManagedRegistrarStep ().Process (context);
		new RegistrarStep ().Process (context);

		Assert.That (logger.Errors, Is.Empty, "Postprocessing errors");
		var header = File.ReadAllLines (Path.Combine (tempDir, "registrar.h"));
		var declaration = header.Single (v => v.StartsWith ("@interface MyClass :"));
		Assert.That (declaration, Does.Contain ("UIWindowSceneDelegate"), "Native protocol conformance");
		Assert.That (declaration, Does.Not.Contain ("MyProtocol"), "Trimmed managed protocol conformance");
		Assert.That (declaration, Does.Not.Contain ("MyTrimmedProtocol"), "Trimmed protocol with wrapper");
		Assert.That (declaration, Does.Contain ("MyRetainedProtocol"), "Retained protocol with trimmed conformance");
		Assert.That (declaration, Does.Contain ("MyExternalProtocol"), "External framework protocol");
		Assert.That (header, Has.Some.StartsWith ("@protocol MyRetainedProtocol"), "Retained protocol declaration");
	}

	[Test]
	public void ModelClassAsGenericArgument ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""sessionOptions"")]
			public NSDictionary<NSString, NSFileManagerDelegate> GetSessionOptions ()
			{
				throw new System.NotImplementedException ();
			}
		}
		";

		using var preparer = CreatePreparer (ApplePlatform.iOS, p => p.Registrar = RegistrarMode.ManagedStatic, code, out _);
		preparer.Prepare (out _);

		var logger = (TestLogger) preparer.Configuration.Logger;
		var exception = logger.Errors.Single (v => v.Code == 4192);
		Assert.That (exception.Message, Is.EqualTo ("The registrar cannot use the model class 'Foundation.NSFileManagerDelegate' as a generic type argument in the generic type 'Foundation.NSDictionary`2<Foundation.NSString,Foundation.NSFileManagerDelegate>'. Use the protocol interface instead of the model class."));
	}

	[Test]
	public void ProtocolInterfaceAsGenericArgument ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""sessionOptions"")]
			public NSDictionary<NSString, INSFileManagerDelegate> GetSessionOptions ()
			{
				throw new System.NotImplementedException ();
			}
		}
		";

		AssertPrepare (ApplePlatform.iOS, RegistrarMode.ManagedStatic, code, out _);
	}
}
