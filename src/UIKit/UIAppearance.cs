//
// UIAppearance.cs: Code to return the class_ptr that we access elsewhere
//
// Author:
//   Miguel de Icaza
//
// Copyright 2011, 2015 Xamarin Inc
//

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
#if __TVOS__
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='tvOS:T:UIKit.UIAppearance']/*" />
#elif __MACCATALYST__
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='MacCatalyst:T:UIKit.UIAppearance']/*" />
#else
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='T:UIKit.UIAppearance']/*" />
#endif
	public partial class UIAppearance {
		/// <param name="other">To be added.</param>
		///         <summary>Whether this is equivalent to <paramref name="other" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object other)
		{
			UIAppearance ao = other as UIAppearance;
			if (ao is null)
				return false;
			return ao.Handle == Handle;
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		///         <returns>A int containing the hash code for this instance.</returns>
		///         <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return Handle.GetHashCode ();
		}

		public static bool operator == (UIAppearance a, UIAppearance b)
		{
			if (ReferenceEquals (a, null))
				return ReferenceEquals (b, null);
			else if (ReferenceEquals (b, null))
				return false;
			return a.Handle == b.Handle;
		}

		public static bool operator != (UIAppearance a, UIAppearance b)
		{
			return !(a == b);
		}

		static IntPtr [] TypesToPointers (Type [] whenFoundIn)
		{
			var ptrs = new IntPtr [whenFoundIn.Length];
			for (int i = 0; i < whenFoundIn.Length; i++) {
				if (whenFoundIn [i] is null)
					throw new ArgumentException (String.Format ("Parameter {0} was null, must specify a valid type", i));
				if (!typeof (NSObject).IsAssignableFrom (whenFoundIn [i]))
					throw new ArgumentException (String.Format ("Type {0} does not derive from NSObject", whenFoundIn [i]));

				var classHandle = Class.GetHandle (whenFoundIn [i]);
				if (classHandle == NativeHandle.Zero)
					throw new ArgumentException (string.Format ("Could not find the Objective-C class for {0}", whenFoundIn [i].FullName));

				ptrs [i] = classHandle;
			}
			return ptrs;
		}

#if TVOS
		// new in iOS9 but the only option for tvOS
		const string selAppearanceWhenContainedInInstancesOfClasses = "appearanceWhenContainedInInstancesOfClasses:";

		// +(instancetype _Nonnull)appearanceWhenContainedInInstancesOfClasses:(NSArray<Class<UIAppearanceContainer>> * _Nonnull)containerTypes
		public static IntPtr GetAppearance (IntPtr class_ptr, params Type [] whenFoundIn)
		{
			using (var array = NSArray.FromIntPtrs (TypesToPointers (whenFoundIn))) {
				return Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr,
					Selector.GetHandle (UIAppearance.selAppearanceWhenContainedInInstancesOfClasses), array.Handle);
			}
		}

		const string selAppearanceForTraitCollectionWhenContainedInInstancesOfClasses = "appearanceForTraitCollection:whenContainedInInstancesOfClasses:";

		// new in iOS9 but the only option for tvOS
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits, params Type [] whenFoundIn)
		{
			if (traits is null)
				throw new ArgumentNullException ("traits");

			using (var array = NSArray.FromIntPtrs (TypesToPointers (whenFoundIn))) {
				IntPtr result = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr,
					Selector.GetHandle (UIAppearance.selAppearanceForTraitCollectionWhenContainedInInstancesOfClasses),
					traits.Handle, array.Handle);
				GC.KeepAlive (traits);
				return result;
			}
		}
#else
		const string selAppearanceWhenContainedIn = "appearanceWhenContainedIn:";
		const string selAppearanceForTraitCollectionWhenContainedIn = "appearanceForTraitCollection:whenContainedIn:";

		/// <param name="class_ptr">To be added.</param>
		///         <param name="whenFoundIn">To be added.</param>
		///         <summary>This object's appearance proxy in the specified containment hierarchy.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static IntPtr GetAppearance (IntPtr class_ptr, params Type [] whenFoundIn)
		{
			var ptrs = TypesToPointers (whenFoundIn);

			// The first type is not a varargs, but the subsequent ones are
			var firstPtr = ptrs [0];
			Array.Copy (ptrs, 1, ptrs, 0, ptrs.Length - 1);
			Array.Resize (ref ptrs, ptrs.Length - 1);
			return Messaging.objc_msgSend_3_vargs (
				class_ptr,
				Selector.GetHandle (UIAppearance.selAppearanceWhenContainedIn),
				firstPtr,
				ptrs);
		}

		/// <param name="class_ptr">To be added.</param>
		///         <param name="traits">To be added.</param>
		///         <param name="whenFoundIn">To be added.</param>
		///         <summary>Returns an appearance proxy for the specified <paramref name="traits" /> when found in the <paramref name="whenFoundIn" /> containment hierarchy.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits, params Type [] whenFoundIn)
		{
			if (traits is null)
				throw new ArgumentNullException ("traits");

			var ptrs = TypesToPointers (whenFoundIn);

			// The first type is not a varargs, but the subsequent ones are
			var firstPtr = ptrs [0];
			Array.Copy (ptrs, 1, ptrs, 0, ptrs.Length - 1);
			Array.Resize (ref ptrs, ptrs.Length - 1);
			IntPtr result = Messaging.objc_msgSend_4_vargs (
				class_ptr,
				Selector.GetHandle (UIAppearance.selAppearanceForTraitCollectionWhenContainedIn),
				traits.Handle,
				firstPtr,
				ptrs);
			GC.KeepAlive (traits);
			return result;
		}

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
		extern static IntPtr IntPtr_objc_msgSend_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr_IntPtr (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4, System.IntPtr arg5, System.IntPtr arg6, System.IntPtr arg7, System.IntPtr arg8, System.IntPtr arg9, System.IntPtr arg10, System.IntPtr arg11);
#endif

		const string selAppearanceForTraitCollection = "appearanceForTraitCollection:";

		/// <param name="class_ptr">To be added.</param>
		///         <param name="traits">To be added.</param>
		///         <summary>Returns an appearance proxy for the specified <paramref name="traits" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits)
		{
			if (traits is null)
				throw new ArgumentNullException ("traits");

			IntPtr result = Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle (UIAppearance.selAppearanceForTraitCollection), traits.Handle);
			GC.KeepAlive (traits);
			return result;
		}
	}
}
