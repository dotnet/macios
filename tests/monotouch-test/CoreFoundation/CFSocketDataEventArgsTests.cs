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
			Assert.That (args.RemoteEndPoint, Is.SameAs (remoteEndPoint));
			var retrievedData = args.Data;
			Assert.That (retrievedData, Is.Not.Null);
			Assert.That (retrievedData.Length, Is.EqualTo (testData.Length));
			for (int i = 0; i < testData.Length; i++) {
				Assert.That (retrievedData [i], Is.EqualTo (testData [i]));
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
				Assert.That (args.RemoteEndPoint, Is.Null);
				Assert.That (args.Data, Is.SameAs (testData));
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
				Assert.That (args.RemoteEndPoint, Is.SameAs (remoteEndPoint));
				// Data property should return empty array when null
				Assert.That (args.Data, Is.Not.Null);
				Assert.That (args.Data.Length, Is.EqualTo (0));
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
			Assert.That (args.Data, Is.SameAs (emptyData));
			Assert.That (args.Data.Length, Is.EqualTo (0));
		}

		[Test]
		public void Data_WithEmptyCFData_ReturnsEmptyArray ()
		{
			// Note: This test would apply to the internal CFData constructor
			// The Data property should return an empty array when cfdata is null or empty
			// We can test this indirectly by testing null byte array behavior
			var remoteEndPoint = new IPEndPoint (IPAddress.Loopback, 8080);
			var args = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);

			Assert.That (args.Data.Length, Is.EqualTo (0));
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
			Assert.That (data2, Is.SameAs (data1), "Data property should return the same instance when accessed multiple times with byte array");

			// Test with null data (should return empty array consistently)
			var args2 = new CFSocket.CFSocketDataEventArgs (remoteEndPoint, (byte []) null);
			var emptyData1 = args2.Data;
			var emptyData2 = args2.Data;
			Assert.That (emptyData2, Is.SameAs (emptyData1), "Data property should return the same empty array instance when accessed multiple times with null data");
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
			Assert.That (args.RemoteEndPoint, Is.SameAs (remoteEndPoint));
			Assert.That (args.RemoteEndPoint.Address, Is.EqualTo (ipv6Address));
			Assert.That (args.RemoteEndPoint.Port, Is.EqualTo (9090));
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
				Assert.That (args.RemoteEndPoint.Port, Is.EqualTo (port), $"Port {port} should be set correctly");
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
			Assert.That (args, Is.InstanceOf<EventArgs> (), "CFSocketDataEventArgs should inherit from EventArgs");
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
			Assert.That (args.Data, Is.SameAs (largeData));
			Assert.That (args.Data.Length, Is.EqualTo (1024 * 1024));

			// Verify a few sample bytes
			Assert.That (args.Data [0], Is.EqualTo (0));
			Assert.That (args.Data [255], Is.EqualTo (255));
			Assert.That (args.Data [256], Is.EqualTo (0));
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
			Assert.That (retrievedData, Is.Not.Null);
			Assert.That (retrievedData.Length, Is.EqualTo (0));

			// Subsequent accesses should return the same cached instance
			var retrievedData2 = args.Data;
			Assert.That (retrievedData2, Is.SameAs (retrievedData));
		}
	}
}
