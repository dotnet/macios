//
// NSStream extensions
//
// If you add or change any of the NSStream convenience constructors, update
// the same code in CFStream.
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using CoreFoundation;
using System.Net;
using System.Net.Sockets;
using CFNetwork;

#nullable enable

namespace Foundation {
	/// <summary>The security protocol to use for an <see cref="NSStream" />.</summary>
	/// <remarks>
	///   <para>
	///     This value controls which security
	///     protocol an <see cref="NSStream" /> uses to transfer the data on the stream, from
	///     nothing, to a specific version of SSL or TLS, or best
	///     possible.
	///   </para>
	///   <para>
	///     Transport Layer Security (TLS) and its predecessor, Secure
	///     Sockets Layer (SSL), are cryptographic protocols designed to
	///     provide communication security over streams.
	///   </para>
	/// </remarks>
	public enum NSStreamSocketSecurityLevel {
		/// <summary>Do not use any security protocol.</summary>
		None,
		/// <summary>Require the use of SSLv2.</summary>
		SslV2,
		/// <summary>Require the use of SSLv3.</summary>
		SslV3,
		/// <summary>Require the use of TLSv1.</summary>
		TlsV1,
		/// <summary>Indicates that the two parties should negotiate a protocol, and it should use the most secure socket protocol available between the client and the server.</summary>
		NegotiatedSsl,
		/// <summary>The protocol is not known (can only happen if you build using an old SDK and run on a new device, and you have set the setting to "negotiated" and then you read the value back).</summary>
		Unknown,
	}

	/// <summary>Possible values for the service type for an <see cref="NSStream" />.</summary>
	/// <remarks>
	///   <para>
	///     The service type of an <see cref="NSStream" /> determine which kind of
	///     service a stream is providing. The Background and Video and
	///     VoIP affect the audio routing and can control whether an application is suspended or not.
	///   </para>
	/// </remarks>
	public enum NSStreamServiceType {
		/// <summary>Default: the stream does not support a background, video or voice operation.</summary>
		Default,
		/// <summary>This stream is used to provide VoIP traffic.</summary>
		VoIP,
		/// <summary>This stream is used to provide video.</summary>
		Video,
		/// <summary>This stream is providing a background service</summary>
		Background,
		/// <summary>This stream is used to provide voice audio.</summary>
		Voice,
	}

	/// <summary>Configuration options for SOCKS proxy servers.</summary>
	public class NSStreamSocksOptions {
		/// <summary>SOCKS proxy hostname.</summary>
		public string? HostName;
		/// <summary>SOCKS proxy port.</summary>
		public int HostPort;
		/// <summary>SOCKS proxy version (4 or 5).</summary>
		/// <remarks>Currently only the values 4 (for SOCKS4) and 5 (for SOCKS5) are supported.</remarks>
		public int Version;
		/// <summary>Username for the SOCKS proxy user.</summary>
		public string? Username;
		/// <summary>Password for the SOCKS proxy user.</summary>
		public string? Password;
	}

