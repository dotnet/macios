using System.Collections.Immutable;

namespace Microsoft.Macios.Generator.DataModel;

readonly struct MultiplatformBinding {

	Dictionary<PlatformName, Binding> Bindings { get; init; }

	/// <summary>
	/// Returns all the selectors for the enum members.
	/// </summary>
	public IEnumerable<string> EnumMemberSelectors => GetSelectorsEnumerable (b => b.EnumMemberSelectors);

	/// <summary>
	/// Returns all the selectors for the properties.
	/// </summary>
	public IEnumerable<string> PropertySelectors => GetSelectorsEnumerable (b => b.PropertySelectors);

	/// <summary>
	/// Returns all the selectors for the constructors.
	/// </summary>
	public IEnumerable<string> ConstructorSelectors => GetSelectorsEnumerable (b => b.ConstructorSelectors);

	/// <summary>
	/// Returns all the selectors for the events.
	/// </summary>
	public IEnumerable<string> EventSelectors => GetSelectorsEnumerable (b => b.EventSelectors);

	/// <summary>
	/// Returns all the selectors for the methods.
	/// </summary>
	public IEnumerable<string> MethodSelectors => GetSelectorsEnumerable (b => b.MethodSelectors);

	IEnumerable<string> GetSelectorsEnumerable (Func<Binding, ImmutableArray<string>> getSelectors)
	{
		HashSet<string> visited = new ();
		foreach (var (_, binding) in Bindings) {
			foreach (var selector in getSelectors (binding)) {
				if (visited.Add (selector)) {
					// only return the selector if it was not already visited
					yield return selector;
				}
			}
		}
	}

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
