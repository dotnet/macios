// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !MONOMAC

using Foundation;
using StoreKit;

namespace MonoTouchFixtures.StoreKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StoreProductParametersTest {

		[Test]
		public void ITunesItemIdentifier_64BitRoundtrip ()
		{
			const long identifier = 2147483648L;

			var withCtor = new StoreProductParameters (identifier);
			var withSetter = new StoreProductParameters {
				ITunesItemIdentifierLong = identifier,
			};

			Assert.That (withCtor.ITunesItemIdentifierLong, Is.EqualTo (identifier), "Ctor");
			Assert.That (withSetter.ITunesItemIdentifierLong, Is.EqualTo (identifier), "Setter");
			Assert.That (((NSNumber) withSetter.Dictionary [SKStoreProductParameterKey.ITunesItemIdentifier]).Int64Value, Is.EqualTo (identifier), "Dictionary");
		}

#if !XAMCORE_5_0
#pragma warning disable 618
		[Test]
		public void ITunesItemIdentifier_LegacyRoundtrip ()
		{
			const int identifier = 123456789;

			var parameters = new StoreProductParameters {
				ITunesItemIdentifier = identifier,
			};

			Assert.That (parameters.ITunesItemIdentifier, Is.EqualTo (identifier), "Legacy");
			Assert.That (parameters.ITunesItemIdentifierLong, Is.EqualTo (identifier), "Long");
		}
#pragma warning restore 618
#endif
	}
}

#endif
