//
// Block support
//
// Copyright 2010, Novell, Inc.
// Copyright 2011 - 2013 Xamarin Inc
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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#if !COREBUILD
using Xamarin.Bundler;
#endif

#nullable enable

// http://clang.llvm.org/docs/Block-ABI-Apple.html

namespace ObjCRuntime {

#pragma warning disable 649 //  Field 'XamarinBlockDescriptor.ref_count' is never assigned to, and will always have its default value 0
	[StructLayout (LayoutKind.Sequential)]
	struct BlockDescriptor {
		public IntPtr reserved;
		public IntPtr size;
		public IntPtr copy_helper;
		public IntPtr dispose;
		public IntPtr signature;
	}
#pragma warning restore 649

	struct XamarinBlockDescriptor {
#pragma warning disable 649 // Field 'XamarinBlockDescriptor.descriptor' is never assigned to, and will always have its default value
		public BlockDescriptor descriptor;
		public volatile int ref_count;
#pragma warning restore 649
		// followed by variable-length string (the signature)
	}

	/// <summary>Wraps an ECMA CLI delegate (C# lambdas, anonymous methods, or delegates) as an Objective-C block.</summary>
	/// <remarks>
	///   <para>
	///   This is a low-level class that is automatically used when using Objective-C block APIs.
	///   </para>
	///   <para>
	///   In the C#/ECMA CLI world, delegates are automatically turned into blocks that can be consumed by Objective-C block-aware APIs.
	///   </para>
	///   <para>
	///     If you need to P/Invoke a native C method that takes a block parameter, you would need to manually setup the <see cref="BlockLiteral" /> object and declare a proxy method that is invoked by the block handler and will invoke your managed code.
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// // Imagine that you want to invoke the following method:
	/// // void SetupHandler (void (^block)(double offset, int count));
	///
	/// // Declare a trampoline method, which is the method that will be called when
	/// // the block is invoked. The trampoline method must have an [UnmanagedCallersOnly]
	/// // attribute, so that it can be called directly from native code.
	/// [UnmanagedCallersOnly]
	/// static void TrampolineHandler (IntPtr block, double offset, int count)
	/// {
	///     // Find the delegate for the block and call it
	///     var callback = BlockLiteral.GetTarget<SetupHandlerCallback> (block);
	///     if (callback is not null)
	///         callback (offset, count);
	/// }
	///
	/// [DllImport ("YourLibrary")]
	/// unsafe static extern void SetupHandler (BlockLiteral* block);
	///
	/// public void SetupHandler (SetupHandlerCallback callback)
	/// {
	///     if (callback is null)
	///         throw new ArgumentNullException (nameof (callback));
	///     delegate* unmanaged<IntPtr, double, int, void> trampoline = &TrampolineHandler;
	///     using var block = new BlockLiteral (trampoline, callback, GetType (), nameof (TrampolineHandler));
	///     SetupHandler (&block);
	/// }
	/// ]]></code>
	///   </example>
	/// </remarks>
	[StructLayout (LayoutKind.Sequential)]
#if XAMCORE_5_0
	// Let's try to make this a ref struct in XAMCORE_5_0, that will mean blocks can't be boxed (which is good, because it would most likely result in broken code).
	// Note that the presence of a Dispose method is enough to be able to do a 'using var block = new BlockLiteral ()' in C# due to pattern-based using for 'ref structs':
	// Ref: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/using#pattern-based-using
	public unsafe ref struct BlockLiteral
#elif COREBUILD
	public unsafe struct BlockLiteral {
#else
	public unsafe struct BlockLiteral : IDisposable {
#endif
#pragma warning disable 169
		IntPtr isa;
		BlockFlags flags;
		int reserved;
		IntPtr invoke;
		IntPtr block_descriptor;
		IntPtr local_handle;
		IntPtr global_handle;
#pragma warning restore 169
#if !COREBUILD
		static IntPtr block_class;

		static IntPtr NSConcreteStackBlock {
			get {
				if (block_class == IntPtr.Zero)
					block_class = Dlfcn.dlsym (Libraries.System.Handle, "_NSConcreteStackBlock");
				return block_class;
			}
		}

		[DllImport ("__Internal")]
		static extern IntPtr xamarin_get_block_descriptor ();

