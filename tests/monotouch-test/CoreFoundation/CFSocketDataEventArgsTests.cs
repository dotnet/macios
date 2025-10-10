//
// Unit tests for CFSocketDataEventArgs
//
// Authors:
//	GitHub Copilot
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

using System;
using System.Net;
using Foundation;
using CoreFoundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CFSocketDataEventArgsTests {

		[Test]
		public void Constructor_WithByteArray_SetsPropertiesCorrectly ()
		{
			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var testData = new byte [] { 1, 2, 3, 4, 5 };

			// Act
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, testData);

			// Assert
			Assert.AreSame (remoteEndPoint, args.RemoteEndPoint);
			var retrievedData = args.Data;
			Assert.IsNotNull (retrievedData);
			Assert.AreEqual (testData.Length, retrievedData.Length);
			for (int i = 0; i < testData.Length; i++) {
				Assert.AreEqual (testData [i], retrievedData [i]);
			}
		}

		[Test]
		public void Constructor_WithNullRemoteEndPoint_AcceptsNull ()
		{
			// Arrange
			var testData = new byte [] { 1, 2, 3 };

			// Act & Assert
			// Since RemoteEndPoint uses nullable reference types, null should be accepted
			Assert.DoesNotThrow (() => {
				var args = new CFSocket.CFSocketDataEventArgs (null, testData);
				Assert.IsNull (args.RemoteEndPoint);
				Assert.AreSame (testData, args.Data);
			});
		}

		[Test]
		public void Constructor_WithNullByteArray_AcceptsNull ()
		{
			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);

			// Act & Assert
			// Since data uses nullable reference types, null should be accepted
			Assert.DoesNotThrow (() => {
				var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);
				Assert.AreSame (remoteEndPoint, args.RemoteEndPoint);
				// Data property should return empty array when null
				Assert.IsNotNull (args.Data);
				Assert.AreEqual (0, args.Data.Length);
			});
		}

		[Test]
		public void Data_WithEmptyByteArray_ReturnsEmptyArray ()
		{
			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var emptyData = new byte [0];

			// Act
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, emptyData);

			// Assert
			Assert.AreSame (emptyData, args.Data);
			Assert.AreEqual (0, args.Data.Length);
		}

		[Test]
		public void Data_WithEmptyCFData_ReturnsEmptyArray ()
		{
			// Note: This test would apply to the internal CFData constructor
			// The Data property should return an empty array when cfdata is null or empty
			// We can test this indirectly by testing null byte array behavior
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);

			Assert.AreEqual (0, args.Data.Length);
		}

		[Test]
		public void Data_AccessedMultipleTimes_ReturnsSameInstance ()
		{
			// Arrange - Test with both populated and null data scenarios
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);

			// Test with actual data
			var testData = new byte [] { 1, 2, 3, 4, 5 };
			var args1 = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, testData);

			// Act
			var data1 = args1.Data;
			var data2 = args1.Data;

			// Assert
			Assert.AreSame (data1, data2, "Data property should return the same instance when accessed multiple times with byte array");

			// Test with null data (should return empty array consistently)
			var args2 = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);
			var emptyData1 = args2.Data;
			var emptyData2 = args2.Data;
			Assert.AreSame (emptyData1, emptyData2, "Data property should return the same empty array instance when accessed multiple times with null data");
		}

		[Test]
		public void RemoteEndPoint_IPv6Address_SetsCorrectly ()
		{
			// Arrange
			var ipv6Address = IPAddress.IPv6Loopback;
			var remoteEndPoint = new IPEndPoint (ipv6Address, 9090);
			var testData = new byte [] { 1, 2, 3 };

			// Act
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, testData);

			// Assert
			Assert.AreSame (remoteEndPoint, args.RemoteEndPoint);
			Assert.AreEqual (ipv6Address, args.RemoteEndPoint.Address);
			Assert.AreEqual (9090, args.RemoteEndPoint.Port);
		}

		[Test]
		public void RemoteEndPoint_DifferentPorts_SetsCorrectly ()
		{
			// Arrange
			var testData = new byte [] { 1, 2, 3 };
			var ports = new int [] { 0, 1, 80, 443, 8080, 65535 };

			foreach (var port in ports) {
				var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, port);

				// Act
				var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, testData);

				// Assert
				Assert.AreEqual (port, args.RemoteEndPoint.Port, $"Port {port} should be set correctly");
			}
		}

		[Test]
		public void InheritsFromEventArgs ()
		{
			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var testData = new byte [] { 1, 2, 3 };

			// Act
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, testData);

			// Assert
			Assert.IsInstanceOf<EventArgs> (args, "CFSocketDataEventArgs should inherit from EventArgs");
		}

		[Test]
		public void LargeDataArray_HandledCorrectly ()
		{
			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var largeData = new byte [1024 * 1024]; // 1MB
			for (int i = 0; i < largeData.Length; i++) {
				largeData [i] = (byte) (i % 256);
			}

			// Act
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, largeData);

			// Assert
			Assert.AreSame (largeData, args.Data);
			Assert.AreEqual (1024 * 1024, args.Data.Length);

			// Verify a few sample bytes
			Assert.AreEqual (0, args.Data [0]);
			Assert.AreEqual (255, args.Data [255]);
			Assert.AreEqual (0, args.Data [256]);
		}

		[Test]
		public void CFData_LazyLoading_WorksCorrectly ()
		{
			// Note: Since CFData property and constructor are internal, we can't directly test the lazy loading
			// behavior from CFData. However, we can test the lazy loading behavior when data is null.

			// Arrange
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);

			// Act & Assert
			// First access to Data should trigger the lazy loading (should return empty array)
			var retrievedData = args.Data;
			Assert.IsNotNull (retrievedData);
			Assert.AreEqual (0, retrievedData.Length);

			// Subsequent accesses should return the same cached instance
			var retrievedData2 = args.Data;
			Assert.AreSame (retrievedData, retrievedData2);
		}
	}
}
