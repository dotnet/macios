//
// Unit tests for CPMessageListItem
//
// Authors:
//	Whitney Schmidt <whschm@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

#if HAS_CARPLAY

using CarPlay;
using Xamarin.Utils;

namespace MonoTouchFixtures.CarPlay {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CPMessageListItemTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
		}

		[Test]
		public void InitUsingConversationIdentifier ()
		{
			var leadingItemConfig = new CPMessageListItemLeadingConfiguration (new CPMessageLeadingItem (), null, false);
			var trailingItemConfig = new CPMessageListItemTrailingConfiguration (new CPMessageTrailingItem (), null);
			CPMessageListItem listItem = new CPMessageListItem ("convoId", "text", leadingItemConfig, trailingItemConfig, "detailText", "trailingText", CPMessageListItemType.Identifier);

			ClassicAssert.NotNull (listItem, "CPMessageListItem not be null.");
			ClassicAssert.AreEqual (listItem.Text, "text");
			ClassicAssert.AreEqual (listItem.ConversationIdentifier, "convoId");
			ClassicAssert.AreSame (listItem.LeadingConfiguration, leadingItemConfig);
			ClassicAssert.AreSame (listItem.TrailingConfiguration, trailingItemConfig);
			ClassicAssert.AreEqual (listItem.DetailText, "detailText");
			ClassicAssert.AreEqual (listItem.TrailingText, "trailingText");
		}

		[Test]
		public void InitUsingFullName ()
		{
			var leadingItemConfig = new CPMessageListItemLeadingConfiguration (new CPMessageLeadingItem (), null, false);
			var trailingItemConfig = new CPMessageListItemTrailingConfiguration (new CPMessageTrailingItem (), null);
			CPMessageListItem listItem = new CPMessageListItem ("fullName", "phoneOrEmail", leadingItemConfig, trailingItemConfig, "detailText", "trailingText", CPMessageListItemType.FullName);

			ClassicAssert.NotNull (listItem, "CPMessageListItem not be null.");
			ClassicAssert.AreEqual (listItem.Text, "fullName");
			ClassicAssert.AreEqual (listItem.PhoneOrEmailAddress, "phoneOrEmail");
			ClassicAssert.AreSame (listItem.LeadingConfiguration, leadingItemConfig);
			ClassicAssert.AreSame (listItem.TrailingConfiguration, trailingItemConfig);
			ClassicAssert.AreEqual (listItem.DetailText, "detailText");
			ClassicAssert.AreEqual (listItem.TrailingText, "trailingText");
		}

	}
}
#endif // HAS_CARPLAY
