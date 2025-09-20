namespace Microsoft.Macios.Generator.DataModel;

readonly struct MultiplatformBinding {

	Dictionary<PlatformName, Binding> Bindings { get; init; }

	/// <summary>
	/// Creates a new instance of the struct using the provided bindings.
	/// </summary>
	/// <param name="bindings"></param>
	public MultiplatformBinding (params (PlatformName Platform, Binding Binding) [] bindings)
	{
		Bindings = new Dictionary<PlatformName, Binding> ();
		foreach (var (platform, binding) in bindings)
			Bindings.Add (platform, binding);
	}

	/// <summary>
	/// Returns if the binding contains a specific platform.
	/// </summary>
	/// <param name="platform">The platform under query.</param>
	/// <returns>True if the binding contains the platform.</returns>
	public bool HasPlatform (PlatformName platform)
		=> Bindings.ContainsKey (platform);

}
