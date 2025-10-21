#if __MACOS__
using System.Threading.Tasks;

using AppKit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTableViewTests {
		[Test]
		public void NSTableView_DelegateDataSourceNull ()
		{
			NSTableView v = new NSTableView ();
			v.WeakDelegate = null;
			v.Delegate = null;
			v.WeakDataSource = null;
			v.DataSource = null;
		}
	}
}

#endif // __MACOS__
