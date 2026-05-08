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

			ClassicAssert.True (contSuccess.IsCompleted, "contSuccess.IsCompleted");
			ClassicAssert.True (contFailed.IsCompleted, "contFailed.IsCompleted");
			ClassicAssert.True (contCanceled.IsCompleted, "contCanceled.IsCompleted");
			ClassicAssert.False (contSuccess.IsCanceled, "contSuccess.IsCanceled");
			ClassicAssert.True (contFailed.IsCanceled, "contFailed.IsCanceled");
			ClassicAssert.True (contCanceled.IsCanceled, "contCanceled.IsCanceled");
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

			ClassicAssert.True (ran, "ran");
			Assert.That (cont.Status, Is.EqualTo (TaskStatus.RanToCompletion), "Status");
		}
	}
}
