//
// UIEvent.cs: Extensions to the UIEvent class
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012, Xamarin Inc
//

using System;
using ObjCRuntime;
using Foundation;

namespace UIKit {
	public partial class UIEvent {

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return String.Format ("[Time={0} ({1}{2})]", Timestamp, Type, Subtype != UIEventSubtype.None ? "." + Subtype : "");
		}
	}
}
