// Copyright 2011 - 2014 Xamarin Inc
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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif
using System.Diagnostics;

using System.Runtime.InteropServices.ObjectiveC;

#if !COREBUILD
using Xamarin.Bundler;
#if HAS_UIKIT
using UIKit;
using CoreAnimation;
#endif
using CoreGraphics;
#endif

#nullable enable

namespace Foundation {

	/// <summary>
	/// Sentinel class.
	/// </summary>
	/// <remarks>
	///   <para>
	///     The sole purpose for the <see cref="NSObjectFlag"/> class is to be used
	///     as a sentinel in the <see cref="NSObject"/> class hierarchy to ensure that the
	///     actual object initialization only happens in <see cref="NSObject"/>.
	///   </para>
	///   <para>
	///     When you chain your constructors using <see cref="NSObjectFlag.Empty"/> the
	///     only thing that will take place is the allocation of the
	///     object instance; no calls to any of the init: methods in base
	///     classes will be performed. If your code depends on this for
	///     initialization, you are responsible for calling the proper
	///     init method directly. For example:
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	///     //
	///     // The NSObjectFlag merely allocates the object and registers the
	///     // C# class with the Objective-C runtime if necessary, but no actual
	///     // initXxx method is invoked, that is done later in the constructor
	///     //
	///     // This is taken from the binding code for UIView:
	///     //
	///     [Export ("initWithFrame:")]
	///     public UIView (CGRect frame) : base (NSObjectFlag.Empty)
	///     {
	///     	// Invoke the init method now.
	///     	var initWithFrame = new Selector ("initWithFrame:").Handle;
	///     	if (IsDirectBinding) {
	///     		Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSend_CGRect (this.Handle, initWithFrame, frame);
	///     	} else {
	///     		Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_CGRect (this.SuperHandle, initWithFrame, frame);
	///     	}
	///     }
	///     ]]></code>
	///   </example>
	///   <para>
	///     Alternatively, if you need a base class to initialize itself,
	///     you should call one of the other constructors that take some
	///     parameters.
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	///     class MyViw : UIView {
	///     	[Export ("initWithFrame:")]
	///     	public MyView (CGRect frame) : base (frame)
	///     	{
	///     		// this initialized MyView by calling the UIView constructor
	///     		// that initializes the object from a CGRect frame.
	///     	}
	///     }
	///     ]]></code>
	///   </example>
	/// </remarks>
	public enum NSObjectFlag {
		/// <summary>Sentinel instance.</summary>
		Empty,
	}

	// This interface will be made public when the managed static registrar is used.
	internal interface INSObjectFactory {
		// The method will be implemented via custom linker step if the managed static registrar is used
		// for NSObject subclasses which have an (NativeHandle) or (IntPtr) constructor.
		[MethodImpl (MethodImplOptions.NoInlining)]
		virtual static NSObject? _Xamarin_ConstructNSObject (NativeHandle handle) => null;
	}

#if !COREBUILD
	// Allocated in native memory, so that it can be accessed from native code without having to deal with the GC.
	// Also put objc_super here, because it simplifies code.
	// This is mirrored in runtime.h and the definition needs to be in sync.
	struct NSObjectData {
		// the layout here is important, the two first fields have to match the objc_super struct.
		public NativeHandle handle;
		public NativeHandle classHandle;
		public NSObject.Flags flags;
	}

	class NSObjectDataHandle : CriticalHandle {
		bool invalidated;
		public NSObjectDataHandle ()
			: base (IntPtr.Zero)
		{
			unsafe {
				this.handle = (IntPtr) NativeMemory.AllocZeroed ((nuint) sizeof (NSObjectData));
			}
		}

		public NSObjectDataHandle (IntPtr handle)
			: base (handle)
		{
		}

		public void Invalidate ()
		{
			invalidated = true;
		}

		public unsafe NSObjectData* Data {
			get => (NSObjectData*) handle;
		}

		public override bool IsInvalid {
			get => handle == IntPtr.Zero;
		}

