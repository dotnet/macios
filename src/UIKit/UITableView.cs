#nullable enable

namespace UIKit {
	public partial class UITableView {
		/// <summary>Gets or sets a <see cref="UITableViewSource" /> subclass to act as both <see cref="UITableView.Delegate" /> and <see cref="UITableView.DataSource" />.</summary>
		/// <value>A class that can behave as both <see cref="UITableView.Delegate" /> and <see cref="UITableView.DataSource" /> for the table view.</value>
		/// <remarks>
		///   <para>The <see cref="UITableViewSource" /> class is an alternative to implementing both <see cref="UITableViewDataSource" /> and <see cref="UITableViewDelegate" />. If a subclass of <see cref="UITableViewSource" /> is created and assigned to this property, the <see cref="UITableView.DataSource" /> and <see cref="UITableView.Delegate" /> properties should not be set.</para>
		/// </remarks>
		public UITableViewSource? Source {
			get {
				return WeakDelegate as UITableViewSource ?? WeakDataSource as UITableViewSource;
			}

			set {
				WeakDelegate = value;
				WeakDataSource = value;
			}
		}

		/// <summary>Registers a type to provide <see cref="UITableViewCell" /> instances for a specific reuse identifier.</summary>
		/// <param name="cellType">The type of a <see cref="UITableViewCell" /> to create when the specified <paramref name="reuseIdentifier" /> is passed to <see cref="DequeueReusableCell(string,Foundation.NSIndexPath)" />, or <see langword="null" /> to unregister the reuse identifier.</param>
		/// <param name="reuseIdentifier">The reuse identifier.</param>
		/// <remarks>
		///   <para>
		///     Registering types with cell identifiers helps reduce the amount of code that you have to write in your <see cref="UITableViewSource.GetCell(UITableView,Foundation.NSIndexPath)" /> method. It means that your <see cref="UITableViewSource.GetCell(UITableView,Foundation.NSIndexPath)" /> method only needs to call <see cref="DequeueReusableCell(string,Foundation.NSIndexPath)" /> with the reuse identifier, and if there is no available cell, the <see cref="UITableView" /> will create an instance of the specified type and return it.
		///   </para>
		///   <para>
		///     The type that you register must provide a constructor that takes a <see cref="ObjCRuntime.NativeHandle" /> parameter and needs to chain to the <see cref="UITableViewCell(ObjCRuntime.NativeHandle)" /> constructor.
		///   </para>
		/// </remarks>
		public void RegisterClassForCellReuse (Type? cellType, NSString reuseIdentifier)
		{
			RegisterClassForCellReuse (Class.GetHandle (cellType), reuseIdentifier);
		}

		/// <summary>Registers the <paramref name="cellType" /> type for reuse, keyed by the identifier <paramref name="reuseIdentifier" />.</summary>
		/// <param name="cellType">The type of a <see cref="UITableViewCell" /> to create, or <see langword="null" /> to unregister the reuse identifier.</param>
		/// <param name="reuseIdentifier">The reuse identifier string.</param>
		public void RegisterClassForCellReuse (Type? cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForCellReuse (Class.GetHandle (cellType), str);
		}

		/// <summary>Registers the <paramref name="cellType" /> type for header or footer view reuse, keyed by the identifier <paramref name="reuseIdentifier" />.</summary>
		/// <param name="cellType">The type of a <see cref="UITableViewHeaderFooterView" /> to create, or <see langword="null" /> to unregister the reuse identifier.</param>
		/// <param name="reuseIdentifier">The reuse identifier string.</param>
		public void RegisterClassForHeaderFooterViewReuse (Type? cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForHeaderFooterViewReuse (Class.GetHandle (cellType), str);
		}

