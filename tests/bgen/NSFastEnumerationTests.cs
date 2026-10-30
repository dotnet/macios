// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Linq;

using Mono.Cecil;

using Xamarin.Tests;
using Xamarin.Utils;

namespace GeneratorTests {
	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class NSFastEnumerationTests : BGenBase {
		[TestCase (Profile.iOS)]
		[TestCase (Profile.tvOS)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.MacCatalyst)]
		public void Collections (Profile profile)
		{
			var bgen = BuildFile (profile, "nsfastenumeration.cs");
			var module = bgen.ApiAssembly.MainModule;
			foreach (var (name, elementType) in new [] {
				("UntypedCollection", "Foundation.NSObject"),
				("StringCollection", "Foundation.NSString"),
				("GenericCollection`1", "T"),
				("GenericKeyCollection`2", "TKey"),
				("ProtocolCollection", "Foundation.NSString"),
				("ProtocolElementCollection", "Foundation.INSCopying"),
				("TypedDerivedCollection", "Foundation.NSString"),
				("ExternalMaterialCollection", "ModelIO.MDLMaterialProperty"),
			}) {
				var type = module.GetType ("FastEnumeration", name);
				Assert.That (type.Interfaces.Select (v => v.InterfaceType.FullName), Does.Contain ($"System.Collections.Generic.IEnumerable`1<{elementType}>"), name);
				var method = type.Methods.Single (v => v.Name == "GetEnumerator");
				Assert.That (method.ReturnType.FullName, Is.EqualTo ($"System.Collections.Generic.IEnumerator`1<{elementType}>"), name);
				Assert.That (type.Methods.SelectMany (v => v.Overrides).Any (v => v.DeclaringType.FullName == "System.Collections.IEnumerable" && v.Name == "GetEnumerator"), Is.True, name);
			}
			var derived = module.GetType ("FastEnumeration", "DerivedStringCollection");
			Assert.That (derived.Methods.Any (v => v.Name == "GetEnumerator"), Is.False, "Use inherited enumerator");
			var repeated = module.GetType ("FastEnumeration", "RepeatedStringCollection");
			Assert.That (repeated.Methods.Any (v => v.Name == "GetEnumerator"), Is.False, "Repeated attribute uses inherited enumerator");
			var protocol = module.GetType ("FastEnumeration", "IStringCollectionProtocol");
			Assert.That (protocol.Interfaces.Select (v => v.InterfaceType.FullName), Does.Contain ("System.Collections.Generic.IEnumerable`1<Foundation.NSString>"), "Protocol interface");
			Assert.That (protocol.Methods.Count (v => v.Name.EndsWith ("GetEnumerator") && v.HasBody), Is.EqualTo (2), "Default implementations");

			var targetFramework = TargetFramework.Parse (BGenTool.GetTargetFramework (profile));
			using var platformAssembly = AssemblyDefinition.ReadAssembly (Configuration.GetBaseLibrary (targetFramework));
			var inputCollection = platformAssembly.MainModule.GetType ("GameController", "GCPhysicalInputElementCollection`2");
			Assert.That (inputCollection.Interfaces.Select (v => v.InterfaceType.FullName), Does.Contain ("System.Collections.Generic.IEnumerable`1<KeyIdentifierType>"), "GameController enumerates keys");
			Assert.That (inputCollection.Methods.Single (v => v.Name == "GetEnumerator").ReturnType.FullName, Is.EqualTo ("System.Collections.Generic.IEnumerator`1<KeyIdentifierType>"), "Generated GameController enumerator");
		}

		[TestCase ("typeof (int)", 1130)]
		[TestCase ("\"MissingParameter\"", 1130)]
		[TestCase ("typeof (NSString)", 1131, "[Static]")]
		[TestCase ("typeof (NSString)", 1131, "[Category]")]
		public void InvalidAttribute (string elementType, int error, string extraAttribute = "")
		{
			Configuration.IgnoreIfIgnoredPlatform (Profile.macOSMobile.AsPlatform ());
			var bgen = new BGenTool {
				Profile = Profile.macOSMobile,
			};
			bgen.CreateTemporaryBinding ($@"
using Foundation;
namespace FastEnumeration {{
	[BaseType (typeof (NSObject))]
	[NSFastEnumeration ({elementType})]
	{extraAttribute}
	interface InvalidCollection {{}}
}}");
			bgen.AssertExecuteError ("Invalid enumeration attribute");
			Assert.That (bgen.Messages.Where (v => v.IsError).Select (v => v.Number), Does.Contain (error), "Diagnostic");
		}
	}
}
