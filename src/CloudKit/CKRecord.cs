#nullable enable

using ObjCRuntime;
using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CloudKit {
	public partial class CKRecord {
		/// <param name="key">To be added.</param>
		/// <summary>Gets or sets the <see cref="T:Foundation.NSObject" /> value of the field specified by <paramref name="key" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public NSObject? this [string key] {
			get { return _ObjectForKey (key); }
			set { _SetObject (value.GetHandle (), key); GC.KeepAlive (value); }
		}
	}
}
