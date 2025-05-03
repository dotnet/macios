using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Windows.Input;
using System.Xml;
using Foundation;
using ObjCRuntime;
using NUnit.Framework;

namespace LinkSdk {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PclTest {

		[Test]
		public void Corlib ()
		{
			BinaryWriter bw = new BinaryWriter (Stream.Null);
			bw.Dispose ();
		}

		[Test]
		public void System ()
		{
			const string url = "http://www.google.com";
			Uri uri = new Uri (url);

			Assert.False (this is ICommand, "ICommand");

			try {
				HttpWebRequest hwr = WebRequest.CreateHttp (uri);
				try {
					Assert.True (hwr.SupportsCookieContainer, "SupportsCookieContainer");
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}

				WebResponse wr = hwr.GetResponse ();
				try {
					Assert.True (wr.SupportsHeaders, "SupportsHeaders");
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}
				wr.Dispose ();

				try {
					Assert.NotNull (WebRequest.CreateHttp (url));
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}

				try {
					Assert.NotNull (WebRequest.CreateHttp (uri));
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}
			} catch (Exception e) {
				TestRuntime.IgnoreInCIIfBadNetwork (e);
				throw;
			}
		}

		[Test]
		public void Xml ()
		{
			try {
				XmlConvert.VerifyPublicId (String.Empty);
			} catch (NotImplementedException) {
				// feature is not available, but the symbol itself is needed
			}

			try {
				XmlConvert.VerifyWhitespace (String.Empty);
			} catch (NotImplementedException) {
				// feature is not available, but the symbol itself is needed
			}

			try {
				XmlConvert.VerifyXmlChars (String.Empty);
			} catch (NotImplementedException) {
				// feature is not available, but the symbol itself is needed
			}

			var xr = XmlReader.Create (Stream.Null);
			xr.Dispose ();

			var xw = XmlWriter.Create (Stream.Null);
			xw.Dispose ();

			XmlReaderSettings xrs = new XmlReaderSettings ();
			xrs.DtdProcessing = DtdProcessing.Ignore;
		}
	}
}
