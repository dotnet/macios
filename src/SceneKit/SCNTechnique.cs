//
// SCNTechnique.cs: extensions to SCNTechnique
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)   
//
// Copyright Xamarin Inc.
//

using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace SceneKit {
	public partial class SCNTechnique {
		public NSObject? this [NSString key] {
			get { return _GetObject (key); }
			set { _SetObject (value, key); }
		}
	}
}
