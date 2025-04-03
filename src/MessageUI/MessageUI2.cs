//
// MessageUI.cs: This file describes the API that the generator will produce for MessageUI
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace MessageUI {

	public class MFComposeResultEventArgs : EventArgs {
		/// <param name="controller">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Initializes a new instance of the MFComposeResultEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public MFComposeResultEventArgs (MFMailComposeViewController controller, MFMailComposeResult result, NSError? error)
		{
			Result = result;
			Error = error;
			Controller = controller;
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MFMailComposeResult Result { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSError? Error { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MFMailComposeViewController Controller { get; private set; }
	}

	public partial class MFMailComposeViewController {
		Mono_MFMailComposeViewControllerDelegate EnsureDelegate ()
		{
			NSObject? del = WeakMailComposeDelegate;
			if (del is null || (!(del is Mono_MFMailComposeViewControllerDelegate))) {
				del = new Mono_MFMailComposeViewControllerDelegate ();
				WeakMailComposeDelegate = del;
			}
			return (Mono_MFMailComposeViewControllerDelegate) del;
		}

		public event EventHandler<MFComposeResultEventArgs> Finished {
			add {
				EnsureDelegate ().cbFinished += value;
			}

			remove {
				EnsureDelegate ().cbFinished -= value;
			}
		}
	}

	class Mono_MFMailComposeViewControllerDelegate : MFMailComposeViewControllerDelegate {
		internal EventHandler<MFComposeResultEventArgs>? cbFinished;

		public Mono_MFMailComposeViewControllerDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override void Finished (MFMailComposeViewController controller, MFMailComposeResult result, NSError? error)
		{
			if (cbFinished is not null)
				cbFinished (controller, new MFComposeResultEventArgs (controller, result, error));
		}
	}


	public class MFMessageComposeResultEventArgs : EventArgs {
		/// <param name="controller">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <summary>Initializes a new instance of the MFMessageComposeResultEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public MFMessageComposeResultEventArgs (MFMessageComposeViewController controller, MessageComposeResult result)
		{
			Result = result;
			Controller = controller;
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MessageComposeResult Result { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MFMessageComposeViewController Controller { get; private set; }
	}


	public partial class MFMessageComposeViewController {
		Mono_MFMessageComposeViewControllerDelegate EnsureDelegate ()
		{
			NSObject? del = WeakMessageComposeDelegate;
			if (del is null || (!(del is Mono_MFMessageComposeViewControllerDelegate))) {
				del = new Mono_MFMessageComposeViewControllerDelegate ();
				WeakMessageComposeDelegate = del;
			}
			return (Mono_MFMessageComposeViewControllerDelegate) del;
		}

		public event EventHandler<MFMessageComposeResultEventArgs> Finished {
			add {
				EnsureDelegate ().cbFinished += value;
			}

			remove {
				EnsureDelegate ().cbFinished -= value;
			}
		}
	}

	class Mono_MFMessageComposeViewControllerDelegate : MFMessageComposeViewControllerDelegate {
		internal EventHandler<MFMessageComposeResultEventArgs>? cbFinished;

		public Mono_MFMessageComposeViewControllerDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override void Finished (MFMessageComposeViewController controller, MessageComposeResult result)
		{
			if (cbFinished is not null)
				cbFinished (controller, new MFMessageComposeResultEventArgs (controller, result));
		}
	}


}
