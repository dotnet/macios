using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreLocation;
#if IOS
using MapKit;
#endif
using UIKit;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UITableView {
		/// <summary>A MonoTouch-specific feature that uses a <see cref="T:UIKit.UITableViewSource" /> subclass to act as both <see cref="P:UIKit.UITableView.Delegate" /> or <see cref="P:UIKit.UITableView.DataSource" />.</summary>
		///         <value>A class that can behave as both <see cref="P:UIKit.UITableView.Delegate" /> and <see cref="P:UIKit.UITableView.DataSource" /> for the table view.</value>
		///         <remarks>
		///           <para>MonoTouch provides the <see cref="T:UIKit.UITableViewSource" /> class as an alternative to implementing both <see cref="T:UIKit.UITableViewDataSource" /> and <see cref="T:UIKit.UITableViewDelegate" />. If a subclass of <see cref="T:UIKit.UITableViewSource" /> is created and assigned to this property, the <see cref="P:UIKit.UITableView.DataSource" /> and <see cref="P:UIKit.UITableView.Delegate" /> properties should not be set.</para>
		///         </remarks>
		public UITableViewSource Source {
			get {
				var d = WeakDelegate as UITableViewSource;
				if (d is not null)
					return d;
				d = WeakDataSource as UITableViewSource;
				if (d is not null)
					return d;
				return null;
			}

			set {
				WeakDelegate = value;
				WeakDataSource = value;
			}
		}

		/// <include file="../../docs/api/UIKit/UITableView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UITableView.RegisterClassForCellReuse(System.Type,Foundation.NSString)']/*" />
		public void RegisterClassForCellReuse (Type cellType, NSString reuseIdentifier)
		{
			RegisterClassForCellReuse (cellType is null ? IntPtr.Zero : Class.GetHandle (cellType), reuseIdentifier);
		}

		/// <param name="cellType">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Registers the <paramref name="cellType" /> type for reuse, keyed by the identifier <paramref name="reuseIdentifier" />.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForCellReuse (Type cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForCellReuse (cellType is null ? IntPtr.Zero : Class.GetHandle (cellType), str);
		}

		/// <param name="cellType">To be added.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Registers the <paramref name="cellType" /> type for reuse, keyed by the identifier <paramref name="reuseIdentifier" />.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForHeaderFooterViewReuse (Type cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForHeaderFooterViewReuse (cellType is null ? IntPtr.Zero : Class.GetHandle (cellType), str);
		}

		/// <include file="../../docs/api/UIKit/UITableView.xml" path="/Documentation/Docs[@DocId='M:UIKit.UITableView.RegisterClassForHeaderFooterViewReuse(System.Type,Foundation.NSString)']/*" />
		public void RegisterClassForHeaderFooterViewReuse (Type cellType, NSString reuseIdentifier)
		{
			RegisterClassForHeaderFooterViewReuse (cellType is null ? IntPtr.Zero : Class.GetHandle (cellType), reuseIdentifier);
		}

		// This is not obsolete, we provide both a (UINib,string) overload and a (UINib,NSString) overload.
		// The difference is that in Unified the overridable method is the (UINib,NSString) overload to
		// be consistent with other API taking a reuseIdentifier.
		/// <param name="nib">A nib object created from a nib file. This value cannot be <see langword="null" />.</param>
		///         <param name="reuseIdentifier">A string to use as an identifier for the cell. This value cannot be <see langword="null" />.</param>
		///         <summary>Registers a nib object (containing a <see cref="T:UIKit.UITableViewCell" />) with the given identifer string.</summary>
		///         <remarks>After a nib object has been registered with a table view, calling <see cref="M:UIKit.UITableView.DequeueReusableCell(Foundation.NSString)" /> with the correct identifer will cause the table view to instantiate the cell from the nib object if there is not already an instance in the reuse queue.</remarks>
		public void RegisterNibForCellReuse (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForCellReuse (nib, str);
		}

		/// <param name="reuseIdentifier">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <summary>Returns a reusable cell identified by <paramref name="reuseIdentifier" /> and located at <paramref name="indexPath" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UITableViewCell DequeueReusableCell (string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return DequeueReusableCell (str, indexPath);
		}

		/// <param name="reuseIdentifier">To be added.</param>
		///         <summary>Returns a reusable header/footer view identified by <paramref name="reuseIdentifier" /></summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public UITableViewHeaderFooterView DequeueReusableHeaderFooterView (string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				return DequeueReusableHeaderFooterView (str);
		}

		// This is not obsolete, we provide both a (UINib,string) overload and a (UINib,NSString) overload.
		// The difference is that in Unified the overridable method is the (UINib,NSString) overload to
		// be consistent with other API taking a reuseIdentifier.
		/// <param name="nib">A nib object created from a nib file. This value cannot be <see langword="null" />.</param>
		///         <param name="reuseIdentifier">To be added.</param>
		///         <summary>Registers a nib object (containing a <see cref="T:UIKit.UITableViewHeaderFooterView" />) with the given identifier string.</summary>
		///         <remarks>
		///           <para>After a nib object has been registered with a table view, as section header and footer views come into view, the appropriate <see cref="T:UIKit.UITableViewHeaderFooterView" />s will be instantiated as necessary from the nib object if there is not already an instance in the reuse queue.</para>
		///         </remarks>
		public void RegisterNibForHeaderFooterViewReuse (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForHeaderFooterViewReuse (nib, str);
		}
	}
}
