//
// UITraitChangeObservable.cs: support for IUITraitChangeObservable
//
// Authors:
//   Rolf Bjarne Kvinge
//
// Copyright 2023 Microsoft Corp. All rights reserved.
//

using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable

namespace UIKit {
	public partial interface IUITraitChangeObservable {
#if XAMCORE_5_0
		private static Class [] ToClasses (params Type [] traits)
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static Class [] ToClasses (params Type [] traits)
#endif
		{
			if (traits is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (traits));

			return Class.FromTypes (traits);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when one of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return _RegisterForTraitChanges (this, traits, handler);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return _RegisterForTraitChanges (This, ToClasses (traits), handler);
		}

		/// <summary>Registers a callback handler that runs when any of the specified traits changes.</summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when a trait changes.</param>
		/// <returns>A token that keeps this observable alive until disposed or passed to <see cref="UnregisterForTraitChanges" />.</returns>
		[RequiredMember]
		[Export ("registerForTraitChanges:withHandler:")]
		public IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return _RegisterForTraitChanges (this, traits, handler);
		}

		[UnmanagedCallersOnly]
		static void TraitChangeHandler (IntPtr block, NativeHandle environment, NativeHandle previousCollection)
		{
			var handler = BlockLiteral.GetTarget<Action<IUITraitEnvironment, UITraitCollection>> (block);
			if (handler is not null) {
				var observable = Runtime.GetINativeObject<IUITraitEnvironment> (environment, false)
					?? throw new InvalidOperationException ("The trait change callback has no environment.");
				var collection = Runtime.GetNSObject<UITraitCollection> (previousCollection)
					?? throw new InvalidOperationException ("The trait change callback has no previous collection.");
				handler (observable, collection);
			}
		}

