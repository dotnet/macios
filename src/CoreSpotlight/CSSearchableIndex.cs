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

namespace CoreSpotlight {

	public partial class CSSearchableIndex {

		// Strongly typed version of initWithName:protectionClass:
		/// <summary>Creates a new <see cref="CoreSpotlight.CSSearchableIndex" /> with the specified <paramref name="name" /> and protection options.</summary>
		/// <param name="name">The name of the searchable index.</param>
		/// <param name="protectionOption">The file protection level for the index.</param>
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
