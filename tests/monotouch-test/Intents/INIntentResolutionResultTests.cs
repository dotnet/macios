//
// Unit tests for INIntentResolutionResult
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//	
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using Intents;

namespace MonoTouchFixtures.Intents {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class INIntentResolutionResultTests {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void INIntentResolutionResultIsAbstractTest ()
		{
			Assert.Throws<NotImplementedException> (() => { var needsValue = INIntentResolutionResult.NeedsValue; }, "Base type must implement NeedsValue");
			Assert.Throws<NotImplementedException> (() => { var notRequired = INIntentResolutionResult.NotRequired; }, "Base type must implement NotRequired");
			Assert.Throws<NotImplementedException> (() => { var unsupported = INIntentResolutionResult.Unsupported; }, "Base type must implement Unsupported");
		}

		[Test]
		public void INCallRecordTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INCallRecordTypeResolutionResult.NeedsValue)
			using (var notRequired = INCallRecordTypeResolutionResult.NotRequired)
			using (var unsupported = INCallRecordTypeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCallRecordTypeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCallRecordTypeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCallRecordTypeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INDateComponentsRangeResolutionResultPropertyTest ()
		{
			using (var needsValue = INDateComponentsRangeResolutionResult.NeedsValue)
			using (var notRequired = INDateComponentsRangeResolutionResult.NotRequired)
			using (var unsupported = INDateComponentsRangeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INDateComponentsRangeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INDateComponentsRangeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INDateComponentsRangeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INMessageAttributeOptionsResolutionResultPropertyTest ()
		{
			using (var needsValue = INMessageAttributeOptionsResolutionResult.NeedsValue)
			using (var notRequired = INMessageAttributeOptionsResolutionResult.NotRequired)
			using (var unsupported = INMessageAttributeOptionsResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INMessageAttributeOptionsResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INMessageAttributeOptionsResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INMessageAttributeOptionsResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INMessageAttributeResolutionResultPropertyTest ()
		{
			using (var needsValue = INMessageAttributeResolutionResult.NeedsValue)
			using (var notRequired = INMessageAttributeResolutionResult.NotRequired)
			using (var unsupported = INMessageAttributeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INMessageAttributeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INMessageAttributeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INMessageAttributeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INPersonResolutionResultPropertyTest ()
		{
			using (var needsValue = INPersonResolutionResult.NeedsValue)
			using (var notRequired = INPersonResolutionResult.NotRequired)
			using (var unsupported = INPersonResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INPersonResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INPersonResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INPersonResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INPlacemarkResolutionResultPropertyTest ()
		{
			using (var needsValue = INPlacemarkResolutionResult.NeedsValue)
			using (var notRequired = INPlacemarkResolutionResult.NotRequired)
			using (var unsupported = INPlacemarkResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INPlacemarkResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INPlacemarkResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INPlacemarkResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INSpeakableStringResolutionResultPropertyTest ()
		{
			using (var needsValue = INSpeakableStringResolutionResult.NeedsValue)
			using (var notRequired = INSpeakableStringResolutionResult.NotRequired)
			using (var unsupported = INSpeakableStringResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INSpeakableStringResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INSpeakableStringResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INSpeakableStringResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INStringResolutionResultPropertyTest ()
		{
			using (var needsValue = INStringResolutionResult.NeedsValue)
			using (var notRequired = INStringResolutionResult.NotRequired)
			using (var unsupported = INStringResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INStringResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INStringResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INStringResolutionResult)), "Unsupported");
			}
		}

#if !MONOMAC // iOS only Result types
		[Test]
		public void INBooleanResolutionResultPropertyTest ()
		{
			using (var needsValue = INBooleanResolutionResult.NeedsValue)
			using (var notRequired = INBooleanResolutionResult.NotRequired)
			using (var unsupported = INBooleanResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INBooleanResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INBooleanResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INBooleanResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCarAirCirculationModeResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarAirCirculationModeResolutionResult.NeedsValue)
			using (var notRequired = INCarAirCirculationModeResolutionResult.NotRequired)
			using (var unsupported = INCarAirCirculationModeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCarAirCirculationModeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCarAirCirculationModeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCarAirCirculationModeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCarAudioSourceResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarAudioSourceResolutionResult.NeedsValue)
			using (var notRequired = INCarAudioSourceResolutionResult.NotRequired)
			using (var unsupported = INCarAudioSourceResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCarAudioSourceResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCarAudioSourceResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCarAudioSourceResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCarDefrosterResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarDefrosterResolutionResult.NeedsValue)
			using (var notRequired = INCarDefrosterResolutionResult.NotRequired)
			using (var unsupported = INCarDefrosterResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCarDefrosterResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCarDefrosterResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCarDefrosterResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCarSeatResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarSeatResolutionResult.NeedsValue)
			using (var notRequired = INCarSeatResolutionResult.NotRequired)
			using (var unsupported = INCarSeatResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCarSeatResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCarSeatResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCarSeatResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCurrencyAmountResolutionResultPropertyTest ()
		{
			using (var needsValue = INCurrencyAmountResolutionResult.NeedsValue)
			using (var notRequired = INCurrencyAmountResolutionResult.NotRequired)
			using (var unsupported = INCurrencyAmountResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCurrencyAmountResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCurrencyAmountResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCurrencyAmountResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INDoubleResolutionResultPropertyTest ()
		{
			using (var needsValue = INDoubleResolutionResult.NeedsValue)
			using (var notRequired = INDoubleResolutionResult.NotRequired)
			using (var unsupported = INDoubleResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INDoubleResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INDoubleResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INDoubleResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INDateComponentsResolutionResultPropertyTest ()
		{
			using (var needsValue = INDateComponentsResolutionResult.NeedsValue)
			using (var notRequired = INDateComponentsResolutionResult.NotRequired)
			using (var unsupported = INDateComponentsResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INDateComponentsResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INDateComponentsResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INDateComponentsResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INIntegerResolutionResultPropertyTest ()
		{
			using (var needsValue = INIntegerResolutionResult.NeedsValue)
			using (var notRequired = INIntegerResolutionResult.NotRequired)
			using (var unsupported = INIntegerResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INIntegerResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INIntegerResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INIntegerResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INRadioTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INRadioTypeResolutionResult.NeedsValue)
			using (var notRequired = INRadioTypeResolutionResult.NotRequired)
			using (var unsupported = INRadioTypeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INRadioTypeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INRadioTypeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INRadioTypeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INRelativeReferenceResolutionResultPropertyTest ()
		{
			using (var needsValue = INRelativeReferenceResolutionResult.NeedsValue)
			using (var notRequired = INRelativeReferenceResolutionResult.NotRequired)
			using (var unsupported = INRelativeReferenceResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INRelativeReferenceResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INRelativeReferenceResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INRelativeReferenceResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INRelativeSettingResolutionResultPropertyTest ()
		{
			using (var needsValue = INRelativeSettingResolutionResult.NeedsValue)
			using (var notRequired = INRelativeSettingResolutionResult.NotRequired)
			using (var unsupported = INRelativeSettingResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INRelativeSettingResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INRelativeSettingResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INRelativeSettingResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INRestaurantGuestResolutionResultPropertyTest ()
		{
			using (var needsValue = INRestaurantGuestResolutionResult.NeedsValue)
			using (var notRequired = INRestaurantGuestResolutionResult.NotRequired)
			using (var unsupported = INRestaurantGuestResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INRestaurantGuestResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INRestaurantGuestResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INRestaurantGuestResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INRestaurantResolutionResultPropertyTest ()
		{
			using (var needsValue = INRestaurantResolutionResult.NeedsValue)
			using (var notRequired = INRestaurantResolutionResult.NotRequired)
			using (var unsupported = INRestaurantResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INRestaurantResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INRestaurantResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INRestaurantResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INTemperatureResolutionResultPropertyTest ()
		{
			using (var needsValue = INTemperatureResolutionResult.NeedsValue)
			using (var notRequired = INTemperatureResolutionResult.NotRequired)
			using (var unsupported = INTemperatureResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INTemperatureResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INTemperatureResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INTemperatureResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INWorkoutGoalUnitTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INWorkoutGoalUnitTypeResolutionResult.NeedsValue)
			using (var notRequired = INWorkoutGoalUnitTypeResolutionResult.NotRequired)
			using (var unsupported = INWorkoutGoalUnitTypeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INWorkoutLocationTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INWorkoutLocationTypeResolutionResult.NeedsValue)
			using (var notRequired = INWorkoutLocationTypeResolutionResult.NotRequired)
			using (var unsupported = INWorkoutLocationTypeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INWorkoutLocationTypeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INWorkoutLocationTypeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INWorkoutLocationTypeResolutionResult)), "Unsupported");
			}
		}
#endif

		[Test]
		public void INBillPayeeResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INBillPayeeResolutionResult.NeedsValue)
			using (var notRequired = INBillPayeeResolutionResult.NotRequired)
			using (var unsupported = INBillPayeeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INBillPayeeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INBillPayeeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INBillPayeeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INBillTypeResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INBillTypeResolutionResult.NeedsValue)
			using (var notRequired = INBillTypeResolutionResult.NotRequired)
			using (var unsupported = INBillTypeResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INBillTypeResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INBillTypeResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INBillTypeResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INCarSignalOptionsResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INCarSignalOptionsResolutionResult.NeedsValue)
			using (var notRequired = INCarSignalOptionsResolutionResult.NotRequired)
			using (var unsupported = INCarSignalOptionsResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INCarSignalOptionsResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INCarSignalOptionsResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INCarSignalOptionsResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INPaymentAmountResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentAmountResolutionResult.NeedsValue)
			using (var notRequired = INPaymentAmountResolutionResult.NotRequired)
			using (var unsupported = INPaymentAmountResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INPaymentAmountResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INPaymentAmountResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INPaymentAmountResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INPaymentStatusResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentStatusResolutionResult.NeedsValue)
			using (var notRequired = INPaymentStatusResolutionResult.NotRequired)
			using (var unsupported = INPaymentStatusResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INPaymentStatusResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INPaymentStatusResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INPaymentStatusResolutionResult)), "Unsupported");
			}
		}

		[Test]
		public void INPaymentAccountResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentAccountResolutionResult.NeedsValue)
			using (var notRequired = INPaymentAccountResolutionResult.NotRequired)
			using (var unsupported = INPaymentAccountResolutionResult.Unsupported) {
				Assert.That (needsValue, Is.Not.Null, "NeedsValue Null");
				Assert.That (notRequired, Is.Not.Null, "NotRequired Null");
				Assert.That (unsupported, Is.Not.Null, "Unsupported Null");

				Assert.That (needsValue, Is.InstanceOf (typeof (INPaymentAccountResolutionResult)), "NeedsValue");
				Assert.That (notRequired, Is.InstanceOf (typeof (INPaymentAccountResolutionResult)), "NotRequired");
				Assert.That (unsupported, Is.InstanceOf (typeof (INPaymentAccountResolutionResult)), "Unsupported");
			}
		}
	}
}
#endif
