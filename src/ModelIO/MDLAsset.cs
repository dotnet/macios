#nullable enable

using System;
namespace ModelIO {
	public partial class MDLAsset {
		/// <param name="index">To be added.</param>
		/// <summary>Gets the top-level <see cref="ModelIO.MDLObject" /> node in this asset's indexed list of <see cref="ModelIO.MDLObject" /> nodes, at the specified index.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public MDLObject this [nuint index] {
			get {
				return GetObject (index);
			}
		}
	}
}
