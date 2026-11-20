// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

using Mono.Cecil;

using Xamarin.Tests;

namespace GeneratorTests {
	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class ProtocolWrapTests : BGenBase {
		[TestCase (Profile.iOS, false)]
		[TestCase (Profile.iOS, true)]
		[TestCase (Profile.tvOS, false)]
		[TestCase (Profile.tvOS, true)]
		[TestCase (Profile.macOSMobile, false)]
		[TestCase (Profile.macOSMobile, true)]
		[TestCase (Profile.MacCatalyst, false)]
		[TestCase (Profile.MacCatalyst, true)]
		public void WrappedMembers (Profile profile, bool compatible)
		{
			var bgen = BuildFile (profile, tool => {
				if (compatible)
					tool.Defines = (tool.Defines ?? []).Concat (new [] { "COMPATIBLE" }).ToArray ();
			}, "protocol-wrap.cs");
			var module = bgen.ApiAssembly.MainModule;
			var protocol = module.GetType ("ProtocolWrap.IWrappedProtocol");
			var wrapperNames = new [] {
				"get_Delegate", "set_Delegate",
				"get_Alias", "set_Alias",
				"get_ConvertedValue", "set_ConvertedValue",
				"get_ReadOnlyValue", "set_WriteOnlyValue",
				"get_NullableValue", "set_NullableValue",
				"get_Attributes", "ComputeAlias", "SetValue", "ComputeValue",
			};

			foreach (var typeName in new [] { "IWrappedProtocol", "WrappedProtocol", "Adopter" }) {
				var type = module.GetType ("ProtocolWrap." + typeName);
				foreach (var name in wrapperNames) {
					var method = type.Methods.Single (v => v.Name == name);
					Assert.That (method.HasBody, Is.True, method.FullName);
					var calls = method.Body.Instructions.Select (v => v.Operand).OfType<MethodReference> ().ToArray ();
					Assert.That (calls.Select (v => v.DeclaringType.Name), Does.Not.Contain ("You_Should_Not_Call_base_In_This_Method"), method.FullName);
					Assert.That (calls.Select (v => v.DeclaringType.FullName), Does.Contain (type.FullName), method.FullName);
					var attributes = method.CustomAttributes.Select (v => v.AttributeType.FullName).ToArray ();
					Assert.That (attributes, Does.Not.Contain ("Foundation.ExportAttribute"), method.FullName);
					Assert.That (attributes, Does.Not.Contain ("Foundation.RequiredMemberAttribute").And.Not.Contain ("Foundation.OptionalMemberAttribute"), method.FullName);
				}
			}

			var protocolMembers = protocol.CustomAttributes
				.Where (v => v.AttributeType.FullName == "Foundation.ProtocolMemberAttribute")
				.Select (v => v.Properties.Single (p => p.Name == "Name").Argument.Value)
				.ToArray ();
			Assert.That (protocolMembers, Is.EquivalentTo (new [] { "Value", "WeakDelegate", "WeakAttributes", "Compute" }), "Native protocol metadata");
			Assert.That (protocol.Properties.Select (v => v.Name), Does.Contain ("Attributes"), "Dictionary wrapper");
			Assert.That (module.GetType ("ProtocolWrap.IDerivedProtocol").Methods.Select (v => v.Name).Intersect (wrapperNames), Is.Empty, "Inherited wrappers need not be redeclared");
			foreach (var name in new [] { "get_Constant", "GetConstant" }) {
				var method = protocol.Methods.Single (v => v.Name == name);
				Assert.That (method.IsStatic, Is.True, method.FullName);
				Assert.That (method.HasBody, Is.True, method.FullName);
			}
		}

		[TestCase (Profile.iOS)]
		[TestCase (Profile.tvOS)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.MacCatalyst)]
		public void PlatformMembers (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			using var assembly = AssemblyDefinition.ReadAssembly (Configuration.GetBaseLibraryImplementations (profile.AsPlatform ()).First ());
			var members = new List<(string Type, string Member)> {
				("CoreImage.ICISystemToneMapProtocol", "PreferredDynamicRange"),
				("CoreImage.CISystemToneMap", "PreferredDynamicRange"),
				("SceneKit.ISCNSceneRenderer", "SceneRendererDelegate"),
				("SceneKit.ISCNSceneRenderer", "HitTest"),
				("SceneKit.ISCNShadable", "ShaderModifiers"),
				("VideoToolbox.IVTFrameProcessorConfiguration", "FrameSupportedPixelFormats"),
				("VideoToolbox.IVTFrameProcessorConfiguration", "SourcePixelBufferAttributes"),
				("VideoToolbox.IVTFrameProcessorConfiguration", "DestinationPixelBufferAttributes"),
			};
			if (profile == Profile.macOSMobile) {
#if !XAMCORE_5_0
				members.Add (("AppKit.INSCollectionViewDataSource", "GetNumberOfItems"));
#endif
				members.Add (("AuthenticationServices.IASAuthorizationProviderExtensionRegistrationHandler", "SupportedDeviceSigningAlgorithms"));
				members.Add (("AuthenticationServices.IASAuthorizationProviderExtensionRegistrationHandler", "SupportedDeviceEncryptionAlgorithms"));
				members.Add (("AuthenticationServices.IASAuthorizationProviderExtensionRegistrationHandler", "SupportedUserSecureEnclaveKeySigningAlgorithms"));
#if !XAMCORE_5_0
				members.Add (("MediaExtension.IMESampleCursor", "SyncInfo"));
				members.Add (("MediaExtension.IMESampleCursor", "DependencyInfo"));
#endif
				members.Add (("PhotosUI.IPHProjectExtensionController", "GetTypeDescriptionDataSource"));
			} else {
				members.Add (("UIKit.IUITextInput", "InputDelegate"));
				members.Add (("UIKit.IUITextInput", "Tokenizer"));
				if (profile == Profile.iOS)
					members.Add (("BrowserEngineKit.IBETextInput", "AsyncInputDelegate"));
#if !XAMCORE_5_0
				if (profile != Profile.tvOS)
					members.Add (("AVFoundation.IAVAssetDownloadDelegate", "WillDownloadToUrl"));
#endif
			}

			foreach (var member in members) {
				var type = assembly.MainModule.GetType (member.Type);
				Assert.That (type, Is.Not.Null, member.Type);
				var property = type.Properties.FirstOrDefault (v => v.Name == member.Member);
				if (property is not null) {
					Assert.That (property.GetMethod?.HasBody, Is.True, property.FullName);
				} else {
					var methods = type.Methods.Where (v => v.Name == member.Member && !v.CustomAttributes.Any (a => a.AttributeType.FullName == "Foundation.ExportAttribute")).ToArray ();
					Assert.That (methods, Is.Not.Empty, member.ToString ());
					foreach (var method in methods)
						Assert.That (method.HasBody, Is.True, method.FullName);
				}
			}
		}
	}
}
