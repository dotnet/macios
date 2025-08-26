// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

using Foundation;
using CoreGraphics;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGRenderingBufferProviderTest {
		[Test]
		public void CreateWithCFData_ReturnsNull ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			using var data = new NSMutableData (10); // Create a small CFMutableDataRef
			var provider = CGRenderingBufferProvider.Create (data);
			Assert.That (provider, Is.Null, "This shouldn't work"); // doesn't work because I have no idea what to put in the NSData to make it valid
		}

		[Test]
		public void SizeProperty_DoesNotThrow ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			var size = 512;
			var calledOnLockPointer = false;
			var calledOnUnlockPointer = false;
			var calledOnReleaseInfo = false;
			var provider = CGRenderingBufferProvider.Create ((nint) 0x0ee1f00d, (nuint) size,
				lockPointer: (info) => {
					calledOnLockPointer = true;
					var rv = Marshal.AllocHGlobal (size);
					// Console.WriteLine ($"CreateAdaptive () OnLockPointer#4 ({info}) => {rv}");
					return rv;
				},
				unlockPointer: (info, pointer) => {
					// Console.WriteLine ($"CreateAdaptive () OnUnlockPointer#4 ({info}, {pointer})");
					calledOnUnlockPointer = true;
					Marshal.FreeHGlobal (pointer);
				},
				releaseInfo: (info) => {
					// Console.WriteLine ($"CreateAdaptive () OnReleaseInfo#4 ({info})");
					calledOnReleaseInfo = true;
				});
			Assert.That (provider, Is.Not.Null, "provider");
			Assert.That (provider.Size, Is.EqualTo ((nuint) size), "size");
			Assert.That (calledOnLockPointer, Is.EqualTo (false), "calledOnLockPointer");
			Assert.That (calledOnUnlockPointer, Is.EqualTo (false), "calledOnUnlockPointer");
			Assert.That (calledOnReleaseInfo, Is.EqualTo (false), "calledOnReleaseInfo");
		}

		[Test]
		public void GetTypeId ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			Assert.DoesNotThrow (() => {
				var typeId = CGRenderingBufferProvider.GetTypeId ();
				Assert.That (typeId, Is.GreaterThan ((nint) 0), "GetTypeId");
			});
		}
	}
}