		internal static unsafe IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler, NSObject? super = null)
		{
			UIApplication.EnsureUIThread ();
			ArgumentNullException.ThrowIfNull (traits);
			ArgumentNullException.ThrowIfNull (handler);
			using var array = NSArray.FromNSObjects (traits);
			delegate* unmanaged<IntPtr, NativeHandle, NativeHandle, void> trampoline = &TraitChangeHandler;
			using var block = new BlockLiteral (trampoline, handler, typeof (IUITraitChangeObservable), nameof (TraitChangeHandler));
			var selector = Selector.GetHandle ("registerForTraitChanges:withHandler:");
			NativeHandle handle;
			if (super is null) {
				handle = Messaging.NativeHandle_objc_msgSend_NativeHandle_NativeHandle (This.Handle, selector, array.Handle, (IntPtr) (&block));
			} else {
				var objcSuper = new ObjCSuper (super);
				handle = Messaging.NativeHandle_objc_msgSendSuper_NativeHandle_NativeHandle (&objcSuper, selector, array.Handle, (IntPtr) (&block));
			}
			GC.KeepAlive (This);
			GC.KeepAlive (super);
			return CreateRegistration (This, handle);
		}

		/// <summary>Registers a selector on the specified target to be called when any of the specified traits changes.</summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="target">The object on which to invoke the selector.</param>
		/// <param name="action">The selector to invoke.</param>
		/// <returns>A token that keeps this observable alive until disposed or passed to <see cref="UnregisterForTraitChanges" />.</returns>
		[RequiredMember]
		[Export ("registerForTraitChanges:withTarget:action:")]
		public IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
		{
			return _RegisterForTraitChanges (this, traits, target, action);
		}

		internal static unsafe IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Class [] traits, NSObject target, Selector action, NSObject? super = null)
		{
			UIApplication.EnsureUIThread ();
			ArgumentNullException.ThrowIfNull (traits);
			var targetHandle = target.GetNonNullHandle (nameof (target));
			var actionHandle = action.GetNonNullHandle (nameof (action));
			using var array = NSArray.FromNSObjects (traits);
			var selector = Selector.GetHandle ("registerForTraitChanges:withTarget:action:");
			NativeHandle handle;
			if (super is null) {
				handle = Messaging.NativeHandle_objc_msgSend_NativeHandle_NativeHandle_NativeHandle (This.Handle, selector, array.Handle, targetHandle, actionHandle);
			} else {
				var objcSuper = new ObjCSuper (super);
				handle = Messaging.NativeHandle_objc_msgSendSuper_NativeHandle_NativeHandle_NativeHandle (&objcSuper, selector, array.Handle, targetHandle, actionHandle);
			}
			GC.KeepAlive (This);
			GC.KeepAlive (super);
			GC.KeepAlive (target);
			GC.KeepAlive (action);
			return CreateRegistration (This, handle);
		}

		/// <summary>Registers a selector on this observable to be called when any of the specified traits changes.</summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="action">The selector to invoke.</param>
		/// <returns>A token that keeps this observable alive until disposed or passed to <see cref="UnregisterForTraitChanges" />.</returns>
		[RequiredMember]
		[Export ("registerForTraitChanges:withAction:")]
		public IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Selector action)
		{
			return _RegisterForTraitChanges (this, traits, action);
		}

		internal static unsafe IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Class [] traits, Selector action, NSObject? super = null)
		{
			UIApplication.EnsureUIThread ();
			ArgumentNullException.ThrowIfNull (traits);
			var actionHandle = action.GetNonNullHandle (nameof (action));
			using var array = NSArray.FromNSObjects (traits);
			var selector = Selector.GetHandle ("registerForTraitChanges:withAction:");
			NativeHandle handle;
			if (super is null) {
				handle = Messaging.NativeHandle_objc_msgSend_NativeHandle_NativeHandle (This.Handle, selector, array.Handle, actionHandle);
			} else {
				var objcSuper = new ObjCSuper (super);
				handle = Messaging.NativeHandle_objc_msgSendSuper_NativeHandle_NativeHandle (&objcSuper, selector, array.Handle, actionHandle);
			}
			GC.KeepAlive (This);
			GC.KeepAlive (super);
			GC.KeepAlive (action);
			return CreateRegistration (This, handle);
		}

		static IUITraitChangeRegistration CreateRegistration (IUITraitChangeObservable observable, NativeHandle handle)
		{
			var registration = Runtime.GetINativeObject<IUITraitChangeRegistration> (handle, false)
				?? throw new InvalidOperationException ("UIKit returned a null trait change registration.");
			return new UITraitChangeRegistrationToken (observable, registration);
		}

		/// <summary>Unregisters a trait-change callback and releases the registration's reference to this observable.</summary>
		/// <param name="registration">The token returned when the callback was registered.</param>
		[RequiredMember]
		[Export ("unregisterForTraitChanges:")]
		public void UnregisterForTraitChanges (IUITraitChangeRegistration registration)
		{
			UnregisterForTraitChangesInternal (this, registration);
		}

		internal static unsafe void UnregisterForTraitChangesInternal (IUITraitChangeObservable This, IUITraitChangeRegistration registration, NSObject? super = null)
		{
			UIApplication.EnsureUIThread ();
			if (registration is UITraitChangeRegistrationToken token) {
				token.Unregister (This, super);
				return;
			}

			var handle = registration.GetNonNullHandle (nameof (registration));
			var selector = Selector.GetHandle ("unregisterForTraitChanges:");
			if (super is null) {
				Messaging.void_objc_msgSend_NativeHandle (This.Handle, selector, handle);
			} else {
				var objcSuper = new ObjCSuper (super);
				Messaging.void_objc_msgSendSuper_NativeHandle (&objcSuper, selector, handle);
			}
			GC.KeepAlive (This);
			GC.KeepAlive (super);
			GC.KeepAlive (registration);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when one of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
		{
			return _RegisterForTraitChanges (this, handler, traits);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
		{
			// Add an override with 'params', unfortunately this means reordering the parameters.
			return _RegisterForTraitChanges (This, ToClasses (traits), handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when the specified trait changes.
		/// </summary>
		/// <typeparam name="T">The trait to observe.</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T : IUITraitDefinition
		{
			return _RegisterForTraitChanges<T> (this, handler);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges<T> (IUITraitChangeObservable This, Action<IUITraitEnvironment, UITraitCollection> handler)
			where T : IUITraitDefinition
		{
			return _RegisterForTraitChanges (This, ToClasses (typeof (T)), handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
		{
			return _RegisterForTraitChanges<T1, T2> (this, handler);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges<T1, T2> (IUITraitChangeObservable This, Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
		{
			return _RegisterForTraitChanges (This, ToClasses (typeof (T1), typeof (T2)), handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <typeparam name="T3">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
		{
			return _RegisterForTraitChanges<T1, T2, T3> (this, handler);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges<T1, T2, T3> (IUITraitChangeObservable This, Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
		{
			return _RegisterForTraitChanges (This, ToClasses (typeof (T1), typeof (T2), typeof (T3)), handler);
		}

		/// <summary>
		/// Registers a callback handler that will be executed when any of the specified traits changes.
		/// </summary>
		/// <typeparam name="T1">A trait to observe</typeparam>
		/// <typeparam name="T2">A trait to observe</typeparam>
		/// <typeparam name="T3">A trait to observe</typeparam>
		/// <typeparam name="T4">A trait to observe</typeparam>
		/// <param name="handler">The callback to execute when any of the specified traits changes.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
			where T4 : IUITraitDefinition
		{
			return _RegisterForTraitChanges<T1, T2, T3, T3> (this, handler);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges<T1, T2, T3, T4> (IUITraitChangeObservable This, Action<IUITraitEnvironment, UITraitCollection> handler)
			where T1 : IUITraitDefinition
			where T2 : IUITraitDefinition
			where T3 : IUITraitDefinition
			where T4 : IUITraitDefinition
		{
			return _RegisterForTraitChanges (This, ToClasses (typeof (T1), typeof (T2), typeof (T3), typeof (T4)), handler);
		}

		/// <summary>
		/// Registers a selector that will be called on the specified object when any of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="target">The object whose specified selector will be called.</param>
		/// <param name="action">The selector to call on the specified object.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
		{
			return _RegisterForTraitChanges (this, traits, target, action);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Type [] traits, NSObject target, Selector action)
		{
			return _RegisterForTraitChanges (This, ToClasses (traits), target, action);
		}

		/// <summary>
		/// Registers a selector that will be called on the current object when any of the specified traits changes.
		/// </summary>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="action">The selector to call on the current object.</param>
		/// <returns>A token that can be used to unregister the callback by calling <see cref="UnregisterForTraitChanges" />.</returns>
		public IUITraitChangeRegistration RegisterForTraitChanges (Type [] traits, Selector action)
		{
			return _RegisterForTraitChanges (this, traits, action);
		}

		internal static IUITraitChangeRegistration _RegisterForTraitChanges (IUITraitChangeObservable This, Type [] traits, Selector action)
		{
			return _RegisterForTraitChanges (This, ToClasses (traits), action);
		}

		sealed class UITraitChangeRegistrationToken : IUITraitChangeRegistration, IDisposable {
			GCHandle observable;
			IUITraitChangeRegistration? registration;

			public NativeHandle Handle => registration?.Handle ?? NativeHandle.Zero;

			public UITraitChangeRegistrationToken (IUITraitChangeObservable observable, IUITraitChangeRegistration registration)
			{
				this.observable = GCHandle.Alloc (observable);
				this.registration = registration;
			}

			public NSObject Copy (NSZone? zone)
			{
				if (registration is null)
					throw new ObjectDisposedException (nameof (UITraitChangeRegistrationToken));
				return registration.Copy (zone);
			}

			~UITraitChangeRegistrationToken ()
			{
				Runtime.NSLog ("Warning: trait change registration object was not disposed manually with Dispose()");
				Dispose (false);
			}

			public void Dispose ()
			{
				Dispose (true);
				GC.SuppressFinalize (this);
			}

			internal void Unregister (IUITraitChangeObservable observable, NSObject? super)
			{
				if (registration is null)
					return;

				// The public override has already run; continue its base/native path.
				UnregisterForTraitChangesInternal (observable, registration, super);
				Dispose (false);
				GC.SuppressFinalize (this);
			}

			void Dispose (bool disposing)
			{
				if (registration is null)
					return;

				if (disposing) {
					var observable = this.observable.Target as IUITraitChangeObservable;
					if (observable is null)
						throw new InvalidOperationException ("The trait change observable has been collected.");
					observable.UnregisterForTraitChanges (registration);
				}
				// UIKit unregisters automatically when the observable is deallocated.
				// The finalizer must not call UIKit's main-thread-only unregister method.
				registration = null;
				this.observable.Free ();
			}
		}

#if XAMCORE_5_0
		private static Class [] ToClasses (IUITraitDefinition [] traits)
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static Class [] ToClasses (IUITraitDefinition [] traits)
#endif
		{
			if (traits is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (traits));
			var traitsClasses = new Class [traits.Length];
			for (var i = 0; i < traits.Length; i++)
				traitsClasses [i] = new Class (traits [i].GetType ());
			return traitsClasses;
		}

#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.")]
		public IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return _RegisterForTraitChanges (this, ToClasses (traits), handler);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.")]
		public IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return _RegisterForTraitChanges (this, ToClasses (traits), target, action);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.")]
		public IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Selector action)
		{
			return _RegisterForTraitChanges (this, ToClasses (traits), action);
		}
#endif // !XACMORE_5_0

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
#if XAMCORE_5_0
		private extern static NativeHandle NativeHandle_objc_msgSend_NativeHandle_NativeHandle (IntPtr receiver, IntPtr selector, NativeHandle arg1, NativeHandle arg2);
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		public extern static NativeHandle NativeHandle_objc_msgSend_NativeHandle_NativeHandle (IntPtr receiver, IntPtr selector, NativeHandle arg1, NativeHandle arg2);
#endif

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
#if XAMCORE_5_0
		private extern unsafe static NativeHandle NativeHandle_objc_msgSend_NativeHandle_BlockLiteral (IntPtr receiver, IntPtr selector, NativeHandle arg1, BlockLiteral* arg2);
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		public extern unsafe static NativeHandle NativeHandle_objc_msgSend_NativeHandle_BlockLiteral (IntPtr receiver, IntPtr selector, NativeHandle arg1, BlockLiteral* arg2);
#endif

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
#if XAMCORE_5_0
		private extern static NativeHandle NativeHandle_objc_msgSend_NativeHandle_NativeHandle_NativeHandle (IntPtr receiver, IntPtr selector, NativeHandle arg1, NativeHandle arg2, NativeHandle arg3);
#else
		[EditorBrowsable (EditorBrowsableState.Never)]
		public extern static NativeHandle NativeHandle_objc_msgSend_NativeHandle_NativeHandle_NativeHandle (IntPtr receiver, IntPtr selector, NativeHandle arg1, NativeHandle arg2, NativeHandle arg3);
#endif
	}

	public static partial class UITraitChangeObservable_Extensions {
		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Action{IUITraitEnvironment, UITraitCollection})" />
		/// <param name="This">The observable on which to register the callback.</param>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="handler">The callback to execute when a trait changes.</param>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, traits, handler);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], NSObject, Selector)" />
		/// <param name="This">The observable on which to register the callback.</param>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="target">The object on which to invoke the selector.</param>
		/// <param name="action">The selector to invoke.</param>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, Class [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, traits, target, action);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Selector)" />
		/// <param name="This">The observable on which to register the callback.</param>
		/// <param name="traits">The traits to observe.</param>
		/// <param name="action">The selector to invoke.</param>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, Class [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, traits, action);
		}
	}

	public partial class UIView {
		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Action{IUITraitEnvironment, UITraitCollection})" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withHandler:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, handler, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], NSObject, Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withTarget:action:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, target, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withAction:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.UnregisterForTraitChanges" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("unregisterForTraitChanges:")]
		public virtual void UnregisterForTraitChanges (IUITraitChangeRegistration registration)
		{
			IUITraitChangeObservable.UnregisterForTraitChangesInternal (this, registration, IsDirectBinding ? null : this);
		}
	}

	public partial class UIViewController {
		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Action{IUITraitEnvironment, UITraitCollection})" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withHandler:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, handler, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], NSObject, Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withTarget:action:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, target, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withAction:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.UnregisterForTraitChanges" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("unregisterForTraitChanges:")]
		public virtual void UnregisterForTraitChanges (IUITraitChangeRegistration registration)
		{
			IUITraitChangeObservable.UnregisterForTraitChangesInternal (this, registration, IsDirectBinding ? null : this);
		}
	}

	public partial class UIWindowScene {
		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Action{IUITraitEnvironment, UITraitCollection})" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withHandler:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, handler, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], NSObject, Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withTarget:action:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, target, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withAction:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.UnregisterForTraitChanges" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("unregisterForTraitChanges:")]
		public virtual void UnregisterForTraitChanges (IUITraitChangeRegistration registration)
		{
			IUITraitChangeObservable.UnregisterForTraitChangesInternal (this, registration, IsDirectBinding ? null : this);
		}
	}

	public partial class UIPresentationController {
		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Action{IUITraitEnvironment, UITraitCollection})" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withHandler:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, handler, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], NSObject, Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withTarget:action:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, target, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.RegisterForTraitChanges(Class[], Selector)" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("registerForTraitChanges:withAction:")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (Class [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, traits, action, IsDirectBinding ? null : this);
		}

		/// <inheritdoc cref="IUITraitChangeObservable.UnregisterForTraitChanges" />
		[SupportedOSPlatform ("ios17.0"), SupportedOSPlatform ("tvos17.0"), SupportedOSPlatform ("maccatalyst17.0")]
		[Export ("unregisterForTraitChanges:")]
		public virtual void UnregisterForTraitChanges (IUITraitChangeRegistration registration)
		{
			IUITraitChangeObservable.UnregisterForTraitChangesInternal (this, registration, IsDirectBinding ? null : this);
		}
	}

#if !XAMCORE_5_0
	public partial class UIPresentationController {
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), handler);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), target, action);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), action);
		}
	}

	public partial class UIView {
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), handler);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), target, action);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), action);
		}
	}

	public partial class UIViewController {
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), handler);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), target, action);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), action);
		}
	}

	public partial class UIWindowScene {
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), handler);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), target, action);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public virtual IUITraitChangeRegistration RegisterForTraitChanges (IUITraitDefinition [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (this, IUITraitChangeObservable.ToClasses (traits), action);
		}
	}

	public static partial class UITraitChangeObservable_Extensions {
		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Action<IUITraitEnvironment, UITraitCollection>)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, IUITraitDefinition [] traits, global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, IUITraitChangeObservable.ToClasses (traits), handler);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], NSObject, Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, IUITraitDefinition [] traits, NSObject target, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, IUITraitChangeObservable.ToClasses (traits), target, action);
		}

		[Obsolete ("Use the 'UITraitChangeObservable.RegisterForTraitChanges (Class[], Selector)' method instead.", false)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IUITraitChangeRegistration RegisterForTraitChanges (this IUITraitChangeObservable This, IUITraitDefinition [] traits, Selector action)
		{
			return IUITraitChangeObservable._RegisterForTraitChanges (This, IUITraitChangeObservable.ToClasses (traits), action);
		}
	}
#endif
}
