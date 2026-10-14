// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Xamarin.Utils;

namespace Cecil.Tests;

[TestFixture]
public class DeprecationTest {
	static void Check (AssemblyInfo info, string typeName, string selector, Version version)
	{
		var type = info.Assembly.MainModule.GetType (typeName);
		Assert.That (type, Is.Not.Null, typeName);
		var method = type.Methods.Single (v => v.CustomAttributes.Any (a =>
			a.AttributeType.Name == "ExportAttribute" && (string) a.ConstructorArguments [0].Value == selector));
		ICustomAttributeProvider member = type.Properties.FirstOrDefault (v => v.GetMethod == method || v.SetMethod == method) is PropertyDefinition property
			? property : method;
		var availability = member.GetAvailabilityAttributes (info.Platform);
		Assert.That (availability?.Obsoleted?.Version, Is.EqualTo (version), $"{info.Platform}: {typeName}::{selector}");
		Assert.That (availability?.Obsoleted?.Message, Is.Not.Null.And.Not.Empty, "Replacement or explanation message");
	}

	[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblyDefinitions))]
	public void DeprecatedBindings (AssemblyInfo info)
	{
		var metalVersion = info.Platform == ApplePlatform.MacOSX ? new Version (13, 0) : new Version (16, 0);
		Check (info, "Metal.IMTLDevice", "newLibraryWithFile:error:", metalVersion);
		Check (info, "Metal.IMTLDevice", "supportsFeatureSet:", metalVersion);
		if (info.Platform != ApplePlatform.TVOS)
			Check (info, "Metal.IMTLDevice", "areBarycentricCoordsSupported", metalVersion);
		Check (info, "Metal.IMTLFunction", "newArgumentEncoderWithBufferIndex:reflection:", metalVersion);
		foreach (var selector in new [] { "useResource:usage:", "useResources:count:usage:", "useHeap:", "useHeaps:count:" })
			Check (info, "Metal.IMTLRenderCommandEncoder", selector, metalVersion);
		foreach (var selector in new [] {
			"getTextureAccessCounters:region:mipLevel:slice:resetCounters:countersBuffer:countersBufferOffset:",
			"resetTextureAccessCounters:region:mipLevel:slice:",
		})
			Check (info, "Metal.IMTLBlitCommandEncoder", selector, new Version (26, 4));

		Check (info, "GameController.IGCDevice", "physicalInputProfile", metalVersion);
		Check (info, "GameController.GCKeyboard", "physicalInputProfile", metalVersion);
		Check (info, "GameController.GCMouse", "physicalInputProfile", metalVersion);
		var controller = info.Assembly.MainModule.GetType ("GameController.GCController");
		var profile = controller.Properties.Single (v => v.Name == "PhysicalInputProfile");
		Assert.That (profile.GetAvailabilityAttributes (info.Platform)?.Obsoleted, Is.Null, "GCController redeclares a supported property");

		if (info.Platform == ApplePlatform.iOS || info.Platform == ApplePlatform.MacCatalyst) {
			Check (info, "CoreNFC.INFCIso15693Tag", "lockDFSIDWithRequestFlag:completionHandler:", new Version (14, 0));
			Check (info, "CoreNFC.INFCIso15693Tag", "getSystemInfoWithRequestFlag:completionHandler:", new Version (14, 0));
		}
		if (info.Platform == ApplePlatform.MacOSX) {
			var version = new Version (10, 15);
			foreach (var selector in new [] { "moduleExecutableArchitecture", "requestYield", "requestEjectOrDisconnect" })
				Check (info, "ImageCaptureCore.ICDevice", selector, version);
			foreach (var selector in new [] { "thumbnailIfAvailable", "largeThumbnailIfAvailable", "metadataIfAvailable" })
				Check (info, "ImageCaptureCore.ICCameraItem", selector, version);
			foreach (var selector in new [] { "cameraDevice:didAddItem:", "cameraDevice:didRemoveItem:", "cameraDevice:didReceiveMetadataForItem:" }) {
				Check (info, "ImageCaptureCore.IICCameraDeviceDelegate", selector, version);
				Check (info, "ImageCaptureCore.ICCameraDeviceDelegate", selector, version);
			}
		}
	}
}
