// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreBluetooth;

namespace MonoTouchFixtures.CoreBluetooth {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AdvertisementDataTest {
		[Test]
		public void ServiceData ()
		{
			using var result = GetServiceData ();
			using var firstUuid = CBUUID.FromString ("180D");
			using var secondUuid = CBUUID.FromString ("180F");
			Assert.That (result, Is.Not.Null, "ServiceData");
			Assert.That (result.Count, Is.EqualTo ((nuint) 2), "Count");
			Assert.That (result [firstUuid].ToArray (), Is.EqualTo (new byte [] { 1, 2, 3 }), "First data");
			Assert.That (result [secondUuid].ToArray (), Is.EqualTo (new byte [] { 4, 5 }), "Second data");
		}

		static NSDictionary<CBUUID, NSData> GetServiceData ()
		{
			using var firstUuid = CBUUID.FromString ("180D");
			using var secondUuid = CBUUID.FromString ("180F");
			using var firstData = NSData.FromArray (new byte [] { 1, 2, 3 });
			using var secondData = NSData.FromArray (new byte [] { 4, 5 });
			using var serviceData = new NSMutableDictionary ();
			serviceData [firstUuid] = firstData;
			serviceData [secondUuid] = secondData;
			using var dictionary = new NSDictionary (CBAdvertisement.DataServiceDataKey, serviceData);
			var advertisement = new AdvertisementData (dictionary);

			var result = advertisement.ServiceData;
			serviceData.Clear ();
			return result;
		}
	}
}
