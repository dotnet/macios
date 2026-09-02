//
// UIAppearance.cs: Code to return the class_ptr that we access elsewhere
//
// Author:
//   Miguel de Icaza
//
// Copyright 2011, 2015 Xamarin Inc
//

#nullable enable

namespace UIKit {
#if __TVOS__
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='tvOS:T:UIKit.UIAppearance']/*" />
#elif __MACCATALYST__
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='MacCatalyst:T:UIKit.UIAppearance']/*" />
#else
	/// <include file="../../docs/api/UIKit/UIAppearance.xml" path="/Documentation/Docs[@DocId='T:UIKit.UIAppearance']/*" />
#endif
	public partial class UIAppearance {
		/// <summary>Determines whether the specified object is equal to the current <see cref="UIAppearance" />.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <returns><see langword="true" /> if <paramref name="other" /> is a <see cref="UIAppearance" /> and has the same <see cref="Foundation.NSObject.Handle" />; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? other)
		{
			var ao = other as UIAppearance;
			if (ao is null)
				return false;
			return ao.Handle == Handle;
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A hash code for the current instance.</returns>
		public override int GetHashCode ()
		{
			return Handle.GetHashCode ();
		}

		/// <summary>Determines whether two <see cref="UIAppearance" /> instances have the same native handle.</summary>
		/// <param name="a">The first instance to compare.</param>
		/// <param name="b">The second instance to compare.</param>
		/// <returns><see langword="true" /> if both instances are <see langword="null" /> or have the same <see cref="Foundation.NSObject.Handle" />; otherwise, <see langword="false" />.</returns>
		public static bool operator == (UIAppearance? a, UIAppearance? b)
		{
			if (ReferenceEquals (a, null))
				return ReferenceEquals (b, null);
			else if (ReferenceEquals (b, null))
				return false;
			return a.Handle == b.Handle;
		}

		/// <summary>Determines whether two <see cref="UIAppearance" /> instances have different native handles.</summary>
		/// <param name="a">The first instance to compare.</param>
		/// <param name="b">The second instance to compare.</param>
		/// <returns><see langword="true" /> if the instances have different native handles or exactly one is <see langword="null" />; otherwise, <see langword="false" />.</returns>
		public static bool operator != (UIAppearance? a, UIAppearance? b)
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
		const string selAppearanceWhenContainedInInstancesOfClasses = "appearanceWhenContainedInInstancesOfClasses:";

		/// <summary>Gets the appearance proxy for a class in the specified containment hierarchy.</summary>
		/// <param name="class_ptr">The Objective-C class pointer for the type to get the appearance proxy for.</param>
		/// <param name="whenFoundIn">The types representing the containment hierarchy in which the appearance should be applied.</param>
		/// <returns>The appearance proxy for the specified class when found in the specified containment hierarchy.</returns>
		public static IntPtr GetAppearance (IntPtr class_ptr, params Type [] whenFoundIn)
		{
			using (var array = NSArray.FromIntPtrs (TypesToPointers (whenFoundIn))) {
				return Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr,
					Selector.GetHandle (UIAppearance.selAppearanceWhenContainedInInstancesOfClasses), array.Handle);
			}
		}

		const string selAppearanceForTraitCollectionWhenContainedInInstancesOfClasses = "appearanceForTraitCollection:whenContainedInInstancesOfClasses:";

		/// <summary>Gets the appearance proxy for a class with the specified trait collection in the specified containment hierarchy.</summary>
		/// <param name="class_ptr">The Objective-C class pointer for the type to get the appearance proxy for.</param>
		/// <param name="traits">The <see cref="UITraitCollection" /> for which to return the appearance proxy.</param>
		/// <param name="whenFoundIn">The types representing the containment hierarchy in which the appearance should be applied.</param>
		/// <returns>The appearance proxy for the specified class and traits when found in the specified containment hierarchy.</returns>
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits, params Type [] whenFoundIn)
		{
			ArgumentNullException.ThrowIfNull (traits);

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

		/// <summary>Gets the appearance proxy for a class in the specified containment hierarchy.</summary>
		/// <param name="class_ptr">The Objective-C class pointer for the type to get the appearance proxy for.</param>
		/// <param name="whenFoundIn">The types representing the containment hierarchy in which the appearance should be applied.</param>
		/// <returns>The appearance proxy for the specified class when found in the specified containment hierarchy.</returns>
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

		/// <summary>Gets the appearance proxy for a class with the specified trait collection in the specified containment hierarchy.</summary>
		/// <param name="class_ptr">The Objective-C class pointer for the type to get the appearance proxy for.</param>
		/// <param name="traits">The <see cref="UITraitCollection" /> for which to return the appearance proxy.</param>
		/// <param name="whenFoundIn">The types representing the containment hierarchy in which the appearance should be applied.</param>
		/// <returns>The appearance proxy for the specified class and traits when found in the specified containment hierarchy.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits, params Type [] whenFoundIn)
		{
			ArgumentNullException.ThrowIfNull (traits);

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
#endif

		const string selAppearanceForTraitCollection = "appearanceForTraitCollection:";

		/// <summary>Gets the appearance proxy for a class with the specified trait collection.</summary>
		/// <param name="class_ptr">The Objective-C class pointer for the type to get the appearance proxy for.</param>
		/// <param name="traits">The <see cref="UITraitCollection" /> for which to return the appearance proxy.</param>
		/// <returns>The appearance proxy for the specified class and traits.</returns>
		public static IntPtr GetAppearance (IntPtr class_ptr, UITraitCollection traits)
		{
			ArgumentNullException.ThrowIfNull (traits);

			IntPtr result = Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle (UIAppearance.selAppearanceForTraitCollection), traits.Handle);
			GC.KeepAlive (traits);
			return result;
		}
	}
}
