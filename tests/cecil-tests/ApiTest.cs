using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {

	[TestClass]
	public partial class ApiTest {
		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformAssemblyTestData), typeof (Helper))]
		public void ARConfiguration_GetSupportedVideoFormats (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			// all subclasses of ARConfiguration must (re)export 'GetSupportedVideoFormats'
			var assembly = info.Assembly;
			List<string>? failures = null;

			if (!assembly.EnumerateTypes ((type) => type.Is ("ARKit", "ARConfiguration")).Any ())
				Assert.Inconclusive ("This assembly does not contain ARKit.ARConfiguration."); // This assembly doesn't contain ARKit.ARConfiguration

			var subclasses = assembly.EnumerateTypes ((type) => !type.Is ("ARKit", "ARConfiguration") && type.IsSubclassOf ("ARKit", "ARConfiguration"));
			Assert.IsNotEmpty (subclasses, "At least some subclasses");

			foreach (var type in subclasses) {
				var method = type.Methods.SingleOrDefault (m => m.Name == "GetSupportedVideoFormats" && m.IsPublic && m.IsStatic && !m.HasParameters);
				if (method is null)
					AddFailure (ref failures, $"The type {type.FullName} does not implement the method GetSupportedVideoFormats.");
			}

			Assert.IsEmpty (failures ?? [], $"All subclasses from ARConfiguration must explicitly implement GetSupportedVideoFormats.\n{string.Join ("\n", failures ?? [])}");
		}

		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformAssemblyTestData), typeof (Helper))]
		public void CNAssetPreprocessConfiguration_Constructors (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			var type = info.Assembly.MainModule.GetType ("Cinematic.CNAssetPreprocessConfiguration");
			if (info.Platform == ApplePlatform.TVOS) {
				Assert.IsNull (type, "CNAssetPreprocessConfiguration is unavailable on tvOS.");
				return;
			}

			Assert.IsNotNull (type, "CNAssetPreprocessConfiguration must be available.");
			var constructors = type.Methods.Where (m => m.IsConstructor && m.IsPublic).ToArray ();
			Assert.AreEqual (1, constructors.Length, "Only the destination URL constructor should be public.");
			Assert.AreEqual (1, constructors [0].Parameters.Count, "Constructor parameter count");
			Assert.AreEqual ("Foundation.NSUrl", constructors [0].Parameters [0].ParameterType.FullName, "Destination URL parameter");
		}

		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformAssemblyTestData), typeof (Helper))]
		[DynamicData (nameof (Helper.NetPlatformImplementationAssemblyTestData), typeof (Helper))]
		public void CSSearchableIndexDelegate_GetSearchableItems (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			string [] typeNames = [
				"CoreSpotlight.ICSSearchableIndexDelegate",
				"CoreSpotlight.CSSearchableIndexDelegate",
#if !XAMCORE_5_0
				"CoreSpotlight.CSSearchableIndexDelegate_Extensions",
#endif
				"CoreSpotlight.CSIndexExtensionRequestHandler",
			];
			string [] parameterTypes = ["System.String[]", "Foundation.NSFileProtectionType", "CoreSpotlight.CSSearchableIndexDelegateGetSearchableItemsHandler"];

			foreach (var typeName in typeNames) {
				var type = info.Assembly.MainModule.GetType (typeName);
				if (info.Platform == ApplePlatform.TVOS) {
					Assert.IsNull (type, $"{typeName} is unavailable on tvOS.");
					continue;
				}

				Assert.IsNotNull (type, typeName);
				var offset = typeName.EndsWith ("_Extensions", StringComparison.Ordinal) ? 1 : 0;
				var method = type.Methods.Single (m => m.Name == "GetSearchableItems" && m.Parameters.Count == 3 + offset);
				Assert.IsTrue (method.IsPublic, $"{typeName} visibility");
				Assert.AreEqual ("System.Void", method.ReturnType.FullName, $"{typeName} return type");
				CollectionAssert.AreEqual (parameterTypes, method.Parameters.Skip (offset).Select (p => p.ParameterType.FullName).ToArray (), $"{typeName} parameter types");
				var olderOverload = type.Methods.Single (m => m.Name == "GetSearchableItems" && m.Parameters.Count == 2 + offset);
				Assert.IsTrue (olderOverload.IsPublic, $"{typeName} older overload visibility");
				Assert.AreEqual ("System.Void", olderOverload.ReturnType.FullName, $"{typeName} older overload return type");
				CollectionAssert.AreEqual (new [] { parameterTypes [0], parameterTypes [2] }, olderOverload.Parameters.Skip (offset).Select (p => p.ParameterType.FullName).ToArray (), $"{typeName} older overload parameter types");
				Assert.IsTrue (olderOverload.Parameters [1 + offset].CustomAttributes.Any (a => a.AttributeType.Is ("ObjCRuntime", "BlockProxyAttribute")), $"{typeName} older overload block proxy");

				var bindAs = method.Parameters [1 + offset].CustomAttributes.Single (a => a.AttributeType.Is ("ObjCRuntime", "BindAsAttribute"));
				Assert.AreEqual ("Foundation.NSFileProtectionType", ((TypeReference) bindAs.ConstructorArguments [0].Value).FullName, $"{typeName} BindAs type");
				Assert.AreEqual ("Foundation.NSString", ((TypeReference) bindAs.Fields.Single (f => f.Name == "OriginalType").Argument.Value).FullName, $"{typeName} native type");
				Assert.IsTrue (method.Parameters [2 + offset].CustomAttributes.Any (a => a.AttributeType.Is ("ObjCRuntime", "BlockProxyAttribute")), $"{typeName} block proxy");
			}

			if (info.Platform != ApplePlatform.TVOS) {
				var protocol = info.Assembly.MainModule.GetType ("CoreSpotlight.ICSSearchableIndexDelegate");
				var member = protocol.CustomAttributes.Single (a => a.AttributeType.Is ("Foundation", "ProtocolMemberAttribute") &&
					a.Properties.Any (p => p.Name == "Selector" && (string) p.Argument.Value == "searchableItemsForIdentifiers:protectionClass:searchableItemsHandler:"));
				Assert.AreEqual (false, member.Properties.Single (p => p.Name == "IsRequired").Argument.Value, "Optional protocol method");
				var nativeTypes = (CustomAttributeArgument []) member.Properties.Single (p => p.Name == "ParameterType").Argument.Value;
				CollectionAssert.AreEqual (new [] { "System.String[]", "Foundation.NSString", "CoreSpotlight.CSSearchableIndexDelegateGetSearchableItemsHandler" }, nativeTypes.Select (p => ((TypeReference) p.Value).FullName).ToArray (), "Native protocol parameter types");
				var blockProxies = (CustomAttributeArgument []) member.Properties.Single (p => p.Name == "ParameterBlockProxy").Argument.Value;
				Assert.AreEqual (3, blockProxies.Length, "Block proxy count");
				Assert.IsNull (blockProxies [0].Value, "Identifiers block proxy");
				Assert.IsNull (blockProxies [1].Value, "Protection class block proxy");
				Assert.IsNotNull (blockProxies [2].Value, "Handler block proxy");
			}
		}

		static void AddFailure (ref List<string>? failures, string failure)
		{
			if (failures is null)
				failures = new List<string> ();

			failures.Add (failure);
			Console.WriteLine (failure);
		}

		// Capitalization rules:
		// * All types, methods, properties, fields, events must start with an upper-cased letter.
		// * All parameters must start with a lower-cased letter
		[TestMethod]
		public void MustStartWithCapitalLetter ()
		{
			Configuration.IgnoreIfAnyIgnoredPlatforms ();

			var failures = new Dictionary<string, (string Message, string Location)> ();

			foreach (var info in Helper.NetPlatformImplementationAssemblyDefinitions) {
				var assembly = info.Assembly;
				foreach (var member in assembly.EnumeratePublicMembers ()) {
					// Presumably obsolete members have been obsoleted for a reason, so assume there's a correctly capitalized version.
					if (((ICustomAttributeProvider) member).IsObsolete ())
						continue;

					if (member is MethodDefinition method) {
						// Check parameter names
						foreach (var param in method.Parameters) {
							if (param.Index == 0 && method.IsExtensionMethod ())
								continue;
							if (!char.IsLower (param.Name [0])) {
								var msg = $"The parameter '{param.Name}' in the method '{method.FullName}' has incorrect capitalization: first letter is not lower case.";
								failures [msg] = new (msg, method.RenderLocation ());
							}
						}

						// skip constructors
						if (method.IsConstructor)
							continue;
						// skip property accessors
						if (method.IsPropertyAccessor ())
							continue;
						// skip event methods
						if (method.IsEventMethod ())
							continue;
						// skip operators
						if (method.IsOperator ())
							continue;
					} else if (member is FieldDefinition field) {
						if (field.Name == "value__" && !field.IsStatic && field.DeclaringType.IsEnum)
							continue;
					}

					if (!char.IsUpper (member.Name [0]) && !IsAcceptableCapitalization (member)) {
						var msg = $"The {member.GetType ().Name.Replace ("Definition", "").ToLower ()} '{member.FullName}' has incorrect capitalization: first letter is not upper case.";
						failures [msg] = new (msg, member.RenderLocation ());
					}
				}
			}

			Helper.AssertFailures (failures, knownFailuresMustStartWithCapitalizationLetter, nameof (knownFailuresMustStartWithCapitalizationLetter), "Incorrect capitalization", (v) => $"{v.Location}: {v.Message}");
		}

		// The difference between this and the known failures is that these aren't failures (that should be fixed), they're exceptions we've deemed acceptable (and shouldn't be fixed).
		bool IsAcceptableCapitalization (MemberReference mr)
		{
			if (mr is FieldDefinition field) {
				switch (field.DeclaringType.FullName) {
				case "Metal.MTLFeatureSet":
					if (field.Name.StartsWith ("iOS_", StringComparison.Ordinal))
						return true;
					if (field.Name.StartsWith ("macOS_", StringComparison.Ordinal))
						return true;
					if (field.Name.StartsWith ("tvOS_", StringComparison.Ordinal))
						return true;
					break;
				case "Metal.MTLGpuFamily":
					if (field.Name.StartsWith ("iOSMac", StringComparison.Ordinal))
						return true;
					break;
				}
			} else if (mr is PropertyDefinition property) {
				switch (property.DeclaringType.FullName) {
				case "AVFoundation.AVMetadata":
					if (property.Name.StartsWith ("iTunes", StringComparison.Ordinal))
						return true;
					break;
				}
			}
			return false;
		}

		[TestMethod]
		public void InvalidStrings ()
		{
			Configuration.IgnoreIfAnyIgnoredPlatforms ();

			var invalidStrings = new [] {
				new { Find = "URL", Replacement = "Url" }
			};

			var failures = new Dictionary<string, (string Message, string Location)> ();

			foreach (var info in Helper.NetPlatformImplementationAssemblyDefinitions) {
				var assembly = info.Assembly;
				foreach (var member in assembly.EnumeratePublicMembers ()) {
					foreach (var term in invalidStrings) {
						if (member.Name.Contains (term.Find)) {
							var msg = $"The {member.GetType ().Name.Replace ("Definition", "").ToLower ()} '{member.FullName}' has an invalid term '{term.Find}'. Replace with: '{term.Replacement}'.";
							failures [msg] = new (msg, member.RenderLocation ());
						}
					}
				}
			}

			Helper.AssertFailures (failures, knownFailuresInvalidStrings, nameof (knownFailuresInvalidStrings), "In the file tests/cecil-tests/ApiTest.cs, read the guide carefully.", (v) => $"{v.Location}: {v.Message}");
		}

		[TestMethod]
		public void BannedAttributes ()
		{
			Configuration.IgnoreIfAnyIgnoredPlatforms ();

			var bannedAttributeTypes = new [] {
				new { Namespace = "Foundation", Name = "PreserveAttribute" },
			};

			var failures = new Dictionary<string, (string Message, string Location, ICustomAttributeProvider Provider)> ();
			foreach (var info in Helper.NetPlatformImplementationAssemblyDefinitions) {
				foreach (var ap in info.Assembly.EnumerateAttributeProviders ()) {
					if (!ap.HasCustomAttributes)
						continue;

					foreach (var ca in ap.CustomAttributes) {
						foreach (var tp in bannedAttributeTypes) {
							if (!ca.AttributeType.Is (tp.Namespace, tp.Name))
								continue;

							var fullname = ap.AsFullName ();
							failures [fullname] = (Message: $"'{fullname}' has a [Preserve] attribute", Location: ap.RenderLocation (), Provider: ap);
							break;
						}
					}
				}
			}

			Helper.AssertFailures (failures, knownFailuresBannedAttributes, nameof (knownFailuresBannedAttributes), "APIs with [Preserve] - alternative solutions must be found!", (v) => $"{v.Location}: {v.Message}");
		}
	}
}
