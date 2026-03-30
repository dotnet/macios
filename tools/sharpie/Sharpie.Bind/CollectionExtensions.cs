// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public static class CollectionExtensions {
	public static void ForEach<T> (this IEnumerable<T> source, Action<T> action)
	{
		if (source is null)
			throw new ArgumentNullException (nameof (source));
		if (action is null)
			throw new ArgumentNullException (nameof (action));

		foreach (var item in source)
			action (item);
	}

	public static void AddRange (this System.Collections.ObjectModel.Collection<string> collection, IEnumerable<string> items)
	{
		if (collection is null)
			throw new ArgumentNullException (nameof (collection));
		if (items is null)
			throw new ArgumentNullException (nameof (items));

		foreach (var item in items)
			collection.Add (item);
	}

	public static T? DequeueOrDefault<T> (this Queue<T> queue)
	{
		if (queue is null)
			throw new ArgumentNullException (nameof (queue));

		return queue.Count == 0 ? default (T) : queue.Dequeue ();
	}

	public static T? PeekOrDefault<T> (this Queue<T> queue)
	{
		if (queue is null)
			throw new ArgumentNullException (nameof (queue));

		return queue.Count == 0 ? default (T) : queue.Peek ();
	}

	public static T? PeekOrDefault<T> (this Stack<T> stack)
	{
		if (stack is null)
			throw new ArgumentNullException (nameof (stack));

		return stack.Count == 0 ? default (T) : stack.Peek ();
	}

	public static IEnumerable<T> Append<T> (this IEnumerable<T> target, T item)
	{
		if (target is null)
			throw new ArgumentNullException (nameof (target));

		return target.Concat (new [] { item });
	}

	public static T? RemoveFirst<T> (this ICollection<T> target, Func<T, bool> predicate)
	{
		if (predicate is null)
			return default (T);

		foreach (var item in target) {
			if (predicate (item)) {
				target.Remove (item);
				return item;
			}
		}

		return default (T);
	}

	/// <summary>
	/// Trims <paramref name="source"/> such that leading and trailing items
	/// satisfying <paramref name="predicate"/> are filtered out. Inefficiently
	/// implemented using two <see cref="System.Linq.Enumerable.Reverse"/>
	/// operations (which in turn creates two arrays from <paramref name="source"/>").
	/// </summary>
	public static IEnumerable<T> Trim<T> (this IEnumerable<T> source, Func<T, bool> predicate)
	{
		return source.SkipWhile (predicate).Reverse ().SkipWhile (predicate).Reverse ();
	}
}
