using System.Collections.Generic;

#nullable enable

namespace System.Linq {
	public static class NSCollectionLinqExtensions {
		// NSSet<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSSet{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSSet{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSSet{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSSet{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSSet{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSSet{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSSet{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSSet<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSSet{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSSet<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSSet{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSSet{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSSet{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSSet{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSSet{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSSet{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSSet<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSSet{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSSet<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}

		// NSMutableSet<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSMutableSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableSet{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSMutableSet{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSMutableSet{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSMutableSet{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSMutableSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableSet{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableSet{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableSet{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableSet<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableSet{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableSet<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableSet{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableSet{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSMutableSet{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSMutableSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSMutableSet{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSMutableSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSMutableSet{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSMutableSet{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSMutableSet<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableSet{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSMutableSet<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}

		// NSArray<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSArray<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSArray{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSArray<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSArray{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSArray{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSArray{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSArray{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSArray{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSArray{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSArray{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSArray{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSArray<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSArray{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSArray<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSArray{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSArray<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSArray{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSArray<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSArray{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSArray<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSArray{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSArray<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSArray{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSArray{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSArray<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSArray{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSArray<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}

		// NSMutableArray<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableArray{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableArray{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSMutableArray<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableArray{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSMutableArray<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSMutableArray{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSMutableArray{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSMutableArray{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableArray{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSMutableArray{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableArray{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableArray{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableArray{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableArray<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableArray{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableArray<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableArray{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableArray{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSMutableArray{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSMutableArray<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSMutableArray{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSMutableArray<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSMutableArray{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSMutableArray{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSMutableArray<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSMutableArray<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableArray{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSMutableArray<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableArray{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSMutableArray<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}

		// NSOrderedSet<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSOrderedSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSOrderedSet{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSOrderedSet{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSOrderedSet{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSOrderedSet{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSOrderedSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSOrderedSet{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSOrderedSet{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSOrderedSet{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSOrderedSet{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSOrderedSet{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSOrderedSet{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSOrderedSet{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSOrderedSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSOrderedSet{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSOrderedSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSOrderedSet{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSOrderedSet{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSOrderedSet<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSOrderedSet{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSOrderedSet<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}

