using System.Linq;
using System.Reflection;

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

					foreach (var enumValue in Enum.GetValuesAsUnderlyingType (type)) {
						var obj = getConstant.Invoke (null, new object [] { enumValue });

						if (valuesToSkip.Remove ((type, enumValue)))
							continue;

						if (obj is not null) {
							var rtrip = getValue.Invoke (null, new object [] { obj });
							Assert.AreEqual (Enum.ToObject (type, enumValue), rtrip, $"{type.FullName}.{enumValue}: Round trip failed: {enumValue}.GetConstant () -> {obj} but GetValue ({obj}) -> {rtrip}");
						}
					}
				}

				// Only verify that all the skipped values are correct if nothing has been trimmed away.
				if (!TestRuntime.IsLinkAny)
					Assert.That (valuesToSkip, Is.Empty, "All values to be skipped were actually skipped");
			});
		}

		(Type EnumType, object UnderlyingValue) [] GetSkippedEnumValues ()
		{
			var rv = new List<(Type EnumType, object UnderlyingValue)> () {
#if !XAMCORE_5_0
				(typeof (global::AVFoundation.AVCaptureDeviceType), (int) global::AVFoundation.AVCaptureDeviceType.BuiltInDualCamera),
#if __MACOS__
				(typeof (global::AVFoundation.AVCaptureDeviceType), (int) global::AVFoundation.AVCaptureDeviceType.External),
#endif
				(typeof (global::AVFoundation.AVCaptureDeviceType), (int) global::AVFoundation.AVCaptureDeviceType.Microphone),
				(typeof (global::Foundation.NSLinguisticTag), (int) global::Foundation.NSLinguisticTag.OtherPunctuation),
				(typeof (global::Foundation.NSLinguisticTag), (int) global::Foundation.NSLinguisticTag.OtherWhitespace),
				(typeof (global::Foundation.NSRunLoopMode), (int) global::Foundation.NSRunLoopMode.Other),
#if !__TVOS__
				(typeof (global::HealthKit.HKCategoryTypeIdentifier), (int) global::HealthKit.HKCategoryTypeIdentifier.EnvironmentalAudioExposureEvent),
#endif
#if __MACOS__
				(typeof (global::iTunesLibrary.ITLibPlaylistProperty), (int) global::iTunesLibrary.ITLibPlaylistProperty.Primary),
				(typeof (global::ImageKit.IKToolMode), (int) global::ImageKit.IKToolMode.SelectRect),
#endif
				(typeof (global::Security.SecKeyType), (int) global::Security.SecKeyType.ECSecPrimeRandom),
#if !__MACOS__
				(typeof (global::UIKit.UIWindowSceneSessionRole), (int) global::UIKit.UIWindowSceneSessionRole.ExternalDisplayNonInteractive),
#endif
#endif // !XAMCORE_5_0
			};

#if __TVOS__ && !XAMCORE_5_0
			if (Runtime.Arch == Arch.SIMULATOR) {
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityTrait> ().Select (v => (typeof (global::BrowserEngineKit.BEAccessibilityTrait), (object) (int) v))); // BrowserEngineKit isn't available in the simulator
				rv.AddRange (Enum.GetValues<global::BrowserEngineKit.BEAccessibilityNotification> ().Select (v => (typeof (global::BrowserEngineKit.BEAccessibilityNotification), (object) (int) v))); // BrowserEngineKit isn't available in the simulator
			}
#endif // __TVOS__ && !XAMCORE_5_0

#if !__MACOS__
			if (TestRuntime.CheckXcodeVersion (26, 0)) {
				// NewScene and NewItem both return 'com.apple.menu.new-item' so
				// Round trip failed: NewItem.GetConstant () -> com.apple.menu.new-item but GetValue (com.apple.menu.new-item) -> NewScene
				// That said NewItem is the one that should be used and NewScene is deprecated in Xcode 26.0.
				rv.Add ((typeof (global::UIKit.UIMenuIdentifier), (int) global::UIKit.UIMenuIdentifier.NewItem));
			}
#endif // !__MACOS__

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
