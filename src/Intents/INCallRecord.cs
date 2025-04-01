//
// INCallRecord.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if !TVOS
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Intents {
	public partial class INCallRecord {

		/// <summary>Gets the length of the call, in seconds.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double? CallDuration {
			get { return WeakCallDuration?.DoubleValue; }
		}

		/// <summary>Gets a Boolean value that tells whether the call record has been viewed by the user.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Unseen {
			get { return WeakUnseen?.BoolValue; }
		}
	}
}
#endif
