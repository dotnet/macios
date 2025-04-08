#nullable enable

using System;
namespace ModelIO {
	public partial class MDLAsset {
		/// <param name="index">To be added.</param>
		/// <summary>Gets the top-level <see cref="T:ModelIO.MDLObject" /> node in this asset's indexed list of <see cref="T:ModelIO.MDLObject" /> nodes, at the specified index.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public MDLObject this [nuint index] {
			get {
				return GetObject (index);
			}
		}

#if !NET
		[Obsolete ("Use the overload that takes an 'MDLLightProbeIrradianceDataSource' instead.")]
		public static MDLLightProbe [] PlaceLightProbes (float density, MDLProbePlacement type, MDLLightProbeIrradianceDataSource dataSource)
		{
			return PlaceLightProbes (density, type, (IMDLLightProbeIrradianceDataSource) dataSource);
		}
#endif
	}
}