		protected override bool ReleaseHandle ()
		{
			if (handle != IntPtr.Zero) {
				if (invalidated) {
					// nothing to do here.
				} else {
					unsafe {
						NativeMemory.Free ((void*) handle);
					}
				}
			}
			handle = IntPtr.Zero;
			return true;
		}
	}
#endif

#if !COREBUILD
	/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='T:Foundation.NSObject']/*" />
	[ObjectiveCTrackedType]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public partial class NSObject : INativeObject
#if !COREBUILD
		, IEquatable<NSObject>
		, IDisposable
#endif
		, INSObjectFactory {
#if !COREBUILD
		const string selConformsToProtocol = "conformsToProtocol:";
		const string selEncodeWithCoder = "encodeWithCoder:";

#if MONOMAC
		static IntPtr selConformsToProtocolHandle = Selector.GetHandle (selConformsToProtocol);
		static IntPtr selEncodeWithCoderHandle = Selector.GetHandle (selEncodeWithCoder);
#endif

		/// <summary>
		/// Gets the assembly containing the platform-specific Foundation types.
		/// </summary>
		/// <value>The assembly containing the platform-specific Foundation types.</value>
		public static readonly Assembly PlatformAssembly = typeof (NSObject).Assembly;

		// This is exclusively for Mono
		unsafe NSObjectData* __data_for_mono; // Read directly from several places in the runtime

		unsafe NativeHandle handle {
			get => GetData ()->handle;
			set => GetData ()->handle = value;
		}

		// The NSObjectData contains some data we want to keep in native memory, so that it can be accessed
		// safely from native code without having to make sure the GC doesn't move the memory around. Among
		// other things, this means it's accessible from threads that has never seen/run managed code without
		// having to attach those threads to to the managed runtime.
		NSObjectDataHandle? data_handle;

		internal unsafe NSObjectData* GetData ()
		{
			var rv = AllocateData ().Data;

			if (rv is null) {
				// Throwing an exception here is better than returning a null pointer, because that will crash the process when the pointer is dereferenced
				// (and none of the callers can do anything useful with a null pointer anyway).
				throw new ObjectDisposedException ($"This object (of type {GetType ().Name}) does not have a data pointer anymore, possibly because of a race condition. Please file a bug at https://github.com/dotnet/macios/issues.");
			}

			return rv;
		}

		unsafe NSObjectDataHandle AllocateData ()
		{
			var dh = data_handle;
			if (dh is not null)
				return dh;

			var data = new NSObjectDataHandle ();
			var previousValue = Interlocked.CompareExchange (ref data_handle, data, null);
			if (previousValue is not null) {
				// somebody beat us to the allocation and assignment.
				data.Dispose ();
				return previousValue;
			}

			if (!Runtime.IsCoreCLR) // This condition (and the assignment to __handle_for_mono if applicable) is trimmed away by the linker.
				__data_for_mono = data.Data;

			return data;
		}

		unsafe Flags flags {
			get { return GetData ()->flags; }
			set { GetData ()->flags = value; }
		}

		// This enum has a native counterpart in runtime.h
		[Flags]
		internal enum Flags : uint {
			Disposed = 1,
			NativeRef = 2,
			IsDirectBinding = 4,
			RegisteredToggleRef = 8,
			InFinalizerQueue = 16,
			HasManagedRef = 32,
			// 64, // Used by SoM
			IsCustomType = 128,
		}

		// Must be kept in sync with the same enum in trampolines.h
		enum XamarinGCHandleFlags : uint {
			None = 0,
			WeakGCHandle = 1,
			HasManagedRef = 2,
			InitialSet = 4,
		}

		[StructLayout (LayoutKind.Sequential)]
		internal struct objc_super {
			public IntPtr Handle;
			public IntPtr ClassHandle;
		}

		bool disposed {
			get { return ((flags & Flags.Disposed) == Flags.Disposed); }
			set { flags = value ? (flags | Flags.Disposed) : (flags & ~Flags.Disposed); }
		}

		bool HasManagedRef {
			get { return (flags & Flags.HasManagedRef) == Flags.HasManagedRef; }
			set { flags = value ? (flags | Flags.HasManagedRef) : (flags & ~Flags.HasManagedRef); }
		}

		internal bool IsRegisteredToggleRef {
			get { return ((flags & Flags.RegisteredToggleRef) == Flags.RegisteredToggleRef); }
			set { flags = value ? (flags | Flags.RegisteredToggleRef) : (flags & ~Flags.RegisteredToggleRef); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance uses direct Objective-C binding.
		/// </summary>
		/// <value><see langword="true"/> if this instance uses direct binding; otherwise, <see langword="false"/>.</value>
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		[EditorBrowsable (EditorBrowsableState.Never)]
		protected internal bool IsDirectBinding {
			get { return ((flags & Flags.IsDirectBinding) == Flags.IsDirectBinding); }
			set { flags = value ? (flags | Flags.IsDirectBinding) : (flags & ~Flags.IsDirectBinding); }
		}

		internal bool InFinalizerQueue {
			get { return ((flags & Flags.InFinalizerQueue) == Flags.InFinalizerQueue); }
		}

		bool IsCustomType {
			get {
				var value = (flags & Flags.IsCustomType) == Flags.IsCustomType;
				if (!value) {
					value = Class.IsCustomType (GetType ());
					if (value)
						flags |= Flags.IsCustomType;
				}
				return value;
			}
		}

		[Export ("init")]
		public NSObject ()
		{
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}

		// This is just here as a constructor chain that can will
		// only do Init at the most derived class.
		/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSObject.#ctor(Foundation.NSObjectFlag)']/*" />
		public NSObject (NSObjectFlag x)
		{
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected internal NSObject (NativeHandle handle)
			: this (handle, false)
		{
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected NSObject (NativeHandle handle, bool alloced)
		{
			this.handle = handle;
			InitializeObject (alloced);
		}

		~NSObject ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the <see cref="NSObject" /> object.</summary>
		/// <remarks>
		///   <para>The <see cref="Dispose()" /> method releases the resources used by the <see cref="NSObject" /> class.</para>
		///   <para>
		///     Calling the <see cref="Dispose()" /> method when the application is finished using the <see cref="NSObject" /> ensures that all
		///     external resources used by this managed object are released as soon as possible. Once developers have invoked
		///     the <see cref="Dispose()" /> method, the object is no longer useful and developers should no longer make any calls to it.
		///     For more information on releasing resources see <see href="https://msdn.microsoft.com/en-us/library/498928w2.aspx">Cleaning up unmanaged resources</see>.
		///   </para>
		/// </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		// This method should never be called when using the managed static registrar, so assert that never happens by throwing an exception in that case.
		// This method doesn't necessarily work with NativeAOT, but this is covered by the exception, because the managed static registrar is required for NativeAOT.
		//
		// IL2072: 'type' argument does not satisfy 'DynamicallyAccessedMemberTypes.PublicConstructors', 'DynamicallyAccessedMemberTypes.NonPublicConstructors' in call to 'System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(Type)'. The return value of method 'ObjCRuntime.Runtime.GetGCHandleTarget(IntPtr)' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2072", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		internal static IntPtr CreateNSObject (IntPtr type_gchandle, IntPtr handle, Flags flags)
		{
			// Note that the code in this method doesn't necessarily work with NativeAOT, so assert that never happens by throwing an exception if using the managed static registrar (which is required for NativeAOT)
			if (Runtime.IsManagedStaticRegistrar) {
				throw new System.Diagnostics.UnreachableException ();
			}

			// This function is called from native code before any constructors have executed.
			var type = (Type?) Runtime.GetGCHandleTarget (type_gchandle);
			if (type is null)
				return IntPtr.Zero;

			try {
				var obj = (NSObject) RuntimeHelpers.GetUninitializedObject (type);
				obj.handle = handle;
				obj.flags = flags;
				return Runtime.AllocGCHandle (obj);
			} catch (Exception e) {
				throw ErrorHelper.CreateError (8041, e, Errors.MX8041 /* Unable to create an instance of the type {0} */, type.FullName);
			}
		}

		unsafe NativeHandle GetSuper ()
		{
			var data = GetData ();
			if (data->classHandle == NativeHandle.Zero)
				data->classHandle = ClassHandle;
			return (IntPtr) (&data->handle);
		}

		internal static NativeHandle Initialize ()
		{
			return class_ptr;
		}

		internal Flags FlagsInternal {
			get { return flags; }
			set { flags = value; }
		}

