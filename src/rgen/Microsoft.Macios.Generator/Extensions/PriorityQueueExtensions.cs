// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.Macios.Generator.Extensions;

public static class PriorityQueueExtensions {

	/// <summary>
	/// Creates an <see cref="ImmutableArray{TElement}"/> from a <see cref="PriorityQueue{TElement, TPriority}"/>, dequeuing elements in sorted order.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the priority queue.</typeparam>
	/// <typeparam name="TPriority">The type of priorities in the priority queue.</typeparam>
	/// <param name="queue">The priority queue to create an immutable array from.</param>
	/// <returns>An immutable array containing the elements from the priority queue in sorted order.</returns>
	public static ImmutableArray<TElement> ToImmutable<TElement, TPriority> (this PriorityQueue<TElement, TPriority> queue)
	{
		// Create a copy of the original queue to preserve its state
		var queueCopy = new PriorityQueue<TElement, TPriority> (queue.UnorderedItems, queue.Comparer);

		var builder = ImmutableArray.CreateBuilder<TElement> (queueCopy.Count);
		while (queueCopy.TryDequeue (out var element, out _)) {
			builder.Add (element);
		}
		return builder.ToImmutable ();
	}
}
