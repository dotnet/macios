// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using System.Runtime.InteropServices;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AEKeywordTest {
		[TestCase ("----", AEKeyword.DirectObject)]
		[TestCase ("errn", AEKeyword.ErrorNumber)]
		[TestCase ("errs", AEKeyword.ErrorString)]
		[TestCase ("psn ", AEKeyword.ProcessSerialNumber)]
		[TestCase ("phac", AEKeyword.PreDispatch)]
		[TestCase ("selh", AEKeyword.SelectProc)]
		[TestCase ("recr", AEKeyword.RecorderCount)]
		[TestCase ("vers", AEKeyword.Version)]
		[TestCase ("tran", AEKeyword.TransactionIdAttribute)]
		[TestCase ("rtid", AEKeyword.ReturnIdAttribute)]
		[TestCase ("evcl", AEKeyword.EventClassAttribute)]
		[TestCase ("evid", AEKeyword.EventIdAttribute)]
		[TestCase ("addr", AEKeyword.AddressAttribute)]
		[TestCase ("optk", AEKeyword.OptionalKeywordAttribute)]
		[TestCase ("timo", AEKeyword.TimeoutAttribute)]
		[TestCase ("inte", AEKeyword.InteractLevelAttribute)]
		[TestCase ("esrc", AEKeyword.EventSourceAttribute)]
		[TestCase ("miss", AEKeyword.MissedKeywordAttribute)]
		[TestCase ("from", AEKeyword.OriginalAddressAttribute)]
		[TestCase ("actm", AEKeyword.AcceptTimeoutAttribute)]
		[TestCase ("repq", AEKeyword.ReplyRequestedAttribute)]
		[TestCase ("seid", AEKeyword.SenderEuidAttribute)]
		[TestCase ("sgid", AEKeyword.SenderEgidAttribute)]
		[TestCase ("uids", AEKeyword.SenderUidAttribute)]
		[TestCase ("gids", AEKeyword.SenderGidAttribute)]
		[TestCase ("spid", AEKeyword.SenderPidAttribute)]
		[TestCase ("tokn", AEKeyword.SenderAuditTokenAttribute)]
		[TestCase ("entl", AEKeyword.SenderAppleScriptEntitlementsAttribute)]
		[TestCase ("aiea", AEKeyword.SenderApplicationIdentifierEntitlementAttribute)]
		[TestCase ("sssb", AEKeyword.SenderApplicationSandboxed)]
		[TestCase ("acat", AEKeyword.ActualSenderAuditToken)]
		[TestCase ("attr", AEKeyword.AppleEventAttributesAttribute)]
		public void Constants (string code, AEKeyword keyword)
		{
			Assert.That (code.ToAEKeyword (), Is.EqualTo (keyword), code);
		}

		[TestCase ("abcd", 0x61626364u)]
		[TestCase ("\u00FF\u0080\0 ", 0xFF800020u)]
		[TestCase ("\0\0\0\0", 0u)]
		[TestCase ("\u00FF\u00FF\u00FF\u00FF", uint.MaxValue)]
		public void CustomKeyword (string code, uint expected)
		{
			Assert.That ((uint) code.ToAEKeyword (), Is.EqualTo (expected));
		}

		[Test]
		public void NullKeyword ()
		{
			var exception = Assert.Throws<ArgumentNullException> (() => AEKeywordExtensions.ToAEKeyword (null));
			Assert.That (exception.ParamName, Is.EqualTo ("self"));
		}

		[TestCase ("")]
		[TestCase ("abc")]
		[TestCase ("abcde")]
		[TestCase ("\u0100abc")]
		[TestCase ("a\u0100bc")]
		[TestCase ("ab\u0100c")]
		[TestCase ("abc\u0100")]
		[TestCase ("\uD83D\uDE00ab")]
		public void InvalidKeyword (string code)
		{
			var exception = Assert.Throws<ArgumentException> (() => code.ToAEKeyword ());
			Assert.That (exception.ParamName, Is.EqualTo ("self"));
		}

		[Test]
		public void RecordFields ()
		{
			using var record = NSAppleEventDescriptor.RecordDescriptor;
			using var value = NSAppleEventDescriptor.DescriptorWithString ("value");
			var keyword = "cust".ToAEKeyword ();
			record.SetDescriptor (value, keyword);
			using (var field = record.DescriptorForKeyword (keyword))
				Assert.That (field?.StringValue, Is.EqualTo ("value"), "enum field");
			Assert.That (record.GetKeywordForDescriptorAtIndex (1), Is.EqualTo (keyword), "enum keyword");
#if !XAMCORE_5_0
			Assert.That (record.KeywordForDescriptorAtIndex (1), Is.EqualTo ((uint) keyword), "uint keyword");
			using (var field = record.DescriptorForKeyword (0x63757374u))
				Assert.That (field?.StringValue, Is.EqualTo ("value"), "uint field");
			record.RemoveDescriptorWithKeyword (0x63757374u);
			Assert.That (record.NumberOfItems, Is.EqualTo (0), "uint remove");
			record.SetDescriptor (value, 0x63757374u);
#endif
			record.RemoveDescriptorWithKeyword (keyword);
			Assert.That (record.NumberOfItems, Is.EqualTo (0), "enum remove");
			Assert.That (record.DescriptorForKeyword (keyword), Is.Null, "missing field");
		}

		[DllImport (Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		static extern IntPtr CreateAppleEvent (IntPtr receiver, IntPtr selector, AEEventClass eventClass, AEEventID eventId, IntPtr target, short returnId, int transactionId);

		[Test]
		public void EventParametersAndAttributes ()
		{
			var handle = CreateAppleEvent (Class.GetHandle (typeof (NSAppleEventDescriptor)), Selector.GetHandle ("appleEventWithEventClass:eventID:targetDescriptor:returnID:transactionID:"),
				AEEventClass.Internet, AEEventID.GetUrl, IntPtr.Zero, -1, 0);
			using var appleEvent = Runtime.GetNSObject<NSAppleEventDescriptor> (handle);
			Assert.That (appleEvent, Is.Not.Null, "event");
			if (appleEvent is null)
				return;

			using var value = NSAppleEventDescriptor.DescriptorWithString ("https://example.com");
			appleEvent.SetParamDescriptor (value, AEKeyword.DirectObject);
			using (var parameter = appleEvent.ParamDescriptorForKeyword (AEKeyword.DirectObject))
				Assert.That (parameter?.StringValue, Is.EqualTo ("https://example.com"), "enum parameter");
#if !XAMCORE_5_0
			using (var parameter = appleEvent.ParamDescriptorForKeyword (0x2D2D2D2Du))
				Assert.That (parameter?.StringValue, Is.EqualTo ("https://example.com"), "uint parameter");
			appleEvent.RemoveParamDescriptorWithKeyword (0x2D2D2D2Du);
			Assert.That (appleEvent.ParamDescriptorForKeyword (AEKeyword.DirectObject), Is.Null, "uint remove");
			appleEvent.SetParamDescriptor (value, 0x2D2D2D2Du);
#endif
			appleEvent.RemoveParamDescriptorWithKeyword (AEKeyword.DirectObject);
			Assert.That (appleEvent.ParamDescriptorForKeyword (AEKeyword.DirectObject), Is.Null, "enum remove");

			using var timeout = NSAppleEventDescriptor.DescriptorWithInt32 (123);
			appleEvent.SetAttributeDescriptor (timeout, AEKeyword.TimeoutAttribute);
			using (var attribute = appleEvent.AttributeDescriptorForKeyword (AEKeyword.TimeoutAttribute))
				Assert.That (attribute?.Int32Value, Is.EqualTo (123), "enum attribute");
#if !XAMCORE_5_0
			using (var attribute = appleEvent.AttributeDescriptorForKeyword (0x74696D6Fu))
				Assert.That (attribute?.Int32Value, Is.EqualTo (123), "uint attribute");
			using var replacement = NSAppleEventDescriptor.DescriptorWithInt32 (456);
			appleEvent.SetAttributeDescriptor (replacement, 0x74696D6Fu);
			using (var attribute = appleEvent.AttributeDescriptorForKeyword (AEKeyword.TimeoutAttribute))
				Assert.That (attribute?.Int32Value, Is.EqualTo (456), "uint set attribute");
#endif
		}
	}
}

#endif
