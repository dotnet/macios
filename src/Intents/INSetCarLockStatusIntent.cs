//
// INSetCarLockStatusIntent.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if IOS

using Intents;

namespace Intents {

	public partial class INSetCarLockStatusIntent {

		/// <summary>Creates a new set car lock status intent for the specified lock state and car name.</summary>
		/// <param name="locked">Whether the car should be locked.</param>
		/// <param name="carName">The name of the car.</param>
		public INSetCarLockStatusIntent (bool? locked, INSpeakableString carName) :
			this (locked.HasValue ? new NSNumber (locked.Value) : null, carName)
		{
		}
	}
}

#endif
