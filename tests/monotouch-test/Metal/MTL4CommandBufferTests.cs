using System;
using System.IO;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[Preserve (AllMembers = true)]
	public class MTL4CommandBufferTests {
		[Test]
		public void UseResidencySets ()
		{
			using var commandBuffer = Helper.CreateMTL4CommandBuffer (out var device);
			using var residencySetDescriptor = new MTLResidencySetDescriptor () {
				Label = "Label",
				InitialCapacity = 3,
			};
			using var residencySet = device.CreateResidencySet (residencySetDescriptor, out var error);
			Assert.That (error, Is.Null, "Error #1");
			Assert.That (residencySet, Is.Not.Null, "ResidencySet #1");

			commandBuffer.UseResidencySets (residencySet);
			commandBuffer.UseResidencySets (new IMTLResidencySet [] { residencySet });
		}
	}
}
