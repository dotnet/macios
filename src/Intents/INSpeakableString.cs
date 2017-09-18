﻿﻿//
// INSpeakableString.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if XAMCORE_2_0
using System;
using XamCore.Foundation;
using XamCore.ObjCRuntime;

namespace XamCore.Intents {
	public partial class INSpeakableString {
		public INSpeakableString (string identifier, string spokenPhrase, string pronunciationHint)
			: base (NSObjectFlag.Empty)
		{
#if IOS
			if (XamCore.UIKit.UIDevice.CurrentDevice.CheckSystemVersion (11, 0))
#elif WATCH
			if (XamCore.WatchKit.WKInterfaceDevice.CurrentDevice.CheckSystemVersion (4, 0))
#elif MONOMAC
			if (PlatformHelper.CheckSystemVersion (10, 13))
#endif
				InitializeHandle (InitWithVocabularyIdentifier (identifier, spokenPhrase, pronunciationHint));
			else
				InitializeHandle (InitWithIdentifier (identifier, spokenPhrase, pronunciationHint));
		}
	}
}
#endif