		/// <summary>
		/// Creates a block literal.
		/// </summary>
		/// <param name="trampoline">A function pointer that will be called when the block is called. This method must have an <see cref="UnmanagedCallersOnlyAttribute"/> attribute.</param>
		/// <param name="context">A context object that can be retrieved from the trampoline. This is typically a delegate to the managed function to call.</param>
		/// <param name="trampolineType">The type where the trampoline is located.</param>
		/// <param name="trampolineMethod">The name of the trampoline method.</param>
		/// <remarks>
		/// <paramref name="trampolineType"/> and <paramref name="trampolineMethod"/> must uniquely define the trampoline method (it is looked up using reflection).
		/// If there are multiple methods with the same name, call <see cref="BlockLiteral(void*,object,MethodInfo)" /> instead.
		/// </remarks>
		public BlockLiteral (void* trampoline, object context, Type trampolineType, string trampolineMethod)
			: this (trampoline, context, FindTrampoline (trampolineType, trampolineMethod))
		{
		}

		/// <summary>
		/// Creates a block literal.
		/// </summary>
		/// <param name="trampoline">A function pointer that will be called when the block is called. This method must have an <see cref="UnmanagedCallersOnlyAttribute"/> attribute.</param>
		/// <param name="context">A context object that can be retrieved from the trampoline. This is typically a delegate to the managed function to call.</param>
		/// <param name="trampolineMethod">The <see cref="MethodInfo"/> instance corresponding to the trampoline method.</param>
		public BlockLiteral (void* trampoline, object context, MethodInfo trampolineMethod)
			: this (trampoline, context, GetBlockSignature (trampoline, trampolineMethod))
		{
		}

		/// <summary>
		/// Creates a block literal.
		/// </summary>
		/// <param name="trampoline">A function pointer that will be called when the block is called. This method must have an <see cref="UnmanagedCallersOnlyAttribute"/> attribute.</param>
		/// <param name="context">A context object that can be retrieved from the trampoline. This is typically a delegate to the managed function to call.</param>
		/// <param name="trampolineSignature">The Objective-C signature of the trampoline method.</param>
		public BlockLiteral (void* trampoline, object context, string trampolineSignature)
		{
			isa = IntPtr.Zero;
			flags = (BlockFlags) 0;
			reserved = 0;
			invoke = IntPtr.Zero;
			block_descriptor = IntPtr.Zero;
			local_handle = IntPtr.Zero;
			global_handle = IntPtr.Zero;
			SetupFunctionPointerBlock ((IntPtr) trampoline, context, System.Text.Encoding.UTF8.GetBytes (trampolineSignature));
		}

