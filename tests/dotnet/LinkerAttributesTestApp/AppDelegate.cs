// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;

namespace LinkerAttributesTestApp {
	public class LinkAttributesTest {
		[Foundation.RequiredMember]
		public void RequiredProtocolMember ()
		{
		}

		[Foundation.OptionalMember]
		public void OptionalProtocolMember ()
		{
		}

		public required string RequiredProperty { get; set; }
	}

	public class Program {
		static int Main ()
		{
			var linkAttributesTest = new LinkAttributesTest {
				RequiredProperty = "",
			};
			linkAttributesTest.RequiredProtocolMember ();
			linkAttributesTest.OptionalProtocolMember ();
			GC.KeepAlive (linkAttributesTest);
			GC.KeepAlive (typeof (NSObject));

			return 0;
		}
	}
}
