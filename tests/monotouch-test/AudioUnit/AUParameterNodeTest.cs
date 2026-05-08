//
// Unit tests for AUParameterNode
//
// Authors:
//	Oleg Demchenko (oleg.demchenko@xamarin.com)
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System.Threading;

using AudioUnit;

namespace monotouchtest {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AUParameterNodeTest {
		[Test]
		public void CreateTokenByAddingParameterRecordingObserver ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const ulong address = 0;
			const float newValue = 10f;

			bool recordingObserverInvoked = false;
			var completion = new ManualResetEvent (false);

			using (var parameter = CreateAUParameter ()) {
				using (var tree = AUParameterTree.CreateTree (new AUParameterNode [] { parameter })) {
					Exception ex = null;
					var recordingObserver = tree.CreateTokenByAddingParameterRecordingObserver ((nint numberOfEvents, ref AURecordedParameterEvent events) => {
						try {
							Assert.That (numberOfEvents == 1, Is.True, $"Number of events was wrong. Expected {1} but was {numberOfEvents}");

							Assert.That (events.Address == address, Is.True, $"Address was wrong. Expected {address} but was {events.Address}");

							Assert.That (events.Value == newValue, Is.True, $"Value was wrong. Expected {newValue} but was {events.Value}");

							recordingObserverInvoked = true;
						} catch (Exception e) {
							ex = e;
						} finally {
							completion.Set ();
						}
					});

					Assert.That (recordingObserver.ObserverToken != IntPtr.Zero, Is.True, "TokenByAddingParameterRecordingObserver return zero pointer for recording observer.");
					parameter.Value = newValue;

					completion.WaitOne (TimeSpan.FromSeconds (1));
					Assert.That (ex, Is.Null, "Exceptions");
					Assert.That (recordingObserverInvoked, Is.True, "Recording observer was not invoked when parameter value was changed.");
				}
			}
		}

		[Test]
		public void RemoveParameterObserver ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const float newValue = 10f;

			bool recordingObserverInvoked = false;
			var completion = new ManualResetEvent (false);

			using (var parameter = CreateAUParameter ()) {
				using (var tree = AUParameterTree.CreateTree (new AUParameterNode [] { parameter })) {
					var recordingObserver = tree.CreateTokenByAddingParameterRecordingObserver ((nint numberOfEvents, ref AURecordedParameterEvent events) => {
						recordingObserverInvoked = true;
						completion.Set ();
					});

					tree.RemoveParameterObserver (recordingObserver);

					Assert.That (recordingObserver.ObserverToken != IntPtr.Zero, Is.True, "TokenByAddingParameterRecordingObserver return zero pointer for recording observer.");
					parameter.Value = newValue;

					completion.WaitOne (TimeSpan.FromSeconds (1));
					Assert.That (recordingObserverInvoked, Is.False, "Recording observer was invoked however observer it should be removed already.");
				}
			}
		}

		[Test]
		public void ImplementorStringFromValueCallback ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const float floatValue = 10f;
			const string expectedStringValue = "10";

			bool implementorCallbackInvoked = false;
			Exception ex = null;

			using (var parameter = CreateAUParameter ()) {
				parameter.ImplementorStringFromValueCallback = new AUImplementorStringFromValueCallback ((AUParameter param, ref float? value) => {
					try {
						Assert.That (floatValue == value.Value, Is.True, $"Passed float value was incorrect. Expected {floatValue} but was {value}");

						Assert.That (param.Identifier == parameter.Identifier, Is.True, $"Passed AUParameter was incorrect. Expected {parameter.Identifier} but was {param.Identifier}");
					} catch (Exception e) {
						ex = e;
					} finally {
						implementorCallbackInvoked = true;
					}
					return (NSString) value.ToString ();
				});

				Assert.That (ex, Is.Null, "Exception");

				var str = parameter.GetString (floatValue);

				Assert.That (implementorCallbackInvoked, Is.True, "StringValueFrom callback was not invoked.");
				Assert.That (str == expectedStringValue, Is.True, $"String doesn't match. Expected {expectedStringValue}, actual {str}");
			}
		}

		[Test]
		public void ImplementorValueFromStringCallback ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const float expectedValue = 10f;
			const string stringValue = "10";

			bool implementorCallbackInvoked = false;

			using (var parameter = CreateAUParameter ()) {
				parameter.ImplementorValueFromStringCallback = new AUImplementorValueFromStringCallback ((param, str) => {
					Assert.That (str == stringValue, Is.True, $"Passed string value was incorrect. Expected {stringValue} but was {str}");

					Assert.That (param.Identifier == parameter.Identifier, Is.True, $"Passed AUParameter was incorrect. Expected {parameter.Identifier} but was {param.Identifier}");

					implementorCallbackInvoked = true;
					return Single.Parse (str);
				});

				var value = parameter.GetValue (stringValue);

				Assert.That (implementorCallbackInvoked, Is.True, "ValueFromString callback was not invoked.");
				Assert.That (Math.Abs (value - expectedValue) > float.Epsilon, Is.False, $"Values doesn't match. Expected {expectedValue}, actual {value}");
			}
		}

		// TODO: Test temporary ignored.
		// Reason: ImplementorDisplayNameWithLengthCallback never invoked when user requests truncated version of node's name.
		// Objc/Swift code demosntrates the same behavior.
		// Waiting for comments/fix from Apple.
		[Test, Ignore ("ImplementorDisplayNameWithLengthCallback never invoked when user requests truncated version of node's name.")]
		public void ImplementorDisplayNameWithLengthCallback ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);

			const string displayName = "Resonance";
			const int length = 5;

			bool implementorCallbackInvoked = false;
			string expectedTruncatedName = displayName.Substring (0, length);

			using (var parameter = CreateAUParameter ()) {
				parameter.ImplementorDisplayNameWithLengthCallback = new AUImplementorDisplayNameWithLengthCallback ((node, desiredLength) => {
					Assert.That ((nint) desiredLength, Is.EqualTo ((nint) length), "Passed length value is incorrect.");
					Assert.That (node.Identifier == parameter.Identifier, Is.True, $"Passed AUParameterNode was incorrect. Expected {parameter.Identifier} but was {node.Identifier}");

					implementorCallbackInvoked = true;
					return node.DisplayName.Substring (0, (int) desiredLength);
				});

				var s = parameter.GetDisplayName (length);
				Assert.That (implementorCallbackInvoked, Is.True, "Display name callback was not invoked.");
				Assert.That (expectedTruncatedName == s, Is.True, $"Truncated node display name was incorrect. Expected {expectedTruncatedName} but was {s}");
			}
		}

		static AUParameter CreateAUParameter ()
		{
			return AUParameterTree.CreateParameter ("resonance", "Resonance", 0, -20, 20, AudioUnitParameterUnit.Decibels, null, (AudioUnitParameterOptions) 0, null, null);
		}
	}
}