	public partial class NSStream {
		/// <summary>Sets a configuration key on the <see cref="NSStream" />.</summary>
		/// <param name="key">The property to set on the <see cref="NSStream" />.</param>
		/// <value>The value associated with the specified key, or <see langword="null" /> if no value is set.</value>
		/// <remarks>
		///   <para>The following is a list of possible keys that you can use, and the possible values to use:</para>
		///   <list type="table">
		///     <listheader>
		///       <term>Key</term>
		///       <description>Possible Values</description>
		///     </listheader>
		///     <item>
		///       <term><see cref="SocketSecurityLevelKey" /></term>
		///       <description>
		///       <para>The security level used by the SSL/TLS stack. It should be one of:</para>
		///       <list type="bullet">
		///         <item><term><see cref="SocketSecurityLevelNone" />: No security level set for the stream.</term></item>
		///         <item><term><see cref="SocketSecurityLevelSslV2" />: Use SSLv2 on the stream.</term></item>
		///         <item><term><see cref="SocketSecurityLevelSslV3" />: Use SSLv3 on the stream.</term></item>
		///         <item><term><see cref="SocketSecurityLevelTlsV1" />: Use TLSv1 on the stream.</term></item>
		///         <item><term><see cref="SocketSecurityLevelNegotiatedSsl" />: Use the highest possible security protocol that can be negotiated on the stream.</term></item>
		///       </list>
		///       </description>
		///     </item>
		///     <item>
		///       <term><see cref="SocksProxyConfigurationKey" /></term>
		///       <description>
		///       <para>Set to a dictionary of configuration information for a SOCKS proxy.</para>
		///       <para>The list of possible values in this dictionary include:</para>
		///       <list type="bullet">
		///         <item><term><see cref="SocksProxyHostKey" />: <see cref="NSString" /> containing the SOCKS proxy host name.</term></item>
		///         <item><term><see cref="SocksProxyPortKey" />: <see cref="NSNumber" /> containing the SOCKS proxy host port.</term></item>
		///         <item><term><see cref="SocksProxyVersionKey" />: The SOCKS proxy server version, use <see cref="SocksProxyVersion4" /> or <see cref="SocksProxyVersion5" />.</term></item>
		///         <item><term><see cref="SocksProxyUserKey" />: <see cref="NSString" /> containing the SOCKS proxy user name.</term></item>
		///         <item><term><see cref="SocksProxyPasswordKey" />: <see cref="NSString" /> containing the SOCKS proxy password.</term></item>
		///       </list>
		///       </description>
		///     </item>
		///     <item>
		///       <term><see cref="DataWrittenToMemoryStreamKey" /></term>
		///       <description>Value is the <see cref="NSData" /> that is collecting data on the stream (get only).</description>
		///     </item>
		///     <item>
		///       <term><see cref="FileCurrentOffsetKey" /></term>
		///       <description><see cref="NSNumber" /> containing the position on the stream.</description>
		///     </item>
		///     <item>
		///       <term><see cref="NetworkServiceType" /></term>
		///       <description>
		///       <para>Type of service that the stream is providing, one of:</para>
		///       <list type="bullet">
		///         <item><term><see cref="NetworkServiceTypeBackground" />: Background data.</term></item>
		///         <item><term><see cref="NetworkServiceTypeVideo" />: Video data.</term></item>
		///         <item><term><see cref="NetworkServiceTypeVoice" />: Voice service.</term></item>
		///         <item><term><see cref="NetworkServiceTypeVoIP" />: Voice Over IP.</term></item>
		///       </list>
		///       </description>
		///     </item>
		///   </list>
		/// </remarks>
		public NSObject? this [NSString key] {
			get {
				return GetProperty (key);
			}
			set {
				SetProperty (value, key);
			}
		}

		/// <summary>Gets or sets the settings for using a SOCKS proxy server.</summary>
		/// <value>An <see cref="NSStreamSocksOptions" /> object containing the SOCKS proxy configuration, or <see langword="null" /> if no SOCKS proxy is configured.</value>
		/// <remarks>
		/// <example>
		/// <code lang="csharp lang-csharp"><![CDATA[
		/// myStream.SocksOptions = new NSStreamSocksOptions () {
		///     HostName = "socks.xamarin.com",
		///     HostPort = 12765,
		///     Username = settings.User,
		///     Password = settings.Password,
		///     Version  = 5 /* SOCKS5 */
		/// };
		/// ]]></code>
		/// </example>
		/// </remarks>
		public NSStreamSocksOptions? SocksOptions {
			get {
				var d = this [SocksProxyConfigurationKey] as NSDictionary;
				if (d is null)
					return null;
				var ret = new NSStreamSocksOptions ();
				var host = d [SocksProxyHostKey] as NSString;
				var port = d [SocksProxyPortKey] as NSNumber;
				var version = d [SocksProxyVersionKey] as NSString;
				var user = d [SocksProxyUserKey] as NSString;
				var pass = d [SocksProxyPasswordKey] as NSString;
				if (host is not null)
					ret.HostName = (string) host;
				if (port is not null)
					ret.HostPort = port.Int32Value;
				if (version is not null)
					ret.Version = (version == SocksProxyVersion4) ? 4 : (version == SocksProxyVersion5 ? 5 : -1);
				if (user is not null)
					ret.Username = (string) user;
				if (pass is not null)
					ret.Password = (string) pass;
				return ret;
			}
			set {
				if (value is null) {
					this [SocksProxyConfigurationKey] = null;
					return;
				}
				var d = new NSMutableDictionary ();
				if (value.HostName is not null)
					d [SocksProxyHostKey] = new NSString (value.HostName);
				if (value.HostPort != 0)
					d [SocksProxyPortKey] = new NSNumber (value.HostPort);
				if (value.Version == 4)
					d [SocksProxyVersionKey] = SocksProxyVersion4;
				if (value.Version == 5)
					d [SocksProxyVersionKey] = SocksProxyVersion5;
				if (value.Username is not null)
					d [SocksProxyUserKey] = new NSString (value.Username);
				if (value.Password is not null)
					d [SocksProxyPasswordKey] = new NSString (value.Password);
				this [SocksProxyConfigurationKey] = d;
			}
		}

