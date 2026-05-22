using System.Collections.Generic;

#nullable enable

namespace System.Linq {
	public static class NSCollectionLinqExtensions {
		public static TKey? FirstOrDefault<TKey> (this Foundation.NSSet<TKey> source) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSArray<TKey> source) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSArray<TKey> source, Func<TKey, bool> predicate) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		public static TValue? FirstOrDefault<TValue> (this Foundation.NSMutableArray<TValue> source) where TValue : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TValue>) source);
		}

		public static TValue? FirstOrDefault<TValue> (this Foundation.NSMutableArray<TValue> source, Func<TValue, bool> predicate) where TValue : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TValue>) source, predicate);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source);
		}

		public static TKey? FirstOrDefault<TKey> (this Foundation.NSMutableOrderedSet<TKey> source, Func<TKey, bool> predicate) where TKey : class, Foundation.INativeObject
		{
			ArgumentNullException.ThrowIfNull (source);
			ArgumentNullException.ThrowIfNull (predicate);
			return Enumerable.FirstOrDefault ((IEnumerable<TKey>) source, predicate);
		}
	}
}
