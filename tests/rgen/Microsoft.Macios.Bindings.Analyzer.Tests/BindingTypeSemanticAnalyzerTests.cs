// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Bindings.Analyzer.Validators;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class BindingTypeSemanticAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {
	class TestDataBindingTypeAnalyzerTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string nonPartialClassBinding = @"
using ObjCBindings;

namespace Test {
	[BindingType<Class>]
	public class Examples {
	}
}
";
			yield return [
				nonPartialClassBinding,
				"RBI0001",
				"The binding type 'Test.Examples' must be declared partial"
			];

			const string classBindingNotClas = @"
using ObjCBindings;

namespace Test {
	[BindingType<Class>]
	public interface Examples {
	}
}
";
			yield return [
				classBindingNotClas,
				"RBI0002",
				"BindingType<Class> can only be used to decorate a class but was found on 'Test.Examples' which is not a class"
			];

			const string nonPartialCategory = @"
using ObjCBindings;

namespace Test {
	[BindingType<Category>]
	public class Examples {
	}
}
";
			yield return [
				nonPartialCategory,
				"RBI0001",
				"The binding type 'Test.Examples' must be declared partial"
			];

			const string nonClassCategory = @"
using ObjCBindings;

namespace Test {
	[BindingType<Category>]
	public interface Examples {
	}
}
";
			yield return [
				nonClassCategory,
				"RBI0003",
				"BindingType<Category> can only be used to decorate a class but was found on 'Test.Examples' which is not a class"
			];

			const string nonStaticCategory = @"
using ObjCBindings;

namespace Test {
	[BindingType<Category>]
	public partial class Examples {
	}
}
";
			yield return [
				nonStaticCategory,
				"RBI0004",
				"BindingType<Category> can only be used to decorate a static class but was found on 'Test.Examples' which is not static"
			];

			const string nonPartialProtocol = @"
using ObjCBindings;

namespace Test {
	[BindingType<Protocol>]
	public interface Examples {
	}
}
";
			yield return [
				nonPartialProtocol,
				"RBI0001",
				"The binding type 'Test.Examples' must be declared partial"
			];

			const string nonInterfaceProtocol = @"
using ObjCBindings;

namespace Test {
	[BindingType<Protocol>]
	public class Examples {
	}
}
";
			yield return [
				nonInterfaceProtocol,
				"RBI0005",
				"BindingType<Protocol> can only be used to decorate an interface but was found on 'Test.Examples' which is not an interface"
			];

			const string nonSmartEnum = @"
using ObjCBindings;

namespace Test {
	[BindingType<SmartEnum>]
	public class Examples {
	}
}
";
			yield return [
				nonSmartEnum,
				"RBI0006",
				"BindingType can only be used to decorate an enumerator but was found on 'Test.Examples' which is not an enumerator"
			];

			const string nonPartialStrongDictionary = @"
using ObjCBindings;

namespace Test {
	[BindingType<StrongDictionary>]
	public class Examples {
	}
}
";
			yield return [
				nonPartialStrongDictionary,
				"RBI0001",
				"The binding type 'Test.Examples' must be declared partial"
			];

			const string nonClassStrongDictionary = @"
using ObjCBindings;

namespace Test {
	[BindingType<StrongDictionary>]
	public interface Examples {
	}
}
";
			yield return [
				nonClassStrongDictionary,
				"RBI0007",
				"BindingType<StrongDictionary> can only be used to decorate a class but was found on 'Test.Examples' which is not a class"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataBindingTypeAnalyzerTests>]
	public async Task BindingTypeAnalyzerTests (ApplePlatform platform, string inputText, string diagnosticId,
		string diagnosticMessage)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new BindingTypeSemanticAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics
			.Where (d => d.Id == diagnosticId).ToArray ();
		Assert.Single (analyzerDiagnotics);
		// verify the diagnostic message
		VerifyDiagnosticMessage (analyzerDiagnotics [0], diagnosticId, DiagnosticSeverity.Error, diagnosticMessage);
	}
}
