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

			Assert.That (listItem, Is.Not.Null, "CPMessageListItem not be null.");
			Assert.That (listItem.Text, Is.EqualTo ("text"));
			Assert.That (listItem.ConversationIdentifier, Is.EqualTo ("convoId"));
			Assert.That (leadingItemConfig, Is.SameAs (listItem.LeadingConfiguration));
			Assert.That (trailingItemConfig, Is.SameAs (listItem.TrailingConfiguration));
			Assert.That (listItem.DetailText, Is.EqualTo ("detailText"));
			Assert.That (listItem.TrailingText, Is.EqualTo ("trailingText"));
		}

		[Test]
		public void InitUsingFullName ()
		{
			var leadingItemConfig = new CPMessageListItemLeadingConfiguration (new CPMessageLeadingItem (), null, false);
			var trailingItemConfig = new CPMessageListItemTrailingConfiguration (new CPMessageTrailingItem (), null);
			CPMessageListItem listItem = new CPMessageListItem ("fullName", "phoneOrEmail", leadingItemConfig, trailingItemConfig, "detailText", "trailingText", CPMessageListItemType.FullName);

			Assert.That (listItem, Is.Not.Null, "CPMessageListItem not be null.");
			Assert.That (listItem.Text, Is.EqualTo ("fullName"));
			Assert.That (listItem.PhoneOrEmailAddress, Is.EqualTo ("phoneOrEmail"));
			Assert.That (leadingItemConfig, Is.SameAs (listItem.LeadingConfiguration));
			Assert.That (trailingItemConfig, Is.SameAs (listItem.TrailingConfiguration));
			Assert.That (listItem.DetailText, Is.EqualTo ("detailText"));
			Assert.That (listItem.TrailingText, Is.EqualTo ("trailingText"));
		}

	}
}
#endif // HAS_CARPLAY
