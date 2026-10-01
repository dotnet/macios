// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CFMessagePortTest {
		static string CreatePortName ()
		{
			return $"com.microsoft.dotnet.macios.cfmessageport.{Guid.NewGuid ():N}";
		}

		[Test]
		public void CreateAndInvalidate ()
		{
			using var port = CFMessagePort.CreateLocalPort (null, (type, data) => new NSData ());

			Assert.That (port, Is.Not.Null, "Port");
			Assert.That (port.IsRemote, Is.False, "IsRemote");
			Assert.That (port.IsValid, Is.True, "IsValid");
			Assert.That (port.Name, Is.Null, "Name");
			Assert.Throws<ArgumentNullException> (() => port.Name = null, "Set null name");
			var name = CreatePortName ();
			Assert.That (port.TrySetName (name), Is.True, "TrySetName");
			Assert.That (port.Name, Is.EqualTo (name), "Changed name");

			var firstCallbackCount = 0;
			var secondCallbackCount = 0;
			port.InvalidationCallback = () => firstCallbackCount++;
			port.InvalidationCallback = () => secondCallbackCount++;
			Assert.That (port.InvalidationCallback, Is.Not.Null, "InvalidationCallback");

			port.Invalidate ();

			Assert.That (port.IsValid, Is.False, "IsValid after invalidation");
			Assert.That (firstCallbackCount, Is.Zero, "First callback");
			Assert.That (secondCallbackCount, Is.EqualTo (1), "Second callback");
			Assert.That (port.InvalidationCallback, Is.Null, "InvalidationCallback after invalidation");
		}

		[Test]
		public void DuplicateLocalPortUsesOriginalCallback ()
		{
			var name = CreatePortName ();
			var callbackCount = 0;
			var invalidationCallbackCount = 0;
			var first = CFMessagePort.CreateLocalPort (name, (type, data) => {
				Interlocked.Increment (ref callbackCount);
				return new NSData ();
			});
			using var second = CFMessagePort.CreateLocalPort (name, (type, data) => throw new InvalidOperationException ());
			using var remote = CFMessagePort.CreateRemotePort (null, name);

			Assert.That (first, Is.Not.Null, "First");
			Assert.That (second, Is.Not.Null, "Second");
			Assert.That (remote, Is.Not.Null, "Remote");
			Assert.That (second.Handle, Is.EqualTo (first.Handle), "Handle");

			using var source = first.CreateRunLoopSource ();
			second.InvalidationCallback = () => invalidationCallbackCount++;
			var runLoop = CFRunLoop.Current;
			runLoop.AddSource (source, CFRunLoop.ModeDefault);
			first.Dispose ();
			try {
				var status = remote.SendRequest (1, null, 5, 5, CFRunLoop.ModeDefault, out var response);
				response?.Dispose ();
				Assert.That (status, Is.EqualTo (CFMessagePortSendRequestStatus.Success), "Status");
				Assert.That (callbackCount, Is.EqualTo (1), "Callback count");
			} finally {
				second.Invalidate ();
				runLoop.RemoveSource (source, CFRunLoop.ModeDefault);
			}
			Assert.That (invalidationCallbackCount, Is.EqualTo (1), "Invalidation callback count");
		}

		[Test]
		public void SendRequest ()
		{
			var name = CreatePortName ();
			var requestBytes = new byte [] { 1, 2, 3 };
			var responseBytes = new byte [] { 4, 5, 6 };
			var callbackCount = 0;
			var receivedType = 0;
			byte [] receivedData = null;
			using var local = CFMessagePort.CreateLocalPort (name, (type, data) => {
				Interlocked.Increment (ref callbackCount);
				receivedType = type;
				receivedData = data.ToArray ();
				return NSData.FromArray (responseBytes);
			});
			using var remote = CFMessagePort.CreateRemotePort (null, name);
			using var queue = new DispatchQueue ("CFMessagePortTest.SendRequest");
			using var request = NSData.FromArray (requestBytes);

			Assert.That (local, Is.Not.Null, "Local");
			Assert.That (remote, Is.Not.Null, "Remote");
			Assert.That (remote.IsRemote, Is.True, "IsRemote");
			Assert.That (remote.TrySetName (CreatePortName ()), Is.False, "TrySetName remote");

			local.SetDispatchQueue (queue);
			var status = remote.SendRequest (42, request, 5, 5, CFRunLoop.ModeDefault, out var response);
			using (response) {
				Assert.That (status, Is.EqualTo (CFMessagePortSendRequestStatus.Success), "Status");
				Assert.That (response, Is.Not.Null, "Response");
				Assert.That (response.ToArray (), Is.EqualTo (responseBytes), "Response data");
				Assert.That (TestRuntime.CFGetRetainCount (response.Handle), Is.EqualTo ((nint) 1), "Response retain count");
			}
			Assert.That (callbackCount, Is.EqualTo (1), "Callback count");
			Assert.That (receivedType, Is.EqualTo (42), "Message identifier");
			Assert.That (receivedData, Is.EqualTo (requestBytes), "Request data");

			var invalidationCallbackCount = 0;
			remote.InvalidationCallback = () => invalidationCallbackCount++;
			remote.Invalidate ();
			Assert.That (invalidationCallbackCount, Is.EqualTo (1), "Remote invalidation callback count");
			status = remote.SendRequest (43, null, 0, 0, CFRunLoop.ModeDefault, out response);
			Assert.That (status, Is.EqualTo (CFMessagePortSendRequestStatus.IsInvalid), "Invalid status");
			Assert.That (response, Is.Null, "Invalid response");
			local.Invalidate ();
		}

		[Test]
		public void CreateRunLoopSourceOwnership ()
		{
			using var local = CFMessagePort.CreateLocalPort (null, (type, data) => new NSData ());
			using var source = local.CreateRunLoopSource ();

			Assert.That (source.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle");
			Assert.That (TestRuntime.CFGetRetainCount (source.Handle), Is.EqualTo ((nint) 2), "Retain count");

			local.Invalidate ();
		}
	}
}
