//
// NSDirectoryEnumerator.cs:
// Author:
//   Miguel de Icaza
//
// Copyright 2011 - 2014 Xamarin Inc
//
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Collections;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSDirectoryEnumerator : IEnumerator<NSString>, IEnumerator<string>, IEnumerator {
		NSObject current;

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		bool IEnumerator.MoveNext ()
		{
			current = NextObject ();
			return current is not null;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IEnumerator.Reset ()
		{
			throw new InvalidOperationException ();
		}

		/// <summary>Gets the current element.</summary>
		/// <value>The current element.</value>
		string IEnumerator<string>.Current {
			get {
				return current.ToString ();
			}
		}

		/// <summary>Gets the current element.</summary>
		/// <value>The current element.</value>
		NSString IEnumerator<NSString>.Current {
			get {
				return current as NSString;
			}
		}

		/// <summary>Gets the current element.</summary>
		///         <value>The current element.</value>
		///         <remarks>To be added.</remarks>
		object IEnumerator.Current {
			get {
				return current;
			}
		}
	}
}
