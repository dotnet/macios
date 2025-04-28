//
// MPSkipIntervalCommand
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MediaPlayer {
	/// <summary>Additional information for the skip interval command properties defined in <see cref="T:MediaPlayer.MPRemoteCommandCenter" />.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPSkipIntervalCommand_Ref/index.html">Apple documentation for <c>MPSkipIntervalCommand</c></related>
	public partial class MPSkipIntervalCommand {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double []? PreferredIntervals {
			get {
				NSArray a = _PreferredIntervals;
				if ((a is null) || (a.Count == 0))
					return null;

				double []? result = NSArray.ArrayFromHandle<double> (a.Handle, input => {
					return new NSNumber (input).DoubleValue;
				});
				GC.KeepAlive (a);
				return result;
			}
			set {
				if (value is null)
					_PreferredIntervals = new NSArray ();
				else {
					NSObject [] nsoa = new NSObject [value.Length];
					for (int i = 0; i < value.Length; i++)
						nsoa [i] = new NSNumber (value [i]);
					_PreferredIntervals = NSArray.FromNSObjects (nsoa);
				}
			}
		}
	}
}
