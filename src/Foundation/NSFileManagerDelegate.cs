//
// NSLayoutConstraint.cs:
//
// Authors:
//  Paola Villarreal <paola.villarreal@xamarin.com>
//
// Copyright 2015, Xamarin Inc
//

using System;
using Foundation;

namespace Foundation {
	public partial class NSFileManagerDelegate {
		/// <param name="fileManager">To be added.</param>
		///         <param name="srcPath">To be added.</param>
		///         <param name="dstPath">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual bool ShouldCopyItemAtPath (NSFileManager fileManager, string srcPath, string dstPath)
		{
			return ShouldCopyItemAtPath (fileManager, (NSString) srcPath, (NSString) dstPath);
		}
	}
	public static partial class NSFileManagerDelegate_Extensions {
		// This was a duplicate [Export] so in order to avoid breaking the API we expose it this way.
		// NOTE: this is an Extension method, (NSFileManagerDelegate is a [Protocol]) so the exported methods are, by default, extensions. 
		/// <param name="This">The instance on which this extension method operates.</param>
		///         <param name="fileManager">To be added.</param>
		///         <param name="srcPath">To be added.</param>
		///         <param name="dstPath">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool ShouldCopyItemAtPath (this INSFileManagerDelegate This, NSFileManager fileManager, string srcPath, string dstPath)
		{
			return This.ShouldCopyItemAtPath (fileManager, (NSString) srcPath, (NSString) dstPath);
		}
	}
}