		/// <summary>Gets or sets the security protocol used for the connection (none, SSL, TLS, negotiated).</summary>
		/// <value>The default does not use any security protocols.</value>
		/// <remarks>
		///   <para>
		///     This setting allows developers to configure which security
		///     protocol is used to transfer the data on the stream, from
		///     nothing, to a specific version of Transport Layer Security
		///     (TLS) to its predecessor Secure Socket Layer (SSL) or the
		///     best possible.
		///   </para>
		///   <para>
		///     The recommended value is
		///     <see cref="NSStreamSocketSecurityLevel.NegotiatedSsl" /> which will pick
		///     the best available protocol between the client and the
		///     server.
		///   </para>
		/// </remarks>
		public NSStreamSocketSecurityLevel SocketSecurityLevel {
			get {
				var k = this [SocketSecurityLevelKey] as NSString;
				if (k == SocketSecurityLevelNone)
					return NSStreamSocketSecurityLevel.None;
				if (k == SocketSecurityLevelSslV2)
					return NSStreamSocketSecurityLevel.SslV2;
				if (k == SocketSecurityLevelSslV3)
					return NSStreamSocketSecurityLevel.SslV3;
				if (k == SocketSecurityLevelTlsV1)
					return NSStreamSocketSecurityLevel.TlsV1;
				if (k == SocketSecurityLevelNegotiatedSsl)
					return NSStreamSocketSecurityLevel.NegotiatedSsl;
				return NSStreamSocketSecurityLevel.Unknown;
			}
			set {
				NSString? v = null;
				switch (value) {
				case NSStreamSocketSecurityLevel.None:
					v = SocketSecurityLevelNone;
					break;
				case NSStreamSocketSecurityLevel.SslV2:
					v = SocketSecurityLevelSslV2;
					break;
				case NSStreamSocketSecurityLevel.SslV3:
					v = SocketSecurityLevelSslV3;
					break;
				case NSStreamSocketSecurityLevel.TlsV1:
					v = SocketSecurityLevelTlsV1;
					break;
				case NSStreamSocketSecurityLevel.NegotiatedSsl:
					v = SocketSecurityLevelNegotiatedSsl;
					break;
				}
				if (v is not null)
					this [SocketSecurityLevelKey] = v;
			}
		}

		/// <summary>Gets the <see cref="NSData" /> containing the stream data that you have written so far.</summary>
		/// <value>An <see cref="NSData" /> object containing the data written to the memory stream, or <see langword="null" /> if no data has been written.</value>
		public NSData? DataWrittenToMemoryStream {
			get {
				return this [DataWrittenToMemoryStreamKey] as NSData;
			}
		}

		/// <summary>Gets the absolute offset of the stream.</summary>
		/// <value>An <see cref="NSNumber" /> representing the current position in the stream, or <see langword="null" /> if the position is not available.</value>
		public NSNumber? FileCurrentOffset {
			get {
				return this [FileCurrentOffsetKey] as NSNumber;
			}
		}

		/// <summary>Gets or sets the service type that the stream is providing.</summary>
		/// <value>An <see cref="NSStreamServiceType" /> value indicating the service type.</value>
		/// <remarks>
		///   <para>
		///     The service type of an <see cref="NSStream" /> determines which kind of
		///     service a stream is providing. The Background, Video and
		///     VoIP types affect the audio routing and can control whether an application is suspended or not.
		///   </para>
		/// </remarks>
		public NSStreamServiceType ServiceType {
			get {
				var v = this [NetworkServiceType] as NSString;
				if (v == NetworkServiceTypeBackground)
					return NSStreamServiceType.Background;
				if (v == NetworkServiceTypeVideo)
					return NSStreamServiceType.Video;
				if (v == NetworkServiceTypeVoice)
					return NSStreamServiceType.Voice;
				if (v == NetworkServiceTypeVoIP)
					return NSStreamServiceType.VoIP;
				return NSStreamServiceType.Default;
			}
			set {
				NSString? v = null;
				switch (value) {
				case NSStreamServiceType.Background:
					v = NetworkServiceTypeBackground;
					break;
				case NSStreamServiceType.Video:
					v = NetworkServiceTypeVideo;
					break;
				case NSStreamServiceType.Voice:
					v = NetworkServiceTypeVoIP;
					break;
				case NSStreamServiceType.VoIP:
					v = NetworkServiceTypeVoIP;
					break;
				case NSStreamServiceType.Default:
					break;
				}
				this [NetworkServiceType] = v;
			}
		}

