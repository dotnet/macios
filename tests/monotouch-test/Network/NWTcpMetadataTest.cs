#nullable enable

using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using CoreFoundation;
using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWTcpMetadataTest {

		[Test]
		public void SetMaximumPacingRate ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var readyCompletion = new TaskCompletionSource<string?> (TaskCreationOptions.RunContinuationsAsynchronously);
			var cancelledCompletion = new TaskCompletionSource<bool> (TaskCreationOptions.RunContinuationsAsynchronously);
			using var listener = new TcpListener (IPAddress.Loopback, 0);
			listener.Start ();

			var port = ((IPEndPoint) listener.LocalEndpoint).Port;
			using var endpoint = NWEndpoint.Create ("127.0.0.1", port.ToString (CultureInfo.InvariantCulture));
			using var parameters = NWParameters.CreateTcp ();
			using var connection = new NWConnection (endpoint, parameters);

			connection.SetQueue (DispatchQueue.DefaultGlobalQueue);
			connection.SetStateChangeHandler ((state, error) => {
				switch (state) {
				case NWConnectionState.Ready:
					readyCompletion.TrySetResult (null);
					break;
				case NWConnectionState.Invalid:
				case NWConnectionState.Failed:
					readyCompletion.TrySetResult (error?.ToString () ?? $"Connection entered the {state} state.");
					break;
				case NWConnectionState.Cancelled:
					cancelledCompletion.TrySetResult (true);
					break;
				}
			});

			TcpClient? acceptedClient = null;
			connection.Start ();
			try {
				Assert.That (readyCompletion.Task.Wait (TimeSpan.FromSeconds (10)), Is.True, "Connection did not become ready.");
				var failureMessage = readyCompletion.Task.Result;
				if (failureMessage is not null)
					Assert.Fail (failureMessage);

				acceptedClient = listener.AcceptTcpClient ();

				using var definition = NWProtocolDefinition.CreateTcpDefinition ();
				using var metadata = connection.GetProtocolMetadata<NWTcpMetadata> (definition);
				Assert.That (metadata, Is.Not.Null, "TCP metadata");
				if (metadata is not null) {
					Assert.That (metadata.SetMaximumPacingRate (12_500), Is.Zero, "Set pacing rate");
					Assert.That (metadata.SetMaximumPacingRate (ulong.MaxValue), Is.Zero, "Disable pacing");
				}
			} finally {
				acceptedClient?.Dispose ();
				connection.Cancel ();
				var cancelled = cancelledCompletion.Task.Wait (TimeSpan.FromSeconds (5));
				if (!cancelled) {
					connection.ForceCancel ();
					cancelled = cancelledCompletion.Task.Wait (TimeSpan.FromSeconds (5));
				}
				Assert.That (cancelled, Is.True, "Connection did not cancel.");
			}
		}
	}
}