		// NSMutableOrderedSet<TKey>
		/// <summary>Returns the first element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.First ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey First<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.First ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The first element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the first element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Last ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Last<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Last ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The last element in <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the last element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static TKey? LastOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LastOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty or contains more than one element.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Single ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when no element or more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey Single<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Single ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>, or <see langword="null"/> if the collection is empty.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The single element of <paramref name="source"/>, or <see langword="null"/> if the collection is empty.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> contains more than one element.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the only element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfies a condition, or <see langword="null"/> if no such element is found.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The single element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>, or <see langword="null"/> if no such element is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one element satisfies the condition in <paramref name="predicate"/>.</exception>
		public static TKey? SingleOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SingleOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in <paramref name="source"/>.</exception>
		public static TKey ElementAt<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAt ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Returns the element at a specified index in <see cref="Foundation.NSMutableOrderedSet{TKey}"/>, or <see langword="null"/> if the index is out of range.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at position <paramref name="index"/> in <paramref name="source"/>, or <see langword="null"/> if the index is out of range.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey? ElementAtOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, int index) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ElementAtOrDefault ((IEnumerable<TKey>) source, index);
		}

		/// <summary>Determines whether <see cref="Foundation.NSMutableOrderedSet{TKey}"/> contains any elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns><see langword="true"/> if <paramref name="source"/> contains any elements; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Any ((IEnumerable<TKey>) source);
		}

		/// <summary>Determines whether any element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> satisfies a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if any element in <paramref name="source"/> passes the test in <paramref name="predicate"/>; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool Any<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Any ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Determines whether all elements of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><see langword="true"/> if every element in <paramref name="source"/> passes the test in <paramref name="predicate"/>, or if the collection is empty; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static bool All<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.All ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Count ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns the number of elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static int Count<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Count ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a <see cref="long"/> that represents the total number of elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The number of elements in <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.LongCount ((IEnumerable<TKey>) source);
		}

		/// <summary>Returns a <see cref="long"/> that represents the number of elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/> that satisfy a condition.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The number of elements in <paramref name="source"/> that pass the test in <paramref name="predicate"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static long LongCount<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.LongCount ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> based on a predicate.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Filters elements of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> based on a predicate that incorporates each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Where<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, int, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.Where ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> into a new form.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Projects each element of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> into a new form by incorporating each element's index.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="selector">A transform function to apply to each element; the second parameter represents the zero-based index of the element.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TResult> Select<TKey, TResult> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, int, TResult> selector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (selector);
			return Enumerable.Select ((IEnumerable<TKey>) source, selector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> in ascending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderBy<TKey, TOrderKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderBy ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Sorts the elements of <see cref="Foundation.NSMutableOrderedSet{TKey}"/> in descending order according to a key.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TOrderKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <returns>An <see cref="IOrderedEnumerable{T}"/> whose elements are sorted in descending order according to a key.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
		public static IOrderedEnumerable<TKey> OrderByDescending<TKey, TOrderKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, TOrderKey> keySelector) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (keySelector);
			return Enumerable.OrderByDescending ((IEnumerable<TKey>) source, keySelector);
		}

		/// <summary>Bypasses a specified number of elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/> and returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements after the skipped ones.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Skip<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Skip ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Bypasses elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/> as long as a condition is true, then returns the remaining elements.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements starting at the first element that does not satisfy the condition.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> SkipWhile<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.SkipWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns a specified number of contiguous elements from the start of <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Take<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, int count) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Take ((IEnumerable<TKey>) source, count);
		}

		/// <summary>Returns elements from <see cref="Foundation.NSMutableOrderedSet{TKey}"/> as long as a specified condition is true.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from <paramref name="source"/> as long as the condition is true.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> TakeWhile<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.TakeWhile ((IEnumerable<TKey>) source, predicate);
		}

		/// <summary>Returns distinct elements from <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Distinct<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Distinct ((IEnumerable<TKey>) source);
		}

		/// <summary>Inverts the order of the elements in <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> whose elements correspond to those of <paramref name="source"/> in reverse order.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Reverse<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.Reverse ((IEnumerable<TKey>) source);
		}

		/// <summary>Concatenates <see cref="Foundation.NSMutableOrderedSet{TKey}"/> with another sequence.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="second">The sequence to concatenate to <paramref name="source"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two sequences.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TKey> Concat<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, IEnumerable<TKey> second) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (second);
			return Enumerable.Concat ((IEnumerable<TKey>) source, second);
		}

		/// <summary>Creates a <see cref="List{T}"/> from <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A <see cref="List{T}"/> that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static List<TKey> ToList<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToList ((IEnumerable<TKey>) source);
		}

		/// <summary>Creates an array from <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>An array that contains elements from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <see langword="null"/>.</exception>
		public static TKey [] ToArray<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.ToArray ((IEnumerable<TKey>) source);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableOrderedSet{TKey}"/>.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="source"/> is empty.</exception>
		public static TKey Aggregate<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, TKey, TKey> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, func);
		}

		/// <summary>Applies an accumulator function over <see cref="Foundation.NSMutableOrderedSet{TKey}"/> with a seed value.</summary>
		/// <typeparam name="TKey">The element type of the collection.</typeparam>
		/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="seed">The initial accumulator value.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
		public static TAccumulate Aggregate<TKey, TAccumulate> (this Foundation.NSMutableOrderedSet<TKey> source, TAccumulate seed, Func<TAccumulate, TKey, TAccumulate> func) where TKey : class, ObjCRuntime.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (func);
			return Enumerable.Aggregate ((IEnumerable<TKey>) source, seed, func);
		}
	}
}
