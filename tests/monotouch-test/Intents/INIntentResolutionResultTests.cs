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
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCallRecordTypeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCallRecordTypeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCallRecordTypeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INDateComponentsRangeResolutionResultPropertyTest ()
		{
			using (var needsValue = INDateComponentsRangeResolutionResult.NeedsValue)
			using (var notRequired = INDateComponentsRangeResolutionResult.NotRequired)
			using (var unsupported = INDateComponentsRangeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INDateComponentsRangeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INDateComponentsRangeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INDateComponentsRangeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INMessageAttributeOptionsResolutionResultPropertyTest ()
		{
			using (var needsValue = INMessageAttributeOptionsResolutionResult.NeedsValue)
			using (var notRequired = INMessageAttributeOptionsResolutionResult.NotRequired)
			using (var unsupported = INMessageAttributeOptionsResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeOptionsResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeOptionsResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeOptionsResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INMessageAttributeResolutionResultPropertyTest ()
		{
			using (var needsValue = INMessageAttributeResolutionResult.NeedsValue)
			using (var notRequired = INMessageAttributeResolutionResult.NotRequired)
			using (var unsupported = INMessageAttributeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INMessageAttributeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INPersonResolutionResultPropertyTest ()
		{
			using (var needsValue = INPersonResolutionResult.NeedsValue)
			using (var notRequired = INPersonResolutionResult.NotRequired)
			using (var unsupported = INPersonResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INPersonResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INPersonResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INPersonResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INPlacemarkResolutionResultPropertyTest ()
		{
			using (var needsValue = INPlacemarkResolutionResult.NeedsValue)
			using (var notRequired = INPlacemarkResolutionResult.NotRequired)
			using (var unsupported = INPlacemarkResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INPlacemarkResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INPlacemarkResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INPlacemarkResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INSpeakableStringResolutionResultPropertyTest ()
		{
			using (var needsValue = INSpeakableStringResolutionResult.NeedsValue)
			using (var notRequired = INSpeakableStringResolutionResult.NotRequired)
			using (var unsupported = INSpeakableStringResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INSpeakableStringResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INSpeakableStringResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INSpeakableStringResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INStringResolutionResultPropertyTest ()
		{
			using (var needsValue = INStringResolutionResult.NeedsValue)
			using (var notRequired = INStringResolutionResult.NotRequired)
			using (var unsupported = INStringResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INStringResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INStringResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INStringResolutionResult), unsupported, "Unsupported");
			}
		}

#if !MONOMAC // iOS only Result types
		[Test]
		public void INBooleanResolutionResultPropertyTest ()
		{
			using (var needsValue = INBooleanResolutionResult.NeedsValue)
			using (var notRequired = INBooleanResolutionResult.NotRequired)
			using (var unsupported = INBooleanResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INBooleanResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INBooleanResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INBooleanResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCarAirCirculationModeResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarAirCirculationModeResolutionResult.NeedsValue)
			using (var notRequired = INCarAirCirculationModeResolutionResult.NotRequired)
			using (var unsupported = INCarAirCirculationModeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCarAirCirculationModeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCarAirCirculationModeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCarAirCirculationModeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCarAudioSourceResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarAudioSourceResolutionResult.NeedsValue)
			using (var notRequired = INCarAudioSourceResolutionResult.NotRequired)
			using (var unsupported = INCarAudioSourceResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCarAudioSourceResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCarAudioSourceResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCarAudioSourceResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCarDefrosterResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarDefrosterResolutionResult.NeedsValue)
			using (var notRequired = INCarDefrosterResolutionResult.NotRequired)
			using (var unsupported = INCarDefrosterResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCarDefrosterResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCarDefrosterResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCarDefrosterResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCarSeatResolutionResultPropertyTest ()
		{
			using (var needsValue = INCarSeatResolutionResult.NeedsValue)
			using (var notRequired = INCarSeatResolutionResult.NotRequired)
			using (var unsupported = INCarSeatResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCarSeatResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCarSeatResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCarSeatResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCurrencyAmountResolutionResultPropertyTest ()
		{
			using (var needsValue = INCurrencyAmountResolutionResult.NeedsValue)
			using (var notRequired = INCurrencyAmountResolutionResult.NotRequired)
			using (var unsupported = INCurrencyAmountResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCurrencyAmountResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCurrencyAmountResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCurrencyAmountResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INDoubleResolutionResultPropertyTest ()
		{
			using (var needsValue = INDoubleResolutionResult.NeedsValue)
			using (var notRequired = INDoubleResolutionResult.NotRequired)
			using (var unsupported = INDoubleResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INDoubleResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INDoubleResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INDoubleResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INDateComponentsResolutionResultPropertyTest ()
		{
			using (var needsValue = INDateComponentsResolutionResult.NeedsValue)
			using (var notRequired = INDateComponentsResolutionResult.NotRequired)
			using (var unsupported = INDateComponentsResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INDateComponentsResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INDateComponentsResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INDateComponentsResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INIntegerResolutionResultPropertyTest ()
		{
			using (var needsValue = INIntegerResolutionResult.NeedsValue)
			using (var notRequired = INIntegerResolutionResult.NotRequired)
			using (var unsupported = INIntegerResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INIntegerResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INIntegerResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INIntegerResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INRadioTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INRadioTypeResolutionResult.NeedsValue)
			using (var notRequired = INRadioTypeResolutionResult.NotRequired)
			using (var unsupported = INRadioTypeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INRadioTypeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INRadioTypeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INRadioTypeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INRelativeReferenceResolutionResultPropertyTest ()
		{
			using (var needsValue = INRelativeReferenceResolutionResult.NeedsValue)
			using (var notRequired = INRelativeReferenceResolutionResult.NotRequired)
			using (var unsupported = INRelativeReferenceResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INRelativeReferenceResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INRelativeReferenceResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INRelativeReferenceResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INRelativeSettingResolutionResultPropertyTest ()
		{
			using (var needsValue = INRelativeSettingResolutionResult.NeedsValue)
			using (var notRequired = INRelativeSettingResolutionResult.NotRequired)
			using (var unsupported = INRelativeSettingResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INRelativeSettingResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INRelativeSettingResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INRelativeSettingResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INRestaurantGuestResolutionResultPropertyTest ()
		{
			using (var needsValue = INRestaurantGuestResolutionResult.NeedsValue)
			using (var notRequired = INRestaurantGuestResolutionResult.NotRequired)
			using (var unsupported = INRestaurantGuestResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INRestaurantGuestResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INRestaurantGuestResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INRestaurantGuestResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INRestaurantResolutionResultPropertyTest ()
		{
			using (var needsValue = INRestaurantResolutionResult.NeedsValue)
			using (var notRequired = INRestaurantResolutionResult.NotRequired)
			using (var unsupported = INRestaurantResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INRestaurantResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INRestaurantResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INRestaurantResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INTemperatureResolutionResultPropertyTest ()
		{
			using (var needsValue = INTemperatureResolutionResult.NeedsValue)
			using (var notRequired = INTemperatureResolutionResult.NotRequired)
			using (var unsupported = INTemperatureResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INTemperatureResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INTemperatureResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INTemperatureResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INWorkoutGoalUnitTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INWorkoutGoalUnitTypeResolutionResult.NeedsValue)
			using (var notRequired = INWorkoutGoalUnitTypeResolutionResult.NotRequired)
			using (var unsupported = INWorkoutGoalUnitTypeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INWorkoutGoalUnitTypeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INWorkoutLocationTypeResolutionResultPropertyTest ()
		{
			using (var needsValue = INWorkoutLocationTypeResolutionResult.NeedsValue)
			using (var notRequired = INWorkoutLocationTypeResolutionResult.NotRequired)
			using (var unsupported = INWorkoutLocationTypeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INWorkoutLocationTypeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INWorkoutLocationTypeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INWorkoutLocationTypeResolutionResult), unsupported, "Unsupported");
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
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INBillPayeeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INBillPayeeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INBillPayeeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INBillTypeResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INBillTypeResolutionResult.NeedsValue)
			using (var notRequired = INBillTypeResolutionResult.NotRequired)
			using (var unsupported = INBillTypeResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INBillTypeResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INBillTypeResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INBillTypeResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INCarSignalOptionsResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INCarSignalOptionsResolutionResult.NeedsValue)
			using (var notRequired = INCarSignalOptionsResolutionResult.NotRequired)
			using (var unsupported = INCarSignalOptionsResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INCarSignalOptionsResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INCarSignalOptionsResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INCarSignalOptionsResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INPaymentAmountResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentAmountResolutionResult.NeedsValue)
			using (var notRequired = INPaymentAmountResolutionResult.NotRequired)
			using (var unsupported = INPaymentAmountResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INPaymentAmountResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INPaymentAmountResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INPaymentAmountResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INPaymentStatusResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentStatusResolutionResult.NeedsValue)
			using (var notRequired = INPaymentStatusResolutionResult.NotRequired)
			using (var unsupported = INPaymentStatusResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INPaymentStatusResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INPaymentStatusResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INPaymentStatusResolutionResult), unsupported, "Unsupported");
			}
		}

		[Test]
		public void INPaymentAccountResolutionResultPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (8, 3);

			using (var needsValue = INPaymentAccountResolutionResult.NeedsValue)
			using (var notRequired = INPaymentAccountResolutionResult.NotRequired)
			using (var unsupported = INPaymentAccountResolutionResult.Unsupported) {
				ClassicAssert.NotNull (needsValue, "NeedsValue Null");
				ClassicAssert.NotNull (notRequired, "NotRequired Null");
				ClassicAssert.NotNull (unsupported, "Unsupported Null");

				ClassicAssert.IsInstanceOf (typeof (INPaymentAccountResolutionResult), needsValue, "NeedsValue");
				ClassicAssert.IsInstanceOf (typeof (INPaymentAccountResolutionResult), notRequired, "NotRequired");
				ClassicAssert.IsInstanceOf (typeof (INPaymentAccountResolutionResult), unsupported, "Unsupported");
			}
		}
	}
}
#endif
