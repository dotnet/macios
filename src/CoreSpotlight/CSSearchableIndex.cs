//
// CSSearchableIndex.cs: Implements some nicer methods for CSSearchableIndex
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

#if IOS

using System;
using Foundation;
using ObjCRuntime;

namespace CoreSpotlight {

	public partial class CSSearchableIndex {

		// Strongly typed version of initWithName:protectionClass:
		/// <param name="name">To be added.</param>
		///         <param name="protectionOption">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreSpotlight.CSSearchableIndex" /> with the specified <paramref name="name" /> and protection options.</summary>
		///         <remarks>To be added.</remarks>
		public CSSearchableIndex (string name, CSFileProtection protectionOption = CSFileProtection.None) : this (name, Translate (protectionOption))
		{ }

		static NSString? Translate (CSFileProtection protectionOption)
		{
			switch (protectionOption) {
			case CSFileProtection.None:
				return null;
			case CSFileProtection.Complete:
				return NSFileManager.FileProtectionComplete;
			case CSFileProtection.CompleteUnlessOpen:
				return NSFileManager.FileProtectionCompleteUnlessOpen;
			case CSFileProtection.CompleteUntilFirstUserAuthentication:
				return NSFileManager.FileProtectionCompleteUntilFirstUserAuthentication;
			default:
				throw new ArgumentOutOfRangeException ("protectionOption");
			}
		}
	}
}

#endif // IOS
