using System.Collections.ObjectModel;

#nullable enable

namespace Foundation {

	public partial class NSTimeZone {

		static ReadOnlyCollection<string>? known_time_zone_names;

		/// <summary>Gets a read-only collection of all known time zone names.</summary>
		/// <value>A <see cref="ReadOnlyCollection{T}" /> of strings containing all known time zone names.</value>
		/// <remarks>This property provides a read-only view of the time zone names to prevent modification of the underlying array.</remarks>
		public static ReadOnlyCollection<string> KnownTimeZoneNames {
			get {
				if (known_time_zone_names is null)
					known_time_zone_names = new ReadOnlyCollection<string> (_KnownTimeZoneNames);
				return known_time_zone_names;
			}
		}

		/// <summary>Returns a string representation of the current <see cref="NSTimeZone" /> instance.</summary>
		/// <returns>A string containing the name of the time zone.</returns>
		public override string ToString ()
		{
			return Name;
		}
	}
}
