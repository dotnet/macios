//
// NSDirectoryEnumerator.cs:
// Author:
//   Miguel de Icaza
//
// Copyright 2011 - 2014 Xamarin Inc
//
using System.Collections.Generic;
using System.Collections;

#nullable enable

namespace Foundation {
	public partial class NSDirectoryEnumerator : IEnumerator<NSString?>, IEnumerator<string?>, IEnumerator {
		NSObject? current;

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
		bool IEnumerator.MoveNext ()
		{
			current = NextObject ();
			return current is not null;
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="InvalidOperationException">This operation is not supported for this enumerator.</exception>
		void IEnumerator.Reset ()
		{
			throw new InvalidOperationException ();
		}

		/// <summary>Gets the current element as a <see cref="string"/>.</summary>
		/// <value>The current element as a <see cref="string"/>, or <see langword="null"/> if the enumerator is positioned before the first element or after the last element.</value>
		string? IEnumerator<string?>.Current {
			get {
				return current?.ToString ();
			}
		}

		/// <summary>Gets the current element as an <see cref="NSString"/>.</summary>
		/// <value>The current element as an <see cref="NSString"/>, or <see langword="null"/> if the enumerator is positioned before the first element or after the last element, or if the current element is not an <see cref="NSString"/>.</value>
		NSString? IEnumerator<NSString?>.Current {
			get {
				return current as NSString;
			}
		}

		/// <summary>Gets the current element in the collection.</summary>
		/// <value>The current element in the collection.</value>
		/// <exception cref="InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
		object IEnumerator.Current {
			get {
				if (current is null)
					throw new InvalidOperationException ();
				return current;
			}
		}
	}
}
