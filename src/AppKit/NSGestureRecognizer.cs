// 
// NSGestureRecognizer: Implements some helper methods for NSGestureRecognizer
//
// Authors:
//   Timothy Risi
//     
// Copyright 2014 Xamarin Inc. All rights reserved
//

#if !__MACCATALYST__

using System;
using System.Collections;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

#nullable enable

namespace AppKit {
	public partial class NSGestureRecognizer {
		object? recognizers;
		static Selector tsel = new Selector ("target");
		internal static Selector ParametrizedSelector = new Selector ("target:");

		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSGestureRecognizer (Action action) : this (tsel, new ParameterlessDispatch (action))
		{
		}

		//
		// Signature swapped, this is only used so we can store the "token" in recognizers
		//
		/// <param name="sel">To be added.</param>
		///         <param name="token">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSGestureRecognizer (Selector sel, Token token) : this (token, sel)
		{
			recognizers = token;
			MarkDirty ();
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[Register ("__NSGestureRecognizerToken")]
		[Preserve (Conditional = true)]
		public class Token : NSObject {
			/// <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			public Token ()
			{
				IsDirectBinding = false;
			}
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[Register ("__NSGestureRecognizerParameterlessToken")]
		[Preserve (Conditional = true)]
		public class ParameterlessDispatch : Token {
			Action action;

			internal ParameterlessDispatch (Action action)
			{
				this.action = action;
			}

			/// <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			[Export ("target")]
			[Preserve (Conditional = true)]
			public void Activated ()
			{
				action ();
			}
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[Register ("__NSGestureRecognizerParametrizedToken")]
		[Preserve (Conditional = true)]
		public class ParametrizedDispatch : Token {
			Action<NSGestureRecognizer> action;

			internal ParametrizedDispatch (Action<NSGestureRecognizer> action)
			{
				this.action = action;
			}

			/// <param name="sender">To be added.</param>
			///         <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}

	public partial class NSClickGestureRecognizer : NSGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSClickGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSClickGestureRecognizer (Action<NSClickGestureRecognizer> action) : base (NSGestureRecognizer.ParametrizedSelector, new Callback (action)) { }

		[Register ("__NSClickGestureRecognizer")]
		[Preserve (Conditional = true)]
		class Callback : Token {
			Action<NSClickGestureRecognizer> action;

			internal Callback (Action<NSClickGestureRecognizer> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSClickGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}

	public partial class NSMagnificationGestureRecognizer : NSGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMagnificationGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMagnificationGestureRecognizer (Action<NSMagnificationGestureRecognizer> action) : base (NSGestureRecognizer.ParametrizedSelector, new Callback (action)) { }

		[Register ("__NSMagnificationGestureRecognizer")]
		[Preserve (Conditional = true)]
		class Callback : Token {
			Action<NSMagnificationGestureRecognizer> action;

			internal Callback (Action<NSMagnificationGestureRecognizer> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSMagnificationGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}

	public partial class NSPanGestureRecognizer : NSGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPanGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPanGestureRecognizer (Action<NSPanGestureRecognizer> action) : base (NSGestureRecognizer.ParametrizedSelector, new Callback (action)) { }

		[Register ("__NSPanGestureRecognizer")]
		[Preserve (Conditional = true)]
		class Callback : Token {
			Action<NSPanGestureRecognizer> action;

			internal Callback (Action<NSPanGestureRecognizer> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSPanGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}

	public partial class NSPressGestureRecognizer : NSGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPressGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPressGestureRecognizer (Action<NSPressGestureRecognizer> action) : base (NSGestureRecognizer.ParametrizedSelector, new Callback (action)) { }

		[Register ("__NSPressGestureRecognizer")]
		[Preserve (Conditional = true)]
		class Callback : Token {
			Action<NSPressGestureRecognizer> action;

			internal Callback (Action<NSPressGestureRecognizer> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSPressGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}

	public partial class NSRotationGestureRecognizer : NSGestureRecognizer {
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSRotationGestureRecognizer (Action action) : base (action) { }
		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSRotationGestureRecognizer (Action<NSRotationGestureRecognizer> action) : base (NSGestureRecognizer.ParametrizedSelector, new Callback (action)) { }

		[Register ("__NSRotationGestureRecognizer")]
		[Preserve (Conditional = true)]
		class Callback : Token {
			Action<NSRotationGestureRecognizer> action;

			internal Callback (Action<NSRotationGestureRecognizer> action)
			{
				this.action = action;
			}

			[Export ("target:")]
			[Preserve (Conditional = true)]
			public void Activated (NSRotationGestureRecognizer sender)
			{
				action (sender);
			}
		}
	}
}
#endif // !__MACCATALYST__
