using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LinkSdk {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class TaskBugsTest {

		[Test]
		public void ContinueWithDifferentOptionsAreCanceledTest ()
		{
			TaskScheduler.UnobservedTaskException += (obj, evt) => evt.SetObserved ();
			var mre = new ManualResetEventSlim ();
			var task = Task.Factory.StartNew (() => mre.Wait (200));
			var contFailed = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnFaulted);
			var contCanceled = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnCanceled);
			var contSuccess = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnRanToCompletion);

			mre.Set ();
			contSuccess.Wait (100);

			Assert.That (contSuccess.IsCompleted, Is.True, "contSuccess.IsCompleted");
			Assert.That (contFailed.IsCompleted, Is.True, "contFailed.IsCompleted");
			Assert.That (contCanceled.IsCompleted, Is.True, "contCanceled.IsCompleted");
			Assert.That (contSuccess.IsCanceled, Is.False, "contSuccess.IsCanceled");
			Assert.That (contFailed.IsCanceled, Is.True, "contFailed.IsCanceled");
			Assert.That (contCanceled.IsCanceled, Is.True, "contCanceled.IsCanceled");
		}

		[Test]
		public void ContinueWhenAll_WithMixedCompletionState ()
		{
			TaskScheduler.UnobservedTaskException += (obj, evt) => evt.SetObserved ();
			var mre = new ManualResetEventSlim ();
			var task = Task.Factory.StartNew (() => mre.Wait (200));
			var contFailed = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnFaulted);
			var contCanceled = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnCanceled);
			var contSuccess = task.ContinueWith (t => { }, TaskContinuationOptions.OnlyOnRanToCompletion);
			bool ran = false;

			var cont = Task.Factory.ContinueWhenAll (new Task [] { contFailed, contCanceled, contSuccess }, _ => ran = true);

			mre.Set ();
			cont.Wait (200);

			Assert.That (ran, Is.True, "ran");
			Assert.That (cont.Status, Is.EqualTo (TaskStatus.RanToCompletion), "Status");
		}
	}
}
