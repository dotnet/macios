// 
// UIView.cs: Implements the managed UIView
//
// Authors:
//   Geoff Norton.
//     
// Copyright 2009 Novell, Inc
// Copyrigh 2014, Xamarin Inc.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace UIKit {
	public partial class UIView : IEnumerable {

		/// <param name="view">The subview to add.</param>
		///         <summary>This is an alias for <see cref="M:UIKit.UIView.AddSubview(UIKit.UIView)" />, but uses the Add pattern as it allows C# 3.0 constructs to add subviews after creating the object.</summary>
		///         <remarks>
		///           <para>
		///             This method is equivalent to <see cref="M:UIKit.UIView.AddSubview(UIKit.UIView)" /> and is present to enable C# 3.0 to add subviews at creation time.
		///           </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		///               var myView = new MyView (new RectangleF (0, 0, 320, 320)){
		///                 new ImageGallery (region [0]),
		///                 new ImageGallery (region [1]),
		///                 new UILabel (new RectangleF (10, 10, 200, 200)){
		///                   Text = "Images from our Trip"
		///                 }
		///               };
		///             ]]></code>
		///           </example>
		///         </remarks>
		public void Add (UIView view)
		{
			AddSubview (view);
		}

		/// <param name="views">An array of zero or more <see cref="T:UIKit.UIView" />s.</param>
		///         <summary>Convenience routine to add various views to a UIView.</summary>
		///         <remarks>
		///           <para>
		///             This is merely a convenience routine that allows the application developer to add a number of views in a single call.
		///           </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// myView.AddSubviews (label, button, entry, image1, image2);
		///             ]]></code>
		///           </example>
		///         </remarks>
		public void AddSubviews (params UIView []? views)
		{
			if (views is null)
				return;
			foreach (var v in views)
				AddSubview (v);
		}

		/// <summary>Returns an enumerator that lists all of the subviews in this view</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public IEnumerator GetEnumerator ()
		{
			UIView [] subviews = Subviews;
			if (subviews is null)
				yield break;
			foreach (UIView uiv in subviews)
				yield return uiv;
		}

		/// <include file="../../docs/api/UIKit/UIView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIView.BeginAnimations(System.String)']/*" />
		public static void BeginAnimations (string animation)
		{
			BeginAnimations (animation, IntPtr.Zero);
		}

		[Register]
		class _UIViewStaticCallback : NSObject {
			static _UIViewStaticCallback? shared;
			public const string start = "start";
			public const string end = "end";
			public event Action? WillStart;
			public event Action? WillEnd;

			public _UIViewStaticCallback ()
			{
				IsDirectBinding = false;
			}

			[Preserve (Conditional = true)]
			[Export ("start")]
			public void OnStart ()
			{
				if (WillStart is not null)
					WillStart ();
			}

			[Preserve (Conditional = true)]
			[Export ("end")]
			public void OnEnd ()
			{
				shared = null;
				if (WillEnd is not null)
					WillEnd ();
			}

			public static _UIViewStaticCallback Prepare ()
			{
				if (shared is null) {
					shared = new _UIViewStaticCallback ();
					SetAnimationDelegate (shared);
				}
				return shared;
			}
		}

		/// <summary>This event is raised when the animations will start.</summary>
		/// <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///           <para>Notice that these events are only fired as long as the application does not install its own animation delegate by calling <see cref="M:UIKit.UIView.SetAnimationDelegate(Foundation.NSObject)" />.</para>
		///         </remarks>
		public static event Action AnimationWillStart {
			add {
				_UIViewStaticCallback.Prepare ().WillStart += value;
			}
			remove {
				_UIViewStaticCallback.Prepare ().WillStart -= value;
			}
		}

		/// <summary>This event is raised when the animations will end.</summary>
		/// <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///           <para>Notice that these events are only fired as long as the application does not install its own animation delegate by calling <see cref="M:UIKit.UIView.SetAnimationDelegate(Foundation.NSObject)" />.</para>
		///         </remarks>
		public static event Action AnimationWillEnd {
			add {
				_UIViewStaticCallback.Prepare ().WillEnd += value;
			}
			remove {
				_UIViewStaticCallback.Prepare ().WillEnd -= value;
			}
		}

		/// <param name="duration">Duration in seconds for the animation.</param>
		///         <param name="animation">Code containing the changes that you will apply to your view.</param>
		///         <param name="completion">Code that is invoked when the animation completes.</param>
		///         <summary>Animates the property changes that take place in the specified action and invokes a completion callback when the animation completes.</summary>
		///         <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///           <para>
		///             This uses the CurveEaseOut and TransitionNone flags for the animation.
		///           </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// // Animates hiding the label by setting the alpha to zero over three seconds.
		/// UIView.Animate (3, () => { label.Alpha = 0; });
		///             ]]></code>
		///           </example>
		///         </remarks>
		[Advice ("Use the *Notify method that has 'UICompletionHandler completion' parameter, the 'bool' will tell you if the operation finished.")]
		public static void Animate (double duration, Action animation, Action completion)
		{
			// animation null check will be done in AnimateNotify
			AnimateNotify (duration, animation, (x) => {
				if (completion is not null)
					completion ();
			});
		}

		/// <param name="duration">Duration in seconds for the animation.</param>
		///         <param name="delay">Delay before the animation begins.</param>
		///         <param name="options">Animation options</param>
		///         <param name="animation">Code containing the changes that you will apply to your view.</param>
		///         <param name="completion">Code that is invoked when the animation completes.</param>
		///         <summary>Invokes animation changes to one or more views by specifying duration, delay, options, and a completion handler.</summary>
		///         <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///           <para>This method initiates a set of animations that areto be performed on this view. The action indicated in the animations parameter contains the code for the animation of the properties of one or more views.</para>
		///         </remarks>
		[Advice ("Use the *Notify method that has 'UICompletionHandler completion' parameter, the 'bool' will tell you if the operation finished.")]
		public static void Animate (double duration, double delay, UIViewAnimationOptions options, Action animation, Action completion)
		{
			// animation null check will be done in AnimateNotify
			AnimateNotify (duration, delay, options, animation, (x) => {
				if (completion is not null)
					completion ();
			});
		}

		/// <param name="fromView">The initial view.</param>
		///         <param name="toView">The final view.</param>
		///         <param name="duration">The duration, in seconds, of the animation.</param>
		///         <param name="options">A mask of options to be used with the animation.</param>
		///         <param name="completion">An action to be executed at the end of the animation.</param>
		///         <summary>Specifies a transition animation on the specified collection view.</summary>
		///         <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///         </remarks>
		[Advice ("Use the *Notify method that has 'UICompletionHandler completion' parameter, the 'bool' will tell you if the operation finished.")]
		public static void Transition (UIView fromView, UIView toView, double duration, UIViewAnimationOptions options, Action completion)
		{
			TransitionNotify (fromView, toView, duration, options, (x) => {
				if (completion is not null)
					completion ();
			});
		}

		/// <param name="withView">The view that performs the transition.</param>
		///         <param name="duration">Duration set for transition animation.</param>
		///         <param name="options">A mask of options defining animations performance.</param>
		///         <param name="animation">Action object containing  changes to make to the specified view.</param>
		///         <param name="completion">Action object for execution when the animation sequence completes.</param>
		///         <summary>Specifies a transition animation on the specified collection view.</summary>
		///         <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///         </remarks>
		[Advice ("Use the *Notify method that has 'UICompletionHandler completion' parameter, the 'bool' will tell you if the operation finished.")]
		public static void Transition (UIView withView, double duration, UIViewAnimationOptions options, Action animation, Action completion)
		{
			// animation null check will be done in AnimateNotify
			TransitionNotify (withView, duration, options, animation, (x) => {
				if (completion is not null)
					completion ();
			});
		}

		/// <param name="duration">Duration in seconds for the animation.</param>
		///         <param name="animation">Code containing the changes that you will apply to your view.</param>
		///         <summary>Animates the property changes that take place in the specified <paramref name="animation" /> as an asynchronous operation.</summary>
		///         <returns>Indicates whether the animation ran to completion or not.</returns>
		///         <remarks>
		///           <para>The use of this method is discouraged. Application developers should prefer to use the <see cref="T:UIKit.UIViewPropertyAnimator" /> class to animate UIViews.</para>
		///         </remarks>
		public static Task<bool> AnimateAsync (double duration, Action animation)
		{
			return AnimateNotifyAsync (duration, animation);
		}

		/// <param name="afterScreenUpdates">If <see langword="true" />, the capture occurs after screen updating has finished.</param>
		///         <summary>Performs a screen-capture of the <see cref="T:UIKit.UIView" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>This method is slower than <see cref="M:UIKit.UIView.SnapshotView(System.Boolean)" />.</para>
		///         </remarks>
		public UIImage Capture (bool afterScreenUpdates = true)
		{
			UIImage snapshot;
			var bounds = Bounds; // try to access objc the smalles amount of times.
			try {
				UIGraphics.BeginImageContextWithOptions (bounds.Size, Opaque, 0.0f);
				DrawViewHierarchy (bounds, afterScreenUpdates);
				snapshot = UIGraphics.GetImageFromCurrentImageContext ();
			} finally {
				UIGraphics.EndImageContext ();
			}
			return snapshot;
		}

		#region Inlined from the UITraitChangeObservable protocol
		/// <summary>
		/// Registers a callback handler that will be executed when one of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when one of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public unsafe IUITraitChangeRegistration RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
		{
			// Add an override with 'params', unfortunately this means reordering the parameters.
			return IUITraitChangeObservable._RegisterForTraitChanges (this, handler, traits);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when the specified trait changes.
		/// </summary>
		/// <typeparam name="T">The trait to observe.</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public unsafe IUITraitChangeRegistration RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T : IUITraitDefinition
		{
			return IUITraitChangeObservable._RegisterForTraitChanges<T> (this, handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public unsafe IUITraitChangeRegistration RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
		{
			return IUITraitChangeObservable._RegisterForTraitChanges<T1, T2> (this, handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <typeparam name="T3">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public unsafe IUITraitChangeRegistration RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
		{
			return IUITraitChangeObservable._RegisterForTraitChanges<T1, T2, T3> (this, handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <typeparam name="T3">A trait to observe</typeparam>
		/// <typeparam name="T4">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public unsafe IUITraitChangeRegistration RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
			where T4 : IUITraitDefinition
		{
			return IUITraitChangeObservable._RegisterForTraitChanges<T1, T2, T3, T4> (this, handler);
		}

		/// <summary>
		/// Registers a selector that will be called on the specified object when any of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="target">The object whose specified selector will be called.</param>
		/// <param name="action">The selector to call on the specified object.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, target, action);
		}

		/// <summary>
		/// Registers a selector that will be called on the current object when any of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="action">The selector to call on the current object.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="M:UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, action);
		}
		#endregion
	}
}
