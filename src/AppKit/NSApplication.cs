//
// Copyright 2010, Novell, Inc.
// Copyright 2012 Xamarin Inc.
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

#if !__MACCATALYST__

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	public partial class NSApplication : NSResponder {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static bool CheckForIllegalCrossThreadCalls = true;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static bool CheckForEventAndDelegateMismatches = true;

		private static Thread? mainThread;

		[DllImport (Constants.AppKitLibrary)]
		extern static int /* int */ NSApplicationMain (int /* int */ argc, IntPtr argv);

		static bool initialized;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Preserve]
		public static void Init ()
		{
			if (initialized) {
				throw new InvalidOperationException ("Init has already been invoked; it can only be invoked once");
			}

			Runtime.EnsureInitialized ();

			initialized = true;

			if (Runtime.DynamicRegistrationSupported)
				Runtime.RegisterAssemblies ();

			// Runtime hosts embedding MonoMac may use a different sync context 
			// and call NSApplicationMain externally prior to this Init, so only
			// initialize the context if it hasn't been set externally. Alternatively,
			// AppKitSynchronizationContext could be made public.
			if (SynchronizationContext.Current is null)
				SynchronizationContext.SetSynchronizationContext (new AppKitSynchronizationContext ());

			// Establish the main thread at the time of Init to support hosts
			// that don't call Main.
			NSApplication.mainThread = Thread.CurrentThread;

			// Launcher sets this to work around https://bugzilla.xamarin.com/show_bug.cgi?id=45279
			// But can affect child xbuild processes, so unset
			Environment.SetEnvironmentVariable ("MONO_CFG_DIR", "");

			// custom initialization might have happened before native NSApplication code was full ready to be queried
			// as such it's possible that `class_ptr` might be empty and that will make things fails later
			// reference: https://github.com/dotnet/macios/issues/7932
			if (class_ptr == IntPtr.Zero)
				ResetHandle ();

			// TODO:
			//   Install hook to register dynamically loaded assemblies
		}

		// separate method so it can be invoked without `Init` (if needed)
		static void ResetHandle ()
		{
			// `class_ptr` is `readonly` so one can't simply do `class_ptr = Class.GetHandle ("NSApplication");`
			typeof (NSApplication).GetField ("class_ptr", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue (null, Class.GetHandle ("NSApplication"));
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void InitDrawingBridge ()
		{
			var UseCocoaDrawableField = Type.GetType ("System.Drawing.GDIPlus, System.Drawing")?.GetField ("UseCocoaDrawable", BindingFlags.Static | BindingFlags.Public);
			var UseCarbonDrawableField = Type.GetType ("System.Drawing.GDIPlus, System.Drawing")?.GetField ("UseCarbonDrawable", BindingFlags.Static | BindingFlags.Public);

			UseCocoaDrawableField?.SetValue (null, true);
			UseCarbonDrawableField?.SetValue (null, false);
		}

		/// <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void Main (string [] args)
		{
			// Switch to an AppKitSynchronizationContext if Main is invoked
			if (SynchronizationContext.Current is null || !typeof (AppKitSynchronizationContext).IsAssignableFrom (SynchronizationContext.Current.GetType ()))
				SynchronizationContext.SetSynchronizationContext (new AppKitSynchronizationContext ());

			// Init where this is set the first time is generally paired
			// with a call to Main, but this guarantees the right thread.
			NSApplication.mainThread = Thread.CurrentThread;

			var argsPtr = TransientString.AllocStringArray (args);
			NSApplicationMain (args.Length, argsPtr);
			if (argsPtr != IntPtr.Zero)
				TransientString.FreeStringArray (argsPtr, args.Length);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void EnsureUIThread ()
		{
			if (NSApplication.CheckForIllegalCrossThreadCalls && NSApplication.mainThread != Thread.CurrentThread)
				throw new AppKitThreadAccessException ();
		}

		/// <param name="del">To be added.</param>
		///         <param name="expectedType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void EnsureEventAndDelegateAreNotMismatched (object del, Type expectedType)
		{
			if (NSApplication.CheckForEventAndDelegateMismatches && !(expectedType.IsAssignableFrom (del.GetType ())))
				throw new InvalidOperationException (string.Format ("Event registration is overwriting existing delegate. Either just use events or your own delegate: {0} {1}", del.GetType (), expectedType));
		}

		/// <param name="currentDelegateValue">To be added.</param>
		///         <param name="newDelegateValue">To be added.</param>
		///         <param name="internalDelegateType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void EnsureDelegateAssignIsNotOverwritingInternalDelegate (object? currentDelegateValue, object? newDelegateValue, Type internalDelegateType)
		{
			if (NSApplication.CheckForEventAndDelegateMismatches && currentDelegateValue is not null && newDelegateValue is not null
				&& currentDelegateValue.GetType ().IsAssignableFrom (internalDelegateType)
				&& !newDelegateValue.GetType ().IsAssignableFrom (internalDelegateType))
				throw new InvalidOperationException (string.Format ("Event registration is overwriting existing delegate. Either just use events or your own delegate: {0} {1}", newDelegateValue.GetType (), internalDelegateType));
		}

		/// <param name="mask">To be added.</param>
		///         <param name="lastEvent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DiscardEvents (NSEventMask mask, NSEvent lastEvent)
		{
			DiscardEvents ((nuint) (ulong) mask, lastEvent);
		}

		// note: if needed override the protected Get|Set methods
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSApplicationActivationPolicy ActivationPolicy {
			get { return GetActivationPolicy (); }
			// ignore return value (bool)
			set { SetActivationPolicy (value); }
		}
	}
}
#endif // !__MACCATALYST__
