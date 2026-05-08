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

				Assert.That (typeof (INCallRecordTypeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCallRecordTypeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCallRecordTypeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INDateComponentsRangeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INDateComponentsRangeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INDateComponentsRangeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INMessageAttributeOptionsResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INMessageAttributeOptionsResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INMessageAttributeOptionsResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INMessageAttributeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INMessageAttributeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INMessageAttributeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INPersonResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INPersonResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INPersonResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INPlacemarkResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INPlacemarkResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INPlacemarkResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INSpeakableStringResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INSpeakableStringResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INSpeakableStringResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INStringResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INStringResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INStringResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INBooleanResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INBooleanResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INBooleanResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCarAirCirculationModeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCarAirCirculationModeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCarAirCirculationModeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCarAudioSourceResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCarAudioSourceResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCarAudioSourceResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCarDefrosterResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCarDefrosterResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCarDefrosterResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCarSeatResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCarSeatResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCarSeatResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCurrencyAmountResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCurrencyAmountResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCurrencyAmountResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INDoubleResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INDoubleResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INDoubleResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INDateComponentsResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INDateComponentsResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INDateComponentsResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INIntegerResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INIntegerResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INIntegerResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INRadioTypeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INRadioTypeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INRadioTypeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INRelativeReferenceResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INRelativeReferenceResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INRelativeReferenceResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INRelativeSettingResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INRelativeSettingResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INRelativeSettingResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INRestaurantGuestResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INRestaurantGuestResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INRestaurantGuestResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INRestaurantResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INRestaurantResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INRestaurantResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INTemperatureResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INTemperatureResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INTemperatureResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INWorkoutGoalUnitTypeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INWorkoutGoalUnitTypeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INWorkoutGoalUnitTypeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INWorkoutLocationTypeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INWorkoutLocationTypeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INWorkoutLocationTypeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INBillPayeeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INBillPayeeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INBillPayeeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INBillTypeResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INBillTypeResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INBillTypeResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INCarSignalOptionsResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INCarSignalOptionsResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INCarSignalOptionsResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INPaymentAmountResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INPaymentAmountResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INPaymentAmountResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INPaymentStatusResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INPaymentStatusResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INPaymentStatusResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
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

				Assert.That (typeof (INPaymentAccountResolutionResult), Is.InstanceOf (), needsValue, "NeedsValue");
				Assert.That (typeof (INPaymentAccountResolutionResult), Is.InstanceOf (), notRequired, "NotRequired");
				Assert.That (typeof (INPaymentAccountResolutionResult), Is.InstanceOf (), unsupported, "Unsupported");
			}
		}
	}
}
#endif
