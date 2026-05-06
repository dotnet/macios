using System.Collections.Generic;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Hardware;

namespace Xharness.Jenkins {

	class TestData {
		public string? Variation;
		public KnownIssue? KnownFailure;
		public bool? Ignored;
		public IEnumerable<IDevice>? Candidates;
		public string? RuntimeIdentifier;
		public string? TestVariation;
	}
}
