#nullable enable

namespace ModelIO {
	public partial class MDLAsset {
		/// <param name="index">The zero-based index of the object to retrieve.</param>
		/// <summary>Gets the top-level <see cref="ModelIO.MDLObject" /> node in this asset's indexed list of <see cref="ModelIO.MDLObject" /> nodes, at the specified index.</summary>
		/// <value>The <see cref="MDLObject" /> at the specified index.</value>
		public MDLObject this [nuint index] {
			get {
				return GetObject (index);
			}
		}
	}
}
