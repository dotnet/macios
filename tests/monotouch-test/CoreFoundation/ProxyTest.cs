//
// Unit tests for CFProxy[Settings]
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.Threading;
using System.IO;
using System.Net;

using MonoTests.System.Net.Http;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ProxyTest {
#if !MONOMAC
		LoopbackHttpServer server;
		[OneTimeSetUp]
		public void Setup ()
		{
			server = new LoopbackHttpServer (context => {
				var request = context.Request;
				var pacPath = Path.Combine (NSBundle.MainBundle.ResourcePath, request.RawUrl.Substring (1));
				pacPath = Path.GetFullPath (pacPath);
				if (!pacPath.StartsWith (Path.GetFullPath (NSBundle.MainBundle.ResourcePath) + Path.DirectorySeparatorChar))
					throw new InvalidOperationException ($"Invalid access: {request.RawUrl}");
				Console.WriteLine ($"    Serving {pacPath}");
				var buf = File.ReadAllBytes (pacPath);
				context.Response.OutputStream.Write (buf, 0, buf.Length);
			});
		}

		[OneTimeTearDown]
		public void TearDown ()
		{
			server.Dispose ();
			server = null;
		}

		[Test]
		public void TestPACParsingScript ()
		{
			// get the path for the pac file, try to parse it and ensure that 
			// our cb was called
			string pacPath = Path.Combine (NSBundle.MainBundle.ResourcePath, "example.pac");
			NSError error = null;
			var script = File.ReadAllText (pacPath);
			var targetUri = NetworkResources.XamarinUri;
			var proxies = global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationScript (script, targetUri, out error);
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies.Length, Is.EqualTo (1), "Length");
			// assert the data of the proxy, although we are really testing the js used
			Assert.That (proxies [0].Port, Is.EqualTo (8080), "Port");
		}

		[Test]
		public void TestPACParsingScriptNoProxy ()
		{
			string pacPath = Path.Combine (NSBundle.MainBundle.ResourcePath, "example.pac");
			NSError error = null;
			var script = File.ReadAllText (pacPath);
			var targetUri = NetworkResources.MicrosoftUri;
			var proxies = global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationScript (script, targetUri, out error);
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Proxies length");
			Assert.That (proxies [0].ProxyType, Is.EqualTo (CFProxyType.None));
		}

		[Test]
		public void TestPACParsingScriptError ()
		{
			NSError error = null;
			var script = "Not VALID js";
			var targetUri = NetworkResources.MicrosoftUri;
			var proxies = global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationScript (script, targetUri, out error);
			Assert.That (error, Is.Not.Null, "Not null error");
			Assert.That (proxies, Is.Null, "Null proxies");
		}

		[Test]
		public void TestPACParsingAsync ()
		{
			CFProxy [] proxies = null;
			NSError error = null;
			NSObject cbClient = null;
			bool done = false;
			string pacPath = Path.Combine (NSBundle.MainBundle.ResourcePath, "example.pac");

			var script = File.ReadAllText (pacPath);
			var targetUri = NetworkResources.XamarinUri;

			Exception ex;
			// similar to the other tests, but we want to ensure that the async/await API works
			TestRuntime.RunAsync (TimeSpan.FromSeconds (30), async () => {
				try {
					CancellationTokenSource cancelSource = new CancellationTokenSource ();
					CancellationToken cancelToken = cancelSource.Token;
					var result = await global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationScriptAsync (script, targetUri, cancelToken);
					proxies = result.proxies;
					error = result.error;
				} catch (Exception e) {
					ex = e;
				} finally {
					done = true;
				}
			}, () => done);
			Assert.That (cbClient, Is.Null, "Null client");
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Length");
			// assert the data of the proxy, although we are really testing the js used
			Assert.That (proxies [0].Port, Is.EqualTo (8080), "Port");
		}

		[Test]
		public void TestPACParsingAsyncNoProxy ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail when trying to assert they are empty.");

			CFProxy [] proxies = null;
			NSError error = null;
			NSObject cbClient = null;
			bool done = false;
			string pacPath = Path.Combine (NSBundle.MainBundle.ResourcePath, "example.pac");

			var script = File.ReadAllText (pacPath);
			var targetUri = NetworkResources.MicrosoftUri;

			Exception ex;
			// similar to the other tests, but we want to ensure that the async/await API works
			TestRuntime.RunAsync (TimeSpan.FromSeconds (30), async () => {
				try {
					CancellationTokenSource cancelSource = new CancellationTokenSource ();
					CancellationToken cancelToken = cancelSource.Token;
					var result = await global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationScriptAsync (script, targetUri, cancelToken);
					proxies = result.proxies;
					error = result.error;
				} catch (Exception e) {
					ex = e;
				} finally {
					done = true;
				}
			}, () => done);
			Assert.That (cbClient, Is.Null, "Null client");
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Proxies length");
			Assert.That (proxies [0].ProxyType, Is.EqualTo (CFProxyType.None));
		}

		[Test]
		public void TestPACParsingUrl ()
		{
			NSError error;
			var pacUri = new Uri ($"{server.BaseUrl}/example.pac");
			var targetUri = NetworkResources.XamarinUri;
			var proxies = global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationUrl (pacUri, targetUri, out error);
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies.Length, Is.EqualTo (1), "Length");
			// assert the data of the proxy, although we are really testing the js used
			Assert.That (proxies [0].Port, Is.EqualTo (8080), "Port");
		}

		[Test]
		public void TestPacParsingUrlNoProxy ()
		{
			NSError error;
			var pacUri = new Uri ($"{server.BaseUrl}/example.pac");
			var targetUri = NetworkResources.MicrosoftUri;
			var proxies = global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationUrl (pacUri, targetUri, out error);
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Proxies length");
			Assert.That (proxies [0].ProxyType, Is.EqualTo (CFProxyType.None));
		}

		[Test]
		public void TestPACParsingUrlAsync ()
		{
			CFProxy [] proxies = null;
			NSError error = null;
			NSObject cbClient = null;
			bool done = false;
			var pacUri = new Uri ($"{server.BaseUrl}/example.pac");
			var targetUri = NetworkResources.XamarinUri;

			Exception ex;
			// similar to the other tests, but we want to ensure that the async/await API works
			TestRuntime.RunAsync (TimeSpan.FromSeconds (30), async () => {
				try {
					CancellationTokenSource cancelSource = new CancellationTokenSource ();
					CancellationToken cancelToken = cancelSource.Token;
					var result = await global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationUrlAsync (pacUri, targetUri, cancelToken);
					proxies = result.proxies;
					error = result.error;
				} catch (Exception e) {
					ex = e;
				} finally {
					done = true;
				}
			}, () => done);
			Assert.That (cbClient, Is.Null, "Null client");
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Length");
			// assert the data of the proxy, although we are really testing the js used
			Assert.That (proxies [0].Port, Is.EqualTo (8080), "Port");
		}

		[Test]
		public void TestPACParsingUrlAsyncNoProxy ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail when trying to assert they are empty.");
			CFProxy [] proxies = null;
			NSError error = null;
			NSObject cbClient = null;
			bool done = false;
			var pacUri = new Uri ($"{server.BaseUrl}/example.pac");
			var targetUri = NetworkResources.MicrosoftUri;

			Exception ex;
			// similar to the other tests, but we want to ensure that the async/await API works
			TestRuntime.RunAsync (TimeSpan.FromSeconds (30), async () => {
				try {
					CancellationTokenSource cancelSource = new CancellationTokenSource ();
					CancellationToken cancelToken = cancelSource.Token;
					var result = await global::CoreFoundation.CFNetwork.ExecuteProxyAutoConfigurationUrlAsync (pacUri, targetUri, cancelToken);
					proxies = result.proxies;
					error = result.error;
				} catch (Exception e) {
					ex = e;
				} finally {
					done = true;
				}
			}, () => done);
			Assert.That (cbClient, Is.Null, "Null client");
			Assert.That (error, Is.Null, "Null error");
			Assert.That (proxies, Is.Not.Null, "Not null proxies");
			Assert.That (proxies.Length, Is.EqualTo (1), "Proxies length");
			Assert.That (proxies [0].ProxyType, Is.EqualTo (CFProxyType.None));
		}
#endif
	}
}
