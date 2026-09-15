//
// NSThread.cs: extensions for NSThread
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#if MONOMAC

namespace Foundation {
	public partial class NSThread {
		class ActionThread : NSThread {
			Action action;

			public ActionThread (Action action)
			{
				this.action = action;
			}

			public override void Main ()
			{
				action ();
			}
		}

		/// <summary>Starts a new thread that executes the specified action.</summary>
		/// <param name="action">The action to perform on the new thread.</param>
		/// <returns>The newly created and started thread.</returns>
		public static NSThread Start (Action action)
		{
			if (action is null) {
				throw new ArgumentNullException ("action");
			}

			var thread = new ActionThread (action);
			thread.Start ();
			return thread;
		}
	}
}

#endif // MONOMAC
