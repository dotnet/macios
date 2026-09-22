// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AssemblyPreparerTests;

public class RegistrarTests : BaseClass {
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

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.ManagedStatic, code, out _);
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

		AssertPrepare (ApplePlatform.iOS, false, RegistrarMode.ManagedStatic, code, out _);
	}
}
