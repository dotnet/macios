using System.IO;
using System.IO.Compression;

using Compression;

using DeflateStream = Compression.CompressionStream; // done to not differ from the mono tests

namespace MonoTouchFixtures.Compression {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DeflateStreamTest {
		private static void CopyStream (Stream src, Stream dest)
		{
			byte [] array = new byte [1024];
			int bytes_read;
			bytes_read = src.Read (array, 0, 1024);
			while (bytes_read != 0) {
				dest.Write (array, 0, bytes_read);
				bytes_read = src.Read (array, 0, 1024);
			}
		}

		private static bool compare_buffers (byte [] first, byte [] second, int length)
		{
			if (first.Length < length || second.Length < length) {
				return false;
			}
			for (int i = 0; i < length; i++) {
				if (first [i] != second [i]) {
					return false;
				}
			}
			return true;
		}

		[Test]
		public void Constructor_Null ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			Assert.Throws<ArgumentNullException> (() => new DeflateStream (null, CompressionMode.Compress, CompressionAlgorithm.Zlib));
		}

		[Test]
		public void Constructor_InvalidCompressionMode ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			Assert.Throws<ArgumentException> (() => new DeflateStream (new MemoryStream (), (CompressionMode) Int32.MinValue, CompressionAlgorithm.Zlib));
		}

		[TestCase (CompressionAlgorithm.LZ4)]
		[TestCase (CompressionAlgorithm.Lzfse)]
		[TestCase (CompressionAlgorithm.Lzma)]
		[TestCase (CompressionAlgorithm.Zlib)]
		public void CheckCompressDecompress (CompressionAlgorithm algorithm)
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			byte [] data = new byte [100000];
			for (int i = 0; i < 100000; i++) {
				data [i] = (byte) i;
			}
			MemoryStream dataStream = new MemoryStream (data);
			MemoryStream backing = new MemoryStream ();
			DeflateStream compressing = new DeflateStream (backing, CompressionMode.Compress, algorithm, true);
			CopyStream (dataStream, compressing);
			dataStream.Close ();
			compressing.Close ();
			backing.Seek (0, SeekOrigin.Begin);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, algorithm);
			MemoryStream output = new MemoryStream ();
			CopyStream (decompressing, output);
			Assert.That (output.Length, Is.Not.EqualTo (0), "Length should be more than 0.");
			Assert.That (compare_buffers (data, output.GetBuffer (), (int) output.Length), Is.True, "Buffers are not equal.");
			decompressing.Close ();
			output.Close ();
		}

		[Test] // Not passing the algorithm because the compressed data is Zlib compressed.
		public void CheckDecompress ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			StreamReader reader = new StreamReader (decompressing);
			Assert.That (reader.ReadLine (), Is.EqualTo ("Hello"));
			decompressing.Close ();
		}

		[Test]
		public void CheckNullRead ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<ArgumentNullException> (() => IgnoreReturnValue (decompressing.Read (null, 0, 20)));
		}

		[Test]
		public void CheckCompressingRead ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			byte [] dummy = new byte [20];
			MemoryStream backing = new MemoryStream ();
			DeflateStream compressing = new DeflateStream (backing, CompressionMode.Compress, CompressionAlgorithm.Zlib);
			Assert.Throws<InvalidOperationException> (() => IgnoreReturnValue (compressing.Read (dummy, 0, 20)));
		}

		[Test]
		public void CheckRangeRead ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			byte [] dummy = new byte [20];
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<ArgumentException> (() => IgnoreReturnValue (decompressing.Read (dummy, 10, 20)));
		}

		[Test]
		public void CheckClosedRead ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			byte [] dummy = new byte [20];
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			decompressing.Close ();
			Assert.Throws<ObjectDisposedException> (() => IgnoreReturnValue (decompressing.Read (dummy, 0, 20)));
		}

		[Test]
		public void CheckClosedFlush ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream ();
			DeflateStream compressing = new DeflateStream (backing, CompressionMode.Compress, CompressionAlgorithm.Zlib);
			compressing.Close ();
			Assert.Throws<ObjectDisposedException> (() => compressing.Flush ());
		}

		[Test]
		public void CheckSeek ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<NotSupportedException> (() => decompressing.Seek (20, SeekOrigin.Current));
		}

		[Test]
		public void CheckSetLength ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<NotSupportedException> (() => decompressing.SetLength (20));
		}

		[Test]
		public void CheckGetCanSeekProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompress = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.That (decompress.CanSeek, Is.False, "#A1");
			Assert.That (backing.CanSeek, Is.True, "#A2");
			decompress.Dispose ();
			Assert.That (decompress.CanSeek, Is.False, "#A3");
			Assert.That (backing.CanSeek, Is.False, "#A4");

			backing = new MemoryStream ();
			DeflateStream compress = new DeflateStream (backing, CompressionMode.Compress, CompressionAlgorithm.Zlib);
			Assert.That (compress.CanSeek, Is.False, "#B1");
			Assert.That (backing.CanSeek, Is.True, "#B2");
			compress.Dispose ();
			Assert.That (decompress.CanSeek, Is.False, "#B3");
			Assert.That (backing.CanSeek, Is.False, "#B4");
		}

		[Test]
		public void CheckGetCanReadProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompress = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.That (decompress.CanRead, Is.True, "#A1");
			Assert.That (backing.CanRead, Is.True, "#A2");
			decompress.Dispose ();
			Assert.That (decompress.CanRead, Is.False, "#A3");
			Assert.That (backing.CanRead, Is.False, "#A4");

			backing = new MemoryStream ();
			DeflateStream compress = new DeflateStream (backing, CompressionMode.Compress, CompressionAlgorithm.Zlib);
			Assert.That (compress.CanRead, Is.False, "#B1");
			Assert.That (backing.CanRead, Is.True, "#B2");
			compress.Dispose ();
			Assert.That (decompress.CanRead, Is.False, "#B3");
			Assert.That (backing.CanRead, Is.False, "#B4");
		}

		[Test]
		public void CheckGetCanWriteProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream ();
			DeflateStream decompress = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.That (decompress.CanWrite, Is.False, "#A1");
			Assert.That (backing.CanWrite, Is.True, "#A2");
			decompress.Dispose ();
			Assert.That (decompress.CanWrite, Is.False, "#A3");
			Assert.That (backing.CanWrite, Is.False, "#A4");

			backing = new MemoryStream ();
			DeflateStream compress = new DeflateStream (backing, CompressionMode.Compress, CompressionAlgorithm.Zlib);
			Assert.That (compress.CanWrite, Is.True, "#B1");
			Assert.That (backing.CanWrite, Is.True, "#B2");
			compress.Dispose ();
			Assert.That (decompress.CanWrite, Is.False, "#B3");
			Assert.That (backing.CanWrite, Is.False, "#B4");
		}

		[Test]
		public void CheckSetLengthProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<NotSupportedException> (() => decompressing.SetLength (20));
		}

		[Test]
		public void CheckGetLengthProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<NotSupportedException> (() => { var length = decompressing.Length; });
		}

		[Test]
		public void CheckGetPositionProp ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			Assert.Throws<NotSupportedException> (() => { var position = decompressing.Position; });
		}

		[Test]
		public void DisposeTest ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			MemoryStream backing = new MemoryStream (compressed_data);
			DeflateStream decompress = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib);
			decompress.Dispose ();
			decompress.Dispose ();
		}

		static byte [] compressed_data = { 0xf3, 0x48, 0xcd, 0xc9, 0xc9,
			0xe7, 0x02, 0x00 };


		[TestCase (CompressionAlgorithm.LZ4)]
		[TestCase (CompressionAlgorithm.Lzfse)]
		[TestCase (CompressionAlgorithm.Lzma)]
		[TestCase (CompressionAlgorithm.Zlib)]
		public void JunkAtTheEnd (CompressionAlgorithm algorithm)
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			// Write a deflated stream, then some additional data...
			using (MemoryStream ms = new MemoryStream ()) {
				// The compressed stream
				using (DeflateStream stream = new DeflateStream (ms, CompressionMode.Compress, algorithm, true)) {
					stream.WriteByte (1);
					stream.Flush ();
				}
				// Junk
				ms.WriteByte (2);

				ms.Position = 0;
				// Reading: this should not hang
				using (DeflateStream stream = new DeflateStream (ms, CompressionMode.Decompress, algorithm)) {
					byte [] buffer = new byte [512];
					int len = stream.Read (buffer, 0, buffer.Length);
					Console.WriteLine (len == 1);
				}
			}
		}

		class Bug19313Stream : MemoryStream {
			public Bug19313Stream (byte [] buffer)
				: base (buffer)
			{
			}

			public override int Read (byte [] buffer, int offset, int count)
			{
				// Thread was blocking when DeflateStream uses a NetworkStream.
				// Because the NetworkStream.Read calls Socket.Receive that
				// blocks the thread waiting for at least a byte to return.
				// This assert guarantees that Read is called only when there 
				// is something to be read.
				Assert.That (Position < Length, Is.True, "Trying to read empty stream.");

				return base.Read (buffer, offset, count);
			}
		}

		[Test] // zlib specific test
		public void Bug19313 ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
			byte [] buffer = new byte [512];
			using (var backing = new Bug19313Stream (compressed_data))
			using (var decompressing = new DeflateStream (backing, CompressionMode.Decompress, CompressionAlgorithm.Zlib))
				IgnoreReturnValue (decompressing.Read (buffer, 0, buffer.Length));
		}

		// Call this with the return value from CompressionStream.Read to avoid:
		//     error CA2022: Avoid inexact read with 'Compression.CompressionStream.Read(byte[], int, int)' (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca2022)
		void IgnoreReturnValue (int value)
		{
		}
	}
}
