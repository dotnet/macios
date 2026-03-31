// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__TVOS__ && !__MACOS__

using NetworkExtension;

namespace MonoTouchFixtures.NetworkExtension {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NEHotspotEapSettingsTest {

		[Test]
		public void SupportedEapTypes_Default ()
		{
			using var settings = new NEHotspotEapSettings ();
			var types = settings.SupportedEapTypes;
			Assert.IsNotNull (types, "Default value should not be null");
			Assert.That (types.Length, Is.EqualTo (0), "Default length");
		}

		[Test]
		public void SupportedEapTypes_Roundtrip ()
		{
			using var settings = new NEHotspotEapSettings ();
			var expected = new [] {
				NEHotspotConfigurationEapType.Tls,
				NEHotspotConfigurationEapType.Peap,
			};
			settings.SupportedEapTypes = expected;
			var actual = settings.SupportedEapTypes;
			Assert.IsNotNull (actual, "not null");
			Assert.That (actual.Length, Is.EqualTo (2), "Length");
			Assert.That (actual [0], Is.EqualTo (NEHotspotConfigurationEapType.Tls), "[0]");
			Assert.That (actual [1], Is.EqualTo (NEHotspotConfigurationEapType.Peap), "[1]");
		}

		[Test]
		public void SupportedEapTypes_AllValues ()
		{
			using var settings = new NEHotspotEapSettings ();
			var all = new [] {
				NEHotspotConfigurationEapType.Tls,
				NEHotspotConfigurationEapType.Ttls,
				NEHotspotConfigurationEapType.Peap,
				NEHotspotConfigurationEapType.Fast,
			};
			settings.SupportedEapTypes = all;
			var actual = settings.SupportedEapTypes;
			Assert.That (actual.Length, Is.EqualTo (4), "Length");
			Assert.That (actual, Is.EqualTo (all), "All values roundtrip");
		}

		[Test]
		public void SupportedEapTypes_SetNull ()
		{
			using var settings = new NEHotspotEapSettings ();
			Assert.Throws<ArgumentNullException> (() => settings.SupportedEapTypes = null!, "null should throw");
		}
	}
}

#endif // !__TVOS__ && !__MACOS__
