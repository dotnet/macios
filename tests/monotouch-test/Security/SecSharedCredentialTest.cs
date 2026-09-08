#if __IOS__
using System.Threading;

using Security;
using Xamarin.Utils;

namespace MonoTouchFixtures.Security {

	[TestFixture]
	// we want the test to be availble if we use the linker
	[Preserve (AllMembers = true)]
	public class SecSharedCredentialTest {

		string domainName;
		string account;
		string password;
		AutoResetEvent waitEvent;

		[SetUp]
		public void SetUp ()
		{
			domainName = "com.xamarin.monotouch-test";
			account = "twitter";
			password = "12345678";
			waitEvent = new AutoResetEvent (false);
		}

		[Test]
		public void AddSharedWebCredentialNullDomain ()
		{
			domainName = null;
			Action<NSError> handler = (NSError e) => {
			};
			Assert.Throws<ArgumentNullException> (() => SecSharedCredential.AddSharedWebCredential (domainName, account, password, handler));
		}

		[Test]
		public void AddSharedWebCredentialNullAccount ()
		{
			account = null;
			Action<NSError> handler = (NSError e) => {
			};
			Assert.Throws<ArgumentNullException> (() => SecSharedCredential.AddSharedWebCredential (domainName, account, password, handler));
		}

		[Test]
		// We are testing the fact that the trampoline works.
		public void AddSharedWebCredentialNotNullPassword ()
		{
			Action<NSError> handler = (NSError e) => {
				// we do nothing, if we did block the test should be interactive because a dialog is shown.
			};
			SecSharedCredential.AddSharedWebCredential (domainName, account, password, handler);
		}

		[Test]
		// We are testing the fact that the trampoline works.
		public void AddSharedWebCredentialNullPassword ()
		{
			password = null;
			Action<NSError> handler = (NSError e) => {
				// we do nothing, if we did block the test should be interactive because a dialog is shown.
			};
			SecSharedCredential.AddSharedWebCredential (domainName, account, password, handler);
		}

		[Test]
		// We are testing the fact that the trampoline works.
		public void RequestSharedWebCredentialTest ()
		{
			Action<SecSharedCredentialInfo [], NSError> handler = (SecSharedCredentialInfo [] creds, NSError e) => {
				// we do nothing, if we did block the test should be interactive because a dialog is shown.
			};
			SecSharedCredential.RequestSharedWebCredential (domainName, account, handler);
		}

		[Test]
		// We are testing the fact that the trampoline works.
		public void RequestSharedWebCredentialNullDomainAndAccountTest ()
		{
			Action<SecSharedCredentialInfo [], NSError> handler = (SecSharedCredentialInfo [] creds, NSError e) => {
				// we do nothing, if we did block the test should be interactive because a dialog is shown.
			};
			SecSharedCredential.RequestSharedWebCredential (null, null, handler);
		}

		[Test]
		public void CreateSharedWebCredentialPassword ()
		{
			var pwd = SecSharedCredential.CreateSharedWebCredentialPassword ();
			Assert.That (pwd, Is.Not.Null);
		}

	}
}
#endif // __IOS__
