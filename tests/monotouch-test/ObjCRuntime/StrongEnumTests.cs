using System;
using System.Collections.Generic;
using System.Linq;

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
			Assert.Multiple (() => {
				var allTypes = typeof (NSObject).Assembly.GetTypes ();
				var types = allTypes.ToDictionary (v => v.FullName, v => v);
				var valuesToSkip = GetSkippedEnumValues ().ToHashSet ();
				var skippedValues = new List<object> ();

				foreach (var type in allTypes) {
					if (!type.IsEnum)
						continue;
					if (!types.TryGetValue (type.FullName + "Extensions", out var extensions))
						continue;
					var getConstant = extensions.GetMethod ("GetConstant", new Type [] { type });
					if (getConstant is null)
						continue;
					var getValue = extensions.GetMethod ("GetValue", new Type [] { GetNonnullableType (getConstant.ReturnType) });
					if (getValue is null)
						continue;

					foreach (var enumValue in Enum.GetValues (type)) {
						var obj = getConstant.Invoke (null, new object [] { enumValue });

						if (valuesToSkip.Remove ((Enum) enumValue))
							continue;

						if (obj is not null) {
							var rtrip = getValue.Invoke (null, new object [] { obj });
							Assert.AreEqual (enumValue, rtrip, $"{type.FullName}.{enumValue}: Round trip failed: {enumValue}.GetConstant () -> {obj} but GetValue ({obj}) -> {rtrip}");
						}
					}
				}

				Assert.That (valuesToSkip, Is.Empty, "All values to be skipped were actually skipped");
			});
		}

		Enum [] GetSkippedEnumValues ()
		{
			var rv = new List<Enum> () {
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
			};

#if __TVOS__
			if (Runtime.Arch == Arch.SIMULATOR) {
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityTrait> ().Cast<Enum> ()); // BrowserEngineKit isn't available in the simulator
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityNotification> ().Cast<Enum> ()); // BrowserEngineKit isn't available in the simulator
			}
#endif // __TVOS__

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