		static void AssignStreams (IntPtr read, IntPtr write,
					out NSInputStream? readStream, out NSOutputStream? writeStream)
		{
			readStream = Runtime.GetNSObject<NSInputStream> (read);
			writeStream = Runtime.GetNSObject<NSOutputStream> (write);
		}

		/// <summary>Creates a pair of <see cref="NSInputStream" /> and <see cref="NSOutputStream" /> connected to the specified socket.</summary>
		/// <param name="socket">The <see cref="CFSocket" /> to connect the streams to.</param>
		/// <param name="readStream">When this method returns, contains the created <see cref="NSInputStream" />.</param>
		/// <param name="writeStream">When this method returns, contains the created <see cref="NSOutputStream" />.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos15.0", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("maccatalyst", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("macos", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("ios15.0", Constants.UseNetworkInstead)]
		public static void CreatePairWithSocket (CFSocket socket,
							 out NSInputStream? readStream,
												 out NSOutputStream? writeStream)
		{
			ArgumentNullException.ThrowIfNull (socket);

			IntPtr read, write;
			unsafe {
				CFStream.CFStreamCreatePairWithSocket (IntPtr.Zero, socket.GetNative (), &read, &write);
			}
			AssignStreams (read, write, out readStream, out writeStream);
		}

		/// <summary>Creates a pair of <see cref="NSInputStream" /> and <see cref="NSOutputStream" /> connected to a peer socket with the specified signature.</summary>
		/// <param name="family">The address family to use.</param>
		/// <param name="type">The socket type to use.</param>
		/// <param name="proto">The protocol type to use.</param>
		/// <param name="endpoint">The IP endpoint to connect to.</param>
		/// <param name="readStream">When this method returns, contains the created <see cref="NSInputStream" />.</param>
		/// <param name="writeStream">When this method returns, contains the created <see cref="NSOutputStream" />.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos15.0", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("maccatalyst", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("macos", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("ios15.0", Constants.UseNetworkInstead)]
		public static void CreatePairWithPeerSocketSignature (AddressFamily family, SocketType type,
															  ProtocolType proto, IPEndPoint endpoint,
															  out NSInputStream? readStream,
															  out NSOutputStream? writeStream)
		{
			using (var address = new CFSocketAddress (endpoint)) {
				var sig = new CFSocketSignature (family, type, proto, address);
				IntPtr read, write;
				unsafe {
					CFStream.CFStreamCreatePairWithPeerSocketSignature (IntPtr.Zero, &sig, &read, &write);
				}
				AssignStreams (read, write, out readStream, out writeStream);
			}
		}

		/// <summary>Creates a pair of <see cref="NSInputStream" /> and <see cref="NSOutputStream" /> connected to the specified host endpoint.</summary>
		/// <param name="endpoint">The IP endpoint to connect to.</param>
		/// <param name="readStream">When this method returns, contains the created <see cref="NSInputStream" />.</param>
		/// <param name="writeStream">When this method returns, contains the created <see cref="NSOutputStream" />.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos15.0", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("maccatalyst", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("macos", Constants.UseNetworkInstead)]
		[ObsoletedOSPlatform ("ios15.0", Constants.UseNetworkInstead)]
		public static void CreatePairWithSocketToHost (IPEndPoint endpoint,
													   out NSInputStream? readStream,
													   out NSOutputStream? writeStream)
		{
			using (var host = CFHost.Create (endpoint)) {
				IntPtr read, write;
				unsafe {
					CFStream.CFStreamCreatePairWithSocketToCFHost (IntPtr.Zero, host.Handle, endpoint.Port, &read, &write);
					GC.KeepAlive (host);
				}
				AssignStreams (read, write, out readStream, out writeStream);
			}
		}

		/// <summary>Creates a pair of bound <see cref="NSInputStream" /> and <see cref="NSOutputStream" /> with the specified buffer size.</summary>
		/// <param name="readStream">When this method returns, contains the created <see cref="NSInputStream" />.</param>
		/// <param name="writeStream">When this method returns, contains the created <see cref="NSOutputStream" />.</param>
		/// <param name="bufferSize">The size of the buffer to use for the streams.</param>
		public static void CreateBoundPair (out NSInputStream? readStream, out NSOutputStream? writeStream, nint bufferSize)
		{
			IntPtr read, write;
			unsafe {
				CFStream.CFStreamCreateBoundPair (IntPtr.Zero, &read, &write, bufferSize);
			}
			AssignStreams (read, write, out readStream, out writeStream);
		}
	}
}
