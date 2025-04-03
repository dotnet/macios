#if __MACOS__

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace WebKit {

	public interface IIndexedContainer<T> {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int Count { get; }
		T this [int index] { get; }
	}

	internal class IndexedContainerEnumerator<T> : IEnumerator<T> {
		public IndexedContainerEnumerator (IIndexedContainer<T> container)
		{
			_container = container;
			Reset ();
		}

		public void Dispose ()
		{
			_container = null;
		}

		public T Current {
			get {
				if (_container is null)
					throw new ObjectDisposedException (nameof (_container));
				return _container [_index];
			}
		}

		object? IEnumerator.Current {
			get { return ((IEnumerator<T>) this).Current; }
		}

		public bool MoveNext ()
		{
			if (_container is null)
				throw new ObjectDisposedException (nameof (_container));
			return ++_index < _container.Count;
		}

		public void Reset ()
		{
			_index = -1;
		}

		IIndexedContainer<T>? _container;
		int _index;
	}

	public partial class DomCssRuleList : IIndexedContainer<DomCssRule>, IEnumerable<DomCssRule> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<DomCssRule> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<DomCssRule> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<DomCssRule>) this).GetEnumerator ();
		}
	}

	public partial class DomCssStyleDeclaration : IIndexedContainer<string>, IEnumerable<string> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<string> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<string> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<string>) this).GetEnumerator ();
		}
	}

	public partial class DomHtmlCollection : IIndexedContainer<DomNode>, IEnumerable<DomNode> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<DomNode> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<DomNode> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<DomNode>) this).GetEnumerator ();
		}
	}

	public partial class DomMediaList : IIndexedContainer<string>, IEnumerable<string> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<string> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<string> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<string>) this).GetEnumerator ();
		}
	}

	public partial class DomNamedNodeMap : IIndexedContainer<DomNode>, IEnumerable<DomNode> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<DomNode> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<DomNode> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<DomNode>) this).GetEnumerator ();
		}
	}

	public partial class DomNodeList : IIndexedContainer<DomNode>, IEnumerable<DomNode> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<DomNode> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<DomNode> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<DomNode>) this).GetEnumerator ();
		}
	}

	public partial class DomStyleSheetList : IIndexedContainer<DomStyleSheet>, IEnumerable<DomStyleSheet> {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IEnumerator<DomStyleSheet> GetEnumerator ()
		{
			return new IndexedContainerEnumerator<DomStyleSheet> (this);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<DomStyleSheet>) this).GetEnumerator ();
		}
	}
}

#endif // __MACOS__
