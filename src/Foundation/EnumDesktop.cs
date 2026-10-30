//
// Copyright 2010, Novell, Inc.
// Copyright 2012 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

// FIXME: EnumDesktop.cs should be merged into Enums.cs

namespace Foundation {
	[NoiOS, NoTV, NoMacCatalyst]
	[Internal]
	internal enum NSAttributedStringDataType {
		RTF,
		RTFD,
		HTML,
	}

	// NSTask.h:typedef NS_ENUM(NSInteger, NSTaskTerminationReason)
	[Native]
	[NoiOS, NoTV, NoMacCatalyst]
	public enum NSTaskTerminationReason : long {
		/// <summary>To be added.</summary>
		Exit = 1,
		/// <summary>To be added.</summary>
		UncaughtSignal = 2,
	}

	// The following constants where taken from MonoDevelop Carbon.cs MacInterop file
	[NoiOS, NoTV]
	[MacCatalyst (13, 1)]
	public enum AEEventID : uint {
		/// <summary>To be added.</summary>
		OpenApplication = 1868656752, // 'oapp'
		/// <summary>To be added.</summary>
		ReopenApplication = 1918988400, //'rapp'
		/// <summary>To be added.</summary>
		OpenDocuments = 1868853091, // 'odoc'
		/// <summary>To be added.</summary>
		PrintDocuments = 188563030, // 'pdoc'
		/// <summary>To be added.</summary>
		OpenContents = 1868787566, // 'ocon'
		/// <summary>To be added.</summary>
		QuitApplication = 1903520116, // 'quit'
		/// <summary>To be added.</summary>
		ShowPreferences = 1886545254, // 'pref'
		/// <summary>To be added.</summary>
		ApplicationDied = 1868720500, // 'obit'
		/// <summary>To be added.</summary>
		GetUrl = 1196773964, // 'GURL'
	}

	[NoiOS, NoTV]
	[MacCatalyst (13, 1)]
	public enum AEEventClass : uint {
		/// <summary>To be added.</summary>
		Mouse = 1836021107, // 'mous'
		/// <summary>To be added.</summary>
		Keyboard = 1801812322, // 'keyb'
		/// <summary>To be added.</summary>
		TextInput = 1952807028, // 'text'
		/// <summary>To be added.</summary>
		Application = 1634758764, // 'appl'
		/// <summary>To be added.</summary>
		RemoteAppleEvent = 1701867619,  //'eppc' //remote apple event?
		/// <summary>To be added.</summary>
		Menu = 1835363957, // 'menu'
		/// <summary>To be added.</summary>
		Window = 2003398244, // 'wind'
		/// <summary>To be added.</summary>
		Control = 1668183148, // 'cntl'
		/// <summary>To be added.</summary>
		Command = 1668113523, // 'cmds'
		/// <summary>To be added.</summary>
		Tablet = 1952607348, // 'tblt'
		/// <summary>To be added.</summary>
		Volume = 1987013664, // 'vol '
		/// <summary>To be added.</summary>
		Appearance = 1634758765, // 'appm'
		/// <summary>To be added.</summary>
		Service = 1936028278, // 'serv'
		/// <summary>To be added.</summary>
		Toolbar = 1952604530, // 'tbar'
		/// <summary>To be added.</summary>
		ToolbarItem = 1952606580, // 'tbit'
		/// <summary>To be added.</summary>
		Accessibility = 1633903461, // 'acce'
		/// <summary>To be added.</summary>
		HIObject = 1751740258, // 'hiob'
		/// <summary>To be added.</summary>
		AppleEvent = 1634039412, // 'aevt'
		/// <summary>To be added.</summary>
		Internet = 1196773964, // 'GURL'
	}

