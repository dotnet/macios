// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreFoundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CFExceptionTest {

		[Test]
		public void CFErrorDomain_Constants ()
		{
			// Verify that all CFErrorDomain native symbols resolve correctly.
			Assert.Multiple (() => {
				Assert.That (CFErrorDomain.Cocoa, Is.Not.Null, "Cocoa");
				Assert.That (CFErrorDomain.Mach, Is.Not.Null, "Mach");
				Assert.That (CFErrorDomain.OSStatus, Is.Not.Null, "OSStatus");
				Assert.That (CFErrorDomain.Posix, Is.Not.Null, "Posix");
			});
		}
	}
}
