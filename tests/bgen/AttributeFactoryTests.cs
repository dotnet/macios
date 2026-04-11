using System.Collections;
using Xamarin.Utils;

#nullable enable

namespace GeneratorTests {

	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class AttributeFactoryTests {
		static void AssertAttributeCreation<T> (Func<PlatformName, int, int, string?, T> callback, PlatformName platform,
			int major, int minor, string? message = null) where T : AvailabilityBaseAttribute
		{
			var typeName = typeof (T).Name;
			var attr = callback (platform, major, minor, message) as T;
			Assert.That (attr, Is.Not.Null, $"{typeName} attribute type");
			Assert.That (attr.Platform, Is.EqualTo (platform), $"{typeName} Platform");
			Assert.That (attr.Version!.Major, Is.EqualTo (major), $"{typeName} Major");
			Assert.That (attr.Version!.Minor, Is.EqualTo (minor), $"{typeName} Minor");
			Assert.That (attr.Message, Is.EqualTo (message));
		}

		static void AssertAttributeCreationNotVersion<T> (Func<PlatformName, string?, T> callback, PlatformName platform,
			string? message = null) where T : AvailabilityBaseAttribute
		{
			var typeName = typeof (T).Name;
			var attr = callback (platform, message) as T;
			Assert.That (attr, Is.Not.Null, $"{typeName} attribute type");
			Assert.That (attr.Platform, Is.EqualTo (platform), $"{typeName} Platform");
			Assert.That (attr.Message, Is.EqualTo (message));
		}


		// simple tests, but we want to test it
		[TestCase (PlatformName.iOS, 13, 4, "message")]
		[TestCase (PlatformName.iOS, 12, 4, null)]
		public void CreateAttributeTest (PlatformName platform, int major, int minor, string? message)
		{
			// call several times with diff types
			AssertAttributeCreation (AttributeFactory.CreateNewAttribute<IntroducedAttribute>, platform, major, minor, message);
			AssertAttributeCreation (AttributeFactory.CreateNewAttribute<DeprecatedAttribute>, platform, major, minor, message);
			AssertAttributeCreation (AttributeFactory.CreateNewAttribute<ObsoletedAttribute>, platform, major, minor, message);
		}

		[TestCase (PlatformName.iOS, "message")]
		[TestCase (PlatformName.iOS, null)]
		public void CreateAttributeNoVersionTest (PlatformName platform, string? message)
		{
			// call several times with diff types
			AssertAttributeCreationNotVersion (AttributeFactory.CreateNewAttribute<IntroducedAttribute>, platform, message);
			AssertAttributeCreationNotVersion (AttributeFactory.CreateNewAttribute<UnavailableAttribute>, platform, message);
			AssertAttributeCreationNotVersion (AttributeFactory.CreateNewAttribute<DeprecatedAttribute>, platform, message);
			AssertAttributeCreationNotVersion (AttributeFactory.CreateNewAttribute<ObsoletedAttribute>, platform, message);
		}

		[TestCase (PlatformName.iOS)]
		[TestCase (PlatformName.MacCatalyst)]
		[TestCase (PlatformName.MacOSX)]
		[TestCase (PlatformName.TvOS)]
		public void CreateNoVersionSupportedAttributeTest (PlatformName platform)
			=> Assert.That (AttributeFactory.CreateNoVersionSupportedAttribute (platform).Platform, Is.EqualTo (platform));

		[TestCase (PlatformName.iOS)]
		[TestCase (PlatformName.MacCatalyst)]
		[TestCase (PlatformName.MacOSX)]
		[TestCase (PlatformName.TvOS)]
		public void CreateUnsupportedAttributeTest (PlatformName platform)
			=> Assert.That (AttributeFactory.CreateUnsupportedAttribute (platform).Platform, Is.EqualTo (platform));

		class CloneCasesNoVersionClass : IEnumerable {
			public IEnumerator GetEnumerator ()
			{
				yield return new object [] {

					new IntroducedAttribute (PlatformName.iOS),
					PlatformName.TvOS,
				};
				yield return new object [] {
					new DeprecatedAttribute (PlatformName.MacCatalyst),
					PlatformName.iOS,
				};
				yield return new object [] {
					new UnavailableAttribute (PlatformName.MacOSX),
					PlatformName.MacCatalyst
				};
			}
		}

		[TestCaseSource (typeof (CloneCasesNoVersionClass))]
		public void CloneNoVersionTest (AvailabilityBaseAttribute attributeToClone, PlatformName targetPlatform)
		{
			var clone = AttributeFactory.CloneFromOtherPlatform (attributeToClone, targetPlatform);
			Assert.That (clone.Platform, Is.EqualTo (targetPlatform), "platform");
			Assert.That (clone.Message, Is.EqualTo (attributeToClone.Message), "message");
			Assert.That (clone.GetType (), Is.EqualTo (attributeToClone.GetType ()), "type");
		}

		class CloneCasesMinVersionClass : IEnumerable {
			public IEnumerator GetEnumerator ()
			{
				yield return new object [] {
					new IntroducedAttribute (PlatformName.iOS, 1, 0),
					PlatformName.TvOS,
				};
				yield return new object [] {
					new DeprecatedAttribute (PlatformName.MacCatalyst, 1, 0),
					PlatformName.iOS,
				};
			}
		}

		[TestCaseSource (typeof (CloneCasesMinVersionClass))]
		public void CloneMinVersion (AvailabilityBaseAttribute attributeToClone, PlatformName targetPlatform)
		{
			var clone = AttributeFactory.CloneFromOtherPlatform (attributeToClone, targetPlatform);
			Assert.That (clone.Platform, Is.EqualTo (targetPlatform), "platform");
			Assert.That (clone.Message, Is.EqualTo (attributeToClone.Message), "message");
			Assert.That (clone.GetType (), Is.EqualTo (attributeToClone.GetType ()), "type");
			if (clone.AvailabilityKind == AvailabilityKind.Introduced) {
				Assert.That (clone.Version, Is.Null, "Version");
			} else {
				Assert.That (clone.Version, Is.EqualTo (Xamarin.SdkVersions.GetMinVersion (targetPlatform.AsApplePlatform ())), "Version");
			}
		}

		class CloneCasesBuildVersionClass : IEnumerable {
			public IEnumerator GetEnumerator ()
			{
				var tvOsMin = Xamarin.SdkVersions.GetMinVersion (ApplePlatform.TVOS);
				tvOsMin = new Version (tvOsMin.Major, tvOsMin.Minor, tvOsMin.Build + 3);
				var iOSMin = Xamarin.SdkVersions.GetMinVersion (ApplePlatform.iOS);
				iOSMin = new Version (iOSMin.Major, iOSMin.Minor, iOSMin.Build + 3);
				yield return new object [] {
					new IntroducedAttribute (PlatformName.iOS, tvOsMin.Major, tvOsMin.Minor, tvOsMin.Build),
					PlatformName.TvOS,
				};
				yield return new object [] {
					new DeprecatedAttribute (PlatformName.MacCatalyst, iOSMin.Major, iOSMin.Minor, iOSMin.Build),
					PlatformName.iOS,
				};
			}
		}

		[TestCaseSource (typeof (CloneCasesBuildVersionClass))]
		public void CloneBuildVersion (AvailabilityBaseAttribute attributeToClone, PlatformName targetPlatform)
		{
			var clone = AttributeFactory.CloneFromOtherPlatform (attributeToClone, targetPlatform);
			Assert.That (clone.Platform, Is.EqualTo (targetPlatform), "platform");
			Assert.That (clone.Message, Is.EqualTo (attributeToClone.Message), "message");
			Assert.That (clone.GetType (), Is.EqualTo (attributeToClone.GetType ()), "type");
			Assert.That (clone.Version, Is.EqualTo (attributeToClone.Version));
		}

	}
}
