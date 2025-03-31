using System;

using Foundation;
using CoreFoundation;

#nullable enable

namespace CoreBluetooth {

	// The init ctor is invalid, but to present a nicer API (the delegate is optional/
	// hidden if events are desired) we fake it and provide a null delegate. This
	// is intentional and should not be obsoleted like the others below.
	public partial class CBCentralManager {

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public CBCentralManager () : this (new _CBCentralManagerDelegate (), null)
		{
		}

		/// <param name="dispatchQueue">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreBluetooth.CBCentralManager" /> with the specified <paramref name="dispatchQueue" />.</summary>
		///         <remarks>To be added.</remarks>
		public CBCentralManager (DispatchQueue dispatchQueue) : this (new _CBCentralManagerDelegate (), dispatchQueue)
		{
		}
	}
}