		// Note that the code in this method shouldn't be called when using NativeAOT, so throw an exception in that case.
		// IL2070: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.PublicMethods', 'DynamicallyAccessedMemberTypes.NonPublicMethods' in call to 'System.Type.GetMethod(String, BindingFlags)'. The parameter 'trampolineType' of method 'ObjCRuntime.BlockLiteral.FindTrampoline(Type, String)' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2070", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		static MethodInfo FindTrampoline (Type trampolineType, string trampolineMethod)
		{
			// Note that the code in this method shouldn't be called when using NativeAOT, so throw an exception in that case.
			if (Runtime.IsNativeAOT)
				throw Runtime.CreateNativeAOTNotSupportedException ();

			var rv = trampolineType.GetMethod (trampolineMethod, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

			if (rv is null)
				throw ErrorHelper.CreateError (8046, Errors.MX8046 /* Unable to find the method '{0}' in the type '{1}' */, trampolineMethod, trampolineType.FullName);

			return rv;
		}

		static string GetTypeName (Type? type)
		{
			return type?.FullName ?? type?.Name ?? "<unknown>";
		}

		static string GetMethodName (MethodInfo method)
		{
			return GetTypeName (method.DeclaringType) + "." + method.Name;
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		static string GetBlockSignature (void* trampoline, MethodInfo trampolineMethod)
		{
			if (!Runtime.DynamicRegistrationSupported)
				throw ErrorHelper.CreateError (8050, Errors.MX8050 /* BlockLiteral.GetBlockSignature is not supported when the dynamic registrar has been linked away. */);

			// Verify that there's at least one parameter, and it must be System.IntPtr, void* or ObjCRuntime.BlockLiteral*.
			var parameters = trampolineMethod.GetParameters ();
			if (parameters.Length < 1)
				throw ErrorHelper.CreateError (8048, Errors.MX8048 /* The trampoline method {0} must have at least one parameter. */, GetMethodName (trampolineMethod));
			var firstParameterType = parameters [0].ParameterType;
			if (firstParameterType != typeof (IntPtr) &&
				firstParameterType != typeof (void*) &&
				firstParameterType != typeof (BlockLiteral*)) {
				throw ErrorHelper.CreateError (8049, Errors.MX8049 /* The first parameter in the trampoline method {0} must be either 'System.IntPtr', 'void*' or 'ObjCRuntime.BlockLiteral*'. */, GetMethodName (trampolineMethod));
			}

			// Verify that the method as an [UnmanagedCallersOnly] attribute
			if (!trampolineMethod.IsDefined (typeof (UnmanagedCallersOnlyAttribute), false))
				throw ErrorHelper.CreateError (8051, Errors.MX8051 /* The trampoline method {0} must have an [UnmanagedCallersOnly] attribute. */, GetMethodName (trampolineMethod));

			// We need to get the signature of the target method, so that we can compute
			// the ObjC signature correctly (the generated method that's actually
			// invoked by native code does not have enough type information to compute
			// the correct signature).
			// This attribute might not exist for third-party libraries created
			// with earlier versions of our SDK, so make sure to cope with
			// the attribute not being available.

			// This logic is mirrored in CoreOptimizeGeneratedCode.ProcessSetupBlock and must be
			// updated if anything changes here.
			TryGetUserDelegateType (trampolineMethod, trampolineMethod, out var userMethod);

			// We're good to go!
			return Runtime.ComputeSignature (userMethod, true);
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		void SetupBlock (Delegate trampoline, Delegate target, bool safe)
		{
			// Note that the code in this method shouldn't be called when using any static registrar, so throw an exception in that case.
			if (!Runtime.DynamicRegistrationSupported)
				throw ErrorHelper.CreateError (8026, "BlockLiteral.SetupBlock is not supported when the dynamic registrar has been linked away.");

			// We need to get the signature of the target method, so that we can compute
			// the ObjC signature correctly (the generated method that's actually
			// invoked by native code does not have enough type information to compute
			// the correct signature).
			// This attribute might not exist for third-party libraries created
			// with earlier versions of Xamarin.iOS, so make sure to cope with
			// the attribute not being available.

			// This logic is mirrored in CoreOptimizeGeneratedCode.ProcessSetupBlock and must be
			// updated if anything changes here.
			var blockSignature = TryGetUserDelegateType (trampoline.GetType (), trampoline.Method, out var userMethod);
			var signature = Runtime.ComputeSignature (userMethod, blockSignature);
			SetupBlockImpl (trampoline, target, safe, System.Text.Encoding.UTF8.GetBytes (signature));
		}

		// Note that the code in this method shouldn't be called when using any static registrar, so throw an exception in that case.
		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.PublicMethods' in call to 'System.Type.GetMethod(String)'. The return value of method 'ObjCRuntime.UserDelegateTypeAttribute.UserDelegateType.get' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2075", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		static bool TryGetUserDelegateType (MemberInfo provider, MethodInfo noUserDelegateTypeMethod, out MethodInfo userMethod)
		{
			var userDelegateType = provider.GetCustomAttribute<UserDelegateTypeAttribute> ()?.UserDelegateType;
			if (userDelegateType is not null) {
				var invokeMethod = userDelegateType.GetMethod ("Invoke");
				if (invokeMethod is null)
					throw new InvalidOperationException ($"The user delegate type {GetTypeName (userDelegateType)} does not have an Invoke method.");
				userMethod = invokeMethod;
				return true;
			} else {
				userMethod = noUserDelegateTypeMethod;
				return false;
			}
		}

		void SetupBlockImpl (Delegate trampoline, Delegate target, bool safe, string signature)
		{
			SetupBlockImpl (trampoline, target, safe, System.Text.Encoding.UTF8.GetBytes (signature));
		}

		void SetupBlockImpl (Delegate trampoline, Delegate target, bool safe, byte [] utf8Signature)
		{
			var invoke = Marshal.GetFunctionPointerForDelegate (trampoline);
			SetupFunctionPointerBlock (invoke, GetContext (trampoline, target, safe), utf8Signature);
		}

		static object GetContext (Delegate trampoline, Delegate target, bool safe)
		{
			if (safe) {
				return new Tuple<Delegate, Delegate> (trampoline, target);
			} else {
				return target;
			}
		}

		void SetupFunctionPointerBlock (IntPtr invokeMethod, object context, byte [] utf8Signature)
		{
			if (utf8Signature is null)
				ThrowHelper.ThrowArgumentNullException (nameof (utf8Signature));

			if (utf8Signature.Length == 0)
				ThrowHelper.ThrowArgumentException (nameof (utf8Signature), Errors.MX8052 /* The signature must be a non-empty string. */);

			isa = NSConcreteStackBlock;
			invoke = invokeMethod;
			local_handle = (IntPtr) GCHandle.Alloc (context);
			global_handle = IntPtr.Zero;
			flags = BlockFlags.BLOCK_HAS_COPY_DISPOSE | BlockFlags.BLOCK_HAS_SIGNATURE;

			/* FIXME: support stret blocks */

			// we allocate one big block of memory, the first part is the BlockDescriptor, 
			// the second part is the signature string (no need to allocate a second time
			// for the signature if we can avoid it). One descriptor is allocated for every 
			// Block; this is potentially something the static registrar can fix, since it
			// should know every possible trampoline signature.
			var bytes = utf8Signature;
			var hasNull = utf8Signature [utf8Signature.Length - 1] == 0;
			var desclen = sizeof (XamarinBlockDescriptor) + bytes.Length + (hasNull ? 0 : 1 /* null character */);
			var descptr = Marshal.AllocHGlobal (desclen);

			block_descriptor = descptr;
			var xblock_descriptor = (XamarinBlockDescriptor*) block_descriptor;
			xblock_descriptor->descriptor = *(BlockDescriptor*) xamarin_get_block_descriptor ();
			xblock_descriptor->descriptor.signature = descptr + sizeof (BlockDescriptor) + 4 /* signature_length */;
			xblock_descriptor->ref_count = 1;
			Marshal.Copy (bytes, 0, xblock_descriptor->descriptor.signature, bytes.Length);
			if (!hasNull)
				Marshal.WriteByte (xblock_descriptor->descriptor.signature + bytes.Length, 0); // null terminate string
		}

		// trampoline must be static, and someone else needs to keep a ref to it
		/// <summary>Sets up a block using a trampoline and a user delegate.</summary>
		/// <param name="trampoline">The trampoline must be a static delegate. The developer's code must keep a reference to this delegate.</param>
		/// <param name="userDelegate">The user code to invoke.</param>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public void SetupBlockUnsafe (Delegate trampoline, Delegate userDelegate)
		{
			SetupBlock (trampoline, userDelegate, safe: false);
		}

		// trampoline must be static, but it's not necessary to keep a ref to it
		/// <summary>Sets up a block using a trampoline and a user delegate.</summary>
		/// <param name="trampoline">The trampoline must be a static delegate. Xamarin.iOS will automatically keep a reference to this delegate.</param>
		/// <param name="userDelegate">The user code to invoke.</param>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public void SetupBlock (Delegate trampoline, Delegate userDelegate)
		{
			if (trampoline is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (trampoline));

			VerifyBlockDelegates (trampoline, userDelegate);

			SetupBlock (trampoline, userDelegate, safe: true);
		}

		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.PublicMethods' in call to 'System.Type.GetMethod(String)'. The return value of method 'ObjCRuntime.MonoPInvokeCallbackAttribute.DelegateType.get' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2075", Justification = "Calling GetMethod('Invoke') on a delegate type will always find something, because the invoke method can't be linked away for a delegate.")]
		void VerifyBlockDelegates (Delegate trampoline, Delegate userDelegate)
		{
#if !MONOMAC && !__MACCATALYST__
			// Check that:
			// * The trampoline is static
			// * The trampoline's method has a [MonoPInvokeCallback] attribute
			// * The delegate in the [MonoPInvokeCallback] has the right signature
			//
			// WARNING: the XAMARIN_IOS_SKIP_BLOCK_CHECK will be removed in a future version, 
			//          if you find you need it, please file a bug with a test case and we'll 
			//          make sure your scenario works without the environment variable before removing it.
			if (Runtime.Arch == Arch.SIMULATOR && string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("XAMARIN_IOS_SKIP_BLOCK_CHECK"))) {
				// It should be enough to run this check in the simulator
				var method = trampoline.Method;
				if (!method.IsStatic)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)} is not static.");
				var attrib = method.GetCustomAttribute<MonoPInvokeCallbackAttribute> (false);
				if (attrib is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)} does not have a [MonoPInvokeCallback] attribute.");

				var delegateType = attrib.DelegateType;
				if (delegateType is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)} has a [MonoPInvokeCallback] attribute with an invalid delegate type.");
				var signatureMethod = delegateType.GetMethod ("Invoke");
				if (signatureMethod is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)} has a [MonoPInvokeCallback] attribute with an invalid delegate type ({GetTypeName (delegateType)}).");
				if (method.ReturnType != signatureMethod.ReturnType)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)}'s return type ({method.ReturnType.FullName}) does not match the return type of the delegate in its [MonoPInvokeCallback] attribute ({signatureMethod.ReturnType.FullName}).");

				var parameters = method.GetParameters ();
				var signatureParameters = signatureMethod.GetParameters ();
				if (parameters.Length != signatureParameters.Length)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)}'s parameter count ({parameters.Length}) does not match the parameter count of the delegate in its [MonoPInvokeCallback] attribute ({signatureParameters.Length}).");

				for (int i = 0; i < parameters.Length; i++) {
					if (parameters [i].ParameterType != signatureParameters [i].ParameterType)
						ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (trampoline), $"The method {GetMethodName (method)}'s parameter #{i + 1}'s type ({parameters [i].ParameterType.FullName}) does not match the corresponding parameter type of the delegate in its [MonoPInvokeCallback] attribute ({signatureParameters [i].ParameterType.FullName}).");
				}
			}