#if !__MACOS__
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		extern static void RegisterToggleRef (NSObject obj, IntPtr handle, bool isCustomType);
#endif // !__MACOS__

		[DllImport ("__Internal")]
		static extern void xamarin_release_managed_ref (IntPtr handle, byte user_type);

		static void RegisterToggleReference (NSObject obj, IntPtr handle, bool isCustomType)
		{
#if __MACOS__
			Runtime.RegisterToggleReferenceCoreCLR (obj, handle, isCustomType);
#else
			if (Runtime.IsCoreCLR) {
				Runtime.RegisterToggleReferenceCoreCLR (obj, handle, isCustomType);
			} else {
				RegisterToggleRef (obj, handle, isCustomType);
			}
#endif
		}

		/*
		Register the current object with the toggleref machinery if the following conditions are met:
		-The new refcounting is enabled; and
		-The class is not a custom type - it must wrap a framework class.
		*/
		/// <summary>Promotes a regular peer object (IsDirectBinding is true) into a toggleref object.</summary>
		/// <remarks>
		///   This turns a regular peer object (one that has <see cref="IsDirectBinding" /> set to true)
		///   into a toggleref object. This is necessary when storing managed state (for instance into
		///   a backing field), so that the managed peer isn't collected by the GC before the native object
		///   is freed. This is an internal method.
		/// </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		protected void MarkDirty ()
		{
			MarkDirty (false);
		}

		internal void MarkDirty (bool allowCustomTypes)
		{
			if (IsRegisteredToggleRef)
				return;

			if (!allowCustomTypes && IsCustomType)
				return;

			IsRegisteredToggleRef = true;
			RegisterToggleReference (this, Handle, allowCustomTypes);
		}

		private void InitializeObject (bool alloced)
		{
			if (alloced && handle == NativeHandle.Zero && Class.ThrowOnInitFailure) {
				if (ClassHandle == NativeHandle.Zero)
					throw new Exception ($"Could not create an native instance of the type '{GetType ().FullName}': the native class hasn't been loaded.\n{Constants.SetThrowOnInitFailureToFalse}.");
				throw new Exception ($"Could not create an native instance of the type '{new Class (ClassHandle).Name}'.\n{Constants.SetThrowOnInitFailureToFalse}.");
			}

			// The authorative value for the IsDirectBinding value is the register attribute:
			//
			//     [Register ("MyClass", true)] // the second parameter specifies the IsDirectBinding value
			//     class MyClass : NSObject {}
			//
			// Unfortunately looking up this attribute every time a class is instantiated is
			// slow (since fetching attributes is slow), so we guess here: if the actual type
			// of the object is in the platform assembly, then we assume IsDirectBinding=true:
			//
			// IsDirectBinding = (this.GetType ().Assembly == PlatformAssembly);
			//
			// and any subclasses in the platform assembly which is not a direct binding have
			// to set the correct value in their constructors.
			IsDirectBinding = (this.GetType ().Assembly == PlatformAssembly);
			Runtime.RegisterNSObject (this, handle);

			bool native_ref = (flags & Flags.NativeRef) == Flags.NativeRef;
			CreateManagedRef (!alloced || native_ref);
		}

		[DllImport ("__Internal")]
		static extern byte xamarin_set_gchandle_with_flags_safe (IntPtr handle, IntPtr gchandle, XamarinGCHandleFlags gchandle_flags, IntPtr data);

		void CreateManagedRef (bool retain)
		{
			HasManagedRef = true;
			if (!Runtime.TryGetIsUserType (handle, out var isUserType, out var error_message))
				throw new InvalidOperationException ($"Unable to create a managed reference for the pointer {handle} whose managed type is {GetType ().FullName} because it wasn't possible to get the class of the pointer: {error_message}");

			if (isUserType) {
				var gchandle_flags = XamarinGCHandleFlags.HasManagedRef | XamarinGCHandleFlags.InitialSet | XamarinGCHandleFlags.WeakGCHandle;
				var gchandle = GCHandle.Alloc (this, GCHandleType.WeakTrackResurrection);
				var h = GCHandle.ToIntPtr (gchandle);
				byte rv;
				unsafe {
					rv = xamarin_set_gchandle_with_flags_safe (handle, h, gchandle_flags, (IntPtr) GetData ());
				}
				if (rv == 0) {
					// A GCHandle already existed: this shouldn't happen, but let's handle it anyway.
					Runtime.NSLog ($"Tried to create a managed reference from an object that already has a managed reference (type: {GetType ()})");
					gchandle.Free ();
				}
			}

			if (retain)
				DangerousRetain ();
		}

		void ReleaseManagedRef ()
		{
			var handle = this.Handle; // Get a copy of the handle, because it will be cleared out when calling Runtime.NativeObjectHasDied, and we still need the handle later.
			if (!Runtime.TryGetIsUserType (handle, out var user_type, out var error_message))
				throw new InvalidOperationException ($"Unable to release the managed reference for the pointer {handle} whose managed type is {GetType ().FullName} because it wasn't possible to get the class of the pointer: {error_message}");
			HasManagedRef = false;
			if (!user_type) {
				/* If we're a wrapper type, we need to unregister here, since we won't enter the release trampoline */
				Runtime.NativeObjectHasDied (handle, this);
			}
			xamarin_release_managed_ref (handle, user_type.AsByte ());
		}

		static bool IsProtocol (Type type, IntPtr protocol)
		{
			while (type != typeof (NSObject) && type is not null) {
				var attrs = type.GetCustomAttributes (typeof (ProtocolAttribute), false);
				var protocolAttribute = (ProtocolAttribute?) (attrs.Length > 0 ? attrs [0] : null);
				if (protocolAttribute is not null && !protocolAttribute.IsInformal) {
					string name;

					if (!string.IsNullOrEmpty (protocolAttribute.Name)) {
						name = protocolAttribute.Name;
					} else {
						attrs = type.GetCustomAttributes (typeof (RegisterAttribute), false);
						var registerAttribute = (RegisterAttribute?) (attrs.Length > 0 ? attrs [0] : null);
						if (registerAttribute is not null && !string.IsNullOrEmpty (registerAttribute.Name)) {
							name = registerAttribute.Name;
						} else {
							name = type.Name;
						}
					}

					var proto = Runtime.GetProtocol (name);
					if (proto != IntPtr.Zero && proto == protocol)
						return true;
				}
				var baseType = type.BaseType;
				if (baseType is null)
					return false;
				type = baseType;
			}

			return false;
		}

		[Preserve]
		bool InvokeConformsToProtocol (NativeHandle protocol)
		{
			return ConformsToProtocol (protocol);
		}

		[Export ("conformsToProtocol:")]
		[Preserve ()]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public virtual bool ConformsToProtocol (NativeHandle protocol)
		{
			bool does;
			bool is_wrapper = IsDirectBinding;
			bool is_third_party;

			if (is_wrapper) {
				is_third_party = this.GetType ().Assembly != NSObject.PlatformAssembly;
				if (is_third_party) {
					// Third-party bindings might lie about IsDirectBinding (see bug #14772),
					// so don't trust any 'true' values unless we're in monotouch.dll.
					var attribs = this.GetType ().GetCustomAttributes (typeof (RegisterAttribute), false);
					if (attribs is not null && attribs.Length == 1)
						is_wrapper = ((RegisterAttribute) attribs [0]).IsWrapper;
				}
			}

#if MONOMAC
			if (is_wrapper) {
				does = Messaging.bool_objc_msgSend_IntPtr (this.Handle, selConformsToProtocolHandle, protocol) != 0;
			} else {
				does = Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, selConformsToProtocolHandle, protocol) != 0;
			}
