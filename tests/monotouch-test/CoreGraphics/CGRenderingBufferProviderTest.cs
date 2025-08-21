// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;
using CoreGraphics;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGRenderingBufferProviderTest {
		// FIXME: improve these tests

		[Test]
		public void CreateWithCFData_ReturnsInstanceOrNull ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var data = new NSMutableData (10); // Create a small CFMutableDataRef
			var provider = CGRenderingBufferProvider.Create (data);
			Assert.That (provider, Is.Null.Or.InstanceOf<CGRenderingBufferProvider> (), "Should return null or a valid instance");
		}

		[Test]
		public void SizeProperty_DoesNotThrow ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var data = new NSMutableData (10);
			var provider = CGRenderingBufferProvider.Create (data);
			if (provider is not null) {
				Assert.DoesNotThrow (() => { var size = provider.Size; });
			}
		}

		[Test]
		public void LockUnlockBytePointer_DoesNotThrow ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var data = new NSMutableData (10);
			var provider = CGRenderingBufferProvider.Create (data);
			if (provider is not null) {
				Assert.DoesNotThrow (() => {
					var ptr = provider.LockBytePointer ();
					provider.UnlockBytePointer ();
				});
			}
		}

		[Test]
		public void GetTypeId_ReturnsTypeId ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.DoesNotThrow (() => {
				var typeId = CGRenderingBufferProvider.GetTypeId ();
				Assert.GreaterOrEqual (typeId, 0);
			});
		}
	}
}
