#nullable enable

using System.Collections;
using System.Collections.Generic;

namespace CloudKit {
	public partial class CKRecord {
		/// <summary>Gets or sets the <see cref="Foundation.NSObject" /> value of the field specified by <paramref name="key" />.</summary>
		/// <value>The value of the field, or <see langword="null" /> if the field does not exist.</value>
		/// <param name="key">The key identifying the record field.</param>
		public NSObject? this [string key] {
			get { return _ObjectForKey (key); }
			set { _SetObject (value.GetHandle (), key); GC.KeepAlive (value); }
		}
	}
}
