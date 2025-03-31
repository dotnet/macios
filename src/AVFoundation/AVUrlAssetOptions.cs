// 
// AVUrlAssetOptions.cs: Implements strongly typed access for AV video settings
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, 2014 Xamarin Inc.
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

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AVUrlAssetOptions : DictionaryContainer {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AVUrlAssetOptions ()
			: base (new NSMutableDictionary ())
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AVUrlAssetOptions (NSDictionary dictionary)
			: base (dictionary)
		{
		}
		/// <summary>Indicates whether the asset should be prepared to indicate a precise duration and provide precise random access by time.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVURLAssetPreferPreciseDurationAndTimingKey value to access the underlying dictionary.</remarks>
		public bool? PreferPreciseDurationAndTiming {
			set {
				SetBooleanValue (AVUrlAsset.PreferPreciseDurationAndTimingKey, value);
			}
			get {
				return GetBoolValue (AVUrlAsset.PreferPreciseDurationAndTimingKey);
			}
		}

		/// <summary>Represents the restrictions used by the asset when resolving references to external media data.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant AVURLAssetReferenceRestrictionsKey value to access the underlying dictionary.</remarks>
		public AVAssetReferenceRestrictions? ReferenceRestrictions {
			set {
				SetNumberValue (AVUrlAsset.ReferenceRestrictionsKey, (nuint?) (ulong?) value);
			}
			get {
				return (AVAssetReferenceRestrictions?) (ulong?) GetNUIntValue (AVUrlAsset.ReferenceRestrictionsKey);
			}
		}
#endif
	}
}
