using System.IO;
using System.Net;
using System.ServiceModel;
using System.Windows.Input;
using System.Xml;

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

			ClassicAssert.False (this is ICommand, "ICommand");

			try {
				HttpWebRequest hwr = WebRequest.CreateHttp (uri);
				try {
					ClassicAssert.True (hwr.SupportsCookieContainer, "SupportsCookieContainer");
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}

				WebResponse wr = hwr.GetResponse ();
				try {
					ClassicAssert.True (wr.SupportsHeaders, "SupportsHeaders");
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}
				wr.Dispose ();

				try {
					ClassicAssert.NotNull (WebRequest.CreateHttp (url));
				} catch (NotImplementedException) {
					// feature is not available, but the symbol itself is needed
				}

				try {
					ClassicAssert.NotNull (WebRequest.CreateHttp (uri));
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
