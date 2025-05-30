//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2011-2014 Xamarin Inc
//
// The class can be either constructed from a string (from user code)
// or from a handle (from iphone-sharp.dll internal calls).  This
// delays the creation of the actual managed string until actually
// required
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

#nullable enable

using System;
#if !MONOMAC
using UIKit;
#endif
using Foundation;
using CoreLocation;
using ObjCRuntime;

namespace CoreLocation {
#if __TVOS__
	/// <include file="../../docs/api/CoreLocation/CLLocationManager.xml" path="/Documentation/Docs[@DocId='tvOS:T:CoreLocation.CLLocationManager']/*" />
#elif __MACCATALYST__ || __MACOS__
	/// <include file="../../docs/api/CoreLocation/CLLocationManager.xml" path="/Documentation/Docs[@DocId='MacCatalyst|macOS:T:CoreLocation.CLLocationManager']/*" />
#else
	/// <include file="../../docs/api/CoreLocation/CLLocationManager.xml" path="/Documentation/Docs[@DocId='T:CoreLocation.CLLocationManager']/*" />
#endif
	public partial class CLLocationManager : NSObject {

#if IOS
		/// <param name="t">Type of the class, must derive from CLRegion.</param>
		///         <summary>Determines whether the device supports region monitoring for the specified kind of CLRegion.</summary>
		///         <returns>True if the device supports it, false otherwise.</returns>
		///         <remarks>
		///           <para>
		///             This method merely determines whether region monitoring is
		///             available in the hardware, it does not determine whether the
		///             user has enabled location services or whether the
		///             application has been granted permission to use this.  You
		///             must request permission separately.
		///           </para>
		///           <para>
		///             To determine whether you have permission to access
		///             location services, use <see cref="CoreLocation.CLLocationManager" />.
		///           </para>
		///         </remarks>
		public static bool IsMonitoringAvailable (Type t)
		{
			if (SystemVersion.CheckiOS (7, 0))
				return IsMonitoringAvailable (new Class (t));
			return false;
		}
#endif
	}
}
