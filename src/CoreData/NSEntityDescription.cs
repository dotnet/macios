//
// NSEntityDescription.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.

#nullable enable

namespace CoreData {
	/// <summary>Describes the value relationships between an in-memory object and its persistent representation.</summary>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Cocoa/Reference/CoreDataFramework/Classes/NSEntityDescription_Class/index.html">Apple documentation for <c>NSEntityDescription</c></related>
	public partial class NSEntityDescription {
		/// <summary>Gets or sets the uniqueness constraints for this entity.</summary>
		/// <value>
		///   A jagged array where each element is an array of <see cref="NSObject" /> properties
		///   that together form a uniqueness constraint. The returned array is never <see langword="null" />; it will be an empty array if no uniqueness constraints are configured.
		/// </value>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSObject [] [] UniquenessConstraints {
			get { return NSArray.FromArrayOfArray (_UniquenessConstraints)!; }
			set { _UniquenessConstraints = NSArray.From (value)!; }
		}
	}
}
