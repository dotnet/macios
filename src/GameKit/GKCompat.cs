// Copyright 2016 Xamarin Inc. All rights reserved.

using System;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace GameKit {
#if !XAMCORE_5_0
#if __IOS__ || __MACCATALYST__
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public interface IGKPeerPickerControllerDelegate : INativeObject, IDisposable {
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public static class GKPeerPickerControllerDelegate_Extensions {
		/// <param name="This">The instance on which this extension method operates.</param>
		///         <param name="picker">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void ConnectionTypeSelected (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, GKPeerPickerConnectionType type)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="This">The instance on which this extension method operates.</param>
		///         <param name="picker">To be added.</param>
		///         <param name="forType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static GKSession GetSession (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, GKPeerPickerConnectionType forType)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="This">The instance on which this extension method operates.</param>
		///         <param name="picker">To be added.</param>
		///         <param name="peerId">To be added.</param>
		///         <param name="toSession">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void PeerConnected (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker, string peerId, GKSession toSession)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="This">The instance on which this extension method operates.</param>
		///         <param name="picker">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void ControllerCancelled (this IGKPeerPickerControllerDelegate This, GKPeerPickerController picker)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	}

	/// <summary>Delegate object for the <see cref="GameKit.GKPeerPickerController" /> class.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameKit/Reference/GKPeerPickerControllerDelegate_Protocol/index.html">Apple documentation for <c>GKPeerPickerControllerDelegate</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public unsafe class GKPeerPickerControllerDelegate : NSObject, IGKPeerPickerControllerDelegate {
		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public GKPeerPickerControllerDelegate () : base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <include file="../../docs/api/GameKit/GKPeerPickerControllerDelegate.xml" path="/Documentation/Docs[@DocId='M:GameKit.GKPeerPickerControllerDelegate.#ctor(Foundation.NSObjectFlag)']/*" />
	protected GKPeerPickerControllerDelegate (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected internal GKPeerPickerControllerDelegate (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="picker">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void ConnectionTypeSelected (GKPeerPickerController picker, GKPeerPickerConnectionType type)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="picker">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void ControllerCancelled (GKPeerPickerController picker)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="picker">To be added.</param>
		///         <param name="forType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual GKSession GetSession (GKPeerPickerController picker, GKPeerPickerConnectionType forType)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <param name="picker">To be added.</param>
		///         <param name="peerId">To be added.</param>
		///         <param name="toSession">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void PeerConnected (GKPeerPickerController picker, string peerId, GKSession toSession)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	} /* class GKPeerPickerControllerDelegate */

	/// <summary>A View Controller that can be use to discover other players on other iPhones or iPads.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameKit/Reference/GKPeerPickerController_Class/index.html">Apple documentation for <c>GKPeerPickerController</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'MCBrowserViewController' from the 'MultipeerConnectivity' framework instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public class GKPeerPickerController : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new PlatformNotSupportedException (Constants.TypeUnavailable); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public GKPeerPickerController () : base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <include file="../../docs/api/GameKit/GKPeerPickerController.xml" path="/Documentation/Docs[@DocId='M:GameKit.GKPeerPickerController.#ctor(Foundation.NSObjectFlag)']/*" />
	protected GKPeerPickerController (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		protected internal GKPeerPickerController (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void Dismiss ()
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void Show ()
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual GKPeerPickerConnectionType ConnectionTypesMask {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>An instance of the GameKit.IGKPeerPickerControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the GameKit.IGKPeerPickerControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		public IGKPeerPickerControllerDelegate Delegate {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual bool Visible {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		public virtual NSObject? WeakDelegate {
			get {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeUnavailable);
			}
		}

		/// <include file="../../docs/api/GameKit/GKPeerPickerController.xml" path="/Documentation/Docs[@DocId='M:GameKit.GKPeerPickerController.Dispose(System.Boolean)']/*" />
	protected override void Dispose (bool disposing)
		{
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	} /* class GKPeerPickerController */
#endif //  __IOS__ || __MACCATALYST__
#endif // !XAMCORE_5_0
}
