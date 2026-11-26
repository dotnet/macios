#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.DotNet.XHarness.Common.Logging;

namespace Xharness.Jenkins {

	public class TestSelection {
		TestLabel selection =
			TestLabel.None |
			TestLabel.Msbuild |
			TestLabel.Monotouch |
			TestLabel.DotnetTest;

		PlatformLabel platform =
			PlatformLabel.None |
			PlatformLabel.tvOS |
			PlatformLabel.iOS |
			PlatformLabel.iOSSimulator |
			PlatformLabel.MacCatalyst;

		public bool ForceExtensionBuildOnly { get; set; }

		public TestLabel SelectedTests {
			get => selection;
			set => selection = value;
		}

		public PlatformLabel SelectedPlatforms => platform;

		public void SetEnabled (TestLabel label, bool enable)
		{
			if (enable) {
				selection |= label;
			} else {
				selection &= ~label;
			}
		}

		public void SetEnabled (PlatformLabel label, bool enable)
		{
			if (enable) {
				platform |= label;
			} else {
				platform &= ~label;
			}
		}

		public void SetEnabled (string label, bool value)
		{
			// there are two possible cases, either we are setting a test label OR a platform
			if (label.TryGetLabel<TestLabel> (out var tLabel)) {
				SetEnabled (tLabel, value);
				// some labels overlap, not ideal, but is just a few
				switch (tLabel) {
				case TestLabel.All:
					SetEnabled (PlatformLabel.All, value);
					break;
				}
				return;
			}

			if (label.TryGetLabel (out PlatformLabel pLabel)) {
				SetEnabled (pLabel, value);
				return;
			}

			throw new InvalidOperationException ($"Unknown label '{label}'");
		}

		public bool IsEnabled (TestLabel label)
			=> selection.HasFlag (label);

		public bool IsEnabled (PlatformLabel label)
			=> platform.HasFlag (label);
	}
	/// <summary>
	/// Selects tests using command-line labels and enabled platforms.
	/// </summary>
	class TestSelector {

		#region private vars

		readonly Jenkins jenkins;

		ILog? MainLog => jenkins?.MainLog;
		Harness Harness => jenkins.Harness;

		#endregion

		public TestSelector (Jenkins jenkins)
		{
			this.jenkins = jenkins;
		}

		void DisableKnownFailingDeviceTests ()
		{
			// https://github.com/xamarin/maccore/issues/1008
			jenkins.ForceExtensionBuildOnly = true;
		}

		IEnumerable<string> GetHarnessLabels ()
		{
			if (Harness is not null && Harness.Labels.Any ()) {
				MainLog?.WriteLine ($"{Harness.Labels.Count} label(s) were passed on the command line.");
				return Harness.Labels;
			}

			MainLog?.WriteLine ($"No labels were passed on the command line.");
			return Array.Empty<string> ();
		}

		public void SelectTestsByLabel (HashSet<string> labels, TestSelection selection)
		{
			// when parsing the labels, order matters, for example:
			// 1. skip-all-tests, run-ios-tests
			// 2. run-ios-tests, skip-all-tests
			// the first example means that we start with all the tests disabled, and then we enable the ios tests
			// the second examples means that we have all tests disable because the skip-all-tests is last
			// In order to make this work we need to get the first label, decide if it is skip or run, then set it
			var labelSelectedTests = new Dictionary<string, bool> ();
			var regexp = new Regex ("(skip-|run-)(.*)(-tests)");
			foreach (var label in labels) {
				// use a regexp to parse the label, the possibilities are
				// skip-*-tests
				// run-*-tests
				// if we have a match, we decide if we skip or run and set the label accordingly
				var match = regexp.Match (label);
				if (!match.Success)
					continue;
				MainLog?.WriteLine ($"Label {label} matches regexp.");
				var matchedLabel = match.Groups [2].Value;
				var run = match.Groups [1].Value == "run-";
				MainLog?.WriteLine ($"Setting label {matchedLabel} to be enabled: {run}");
				if (labelSelectedTests.ContainsKey (matchedLabel)) {
					// if we find a label more than once, we or the value so that we always run the tests if there
					// was a single run-* match
					labelSelectedTests [matchedLabel] |= run;
				} else {
					labelSelectedTests.Add (matchedLabel, run);
				}
				MainLog?.WriteLine ($"Selected tests are {selection.SelectedTests}");
			}

			// special case to consider, since might set or remove all tests, we need to process it first
			if (labelSelectedTests.ContainsKey ("all")) {
				selection.SetEnabled ("all", labelSelectedTests ["all"]);
				if (labelSelectedTests ["all"])
					Harness.IncludeSystemPermissionTests = true;
				labelSelectedTests.Remove ("all");
			}

			foreach (var entry in labelSelectedTests) {
				selection.SetEnabled (entry.Key, entry.Value);
			}

			if (labelSelectedTests.TryGetValue ("system-permission", out var includeSystemPermissionTests))
				Harness.IncludeSystemPermissionTests = includeSystemPermissionTests;

			// old simulator tests is also a bit special:
			// - enabled by default if using a beta Xcode, otherwise disabled by default
			if (!labelSelectedTests.ContainsKey ("old-simulator") && jenkins.IsBetaXcode) {
				selection.SetEnabled (PlatformLabel.OldiOSSimulator, true);
				MainLog?.WriteLine ("Enabled 'old-simulator' tests because we're using a beta Xcode.");
			}
		}

		public void SelectTests (TestSelection selection)
		{
			var labels = new HashSet<string> (GetHarnessLabels ());
			SelectTestsByLabel (labels, selection);

			DisableKnownFailingDeviceTests ();

			if (!Harness.INCLUDE_IOS) {
				MainLog?.WriteLine ("The iOS build is disabled, so any iOS tests will be disabled as well.");
				selection.SetEnabled (PlatformLabel.iOS, false);
			}

			if (!Harness.INCLUDE_TVOS) {
				MainLog?.WriteLine ("The tvOS build is disabled, so any tvOS tests will be disabled as well.");
				selection.SetEnabled (PlatformLabel.tvOS, false);
			}

			if (!Harness.INCLUDE_MAC) {
				MainLog?.WriteLine ("The macOS build is disabled, so any macOS tests will be disabled as well.");
				selection.SetEnabled (PlatformLabel.Mac, false);
			}

			if (!Harness.INCLUDE_MACCATALYST) {
				MainLog?.WriteLine ("The Mac Catalyst build is disabled, so any Mac Catalyst tests will be disabled as well.");
				selection.SetEnabled (PlatformLabel.MacCatalyst, false);
			}

			MainLog?.WriteLine ($"Final test selection: tests: {selection.SelectedTests} platforms: {selection.SelectedPlatforms}");
		}
	}
}
