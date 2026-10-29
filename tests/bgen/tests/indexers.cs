// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;

#nullable enable

namespace IndexerTests {
	[BaseType (typeof (NSObject))]
	interface WrappedIndexers {
		[Export ("valueAtIndex:")]
		int GetValue (int index);

		[Export ("setValue:atIndex:")]
		void SetValue (int value, int index);

		/// <summary>Gets or sets the value at the specified index.</summary>
		/// <param name="index">The index of the value.</param>
		int this [int index] {
			[Wrap ("GetValue (index)")]
			get;
			[Wrap ("SetValue (value, index)")]
			set;
		}

		[Export ("objectForKey:")]
		[return: NullAllowed]
		NSObject GetObject ([NullAllowed] string key);

		[Export ("setObject:forKey:")]
		void SetObject ([NullAllowed] NSObject value, [NullAllowed] string key);

		[NullAllowed]
		NSObject this [[NullAllowed] string @event] {
			[Wrap ("GetObject (@event)")]
			get;
			[Wrap ("SetObject (value, @event)")]
			set;
		}

		[Export ("cellAtRow:column:")]
		string GetCell (int row, int column);

		string this [int row, int column] {
			[Wrap ("GetCell (row, column)")]
			get;
		}

		int this [params int [] indices] {
			[Wrap ("indices.Length")]
			get;
			[Wrap ("SetValue (value, indices.Length)")]
			set;
		}

		int this [double index] {
			[Wrap ("SetValue (value, (int) index)")]
			set;
		}

		int this [char index] {
			[Wrap ("GetValue (index)")]
			get;
			[NotImplemented ("Read-only indexer")]
			set;
		}

		int this [float index] {
			[NotImplemented]
			set;
		}
	}

	[BaseType (typeof (WrappedIndexers))]
	interface DerivedIndexers {
		[New]
		int this [int index] {
			[Wrap ("GetValue (index)")]
			get;
		}

		string this [nuint index] {
			[Wrap ("GetCell ((int) index, 0)")]
			get;
		}
	}

	[Protocol]
	interface IndexedProtocol {
		[Export ("protocolValueAtIndex:")]
		int GetProtocolValue (int index);

		int this [int index] {
			[Wrap ("GetProtocolValue (index)")]
			get;
		}

		int this [nuint index] {
			[Wrap ("GetProtocolValue ((int) index)")]
			get;
		}
	}

	[BaseType (typeof (NSObject))]
	interface ProtocolIndexers : IndexedProtocol {
		int this [char index] {
			[Wrap ("index")]
			get;
		}
	}
}
