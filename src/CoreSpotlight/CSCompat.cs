// Compatibility stubs

#if !XAMCORE_4_0 && IOS

using System;

namespace XamCore.CoreSpotlight {

	partial class CSCustomAttributeKey {

		[Obsolete ("Use .ctor(string)")]
		public CSCustomAttributeKey () : this (String.Empty)
		{
		}
	}
}

#endif
