// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace GameController {
	public partial class GCPhysicalInputElementCollection<KeyIdentifierType, ElementIdentifierType> : IEnumerable<KeyIdentifierType> {
		#region IEnumerable<KeyIdentifierType>
		IEnumerator<KeyIdentifierType> IEnumerable<KeyIdentifierType>.GetEnumerator ()
		{
			return new NSFastEnumerator<KeyIdentifierType> (this);
		}
		#endregion

		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<KeyIdentifierType> (this);
		}
		#endregion
	}
}
