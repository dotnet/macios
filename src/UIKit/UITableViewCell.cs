#nullable enable

namespace UIKit {

	public partial class UITableViewCell {
		/// <summary>Creates a table cell with the given style and reuse identifier.</summary>
		/// <param name="style">The style to use for this cell.</param>
		/// <param name="reuseIdentifier">A string used to identify the cell object if it should be reused for multiple rows in a table view. Pass <see langword="null" /> if the object should not be reused. The same reuse identifier string should be used for all cells that use the same class and layout.</param>
		/// <remarks>
		///   <para>The reuse identifier is associated with all cells (rows) in a table view that have the same layout (irrespective of their content) and can therefore be used interchangeably. The <see cref="UITableViewSource.GetCell(UITableView,Foundation.NSIndexPath)" /> implementation calls <see cref="UITableView.DequeueReusableCell(Foundation.NSString)" /> with a specific reuse identifier string to obtain a cached cell object with a particular layout to use as the basis for the row being constructed for viewing.</para>
		///   <para>To produce a layout different to those built-in to <see cref="UITableViewCell" />, create a custom cell. To set the row height of each cell differently, implement <see cref="UITableViewSource.GetHeightForRow(UITableView,Foundation.NSIndexPath)" />.</para>
		/// </remarks>
		public UITableViewCell (UITableViewCellStyle style, string? reuseIdentifier) : this (style, reuseIdentifier is null ? (NSString?) null : new NSString (reuseIdentifier))
		{
		}
	} /* class UITableViewCell */
}
