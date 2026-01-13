//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2014 Xamarin Inc
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
//
using System.Collections.Generic;

#nullable enable

namespace Foundation {

	[Register]
	internal class InternalNSNotificationHandler : NSObject {
		NSNotificationCenter? notificationCenter;
		Action<NSNotification> notify;

		public InternalNSNotificationHandler (NSNotificationCenter notificationCenter, Action<NSNotification> notify)
		{
			this.notificationCenter = notificationCenter;
			this.notify = notify;
			IsDirectBinding = false;
		}

		[Export ("post:")]
		[Preserve (Conditional = true)]
		public void Post (NSNotification s)
		{
			notify (s);
			s.Dispose ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing && notificationCenter is not null) {
				notificationCenter.RemoveObserver (this);
				notificationCenter = null;
			}
			base.Dispose (disposing);
		}
	}

	// The C# overloads
	public partial class NSNotificationCenter {
		const string postSelector = "post:";

		class ObservedData {
			public NSObject? Observer;
			public string? Name;
			public NSObject? Object;
		}

		List<ObservedData> __mt_ObserverList_var = new List<ObservedData> ();

		/// <summary>
		/// Adds an observer for the specified notification.
		/// </summary>
		/// <param name="aName">The name of the notification to observe.</param>
		/// <param name="notify">The delegate that will be invoked when the notification is posted.</param>
		/// <param name="fromObject">If not <see langword="null"/>, filters the notifications to those sent by this object.</param>
		/// <returns>An observer token that can be used later as the parameter passed to <see cref="RemoveObserver(NSObject)"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="notify"/> is <see langword="null"/>.</exception>
		public NSObject AddObserver (NSString? aName, Action<NSNotification> notify, NSObject? fromObject)
		{
			ArgumentNullException.ThrowIfNull (notify);

			var proxy = new InternalNSNotificationHandler (this, notify);

			AddObserver (proxy, new Selector (postSelector), aName, fromObject);

			return proxy;
		}

		/// <summary>
		/// Adds an observer for the specified notification.
		/// </summary>
		/// <param name="aName">The name of the notification to observe.</param>
		/// <param name="notify">The delegate that will be invoked when the notification is posted.</param>
		/// <returns>An observer token that can be used later as the parameter passed to <see cref="RemoveObserver(NSObject)"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="notify"/> is <see langword="null"/>.</exception>
		public NSObject AddObserver (NSString? aName, Action<NSNotification> notify)
		{
			return AddObserver (aName, notify, null);
		}

		/// <summary>
		/// Removes multiple observers in one call.
		/// </summary>
		/// <param name="keys">The collection of observer tokens to remove.</param>
		/// <remarks>This removes all of the observers in the <see cref="IEnumerable{NSObject}"/> parameter.</remarks>
		public void RemoveObservers (IEnumerable<NSObject>? keys)
		{
			if (keys is null)
				return;
			foreach (var k in keys)
				RemoveObserver (k);
		}

		void AddObserverToList (NSObject observer, string? aName, NSObject? anObject)
		{
			lock (__mt_ObserverList_var)
				__mt_ObserverList_var.Add (new ObservedData { Observer = observer, Name = aName, Object = anObject });
			MarkDirty ();
		}

		void RemoveObserversFromList (NSObject observer, string? aName, NSObject? anObject)
		{
			lock (__mt_ObserverList_var) {
				for (int i = __mt_ObserverList_var.Count - 1; i >= 0; i--) {
					ObservedData od = __mt_ObserverList_var [i];

					if (observer != od.Observer)
						continue;

					if (aName is not null && aName != od.Name)
						continue;

					if (anObject is not null && anObject != od.Object)
						continue;

					__mt_ObserverList_var.RemoveAt (i);
				}
			}
		}
	}

	/// <summary>
	/// Provides data for an event based on a posted <see cref="NSNotification"/> object.
	/// </summary>
	public class NSNotificationEventArgs : EventArgs {
		/// <summary>
		/// Gets the underlying <see cref="NSNotification"/> object from the posted notification.
		/// </summary>
		/// <value>The notification object.</value>
		public NSNotification Notification { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NSNotificationEventArgs"/> class.
		/// </summary>
		/// <param name="notification">The underlying <see cref="NSNotification"/> object from the posted notification.</param>
		public NSNotificationEventArgs (NSNotification notification)
		{
			Notification = notification;
		}
	}
}
