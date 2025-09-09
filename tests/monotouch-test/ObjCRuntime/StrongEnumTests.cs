using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Foundation;
using ObjCRuntime;

using NUnit.Framework;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StrongEnumTest {
		[Test]
		public void GetConstant ()
		{
			TestRuntime.AssertMatchingOSVersionAndSdkVersion ();

			Assert.Multiple (() => {
#pragma warning disable IL2026 // we only care about types that survived trimming, so this is safe.
				var allTypes = typeof (NSObject).Assembly.GetTypes ();
#pragma warning restore IL2026
				var types = allTypes.ToDictionary (v => v.FullName, v => v);
				var valuesToSkip = GetSkippedEnumValues ().ToHashSet ();
				var skippedValues = new List<object> ();

				foreach (var type in allTypes) {
					if (!type.IsEnum)
						continue;
					if (type.IsDefined (typeof (ObsoleteAttribute)))
						continue;
					if (!types.TryGetValue (type.FullName + "Extensions", out var extensions))
						continue;
					var getConstant = extensions.GetMethod ("GetConstant", new Type [] { type });
					if (getConstant is null || getConstant.IsDefined (typeof (ObsoleteAttribute)))
						continue;
					var getValue = extensions.GetMethod ("GetValue", new Type [] { GetNonnullableType (getConstant.ReturnType) });
					if (getValue is null || getConstant.IsDefined (typeof (ObsoleteAttribute)))
						continue;

#pragma warning disable IL3050 // Using member 'System.Enum.GetValues(Type)' which has 'RequiresDynamicCodeAttribute' can break functionality when AOT compiling. It might not be possible to create an array of the enum type at runtime. Use the GetValues<TEnum> overload or the GetValuesAsUnderlyingType method instead.
					foreach (var enumValue in Enum.GetValues (type)) {
#pragma warning restore IL3050
						var obj = getConstant.Invoke (null, new object [] { enumValue });

						if (valuesToSkip.Remove ((Enum) enumValue))
							continue;

						if (obj is not null) {
							var rtrip = getValue.Invoke (null, new object [] { obj });
							Assert.AreEqual (enumValue, rtrip, $"{type.FullName}.{enumValue}: Round trip failed: {enumValue}.GetConstant () -> {obj} but GetValue ({obj}) -> {rtrip}");
						}
					}
				}

				// Only very that all the skipped values are correct if nothing has been trimmed away.
				if (!TestRuntime.IsLinkAny)
					Assert.That (valuesToSkip, Is.Empty, "All values to be skipped were actually skipped");
			});
		}

		Enum [] GetSkippedEnumValues ()
		{
			var rv = new List<Enum> () {
#if !XAMCORE_5_0
				global::AVFoundation.AVCaptureDeviceType.BuiltInDualCamera,
#if __MACOS__
				global::AVFoundation.AVCaptureDeviceType.External,
#endif
				global::AVFoundation.AVCaptureDeviceType.Microphone,
				global::Foundation.NSLinguisticTag.OtherPunctuation,
				global::Foundation.NSLinguisticTag.OtherWhitespace,
				global::Foundation.NSRunLoopMode.Other,
#if !__TVOS__
				global::HealthKit.HKCategoryTypeIdentifier.EnvironmentalAudioExposureEvent,
#endif
#if __MACOS__
				global::iTunesLibrary.ITLibPlaylistProperty.Primary,
				global::ImageKit.IKToolMode.SelectRect,
#endif
				global::Security.SecKeyType.ECSecPrimeRandom,
#if !__MACOS__
				global::UIKit.UIWindowSceneSessionRole.ExternalDisplayNonInteractive,
#endif
#endif // !XAMCORE_5_0
			};

#if __TVOS__ && !XAMCORE_5_0
			if (Runtime.Arch == Arch.SIMULATOR) {
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityTrait> ().Cast<Enum> ()); // BrowserEngineKit isn't available in the simulator
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityNotification> ().Cast<Enum> ()); // BrowserEngineKit isn't available in the simulator
			}
#endif // __TVOS__ && !XAMCORE_5_0

			return rv.ToArray ();
		}

		static Type GetNonnullableType (Type type)
		{
			if (!type.IsValueType)
				return type;
			if (!type.IsGenericType)
				return type;
			var ggtd = type.GetGenericTypeDefinition ();
			if (ggtd.Name != "Nullable`1")
				return type;
			return type.GetGenericArguments () [0];
		}
	}
}