#endif

		}

		/// <summary>Releases the resources associated with this block.</summary>
		/// <remarks>
		/// <para>This releases the <see cref="GCHandle"/> that points to the user delegate.</para>
		/// </remarks>
		public void CleanupBlock ()
		{
			Dispose ();
		}

		/// <summary>Releases the resources associated with this block.</summary>
		public void Dispose ()
		{
			if (local_handle != IntPtr.Zero) {
				GCHandle.FromIntPtr (local_handle).Free ();
				local_handle = IntPtr.Zero;
			}

			if (block_descriptor != IntPtr.Zero) {
				var xblock_descriptor = (XamarinBlockDescriptor*) block_descriptor;
#pragma warning disable 420
				// CS0420: A volatile field references will not be treated as volatile
				// Documentation says: "A volatile field should not normally be passed using a ref or out parameter, since it will not be treated as volatile within the scope of the function. There are exceptions to this, such as when calling an interlocked API."
				// So ignoring the warning, since it's a documented exception.
				// Interlocked.Decrement returns the new value after the subtraction
				var rc = Interlocked.Decrement (ref xblock_descriptor->ref_count);
#pragma warning restore 420

				if (rc == 0)
					Marshal.FreeHGlobal (block_descriptor);
				block_descriptor = IntPtr.Zero;
			}
		}

		/// <summary>Gets the context value that was specified when creating the <see cref="BlockLiteral"/>.</summary>
		/// <value>The context value.</value>
		public object? Context {
			get {
				var handle = global_handle != IntPtr.Zero ? global_handle : local_handle;
				return GCHandle.FromIntPtr (handle).Target;
			}
		}

		/// <summary>Gets the target object for the block.</summary>
		/// <value>The target object if this block wraps a managed delegate; otherwise the context value.</value>
		public object? Target {
			get {
				var target = Context;
				var tuple = target as Tuple<Delegate, Delegate>;
				if (tuple is not null)
					return tuple.Item2;
				return target;
			}
		}

		/// <summary>Returns a managed delegate of type <typeparamref name="T"/> that can invoke the native block.</summary>
		/// <typeparam name="T">The delegate type to create.</typeparam>
		/// <returns>The delegate of type <typeparamref name="T"/> for this block.</returns>
		/// <remarks>This method supports the Xamarin.iOS runtime and is not intended for use by application developers.</remarks>
		public T GetDelegateForBlock<T> () where T : System.MulticastDelegate
		{
			return Runtime.GetDelegateForBlock<T> (invoke);
		}

		/// <summary>Returns the managed delegate represented by the specified block.</summary>
		/// <typeparam name="T">The type of the managed delegate to return.</typeparam>
		/// <param name="block">The pointer to the native block.</param>
		/// <returns>The managed delegate for this block, or <see langword="null"/> if no managed delegate is available.</returns>
		/// <remarks>
		/// <para>Behavior is undefined if this block does not represent a managed delegate.</para>
		/// </remarks>
		public unsafe static T? GetTarget<T> (IntPtr block) where T : System.MulticastDelegate
		{
			var target = ((BlockLiteral*) block)->Target;
			if (target is null)
				return null;
			return (T) target;
		}

		/// <summary>Determines whether the specified block wraps a managed delegate.</summary>
		/// <param name="block">The pointer to the native block.</param>
		/// <returns><see langword="true"/> if the block contains a managed delegate; otherwise, <see langword="false"/>.</returns>
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static bool IsManagedBlock (IntPtr block)
		{
			if (block == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (block));

			BlockLiteral* literal = (BlockLiteral*) block;
			BlockDescriptor* descriptor = (BlockDescriptor*) xamarin_get_block_descriptor ();
			return descriptor->copy_helper == ((BlockDescriptor*) literal->block_descriptor)->copy_helper;
		}

		// This method should never be called when using the managed static registrar, so assert that never happens by throwing an exception in that case.
		// This method doesn't necessarily work with NativeAOT, but this is covered by the exception, because the managed static registrar is required for NativeAOT.
		//
		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.Interfaces' in call to 'System.Type.GetInterfaces()'. The return value of method 'System.Reflection.MemberInfo.DeclaringType.get' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2075", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		// IL2072: 'interfaceType' argument does not satisfy 'DynamicallyAccessedMemberTypes.PublicMethods', 'DynamicallyAccessedMemberTypes.NonPublicMethods' in call to 'System.Type.GetInterfaceMap(Type)'. The return value of method 'System.Type.GetInterfaces()' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		[UnconditionalSuppressMessage ("", "IL2072", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		static Type? GetDelegateProxyType (MethodInfo minfo, uint token_ref, out MethodInfo? baseMethod)
		{
			// Note that the code in this method doesn't necessarily work with NativeAOT, so assert that never happens by throwing an exception if using the managed static registrar (which is required for NativeAOT)
			if (Runtime.IsManagedStaticRegistrar)
				throw new System.Diagnostics.UnreachableException ();

			// A mirror of this method is also implemented in StaticRegistrar:GetDelegateProxyType
			// If this method is changed, that method will probably have to be updated too (tests!!!)
			baseMethod = null;

			if (token_ref != Runtime.INVALID_TOKEN_REF)
				return Class.ResolveTypeTokenReference (token_ref);

			baseMethod = minfo.GetBaseDefinition ();
			var delegateProxies = baseMethod.ReturnTypeCustomAttributes.GetCustomAttributes (typeof (DelegateProxyAttribute), false);
			if (delegateProxies.Length > 0)
				return ((DelegateProxyAttribute) delegateProxies [0]).DelegateType;

			// We might be implementing a protocol, find any DelegateProxy attributes on the corresponding interface as well.
			string? selector = null;
			var declaringType = minfo.DeclaringType;
			if (declaringType is null)
				throw ErrorHelper.CreateError (8011, $"Unable to locate the delegate to block conversion attribute ([DelegateProxy]) for the return value for the method {GetMethodName (minfo)}. {Constants.PleaseFileBugReport}");
			foreach (var iface in declaringType.GetInterfaces ()) {
				if (!iface.IsDefined (typeof (ProtocolAttribute), false))
					continue;

				var map = declaringType.GetInterfaceMap (iface);
				for (int i = 0; i < map.TargetMethods.Length; i++) {
					if (map.TargetMethods [i] == minfo) {
						delegateProxies = map.InterfaceMethods [i].ReturnTypeCustomAttributes.GetCustomAttributes (typeof (DelegateProxyAttribute), false);
						if (delegateProxies.Length > 0)
							return ((DelegateProxyAttribute) delegateProxies [0]).DelegateType;
					}
				}

				// It might be an optional method/property, in which case we need to check any ProtocolMember attributes
				if (selector is null)
					selector = Runtime.GetExportAttribute (minfo)?.Selector ?? string.Empty;
				if (!string.IsNullOrEmpty (selector)) {
					var attrib = Runtime.GetProtocolMemberAttribute (iface, selector, minfo);
					if (attrib?.ReturnTypeDelegateProxy is not null)
						return attrib.ReturnTypeDelegateProxy;
				}
			}

			throw ErrorHelper.CreateError (8011, $"Unable to locate the delegate to block conversion attribute ([DelegateProxy]) for the return value for the method {GetMethodName (baseMethod ?? minfo)}. {Constants.PleaseFileBugReport}");
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		unsafe static IntPtr GetBlockForFunctionPointer (MethodInfo delegateInvokeMethod, object @delegate, string? signature)
		{
			void* invokeFunctionPointer = (void*) delegateInvokeMethod.MethodHandle.GetFunctionPointer ();
			if (signature is null) {
				if (!Runtime.DynamicRegistrationSupported)
					throw ErrorHelper.CreateError (8026, $"BlockLiteral.GetBlockForDelegate with a null signature is not supported when the dynamic registrar has been linked away (delegate type: {@delegate.GetType ().FullName}).");

				using (var block = new BlockLiteral (invokeFunctionPointer, @delegate, delegateInvokeMethod))
					return _Block_copy (&block);
			} else {
				using (var block = new BlockLiteral (invokeFunctionPointer, @delegate, signature))
					return _Block_copy (&block);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[BindingImpl (BindingImplOptions.Optimizable)]
		static IntPtr CreateBlockForDelegate (Delegate @delegate, Delegate delegateProxyFieldValue, string? signature)
		{
			if (@delegate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (@delegate));

			if (delegateProxyFieldValue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (delegateProxyFieldValue));

			// Note that we must create a heap-allocated block, so we
			// start off by creating a stack-allocated block, and then
			// call _Block_copy, which will create a heap-allocated block
			// with the proper reference count.
			using var block = new BlockLiteral ();
			if (signature is null) {
				if (Runtime.DynamicRegistrationSupported) {
					block.SetupBlock (delegateProxyFieldValue, @delegate);
				} else {
					throw ErrorHelper.CreateError (8026, $"BlockLiteral.GetBlockForDelegate with a null signature is not supported when the dynamic registrar has been linked away (delegate type: {@delegate.GetType ().FullName}).");
				}
			} else {
				block.SetupBlockImpl (delegateProxyFieldValue, @delegate, true, signature);
			}
			return _Block_copy (&block);
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		// This method should never be called when using the managed static registrar, so assert that never happens by throwing an exception in that case.
		// This method doesn't necessarily work with NativeAOT, but this is covered by the exception, because the managed static registrar is required for NativeAOT.
		//
		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.NonPublicFields' in call to 'System.Type.GetField(String, BindingFlags)'. The return value of method 'ObjCRuntime.BlockLiteral.GetDelegateProxyType(MethodInfo, UInt32, MethodInfo&)' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to.
		// IL2075: 'this' argument does not satisfy 'DynamicallyAccessedMemberTypes.NonPublicMethods' in call to 'System.Type.GetMethod(String, BindingFlags)'. The return value of method 'ObjCRuntime.BlockLiteral.GetDelegateProxyType(MethodInfo, UInt32, MethodInfo&)' does not have matching annotations. The source value must declare at least the same requirements as those declared on the target location it is assigned to."
		[UnconditionalSuppressMessage ("", "IL2075", Justification = "The APIs this method tries to access are marked by other means, so this is linker-safe.")]
		internal static IntPtr GetBlockForDelegate (MethodInfo minfo, object? @delegate, uint token_ref, string? signature)
		{
			// Note that the code in this method doesn't necessarily work with NativeAOT, so assert that never happens by throwing an exception if using the managed static registrar (which is required for NativeAOT)
			if (Runtime.IsManagedStaticRegistrar)
				throw new System.Diagnostics.UnreachableException ();

			if (@delegate is null)
				return IntPtr.Zero;

			if (@delegate is not Delegate managedDelegate)
				throw ErrorHelper.CreateError (8016, $"Unable to convert delegate to block for the return value for the method {GetMethodName (minfo)}, because the input isn't a delegate, it's a {@delegate.GetType ().FullName}. {Constants.PleaseFileBugReport}");

			if (Runtime.IsNativeAOT)
				throw Runtime.CreateNativeAOTNotSupportedException ();

			var delegateProxyType = GetDelegateProxyType (minfo, token_ref, out var baseMethod);
			if (baseMethod is null)
				baseMethod = minfo; // 'baseMethod' is only used in error messages, and if it's null, we just use the closest alternative we have (minfo).
			if (delegateProxyType is null)
				throw ErrorHelper.CreateError (8012, $"Invalid DelegateProxyAttribute for the return value for the method {GetMethodName (baseMethod)}: DelegateType is null. {Constants.PleaseFileBugReport}");

			var delegateInvokeMethod = delegateProxyType.GetMethod ("Invoke", BindingFlags.NonPublic | BindingFlags.Static);
			if (delegateInvokeMethod is null)
				throw ErrorHelper.CreateError (8060, Errors.MX8060 /* Invalid DelegateProxyAttribute for the return value for the method {0}.{1}: No 'Invoke' method found. {Constants.PleaseFileBugReport} */, GetTypeName (baseMethod.DeclaringType), baseMethod.Name);

			if (delegateInvokeMethod.IsDefined (typeof (UnmanagedCallersOnlyAttribute), false))
				return GetBlockForFunctionPointer (delegateInvokeMethod, managedDelegate, signature);

			var delegateProxyField = delegateProxyType.GetField ("Handler", BindingFlags.NonPublic | BindingFlags.Static);
			if (delegateProxyField is null)
				throw ErrorHelper.CreateError (8013, $"Invalid DelegateProxyAttribute for the return value for the method {GetMethodName (baseMethod)}: DelegateType ({GetTypeName (delegateProxyType)}) specifies a type without a 'Handler' field. {Constants.PleaseFileBugReport}");

			var handlerDelegate = delegateProxyField.GetValue (null);
			if (handlerDelegate is null)
				throw ErrorHelper.CreateError (8014, $"Invalid DelegateProxyAttribute for the return value for the method {GetMethodName (baseMethod)}: The DelegateType's ({GetTypeName (delegateProxyType)}) 'Handler' field is null. {Constants.PleaseFileBugReport}");

			if (handlerDelegate is not Delegate handler)
				throw ErrorHelper.CreateError (8015, $"Invalid DelegateProxyAttribute for the return value for the method {GetMethodName (baseMethod)}: The DelegateType's ({GetTypeName (delegateProxyType)}) 'Handler' field is not a delegate, it's a {handlerDelegate.GetType ().FullName}. {Constants.PleaseFileBugReport}");

			// We now have the information we need to create the block.
			// Note that we must create a heap-allocated block, so we 
			// start off by creating a stack-allocated block, and then
			// call _Block_copy, which will create a heap-allocated block
			// with the proper reference count.
			using var block = new BlockLiteral ();
			if (signature is null) {
				if (Runtime.DynamicRegistrationSupported) {
					block.SetupBlock (handler, managedDelegate);
				} else {
					throw ErrorHelper.CreateError (8026, $"BlockLiteral.GetBlockForDelegate with a null signature is not supported when the dynamic registrar has been linked away (delegate type: {managedDelegate.GetType ().FullName}).");
				}
			} else {
				block.SetupBlockImpl (handler, managedDelegate, true, signature);
			}

			unsafe {
				return _Block_copy (&block);
			}
		}

		[DllImport (Messaging.LIBOBJC_DYLIB)]
		internal static extern IntPtr _Block_copy (BlockLiteral* block);

		[DllImport (Messaging.LIBOBJC_DYLIB)]
		internal static extern IntPtr _Block_copy (IntPtr block);

		[DllImport (Messaging.LIBOBJC_DYLIB)]
		internal static extern void _Block_release (IntPtr block);

		internal static IntPtr Copy (IntPtr block)
		{
			return _Block_copy (block);
		}
#endif
	}

#if !COREBUILD
	// This class sole purpose is to keep a static field that is initialized on
	// first use of the class

	internal class BlockStaticDispatchClass {
		[UnmanagedCallersOnly]
		internal static unsafe void TrampolineDispatchBlock (IntPtr block)
		{
			var del = BlockLiteral.GetTarget<Action> (block);
			if (del is not null) {
				del ();
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		unsafe internal static BlockLiteral CreateBlock (Action action)
		{
			delegate* unmanaged<IntPtr, void> trampoline = &BlockStaticDispatchClass.TrampolineDispatchBlock;
			return new BlockLiteral (trampoline, action, typeof (BlockStaticDispatchClass), nameof (TrampolineDispatchBlock));
		}
	}

	// This class will free the specified block when it's collected by the GC.
	internal class BlockCollector {
		IntPtr block;
		int count;
		public BlockCollector (IntPtr block)
		{
			this.block = block;
			count = 1;
		}

		public void Add (IntPtr block)
		{
			if (block != this.block)
				throw new InvalidOperationException (string.Format ("Can't release the block 0x{0} because this BlockCollector instance is already tracking 0x{1}.", block.ToString ("x"), this.block.ToString ("x")));
			Interlocked.Increment (ref count);
		}

		~BlockCollector ()
		{
			for (var i = 0; i < count; i++)
				Runtime.ReleaseBlockOnMainThread (block);
			count = 0;
		}
	}
#endif

	/// <summary>Flags for the BlockLiteral enum.</summary>
	/// <remarks>
	///   <para>Only the flags <see cref="BlockFlags.BLOCK_HAS_COPY_DISPOSE" /> and <see cref="BlockFlags.BLOCK_HAS_SIGNATURE" /> are used for the blocks we create.</para>
	///   <para>See <see href="https://clang.llvm.org/docs/Block-ABI-Apple.html">Block ABI</see> for more detailed information about the Block ABI.</para>
	/// </remarks>
	[Flags]
	internal enum BlockFlags : int {
		/// <summary>Objective-C Block ABI Flags.</summary>
		BLOCK_REFCOUNT_MASK = (0xffff),
		/// <summary>Objective-C Block ABI Flags.</summary>
		BLOCK_NEEDS_FREE = (1 << 24),
		/// <summary>Objective-C Block ABI Flags</summary>
		BLOCK_HAS_COPY_DISPOSE = (1 << 25),
		/// <summary>Objective-C Block ABI Flags</summary>
		BLOCK_HAS_CTOR = (1 << 26), /* Helpers have C++ code. */
		/// <summary>Objective-C Block ABI Flags.</summary>
		BLOCK_IS_GC = (1 << 27),
		/// <summary>Objective-C Block ABI Flags.</summary>
		BLOCK_IS_GLOBAL = (1 << 28),
		/// <summary>Whether the block_descriptor field is filled in.</summary>
		BLOCK_HAS_DESCRIPTOR = (1 << 29), // This meaning was deprecated 
		/// <summary>Objective-C Block ABI Flags.</summary>
		BLOCK_HAS_STRET = (1 << 29),
		/// <summary>Objective-C Block ABI Flags</summary>
		BLOCK_HAS_SIGNATURE = (1 << 30),
	}
}
