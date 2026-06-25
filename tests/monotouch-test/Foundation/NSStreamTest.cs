using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSStreamTest {
		[Test]
		public void BoundPairTest ()
		{
			NSInputStream read;
			NSOutputStream write;

			NSStream.CreateBoundPair (out read, out write, 1024);
			read.Open ();
			write.Open ();

			var send = Encoding.ASCII.GetBytes ("hello, world");
			nint n = send.Length;

			Assert.That (write.Write (send), Is.EqualTo (n));
			var result = new byte [n + 10];

			Assert.That (read.Read (result, (uint) n), Is.EqualTo (n));
			for (int i = 0; i < n; i++)
				Assert.That (result [i], Is.EqualTo (send [i]), "Item " + i);

		}

		TcpListener FindPort (out int port)
		{
			// This does not work well on watchOS:
			// The request to start the tcp listener will fail, but
			// at the same time leave a file descriptor (the socket) open
			// until the TcpListener is collected by the GC.
			// Since we create 3000 TcpListeners here, we end up using
			// up all the available file descriptors, causing trouble
			// for later tests.
			for (port = 3000; port < 6000; port++) {
				var listener = new TcpListener (IPAddress.Any, port);
				try {
					listener.Start ();
					return listener;
				} catch {
				}
			}
			return null;
		}

		[Test]
		public void ConnectToHost ()
		{
			NSInputStream read;
			NSOutputStream write;

			int port;
			var listener = FindPort (out port);
			if (listener is null) {
				Assert.Inconclusive ("Not possible to bind a port");
				return;
			}

			var listenThread = new Thread (new ParameterizedThreadStart (DebugListener)) {
				IsBackground = true,
			};
			listenThread.Start (listener);
			NSStream.CreatePairWithSocketToHost (new IPEndPoint (IPAddress.Loopback, port), out read, out write);
			read.Open ();
			write.Open ();
			var send = new byte [] { 1, 2, 3, 4, 5 };
			Assert.That (write.Write (send), Is.EqualTo ((nint) 5));
			var result = new byte [5];
			Assert.That (read.Read (result, 5), Is.EqualTo ((nint) 5));
			for (int i = 0; i < 5; i++)
				Assert.That (result [i], Is.EqualTo (send [i] * 10));
			Assert.That (listenThread.Join (TimeSpan.FromSeconds (10)), Is.True, "listenThread.Join timed out");
			listener.Stop ();
			read.Close ();
			write.Close ();
		}

		[Test]
		public void ConnectToPeer ()
		{
			int port;
			var listener = FindPort (out port);
			if (listener is null) {
				Assert.Inconclusive ("Not possible to bind a port");
				return;
			}

			Exception ex = null;
			var thread = new Thread (() => {
				try {
					NSInputStream read = null;
					NSOutputStream write = null;

					var listenThread = new Thread (new ParameterizedThreadStart (DebugListener)) {
						IsBackground = true,
					};
					var listenThreadCompleted = false;
					try {
						listenThread.Start (listener);
						NSStream.CreatePairWithPeerSocketSignature (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, new IPEndPoint (IPAddress.Loopback, port), out read, out write);
						read.Open ();
						write.Open ();
						var send = new byte [] { 1, 2, 3, 4, 5 };
						Assert.That (write.Write (send), Is.EqualTo ((nint) 5), "Write");
						var result = new byte [5];
						Assert.That (read.Read (result, 5), Is.EqualTo ((nint) 5), "Read");
						for (int i = 0; i < 5; i++)
							Assert.That (result [i], Is.EqualTo (send [i] * 10), "Item " + i);
						listenThreadCompleted = listenThread.Join (TimeSpan.FromSeconds (5));
						Assert.That (listenThreadCompleted, Is.True, "Listener thread");
					} finally {
						listener.Stop ();
						read?.Close ();
						write?.Close ();
					}
				} catch (Exception e) {
					ex = e;
				}
			}) {
				IsBackground = true,
			};
			thread.Start ();
			Assert.That (thread.Join (TimeSpan.FromSeconds (10)), Is.True, "Background thread completion");
			Assert.That (ex, Is.Null, "No exception");
		}

		void DebugListener (object data)
		{
			var listener = data as TcpListener;
			TcpClient client;
			try {
				client = listener.AcceptTcpClient ();
			} catch (ObjectDisposedException) {
				return;
			} catch (SocketException) {
				return;
			} catch (InvalidOperationException) {
				return;
			}
			var stream = client.GetStream ();

			byte [] buffer = new byte [512];
			if (stream.Read (buffer, 0, 5) == 5) {
				stream.Write (new byte [] { 10, 20, 30, 40, 50 }, 0, 5);
				stream.Flush ();
			}
			client.Close ();
		}
	}
}