#else
			if (is_wrapper) {
				does = Messaging.bool_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle (selConformsToProtocol), protocol) != 0;
			} else {
				does = Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle (selConformsToProtocol), protocol) != 0;
			}
#endif

			if (does)
				return true;

			if (!Runtime.DynamicRegistrationSupported)
				return false;

			// the linker/trimmer will remove the following code if the dynamic registrar is removed from the app
			var classHandle = ClassHandle;
			lock (Runtime.protocol_cache) {
				ref var map = ref CollectionsMarshal.GetValueRefOrAddDefault (Runtime.protocol_cache, classHandle, out var exists);
				if (!exists || map is null)
					map = new ();
				ref var result = ref CollectionsMarshal.GetValueRefOrAddDefault (map, protocol, out exists);
				if (!exists)
					result = DynamicConformsToProtocol (protocol);
				return result;
			}
		}

		// Note that this method does not work with NativeAOT, so throw an exception in that case.
		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.Interfaces' in call to 'System.Type.GetInterfaces()'. The return value of method 'System.Object.GetType()' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2075", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		bool DynamicConformsToProtocol (NativeHandle protocol)
		{
			// Note that this method does not work with NativeAOT, so throw an exception in that case.
			if (Runtime.IsNativeAOT)
				throw Runtime.CreateNativeAOTNotSupportedException ();

			object [] adoptedProtocols = GetType ().GetCustomAttributes (typeof (AdoptsAttribute), true);
			foreach (AdoptsAttribute adopts in adoptedProtocols) {
				if (adopts.ProtocolHandle == protocol)
					return true;
			}

			// Check if this class or any of the interfaces
			// it implements are protocols.

			if (IsProtocol (GetType (), protocol))
				return true;

			var ifaces = GetType ().GetInterfaces ();
			foreach (var iface in ifaces) {
				if (IsProtocol (iface, protocol))
					return true;
			}

			return false;
		}

		/// <summary>Calls the 'release' selector on this object.</summary>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public void DangerousRelease ()
		{
			DangerousRelease (handle);
		}

		/// <summary>Calls the 'release' selector on an Objective-C object.</summary>
		/// <param name="handle">The Objective-C object to release.</param>
		/// <remarks>It's safe to call this function with <see cref="NativeHandle.Zero" />.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		internal static void DangerousRelease (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return;
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.ReleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Release));
#endif
		}

		/// <summary>Calls the 'retain' selector on an Objective-C object.</summary>
		/// <param name="handle">The Objective-C object to retain.</param>
		/// <remarks>It's safe to call this function with <see cref="NativeHandle.Zero" />.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		internal static void DangerousRetain (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return;
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.RetainHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Retain));
#endif
		}

		/// <summary>Calls the 'autorelease' selector on an Objective-C object.</summary>
		/// <param name="handle">The Objective-C object to autorelease.</param>
		/// <remarks>It's safe to call this function with <see cref="NativeHandle.Zero" />.</remarks>
		internal static void DangerousAutorelease (NativeHandle handle)
		{
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.AutoreleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Autorelease));
#endif
		}

		/// <summary>Calls the 'retain' selector on this object.</summary>
		/// <returns>This object.</returns>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSObject DangerousRetain ()
		{
			DangerousRetain (handle);
			return this;
		}

		/// <summary>Calls the 'autorelease' selector on this object.</summary>
		/// <returns>This object.</returns>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSObject DangerousAutorelease ()
		{
			DangerousAutorelease (handle);
			return this;
		}

		/// <summary>Handle used to represent the methods in the base class for this <see cref="NSObject" />.</summary>
		/// <value>An opaque pointer, represents an Objective-C objc_super object pointing to our base class.</value>
		/// <remarks>
		///   This property is used to access members of a base class.
		///   This is typically used when you call any of the Messaging
		///   methods to invoke methods that were implemented in your base
		///   class, instead of invoking the implementation in the current
		///   class.
		/// </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public NativeHandle SuperHandle {
			get {
				if (handle == IntPtr.Zero)
					ObjCRuntime.ThrowHelper.ThrowObjectDisposedException (this);

				return GetSuper ();
			}
		}

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		/// <value>A pointer.</value>
		/// <remarks>This is a handle to the underlying unmanaged representation for this object.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public NativeHandle Handle {
			get { return handle; }
			set {
				if (handle == value)
					return;

				if (handle != IntPtr.Zero)
					Runtime.UnregisterNSObject (handle);

				handle = value;

				if (handle != IntPtr.Zero)
					Runtime.RegisterNSObject (this, handle);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected void InitializeHandle (NativeHandle handle)
		{
			InitializeHandle (handle, "init*", Class.ThrowOnInitFailure);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected void InitializeHandle (NativeHandle handle, string initSelector)
		{
			InitializeHandle (handle, initSelector, Class.ThrowOnInitFailure);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		internal void InitializeHandle (NativeHandle handle, string initSelector, bool throwOnInitFailure)
		{
			if (this.handle == NativeHandle.Zero && throwOnInitFailure) {
				if (ClassHandle == NativeHandle.Zero)
					throw new Exception ($"Could not create an native instance of the type '{GetType ().FullName}': the native class hasn't been loaded.\n{Constants.SetThrowOnInitFailureToFalse}.");
				throw new Exception ($"Could not create an native instance of the type '{new Class (ClassHandle).Name}'.\n{Constants.SetThrowOnInitFailureToFalse}.");
			}

			if (handle == NativeHandle.Zero && throwOnInitFailure) {
				Handle = NativeHandle.Zero; // We'll crash if we don't do this.
				throw new Exception ($"Could not initialize an instance of the type '{GetType ().FullName}': the native '{initSelector}' method returned nil.\n{Constants.SetThrowOnInitFailureToFalse}.");
			}

			this.Handle = handle;
		}

		private bool AllocIfNeeded ()
		{
			if (handle == NativeHandle.Zero) {
#if MONOMAC
				handle = Messaging.IntPtr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.AllocHandle);
#else
				handle = Messaging.IntPtr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.GetHandle (Selector.Alloc));
#endif
				return true;
			}
			return false;
		}

		private void InvokeOnMainThread (Selector sel, NSObject? obj, bool wait)
		{
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (this.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), sel.Handle, obj.GetHandle (), wait ? (byte) 1 : (byte) 0);
			GC.KeepAlive (sel);
			GC.KeepAlive (obj);
		}

		/// <summary>Invokes asynchronously the specified code on the main UI thread.</summary>
		/// <param name="sel">Selector to invoke</param>
		/// <param name="obj">Object in which the selector is invoked</param>
		/// <remarks>
		///   <para>
		///     You use this method from a thread to invoke the code in
		///     the specified object that is exposed with the specified
		///     selector in the UI thread. This is required for most
		///     operations that affect UIKit or AppKit as neither one of
		///     those APIs is thread safe.
		///   </para>
		///   <para>
		///     The code is executed when the main thread goes back to its
		///     main loop for processing events.
		///   </para>
		///   <para>
		///     Unlike <see cref="InvokeOnMainThread(ObjCRuntime.Selector,Foundation.NSObject)" />
		///     this method merely queues the invocation and returns
		///     immediately to the caller.
		///   </para>
		/// </remarks>
		public void BeginInvokeOnMainThread (Selector sel, NSObject? obj)
		{
			InvokeOnMainThread (sel, obj, false);
		}

		/// <summary>Invokes synchronously the specified code on the main UI thread.</summary>
		/// <param name="sel">Selector to invoke</param>
		/// <param name="obj">Object in which the selector is invoked</param>
		/// <remarks>
		///   <para>
		///     You use this method from a thread to invoke the code in
		///     the specified object that is exposed with the specified
		///     selector in the UI thread. This is required for most
		///     operations that affect UIKit or AppKit as neither one of
		///     those APIs is thread safe.
		///   </para>
		///   <para>
		///     The code is executed when the main thread goes back to its
		///     main loop for processing events.
		///   </para>
		///   <para>
		///     Unlike <see cref="BeginInvokeOnMainThread(ObjCRuntime.Selector,Foundation.NSObject)" />
		///     this method waits for the main thread to execute the method, and does not return until the code pointed by action has completed.
		///   </para>
		/// </remarks>
		public void InvokeOnMainThread (Selector sel, NSObject? obj)
		{
			InvokeOnMainThread (sel, obj, true);
		}

		/// <summary>
		/// Invokes the specified action asynchronously on the main UI thread.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		/// <remarks>
		/// This method queues the action to be executed when the main thread goes back to its
		/// main loop for processing events. The method returns immediately to the caller.
		/// </remarks>
		public void BeginInvokeOnMainThread (Action action)
		{
			var d = new NSAsyncActionDispatcher (action);
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
																NSDispatcher.Selector.Handle, d.Handle, 0);
			GC.KeepAlive (d);
		}

		internal void BeginInvokeOnMainThread (System.Threading.SendOrPostCallback cb, object? state)
		{
			var d = new NSAsyncSynchronizationContextDispatcher (cb, state);
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
															Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, 0);
			GC.KeepAlive (d);
		}

		/// <summary>
		/// Invokes the specified action synchronously on the main UI thread.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		/// <remarks>
		/// This method waits for the main thread to execute the action, and does not return until the action has completed.
		/// </remarks>
		public void InvokeOnMainThread (Action action)
		{
			using (var d = new NSActionDispatcher (action)) {
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
																Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, 1);
			}
		}

		internal void InvokeOnMainThread (System.Threading.SendOrPostCallback cb, object? state)
		{
			using (var d = new NSSynchronizationContextDispatcher (cb, state)) {
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
																Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, 1);
			}
		}

		/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSObject.FromObject(System.Object)']/*" />
		public static NSObject? FromObject (object? obj)
		{
			if (obj is null)
				return NSNull.Null;
			var t = obj.GetType ();
			if (obj is NSObject nsobj)
				return nsobj;

			switch (Type.GetTypeCode (t)) {
			case TypeCode.Boolean:
				return new NSNumber ((bool) obj);
			case TypeCode.Char:
				return new NSNumber ((ushort) (char) obj);
			case TypeCode.SByte:
				return new NSNumber ((sbyte) obj);
			case TypeCode.Byte:
				return new NSNumber ((byte) obj);
			case TypeCode.Int16:
				return new NSNumber ((short) obj);
			case TypeCode.UInt16:
				return new NSNumber ((ushort) obj);
			case TypeCode.Int32:
				return new NSNumber ((int) obj);
			case TypeCode.UInt32:
				return new NSNumber ((uint) obj);
			case TypeCode.Int64:
				return new NSNumber ((long) obj);
			case TypeCode.UInt64:
				return new NSNumber ((ulong) obj);
			case TypeCode.Single:
				return new NSNumber ((float) obj);
			case TypeCode.Double:
				return new NSNumber ((double) obj);
			case TypeCode.String:
				return new NSString ((string) obj);
			default:
				if (t == typeof (NativeHandle))
					return NSValue.ValueFromPointer ((NativeHandle) obj);
#if !NO_SYSTEM_DRAWING
				if (t == typeof (SizeF))
					return NSValue.FromSizeF ((SizeF) obj);
				else if (t == typeof (RectangleF))
					return NSValue.FromRectangleF ((RectangleF) obj);
				else if (t == typeof (PointF))
					return NSValue.FromPointF ((PointF) obj);
#endif
				if (t == typeof (nint))
					return NSNumber.FromNInt ((nint) obj);
				else if (t == typeof (nuint))
					return NSNumber.FromNUInt ((nuint) obj);
				else if (t == typeof (nfloat))
					return NSNumber.FromNFloat ((nfloat) obj);
				else if (t == typeof (CGSize))
					return NSValue.FromCGSize ((CGSize) obj);
				else if (t == typeof (CGRect))
					return NSValue.FromCGRect ((CGRect) obj);
				else if (t == typeof (CGPoint))
					return NSValue.FromCGPoint ((CGPoint) obj);

#if !MONOMAC
				if (t == typeof (CGAffineTransform))
					return NSValue.FromCGAffineTransform ((CGAffineTransform) obj);
				else if (t == typeof (UIEdgeInsets))
					return NSValue.FromUIEdgeInsets ((UIEdgeInsets) obj);
				else if (t == typeof (CATransform3D))
					return NSValue.FromCATransform3D ((CATransform3D) obj);
#endif
				// last chance for types like CGPath, CGColor... that are not NSObject but are CFObject
				// see https://bugzilla.xamarin.com/show_bug.cgi?id=8458
				var native = (obj as INativeObject);
				if (native is not null) {
					var result = Runtime.GetNSObject (native.Handle);
					GC.KeepAlive (native);
					return result;
				}
				return null;
			}
		}

		/// <summary>
		/// Sets the value for the property identified by a given key path to a given value.
		/// </summary>
		/// <param name="handle">A handle to the value to set.</param>
		/// <param name="keyPath">A key path of the form relationship.property (with one or more relationships); for example "department.name" or "department.manager.lastName".</param>
		/// <remarks>
		/// This method is useful for setting a value for a property that can be reached by following a key path.
		/// The key path is a series of property names separated by periods.
		/// </remarks>
		public void SetValueForKeyPath (NativeHandle handle, NSString keyPath)
		{
			if (keyPath is null)
				throw new ArgumentNullException (nameof (keyPath));
			if (IsDirectBinding) {
				ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle_NativeHandle (this.Handle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
				GC.KeepAlive (keyPath);
			} else {
				ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle_NativeHandle (this.SuperHandle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
				GC.KeepAlive (keyPath);
			}
		}

		// if IsDirectBinding is false then we _likely_ have managed state and it's up to the subclass to provide
		// a correct implementation of GetHashCode / Equals. We default to Object.GetHashCode (like classic)

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A int containing the hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			if (!IsDirectBinding)
				return base.GetHashCode ();
			// Hash is nuint so 64 bits, and Int32.GetHashCode == same Int32
			return GetNativeHash ().GetHashCode ();
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current <see cref="NSObject"/>.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// For direct bindings, this method uses the Objective-C <c>isEqual:</c> method.
		/// For non-direct bindings, this method uses reference equality.
		/// </remarks>
		public override bool Equals (object? obj)
		{
			var o = obj as NSObject;
			if (o is null)
				return false;

			bool isDirectBinding = IsDirectBinding;
			// is only one is a direct binding then both cannot be equals
			if (isDirectBinding != o.IsDirectBinding)
				return false;

			// we can only ask `isEqual:` to test equality if both objects are direct bindings
			return isDirectBinding ? IsEqual (o) : ReferenceEquals (this, o);
		}

		// IEquatable<T>
		/// <summary>
		/// Determines whether the specified <see cref="NSObject"/> is equal to the current <see cref="NSObject"/>.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// For direct bindings, this method uses the Objective-C <c>isEqual:</c> method.
		/// For non-direct bindings, this method uses reference equality.
		/// </remarks>
		public bool Equals (NSObject? obj) => Equals ((object?) obj);

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		public override string? ToString ()
		{
			if (disposed)
				return base.ToString ();
			return Description ?? base.ToString ();
		}

		/// <summary>
		/// Invokes the specified action after the specified delay.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		/// <param name="delay">The delay in seconds.</param>
		public virtual void Invoke (Action action, double delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			d.PerformSelector (NSDispatcher.Selector, null, delay);
		}

		/// <summary>
		/// Invokes the specified action after the specified delay.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		/// <param name="delay">The delay as a <see cref="TimeSpan"/>.</param>
		public virtual void Invoke (Action action, TimeSpan delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			d.PerformSelector (NSDispatcher.Selector, null, delay.TotalSeconds);
		}

		internal void ClearHandle ()
		{
			handle = NativeHandle.Zero;
		}

		/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSObject.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (disposed)
				return;
			disposed = true;

			if (handle != NativeHandle.Zero) {
				if (disposing) {
					ReleaseManagedRef ();
				} else {
					NSObject_Disposer.Add (this);
					RecreateDataHandle ();
				}
			}
		}

		void RecreateDataHandle ()
		{
			// OK, this code is _weird_.
			// We need to delay the deletion of the native memory pointed to by data_handle until
			// after this instance has been collected. A CriticalHandle seems to fit this purpose like a glove, until
			// you realize that a CriticalHandle is only kept alive until the parent object _becomes finalizable_,
			// not _is collected_, which is very different - in other words, resurrected objects don't keep CriticalHandles
			// they contain alive. This is a problem because every single managed NSObject instance is resurrected, and we
			// need the native memory to stay alive after resurrection.
			//
			// So this solution depends on a few bits:
			// * At this point, this instance may have become finalizable, but the native memory shouldn't have been freed yet.
			// * The original NSObjectDataHandle (aka CriticalHandle) will be collected in this/upcoming GC cycle, and can't
			//   be trusted to keep the native memory alive anymore.
			// * So we just create a new one, pointing to the same native memory, and replace the original NSObjectDataHandle (aka
			//   CriticalHandle) with it
			// * This works, because since this instance has become / will become resurrected, it's not finalizable anymore,
			//   and it will keep the new NSObjectDataHandle instance (and the native memory it points to) alive.
			// * Now if this instance is deemed finalizable, and then resurrected *again*, bad things will likely happen. This
			//   is a bit more unlikely though, because we don't re-register the finalizer for execution, so unless somebody
			//   else does that, it's quite unlikely this instance will become resurrected a second time.
			var previous_data = data_handle;
			if (previous_data is null) {
				var msg = $"This object (of type {GetType ().Name}) does not have an existing data pointer, possibly because of a race condition. Please file a bug at https://github.com/dotnet/macios/issues.";
#if CONSISTENCY_CHECKS
				throw new InvalidOperationException (msg);
#else
				Runtime.NSLog (msg);
				return;
#endif
			}

			unsafe {
				data_handle = new NSObjectDataHandle ((IntPtr) previous_data.Data);
			}

			if (previous_data.IsInvalid) {
				var msg = $"This object (of type {GetType ().Name}) does not have valid data pointer, possibly because of a race condition. Please file a bug at https://github.com/dotnet/macios/issues.";
#if CONSISTENCY_CHECKS
				throw new InvalidOperationException (msg);
#else
				Runtime.NSLog (msg);
				return;
#endif
			}

			previous_data.Invalidate ();
			// Don't dispose previous_data, because another thread might be referencing it, and trying to access its pointer - which is still valid.
			// The GC will dispose of previous_data when its not accessible anymore.
		}

		[Register ("__NSObject_Disposer")]
		[Preserve (AllMembers = true)]
		internal class NSObject_Disposer : NSObject {
			static readonly List<NSObject> drainList1 = new List<NSObject> ();
			static readonly List<NSObject> drainList2 = new List<NSObject> ();
			static List<NSObject> handles = drainList1;

			static readonly IntPtr class_ptr = Class.GetHandle ("__NSObject_Disposer");
#if MONOMAC
			static readonly IntPtr drainHandle = Selector.GetHandle ("drain:");
#endif

			static readonly object lock_obj = new object ();

			private NSObject_Disposer ()
			{
				// Disable default ctor, there should be no instances of this class.
			}

			static internal void Add (NSObject handle)
			{
				bool call_drain;
				lock (lock_obj) {
					handles.Add (handle);
					call_drain = handles.Count == 1;
				}
				if (!call_drain)
					return;
				ScheduleDrain ();
			}

			static void ScheduleDrain ()
			{
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (class_ptr, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), Selector.GetHandle ("drain:"), NativeHandle.Zero, 0);
			}

			static bool draining;

			[Export ("drain:")]
			static void Drain (NSObject ctx)
			{
				List<NSObject> drainList;

				lock (lock_obj) {
					// This function isn't re-entrant safe, so protect against it. The only possibility I can
					// see where this function would be re-entrant, is if in the call to ReleaseManagedRef below,
					// the native dealloc method for a type ended up executing the run loop, and that runloop
					// processed a drain request, ending up in this method (again).
					if (draining) {
						ScheduleDrain ();
						return;
					}
					draining = true;

					drainList = handles;
					if (handles == drainList1)
						handles = drainList2;
					else
						handles = drainList1;
				}

				foreach (NSObject x in drainList)
					x.ReleaseManagedRef ();
				drainList.Clear ();

				lock (lock_obj) {
					draining = false;
				}
			}
		}

		[Register ("__XamarinObjectObserver")]
		class Observer : NSObject {
			WeakReference? obj;
			Action<NSObservedChange>? cback;
			NSString key;

			public Observer (NSObject obj, NSString key, Action<NSObservedChange> observer)
			{
				if (observer is null)
					throw new ArgumentNullException (nameof (observer));

				this.obj = new WeakReference (obj);
				this.key = key;
				this.cback = observer;
				IsDirectBinding = false;
			}

			[Preserve (Conditional = true)]
			public override void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
			{
				if (keyPath == key && context == Handle)
					cback!.Invoke (new NSObservedChange (change));
				else
					base.ObserveValue (keyPath, ofObject, change, context);
			}

			protected override void Dispose (bool disposing)
			{
				if (disposing) {
					if (obj is not null) {
						var target = (NSObject?) obj.Target;
						if (target is not null)
							target.RemoveObserver (this, key, Handle);
					}
					obj = null;
					cback = null;
				} else {
					Runtime.NSLog ("Warning: observer object was not disposed manually with Dispose()");
				}
				base.Dispose (disposing);
			}
		}

		/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSObject.AddObserver(System.String,Foundation.NSKeyValueObservingOptions,System.Action{Foundation.NSObservedChange})']/*" />
		public IDisposable AddObserver (string key, NSKeyValueObservingOptions options, Action<NSObservedChange> observer)
		{
			return AddObserver (new NSString (key), options, observer);
		}

		/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='M:Foundation.NSObject.AddObserver(Foundation.NSString,Foundation.NSKeyValueObservingOptions,System.Action{Foundation.NSObservedChange})']/*" />
		public IDisposable AddObserver (NSString key, NSKeyValueObservingOptions options, Action<NSObservedChange> observer)
		{
			var o = new Observer (this, key, observer);
			AddObserver (o, key, options, o.Handle);
			return o;
		}

		/// <summary>
		/// Allocates an uninitialized instance of the specified class.
		/// </summary>
		/// <param name="kls">The class to allocate.</param>
		/// <returns>A new uninitialized <see cref="NSObject"/> instance.</returns>
		/// <remarks>
		/// This method should typically be followed by a call to an init method to properly initialize the object.
		/// </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static NSObject Alloc (Class kls)
		{
			var h = Messaging.IntPtr_objc_msgSend (kls.Handle, Selector.GetHandle (Selector.Alloc));
			GC.KeepAlive (kls);
			return new NSObject (h, true);
		}

		/// <summary>
		/// Initializes the object by calling the Objective-C <c>init</c> method.
		/// </summary>
		/// <remarks>
		/// This method should only be called on objects that have been allocated but not yet initialized.
		/// </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public void Init ()
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("you have not allocated the native object");

			handle = Messaging.IntPtr_objc_msgSend (handle, Selector.GetHandle ("init"));
		}

		/// <summary>
		/// Invokes the specified action on a background thread.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		public static void InvokeInBackground (Action action)
		{
			// using the parameterized Thread.Start to avoid capturing
			// the 'action' parameter (it'll needlessly create an extra
			// object).
			new System.Threading.Thread ((v) => {
				((Action) v!) ();
			}) {
				IsBackground = true,
			}.Start (action);
		}
#endif // !COREBUILD
	}

#if !COREBUILD
	/// <summary>
	/// Changes that occurred to an object being observed by Key-Value-Observing.
	/// </summary>
	/// <remarks>
	/// <para>This class exposes the various components that were changed in a Key-Value-Observed property.</para>
	/// <para>These are merely accessors to the underlying <see cref="NSDictionary"/> that is provided to the <see cref="NSObject.ObserveValue(NSString, NSObject, NSDictionary, IntPtr)"/> method.</para>
	/// <para>Instances of this class are provided to your callback methods that you provide to <see cref="NSObject.AddObserver(NSObject, string, NSKeyValueObservingOptions, IntPtr)"/>.</para>
	/// <para>You can also create these objects if you have a dictionary that contains the keys from a key-value-observing change. For example if you override the <see cref="NSObject.ObserveValue(NSString, NSObject, NSDictionary, IntPtr)"/> method.</para>
	/// <example>
	/// <code lang="csharp lang-csharp"><![CDATA[
	/// class MyObserved : NSObject {
	///     public override ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
	///     {
	///         var change = new NSObservedChange (change);
	///         // Now you can access the details about the change with the
	///         // properties in the `change' variable.
	///
	///         if (context == MyObservedContext){
	///             //...
	///         }
	///         base.ObserveValue (keyPath, ofObject, change, context);
	///     }
	/// }
	/// ]]></code>
	/// </example>
	/// </remarks>
	public class NSObservedChange {
		NSDictionary dict;
		/// <summary>
		/// Initializes a new instance of the <see cref="NSObservedChange"/> class.
		/// </summary>
		/// <param name="source">The dictionary containing the change information.</param>
		public NSObservedChange (NSDictionary source)
		{
			dict = source;
		}

		/// <summary>Records the kind of change that was done to the property.</summary>
		/// <value>The current state of the changes being reported.</value>
		/// <remarks>You can use the value of this property to determine which information is available on the other properties of this class.</remarks>
		public NSKeyValueChange Change {
			get {
				var n = (NSNumber?) dict [NSObject.ChangeKindKey];
				return (NSKeyValueChange) (n?.Int32Value ?? 0);
			}
		}

		/// <summary>The new value being set on the observed property.</summary>
		/// <remarks>For this property to have a value, the options passed to <see cref="Foundation.NSObject.AddObserver(Foundation.NSObject,System.String,Foundation.NSKeyValueObservingOptions,System.IntPtr)" /> method should contain the value <see cref="Foundation.NSKeyValueObservingOptions" />New.</remarks>
		public NSObject? NewValue {
			get {
				return dict [NSObject.ChangeNewKey];
			}
		}

		/// <summary>The previous value on the observed property.</summary>
		/// <value>The old value.</value>
		/// <remarks>For this property to have a value, the options passed to <see cref="Foundation.NSObject.AddObserver(Foundation.NSObject,System.String,Foundation.NSKeyValueObservingOptions,System.IntPtr)" /> method should contain the value <see cref="Foundation.NSKeyValueObservingOptions.Old" />.</remarks>
		public NSObject? OldValue {
			get {
				return dict [NSObject.ChangeOldKey];
			}
		}

		/// <summary>The indexes of the objects that were added, removed or changed.</summary>
		/// <value>An <see cref="NSIndexSet"/> containing the indexes, or <see langword="null"/> if not applicable.</value>
		/// <remarks>This value is set if the Change property is either <see cref="NSKeyValueChange.Insertion" />, <see cref="NSKeyValueChange.Removal" /> or <see cref="NSKeyValueChange.Replacement" />.</remarks>
		public NSIndexSet? Indexes {
			get {
				return (NSIndexSet?) dict [NSObject.ChangeIndexesKey];
			}
		}

		/// <summary>If this flag is true, this indicates that the observed method is being called before the change takes place.</summary>
		/// <remarks>For this property to have a value, the options passed to <see cref="Foundation.NSObject.AddObserver(Foundation.NSObject,System.String,Foundation.NSKeyValueObservingOptions,System.IntPtr)" /> method should contain the value <see cref="Foundation.NSKeyValueObservingOptions.Prior" />.</remarks>
		public bool IsPrior {
			get {
				var n = dict [NSObject.ChangeNotificationIsPriorKey] as NSNumber;
				if (n is null)
					return false;
				return n.BoolValue;
			}
		}
	}
#endif
}
