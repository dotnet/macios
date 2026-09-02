//
// Unit tests for NSDimension
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DimensionTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void BaseUnit ()
		{
			Assert.Throws<NotImplementedException> (() => { var bu = NSDimension.BaseUnit; }, "Base type must implement this");
		}

		[Test]
		public void NSUnitAcceleration_BaseUnit ()
		{
			using (var bu = NSUnitAcceleration.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitAcceleration)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("m/s²"), "Symbol");
			}
		}

		[Test]
		public void NSUnitAngle_BaseUnit ()
		{
			using (var bu = NSUnitAngle.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitAngle)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("°"), "Symbol");
			}
		}

		[Test]
		public void NSUnitArea_BaseUnit ()
		{
			using (var bu = NSUnitArea.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitArea)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("m²"), "Symbol");
			}
		}

		[Test]
		public void NSUnitConcentrationMass_BaseUnit ()
		{
			using (var bu = NSUnitConcentrationMass.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitConcentrationMass)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("g/L"), "Symbol");
			}
		}

		[Test]
		public void NSUnitDispersion_BaseUnit ()
		{
			using (var bu = NSUnitDispersion.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitDispersion)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("ppm"), "Symbol");
			}
		}

		[Test]
		public void NSUnitDuration_BaseUnit ()
		{
			using (var bu = NSUnitDuration.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitDuration)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("s"), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricCharge_BaseUnit ()
		{
			using (var bu = NSUnitElectricCharge.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitElectricCharge)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("C"), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricCurrent_BaseUnit ()
		{
			using (var bu = NSUnitElectricCurrent.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitElectricCurrent)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("A"), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricPotentialDifference_BaseUnit ()
		{
			using (var bu = NSUnitElectricPotentialDifference.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitElectricPotentialDifference)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("V"), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricResistance_BaseUnit ()
		{
			using (var bu = NSUnitElectricResistance.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitElectricResistance)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("Ω"), "Symbol");
			}
		}

		[Test]
		public void NSUnitEnergy_BaseUnit ()
		{
			using (var bu = NSUnitEnergy.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitEnergy)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("J"), "Symbol");
			}
		}

		[Test]
		public void NSUnitFrequency_BaseUnit ()
		{
			using (var bu = NSUnitFrequency.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitFrequency)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("Hz"), "Symbol");
			}
		}

		[Test]
		public void NSUnitFuelEfficiency_BaseUnit ()
		{
			using (var bu = NSUnitFuelEfficiency.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitFuelEfficiency)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("L/100km"), "Symbol");
			}
		}

		[Test]
		public void NSUnitIlluminance_BaseUnit ()
		{
			using (var bu = NSUnitIlluminance.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitIlluminance)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("lx"), "Symbol");
			}
		}

		[Test]
		public void NSUnitLength_BaseUnit ()
		{
			using (var bu = NSUnitLength.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitLength)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("m"), "Symbol");
			}
		}

		[Test]
		public void NSUnitMass_BaseUnit ()
		{
			using (var bu = NSUnitMass.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitMass)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("kg"), "Symbol");
			}
		}

		[Test]
		public void NSUnitPower_BaseUnit ()
		{
			using (var bu = NSUnitPower.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitPower)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("W"), "Symbol");
			}
		}

		[Test]
		public void NSUnitPressure_BaseUnit ()
		{
			using (var bu = NSUnitPressure.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitPressure)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("N/m²"), "Symbol");
			}
		}

		[Test]
		public void NSUnitSpeed_BaseUnit ()
		{
			using (var bu = NSUnitSpeed.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitSpeed)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("m/s"), "Symbol");
			}
		}

		[Test]
		public void NSUnitTemperature_BaseUnit ()
		{
			using (var bu = NSUnitTemperature.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitTemperature)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("K"), "Symbol");
			}
		}

		[Test]
		public void NSUnitVolume_BaseUnit ()
		{
			using (var bu = NSUnitVolume.BaseUnit) {
				Assert.That (bu, Is.InstanceOf (typeof (NSUnitVolume)), "type");
				Assert.That (bu.Symbol, Is.EqualTo ("L"), "Symbol");
			}
		}
	}
}
