//
// INSetCarLockStatusIntent.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if IOS

using Foundation;
using Intents;
using ObjCRuntime;

namespace Intents {

	public partial class INSetCarLockStatusIntent {

		/// <param name="locked">To be added.</param>
		///         <param name="carName">To be added.</param>
		///         <summary>Creates a new set car lock status intent for the specified lock state and car name.</summary>
		///         <remarks>To be added.</remarks>
		public INSetCarLockStatusIntent (bool? locked, INSpeakableString carName) :
			this (locked.HasValue ? new NSNumber (locked.Value) : null, carName)
		{
		}
	}
}

#endif
