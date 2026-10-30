//
// This file contains a class used to consume NSFastEnumeration
//
// Authors:
//   Rolf Bjarne Kvinge
//
// Copyright 2015, Xamarin Inc.
//

using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace Foundation {
	internal class NSFastEnumerator {
		internal static readonly IntPtr CountByEnumeratingWithStateHandle = Selector.GetHandle ("countByEnumeratingWithState:objects:count:");

		[DllImport (Messaging.LIBOBJC_DYLIB, EntryPoint = "objc_msgSend")]
		public unsafe extern static nuint objc_msgSend (IntPtr receiver, IntPtr selector, NSFastEnumerationState* arg1, IntPtr* arg2, nuint arg3);
	}

	/// <summary>Enumerates the objects in a native collection using the NSFastEnumeration protocol.</summary>
	/// <typeparam name="T">The managed native object or protocol type of each element.</typeparam>
	public sealed class NSFastEnumerator<T> : IEnumerator<T>
		where T : class, INativeObject {
		unsafe NSFastEnumerationState* state;
		INativeObject collection;
		nuint count;
		IntPtr mutationValue;
		nuint current;
		bool started;
		bool finished;

		/// <summary>Creates an enumerator for a native collection.</summary>
		/// <param name="collection">A native object that implements the NSFastEnumeration protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="collection"/> has been disposed.</exception>
		public NSFastEnumerator (INativeObject collection)
		{
			collection.GetNonNullHandle (nameof (collection));
			this.collection = collection;

			unsafe {
				// Create one blob of native memory that holds both our NSFastEnumerationState and the array of pointers we pass to the enumerator.
				//
				// Note that we *must* pass native memory to the countByEnumeratingWithState:objects:count: method
				// (and not a field on the NSFastEnumerator instance), because:
				// * The pointers in the state (NSFastEnumerationState.mutationsPtr / NSFastEnumerationState.itemsPtr) might point back into the structure.
				// * We access those pointers using unsafe code (in a way the GC doesn't see).
				// * If the GC happens to move the NSFastEnumerator instance in memory, it won't update these pointers.
				// * The next time we read these pointers, we'll read random memory, and thus get random results.
				// * Ref: https://github.com/xamarin/maccore/issues/2606.
				// * It would probably also work to create a pinned GCHandle to the NSFastEnumerator structure (instead of allocating native memory), but that doesn't seem easier on the GC.
				state = (NSFastEnumerationState*) Marshal.AllocHGlobal (sizeof (NSFastEnumerationState));
				// Zero-initialize
				*state = default (NSFastEnumerationState);
			}
		}

		/// <summary>Releases the native enumeration state.</summary>
		~NSFastEnumerator ()
		{
			FreeState ();
		}

		void Fetch ()
		{
			unsafe {
				count = NSFastEnumerator.objc_msgSend (collection.Handle, NSFastEnumerator.CountByEnumeratingWithStateHandle, state, &state->array1, (nuint) NSFastEnumerationState.ArrayLength);
				GC.KeepAlive (collection);
				if (!started) {
					started = true;
					if (count > 0)
						mutationValue = *state->mutationsPtr;
				}
			}
			current = 0;
		}

		unsafe void VerifyNonMutated ()
		{
			if (mutationValue != *state->mutationsPtr)
				throw new InvalidOperationException ("Collection was modified");
			GC.KeepAlive (this);
		}

		#region IEnumerator implementation
		/// <inheritdoc />
		public bool MoveNext ()
		{
			ThrowIfDisposed ();
			if (finished)
				return false;
			if (started)
				VerifyNonMutated ();
			if (!started || current == count - 1) {
				Fetch ();
				if (count == 0) {
					finished = true;
					return false;
				}
			} else {
				current++;
			}
			VerifyNonMutated ();
			return true;
		}

		/// <inheritdoc />
		public void Reset ()
		{
			ThrowIfDisposed ();
			unsafe {
				*state = new NSFastEnumerationState ();
			}
			started = false;
			finished = false;
			count = 0;
			current = 0;
		}

		object System.Collections.IEnumerator.Current {
			get {
				return Current;
			}
		}
		#endregion

		#region IDisposable implementation
		/// <inheritdoc />
		public void Dispose ()
		{
			FreeState ();
			GC.SuppressFinalize (this);
		}

		unsafe void ThrowIfDisposed ()
		{
			ObjectDisposedException.ThrowIf (state is null, this);
			collection.GetNonNullHandle (nameof (collection));
			GC.KeepAlive (collection);
		}

		void FreeState ()
		{
			unsafe {
				Marshal.FreeHGlobal ((IntPtr) state);
				state = null;
			}
		}
		#endregion

		#region IEnumerator<T> implementation
		/// <inheritdoc />
		public unsafe T Current {
			get {
				ThrowIfDisposed ();
				if (!started || finished)
					throw new InvalidOperationException ("Enumeration has not started or has already finished.");
				VerifyNonMutated ();
				IntPtr ptr;
				unsafe {
					ptr = state->itemsPtr [(int) current];
				}
				var value = Runtime.GetINativeObject<T> (ptr, false) ?? throw new InvalidOperationException ("The collection returned a null object.");
				GC.KeepAlive (collection);
				return value;
			}
		}
		#endregion
	}
}
