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
using System;

using ObjCRuntime;

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