		/// <summary>Registers a type to provide <see cref="UITableViewHeaderFooterView" /> instances for headers or footers for a specific reuse identifier.</summary>
		/// <param name="cellType">The type of a <see cref="UITableViewHeaderFooterView" /> to create when the specified <paramref name="reuseIdentifier" /> is passed to <see cref="DequeueReusableHeaderFooterView(string)" />, or <see langword="null" /> to unregister the reuse identifier.</param>
		/// <param name="reuseIdentifier">The reuse identifier.</param>
		/// <remarks>
		///   <para>
		///     Registering types with reuse identifiers helps reduce the amount of code that you have to write in your <see cref="UITableViewSource.GetViewForFooter(UITableView,System.IntPtr)" /> or <see cref="UITableViewSource.GetViewForHeader(UITableView,System.IntPtr)" /> methods. It means that your code only needs to call <see cref="DequeueReusableHeaderFooterView(string)" /> with the reuse identifier, and if there is no available header or footer view, the <see cref="UITableView" /> will create an instance of the specified type and return it.
		///   </para>
		///   <para>
		///     The type that you register must provide a constructor that takes a <see cref="ObjCRuntime.NativeHandle" /> parameter and needs to chain to the <see cref="UITableViewHeaderFooterView(ObjCRuntime.NativeHandle)" /> constructor.
		///   </para>
		/// </remarks>
		public void RegisterClassForHeaderFooterViewReuse (Type? cellType, NSString reuseIdentifier)
		{
			RegisterClassForHeaderFooterViewReuse (Class.GetHandle (cellType), reuseIdentifier);
		}

		// This is not obsolete, we provide both a (UINib,string) overload and a (UINib,NSString) overload.
		// The difference is that in Unified the overridable method is the (UINib,NSString) overload to
		// be consistent with other API taking a reuseIdentifier.
		/// <summary>Registers a nib object (containing a <see cref="UITableViewCell" />) with the given identifier string.</summary>
		/// <param name="nib">A nib object created from a nib file. This value cannot be <see langword="null" />.</param>
		/// <param name="reuseIdentifier">A string to use as an identifier for the cell. This value cannot be <see langword="null" />.</param>
		/// <remarks>After a nib object has been registered with a table view, calling <see cref="DequeueReusableCell(Foundation.NSString)" /> with the correct identifier will cause the table view to instantiate the cell from the nib object if there is not already an instance in the reuse queue.</remarks>
		public void RegisterNibForCellReuse (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForCellReuse (nib, str);
		}

		/// <summary>Returns a reusable cell identified by <paramref name="reuseIdentifier" /> and located at <paramref name="indexPath" />.</summary>
		/// <param name="reuseIdentifier">The reuse identifier for the cell.</param>
		/// <param name="indexPath">The index path locating the cell in the table view.</param>
		/// <returns>A <see cref="UITableViewCell" /> for the specified reuse identifier and index path.</returns>
		public UITableViewCell DequeueReusableCell (string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return DequeueReusableCell (str, indexPath);
		}

		/// <summary>Returns a reusable header or footer view identified by <paramref name="reuseIdentifier" />.</summary>
		/// <param name="reuseIdentifier">The reuse identifier for the header or footer view.</param>
		/// <returns>A <see cref="UITableViewHeaderFooterView" /> with the associated identifier, or <see langword="null" /> if no such object exists in the reuse queue.</returns>
		public UITableViewHeaderFooterView? DequeueReusableHeaderFooterView (string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				return DequeueReusableHeaderFooterView (str);
		}

		// This is not obsolete, we provide both a (UINib,string) overload and a (UINib,NSString) overload.
		// The difference is that in Unified the overridable method is the (UINib,NSString) overload to
		// be consistent with other API taking a reuseIdentifier.
		/// <summary>Registers a nib object (containing a <see cref="UITableViewHeaderFooterView" />) with the given identifier string.</summary>
		/// <param name="nib">A nib object created from a nib file. This value cannot be <see langword="null" />.</param>
		/// <param name="reuseIdentifier">The reuse identifier for the header or footer view.</param>
		/// <remarks>
		///   <para>After a nib object has been registered with a table view, as section header and footer views come into view, the appropriate <see cref="UITableViewHeaderFooterView" /> instances will be instantiated as necessary from the nib object if there is not already an instance in the reuse queue.</para>
		/// </remarks>
		public void RegisterNibForHeaderFooterViewReuse (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForHeaderFooterViewReuse (nib, str);
		}
	}
}
