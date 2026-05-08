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
				Assert.That (typeof (NSUnitAcceleration), Is.InstanceOf (), bu, "type");
				Assert.That ("m/s²", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitAngle_BaseUnit ()
		{
			using (var bu = NSUnitAngle.BaseUnit) {
				Assert.That (typeof (NSUnitAngle), Is.InstanceOf (), bu, "type");
				Assert.That ("°", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitArea_BaseUnit ()
		{
			using (var bu = NSUnitArea.BaseUnit) {
				Assert.That (typeof (NSUnitArea), Is.InstanceOf (), bu, "type");
				Assert.That ("m²", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitConcentrationMass_BaseUnit ()
		{
			using (var bu = NSUnitConcentrationMass.BaseUnit) {
				Assert.That (typeof (NSUnitConcentrationMass), Is.InstanceOf (), bu, "type");
				Assert.That ("g/L", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitDispersion_BaseUnit ()
		{
			using (var bu = NSUnitDispersion.BaseUnit) {
				Assert.That (typeof (NSUnitDispersion), Is.InstanceOf (), bu, "type");
				Assert.That ("ppm", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitDuration_BaseUnit ()
		{
			using (var bu = NSUnitDuration.BaseUnit) {
				Assert.That (typeof (NSUnitDuration), Is.InstanceOf (), bu, "type");
				Assert.That ("s", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricCharge_BaseUnit ()
		{
			using (var bu = NSUnitElectricCharge.BaseUnit) {
				Assert.That (typeof (NSUnitElectricCharge), Is.InstanceOf (), bu, "type");
				Assert.That ("C", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricCurrent_BaseUnit ()
		{
			using (var bu = NSUnitElectricCurrent.BaseUnit) {
				Assert.That (typeof (NSUnitElectricCurrent), Is.InstanceOf (), bu, "type");
				Assert.That ("A", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricPotentialDifference_BaseUnit ()
		{
			using (var bu = NSUnitElectricPotentialDifference.BaseUnit) {
				Assert.That (typeof (NSUnitElectricPotentialDifference), Is.InstanceOf (), bu, "type");
				Assert.That ("V", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitElectricResistance_BaseUnit ()
		{
			using (var bu = NSUnitElectricResistance.BaseUnit) {
				Assert.That (typeof (NSUnitElectricResistance), Is.InstanceOf (), bu, "type");
				Assert.That ("Ω", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitEnergy_BaseUnit ()
		{
			using (var bu = NSUnitEnergy.BaseUnit) {
				Assert.That (typeof (NSUnitEnergy), Is.InstanceOf (), bu, "type");
				Assert.That ("J", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitFrequency_BaseUnit ()
		{
			using (var bu = NSUnitFrequency.BaseUnit) {
				Assert.That (typeof (NSUnitFrequency), Is.InstanceOf (), bu, "type");
				Assert.That ("Hz", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitFuelEfficiency_BaseUnit ()
		{
			using (var bu = NSUnitFuelEfficiency.BaseUnit) {
				Assert.That (typeof (NSUnitFuelEfficiency), Is.InstanceOf (), bu, "type");
				Assert.That ("L/100km", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitIlluminance_BaseUnit ()
		{
			using (var bu = NSUnitIlluminance.BaseUnit) {
				Assert.That (typeof (NSUnitIlluminance), Is.InstanceOf (), bu, "type");
				Assert.That ("lx", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitLength_BaseUnit ()
		{
			using (var bu = NSUnitLength.BaseUnit) {
				Assert.That (typeof (NSUnitLength), Is.InstanceOf (), bu, "type");
				Assert.That ("m", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitMass_BaseUnit ()
		{
			using (var bu = NSUnitMass.BaseUnit) {
				Assert.That (typeof (NSUnitMass), Is.InstanceOf (), bu, "type");
				Assert.That ("kg", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitPower_BaseUnit ()
		{
			using (var bu = NSUnitPower.BaseUnit) {
				Assert.That (typeof (NSUnitPower), Is.InstanceOf (), bu, "type");
				Assert.That ("W", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitPressure_BaseUnit ()
		{
			using (var bu = NSUnitPressure.BaseUnit) {
				Assert.That (typeof (NSUnitPressure), Is.InstanceOf (), bu, "type");
				Assert.That ("N/m²", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitSpeed_BaseUnit ()
		{
			using (var bu = NSUnitSpeed.BaseUnit) {
				Assert.That (typeof (NSUnitSpeed), Is.InstanceOf (), bu, "type");
				Assert.That ("m/s", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitTemperature_BaseUnit ()
		{
			using (var bu = NSUnitTemperature.BaseUnit) {
				Assert.That (typeof (NSUnitTemperature), Is.InstanceOf (), bu, "type");
				Assert.That ("K", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}

		[Test]
		public void NSUnitVolume_BaseUnit ()
		{
			using (var bu = NSUnitVolume.BaseUnit) {
				Assert.That (typeof (NSUnitVolume), Is.InstanceOf (), bu, "type");
				Assert.That ("L", Is.EqualTo (bu.Symbol), "Symbol");
			}
		}
	}
}