	/// <summary>Four-character codes identifying Apple event parameters and attributes.</summary>
	[NoiOS, NoTV]
	[MacCatalyst (13, 1)]
	public enum AEKeyword : uint {
		/// <summary>The direct object parameter ('----').</summary>
		DirectObject = 0x2D2D2D2D,
		/// <summary>The error number parameter ('errn').</summary>
		ErrorNumber = 0x6572726E,
		/// <summary>The error message parameter ('errs').</summary>
		ErrorString = 0x65727273,
		/// <summary>The process serial number parameter ('psn ').</summary>
		ProcessSerialNumber = 0x70736E20,
		/// <summary>The pre-dispatch handler keyword ('phac').</summary>
		PreDispatch = 0x70686163,
		/// <summary>The handler selection keyword ('selh').</summary>
		SelectProc = 0x73656C68,
		/// <summary>The number of Apple event recorders ('recr').</summary>
		RecorderCount = 0x72656372,
		/// <summary>The Apple Event Manager version ('vers').</summary>
		Version = 0x76657273,
		/// <summary>The transaction identifier attribute ('tran').</summary>
		TransactionIdAttribute = 0x7472616E,
		/// <summary>The reply return identifier attribute ('rtid').</summary>
		ReturnIdAttribute = 0x72746964,
		/// <summary>The event class attribute ('evcl').</summary>
		EventClassAttribute = 0x6576636C,
		/// <summary>The event identifier attribute ('evid').</summary>
		EventIdAttribute = 0x65766964,
		/// <summary>The target address attribute ('addr').</summary>
		AddressAttribute = 0x61646472,
		/// <summary>The list of optional parameter keywords ('optk').</summary>
		OptionalKeywordAttribute = 0x6F70746B,
		/// <summary>The reply timeout attribute ('timo').</summary>
		TimeoutAttribute = 0x74696D6F,
		/// <summary>The permitted user interaction level attribute ('inte').</summary>
		InteractLevelAttribute = 0x696E7465,
		/// <summary>The event source attribute ('esrc').</summary>
		EventSourceAttribute = 0x65737263,
		/// <summary>The first unread parameter keyword attribute ('miss').</summary>
		MissedKeywordAttribute = 0x6D697373,
		/// <summary>The original sender address attribute ('from').</summary>
		OriginalAddressAttribute = 0x66726F6D,
		/// <summary>The event acceptance timeout attribute ('actm').</summary>
		AcceptTimeoutAttribute = 0x6163746D,
		/// <summary>The attribute indicating whether a reply was requested ('repq').</summary>
		ReplyRequestedAttribute = 0x72657071,
		/// <summary>The sender's effective user identifier attribute ('seid').</summary>
		SenderEuidAttribute = 0x73656964,
		/// <summary>The sender's effective group identifier attribute ('sgid').</summary>
		SenderEgidAttribute = 0x73676964,
		/// <summary>The sender's user identifier attribute ('uids').</summary>
		SenderUidAttribute = 0x75696473,
		/// <summary>The sender's group identifier attribute ('gids').</summary>
		SenderGidAttribute = 0x67696473,
		/// <summary>The sender's process identifier attribute ('spid').</summary>
		SenderPidAttribute = 0x73706964,
		/// <summary>The sender's audit token attribute ('tokn').</summary>
		SenderAuditTokenAttribute = 0x746F6B6E,
		/// <summary>The sender's AppleScript entitlements attribute ('entl').</summary>
		SenderAppleScriptEntitlementsAttribute = 0x656E746C,
		/// <summary>The sender's application identifier entitlement attribute ('aiea').</summary>
		SenderApplicationIdentifierEntitlementAttribute = 0x61696561,
		/// <summary>The attribute indicating whether the sender is sandboxed ('sssb').</summary>
		SenderApplicationSandboxed = 0x73737362,
		/// <summary>The actual sender's audit token ('acat').</summary>
		ActualSenderAuditToken = 0x61636174,
		/// <summary>The list of attribute keywords present in an event ('attr').</summary>
		AppleEventAttributesAttribute = 0x61747472,
	}

	// Added from NSUserNotification.h
	[NoiOS, NoTV, NoMacCatalyst]
	[Native]
	[Advice ("'NSUserNotification' usages should be replaced with 'UserNotifications' framework.")]
	public enum NSUserNotificationActivationType : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		ContentsClicked = 1,
		/// <summary>To be added.</summary>
		ActionButtonClicked = 2,
		/// <summary>To be added.</summary>
		Replied = 3,
		/// <summary>To be added.</summary>
		AdditionalActionClicked = 4,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSAppleEventSendOptions : ulong {
		/// <summary>To be added.</summary>
		NoReply = 0x00000001, // kAENoReply,
		/// <summary>To be added.</summary>
		QueueReply = 0x00000002, // kAEQueueReply,
		/// <summary>To be added.</summary>
		WaitForReply = 0x00000003, // kAEWaitReply,
		/// <summary>To be added.</summary>
		NeverInteract = 0x00000010, // kAENeverInteract,
		/// <summary>To be added.</summary>
		CanInteract = 0x00000020, // kAECanInteract,
		/// <summary>To be added.</summary>
		AlwaysInteract = 0x00000030, // kAEAlwaysInteract,
		/// <summary>To be added.</summary>
		CanSwitchLayer = 0x00000040, // kAECanSwitchLayer,
		/// <summary>To be added.</summary>
		DontRecord = 0x00001000, // kAEDontRecord,
		/// <summary>To be added.</summary>
		DontExecute = 0x00002000, // kAEDontExecute,
		/// <summary>To be added.</summary>
		DontAnnotate = 0x00010000, // kAEDoNotAutomaticallyAddAnnotationsToEvent,
		/// <summary>To be added.</summary>
		DefaultOptions = WaitForReply | CanInteract,
	}
}
